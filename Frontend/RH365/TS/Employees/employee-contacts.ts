// ============================================================================
// Archivo: employee-contacts.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Employees/employee-contacts.ts
// Descripcion:
//   - Tabla de contactos dentro del formulario de empleado
//   - Endpoint: /api/EmployeeContactsInf
//   - CRUD con MODAL para crear/editar
// ISO 27001: Trazabilidad de operaciones sobre contactos
// ============================================================================

interface ContactResponse {
    RecID: number;
    EmployeeRefRecID: number;
    ContactType: number;
    ContactValue: string;
    IsPrincipal: boolean;
    Comment?: string;
    CreatedBy?: string;
    CreatedOn?: string;
    ModifiedBy?: string;
    ModifiedOn?: string;
}

type ContactRow = ContactResponse;

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

    const $table: any = $("#employee-contacts-table");
    if (!$table.length) return;

    let contactsData: ContactRow[] = [];
    let isEditMode: boolean = false;

    // ========================================================================
    // MAPEO DE TIPOS DE CONTACTO (GlobalsEnum.ContactType)
    // ========================================================================
    const contactTypeMap: { [key: number]: string } = {
        0: 'Celular',
        1: 'Correo',
        2: 'Telefono',
        3: 'Otro'
    };

    const getContactTypeName = (type: number): string => {
        return contactTypeMap[type] || 'Desconocido';
    };

    // ========================================================================
    // TRADUCCIONES Y FORMATEO
    // ========================================================================

    const titleize = (field: string): string => {
        const translations: Record<string, string> = {
            'RecID': 'ID',
            'ContactType': 'Tipo',
            'ContactValue': 'Valor',
            'IsPrincipal': 'Principal',
            'Comment': 'Comentario',
            'CreatedOn': 'Fecha Creacion'
        };
        return translations[field] || field.replace(/([a-z])([A-Z])/g, "$1 $2").replace(/_/g, " ").replace(/^./, (c) => c.toUpperCase());
    };

    const formatCell = (value: unknown, field: string): string => {
        if (value == null) return "";

        if (field === "ContactType") {
            return getContactTypeName(value as number);
        }

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

    const loadContacts = async (): Promise<ContactRow[]> => {
        if (isNew) {
            return [];
        }

        try {
            const url = `${apiBase}/EmployeeContactsInf?skip=0&take=100`;
            const response = await fetchJson(url);

            let allContacts: ContactRow[] = [];

            if (Array.isArray(response)) {
                allContacts = response as ContactRow[];
            } else if (response?.Data && Array.isArray(response.Data)) {
                allContacts = response.Data as ContactRow[];
            }

            const filteredContacts = allContacts.filter(
                (contact: ContactRow) => contact.EmployeeRefRecID === employeeRecId
            );

            console.log(`Contactos totales: ${allContacts.length}, del empleado ${employeeRecId}: ${filteredContacts.length}`);

            return filteredContacts;

        } catch (error) {
            console.error('Error cargando contactos:', error);
            return [];
        }
    };

    // ========================================================================
    // MODAL
    // ========================================================================

    const openModal = (recId?: number): void => {
        isEditMode = !!recId;

        $('#modal-contact-title').text(isEditMode ? 'Editar Contacto' : 'Nuevo Contacto');

        // Limpiar formulario
        const form = d.getElementById('frm-contact') as HTMLFormElement;
        if (form) form.reset();

        $('#contact-RecID').val(recId || 0);
        $('#contact-EmployeeRefRecID').val(employeeRecId);

        // Inicializar iCheck ANTES de establecer valores
        if ($.fn.iCheck) {
            const $checkbox = $('#contact-IsPrincipal');
            // Destruir iCheck si existe
            $checkbox.iCheck('destroy');
            // Re-inicializar
            $checkbox.iCheck({ 
                checkboxClass: 'icheckbox_flat-green',
                radioClass: 'iradio_flat-green'
            });
        }

        if (isEditMode && recId) {
            const contact = contactsData.find((c: ContactRow) => c.RecID === recId);
            if (contact) {
                console.log('Cargando contacto en modal:', contact);
                
                // Establecer valores
                $('#contact-ContactType').val(contact.ContactType.toString());
                $('#contact-ContactValue').val(contact.ContactValue || '');
                $('#contact-Comment').val(contact.Comment || '');

                console.log('Comment cargado:', contact.Comment);
                console.log('Valor en textarea:', $('#contact-Comment').val());

                // CRITICAL: Establecer checkbox usando iCheck
                const $checkbox = $('#contact-IsPrincipal');
                
                // Esperar un tick para que iCheck esté completamente inicializado
                setTimeout(() => {
                    if (contact.IsPrincipal) {
                        console.log('Marcando checkbox como checked');
                        $checkbox.iCheck('check');
                    } else {
                        console.log('Desmarcando checkbox');
                        $checkbox.iCheck('uncheck');
                    }
                    
                    // Verificar estado después de establecer
                    console.log('IsPrincipal del contacto:', contact.IsPrincipal);
                    console.log('Checkbox is checked?', $checkbox.is(':checked'));
                }, 100);
            }
        } else {
            // Modo creación: desmarcar checkbox
            $('#contact-IsPrincipal').iCheck('uncheck');
        }

        ($ as any)('#modal-contact').modal('show');
    };

    const saveContact = async (): Promise<void> => {
        const form = d.getElementById('frm-contact') as HTMLFormElement;
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        const recId = parseInt($('#contact-RecID').val() as string) || 0;
        
        // CRITICAL: Leer IsPrincipal correctamente con iCheck
        const $checkbox = $('#contact-IsPrincipal');
        const isPrincipal = $checkbox.is(':checked');
        
        console.log('=== GUARDANDO CONTACTO ===');
        console.log('Checkbox element:', $checkbox);
        console.log('Is checked?', isPrincipal);
        console.log('Checkbox prop checked:', $checkbox.prop('checked'));
        
        const payload = {
            EmployeeRefRecID: employeeRecId,
            ContactType: parseInt($('#contact-ContactType').val() as string),
            ContactValue: ($('#contact-ContactValue').val() as string).trim(),
            IsPrincipal: isPrincipal,
            Comment: ($('#contact-Comment').val() as string || '').trim()
        };

        console.log('Payload a enviar:', payload);

        try {
            const url = isEditMode
                ? `${apiBase}/EmployeeContactsInf/${recId}`
                : `${apiBase}/EmployeeContactsInf`;

            const method = isEditMode ? 'PUT' : 'POST';

            const result = await fetchJson(url, {
                method: method,
                body: JSON.stringify(payload)
            });

            console.log('Respuesta del servidor:', result);

            (w as any).ALERTS.ok(
                isEditMode ? 'Contacto actualizado exitosamente' : 'Contacto creado exitosamente',
                'Exito'
            );

            ($ as any)('#modal-contact').modal('hide');
            
            // CRITICAL: Recargar tabla correctamente
            if ($.fn.DataTable.isDataTable($table)) {
                $table.DataTable().ajax.reload(null, false); // false = mantener paginación
            } else {
                initializeDataTable();
            }

        } catch (error) {
            console.error('Error al guardar contacto:', error);
            (w as any).ALERTS.error('Error al guardar contacto', 'Error');
        }
    };

    const deleteContact = async (recId: number): Promise<void> => {
        const url = `${apiBase}/EmployeeContactsInf/${recId}`;
        const response = await fetch(url, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (!response.ok) {
            throw new Error(`Error al eliminar contacto: ${response.status}`);
        }
    };

    // ========================================================================
    // DATATABLE
    // ========================================================================

    const initializeDataTable = (): void => {
        if ($.fn.DataTable.isDataTable($table)) {
            $table.DataTable().destroy();
        }

        const columns = ['ContactType', 'ContactValue', 'IsPrincipal', 'Comment'];

        const theadHtml = `
            <tr>
                <th style="width:40px;"><input type="checkbox" id="check-all-contacts" class="flat"/></th>
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
            pageLength: 10,
            lengthMenu: [[5, 10, 25], [5, 10, 25]],
            language: {
                lengthMenu: 'Mostrar _MENU_ registros',
                zeroRecords: 'No hay contactos registrados',
                info: 'Mostrando _START_ a _END_ de _TOTAL_ contactos',
                infoEmpty: 'No hay contactos',
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
                    render: (_data: any, _type: string, row: ContactRow) => {
                        return `<input type="checkbox" class="flat row-check-contact" data-recid="${row.RecID || ''}"/>`;
                    }
                },
                ...columns.map(col => ({
                    data: col,
                    name: col,
                    render: (data: unknown) => formatCell(data, col)
                }))
            ],
            ajax: (_data: any, callback: (result: { data: ContactRow[] }) => void) => {
                loadContacts().then(items => {
                    contactsData = items;
                    callback({ data: items });
                    updateSummary(items.length);
                }).catch(err => {
                    console.error('Error cargando contactos:', err);
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
        const summary = d.getElementById('contacts-summary');
        if (summary) {
            summary.textContent = `${count} contacto${count !== 1 ? 's' : ''}`;
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
        const checkedCount = $table.find('tbody input.row-check-contact:checked').length;
        $('#btn-edit-contact').prop('disabled', checkedCount !== 1);
        $('#btn-delete-contact').prop('disabled', checkedCount === 0);
    };

    // ========================================================================
    // EVENT HANDLERS
    // ========================================================================

    $(document).on('ifChanged', '#check-all-contacts', function (this: HTMLInputElement) {
        const isChecked = $(this).is(':checked');
        $table.find('tbody input.row-check-contact').iCheck(isChecked ? 'check' : 'uncheck');
    });

    $(document).on('ifChanged', '.row-check-contact', function () {
        const total = $table.find('tbody input.row-check-contact').length;
        const checked = $table.find('tbody input.row-check-contact:checked').length;

        if (checked === total && total > 0) {
            $('#check-all-contacts').iCheck('check');
        } else {
            $('#check-all-contacts').iCheck('uncheck');
        }

        updateButtonStates();
    });

    $('#btn-new-contact').on('click', () => {
        if (isNew) {
            (w as any).ALERTS.warn('Debe guardar el empleado antes de agregar contactos', 'Advertencia');
            return;
        }

        openModal();
    });

    $('#btn-edit-contact').on('click', () => {
        const $checked = $table.find('tbody input.row-check-contact:checked').first();
        if ($checked.length) {
            const recId = parseInt($checked.data('recid'));
            openModal(recId);
        }
    });

    $('#btn-save-contact').on('click', async () => {
        await saveContact();
    });

    $('#btn-delete-contact').on('click', async () => {
        const $checked = $table.find('tbody input.row-check-contact:checked');
        const count = $checked.length;

        if (count === 0) return;

        const message = count === 1
            ? '¿Esta seguro de eliminar este contacto?'
            : `¿Esta seguro de eliminar ${count} contactos?`;

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
                            promises.push(deleteContact(recId));
                        }
                    });

                    await Promise.all(promises);
                    (w as any).ALERTS.ok('Contacto(s) eliminado(s) correctamente', 'Exito');
                    $table.DataTable().ajax.reload(null, false);
                } catch (error) {
                    console.error('Error al eliminar:', error);
                    (w as any).ALERTS.error('Error al eliminar contacto(s)', 'Error');
                }
            },
            { type: 'danger' }
        );
    });

    $table.on('click', 'tbody tr', function (e: any) {
        if ($(e.target).is('input.row-check-contact') || $(e.target).closest('.icheckbox_flat-green').length) {
            return;
        }

        const $row = $(this);
        const $checkbox = $row.find('input.row-check-contact');

        if ($checkbox.length) {
            const isChecked = $checkbox.is(':checked');
            $checkbox.iCheck(isChecked ? 'uncheck' : 'check');
        }
    });

    $table.on('dblclick', 'tbody tr', function () {
        const $row = $(this);
        const $checkbox = $row.find('input.row-check-contact');
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
                        Guarde primero los datos del empleado para poder agregar contactos.
                    </div>
                `;
                $('#contacts-info-container').html(infoHtml);
                $('#btn-new-contact').prop('disabled', true);
                return;
            }

            initializeDataTable();

        } catch (error) {
            console.error('Error en inicializacion de contactos:', error);
            showError('Error al cargar los contactos');
        }
    });
})();
