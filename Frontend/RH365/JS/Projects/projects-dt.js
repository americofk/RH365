// ============================================================================
// Archivo: projects-list.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Projects/projects-list.ts
// Descripción:
//   - Lista de Proyectos con DataTables
//   - Genera columnas y filas dinámicamente desde API
//   - Compatible con compilación TypeScript
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
    var _a, _b;
    const w = window;
    const d = document;
    const $ = w.jQuery || w.$;
    // Configuración
    const apiBase = ((_b = (_a = w.RH365) === null || _a === void 0 ? void 0 : _a.urls) === null || _b === void 0 ? void 0 : _b.apiBase) || "http://localhost:9595/api";
    const pageContainer = d.querySelector("#projects-page");
    if (!pageContainer)
        return;
    const token = pageContainer.getAttribute("data-token") || "";
    const dataareaId = pageContainer.getAttribute("data-dataarea") || "DAT";
    const userRefRecID = pageContainer.getAttribute("data-user") || "0";
    const $table = $("#projects-table");
    if (!$table.length)
        return;
    let projectsData = [];
    // ========================================================================
    // UTILIDADES
    // ========================================================================
    /**
     * Convertir campo a título legible
     */
    const titleize = (field) => {
        const translations = {
            'ID': 'ID Sistema',
            'ProjectCode': 'Código Proyecto',
            'Name': 'Nombre',
            'LedgerAccount': 'Cuenta Contable',
            'ProjectStatus': 'Estado',
            'CreatedOn': 'Fecha Creación',
            'CreatedBy': 'Creado Por',
            'ModifiedOn': 'Modificado',
            'ModifiedBy': 'Modificado Por',
            'Observations': 'Observaciones'
        };
        return translations[field] || field
            .replace(/([a-z])([A-Z])/g, "$1 $2")
            .replace(/_/g, " ")
            .replace(/^./, (c) => c.toUpperCase());
    };
    /**
     * Formatear valor de celda según tipo
     */
    const formatCell = (value, field) => {
        if (value == null)
            return "";
        // Booleanos
        if (typeof value === "boolean") {
            if (field === "ProjectStatus") {
                return value
                    ? '<span class="label label-success">Activo</span>'
                    : '<span class="label label-danger">Inactivo</span>';
            }
            return value ? "Sí" : "No";
        }
        // Fechas
        if (typeof value === "string" && /^\d{4}-\d{2}-\d{2}T/.test(value)) {
            const dt = new Date(value);
            if (!isNaN(dt.getTime())) {
                return dt.toLocaleDateString('es-DO', {
                    day: '2-digit',
                    month: '2-digit',
                    year: 'numeric'
                });
            }
        }
        return String(value);
    };
    /**
     * Realizar petición al API
     */
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
    // ========================================================================
    // FUNCIONES PRINCIPALES
    // ========================================================================
    /**
     * Obtener columnas desde primer registro
     */
    const getColumnsFromData = (sample) => {
        if (!sample || typeof sample !== 'object') {
            return ['ID', 'ProjectCode', 'Name', 'LedgerAccount', 'ProjectStatus', 'CreatedOn'];
        }
        const excluded = new Set(['RecID', 'RowVersion', 'DataareaID']);
        return Object.keys(sample).filter(col => !excluded.has(col));
    };
    /**
     * Inicializar DataTable
     */
    const initializeDataTable = (columns) => {
        // Limpiar thead
        const theadHtml = `
            <tr>
                <th style="width:40px;"><input type="checkbox" id="check-all" class="flat"/></th>
                ${columns.map(col => `<th data-field="${col}">${titleize(col)}</th>`).join('')}
            </tr>
        `;
        $table.find('thead').html(theadHtml);
        // Inicializar iCheck para checkboxes
        if ($.fn.iCheck) {
            $('.flat').iCheck({
                checkboxClass: 'icheckbox_flat-green'
            });
        }
        // Configurar DataTable
        const dtConfig = {
            processing: true,
            serverSide: false,
            responsive: true,
            autoWidth: false,
            order: [[1, 'asc']], // Ordenar por primera columna después del checkbox
            pageLength: 25,
            lengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
            language: {
                lengthMenu: 'Mostrar _MENU_ registros',
                zeroRecords: 'No se encontraron resultados',
                info: 'Mostrando _START_ a _END_ de _TOTAL_ registros',
                infoEmpty: 'No hay registros',
                infoFiltered: '(filtrado de _MAX_ registros)',
                search: 'Buscar:',
                paginate: {
                    first: 'Primera',
                    last: 'Última',
                    next: 'Siguiente',
                    previous: 'Anterior'
                },
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
                ...columns.map(col => ({
                    data: col,
                    name: col,
                    render: (data) => formatCell(data, col)
                }))
            ],
            ajax: (_data, callback) => {
                loadProjects()
                    .then(items => {
                    projectsData = items;
                    callback({ data: items });
                    updateSummary(items.length);
                })
                    .catch(err => {
                    console.error('Error cargando proyectos:', err);
                    showError(err.message);
                    callback({ data: [] });
                });
            },
            drawCallback: function () {
                // Re-inicializar iCheck después de cada draw
                if ($.fn.iCheck) {
                    $table.find('.flat').iCheck({
                        checkboxClass: 'icheckbox_flat-green'
                    });
                }
            }
        };
        $table.DataTable(dtConfig);
    };
    /**
     * Cargar proyectos desde API
     */
    const loadProjects = () => __awaiter(this, void 0, void 0, function* () {
        const url = `${apiBase}/Projects?pageNumber=1&pageSize=1000`;
        console.log('Cargando proyectos desde:', url);
        const response = yield fetchJson(url);
        if ((response === null || response === void 0 ? void 0 : response.Data) && Array.isArray(response.Data)) {
            console.log(`✓ ${response.Data.length} proyectos cargados`);
            return response.Data;
        }
        throw new Error('Respuesta del API inválida');
    });
    /**
     * Actualizar resumen de registros
     */
    const updateSummary = (count) => {
        const summary = d.getElementById('projects-summary');
        if (summary) {
            summary.textContent = `${count} proyecto${count !== 1 ? 's' : ''}`;
        }
    };
    /**
     * Mostrar error en la tabla
     */
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
    /**
     * Actualizar estado de botones según selección
     */
    const updateButtonStates = () => {
        const checkedCount = $table.find('tbody input.row-check:checked').length;
        $('#btn-edit').prop('disabled', checkedCount !== 1);
        $('#btn-delete').prop('disabled', checkedCount === 0);
    };
    /**
     * Exportar a CSV
     */
    const exportToCSV = () => {
        if (!projectsData.length) {
            alert('No hay datos para exportar');
            return;
        }
        const dt = $table.DataTable();
        const rows = dt.rows({ search: 'applied' }).data().toArray();
        if (!rows.length) {
            alert('No hay datos visibles para exportar');
            return;
        }
        // Obtener columnas (excluyendo el checkbox)
        const columns = dt.columns().header().toArray()
            .slice(1) // Saltar checkbox
            .map((th) => th.getAttribute('data-field') || '');
        // Crear CSV
        const csvLines = [
            columns.map(col => titleize(col)).join(',')
        ];
        rows.forEach(row => {
            const line = columns.map(col => {
                const value = row[col];
                const str = value == null ? '' : String(value).replace(/"/g, '""');
                return /[",\n]/.test(str) ? `"${str}"` : str;
            }).join(',');
            csvLines.push(line);
        });
        // Descargar archivo
        const blob = new Blob([csvLines.join('\n')], { type: 'text/csv;charset=utf-8;' });
        const url = URL.createObjectURL(blob);
        const link = d.createElement('a');
        const fileName = `Proyectos_${new Date().toISOString().slice(0, 10)}.csv`;
        link.href = url;
        link.download = fileName;
        link.style.visibility = 'hidden';
        d.body.appendChild(link);
        link.click();
        d.body.removeChild(link);
        URL.revokeObjectURL(url);
    };
    /**
     * Eliminar proyecto
     */
    const deleteProject = (recId) => __awaiter(this, void 0, void 0, function* () {
        const url = `${apiBase}/Projects/${recId}`;
        const headers = {
            'Authorization': `Bearer ${token}`
        };
        const response = yield fetch(url, { method: 'DELETE', headers });
        if (!response.ok) {
            throw new Error(`Error al eliminar proyecto: ${response.status}`);
        }
    });
    // ========================================================================
    // EVENT HANDLERS
    // ========================================================================
    /**
     * Checkbox "Seleccionar todos"
     */
    $(document).on('ifChanged', '#check-all', function () {
        const isChecked = $(this).is(':checked');
        $table.find('tbody input.row-check').iCheck(isChecked ? 'check' : 'uncheck');
    });
    /**
     * Checkboxes individuales
     */
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
    /**
     * Botón Nuevo
     */
    $('#btn-new').on('click', () => {
        window.location.href = '/Project/NewEdit';
    });
    /**
     * Botón Editar
     */
    $('#btn-edit').on('click', () => {
        const $checked = $table.find('tbody input.row-check:checked').first();
        if ($checked.length) {
            const recId = $checked.data('recid');
            window.location.href = `/Project/NewEdit?recId=${recId}`;
        }
    });
    /**
     * Botón Eliminar
     */
    $('#btn-delete').on('click', () => __awaiter(this, void 0, void 0, function* () {
        const $checked = $table.find('tbody input.row-check:checked');
        const count = $checked.length;
        if (count === 0)
            return;
        const message = count === 1
            ? '¿Está seguro de eliminar este proyecto?'
            : `¿Está seguro de eliminar ${count} proyectos?`;
        if (!confirm(message))
            return;
        try {
            const promises = [];
            $checked.each(function () {
                const recId = $(this).data('recid');
                if (recId) {
                    promises.push(deleteProject(recId));
                }
            });
            yield Promise.all(promises);
            alert('Proyecto(s) eliminado(s) correctamente');
            // Recargar tabla
            $table.DataTable().ajax.reload();
        }
        catch (error) {
            console.error('Error al eliminar:', error);
            alert('Error al eliminar proyecto(s)');
        }
    }));
    /**
     * Botón Exportar
     */
    $('#btn-export').on('click', exportToCSV);
    /**
     * Botón Importar
     */
    $('#btn-import').on('click', () => {
        alert('Funcionalidad de importación próximamente');
    });
    /**
     * Click en fila (toggle checkbox)
     */
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
    /**
     * Evento personalizado para recargar
     */
    d.addEventListener('projects:list:reload', () => {
        $table.DataTable().ajax.reload();
    });
    // ========================================================================
    // INICIALIZACIÓN
    // ========================================================================
    $(function () {
        return __awaiter(this, void 0, void 0, function* () {
            var _a;
            try {
                console.log('Inicializando lista de proyectos...');
                console.log('Token:', token ? '✓' : '✗');
                console.log('DataareaID:', dataareaId);
                // Hacer una petición de prueba para obtener estructura
                const probeUrl = `${apiBase}/Projects?pageNumber=1&pageSize=1`;
                const probe = yield fetchJson(probeUrl);
                let columns = [];
                if ((_a = probe === null || probe === void 0 ? void 0 : probe.Data) === null || _a === void 0 ? void 0 : _a.length) {
                    columns = getColumnsFromData(probe.Data[0]);
                }
                else {
                    // Columnas por defecto si no hay datos
                    columns = ['ID', 'ProjectCode', 'Name', 'LedgerAccount', 'ProjectStatus', 'CreatedOn'];
                }
                console.log('Columnas detectadas:', columns);
                // Inicializar DataTable con columnas dinámicas
                initializeDataTable(columns);
            }
            catch (error) {
                console.error('Error en inicialización:', error);
                showError('Error al cargar la configuración de la tabla');
            }
        });
    });
})();
//# sourceMappingURL=projects-dt.js.map