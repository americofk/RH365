// ============================================================================
// Archivo: courses-dt.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Courses/courses-dt.ts
// Descripción:
//   - Lista de Cursos con DataTables
//   - Gestión de vistas de usuario (UserGridViews)
//   - Genera columnas y filas dinámicamente desde API
// Estándar: ISO 27001 - Gestión segura de datos de formación
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
    const apiBase = w.RH365.urls.apiBase;
    const pageContainer = d.querySelector("#courses-page");
    if (!pageContainer)
        return;
    const token = pageContainer.getAttribute("data-token") || "";
    const dataareaId = pageContainer.getAttribute("data-dataarea") || "DAT";
    const userRefRecID = parseInt(pageContainer.getAttribute("data-user") || "0", 10);
    const $table = $("#courses-table");
    if (!$table.length)
        return;
    let coursesData = [];
    let allColumns = [];
    let visibleColumns = [];
    const defaultColumns = ['ID', 'CourseCode', 'Name', 'StartDate', 'EndDate', 'CourseStatus', 'CreatedOn'];
    let gridViewsManager;
    let gridColumnsManager;
    // Cache de tipos de curso y salones
    let courseTypesMap = new Map();
    let classRoomsMap = new Map();
    const titleize = (field) => {
        const translations = {
            'RecID': 'ID Registro',
            'ID': 'ID',
            'CourseCode': 'Código Curso',
            'Name': 'Nombre',
            'CourseTypeRefRecID': 'Tipo de Curso',
            'ClassRoomRefRecID': 'Salón',
            'Description': 'Descripción',
            'StartDate': 'Fecha Inicio',
            'EndDate': 'Fecha Fin',
            'IsMatrixTraining': 'Matriz Formación',
            'InternalExternal': 'Interno/Externo',
            'MinStudents': 'Mín. Estudiantes',
            'MaxStudents': 'Máx. Estudiantes',
            'Periodicity': 'Periodicidad',
            'QtySessions': 'Cant. Sesiones',
            'Objetives': 'Objetivos',
            'Topics': 'Temas',
            'CourseStatus': 'Estado',
            'Observations': 'Observaciones',
            'CreatedOn': 'Fecha Creación',
            'CreatedBy': 'Creado Por',
            'ModifiedOn': 'Modificado',
            'ModifiedBy': 'Modificado Por'
        };
        return translations[field] || field.replace(/([a-z])([A-Z])/g, "$1 $2").replace(/_/g, " ").replace(/^./, (c) => c.toUpperCase());
    };
    const formatCell = (value, field) => {
        if (value == null)
            return "";
        if (typeof value === "boolean") {
            if (field === "IsMatrixTraining") {
                return value ? '<span class="label label-info">Sí</span>' : '<span class="label label-default">No</span>';
            }
            return value ? "Sí" : "No";
        }
        if (field === "CourseTypeRefRecID" && typeof value === "number") {
            const typeName = courseTypesMap.get(value);
            return typeName || `ID: ${value}`;
        }
        if (field === "ClassRoomRefRecID" && typeof value === "number") {
            const roomName = classRoomsMap.get(value);
            return roomName || `ID: ${value}`;
        }
        if (field === "CourseStatus" && typeof value === "number") {
            const statusMap = {
                0: '<span class="label label-warning">Borrador</span>',
                1: '<span class="label label-info">Planificado</span>',
                2: '<span class="label label-success">En Curso</span>',
                3: '<span class="label label-default">Finalizado</span>',
                4: '<span class="label label-danger">Cancelado</span>'
            };
            return statusMap[value] || String(value);
        }
        if (field === "InternalExternal" && typeof value === "number") {
            return value === 0 ? 'Interno' : 'Externo';
        }
        if (typeof value === "string" && /^\d{4}-\d{2}-\d{2}T/.test(value)) {
            const dt = new Date(value);
            if (!isNaN(dt.getTime())) {
                return dt.toLocaleDateString('es-DO', { day: '2-digit', month: '2-digit', year: 'numeric' });
            }
        }
        return String(value);
    };
    const fetchJson = (url) => __awaiter(this, void 0, void 0, function* () {
        const headers = {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        };
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }
        const response = yield fetch(url, { headers });
        if (!response.ok) {
            throw new Error(`HTTP ${response.status} @ ${url}`);
        }
        return response.json();
    });
    const getColumnsFromData = (sample) => {
        if (!sample || typeof sample !== 'object') {
            return [...defaultColumns];
        }
        const excluded = new Set(['RecID', 'RowVersion', 'DataareaID']);
        return Object.keys(sample).filter(col => !excluded.has(col));
    };
    const initializeDataTable = (columns) => {
        if ($.fn.DataTable.isDataTable($table)) {
            $table.DataTable().destroy();
        }
        const theadHtml = `
            <tr>
                <th style="width:40px;"><input type="checkbox" id="check-all" class="flat"/></th>
                ${columns.map(col => `<th data-field="${col}">${titleize(col)}</th>`).join('')}
            </tr>
        `;
        $table.find('thead').html(theadHtml);
        if ($.fn.iCheck) {
            $('.flat').iCheck({ checkboxClass: 'icheckbox_flat-green' });
        }
        const dtConfig = {
            processing: true,
            serverSide: false,
            responsive: true,
            autoWidth: false,
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
                    render: (_data, _type, row) => {
                        return `<input type="checkbox" class="flat row-check" data-recid="${row.RecID || ''}"/>`;
                    }
                },
                ...columns.map(col => ({ data: col, name: col, render: (data) => formatCell(data, col) }))
            ],
            ajax: (_data, callback) => {
                loadCourses().then(items => {
                    coursesData = items;
                    callback({ data: items });
                    updateSummary(items.length);
                }).catch(err => {
                    console.error('Error cargando cursos:', err);
                    showError(err.message);
                    callback({ data: [] });
                });
            },
            drawCallback: function () {
                if ($.fn.iCheck) {
                    $table.find('.flat').iCheck({ checkboxClass: 'icheckbox_flat-green' });
                }
            }
        };
        $table.DataTable(dtConfig);
    };
    const loadCourses = () => __awaiter(this, void 0, void 0, function* () {
        try {
            const url = `${apiBase}/Courses?pageNumber=1&pageSize=100`;
            const response = yield fetchJson(url);
            // Manejar array directo
            if (Array.isArray(response)) {
                return response;
            }
            // Manejar objeto con propiedad Data
            if ((response === null || response === void 0 ? void 0 : response.Data) && Array.isArray(response.Data)) {
                return response.Data;
            }
            console.error('❌ Formato de respuesta no reconocido');
            throw new Error('Respuesta del API inválida');
        }
        catch (error) {
            console.error('❌ Error en loadCourses:', error);
            throw error;
        }
    });
    const loadCourseTypes = () => __awaiter(this, void 0, void 0, function* () {
        try {
            const url = `${apiBase}/CourseTypes?pageNumber=1&pageSize=1000`;
            const response = yield fetchJson(url);
            let courseTypes = [];
            if (Array.isArray(response)) {
                courseTypes = response;
            }
            else if ((response === null || response === void 0 ? void 0 : response.Data) && Array.isArray(response.Data)) {
                courseTypes = response.Data;
            }
            courseTypes.forEach((type) => {
                if (type.RecID && type.Name) {
                    courseTypesMap.set(type.RecID, type.Name);
                }
            });
        }
        catch (error) {
            console.error('⚠️ Error cargando tipos de curso:', error);
        }
    });
    const loadClassRooms = () => __awaiter(this, void 0, void 0, function* () {
        try {
            const url = `${apiBase}/ClassRooms?pageNumber=1&pageSize=1000`;
            const response = yield fetchJson(url);
            let classRooms = [];
            if (Array.isArray(response)) {
                classRooms = response;
            }
            else if ((response === null || response === void 0 ? void 0 : response.Data) && Array.isArray(response.Data)) {
                classRooms = response.Data;
            }
            classRooms.forEach((room) => {
                if (room.RecID && room.Name) {
                    classRoomsMap.set(room.RecID, room.Name);
                }
            });
        }
        catch (error) {
            console.error('⚠️ Error cargando salones:', error);
        }
    });
    const updateSummary = (count) => {
        const summary = d.getElementById('courses-summary');
        if (summary) {
            summary.textContent = `${count} curso${count !== 1 ? 's' : ''}`;
        }
    };
    const showError = (message) => {
        const errorHtml = `
            <tr>
                <td colspan="10" class="text-center text-danger">
                    <i class="fa fa-exclamation-triangle"></i> ${message}
                </td>
            </tr>
        `;
        $table.find('tbody').html(errorHtml);
    };
    const updateButtonStates = () => {
        const checkedCount = $table.find('tbody input.row-check:checked').length;
        $('#btn-edit').prop('disabled', checkedCount !== 1);
        $('#btn-delete').prop('disabled', checkedCount === 0);
    };
    const exportToCSV = () => {
        if (!coursesData.length) {
            w.ALERTS.warn('No hay datos para exportar', 'Advertencia');
            return;
        }
        const dt = $table.DataTable();
        const rows = dt.rows({ search: 'applied' }).data().toArray();
        if (!rows.length) {
            w.ALERTS.warn('No hay datos visibles para exportar', 'Advertencia');
            return;
        }
        const columns = visibleColumns;
        const csvLines = [columns.map(col => titleize(col)).join(',')];
        rows.forEach(row => {
            const line = columns.map(col => {
                const value = row[col];
                const str = value == null ? '' : String(value).replace(/"/g, '""');
                return /[",\n]/.test(str) ? `"${str}"` : str;
            }).join(',');
            csvLines.push(line);
        });
        const blob = new Blob([csvLines.join('\n')], { type: 'text/csv;charset=utf-8;' });
        const url = URL.createObjectURL(blob);
        const link = d.createElement('a');
        const fileName = `Cursos_${new Date().toISOString().slice(0, 10)}.csv`;
        link.href = url;
        link.download = fileName;
        link.style.visibility = 'hidden';
        d.body.appendChild(link);
        link.click();
        d.body.removeChild(link);
        URL.revokeObjectURL(url);
    };
    const deleteCourse = (recId) => __awaiter(this, void 0, void 0, function* () {
        const url = `${apiBase}/Courses/${recId}`;
        const headers = { 'Authorization': `Bearer ${token}` };
        const response = yield fetch(url, { method: 'DELETE', headers });
        if (!response.ok) {
            throw new Error(`Error al eliminar curso: ${response.status}`);
        }
    });
    const applyColumnChanges = (newColumns) => {
        visibleColumns = newColumns;
        initializeDataTable(newColumns);
        if (gridViewsManager && gridViewsManager.hasCurrentView()) {
            $('#btn-save-view-changes').show();
        }
    };
    const updateViewsDropdown = () => {
        const views = gridViewsManager.getAvailableViews();
        const container = $('#saved-views-container');
        container.empty();
        if (views.length === 0) {
            container.append('<li class="text-muted" style="padding: 5px 20px;">No hay vistas guardadas</li>');
            return;
        }
        views.forEach((view) => {
            const icon = view.IsDefault ? '<i class="fa fa-star text-warning"></i>' : '<i class="fa fa-file-o"></i>';
            const item = $(`
            <li style="position: relative;">
                <a href="#" data-view-id="${view.RecID}" style="display: inline-block; width: 80%;">
                    ${icon} ${view.ViewName}
                    ${view.IsPublic ? '<span class="label label-info label-sm">Pública</span>' : ''}
                </a>
                ${!view.IsDefault ? `
                    <a href="#" class="btn-set-default" data-view-id="${view.RecID}" 
                       style="position: absolute; right: 10px; top: 5px; color: #999;" 
                       title="Establecer como predeterminada">
                        <i class="fa fa-star-o"></i>
                    </a>
                ` : ''}
            </li>
        `);
            item.find('a[data-view-id]').first().on('click', function (e) {
                return __awaiter(this, void 0, void 0, function* () {
                    e.preventDefault();
                    yield loadViewById(view.RecID);
                });
            });
            item.find('.btn-set-default').on('click', function (e) {
                return __awaiter(this, void 0, void 0, function* () {
                    e.preventDefault();
                    e.stopPropagation();
                    w.ALERTS.confirm(`¿Establecer "${view.ViewName}" como vista predeterminada?`, 'Confirmar', (confirmed) => __awaiter(this, void 0, void 0, function* () {
                        if (confirmed) {
                            const success = yield gridViewsManager.setDefaultView(view.RecID);
                            if (success) {
                                updateViewsDropdown();
                            }
                        }
                    }));
                });
            });
            container.append(item);
        });
    };
    const loadViewById = (viewId) => __awaiter(this, void 0, void 0, function* () {
        try {
            const columnConfigs = yield gridViewsManager.loadView(viewId);
            if (columnConfigs.length > 0) {
                gridColumnsManager.applyColumnConfig(columnConfigs);
                const newColumns = columnConfigs.filter(c => c.visible).sort((a, b) => a.order - b.order).map(c => c.field);
                applyColumnChanges(newColumns);
                const viewName = gridViewsManager.getCurrentViewName();
                $('#current-view-name').text(viewName);
                $('#btn-save-view-changes').hide();
            }
        }
        catch (error) {
            console.error('Error cargando vista:', error);
        }
    });
    $(document).on('ifChanged', '#check-all', function () {
        const isChecked = $(this).is(':checked');
        $table.find('tbody input.row-check').iCheck(isChecked ? 'check' : 'uncheck');
    });
    $(document).on('ifChanged', '.row-check', function () {
        const total = $table.find('tbody input.row-check').length;
        const checked = $table.find('tbody input.row-check:checked').length;
        if (checked === total && total > 0) {
            $('#check-all').iCheck('check');
        }
        else {
            $('#check-all').iCheck('uncheck');
        }
        updateButtonStates();
    });
    $('#btn-new').on('click', () => { window.location.href = '/Course/NewEdit'; });
    $('#btn-edit').on('click', () => {
        const $checked = $table.find('tbody input.row-check:checked').first();
        if ($checked.length) {
            const recId = $checked.data('recid');
            window.location.href = `/Course/NewEdit?recId=${recId}`;
        }
    });
    $('#btn-delete').on('click', () => __awaiter(this, void 0, void 0, function* () {
        const $checked = $table.find('tbody input.row-check:checked');
        const count = $checked.length;
        if (count === 0)
            return;
        const message = count === 1
            ? '¿Está seguro de eliminar este curso?'
            : `¿Está seguro de eliminar ${count} cursos?`;
        w.ALERTS.confirm(message, 'Confirmar Eliminación', (confirmed) => __awaiter(this, void 0, void 0, function* () {
            if (!confirmed)
                return;
            try {
                const promises = [];
                $checked.each(function () {
                    const recId = $(this).data('recid');
                    if (recId) {
                        promises.push(deleteCourse(recId));
                    }
                });
                yield Promise.all(promises);
                w.ALERTS.ok('Curso(s) eliminado(s) correctamente', 'Éxito');
                $table.DataTable().ajax.reload();
            }
            catch (error) {
                console.error('Error al eliminar:', error);
                w.ALERTS.error('Error al eliminar curso(s)', 'Error');
            }
        }), { type: 'danger' });
    }));
    $('#btn-export').on('click', exportToCSV);
    $('#btn-import').on('click', () => {
        w.ALERTS.info('Funcionalidad de importación próximamente', 'Información');
    });
    $table.on('click', 'tbody tr', function (e) {
        if ($(e.target).is('input.row-check') || $(e.target).closest('.icheckbox_flat-green').length) {
            return;
        }
        const $row = $(this);
        const $checkbox = $row.find('input.row-check');
        if ($checkbox.length) {
            const isChecked = $checkbox.is(':checked');
            $checkbox.iCheck(isChecked ? 'uncheck' : 'check');
        }
    });
    $table.on('dblclick', 'tbody tr', function () {
        const $row = $(this);
        const $checkbox = $row.find('input.row-check');
        const recId = $checkbox.data('recid');
        if (recId) {
            window.location.href = `/Course/NewEdit?recId=${recId}`;
        }
    });
    $('#btn-manage-columns').on('click', () => { gridColumnsManager.showColumnsModal(); });
    $('#btn-apply-columns').on('click', () => {
        const columnConfigs = gridColumnsManager.applyColumns();
        const newColumns = columnConfigs.filter(c => c.visible).sort((a, b) => a.order - b.order).map(c => c.field);
        applyColumnChanges(newColumns);
        $('#modal-manage-columns').modal('hide');
    });
    $('#btn-new-view').on('click', (e) => {
        e.preventDefault();
        $('#view-name').val('');
        $('#view-is-default').prop('checked', false);
        $('#view-is-public').prop('checked', false);
        $('#modal-save-view').modal('show');
    });
    $('#btn-confirm-save-view').on('click', () => __awaiter(this, void 0, void 0, function* () {
        const viewName = $('#view-name').val().trim();
        const isDefault = $('#view-is-default').is(':checked');
        const isPublic = $('#view-is-public').is(':checked');
        if (!viewName) {
            w.ALERTS.warn('Por favor ingrese un nombre para la vista', 'Advertencia');
            return;
        }
        const columnConfigs = gridColumnsManager.getCurrentColumnConfig();
        const success = yield gridViewsManager.saveView(viewName, columnConfigs, isDefault, isPublic);
        if (success) {
            $('#modal-save-view').modal('hide');
            $('#current-view-name').text(viewName);
            $('#btn-save-view-changes').hide();
            updateViewsDropdown();
        }
    }));
    $('#btn-save-as-view').on('click', () => {
        $('#view-name-saveas').val('');
        $('#view-is-default-saveas').prop('checked', false);
        $('#view-is-public-saveas').prop('checked', false);
        $('#modal-save-as-view').modal('show');
    });
    $('#btn-confirm-save-as').on('click', () => __awaiter(this, void 0, void 0, function* () {
        const viewName = $('#view-name-saveas').val().trim();
        const isDefault = $('#view-is-default-saveas').is(':checked');
        const isPublic = $('#view-is-public-saveas').is(':checked');
        if (!viewName) {
            w.ALERTS.warn('Por favor ingrese un nombre para la vista', 'Advertencia');
            return;
        }
        const columnConfigs = gridColumnsManager.getCurrentColumnConfig();
        const success = yield gridViewsManager.saveView(viewName, columnConfigs, isDefault, isPublic);
        if (success) {
            $('#modal-save-as-view').modal('hide');
            $('#current-view-name').text(viewName);
            $('#btn-save-view-changes').hide();
            updateViewsDropdown();
        }
    }));
    $('#btn-save-view-changes').on('click', () => __awaiter(this, void 0, void 0, function* () {
        if (!gridViewsManager.hasCurrentView()) {
            w.ALERTS.warn('No hay vista activa para actualizar', 'Advertencia');
            return;
        }
        const columnConfigs = gridColumnsManager.getCurrentColumnConfig();
        const success = yield gridViewsManager.updateView(columnConfigs);
        if (success) {
            $('#btn-save-view-changes').hide();
        }
    }));
    $('#btn-reset-view').on('click', () => {
        w.ALERTS.confirm('¿Desea restablecer a la vista por defecto?', 'Confirmar', (confirmed) => {
            if (confirmed) {
                gridColumnsManager.resetToDefault(defaultColumns);
                applyColumnChanges(defaultColumns);
                $('#current-view-name').text('Vista por defecto');
                $('#btn-save-view-changes').hide();
            }
        });
    });
    $(document).on('click', '[data-view="default"]', (e) => {
        e.preventDefault();
        gridColumnsManager.resetToDefault(defaultColumns);
        applyColumnChanges(defaultColumns);
        $('#current-view-name').text('Vista por defecto');
        $('#btn-save-view-changes').hide();
    });
    $(function () {
        return __awaiter(this, void 0, void 0, function* () {
            var _a;
            try {
                // Cargar tipos de curso y salones en paralelo
                yield Promise.all([
                    loadCourseTypes(),
                    loadClassRooms()
                ]);
                const probeUrl = `${apiBase}/Courses?pageNumber=1&pageSize=1`;
                const probe = yield fetchJson(probeUrl);
                if ((_a = probe === null || probe === void 0 ? void 0 : probe.Data) === null || _a === void 0 ? void 0 : _a.length) {
                    allColumns = getColumnsFromData(probe.Data[0]);
                }
                else {
                    allColumns = [...defaultColumns];
                }
                const GridViewsManagerClass = w.GridViewsManager;
                const GridColumnsManagerClass = w.GridColumnsManager;
                if (!GridViewsManagerClass || !GridColumnsManagerClass) {
                    console.error('GridViewsManager o GridColumnsManager no están disponibles');
                    visibleColumns = [...defaultColumns];
                    initializeDataTable(visibleColumns);
                    return;
                }
                gridViewsManager = new GridViewsManagerClass(apiBase, token, "Courses", userRefRecID, dataareaId);
                const savedColumns = yield gridViewsManager.initialize();
                if (savedColumns.length > 0) {
                    visibleColumns = savedColumns.filter(c => c.visible).sort((a, b) => a.order - b.order).map(c => c.field);
                    const viewName = gridViewsManager.getCurrentViewName();
                    $('#current-view-name').text(viewName);
                }
                else {
                    visibleColumns = [...defaultColumns];
                }
                gridColumnsManager = new GridColumnsManagerClass(allColumns, visibleColumns, (newColumns) => {
                    applyColumnChanges(newColumns);
                });
                updateViewsDropdown();
                initializeDataTable(visibleColumns);
            }
            catch (error) {
                console.error('Error en inicialización:', error);
                visibleColumns = [...defaultColumns];
                initializeDataTable(visibleColumns);
                showError('Error al cargar la configuración');
            }
        });
    });
})();
//# sourceMappingURL=courses-dt.js.map