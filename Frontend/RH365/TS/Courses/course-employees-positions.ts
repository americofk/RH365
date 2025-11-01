// ============================================================================
// Archivo: course-employees-positions.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Courses/course-employees-positions.ts
// Descripción:
//   - Gestión de empleados y posiciones asignados a un Curso
//   - Validaciones y confirmaciones con sistema ALERTS
//   - CRUD completo con manejo de errores
// Estándar: ISO 27001 - Gestión de relaciones de datos
// ============================================================================

(function () {
    const w: any = window;
    const d: Document = document;
    const $: any = w.jQuery || w.$;

    const apiBase: string = w.RH365?.urls?.apiBase || '';
    const pageContainer = d.querySelector("#course-form-page");
    if (!pageContainer) return;

    const token: string = pageContainer.getAttribute("data-token") || "";
    const recId: number = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    const isNew: boolean = pageContainer.getAttribute("data-isnew") === "true";

    // Acceso a sistema de alertas
    const A = (w as any).ALERTS;
    const assertAlerts = () => {
        if (!A || typeof A.confirm !== 'function' || typeof A.ok !== 'function' || typeof A.error !== 'function') {
            throw new Error("Sistema ALERTS no disponible. Verifica la carga de scripts.");
        }
    };

    // Estado en memoria
    let allEmployees: any[] = [];
    let allPositions: any[] = [];
    let assignedEmployees: Array<{ EmployeeRecID: number; CourseEmployeeRecID: number }> = [];
    let assignedPositions: Array<{ PositionRecID: number; CoursePositionRecID: number }> = [];

    // ========================================================================
    // UTILIDADES - COMUNICACIÓN CON API
    // ========================================================================
    const fetchJson = async (url: string, options?: RequestInit): Promise<any> => {
        const headers: Record<string, string> = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        if (token) headers['Authorization'] = `Bearer ${token}`;

        const response = await fetch(url, { ...options, headers });

        if (!response.ok) {
            const errorText = await response.text().catch(() => '');
            throw new Error(errorText || `HTTP ${response.status}`);
        }

        if (response.status === 204) return null;

        const contentType = response.headers.get('content-type') || '';
        return contentType.includes('application/json') ? response.json() : null;
    };

    // ========================================================================
    // CARGA DE CATÁLOGOS
    // ========================================================================
    const loadAllEmployees = async (): Promise<void> => {
        try {
            const resp = await fetchJson(`${apiBase}/Employees?skip=0&take=5000`);
            allEmployees = Array.isArray(resp) ? resp : (resp?.Data || []);
        } catch (error) {
            A.error('Error al cargar catálogo de empleados', 'Error');
            throw error;
        }
    };

    const loadAllPositions = async (): Promise<void> => {
        try {
            const resp = await fetchJson(`${apiBase}/Positions?skip=0&take=5000`);
            allPositions = Array.isArray(resp) ? resp : (resp?.Data || []);
        } catch (error) {
            A.error('Error al cargar catálogo de posiciones', 'Error');
            throw error;
        }
    };

    // ========================================================================
    // RENDERIZADO - EMPLEADOS DISPONIBLES (MODAL)
    // ========================================================================
    const renderAvailableEmployees = (search: string = ''): void => {
        const $tbody = $('#tbody-available-employees');
        $tbody.empty();

        let rows = allEmployees.filter(e => !assignedEmployees.some(a => a.EmployeeRecID === e.RecID));

        if (search) {
            const s = search.toLowerCase();
            rows = rows.filter(e =>
                (e.EmployeeCode || '').toLowerCase().includes(s) ||
                (e.Name || '').toLowerCase().includes(s) ||
                (e.LastName || '').toLowerCase().includes(s)
            );
        }

        if (rows.length === 0) {
            $tbody.html('<tr><td colspan="3" class="text-center text-muted"><i class="fa fa-info-circle"></i> No hay empleados disponibles</td></tr>');
            return;
        }

        rows.forEach(e => {
            const name = `${e.Name || ''} ${e.LastName || ''}`.trim() || 'Sin nombre';
            $tbody.append(`
                <tr>
                  <td class="text-center"><input type="checkbox" class="flat employee-checkbox" data-recid="${e.RecID}"/></td>
                  <td>${e.EmployeeCode || 'N/A'}</td>
                  <td>${name}</td>
                </tr>`);
        });

        if ($.fn.iCheck) $('.employee-checkbox').iCheck({ checkboxClass: 'icheckbox_flat-green' });
    };

    // ========================================================================
    // RENDERIZADO - POSICIONES DISPONIBLES (MODAL)
    // ========================================================================
    const renderAvailablePositions = (search: string = ''): void => {
        const $tbody = $('#tbody-available-positions');
        $tbody.empty();

        let rows = allPositions.filter(p => !assignedPositions.some(a => a.PositionRecID === p.RecID));

        if (search) {
            const s = search.toLowerCase();
            rows = rows.filter(p =>
                (p.PositionCode || '').toLowerCase().includes(s) ||
                (p.PositionName || '').toLowerCase().includes(s)
            );
        }

        if (rows.length === 0) {
            $tbody.html('<tr><td colspan="3" class="text-center text-muted"><i class="fa fa-info-circle"></i> No hay posiciones disponibles</td></tr>');
            return;
        }

        rows.forEach(p => {
            $tbody.append(`
                <tr>
                  <td class="text-center"><input type="checkbox" class="flat position-checkbox" data-recid="${p.RecID}"/></td>
                  <td>${p.PositionCode || 'N/A'}</td>
                  <td>${p.PositionName || 'N/A'}</td>
                </tr>`);
        });

        if ($.fn.iCheck) $('.position-checkbox').iCheck({ checkboxClass: 'icheckbox_flat-green' });
    };

    // ========================================================================
    // RENDERIZADO - EMPLEADOS ASIGNADOS
    // ========================================================================
    const renderAssignedEmployees = (): void => {
        const $tbody = $('#tbody-course-employees');
        $tbody.empty();

        if (assignedEmployees.length === 0) {
            $tbody.html('<tr><td colspan="4" class="text-center text-muted"><i class="fa fa-info-circle"></i> No hay empleados asignados al curso</td></tr>');
            return;
        }

        assignedEmployees.forEach(item => {
            const e = allEmployees.find(x => x.RecID === item.EmployeeRecID);
            if (!e) return;

            const name = `${e.Name || ''} ${e.LastName || ''}`.trim() || 'Sin nombre';
            $tbody.append(`
                <tr>
                  <td class="text-center"><input type="checkbox" class="flat assigned-employee-checkbox" data-recid="${item.EmployeeRecID}"/></td>
                  <td>${e.EmployeeCode || 'N/A'}</td>
                  <td>${name}</td>
                  <td class="text-center">
                    <button type="button" class="btn btn-danger btn-xs btn-delete-employee"
                            data-recid="${item.EmployeeRecID}"
                            data-course-employee-recid="${item.CourseEmployeeRecID}"
                            title="Eliminar empleado">
                      <i class="fa fa-trash"></i>
                    </button>
                  </td>
                </tr>`);
        });

        if ($.fn.iCheck) $('.assigned-employee-checkbox').iCheck({ checkboxClass: 'icheckbox_flat-green' });
    };

    // ========================================================================
    // RENDERIZADO - POSICIONES ASIGNADAS
    // ========================================================================
    const renderAssignedPositions = (): void => {
        const $tbody = $('#tbody-course-positions');
        $tbody.empty();

        if (assignedPositions.length === 0) {
            $tbody.html('<tr><td colspan="4" class="text-center text-muted"><i class="fa fa-info-circle"></i> No hay posiciones asignadas al curso</td></tr>');
            return;
        }

        assignedPositions.forEach(item => {
            const p = allPositions.find(x => x.RecID === item.PositionRecID);
            if (!p) return;

            $tbody.append(`
                <tr>
                  <td class="text-center"><input type="checkbox" class="flat assigned-position-checkbox" data-recid="${item.PositionRecID}"/></td>
                  <td>${p.PositionCode || 'N/A'}</td>
                  <td>${p.PositionName || 'N/A'}</td>
                  <td class="text-center">
                    <button type="button" class="btn btn-danger btn-xs btn-delete-position"
                            data-recid="${item.PositionRecID}"
                            data-course-position-recid="${item.CoursePositionRecID}"
                            title="Eliminar posición">
                      <i class="fa fa-trash"></i>
                    </button>
                  </td>
                </tr>`);
        });

        if ($.fn.iCheck) $('.assigned-position-checkbox').iCheck({ checkboxClass: 'icheckbox_flat-green' });
    };

    // ========================================================================
    // CARGA DE DATOS ASIGNADOS
    // ========================================================================
    const loadAssignedData = async (): Promise<void> => {
        if (isNew || recId <= 0) {
            renderAssignedEmployees();
            renderAssignedPositions();
            return;
        }

        try {
            const [empResp, posResp] = await Promise.all([
                fetchJson(`${apiBase}/CourseEmployees`),
                fetchJson(`${apiBase}/CoursePositions`)
            ]);

            const empData = Array.isArray(empResp) ? empResp : (empResp?.Data || []);
            const posData = Array.isArray(posResp) ? posResp : (posResp?.Data || []);

            assignedEmployees = empData
                .filter((e: any) => e.CourseRefRecID === recId)
                .map((e: any) => ({ EmployeeRecID: e.EmployeeRefRecID, CourseEmployeeRecID: e.RecID }));

            assignedPositions = posData
                .filter((p: any) => p.CourseRefRecID === recId)
                .map((p: any) => ({ PositionRecID: p.PositionRefRecID, CoursePositionRecID: p.RecID }));

            renderAssignedEmployees();
            renderAssignedPositions();
        } catch (error) {
            A.error('Error al cargar los datos asignados al curso', 'Error');
            throw error;
        }
    };

    // ========================================================================
    // EVENTOS - MODALES
    // ========================================================================
    $('#modal-add-employee').on('show.bs.modal', () => renderAvailableEmployees());
    $('#modal-add-position').on('show.bs.modal', () => renderAvailablePositions());

    $('#btn-search-employee, #employee-search').on('keyup change', function (e: any) {
        if (e.type === 'keyup' && e.keyCode !== 13) return;
        renderAvailableEmployees(String($('#employee-search').val() || ''));
    });

    $('#btn-search-position, #position-search').on('keyup change', function (e: any) {
        if (e.type === 'keyup' && e.keyCode !== 13) return;
        renderAvailablePositions(String($('#position-search').val() || ''));
    });

    // ========================================================================
    // AGREGAR EMPLEADOS SELECCIONADOS
    // ========================================================================
    $('#btn-confirm-add-employees').on('click', async function () {
        assertAlerts();

        const selected: number[] = [];
        $('#tbody-available-employees input.employee-checkbox:checked').each(function () {
            selected.push($(this).data('recid'));
        });

        if (selected.length === 0) {
            A.warn('Debe seleccionar al menos un empleado', 'Advertencia');
            return;
        }

        try {
            for (const empRecId of selected) {
                const resp = await fetchJson(`${apiBase}/CourseEmployees`, {
                    method: 'POST',
                    body: JSON.stringify({
                        CourseRefRecID: recId,
                        EmployeeRefRecID: empRecId,
                        Comment: '',
                        Observations: ''
                    })
                });
                assignedEmployees.push({ EmployeeRecID: empRecId, CourseEmployeeRecID: resp.RecID });
            }

            renderAssignedEmployees();
            $('#modal-add-employee').modal('hide');
            A.ok(`Se ${selected.length === 1 ? 'agregó' : 'agregaron'} ${selected.length} empleado${selected.length > 1 ? 's' : ''} al curso`, 'Éxito');
        } catch (error) {
            A.error('Error al agregar empleados al curso', 'Error');
        }
    });

    // ========================================================================
    // AGREGAR POSICIONES SELECCIONADAS
    // ========================================================================
    $('#btn-confirm-add-positions').on('click', async function () {
        assertAlerts();

        const selected: number[] = [];
        $('#tbody-available-positions input.position-checkbox:checked').each(function () {
            selected.push($(this).data('recid'));
        });

        if (selected.length === 0) {
            A.warn('Debe seleccionar al menos una posición', 'Advertencia');
            return;
        }

        try {
            for (const posRecId of selected) {
                const resp = await fetchJson(`${apiBase}/CoursePositions`, {
                    method: 'POST',
                    body: JSON.stringify({
                        CourseRefRecID: recId,
                        PositionRefRecID: posRecId,
                        Comment: '',
                        Observations: ''
                    })
                });
                assignedPositions.push({ PositionRecID: posRecId, CoursePositionRecID: resp.RecID });
            }

            renderAssignedPositions();
            $('#modal-add-position').modal('hide');
            A.ok(`Se ${selected.length === 1 ? 'agregó' : 'agregaron'} ${selected.length} posición${selected.length > 1 ? 'es' : ''} al curso`, 'Éxito');
        } catch (error) {
            A.error('Error al agregar posiciones al curso', 'Error');
        }
    });

    // ========================================================================
    // ELIMINAR EMPLEADO INDIVIDUAL
    // ========================================================================
    $(d).on('click', '.btn-delete-employee', function () {
        assertAlerts();

        const empRecId = $(this).data('recid');
        const courseEmpRecId = $(this).data('course-employee-recid');

        (w as any).ALERTS.confirm(
            '¿Está seguro de eliminar este empleado del curso?',
            'Confirmar Eliminación',
            async (confirmed: boolean) => {
                if (!confirmed) return;

                try {
                    await fetchJson(`${apiBase}/CourseEmployees/${courseEmpRecId}`, { method: 'DELETE' });
                    assignedEmployees = assignedEmployees.filter(e => e.EmployeeRecID !== empRecId);
                    renderAssignedEmployees();
                    A.ok('El empleado ha sido eliminado del curso', 'Éxito');
                } catch (error) {
                    A.error('Error al eliminar el empleado del curso', 'Error');
                }
            },
            { type: 'danger' }
        );
    });

    // ========================================================================
    // ELIMINAR POSICIÓN INDIVIDUAL
    // ========================================================================
    $(d).on('click', '.btn-delete-position', function () {
        assertAlerts();

        const posRecId = $(this).data('recid');
        const coursePosRecId = $(this).data('course-position-recid');

        (w as any).ALERTS.confirm(
            '¿Está seguro de eliminar esta posición del curso?',
            'Confirmar Eliminación',
            async (confirmed: boolean) => {
                if (!confirmed) return;

                try {
                    await fetchJson(`${apiBase}/CoursePositions/${coursePosRecId}`, { method: 'DELETE' });
                    assignedPositions = assignedPositions.filter(p => p.PositionRecID !== posRecId);
                    renderAssignedPositions();
                    A.ok('La posición ha sido eliminada del curso', 'Éxito');
                } catch (error) {
                    A.error('Error al eliminar la posición del curso', 'Error');
                }
            },
            { type: 'danger' }
        );
    });

    // ========================================================================
    // ELIMINAR EMPLEADOS MÚLTIPLES
    // ========================================================================
    $('#btn-delete-selected-employees').on('click', function () {
        assertAlerts();

        const toDelete: { EmployeeRecID: number; CourseEmployeeRecID: number }[] = [];
        $('#tbody-course-employees input.assigned-employee-checkbox:checked').each(function () {
            const found = assignedEmployees.find(e => e.EmployeeRecID === $(this).data('recid'));
            if (found) toDelete.push(found);
        });

        if (toDelete.length === 0) return;

        const message = toDelete.length === 1
            ? '¿Está seguro de eliminar este empleado del curso?'
            : `¿Está seguro de eliminar los ${toDelete.length} empleados seleccionados del curso?`;

        (w as any).ALERTS.confirm(
            message,
            'Confirmar Eliminación',
            async (confirmed: boolean) => {
                if (!confirmed) return;

                try {
                    for (const item of toDelete) {
                        await fetchJson(`${apiBase}/CourseEmployees/${item.CourseEmployeeRecID}`, { method: 'DELETE' });
                    }
                    assignedEmployees = assignedEmployees.filter(e => !toDelete.some(d => d.EmployeeRecID === e.EmployeeRecID));
                    renderAssignedEmployees();
                    A.ok(`Se ${toDelete.length === 1 ? 'eliminó' : 'eliminaron'} ${toDelete.length} empleado${toDelete.length > 1 ? 's' : ''} del curso`, 'Éxito');
                } catch (error) {
                    A.error('Error al eliminar los empleados del curso', 'Error');
                }
            },
            { type: 'danger' }
        );
    });

    // ========================================================================
    // ELIMINAR POSICIONES MÚLTIPLES
    // ========================================================================
    $('#btn-delete-selected-positions').on('click', function () {
        assertAlerts();

        const toDelete: { PositionRecID: number; CoursePositionRecID: number }[] = [];
        $('#tbody-course-positions input.assigned-position-checkbox:checked').each(function () {
            const found = assignedPositions.find(p => p.PositionRecID === $(this).data('recid'));
            if (found) toDelete.push(found);
        });

        if (toDelete.length === 0) return;

        const message = toDelete.length === 1
            ? '¿Está seguro de eliminar esta posición del curso?'
            : `¿Está seguro de eliminar las ${toDelete.length} posiciones seleccionadas del curso?`;

        (w as any).ALERTS.confirm(
            message,
            'Confirmar Eliminación',
            async (confirmed: boolean) => {
                if (!confirmed) return;

                try {
                    for (const item of toDelete) {
                        await fetchJson(`${apiBase}/CoursePositions/${item.CoursePositionRecID}`, { method: 'DELETE' });
                    }
                    assignedPositions = assignedPositions.filter(p => !toDelete.some(d => d.PositionRecID === p.PositionRecID));
                    renderAssignedPositions();
                    A.ok(`Se ${toDelete.length === 1 ? 'eliminó' : 'eliminaron'} ${toDelete.length} posición${toDelete.length > 1 ? 'es' : ''} del curso`, 'Éxito');
                } catch (error) {
                    A.error('Error al eliminar las posiciones del curso', 'Error');
                }
            },
            { type: 'danger' }
        );
    });

    // ========================================================================
    // EVENTOS - ICHECK TOGGLES
    // ========================================================================
    $(d).on('ifChanged', '.assigned-employee-checkbox', function () {
        $('#btn-delete-selected-employees').prop('disabled',
            $('#tbody-course-employees input.assigned-employee-checkbox:checked').length === 0);
    });

    $(d).on('ifChanged', '.assigned-position-checkbox', function () {
        $('#btn-delete-selected-positions').prop('disabled',
            $('#tbody-course-positions input.assigned-position-checkbox:checked').length === 0);
    });

    $(d).on('ifChanged', '#check-all-modal-employees', function () {
        $('.employee-checkbox').iCheck($(this).is(':checked') ? 'check' : 'uncheck');
    });

    $(d).on('ifChanged', '#check-all-modal-positions', function () {
        $('.position-checkbox').iCheck($(this).is(':checked') ? 'check' : 'uncheck');
    });

    $(d).on('ifChanged', '#check-all-employees', function () {
        $('.assigned-employee-checkbox').iCheck($(this).is(':checked') ? 'check' : 'uncheck');
    });

    $(d).on('ifChanged', '#check-all-positions', function () {
        $('.assigned-position-checkbox').iCheck($(this).is(':checked') ? 'check' : 'uncheck');
    });

    // ========================================================================
    // INICIALIZACIÓN
    // ========================================================================
    $(async function () {
        try {
            await Promise.all([loadAllEmployees(), loadAllPositions()]);
            await loadAssignedData();
        } catch (error) {
            A.error('Error al inicializar el módulo de empleados y posiciones', 'Error de Inicialización');
        }
    });
})();