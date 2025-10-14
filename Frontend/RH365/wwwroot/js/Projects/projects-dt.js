"use strict";
// ============================================================================
// Archivo: projects-dt.ts
// Proyecto: RH365 (Front-End MVC .NET 8)
// Ruta: TS/projects/projects-dt.ts
// Descripción:
//   - Construye columnas/filas dinámicas para la tabla de Proyectos (DataTables).
//   - Habilita reordenamiento (drag & drop) usando ColReorder.
//   - Guarda automáticamente la vista personalizada del usuario en UserGridViews.
//   - Restaura el layout guardado (orden, visibilidad, anchos) al iniciar.
//   - Maneja exportación, selección y refresco de tabla.
// Requisitos:
//   - jQuery + DataTables + ColReorder cargados (vendors).
//   - Endpoint /api/UserGridViews con métodos GET y POST/PUT.
//   - Tabla con id="datatable-checkbox".
// ============================================================================
// ---- Utilidades ----
const titleize = (s) => (s || "")
    .replace(/([a-z])([A-Z])/g, "$1 $2")
    .replace(/_/g, " ")
    .replace(/^./, (c) => c.toUpperCase());
const fmtCell = (v) => {
    if (v == null)
        return "";
    if (typeof v === "string" && /^\d{4}-\d{2}-\d{2}T/.test(v)) {
        const d = new Date(v);
        if (!isNaN(d.getTime()))
            return d.toLocaleDateString();
    }
    if (typeof v === "boolean")
        return v ? "Sí" : "No";
    return String(v);
};
async function fetchJson(url, options) {
    const res = await fetch(url, options ?? { headers: { Accept: "application/json" } });
    if (!res.ok)
        throw new Error(`HTTP ${res.status} @ ${url}`);
    return (await res.json());
}
function inferColumns(sample) {
    const preferred = ["recID", "id", "projectCode", "projectName", "status", "owner", "startDate", "endDate"];
    const keys = Object.keys(sample);
    const cols = [];
    for (const k of preferred) {
        const hit = keys.find((x) => x.toLowerCase() === k.toLowerCase());
        if (hit && !cols.includes(hit))
            cols.push(hit);
    }
    for (const k of keys)
        if (!cols.includes(k))
            cols.push(k);
    return cols;
}
// ============================================================================
// IIFE principal
// ============================================================================
(function () {
    const w = window;
    const d = document;
    const $ = w.jQuery || w.$;
    const apiBase = (w.RH365?.urls?.apiBase) || "/api";
    const dataareaId = (w.RH365?.ctx?.dataareaId) ||
        (d.querySelector("#projects-page")?.getAttribute("data-dataarea") || "DAT");
    const userRefRecID = Number(d.querySelector("#projects-page")?.getAttribute("data-user") || 0);
    const $table = $("#datatable-checkbox");
    if (!$table.length)
        return;
    // Detectar si ColReorder está cargado
    const hasColReorder = !!$.fn.dataTable?.ColReorder;
    $(async function () {
        try {
            // 1) Evitar reinicialización
            if ($.fn.DataTable.isDataTable($table)) {
                $table.DataTable().clear().destroy();
            }
            // 2) Recuperar vista guardada (si existe)
            let userView = null;
            try {
                userView = await fetchJson(`${apiBase}/UserGridViews/by-entity?entityName=Projects&userRefRecID=${userRefRecID}`);
            }
            catch {
                userView = null;
            }
            // 3) Obtener muestra para inferir columnas
            const probeUrl = `${apiBase}/Projects?dataareaId=${encodeURIComponent(dataareaId)}&page=1&pageSize=1`;
            const probe = await fetchJson(probeUrl);
            const sample = (Array.isArray(probe) ? probe : (probe.items || []))[0] || {};
            const defaultCols = inferColumns(sample);
            // 4) Aplicar vista guardada (si existe)
            let cols = defaultCols;
            if (userView && userView.viewConfig) {
                try {
                    const parsed = JSON.parse(userView.viewConfig);
                    if (parsed?.columns?.length)
                        cols = parsed.columns.map((c) => c.field);
                }
                catch {
                    cols = defaultCols;
                }
            }
            // 5) Renderizar encabezado dinámico
            const theadHtml = `
        <tr>
          <th style="width:36px;"><input type="checkbox" id="chk-all"/></th>
          ${cols.map((c) => `<th data-col="${c}">${titleize(c)}</th>`).join("")}
        </tr>`;
            $table.find("thead").html(theadHtml);
            // 6) Inicializar DataTable
            const dt = $table.DataTable({
                processing: true,
                serverSide: false,
                responsive: true,
                autoWidth: false,
                pageLength: 10,
                lengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
                order: [],
                colReorder: hasColReorder ? { realtime: true, fixedColumnsLeft: 1 } : false,
                stateSave: true,
                stateDuration: 0,
                language: {
                    zeroRecords: "No hay datos",
                    info: "Mostrando _START_ a _END_ de _TOTAL_ entradas",
                    infoEmpty: "0 registros",
                    search: "Buscar:",
                    lengthMenu: "Mostrar _MENU_ entradas",
                    paginate: { previous: "Anterior", next: "Siguiente" }
                },
                columns: [
                    {
                        data: null,
                        orderable: false,
                        className: "text-center",
                        render: () => '<input type="checkbox" class="row-chk"/>'
                    },
                    ...cols.map((c) => ({
                        data: c,
                        name: c,
                        render: (d) => fmtCell(d)
                    }))
                ],
                ajax: (_data, callback) => {
                    const url = `${apiBase}/Projects?dataareaId=${encodeURIComponent(dataareaId)}&page=1&pageSize=1000`;
                    fetchJson(url)
                        .then((json) => {
                        const items = Array.isArray(json) ? json : (json.items || []);
                        callback({ data: items });
                        const total = Array.isArray(json) ? items.length : (json.total ?? items.length);
                        const summary = d.getElementById("prj-summary");
                        if (summary)
                            summary.textContent = `${total} registros`;
                    })
                        .catch((err) => { console.error(err); callback({ data: [] }); });
                }
            });
            // 7) Guardar vista automáticamente si el usuario reordena columnas
            if (hasColReorder) {
                dt.on("column-reorder", () => {
                    const order = dt.colReorder.order(); // array de posiciones
                    const headers = dt.columns().header().toArray().slice(1); // sin checkbox
                    const config = headers.map((h, i) => ({
                        field: $(h).data("col"),
                        order: order[i + 1]
                    }));
                    const viewConfig = JSON.stringify({ columns: config });
                    const payload = {
                        userRefRecID,
                        entityName: "Projects",
                        viewName: "Default",
                        isDefault: true,
                        viewConfig,
                        dataareaID: dataareaId
                    };
                    fetch(`${apiBase}/UserGridViews`, {
                        method: userView ? "PUT" : "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify(payload)
                    }).catch(console.error);
                });
            }
            // 8) Control de selección
            const updateActions = () => {
                const any = $table.find("tbody input.row-chk:checked").length > 0;
                $("#btn-prj-edit").prop("disabled", !any);
                $("#btn-prj-delete").prop("disabled", !any);
            };
            $table.on("change", "tbody input.row-chk", updateActions);
            $table.on("click", "tbody tr", (e) => {
                if ($(e.target).is("input.row-chk"))
                    return;
                const $chk = $(e.currentTarget).find("input.row-chk");
                $chk.prop("checked", !$chk.prop("checked"));
                updateActions();
            });
            $table.on("change", "#chk-all", function () {
                const checked = $(this).is(":checked");
                $table.find("tbody input.row-chk").prop("checked", checked);
                updateActions();
            });
            // 9) Export CSV
            $("#btn-prj-export").on("click", () => {
                const rows = dt.rows({ search: "applied" }).data().toArray();
                if (!rows.length) {
                    alert("No hay datos para exportar.");
                    return;
                }
                const lines = [cols.join(",")];
                rows.forEach((r) => {
                    lines.push(cols.map((c) => {
                        const v = r[c];
                        const s = v == null ? "" : String(v).replace(/"/g, '""');
                        return /[",\n]/.test(s) ? `"${s}"` : s;
                    }).join(","));
                });
                const blob = new Blob([lines.join("\n")], { type: "text/csv;charset=utf-8;" });
                const url = URL.createObjectURL(blob);
                const a = d.createElement("a");
                a.href = url;
                a.download = `Projects_${new Date().toISOString().slice(0, 10)}.csv`;
                d.body.appendChild(a);
                a.click();
                d.body.removeChild(a);
                URL.revokeObjectURL(url);
            });
            // 10) Refrescar tabla desde otros módulos
            d.addEventListener("projects:list:reload", () => dt.ajax.reload());
        }
        catch (err) {
            console.error(err);
        }
    });
})();
//# sourceMappingURL=projects-dt.js.map