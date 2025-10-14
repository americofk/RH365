// ============================================================================
// Archivo: projects-dt.ts
// Proyecto: RH365 (Front-End MVC .NET 8)
// Descripción:
//   - Tabla dinámica de proyectos compatible con { Data, TotalCount }.
//   - Previene error “aDataSort undefined” si no hay columnas.
// ============================================================================

type RowObj = Record<string, unknown>;
interface PagedResponse<T> { items?: T[]; total?: number; }

(function () {
    const w: any = window;
    const d: Document = document;
    const $: any = w.jQuery || w.$;

    const apiBase: string = (w.RH365?.urls?.apiBase) || "/api";
    const dataareaId: string =
        d.querySelector("#projects-page")?.getAttribute("data-dataarea") || "DAT";

    const $table: any = $("#datatable-checkbox");
    if (!$table.length) return;

    $(async function () {
        try {
            // 1) Obtener datos de muestra
            const probeUrl = `${apiBase}/Projects?dataareaId=${encodeURIComponent(dataareaId)}&page=1&pageSize=1`;
            const probe = await fetch(probeUrl).then(r => r.json());

            let sample: RowObj = {};
            if (probe?.Data?.length) sample = probe.Data[0];
            else if (Array.isArray(probe) && probe.length) sample = probe[0];
            else if (probe?.items?.length) sample = probe.items[0];

            // 2) Inferir columnas
            let cols = Object.keys(sample || {});
            if (cols.length === 0) cols = ["ID", "ProjectCode", "Name", "LedgerAccount"];

            // 3) Renderizar encabezado seguro
            const theadHtml = `
                <tr>
                    <th style="width:36px;"><input type="checkbox" id="chk-all"/></th>
                    ${cols.map((c) => `<th>${c}</th>`).join("")}
                </tr>`;
            $table.find("thead").html(theadHtml);

            // 4) Si aún no hay columnas válidas ? no inicializar
            if (!cols || cols.length === 0) {
                console.warn("?? No se encontraron columnas válidas para DataTable");
                return;
            }

            // 5) Inicializar DataTable
            const dt: any = $table.DataTable({
                processing: true,
                serverSide: false,
                responsive: true,
                autoWidth: false,
                columns: [
                    {
                        data: null,
                        orderable: false,
                        className: "text-center",
                        render: () => '<input type="checkbox" class="row-chk"/>'
                    },
                    ...cols.map((c) => ({ data: c, name: c, render: (d: any) => d ?? "" }))
                ],
                ajax: (_data: unknown, callback: (arg: { data: RowObj[] }) => void) => {
                    const url = `${apiBase}/Projects?dataareaId=${encodeURIComponent(dataareaId)}&page=1&pageSize=1000`;
                    fetch(url)
                        .then(r => r.json())
                        .then(json => {
                            let items: RowObj[] = [];
                            let total = 0;
                            if (json?.Data) {
                                items = json.Data;
                                total = json.TotalCount ?? items.length;
                            } else if (Array.isArray(json)) {
                                items = json;
                                total = items.length;
                            } else if (json?.items) {
                                items = json.items;
                                total = json.total ?? items.length;
                            }
                            callback({ data: items });
                            const summary = d.getElementById("prj-summary");
                            if (summary) summary.textContent = `${total} registros`;
                        })
                        .catch(err => {
                            console.error("Error cargando proyectos:", err);
                            callback({ data: [] });
                        });
                },
                language: {
                    zeroRecords: "No hay datos",
                    info: "Mostrando _START_ a _END_ de _TOTAL_ entradas",
                    infoEmpty: "0 registros",
                    search: "Buscar:",
                    lengthMenu: "Mostrar _MENU_ entradas",
                    paginate: { previous: "Anterior", next: "Siguiente" }
                }
            });

            // 6) Acciones de selección
            const updateActions = (): void => {
                const any = $table.find("tbody input.row-chk:checked").length > 0;
                $("#btn-prj-edit").prop("disabled", !any);
                $("#btn-prj-delete").prop("disabled", !any);
            };
            $table.on("change", "tbody input.row-chk", updateActions);
            $table.on("click", "tbody tr", (e: any) => {
                if ($(e.target).is("input.row-chk")) return;
                const $chk = $(e.currentTarget).find("input.row-chk");
                $chk.prop("checked", !$chk.prop("checked"));
                updateActions();
            });

        } catch (err) {
            console.error("Error general:", err);
        }
    });
})();
