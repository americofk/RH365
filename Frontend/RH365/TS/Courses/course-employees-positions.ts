// ============================================================================
// Archivo: course-employees-positions.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Courses/course-employees-positions.ts
// Descripción:
//   - Gestión de empleados y posiciones asignados a un curso
//   - Modales para selección múltiple
//   - Integración con APIs de Employees y Positions
//   - Manejo de relaciones CourseEmployees y CoursePositions
// Estándar: ISO 27001 - Gestión de relaciones de datos
// ============================================================================

(function () {
    const w: any = window;
    const d: Document = document;
    const $: any = w.jQuery || w.$;

    const apiBase: string = w.RH365.urls.apiBase;
    const pageContainer = d.querySelector("#course-form-page");

    if (!pageContainer) return;

    const token: string = pageContainer.getAttribute("data-token") || "";
    const recId: number = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    const isNew: boolean = pageContainer.getAttribute("data-isnew") === "true";

    // ========================================================================
    // VARIABLES GLOBALES
    // ========================================================================
    let availableEmployees: any[] = [];
    let availablePositions: any[] = [];
    let assignedEmployees: any[] = [];
    let assignedPositions: any[] = [];

    // ========================================================================
    // UTILIDADES - COMUNICACIÓN CON API
    // ========================================================================
    const fetchJson = async (url: string, options?: RequestInit): Promise<any> => {
        const headers: Record<string, string> = {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        };

        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        const response = await fetch(url, { ...options, headers });

        if (!response.ok) {
            const errorData = await response.json().catch(() => ({}));
            throw new Error(JSON.stringify(errorData));
        }

        return response.json();
    };

    // ========================================================================
    // CARGA DE EMPLEADOS DISPONIBLES
    // ========================================================================
    const loadAvailableEmployees = async (): Promise<void> => {
        try {
            const url = `${apiBase}/Employees?skip=0&take=1000`;
            const response = await fetchJson(url);

            if (Array.isArray(response)) {
                availableEmployees = response;
            } else if (response?.Data && Array.isArray(response.Data)) {
                availableEmployees = response.Data;
            } else if (response?.data && Array.isArray(response.data)) {
                availableEmployees = response.data;
            }

            renderAvailableEmployees();
        } catch (error) {
            console.error('Error cargando empleados:', error);
            $('#tbody-available-employees').html(`
                <tr>
                    <td colspan="4" class="text-center text-danger">
                        <i class="fa fa-exclamation-triangle"></i> Error al cargar empleados
                    </td>
                </tr>
            `);
        }
    };

    // ========================================================================
    // CARGA DE POSICIONES DISPONIBLES
    // ========================================================================
    const loadAvailablePositions = async (): Promise<void> => {
        try {
            const url = `${apiBase}/Positions?skip=0&take=1000`;
            const response = await fetchJson(url);

            if (Array.isArray(response)) {
                availablePositions = response;
            } else if (response?.Data && Array.isArray(response.Data)) {
                availablePositions = response.Data;
            } else if (response?.data && Array.isArray(response.data)) {
                availablePositions = response.data;
            }

            renderAvailablePositions();
        } catch (error) {
            console.error('Error cargando posiciones:', error);
            $('#tbody-available-positions').html(`
                <tr>
                    <td colspan="4" class="text-center text-danger">
                        <i class="fa fa-exclamation-triangle"></i> Error al cargar posiciones
                    </td>
                </tr>
            `);
        }
    };

    // ========================================================================
    // RENDERIZADO DE EMPLEADOS DISPONIBLES EN MODAL
    // ========================================================================
    const renderAvailableEmployees = (searchTerm: string = ''): void => {
        const tbody = $('#tbody-available-employees');
        tbody.empty();

        let filtered = availableEmployees;

        // Filtrar por término de búsqueda
        if (searchTerm) {
            const term = searchTerm.toLowerCase();
            filtered = availableEmployees.filter(emp =>
                (emp.EmployeeCode || '').toLowerCase().includes(term) ||
                (emp.FirstName || '').toLowerCase().includes(term) ||
                (emp.LastName || '').toLowerCase().includes(term)
            );
        }

        // Excluir empleados ya asignados
        filtered = filtered.filter(emp =>
            !assignedEmployees.some(assigned => assigned.RecID === emp.RecID)
        );

        if (filtered.length === 0) {
            tbody.html(`
                <tr>
                    <td colspan="4" class="text-center text-muted">
                        <i class="fa fa-info-circle"></i> No hay empleados disponibles
                    </td>
                </tr>
            `);
            return;
        }

        filtered.forEach(emp => {
            const fullName = `${emp.FirstName || ''} ${emp.LastName || ''}`.trim();
            const row = $(`
                <tr>
                    <td class="text-center">
                        <input type="checkbox" class="flat employee-checkbox" data-recid="${emp.RecID}" 
                               data-code="${emp.EmployeeCode || ''}" 
                               data-name="${fullName}" />
                    </td>
                    <td>${emp.EmployeeCode || 'N/A'}</td>
                    <td>${fullName}</td>
                    <td>${emp.DepartmentName || 'N/A'}</td>
                </tr>
            `);
            tbody.append(row);
        });

        // Inicializar iCheck
        if ($.fn.iCheck) {
            $('.employee-checkbox').iCheck({
                checkboxClass: 'icheckbox_flat-green'
            });
        }
    };

    // ========================================================================
    // RENDERIZADO DE POSICIONES DISPONIBLES EN MODAL
    // ========================================================================
    const renderAvailablePositions = (searchTerm: string = ''): void => {
        const tbody = $('#tbody-available-positions');
        tbody.empty();

        let filtered = availablePositions;

        // Filtrar por término de búsqueda
        if (searchTerm) {
            const term = searchTerm.toLowerCase();
            filtered = availablePositions.filter(pos =>
                (pos.PositionCode || '').toLowerCase().includes(term) ||
                (pos.Name || '').toLowerCase().includes(term)
            );
        }

        // Excluir posiciones ya asignadas
        filtered = filtered.filter(pos =>
            !assignedPositions.some(assigned => assigned.RecID === pos.RecID)
        );

        if (filtered.length === 0) {
            tbody.html(`
                <tr>
                    <td colspan="4" class="text-center text-muted">
                        <i class="fa fa-info-circle"></i> No hay posiciones disponibles
                    </td>
                </tr>
            `);
            return;
        }

        filtered.forEach(pos => {
            const row = $(`
                <tr>
                    <td class="text-center">
                        <input type="checkbox" class="flat position-checkbox" data-recid="${pos.RecID}" 
                               data-code="${pos.PositionCode || ''}" 
                               data-name="${pos.Name || ''}" />
                    </td>
                    <td>${pos.PositionCode || 'N/A'}</td>
                    <td>${pos.Name || 'N/A'}</td>
                    <td>${pos.Description || 'N/A'}</td>
                </tr>
            `);
            tbody.append(row);
        });

        // Inicializar iCheck
        if ($.fn.iCheck) {
            $('.position-checkbox').iCheck({
                checkboxClass: 'icheckbox_flat-green'
            });
        }
    };

    // ========================================================================
    // RENDERIZADO DE EMPLEADOS ASIGNADOS
    // ========================================================================
    const renderAssignedEmployees = (): void => {
        const tbody = $('#tbody-course-employees');
        tbody.empty();

        if (assignedEmployees.length === 0) {
            tbody.html(`
                <tr>
                    <td colspan="4" class="text-center text-muted">
                        <i class="fa fa-info-circle"></i> No hay empleados asignados
                    </td>
                </tr>
            `);
            $('#btn-delete-selected-employees').prop('disabled', true);
            return;
        }

        assignedEmployees.forEach(emp => {
            const row = $(`
                <tr>
                    <td class="text-center">
                        <input type="checkbox" class="flat assigned-employee-checkbox" data-recid="${emp.RecID}" />
                    </td>
                    <td>${emp.Code}</td>
                    <td>${emp.Name}</td>
                    <td class="text-center">
                        <button type="button" class="btn btn-danger btn-xs btn-delete-employee" data-recid="${emp.RecID}">
                            <i class="fa fa-trash"></i>
                        </button>
                    </td>
                </tr>
            `);
            tbody.append(row);
        });

        // Inicializar iCheck
        if ($.fn.iCheck) {
            $('.assigned-employee-checkbox').iCheck({
                checkboxClass: 'icheckbox_flat-green'
            });
        }
    };

    // ========================================================================
    // RENDERIZADO DE POSICIONES ASIGNADAS
    // ========================================================================
    const renderAssignedPositions = (): void => {
        const tbody = $('#tbody-course-positions');
        tbody.empty();

        if (assignedPositions.length === 0) {
            tbody.html(`
                <tr>
                    <td colspan="4" class="text-center text-muted">
                        <i class="fa fa-info-circle"></i> No hay posiciones asignadas
                    </td>
                </tr>
            `);
            $('#btn-delete-selected-positions').prop('disabled', true);
            return;
        }

        assignedPositions.forEach(pos => {
            const row = $(`
                <tr>
                    <td class="text-center">
                        <input type="checkbox" class="flat assigned-position-checkbox" data-recid="${pos.RecID}" />
                    </td>
                    <td>${pos.Code}</td>
                    <td>${pos.Name}</td>
                    <td class="text-center">
                        <button type="button" class="btn btn-danger btn-xs btn-delete-position" data-recid="${pos.RecID}">
                            <i class="fa fa-trash"></i>
                        </button>
                    </td>
                </tr>
            `);
            tbody.append(row);
        });

        // Inicializar iCheck
        if ($.fn.iCheck) {
            $('.assigned-position-checkbox').iCheck({
                checkboxClass: 'icheckbox_flat-green'
            });
        }
    };

    // ========================================================================
    // CARGAR EMPLEADOS Y POSICIONES ASIGNADOS (SI ES EDICIÓN)
    // ========================================================================
    const loadAssignedData = async (): Promise<void> => {
        if (isNew || recId === 0) {
            renderAssignedEmployees();
            renderAssignedPositions();
            return;
        }

        try {
            // Cargar empleados asignados
            const employeesUrl = `${apiBase}/CourseEmployees/ByCourse/${recId}`;
            const employeesResponse = await fetchJson(employeesUrl);

            if (Array.isArray(employeesResponse)) {
                assignedEmployees = employeesResponse.map(e => ({
                    RecID: e.EmployeeRefRecID,
                    Code: e.EmployeeCode || 'N/A',
                    Name: e.EmployeeName || 'N/A',
                    CourseEmployeeRecID: e.RecID
                }));
            }

            // Cargar posiciones asignadas
            const positionsUrl = `${apiBase}/CoursePositions/ByCourse/${recId}`;
            const positionsResponse = await fetchJson(positionsUrl);

            if (Array.isArray(positionsResponse)) {
                assignedPositions = positionsResponse.map(p => ({
                    RecID: p.PositionRefRecID,
                    Code: p.PositionCode || 'N/A',
                    Name: p.PositionName || 'N/A',
                    CoursePositionRecID: p.RecID
                }));
            }

            renderAssignedEmployees();
            renderAssignedPositions();
        } catch (error) {
            console.error('Error cargando datos asignados:', error);
        }
    };

    // ========================================================================
    // EVENT HANDLERS
    // ========================================================================

    // Modal de empleados - Al abrirse
    $('#modal-add-employee').on('show.bs.modal', async function () {
        await loadAvailableEmployees();
    });

    // Modal de posiciones - Al abrirse
    $('#modal-add-position').on('show.bs.modal', async function () {
        await loadAvailablePositions();
    });

    // Búsqueda de empleados
    $('#btn-search-employee, #employee-search').on('keyup change', function (e: any) {
        if (e.type === 'keyup' && e.keyCode !== 13) return;
        const searchTerm = $('#employee-search').val() as string;
        renderAvailableEmployees(searchTerm);
    });

    // Búsqueda de posiciones
    $('#btn-search-position, #position-search').on('keyup change', function (e: any) {
        if (e.type === 'keyup' && e.keyCode !== 13) return;
        const searchTerm = $('#position-search').val() as string;
        renderAvailablePositions(searchTerm);
    });

    // Agregar empleados seleccionados
    $('#btn-confirm-add-employees').on('click', function () {
        const selected: any[] = [];

        $('#tbody-available-employees input.employee-checkbox:checked').each(function () {
            const $checkbox = $(this);
            selected.push({
                RecID: $checkbox.data('recid'),
                Code: $checkbox.data('code'),
                Name: $checkbox.data('name')
            });
        });

        if (selected.length === 0) {
            (w as any).ALERTS.warn('Seleccione al menos un empleado', 'Advertencia');
            return;
        }

        assignedEmployees.push(...selected);
        renderAssignedEmployees();

        $('#modal-add-employee').modal('hide');
        (w as any).ALERTS.ok(`${selected.length} empleado(s) agregado(s)`, 'Éxito');
    });

    // Agregar posiciones seleccionadas
    $('#btn-confirm-add-positions').on('click', function () {
        const selected: any[] = [];

        $('#tbody-available-positions input.position-checkbox:checked').each(function () {
            const $checkbox = $(this);
            selected.push({
                RecID: $checkbox.data('recid'),
                Code: $checkbox.data('code'),
                Name: $checkbox.data('name')
            });
        });

        if (selected.length === 0) {
            (w as any).ALERTS.warn('Seleccione al menos una posición', 'Advertencia');
            return;
        }

        assignedPositions.push(...selected);
        renderAssignedPositions();

        $('#modal-add-position').modal('hide');
        (w as any).ALERTS.ok(`${selected.length} posición(es) agregada(s)`, 'Éxito');
    });

    // Eliminar empleado individual
    $(document).on('click', '.btn-delete-employee', function () {
        const recId = $(this).data('recid');
        assignedEmployees = assignedEmployees.filter(e => e.RecID !== recId);
        renderAssignedEmployees();
    });

    // Eliminar posición individual
    $(document).on('click', '.btn-delete-position', function () {
        const recId = $(this).data('recid');
        assignedPositions = assignedPositions.filter(p => p.RecID !== recId);
        renderAssignedPositions();
    });

    // Eliminar empleados seleccionados
    $('#btn-delete-selected-employees').on('click', function () {
        const toDelete: number[] = [];

        $('#tbody-course-employees input.assigned-employee-checkbox:checked').each(function () {
            toDelete.push($(this).data('recid'));
        });

        if (toDelete.length === 0) return;

        assignedEmployees = assignedEmployees.filter(e => !toDelete.includes(e.RecID));
        renderAssignedEmployees();

        (w as any).ALERTS.ok(`${toDelete.length} empleado(s) eliminado(s)`, 'Éxito');
    });

    // Eliminar posiciones seleccionadas
    $('#btn-delete-selected-positions').on('click', function () {
        const toDelete: number[] = [];

        $('#tbody-course-positions input.assigned-position-checkbox:checked').each(function () {
            toDelete.push($(this).data('recid'));
        });

        if (toDelete.length === 0) return;

        assignedPositions = assignedPositions.filter(p => !toDelete.includes(p.RecID));
        renderAssignedPositions();

        (w as any).ALERTS.ok(`${toDelete.length} posición(es) eliminada(s)`, 'Éxito');
    });

    // Habilitar/deshabilitar botones de eliminación
    $(document).on('ifChanged', '.assigned-employee-checkbox', function () {
        const checkedCount = $('#tbody-course-employees input.assigned-employee-checkbox:checked').length;
        $('#btn-delete-selected-employees').prop('disabled', checkedCount === 0);
    });

    $(document).on('ifChanged', '.assigned-position-checkbox', function () {
        const checkedCount = $('#tbody-course-positions input.assigned-position-checkbox:checked').length;
        $('#btn-delete-selected-positions').prop('disabled', checkedCount === 0);
    });

    // Check all en modales
    $(document).on('ifChanged', '#check-all-modal-employees', function () {
        const isChecked = $(this).is(':checked');
        $('.employee-checkbox').iCheck(isChecked ? 'check' : 'uncheck');
    });

    $(document).on('ifChanged', '#check-all-modal-positions', function () {
        const isChecked = $(this).is(':checked');
        $('.position-checkbox').iCheck(isChecked ? 'check' : 'uncheck');
    });

    // Check all en tablas asignadas
    $(document).on('ifChanged', '#check-all-employees', function () {
        const isChecked = $(this).is(':checked');
        $('.assigned-employee-checkbox').iCheck(isChecked ? 'check' : 'uncheck');
    });

    $(document).on('ifChanged', '#check-all-positions', function () {
        const isChecked = $(this).is(':checked');
        $('.assigned-position-checkbox').iCheck(isChecked ? 'check' : 'uncheck');
    });

    // ========================================================================
    // FUNCIÓN PÚBLICA PARA OBTENER DATOS
    // ========================================================================
    (w as any).CourseEmployeesPositions = {
        getAssignedEmployees: () => assignedEmployees,
        getAssignedPositions: () => assignedPositions
    };

    // ========================================================================
    // INICIALIZACIÓN
    // ========================================================================
    $(async function () {
        try {
            await loadAssignedData();
        } catch (error) {
            console.error('Error en inicialización de empleados/posiciones:', error);
        }
    });
})();