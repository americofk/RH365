// ============================================================================
// Archivo: loan-dt.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Loans/loan-dt.ts
// Descripción:
//   - Lista de Préstamos con DataTables (tabla inteligente).
//   - Gestor de vistas (UserGridViews) y de columnas (GridColumnsManager).
//   - Corrige desaparición de tabla al cambiar de vista (re-init seguro).
// Requisitos globales: RH365.urls.apiBase, ALERTS, GridViewsManager, GridColumnsManager,
//   jQuery, DataTables (+ responsive), iCheck.
// ============================================================================

type LoanRow = Record<string, unknown>;

interface LoanResponse {
    Data: LoanRow[];
    TotalCount: number;
    PageNumber: number;
    PageSize: number;
    TotalPages: number;
    HasNextPage: boolean;
    HasPreviousPage: boolean;
}

interface ColumnConfig {
    field: string;
    visible: boolean;
    order: number;
    width?: number;
}

(function () {
    const w: any = window;
    const d: Document = document;
    const $: any = w.jQuery || w.$;

    // Contexto de página
    const apiBase: string = (w.RH365 && w.RH365.urls && w.RH365.urls.apiBase) ? w.RH365.urls.apiBase : "/api";
    const page = d.querySelector("#loans-page");
    if (!page) return;

    const token: string = page.getAttribute("data-token") || "";
    const dataareaId: string = page.getAttribute("data-dataarea") || "DAT";
    const userRefRecID: number = parseInt(page.getAttribute("data-user") || "0", 10);

    const $table: any = $("#loans-table");
    if (!$table.length) return;

    // Estado
    let loansData: LoanRow[] = [];
    let allColumns: string[] = [];
    let visibleColumns: string[] = [];
    const defaultColumns: string[] = ['ID', 'LoanCode', 'Name', 'LedgerAccount', 'LoanStatus', 'CreatedOn', 'ValidFrom', 'ValidTo'];

    let gridViewsManager: any;
    let gridColumnsManager: any;

    // Etiquetas
    const titleize = (field: string): string => {
        const map: Record<string, string> = {
            'RecID': 'ID Registro',
            'ID': 'ID',
            'LoanCode': 'Código Préstamo',
            'Name': 'Nombre',
            'LedgerAccount': 'Cuenta Contable',
            'LoanStatus': 'Estado',
            'CreatedOn': 'Fecha Creación',
            'CreatedBy': 'Creado Por',
            'ModifiedOn': 'Modificado',
            'ModifiedBy': 'Modificado Por',
            'ValidFrom': 'Válido Desde',
            'ValidTo': 'Válido Hasta',
            'Observations': 'Observaciones'
        };
        return map[field] || field.replace(/([a-z])([A-Z])/g, "$1 $2").replace(/_/g, " ").replace(/^./, c => c.toUpperCase());
    };

    // Formato de celdas (fechas/bools)
    const formatCell = (value: unknown, field: string): string => {
        if (value == null) return "";
        if (typeof value === "boolean" || value === 0 || value === 1 || value === "0" || value === "1") {
            const truthy = (value === true || value === 1 || value === "1");
            if (field === "LoanStatus") {
                return truthy ? '<span class="label label-success">Activo</span>' : '<span class="label label-danger">Inactivo</span>';
            }
            return truthy ? "Sí" : "No";
        }
        if (typeof value === "string" && /^\d{4}-\d{2}-\d{2}T/.test(value)) {
            const dt = new Date(value);
            if (!isNaN(dt.getTime())) {
                return dt.toLocaleDateString('es-DO', { day: '2-digit', month: '2-digit', year: 'numeric' });
            }
        }
        return String(value);
    };

    // Fetch JSON con bearer opcional
    const fetchJson = async (url: string): Promise<any> => {
        const headers: Record<string, string> = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        if (token) headers['Authorization'] = `Bearer ${token}`;
        const r = await fetch(url, { headers });
        if (!r.ok) throw new Error(`HTTP ${r.status} @ ${url}`);
        return r.json();
    };

    // Derivar columnas desde un registro muestra
    const getColumnsFromData = (sample: LoanRow): string[] => {
        if (!sample || typeof sample !== 'object') return [...defaultColumns];
        const excluded = new Set(['RecID', 'RowVersion', 'DataareaID']);
        return Object.keys(sample).filter(k => !excluded.has(k));
    };

    // ==== DataTables ====

    // (1) Genera cabecera y crea DT. Sin .destroy(true), sin vaciar manualmente tbody.
    const initializeDataTable = (columns: string[]): void => {
        // Asegurar columnas válidas vs. datos (previene "unknown parameter" y tabla invisible)
        const safeCols = columns.filter(c => allColumns.includes(c));
        const finalCols = safeCols.length ? safeCols : [...defaultColumns];

        if ($.fn.DataTable.isDataTable($table)) {
            const dt = $table.DataTable();
            dt.clear();            // limpia datos internos
            dt.destroy();          // destruye sin tocar la tabla base
        }

        const theadHtml = `
            <tr>
                <th style="width:40px;"><input type="checkbox" id="check-all" class="flat"/></th>
                ${finalCols.map(col => `<th data-field="${col}">${titleize(col)}</th>`).join('')}
            </tr>
        `;
        $table.find('thead').html(theadHtml);

        if ($.fn.iCheck) $('.flat').iCheck({ checkboxClass: 'icheckbox_flat-green' });

        const dtConfig: any = {
            processing: true,
            serverSide: false,
            responsive: true,
            autoWidth: false,
            deferRender: true,
            order: [[1, 'asc']],
            pageLength: 25,
            lengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
            language: {
                lengthMenu: 'Mostrar _MENU_ registros',
                zeroRecords: 'No se encontraron resultados',
                info: 'Mostrando _START_ a _END_ de _TOTAL_ registros',
                infoEmpty: 'No hay registros',
                infoFiltered: '(filtrado de _MAX_ registros)',
                search: 'Buscar:',
                paginate: { first: 'Primera', last: 'Última', next: 'Siguiente', previous: 'Anterior' },
                processing: 'Procesando...'
            },
            columns: [
                {
                    data: null,
                    orderable: false,
                    searchable: false,
                    className: 'text-center',
                    defaultContent: '',
                    render: (_d: any, _t: string, row: LoanRow) =>
                        `<input type="checkbox" class="flat row-check" data-recid="${(row as any).RecID ?? ''}"/>`
                },
                ...finalCols.map(col => ({
                    data: col,
                    name: col,
                    defaultContent: '',            // evita "unknown parameter" si falta el campo
                    render: (data: unknown) => formatCell(data, col)
                }))
            ],
            ajax: (_rq: any, callback: (r: { data: LoanRow[] }) => void) => {
                loadLoans()
                    .then(items => {
                        loansData = items;
                        callback({ data: items });
                        updateSummary(items.length);
                    })
                    .catch(err => {
                        console.error('Error cargando préstamos:', err);
                        showError(err.message);
                        callback({ data: [] });
                    });
            },
            drawCallback: () => {
                if ($.fn.iCheck) $table.find('.flat').iCheck({ checkboxClass: 'icheckbox_flat-green' });
                const dt = $table.DataTable();
                dt.columns.adjust().responsive.recalc(); // asegura layout correcto
            }
        };

        $table.DataTable(dtConfig);
    };

    // Carga de datos
    const loadLoans = async (): Promise<LoanRow[]> => {
        const url = `${apiBase}/Loans?pageNumber=1&pageSize=100`;
        const response: LoanResponse | LoanRow[] = await fetchJson(url);
        if (Array.isArray(response)) return response;
        if (response?.Data && Array.isArray(response.Data)) return response.Data;
        return [];
    };

    // ==== UI ====

    const updateSummary = (count: number): void => {
        const el = d.getElementById('loans-summary');
        if (el) el.textContent = `${count} préstamo${count !== 1 ? 's' : ''}`;
    };

    const showError = (message: string): void => {
        $table.find('tbody').html(`
            <tr>
                <td colspan="10" class="text-center text-danger">
                    <i class="fa fa-exclamation-triangle"></i> ${message}
                </td>
            </tr>
        `);
    };

    const updateButtonStates = (): void => {
        const n = $table.find('tbody input.row-check:checked').length;
        $('#btn-edit').prop('disabled', n !== 1);
        $('#btn-delete').prop('disabled', n === 0);
    };

    const exportToCSV = (): void => {
        if (!loansData.length) { (w as any).ALERTS?.warn?.('No hay datos para exportar', 'Advertencia'); return; }
        const dt: any = $table.DataTable();
        const rows: LoanRow[] = dt.rows({ search: 'applied' }).data().toArray();
        if (!rows.length) { (w as any).ALERTS?.warn?.('No hay datos visibles para exportar', 'Advertencia'); return; }

        const cols = visibleColumns.filter(c => allColumns.includes(c));
        const lines: string[] = [cols.map(c => titleize(c)).join(',')];
        rows.forEach(row => {
            const line = cols.map(c => {
                const str = String(formatCell((row as any)[c], c)).replace(/<[^>]*>/g, '').replace(/"/g, '""');
                return /[",\n]/.test(str) ? `"${str}"` : str;
            }).join(',');
            lines.push(line);
        });

        const blob = new Blob(['\ufeff' + lines.join('\n')], { type: 'text/csv;charset=utf-8;' });
        const url = URL.createObjectURL(blob);
        const a = d.createElement('a');
        a.href = url; a.download = `Prestamos_${new Date().toISOString().slice(0, 10)}.csv`;
        d.body.appendChild(a); a.click(); d.body.removeChild(a); URL.revokeObjectURL(url);
        (w as any).ALERTS?.ok?.('Datos exportados correctamente', 'Éxito');
    };

    const deleteLoan = async (recId: string): Promise<void> => {
        const url = `${apiBase}/Loans/${recId}`;
        const headers: Record<string, string> = token ? { 'Authorization': `Bearer ${token}` } : {};
        const r = await fetch(url, { method: 'DELETE', headers });
        if (!r.ok) throw new Error(`Error al eliminar préstamo: ${r.status}`);
    };

    // ==== Vistas y Columnas ====

    const applyColumnChanges = (newColumns: string[]): void => {
        visibleColumns = newColumns;
        initializeDataTable(newColumns); // re-init limpio (sin duplicados ni desaparición)
        if (gridViewsManager?.hasCurrentView?.()) $('#btn-save-view-changes').show();
    };

    const updateViewsDropdown = (): void => {
        try {
            const views = gridViewsManager.getAvailableViews();
            const $container = $('#saved-views-container');
            $container.empty();

            if (!views?.length) {
                $container.append('<li class="text-muted" style="padding: 5px 20px;">No hay vistas guardadas</li>');
                return;
            }

            views.forEach((v: any) => {
                const icon = v.IsDefault ? '<i class="fa fa-star text-warning"></i>' : '<i class="fa fa-file-o"></i>';
                const $item = $(`
                    <li style="position: relative;">
                        <a href="#" data-view-id="${v.RecID}" style="display:inline-block;width:80%;">${icon} ${v.ViewName} ${v.IsPublic ? '<span class="label label-info label-sm">Pública</span>' : ''}</a>
                        ${!v.IsDefault ? `<a href="#" class="btn-set-default" data-view-id="${v.RecID}" style="position:absolute;right:10px;top:5px;color:#999;" title="Predeterminada"><i class="fa fa-star-o"></i></a>` : ''}
                    </li>
                `);
                $item.find('a[data-view-id]').on('click', async (e) => { e.preventDefault(); await loadViewById(v.RecID); });
                $item.find('.btn-set-default').on('click', (e) => {
                    e.preventDefault(); e.stopPropagation();
                    (w as any).ALERTS?.confirm?.(`¿Establecer "${v.ViewName}" como predeterminada?`, 'Confirmar', async (ok: boolean) => {
                        if (!ok) return;
                        const success = await gridViewsManager.setDefaultView(v.RecID);
                        if (success) updateViewsDropdown();
                    });
                });
                $container.append($item);
            });
        } catch (err) {
            console.error('Error actualizando dropdown de vistas:', err);
        }
    };

    const loadViewById = async (viewId: number): Promise<void> => {
        try {
            const cfg: ColumnConfig[] = await gridViewsManager.loadView(viewId);
            if (!cfg?.length) return;

            gridColumnsManager.applyColumnConfig(cfg);
            const newCols = cfg.filter(c => c.visible).sort((a, b) => a.order - b.order).map(c => c.field);
            applyColumnChanges(newCols);

            const viewName = gridViewsManager.getCurrentViewName?.() || 'Vista por defecto';
            $('#current-view-name').text(viewName);
            $('#btn-save-view-changes').hide();
        } catch (err) {
            console.error('Error cargando vista:', err);
            (w as any).ALERTS?.error?.('Error al cargar la vista', 'Error');
        }
    };

    // ==== Eventos UI ====

    // Check-all (delegado, funciona tras re-init)
    $(document).on('ifChanged', '#check-all', function (this: HTMLInputElement) {
        const checked = $(this).is(':checked');
        const $checks = $table.find('tbody input.row-check');
        if ($.fn.iCheck) $checks.iCheck(checked ? 'check' : 'uncheck'); else $checks.prop('checked', checked);
    });

    // Sincroniza botones por selección
    $(document).on('ifChanged', '.row-check', updateButtonStates);

    // Nuevo / Editar / Eliminar
    $('#btn-new').on('click', () => { window.location.href = '/Loan/NewEdit'; });
    $('#btn-edit').on('click', () => {
        const $c = $table.find('tbody input.row-check:checked').first();
        if ($c.length) window.location.href = `/Loan/NewEdit?recId=${$c.data('recid')}`;
    });
    $('#btn-delete').on('click', async () => {
        const $checked = $table.find('tbody input.row-check:checked');
        const count = $checked.length;
        if (count === 0) return;

        const msg = count === 1 ? '¿Está seguro de eliminar este préstamo?' : `¿Está seguro de eliminar ${count} préstamos?`;
        (w as any).ALERTS?.confirm?.(msg, 'Confirmar Eliminación', async (ok: boolean) => {
            if (!ok) return;
            try {
                const ops: Promise<void>[] = [];
                $checked.each(function () {
                    const id = $(this).data('recid');
                    if (id) ops.push(deleteLoan(String(id)));
                });
                await Promise.all(ops);
                (w as any).ALERTS?.ok?.('Préstamo(s) eliminado(s) correctamente', 'Éxito');
                $table.DataTable().ajax.reload();
            } catch (err) {
                console.error('Error al eliminar:', err);
                (w as any).ALERTS?.error?.('Error al eliminar préstamo(s)', 'Error');
            }
        }, { type: 'danger' });
    });

    // Exportar/Importar
    $('#btn-export').on('click', exportToCSV);
    $('#btn-import').on('click', () => (w as any).ALERTS?.info?.('Funcionalidad de importación próximamente', 'Información'));

    // Click fila → toggle checkbox
    $table.on('click', 'tbody tr', function (e: any) {
        if ($(e.target).is('input.row-check') || $(e.target).closest('.icheckbox_flat-green').length) return;
        const $cb = $(this).find('input.row-check');
        if (!$cb.length) return;
        if ($.fn.iCheck) $cb.iCheck($cb.is(':checked') ? 'uncheck' : 'check'); else $cb.prop('checked', !$cb.prop('checked'));
    });

    // Doble clic → editar
    $table.on('dblclick', 'tbody tr', function () {
        const id = $(this).find('input.row-check').data('recid');
        if (id) window.location.href = `/Loan/NewEdit?recId=${id}`;
    });

    // Columnas
    $('#btn-manage-columns').on('click', () => { gridColumnsManager?.showColumnsModal?.(); });
    $('#btn-apply-columns').on('click', () => {
        const cfg = gridColumnsManager.applyColumns() as ColumnConfig[];
        const newCols = cfg.filter(c => c.visible).sort((a, b) => a.order - b.order).map(c => c.field);
        applyColumnChanges(newCols);
        ($('#modal-manage-columns') as any).modal('hide');
    });

    // Guardar vista
    $('#btn-new-view').on('click', (e) => {
        e.preventDefault();
        $('#view-name').val('');
        $('#view-is-default').prop('checked', false);
        $('#view-is-public').prop('checked', false);
        ($('#modal-save-view') as any).modal('show');
    });

    $('#btn-confirm-save-view').on('click', async () => {
        const viewName = String($('#view-name').val() || '').trim();
        const isDefault = $('#view-is-default').is(':checked');
        const isPublic = $('#view-is-public').is(':checked');
        if (!viewName) { (w as any).ALERTS?.warn?.('Ingrese un nombre para la vista', 'Advertencia'); return; }

        const cfg = gridColumnsManager.getCurrentColumnConfig();
        const ok = await gridViewsManager.saveView(viewName, cfg, isDefault, isPublic);
        if (ok) {
            ($('#modal-save-view') as any).modal('hide');
            $('#current-view-name').text(viewName);
            $('#btn-save-view-changes').hide();
            updateViewsDropdown();
        }
    });

    // Guardar Como
    $('#btn-save-as-view').on('click', () => {
        $('#view-name-saveas').val('');
        $('#view-is-default-saveas').prop('checked', false);
        $('#view-is-public-saveas').prop('checked', false);
        ($('#modal-save-as-view') as any).modal('show');
    });

    $('#btn-confirm-save-as').on('click', async () => {
        const viewName = String($('#view-name-saveas').val() || '').trim();
        const isDefault = $('#view-is-default-saveas').is(':checked');
        const isPublic = $('#view-is-public-saveas').is(':checked');
        if (!viewName) { (w as any).ALERTS?.warn?.('Ingrese un nombre para la vista', 'Advertencia'); return; }

        const cfg = gridColumnsManager.getCurrentColumnConfig();
        const ok = await gridViewsManager.saveView(viewName, cfg, isDefault, isPublic);
        if (ok) {
            ($('#modal-save-as-view') as any).modal('hide');
            $('#current-view-name').text(viewName);
            $('#btn-save-view-changes').hide();
            updateViewsDropdown();
        }
    });

    // Guardar cambios de vista activa
    $('#btn-save-view-changes').on('click', async () => {
        if (!gridViewsManager?.hasCurrentView?.()) { (w as any).ALERTS?.warn?.('No hay vista activa para actualizar', 'Advertencia'); return; }
        const cfg = gridColumnsManager.getCurrentColumnConfig();
        const ok = await gridViewsManager.updateView(cfg);
        if (ok) $('#btn-save-view-changes').hide();
    });

    // Restablecer vista por defecto
    $('#btn-reset-view').on('click', () => {
        (w as any).ALERTS?.confirm?.('¿Desea restablecer a la vista por defecto?', 'Confirmar', (ok: boolean) => {
            if (!ok) return;
            gridColumnsManager.resetToDefault(defaultColumns);
            applyColumnChanges(defaultColumns);
            $('#current-view-name').text('Vista por defecto');
            $('#btn-save-view-changes').hide();
        });
    });

    // Cambiar a "Vista por defecto" desde el dropdown
    $(document).on('click', '[data-view="default"]', (e) => {
        e.preventDefault();
        gridColumnsManager.resetToDefault(defaultColumns);
        applyColumnChanges(defaultColumns);
        $('#current-view-name').text('Vista por defecto');
        $('#btn-save-view-changes').hide();
    });

    // ==== Bootstrap ====
    $(async function () {
        try {
            // Descubrir columnas
            const probeUrl = `${apiBase}/Loans?pageNumber=1&pageSize=1`;
            const probe: LoanResponse | LoanRow[] = await fetchJson(probeUrl);
            if (Array.isArray(probe) && probe.length) {
                allColumns = getColumnsFromData(probe[0]);
            } else if (!Array.isArray(probe) && probe?.Data?.length) {
                allColumns = getColumnsFromData(probe.Data[0]);
            } else {
                allColumns = [...defaultColumns];
            }

            const GVM = (w as any).GridViewsManager;
            const GCM = (w as any).GridColumnsManager;
            if (!GVM || !GCM) {
                console.warn('GridViewsManager / GridColumnsManager no disponibles. Usando vista por defecto.');
                visibleColumns = [...defaultColumns];
                initializeDataTable(visibleColumns);
                return;
            }

            gridViewsManager = new GVM(apiBase, token, "Loans", userRefRecID, dataareaId);
            const saved: ColumnConfig[] = await gridViewsManager.initialize();
            if (saved?.length) {
                visibleColumns = saved.filter(c => c.visible).sort((a, b) => a.order - b.order).map(c => c.field);
                $('#current-view-name').text(gridViewsManager.getCurrentViewName?.() || 'Vista por defecto');
            } else {
                visibleColumns = [...defaultColumns];
            }

            gridColumnsManager = new GCM(allColumns, visibleColumns, (newCols: string[]) => applyColumnChanges(newCols));

            updateViewsDropdown();
            initializeDataTable(visibleColumns); // Primera carga (sin duplicados y sin desaparecer)
        } catch (err) {
            console.error('Error en inicialización:', err);
            visibleColumns = [...defaultColumns];
            initializeDataTable(visibleColumns);
            showError('Error al cargar la configuración');
        }
    });
})();
