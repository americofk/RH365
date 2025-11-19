// ============================================================================
// Archivo: employee-addresses.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Employees/employee-addresses.ts
// Descripcion:
//   - Tabla de direcciones dentro del formulario de empleado
//   - Endpoint correcto: /api/Countries (plural)
//   - CRUD con MODAL para crear/editar
// ISO 27001: Trazabilidad de operaciones sobre direcciones
// ============================================================================

interface AddressResponse {
    RecID: number;
    EmployeeRefRecID: number;
    CountryRefRecID?: number;
    Street?: string;
    Home?: string;
    Sector?: string;
    City?: string;
    Province?: string;
    ProvinceName?: string;
    Comment?: string;
    IsPrincipal: boolean;
    CreatedBy?: string;
    CreatedOn?: string;
    ModifiedBy?: string;
    ModifiedOn?: string;
}

type AddressRow = AddressResponse;

(function () {
    const w: any = window;
    const d: Document = document;
    const $: any = w.jQuery || w.$;

    const apiBase: string = w.RH365.urls.apiBase;
    const formContainer = d.querySelector("#employee-form-page");

    if (!formContainer) return;

    const token: string = formContainer.getAttribute("data-token") || "";
    const employeeRecId: number = parseInt(formContainer.getAttribute("data-recid") || "0", 10);
    const isNew: boolean = formContainer.getAttribute("data-isnew") === "true";

    const $table: any = $("#employee-addresses-table");
    if (!$table.length) return;

    let addressesData: AddressRow[] = [];
    let isEditMode: boolean = false;

    // ========================================================================
    // TRADUCCIONES Y FORMATEO
    // ========================================================================

    const titleize = (field: string): string => {
        const translations: Record<string, string> = {
            'RecID': 'ID',
            'Street': 'Calle',
            'Home': 'Casa/Apto',
            'Sector': 'Sector',
            'City': 'Ciudad',
            'Province': 'Provincia',
            'ProvinceName': 'Provincia',
            'IsPrincipal': 'Principal',
            'CreatedOn': 'Fecha Creacion'
        };
        return translations[field] || field.replace(/([a-z])([A-Z])/g, "$1 $2").replace(/_/g, " ").replace(/^./, (c) => c.toUpperCase());
    };

    const formatCell = (value: unknown, field: string): string => {
        if (value == null) return "";

        if (typeof value === "boolean") {
            if (field === "IsPrincipal") {
                return value ? '<span class="label label-primary">Si</span>' : '<span class="label label-default">No</span>';
            }
            return value ? "Si" : "No";
        }

        if (typeof value === "string" && /^\d{4}-\d{2}-\d{2}T/.test(value)) {
            const dt = new Date(value);
            if (!isNaN(dt.getTime())) {
                return dt.toLocaleDateString('es-DO', { day: '2-digit', month: '2-digit', year: 'numeric' });
            }
        }

        return String(value);
    };

    // ========================================================================
    // COMUNICACION CON API
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
            throw new Error(`HTTP ${response.status} @ ${url}`);
        }

        return response.json();
    };

    // ========================================================================
    // CARGA DE DATOS
    // ========================================================================

    const loadAddresses = async (): Promise<AddressRow[]> => {
        if (isNew) {
            return [];
        }

        try {
            const url = `${apiBase}/EmployeesAddress?pageNumber=1&pageSize=100`;
            const response = await fetchJson(url);

            let allAddresses: AddressRow[] = [];

            if (response?.Data && Array.isArray(response.Data)) {
                allAddresses = response.Data as AddressRow[];
            } else if (Array.isArray(response)) {
                allAddresses = response as AddressRow[];
            }

            const filteredAddresses = allAddresses.filter(
                (address: AddressRow) => address.EmployeeRefRecID === employeeRecId
            );

            console.log(`Direcciones totales: ${allAddresses.length}, del empleado ${employeeRecId}: ${filteredAddresses.length}`);

            return filteredAddresses;

        } catch (error) {
            console.error('Error cargando direcciones:', error);
            return [];
        }
    };

    const loadCountries = async (): Promise<void> => {
        try {
            // Endpoint correcto: /api/Countries (plural)
            const url = `${apiBase}/Countries?pageNumber=1&pageSize=100`;
            const response = await fetchJson(url);

            const $select = $('#address-CountryRefRecID');
            $select.empty().append('<option value="">-- Seleccione --</option>');

            if (response?.Data && Array.isArray(response.Data)) {
                response.Data.forEach((country: any) => {
                    $select.append(`<option value="${country.RecID}">${country.Name}</option>`);
                });
            }
        } catch (error) {
            console.error('Error cargando paises:', error);
        }
    };

    // ========================================================================
    // MODAL
    // ========================================================================

    const openModal = (recId?: number): void => {
        isEditMode = !!recId;

        $('#modal-address-title').text(isEditMode ? 'Editar Direccion' : 'Nueva Direccion');

        const form = d.getElementById('frm-address') as HTMLFormElement;
        if (form) form.reset();

        $('#address-RecID').val(recId || 0);
        $('#address-EmployeeRefRecID').val(employeeRecId);

        loadCountries();

        if (isEditMode && recId) {
            const address = addressesData.find((a: AddressRow) => a.RecID === recId);
            if (address) {
                $('#address-CountryRefRecID').val(address.CountryRefRecID || '');
                $('#address-City').val(address.City || '');
                $('#address-Province').val(address.Province || '');
                $('#address-Sector').val(address.Sector || '');
                $('#address-Street').val(address.Street || '');
                $('#address-Home').val(address.Home || '');
                $('#address-Comment').val(address.Comment || '');

                if (address.IsPrincipal) {
                    $('#address-IsPrincipal').iCheck('check');
                } else {
                    $('#address-IsPrincipal').iCheck('uncheck');
                }
            }
        } else {
            $('#address-IsPrincipal').iCheck('uncheck');
        }

        if ($.fn.iCheck) {
            $('.flat').iCheck({ checkboxClass: 'icheckbox_flat-green' });
        }

        ($ as any)('#modal-address').modal('show');
    };

    const saveAddress = async (): Promise<void> => {
        const form = d.getElementById('frm-address') as HTMLFormElement;
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        const recId = parseInt($('#address-RecID').val() as string) || 0;
        const payload = {
            EmployeeRefRecID: employeeRecId,
            CountryRefRecID: parseInt($('#address-CountryRefRecID').val() as string) || null,
            City: $('#address-City').val() as string || null,
            Province: $('#address-Province').val() as string || null,
            Sector: $('#address-Sector').val() as string || null,
            Street: $('#address-Street').val() as string || null,
            Home: $('#address-Home').val() as string || null,
            Comment: $('#address-Comment').val() as string || null,
            IsPrincipal: $('#address-IsPrincipal').is(':checked')
        };

        try {
            const url = recId > 0
                ? `${apiBase}/EmployeesAddress/${recId}`
                : `${apiBase}/EmployeesAddress`;

            const method = recId > 0 ? 'PUT' : 'POST';

            await fetchJson(url, {
                method: method,
                body: JSON.stringify(payload)
            });

            (w as any).ALERTS.ok(
                recId > 0 ? 'Direccion actualizada exitosamente' : 'Direccion creada exitosamente',
                'Exito'
            );

            ($ as any)('#modal-address').modal('hide');
            $table.DataTable().ajax.reload();

        } catch (error: any) {
            console.error('Error al guardar direccion:', error);
            (w as any).ALERTS.error('Error al guardar la direccion', 'Error');
        }
    };

    const deleteAddress = async (recId: string): Promise<void> => {
        const url = `${apiBase}/EmployeesAddress/${recId}`;
        const response = await fetch(url, {
            method: 'DELETE',
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (!response.ok) {
            throw new Error(`Error al eliminar direccion: ${response.status}`);
        }
    };

    // ========================================================================
    // DATATABLE
    // ========================================================================

    const initializeDataTable = (): void => {
        if ($.fn.DataTable.isDataTable($table)) {
            $table.DataTable().destroy();
        }

        const columns = ['RecID', 'Province', 'City', 'Street', 'Home', 'IsPrincipal'];

        const theadHtml = `
            <tr>
                <th style="width:40px;"><input type="checkbox" id="check-all-addresses" class="flat"/></th>
                ${columns.map(col => `<th>${titleize(col)}</th>`).join('')}
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
            pageLength: 10,
            lengthMenu: [[5, 10, 25], [5, 10, 25]],
            language: {
                lengthMenu: 'Mostrar _MENU_ registros',
                zeroRecords: 'No hay direcciones registradas',
                info: 'Mostrando _START_ a _END_ de _TOTAL_ direcciones',
                infoEmpty: 'No hay direcciones',
                infoFiltered: '(filtrado de _MAX_ registros)',
                search: 'Buscar:',
                paginate: {
                    first: 'Primera',
                    last: 'Ultima',
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
                    render: (_data: any, _type: string, row: AddressRow) => {
                        return `<input type="checkbox" class="flat row-check-address" data-recid="${row.RecID || ''}"/>`;
                    }
                },
                ...columns.map(col => ({
                    data: col,
                    name: col,
                    render: (data: unknown) => formatCell(data, col)
                }))
            ],
            ajax: (_data: any, callback: (result: { data: AddressRow[] }) => void) => {
                loadAddresses().then(items => {
                    addressesData = items;
                    callback({ data: items });
                    updateSummary(items.length);
                }).catch(err => {
                    console.error('Error cargando direcciones:', err);
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

    const updateSummary = (count: number): void => {
        const summary = d.getElementById('addresses-summary');
        if (summary) {
            summary.textContent = `${count} direccion${count !== 1 ? 'es' : ''}`;
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
        const checkedCount = $table.find('tbody input.row-check-address:checked').length;
        $('#btn-edit-address').prop('disabled', checkedCount !== 1);
        $('#btn-delete-address').prop('disabled', checkedCount === 0);
    };

    // ========================================================================
    // EVENT HANDLERS
    // ========================================================================

    $(document).on('ifChanged', '#check-all-addresses', function (this: HTMLInputElement) {
        const isChecked = $(this).is(':checked');
        $table.find('tbody input.row-check-address').iCheck(isChecked ? 'check' : 'uncheck');
    });

    $(document).on('ifChanged', '.row-check-address', function () {
        const total = $table.find('tbody input.row-check-address').length;
        const checked = $table.find('tbody input.row-check-address:checked').length;

        if (checked === total && total > 0) {
            $('#check-all-addresses').iCheck('check');
        } else {
            $('#check-all-addresses').iCheck('uncheck');
        }

        updateButtonStates();
    });

    $('#btn-new-address').on('click', () => {
        if (isNew) {
            (w as any).ALERTS.warn('Debe guardar el empleado antes de agregar direcciones', 'Advertencia');
            return;
        }

        openModal();
    });

    $('#btn-edit-address').on('click', () => {
        const $checked = $table.find('tbody input.row-check-address:checked').first();
        if ($checked.length) {
            const recId = parseInt($checked.data('recid'));
            openModal(recId);
        }
    });

    $('#btn-save-address').on('click', async () => {
        await saveAddress();
    });

    $('#btn-delete-address').on('click', async () => {
        const $checked = $table.find('tbody input.row-check-address:checked');
        const count = $checked.length;

        if (count === 0) return;

        const message = count === 1
            ? '¿Esta seguro de eliminar esta direccion?'
            : `¿Esta seguro de eliminar ${count} direcciones?`;

        (w as any).ALERTS.confirm(
            message,
            'Confirmar Eliminacion',
            async (confirmed: boolean) => {
                if (!confirmed) return;

                try {
                    const promises: Promise<void>[] = [];
                    $checked.each(function () {
                        const recId = $(this).data('recid');
                        if (recId) {
                            promises.push(deleteAddress(recId));
                        }
                    });

                    await Promise.all(promises);
                    (w as any).ALERTS.ok('Direccion(es) eliminada(s) correctamente', 'Exito');
                    $table.DataTable().ajax.reload();
                } catch (error) {
                    console.error('Error al eliminar:', error);
                    (w as any).ALERTS.error('Error al eliminar direccion(es)', 'Error');
                }
            },
            { type: 'danger' }
        );
    });

    $table.on('click', 'tbody tr', function (e: any) {
        if ($(e.target).is('input.row-check-address') || $(e.target).closest('.icheckbox_flat-green').length) {
            return;
        }

        const $row = $(this);
        const $checkbox = $row.find('input.row-check-address');

        if ($checkbox.length) {
            const isChecked = $checkbox.is(':checked');
            $checkbox.iCheck(isChecked ? 'uncheck' : 'check');
        }
    });

    $table.on('dblclick', 'tbody tr', function () {
        const $row = $(this);
        const $checkbox = $row.find('input.row-check-address');
        const recId = parseInt($checkbox.data('recid'));

        if (recId) {
            openModal(recId);
        }
    });

    // ========================================================================
    // INICIALIZACION
    // ========================================================================

    $(async function () {
        try {
            if (isNew) {
                const infoHtml = `
                    <div class="alert alert-info" role="alert">
                        <i class="fa fa-info-circle"></i>
                        <strong>Empleado Nuevo:</strong> 
                        Guarde primero los datos del empleado para poder agregar direcciones.
                    </div>
                `;
                $('#addresses-info-container').html(infoHtml);
                $('#btn-new-address').prop('disabled', true);
                return;
            }

            initializeDataTable();

        } catch (error) {
            console.error('Error en inicializacion de direcciones:', error);
            showError('Error al cargar las direcciones');
        }
    });
})();