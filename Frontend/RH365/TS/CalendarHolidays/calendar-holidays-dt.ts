// ============================================================================
// Archivo: calendar-holidays-dt.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/CalendarHolidays/calendar-holidays-dt.ts
// Descripción:
//   - Lista de Días Feriados con DataTables
//   - Gestión de vistas de usuario (UserGridViews)
//   - Genera columnas y filas dinámicamente desde API
// ============================================================================

type CalendarHolidayRow = Record<string, unknown>;

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

    const apiBase: string = w.RH365.urls.apiBase;
    const pageContainer = d.querySelector("#calendar-holidays-page");

    if (!pageContainer) return;

    const token: string = pageContainer.getAttribute("data-token") || "";
    const dataareaId: string = pageContainer.getAttribute("data-dataarea") || "DAT";
    const userRefRecID: number = parseInt(pageContainer.getAttribute("data-user") || "0", 10);

    const $table: any = $("#calendar-holidays-table");
    if (!$table.length) return;

    let calendarHolidaysData: CalendarHolidayRow[] = [];
    let allColumns: string[] = [];
    let visibleColumns: string[] = [];
    const defaultColumns: string[] = ['ID', 'CalendarDate', 'Description', 'Observations', 'CreatedOn'];

    let gridViewsManager: any;
    let gridColumnsManager: any;

    const titleize = (field: string): string => {
        const translations: Record<string, string> = {
            'RecID': 'ID Registro',
            'ID': 'ID',
            'CalendarDate': 'Fecha',
            'Description': 'Descripción',
            'Observations': 'Observaciones',
            'CreatedOn': 'Fecha Creación',
            'CreatedBy': 'Creado Por',
            'ModifiedOn': 'Modificado',
            'ModifiedBy': 'Modificado Por'
        };
        return translations[field] || field.replace(/([a-z])([A-Z])/g, "$1 $2").replace(/_/g, " ").replace(/^./, (c) => c.toUpperCase());
    };

    const formatCell = (value: unknown, field: string): string => {
        if (value == null) return "";
        if (typeof value === "boolean") {
            return value ? "Sí" : "No";
        }
        if (typeof value === "string" && /^\d{4}-\d{2}-\d{2}T/.test(value)) {
            const dt = new Date(value);
            if (!isNaN(dt.getTime())) {
                return dt.toLocaleDateString('es-DO', { day: '2-digit', month: '2-digit', year: 'numeric' });
            }
        }
        return String(value);
    };

    const fetchJson = async (url: string): Promise<any> => {
        const headers: Record<string, string> = {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        };
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }
        
        console.log('🌐 Fetch URL:', url);
        console.log('🔑 Token presente?:', !!token);
        
        const response = await fetch(url, { headers });
        
        console.log('📡 Status:', response.status, response.statusText);
        
        if (!response.ok) {
            const errorText = await response.text();
            console.error('❌ Error del servidor:', errorText);
            throw new Error(`HTTP ${response.status} @ ${url}`);
        }
        
        const json = await response.json();
        console.log('✅ JSON parseado exitosamente');
        return json;
    };

    const getColumnsFromData = (sample: CalendarHolidayRow): string[] => {
        if (!sample || typeof sample !== 'object') {
            return [...defaultColumns];
        }
        const excluded = new Set(['RecID', 'RowVersion', 'DataareaID']);
        return Object.keys(sample).filter(col => !excluded.has(col));
    };

    const initializeDataTable = (columns: string[]): void => {
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
        const dtConfig: any = {
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
                    render: (_data: any, _type: string, row: CalendarHolidayRow) => {
                        return `<input type="checkbox" class="flat row-check" data-recid="${row.RecID || ''}"/>`;
                    }
                },
                ...columns.map(col => ({ data: col, name: col, render: (data: unknown) => formatCell(data, col) }))
            ],
            ajax: (_data: any, callback: (result: { data: CalendarHolidayRow[] }) => void) => {
                loadCalendarHolidays().then(items => {
                    calendarHolidaysData = items;
                    callback({ data: items });
                    updateSummary(items.length);
                }).catch(err => {
                    console.error('Error cargando días feriados:', err);
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

    const loadCalendarHolidays = async (): Promise<CalendarHolidayRow[]> => {
        const url = `${apiBase}/CalendarHolidays?skip=0&take=100`;
        console.log('🔍 Cargando desde:', url);
        
        const response: any = await fetchJson(url);
        
        console.log('📦 Respuesta recibida:', response);
        console.log('📊 Tipo:', typeof response);
        console.log('📊 Es array?:', Array.isArray(response));
        console.log('📊 Length:', Array.isArray(response) ? response.length : 'N/A');
        
        // El API devuelve un array directo
        if (Array.isArray(response)) {
            console.log('✅ Array válido con', response.length, 'elementos');
            return response;
        }
        
        console.error('❌ Respuesta inválida (no es array):', response);
        throw new Error('Respuesta del API inválida: se esperaba un array');
    };

    const updateSummary = (count: number): void => {
        const summary = d.getElementById('calendar-holidays-summary');
        if (summary) {
            summary.textContent = `${count} día${count !== 1 ? 's feriados' : ' feriado'}`;
        }
    };

    const showError = (message: string): void => {
        const errorHtml = `
            <tr>
                <td colspan="10" class="text-center text-danger">
                    <i class="fa fa-exclamation-triangle"></i> ${message}
                </td>
            </tr>
        `;
        $table.find('tbody').html(errorHtml);
    };

    const updateButtonStates = (): void => {
        const checkedCount = $table.find('tbody input.row-check:checked').length;
        $('#btn-edit').prop('disabled', checkedCount !== 1);
        $('#btn-delete').prop('disabled', checkedCount === 0);
    };

    const exportToCSV = (): void => {
        if (!calendarHolidaysData.length) {
            (w as any).ALERTS.warn('No hay datos para exportar', 'Advertencia');
            return;
        }
        const dt: any = $table.DataTable();
        const rows: CalendarHolidayRow[] = dt.rows({ search: 'applied' }).data().toArray();
        if (!rows.length) {
            (w as any).ALERTS.warn('No hay datos visibles para exportar', 'Advertencia');
            return;
        }
        const columns = visibleColumns;
        const csvLines: string[] = [columns.map(col => titleize(col)).join(',')];
        rows.forEach(row => {
            const line = columns.map(col => {
                const value = (row as any)[col];
                const str = value == null ? '' : String(value).replace(/"/g, '""');
                return /[",\n]/.test(str) ? `"${str}"` : str;
            }).join(',');
            csvLines.push(line);
        });
        const blob = new Blob([csvLines.join('\n')], { type: 'text/csv;charset=utf-8;' });
        const url = URL.createObjectURL(blob);
        const link = d.createElement('a');
        const fileName = `DiasFeriados_${new Date().toISOString().slice(0, 10)}.csv`;
        link.href = url;
        link.download = fileName;
        link.style.visibility = 'hidden';
        d.body.appendChild(link);
        link.click();
        d.body.removeChild(link);
        URL.revokeObjectURL(url);
    };

    const deleteCalendarHoliday = async (recId: string): Promise<void> => {
        const url = `${apiBase}/CalendarHolidays/${recId}`;
        const headers: Record<string, string> = { 'Authorization': `Bearer ${token}` };
        const response = await fetch(url, { method: 'DELETE', headers });
        if (!response.ok) {
            throw new Error(`Error al eliminar día feriado: ${response.status}`);
        }
    };

    const applyColumnChanges = (newColumns: string[]): void => {
        visibleColumns = newColumns;
        initializeDataTable(newColumns);
        if (gridViewsManager && gridViewsManager.hasCurrentView()) {
            $('#btn-save-view-changes').show();
        }
    };

    const updateViewsDropdown = (): void => {
        const views = gridViewsManager.getAvailableViews();
        const container = $('#saved-views-container');
        container.empty();

        if (views.length === 0) {
            container.append('<li class="text-muted" style="padding: 5px 20px;">No hay vistas guardadas</li>');
            return;
        }

        views.forEach((view: any) => {
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

            item.find('a[data-view-id]').first().on('click', async function (e) {
                e.preventDefault();
                await loadViewById(view.RecID);
            });

            item.find('.btn-set-default').on('click', async function (e) {
                e.preventDefault();
                e.stopPropagation();

                (w as any).ALERTS.confirm(
                    `¿Establecer "${view.ViewName}" como vista predeterminada?`,
                    'Confirmar',
                    async (confirmed: boolean) => {
                        if (confirmed) {
                            const success = await gridViewsManager.setDefaultView(view.RecID);
                            if (success) {
                                updateViewsDropdown();
                            }
                        }
                    }
                );
            });

            container.append(item);
        });
    };

    const loadViewById = async (viewId: number): Promise<void> => {
        try {
            const columnConfigs: ColumnConfig[] = await gridViewsManager.loadView(viewId);
            if (columnConfigs.length > 0) {
                gridColumnsManager.applyColumnConfig(columnConfigs);
                const newColumns = columnConfigs.filter(c => c.visible).sort((a, b) => a.order - b.order).map(c => c.field);
                applyColumnChanges(newColumns);
                const viewName = gridViewsManager.getCurrentViewName();
                $('#current-view-name').text(viewName);
                $('#btn-save-view-changes').hide();
            }
        } catch (error) {
            console.error('Error cargando vista:', error);
        }
    };

    $(document).on('ifChanged', '#check-all', function (this: HTMLInputElement) {
        const isChecked = $(this).is(':checked');
        $table.find('tbody input.row-check').iCheck(isChecked ? 'check' : 'uncheck');
    });

    $(document).on('ifChanged', '.row-check', function () {
        const total = $table.find('tbody input.row-check').length;
        const checked = $table.find('tbody input.row-check:checked').length;
        if (checked === total && total > 0) {
            $('#check-all').iCheck('check');
        } else {
            $('#check-all').iCheck('uncheck');
        }
        updateButtonStates();
    });

    $('#btn-new').on('click', () => { window.location.href = '/CalendarHoliday/NewEdit'; });

    $('#btn-edit').on('click', () => {
        const $checked = $table.find('tbody input.row-check:checked').first();
        if ($checked.length) {
            const recId = $checked.data('recid');
            window.location.href = `/CalendarHoliday/NewEdit?recId=${recId}`;
        }
    });

    $('#btn-delete').on('click', async () => {
        const $checked = $table.find('tbody input.row-check:checked');
        const count = $checked.length;
        if (count === 0) return;

        const message = count === 1
            ? '¿Está seguro de eliminar este día feriado?'
            : `¿Está seguro de eliminar ${count} días feriados?`;

        (w as any).ALERTS.confirm(
            message,
            'Confirmar Eliminación',
            async (confirmed: boolean) => {
                if (!confirmed) return;

                try {
                    const promises: Promise<void>[] = [];
                    $checked.each(function () {
                        const recId = $(this).data('recid');
                        if (recId) {
                            promises.push(deleteCalendarHoliday(recId));
                        }
                    });
                    await Promise.all(promises);
                    (w as any).ALERTS.ok('Día(s) feriado(s) eliminado(s) correctamente', 'Éxito');
                    $table.DataTable().ajax.reload();
                } catch (error) {
                    console.error('Error al eliminar:', error);
                    (w as any).ALERTS.error('Error al eliminar día(s) feriado(s)', 'Error');
                }
            },
            { type: 'danger' }
        );
    });

    $('#btn-export').on('click', exportToCSV);

    $('#btn-import').on('click', () => {
        (w as any).ALERTS.info('Funcionalidad de importación próximamente', 'Información');
    });

    $table.on('click', 'tbody tr', function (e: any) {
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
            window.location.href = `/CalendarHoliday/NewEdit?recId=${recId}`;
        }
    });

    $('#btn-manage-columns').on('click', () => { gridColumnsManager.showColumnsModal(); });

    $('#btn-apply-columns').on('click', () => {
        const columnConfigs = gridColumnsManager.applyColumns();
        const newColumns = columnConfigs.filter(c => c.visible).sort((a, b) => a.order - b.order).map(c => c.field);
        applyColumnChanges(newColumns);
        ($ as any)('#modal-manage-columns').modal('hide');
    });

    $('#btn-new-view').on('click', (e) => {
        e.preventDefault();
        $('#view-name').val('');
        $('#view-is-default').prop('checked', false);
        $('#view-is-public').prop('checked', false);
        ($ as any)('#modal-save-view').modal('show');
    });

    $('#btn-confirm-save-view').on('click', async () => {
        const viewName = ($('#view-name').val() as string).trim();
        const isDefault = $('#view-is-default').is(':checked');
        const isPublic = $('#view-is-public').is(':checked');

        if (!viewName) {
            (w as any).ALERTS.warn('Por favor ingrese un nombre para la vista', 'Advertencia');
            return;
        }

        const columnConfigs = gridColumnsManager.getCurrentColumnConfig();
        const success = await gridViewsManager.saveView(viewName, columnConfigs, isDefault, isPublic);

        if (success) {
            ($ as any)('#modal-save-view').modal('hide');
            $('#current-view-name').text(viewName);
            $('#btn-save-view-changes').hide();
            updateViewsDropdown();
        }
    });

    $('#btn-save-as-view').on('click', () => {
        $('#view-name-saveas').val('');
        $('#view-is-default-saveas').prop('checked', false);
        $('#view-is-public-saveas').prop('checked', false);
        ($ as any)('#modal-save-as-view').modal('show');
    });

    $('#btn-confirm-save-as').on('click', async () => {
        const viewName = ($('#view-name-saveas').val() as string).trim();
        const isDefault = $('#view-is-default-saveas').is(':checked');
        const isPublic = $('#view-is-public-saveas').is(':checked');

        if (!viewName) {
            (w as any).ALERTS.warn('Por favor ingrese un nombre para la vista', 'Advertencia');
            return;
        }

        const columnConfigs = gridColumnsManager.getCurrentColumnConfig();
        const success = await gridViewsManager.saveView(viewName, columnConfigs, isDefault, isPublic);

        if (success) {
            ($ as any)('#modal-save-as-view').modal('hide');
            $('#current-view-name').text(viewName);
            $('#btn-save-view-changes').hide();
            updateViewsDropdown();
        }
    });

    $('#btn-save-view-changes').on('click', async () => {
        if (!gridViewsManager.hasCurrentView()) {
            (w as any).ALERTS.warn('No hay vista activa para actualizar', 'Advertencia');
            return;
        }

        const columnConfigs = gridColumnsManager.getCurrentColumnConfig();
        const success = await gridViewsManager.updateView(columnConfigs);

        if (success) {
            $('#btn-save-view-changes').hide();
        }
    });

    $('#btn-reset-view').on('click', () => {
        (w as any).ALERTS.confirm(
            '¿Desea restablecer a la vista por defecto?',
            'Confirmar',
            (confirmed: boolean) => {
                if (confirmed) {
                    gridColumnsManager.resetToDefault(defaultColumns);
                    applyColumnChanges(defaultColumns);
                    $('#current-view-name').text('Vista por defecto');
                    $('#btn-save-view-changes').hide();
                }
            }
        );
    });

    $(document).on('click', '[data-view="default"]', (e) => {
        e.preventDefault();
        gridColumnsManager.resetToDefault(defaultColumns);
        applyColumnChanges(defaultColumns);
        $('#current-view-name').text('Vista por defecto');
        $('#btn-save-view-changes').hide();
    });

    $(async function () {
        try {
            const probeUrl = `${apiBase}/CalendarHolidays?skip=0&take=1`;
            const probe: CalendarHolidayRow[] = await fetchJson(probeUrl);
            
            // El API devuelve un array directo
            if (Array.isArray(probe) && probe.length > 0) {
                allColumns = getColumnsFromData(probe[0]);
            } else {
                allColumns = [...defaultColumns];
            }
            const GridViewsManagerClass = (w as any).GridViewsManager;
            const GridColumnsManagerClass = (w as any).GridColumnsManager;
            if (!GridViewsManagerClass || !GridColumnsManagerClass) {
                console.error('GridViewsManager o GridColumnsManager no están disponibles');
                visibleColumns = [...defaultColumns];
                initializeDataTable(visibleColumns);
                return;
            }
            gridViewsManager = new GridViewsManagerClass(apiBase, token, "CalendarHolidays", userRefRecID, dataareaId);
            const savedColumns: ColumnConfig[] = await gridViewsManager.initialize();
            if (savedColumns.length > 0) {
                visibleColumns = savedColumns.filter(c => c.visible).sort((a, b) => a.order - b.order).map(c => c.field);
                const viewName = gridViewsManager.getCurrentViewName();
                $('#current-view-name').text(viewName);
            } else {
                visibleColumns = [...defaultColumns];
            }
            gridColumnsManager = new GridColumnsManagerClass(allColumns, visibleColumns, (newColumns: string[]) => {
                applyColumnChanges(newColumns);
            });
            updateViewsDropdown();
            initializeDataTable(visibleColumns);
        } catch (error) {
            console.error('Error en inicialización:', error);
            visibleColumns = [...defaultColumns];
            initializeDataTable(visibleColumns);
            showError('Error al cargar la configuración');
        }
    });
})();
