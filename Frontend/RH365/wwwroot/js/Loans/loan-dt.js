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
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
(function () {
    const w = window;
    const d = document;
    const $ = w.jQuery || w.$;
    // Contexto de página
    const apiBase = (w.RH365 && w.RH365.urls && w.RH365.urls.apiBase) ? w.RH365.urls.apiBase : "/api";
    const page = d.querySelector("#loans-page");
    if (!page)
        return;
    const token = page.getAttribute("data-token") || "";
    const dataareaId = page.getAttribute("data-dataarea") || "DAT";
    const userRefRecID = parseInt(page.getAttribute("data-user") || "0", 10);
    const $table = $("#loans-table");
    if (!$table.length)
        return;
    // Estado
    let loansData = [];
    let allColumns = [];
    let visibleColumns = [];
    const defaultColumns = ['ID', 'LoanCode', 'Name', 'LedgerAccount', 'LoanStatus', 'CreatedOn', 'ValidFrom', 'ValidTo'];
    let gridViewsManager;
    let gridColumnsManager;
    // Etiquetas
    const titleize = (field) => {
        const map = {
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
    const formatCell = (value, field) => {
        if (value == null)
            return "";
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
    const fetchJson = (url) => __awaiter(this, void 0, void 0, function* () {
        const headers = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        if (token)
            headers['Authorization'] = `Bearer ${token}`;
        const r = yield fetch(url, { headers });
        if (!r.ok)
            throw new Error(`HTTP ${r.status} @ ${url}`);
        return r.json();
    });
    // Derivar columnas desde un registro muestra
    const getColumnsFromData = (sample) => {
        if (!sample || typeof sample !== 'object')
            return [...defaultColumns];
        const excluded = new Set(['RecID', 'RowVersion', 'DataareaID']);
        return Object.keys(sample).filter(k => !excluded.has(k));
    };
    // ==== DataTables ====
    // (1) Genera cabecera y crea DT. Sin .destroy(true), sin vaciar manualmente tbody.
    const initializeDataTable = (columns) => {
        // Asegurar columnas válidas vs. datos (previene "unknown parameter" y tabla invisible)
        const safeCols = columns.filter(c => allColumns.includes(c));
        const finalCols = safeCols.length ? safeCols : [...defaultColumns];
        if ($.fn.DataTable.isDataTable($table)) {
            const dt = $table.DataTable();
            dt.clear(); // limpia datos internos
            dt.destroy(); // destruye sin tocar la tabla base
        }
        const theadHtml = `
            <tr>
                <th style="width:40px;"><input type="checkbox" id="check-all" class="flat"/></th>
                ${finalCols.map(col => `<th data-field="${col}">${titleize(col)}</th>`).join('')}
            </tr>
        `;
        $table.find('thead').html(theadHtml);
        if ($.fn.iCheck)
            $('.flat').iCheck({ checkboxClass: 'icheckbox_flat-green' });
        const dtConfig = {
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
                    render: (_d, _t, row) => { var _a; return `<input type="checkbox" class="flat row-check" data-recid="${(_a = row.RecID) !== null && _a !== void 0 ? _a : ''}"/>`; }
                },
                ...finalCols.map(col => ({
                    data: col,
                    name: col,
                    defaultContent: '', // evita "unknown parameter" si falta el campo
                    render: (data) => formatCell(data, col)
                }))
            ],
            ajax: (_rq, callback) => {
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
                if ($.fn.iCheck)
                    $table.find('.flat').iCheck({ checkboxClass: 'icheckbox_flat-green' });
                const dt = $table.DataTable();
                dt.columns.adjust().responsive.recalc(); // asegura layout correcto
            }
        };
        $table.DataTable(dtConfig);
    };
    // Carga de datos
    const loadLoans = () => __awaiter(this, void 0, void 0, function* () {
        const url = `${apiBase}/Loans?pageNumber=1&pageSize=100`;
        const response = yield fetchJson(url);
        if (Array.isArray(response))
            return response;
        if ((response === null || response === void 0 ? void 0 : response.Data) && Array.isArray(response.Data))
            return response.Data;
        return [];
    });
    // ==== UI ====
    const updateSummary = (count) => {
        const el = d.getElementById('loans-summary');
        if (el)
            el.textContent = `${count} préstamo${count !== 1 ? 's' : ''}`;
    };
    const showError = (message) => {
        $table.find('tbody').html(`
            <tr>
                <td colspan="10" class="text-center text-danger">
                    <i class="fa fa-exclamation-triangle"></i> ${message}
                </td>
            </tr>
        `);
    };
    const updateButtonStates = () => {
        const n = $table.find('tbody input.row-check:checked').length;
        $('#btn-edit').prop('disabled', n !== 1);
        $('#btn-delete').prop('disabled', n === 0);
    };
    const exportToCSV = () => {
        var _a, _b, _c, _e, _f, _g;
        if (!loansData.length) {
            (_b = (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.warn) === null || _b === void 0 ? void 0 : _b.call(_a, 'No hay datos para exportar', 'Advertencia');
            return;
        }
        const dt = $table.DataTable();
        const rows = dt.rows({ search: 'applied' }).data().toArray();
        if (!rows.length) {
            (_e = (_c = w.ALERTS) === null || _c === void 0 ? void 0 : _c.warn) === null || _e === void 0 ? void 0 : _e.call(_c, 'No hay datos visibles para exportar', 'Advertencia');
            return;
        }
        const cols = visibleColumns.filter(c => allColumns.includes(c));
        const lines = [cols.map(c => titleize(c)).join(',')];
        rows.forEach(row => {
            const line = cols.map(c => {
                const str = String(formatCell(row[c], c)).replace(/<[^>]*>/g, '').replace(/"/g, '""');
                return /[",\n]/.test(str) ? `"${str}"` : str;
            }).join(',');
            lines.push(line);
        });
        const blob = new Blob(['\ufeff' + lines.join('\n')], { type: 'text/csv;charset=utf-8;' });
        const url = URL.createObjectURL(blob);
        const a = d.createElement('a');
        a.href = url;
        a.download = `Prestamos_${new Date().toISOString().slice(0, 10)}.csv`;
        d.body.appendChild(a);
        a.click();
        d.body.removeChild(a);
        URL.revokeObjectURL(url);
        (_g = (_f = w.ALERTS) === null || _f === void 0 ? void 0 : _f.ok) === null || _g === void 0 ? void 0 : _g.call(_f, 'Datos exportados correctamente', 'Éxito');
    };
    const deleteLoan = (recId) => __awaiter(this, void 0, void 0, function* () {
        const url = `${apiBase}/Loans/${recId}`;
        const headers = token ? { 'Authorization': `Bearer ${token}` } : {};
        const r = yield fetch(url, { method: 'DELETE', headers });
        if (!r.ok)
            throw new Error(`Error al eliminar préstamo: ${r.status}`);
    });
    // ==== Vistas y Columnas ====
    const applyColumnChanges = (newColumns) => {
        var _a;
        visibleColumns = newColumns;
        initializeDataTable(newColumns); // re-init limpio (sin duplicados ni desaparición)
        if ((_a = gridViewsManager === null || gridViewsManager === void 0 ? void 0 : gridViewsManager.hasCurrentView) === null || _a === void 0 ? void 0 : _a.call(gridViewsManager))
            $('#btn-save-view-changes').show();
    };
    const updateViewsDropdown = () => {
        try {
            const views = gridViewsManager.getAvailableViews();
            const $container = $('#saved-views-container');
            $container.empty();
            if (!(views === null || views === void 0 ? void 0 : views.length)) {
                $container.append('<li class="text-muted" style="padding: 5px 20px;">No hay vistas guardadas</li>');
                return;
            }
            views.forEach((v) => {
                const icon = v.IsDefault ? '<i class="fa fa-star text-warning"></i>' : '<i class="fa fa-file-o"></i>';
                const $item = $(`
                    <li style="position: relative;">
                        <a href="#" data-view-id="${v.RecID}" style="display:inline-block;width:80%;">${icon} ${v.ViewName} ${v.IsPublic ? '<span class="label label-info label-sm">Pública</span>' : ''}</a>
                        ${!v.IsDefault ? `<a href="#" class="btn-set-default" data-view-id="${v.RecID}" style="position:absolute;right:10px;top:5px;color:#999;" title="Predeterminada"><i class="fa fa-star-o"></i></a>` : ''}
                    </li>
                `);
                $item.find('a[data-view-id]').on('click', (e) => __awaiter(this, void 0, void 0, function* () { e.preventDefault(); yield loadViewById(v.RecID); }));
                $item.find('.btn-set-default').on('click', (e) => {
                    var _a, _b;
                    e.preventDefault();
                    e.stopPropagation();
                    (_b = (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.confirm) === null || _b === void 0 ? void 0 : _b.call(_a, `¿Establecer "${v.ViewName}" como predeterminada?`, 'Confirmar', (ok) => __awaiter(this, void 0, void 0, function* () {
                        if (!ok)
                            return;
                        const success = yield gridViewsManager.setDefaultView(v.RecID);
                        if (success)
                            updateViewsDropdown();
                    }));
                });
                $container.append($item);
            });
        }
        catch (err) {
            console.error('Error actualizando dropdown de vistas:', err);
        }
    };
    const loadViewById = (viewId) => __awaiter(this, void 0, void 0, function* () {
        var _a, _b, _c;
        try {
            const cfg = yield gridViewsManager.loadView(viewId);
            if (!(cfg === null || cfg === void 0 ? void 0 : cfg.length))
                return;
            gridColumnsManager.applyColumnConfig(cfg);
            const newCols = cfg.filter(c => c.visible).sort((a, b) => a.order - b.order).map(c => c.field);
            applyColumnChanges(newCols);
            const viewName = ((_a = gridViewsManager.getCurrentViewName) === null || _a === void 0 ? void 0 : _a.call(gridViewsManager)) || 'Vista por defecto';
            $('#current-view-name').text(viewName);
            $('#btn-save-view-changes').hide();
        }
        catch (err) {
            console.error('Error cargando vista:', err);
            (_c = (_b = w.ALERTS) === null || _b === void 0 ? void 0 : _b.error) === null || _c === void 0 ? void 0 : _c.call(_b, 'Error al cargar la vista', 'Error');
        }
    });
    // ==== Eventos UI ====
    // Check-all (delegado, funciona tras re-init)
    $(document).on('ifChanged', '#check-all', function () {
        const checked = $(this).is(':checked');
        const $checks = $table.find('tbody input.row-check');
        if ($.fn.iCheck)
            $checks.iCheck(checked ? 'check' : 'uncheck');
        else
            $checks.prop('checked', checked);
    });
    // Sincroniza botones por selección
    $(document).on('ifChanged', '.row-check', updateButtonStates);
    // Nuevo / Editar / Eliminar
    $('#btn-new').on('click', () => { window.location.href = '/Loan/NewEdit'; });
    $('#btn-edit').on('click', () => {
        const $c = $table.find('tbody input.row-check:checked').first();
        if ($c.length)
            window.location.href = `/Loan/NewEdit?recId=${$c.data('recid')}`;
    });
    $('#btn-delete').on('click', () => __awaiter(this, void 0, void 0, function* () {
        var _a, _b;
        const $checked = $table.find('tbody input.row-check:checked');
        const count = $checked.length;
        if (count === 0)
            return;
        const msg = count === 1 ? '¿Está seguro de eliminar este préstamo?' : `¿Está seguro de eliminar ${count} préstamos?`;
        (_b = (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.confirm) === null || _b === void 0 ? void 0 : _b.call(_a, msg, 'Confirmar Eliminación', (ok) => __awaiter(this, void 0, void 0, function* () {
            var _a, _b, _c, _e;
            if (!ok)
                return;
            try {
                const ops = [];
                $checked.each(function () {
                    const id = $(this).data('recid');
                    if (id)
                        ops.push(deleteLoan(String(id)));
                });
                yield Promise.all(ops);
                (_b = (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.ok) === null || _b === void 0 ? void 0 : _b.call(_a, 'Préstamo(s) eliminado(s) correctamente', 'Éxito');
                $table.DataTable().ajax.reload();
            }
            catch (err) {
                console.error('Error al eliminar:', err);
                (_e = (_c = w.ALERTS) === null || _c === void 0 ? void 0 : _c.error) === null || _e === void 0 ? void 0 : _e.call(_c, 'Error al eliminar préstamo(s)', 'Error');
            }
        }), { type: 'danger' });
    }));
    // Exportar/Importar
    $('#btn-export').on('click', exportToCSV);
    $('#btn-import').on('click', () => { var _a, _b; return (_b = (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.info) === null || _b === void 0 ? void 0 : _b.call(_a, 'Funcionalidad de importación próximamente', 'Información'); });
    // Click fila → toggle checkbox
    $table.on('click', 'tbody tr', function (e) {
        if ($(e.target).is('input.row-check') || $(e.target).closest('.icheckbox_flat-green').length)
            return;
        const $cb = $(this).find('input.row-check');
        if (!$cb.length)
            return;
        if ($.fn.iCheck)
            $cb.iCheck($cb.is(':checked') ? 'uncheck' : 'check');
        else
            $cb.prop('checked', !$cb.prop('checked'));
    });
    // Doble clic → editar
    $table.on('dblclick', 'tbody tr', function () {
        const id = $(this).find('input.row-check').data('recid');
        if (id)
            window.location.href = `/Loan/NewEdit?recId=${id}`;
    });
    // Columnas
    $('#btn-manage-columns').on('click', () => { var _a; (_a = gridColumnsManager === null || gridColumnsManager === void 0 ? void 0 : gridColumnsManager.showColumnsModal) === null || _a === void 0 ? void 0 : _a.call(gridColumnsManager); });
    $('#btn-apply-columns').on('click', () => {
        const cfg = gridColumnsManager.applyColumns();
        const newCols = cfg.filter(c => c.visible).sort((a, b) => a.order - b.order).map(c => c.field);
        applyColumnChanges(newCols);
        $('#modal-manage-columns').modal('hide');
    });
    // Guardar vista
    $('#btn-new-view').on('click', (e) => {
        e.preventDefault();
        $('#view-name').val('');
        $('#view-is-default').prop('checked', false);
        $('#view-is-public').prop('checked', false);
        $('#modal-save-view').modal('show');
    });
    $('#btn-confirm-save-view').on('click', () => __awaiter(this, void 0, void 0, function* () {
        var _a, _b;
        const viewName = String($('#view-name').val() || '').trim();
        const isDefault = $('#view-is-default').is(':checked');
        const isPublic = $('#view-is-public').is(':checked');
        if (!viewName) {
            (_b = (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.warn) === null || _b === void 0 ? void 0 : _b.call(_a, 'Ingrese un nombre para la vista', 'Advertencia');
            return;
        }
        const cfg = gridColumnsManager.getCurrentColumnConfig();
        const ok = yield gridViewsManager.saveView(viewName, cfg, isDefault, isPublic);
        if (ok) {
            $('#modal-save-view').modal('hide');
            $('#current-view-name').text(viewName);
            $('#btn-save-view-changes').hide();
            updateViewsDropdown();
        }
    }));
    // Guardar Como
    $('#btn-save-as-view').on('click', () => {
        $('#view-name-saveas').val('');
        $('#view-is-default-saveas').prop('checked', false);
        $('#view-is-public-saveas').prop('checked', false);
        $('#modal-save-as-view').modal('show');
    });
    $('#btn-confirm-save-as').on('click', () => __awaiter(this, void 0, void 0, function* () {
        var _a, _b;
        const viewName = String($('#view-name-saveas').val() || '').trim();
        const isDefault = $('#view-is-default-saveas').is(':checked');
        const isPublic = $('#view-is-public-saveas').is(':checked');
        if (!viewName) {
            (_b = (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.warn) === null || _b === void 0 ? void 0 : _b.call(_a, 'Ingrese un nombre para la vista', 'Advertencia');
            return;
        }
        const cfg = gridColumnsManager.getCurrentColumnConfig();
        const ok = yield gridViewsManager.saveView(viewName, cfg, isDefault, isPublic);
        if (ok) {
            $('#modal-save-as-view').modal('hide');
            $('#current-view-name').text(viewName);
            $('#btn-save-view-changes').hide();
            updateViewsDropdown();
        }
    }));
    // Guardar cambios de vista activa
    $('#btn-save-view-changes').on('click', () => __awaiter(this, void 0, void 0, function* () {
        var _a, _b, _c;
        if (!((_a = gridViewsManager === null || gridViewsManager === void 0 ? void 0 : gridViewsManager.hasCurrentView) === null || _a === void 0 ? void 0 : _a.call(gridViewsManager))) {
            (_c = (_b = w.ALERTS) === null || _b === void 0 ? void 0 : _b.warn) === null || _c === void 0 ? void 0 : _c.call(_b, 'No hay vista activa para actualizar', 'Advertencia');
            return;
        }
        const cfg = gridColumnsManager.getCurrentColumnConfig();
        const ok = yield gridViewsManager.updateView(cfg);
        if (ok)
            $('#btn-save-view-changes').hide();
    }));
    // Restablecer vista por defecto
    $('#btn-reset-view').on('click', () => {
        var _a, _b;
        (_b = (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.confirm) === null || _b === void 0 ? void 0 : _b.call(_a, '¿Desea restablecer a la vista por defecto?', 'Confirmar', (ok) => {
            if (!ok)
                return;
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
    $(function () {
        return __awaiter(this, void 0, void 0, function* () {
            var _a, _b;
            try {
                // Descubrir columnas
                const probeUrl = `${apiBase}/Loans?pageNumber=1&pageSize=1`;
                const probe = yield fetchJson(probeUrl);
                if (Array.isArray(probe) && probe.length) {
                    allColumns = getColumnsFromData(probe[0]);
                }
                else if (!Array.isArray(probe) && ((_a = probe === null || probe === void 0 ? void 0 : probe.Data) === null || _a === void 0 ? void 0 : _a.length)) {
                    allColumns = getColumnsFromData(probe.Data[0]);
                }
                else {
                    allColumns = [...defaultColumns];
                }
                const GVM = w.GridViewsManager;
                const GCM = w.GridColumnsManager;
                if (!GVM || !GCM) {
                    console.warn('GridViewsManager / GridColumnsManager no disponibles. Usando vista por defecto.');
                    visibleColumns = [...defaultColumns];
                    initializeDataTable(visibleColumns);
                    return;
                }
                gridViewsManager = new GVM(apiBase, token, "Loans", userRefRecID, dataareaId);
                const saved = yield gridViewsManager.initialize();
                if (saved === null || saved === void 0 ? void 0 : saved.length) {
                    visibleColumns = saved.filter(c => c.visible).sort((a, b) => a.order - b.order).map(c => c.field);
                    $('#current-view-name').text(((_b = gridViewsManager.getCurrentViewName) === null || _b === void 0 ? void 0 : _b.call(gridViewsManager)) || 'Vista por defecto');
                }
                else {
                    visibleColumns = [...defaultColumns];
                }
                gridColumnsManager = new GCM(allColumns, visibleColumns, (newCols) => applyColumnChanges(newCols));
                updateViewsDropdown();
                initializeDataTable(visibleColumns); // Primera carga (sin duplicados y sin desaparecer)
            }
            catch (err) {
                console.error('Error en inicialización:', err);
                visibleColumns = [...defaultColumns];
                initializeDataTable(visibleColumns);
                showError('Error al cargar la configuración');
            }
        });
    });
})();
//# sourceMappingURL=loan-dt.js.map