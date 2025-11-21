// ============================================================================
// Archivo: employee-documents.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Employees/employee-documents.ts
// Descripcion:
//   - Tabla de documentos dentro del formulario de empleado
//   - CRUD con MODAL para crear/editar documentos de identidad
//   - Soporta tipos: Cedula, Pasaporte, Residencia, Licencia de conducir
//   - Filtra documentos en frontend por EmployeeRefRecID
// ISO 27001: Trazabilidad y seguridad en gestion de documentos
// ============================================================================

interface DocumentResponse {
    RecID: number;
    EmployeeRefRecID: number;
    DocumentType: number;
    DocumentTypeName?: string;
    DocumentNumber?: string;
    DueDate?: string;
    IsPrincipal: boolean;
    Comment?: string;
    CreatedBy?: string;
    CreatedOn?: string;
    ModifiedBy?: string;
    ModifiedOn?: string;
}

type DocumentRow = DocumentResponse;

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

    const $table: any = $("#employee-documents-table");
    if (!$table.length) return;

    let documentsData: DocumentRow[] = [];
    let isEditMode: boolean = false;

    // ========================================================================
    // TRADUCCIONES Y FORMATEO
    // ========================================================================

    const titleize = (field: string): string => {
        const translations: Record<string, string> = {
            'RecID': 'ID',
            'DocumentTypeName': 'Tipo Documento',
            'DocumentNumber': 'Numero',
            'DueDate': 'Vencimiento',
            'IsPrincipal': 'Principal',
            'CreatedOn': 'Creado'
        };
        return translations[field] || field.replace(/([a-z])([A-Z])/g, "$1 $2").replace(/_/g, " ").replace(/^./, (c) => c.toUpperCase());
    };

    const formatCell = (value: unknown, field: string): string => {
        if (value == null || value === undefined || value === '') return "";

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

    // Opciones de tipo de documento (GlobalEnum.DocumentType)
    const documentTypeOptions: { value: number; text: string }[] = [
        { value: 0, text: 'Cedula' },
        { value: 1, text: 'Pasaporte' },
        { value: 2, text: 'Residencia' },
        { value: 3, text: 'Licencia de conducir' }
    ];

    const getDocumentTypeName = (typeValue: number): string => {
        const option = documentTypeOptions.find(opt => opt.value === typeValue);
        return option ? option.text : 'Desconocido';
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
            const errorData = await response.json().catch(() => ({}));
            throw new Error(JSON.stringify(errorData));
        }

        // Si la respuesta no tiene contenido (ej: DELETE), retornar objeto vacio
        const contentType = response.headers.get('content-type');
        if (!contentType || !contentType.includes('application/json')) {
            return {};
        }

        const text = await response.text();
        if (!text || text.trim().length === 0) {
            return {};
        }

        return JSON.parse(text);
    };

    // ========================================================================
    // DATATABLE
    // ========================================================================

    const initializeDataTable = (): void => {
        if ($.fn.DataTable.isDataTable($table)) {
            $table.DataTable().destroy();
        }

        const columns: string[] = ['RecID', 'DocumentTypeName', 'DocumentNumber', 'DueDate', 'IsPrincipal', 'CreatedOn'];

        const theadHtml = `
            <tr>
                <th style="width:40px;"><input type="checkbox" id="check-all-documents" class="flat"/></th>
                ${columns.map(col => `<th data-field="${col}">${titleize(col)}</th>`).join('')}
            </tr>
        `;
        $table.find('thead').html(theadHtml);

        if ($.fn.iCheck) {
            $('.flat').iCheck({ checkboxClass: 'icheckbox_flat-green' });
        }

        const dtConfig: any = {
            processing: false,
            serverSide: false,
            responsive: true,
            autoWidth: false,
            paging: false,
            searching: false,
            lengthChange: false,
            info: false,
            order: [[1, 'asc']],
            language: {
                zeroRecords: 'No hay documentos registrados',
                processing: 'Procesando...'
            },
            columns: [
                {
                    data: null,
                    orderable: false,
                    searchable: false,
                    className: 'text-center',
                    render: (_data: any, _type: string, row: DocumentRow) => {
                        return `<input type="checkbox" class="flat row-check-document" data-recid="${row.RecID}"/>`;
                    }
                },
                {
                    data: 'RecID',
                    name: 'RecID',
                    render: (data: unknown) => formatCell(data, 'RecID')
                },
                {
                    data: 'DocumentTypeName',
                    name: 'DocumentTypeName',
                    render: (data: unknown, type: string, row: DocumentRow) => {
                        const typeName = data || getDocumentTypeName(row.DocumentType);
                        return formatCell(typeName, 'DocumentTypeName');
                    }
                },
                {
                    data: 'DocumentNumber',
                    name: 'DocumentNumber',
                    render: (data: unknown) => formatCell(data, 'DocumentNumber')
                },
                {
                    data: 'DueDate',
                    name: 'DueDate',
                    render: (data: unknown) => formatCell(data, 'DueDate')
                },
                {
                    data: 'IsPrincipal',
                    name: 'IsPrincipal',
                    render: (data: unknown) => formatCell(data, 'IsPrincipal')
                },
                {
                    data: 'CreatedOn',
                    name: 'CreatedOn',
                    render: (data: unknown) => formatCell(data, 'CreatedOn')
                }
            ],
            ajax: (_data: any, callback: (result: { data: DocumentRow[] }) => void) => {
                loadDocuments().then(items => {
                    documentsData = items;
                    callback({ data: items });
                }).catch(err => {
                    console.error('Error cargando documentos:', err);
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

    // ========================================================================
    // CARGA DE DATOS
    // ========================================================================

    const loadDocuments = async (): Promise<DocumentRow[]> => {
        if (isNew) {
            showInfoMessage('Guarde el empleado primero para gestionar documentos');
            return [];
        }

        try {
            const url = `${apiBase}/EmployeeDocuments?pageNumber=1&pageSize=1000`;
            const response: any = await fetchJson(url);

            let allDocuments: DocumentRow[] = [];

            if (response?.Data && Array.isArray(response.Data)) {
                allDocuments = response.Data as DocumentRow[];
            } else if (Array.isArray(response)) {
                allDocuments = response as DocumentRow[];
            }

            const filteredDocuments = allDocuments.filter(
                (doc: DocumentRow) => doc.EmployeeRefRecID === employeeRecId
            );

            filteredDocuments.forEach(doc => {
                if (!doc.DocumentTypeName) {
                    doc.DocumentTypeName = getDocumentTypeName(doc.DocumentType);
                }
            });

            console.log(`Documentos totales: ${allDocuments.length}, del empleado ${employeeRecId}: ${filteredDocuments.length}`);

            return filteredDocuments;

        } catch (error) {
            console.error('Error cargando documentos:', error);
            return [];
        }
    };

    const showInfoMessage = (message: string): void => {
        const container = $('#documents-info-container');
        container.html(`
            <div class="alert alert-info alert-dismissible fade in" role="alert">
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">×</span>
                </button>
                <i class="fa fa-info-circle"></i> ${message}
            </div>
        `);
    };

    const showError = (message: string): void => {
        (w as any).ALERTS.error(message, 'Error');
    };

    const updateButtonStates = (): void => {
        const checkedCount = $table.find('tbody input.row-check-document:checked').length;
        $('#btn-edit-document').prop('disabled', checkedCount !== 1);
        $('#btn-delete-document').prop('disabled', checkedCount === 0);
    };

    // ========================================================================
    // MODAL
    // ========================================================================

    const openModal = async (recId?: number): Promise<void> => {
        isEditMode = !!recId;

        $('#modal-document-title').text(isEditMode ? 'Editar Documento' : 'Nuevo Documento');

        const form = d.getElementById('frm-document') as HTMLFormElement;
        if (form) form.reset();

        $('#document-RecID').val(recId || 0);
        $('#document-EmployeeRefRecID').val(employeeRecId);

        const selectHtml = '<option value="">-- Seleccione --</option>' +
            documentTypeOptions.map(opt => `<option value="${opt.value}">${opt.text}</option>`).join('');
        $('#document-DocumentType').html(selectHtml);

        // Inicializar IsPrincipal checkbox desmarcado
        if ($.fn.iCheck) {
            $('#document-IsPrincipal').iCheck('uncheck');
        }

        if (isEditMode && recId) {
            const document = documentsData.find((d: DocumentRow) => d.RecID === recId);
            if (document) {
                $('#document-DocumentType').val(document.DocumentType);
                $('#document-DocumentNumber').val(document.DocumentNumber || '');

                if (document.DueDate) {
                    const dueDate = new Date(document.DueDate);
                    if (!isNaN(dueDate.getTime())) {
                        $('#document-DueDate').val(dueDate.toISOString().split('T')[0]);
                    }
                }

                $('#document-Comment').val(document.Comment || '');

                if (document.IsPrincipal) {
                    $('#document-IsPrincipal').iCheck('check');
                } else {
                    $('#document-IsPrincipal').iCheck('uncheck');
                }
            }
        }

        ($('#modal-document') as any).modal('show');
    };

    // ========================================================================
    // GUARDAR DOCUMENTO
    // ========================================================================

    const saveDocument = async (): Promise<void> => {
        const form = document.getElementById('frm-document') as HTMLFormElement;

        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        const recId = parseInt($('#document-RecID').val() as string, 10);
        const employeeRefRecID = parseInt($('#document-EmployeeRefRecID').val() as string, 10);
        const documentType = parseInt($('#document-DocumentType').val() as string, 10);
        const documentNumber = ($('#document-DocumentNumber').val() as string).trim();
        const dueDateStr = $('#document-DueDate').val() as string;
        const isPrincipal = $('#document-IsPrincipal').is(':checked');
        const comment = ($('#document-Comment').val() as string).trim();

        if (isNaN(documentType)) {
            showError('Debe seleccionar un tipo de documento');
            return;
        }

        if (!documentNumber) {
            showError('Debe ingresar el numero de documento');
            return;
        }

        const payload: any = {
            EmployeeRefRecID: employeeRefRecID,
            DocumentType: documentType,
            DocumentNumber: documentNumber,
            DueDate: dueDateStr || null,
            IsPrincipal: isPrincipal,
            Comment: comment || null
        };

        try {
            if (isEditMode && recId > 0) {
                const url = `${apiBase}/EmployeeDocuments/${recId}`;
                await fetchJson(url, {
                    method: 'PUT',
                    body: JSON.stringify(payload)
                });
                (w as any).ALERTS.ok('Documento actualizado exitosamente', 'Exito');
            } else {
                const url = `${apiBase}/EmployeeDocuments`;
                await fetchJson(url, {
                    method: 'POST',
                    body: JSON.stringify(payload)
                });
                (w as any).ALERTS.ok('Documento creado exitosamente', 'Exito');
            }

            ($('#modal-document') as any).modal('hide');
            $table.DataTable().ajax.reload();

        } catch (error: any) {
            console.error('Error al guardar documento:', error);
            let errorMessage = 'Error al guardar el documento';

            try {
                const errorData = JSON.parse(error.message);
                if (errorData.errors) {
                    const errorsArray: string[] = [];
                    for (const key in errorData.errors) {
                        if (errorData.errors.hasOwnProperty(key)) {
                            const errList = errorData.errors[key];
                            if (Array.isArray(errList)) {
                                errorsArray.push(...errList);
                            } else {
                                errorsArray.push(errList);
                            }
                        }
                    }
                    errorMessage = errorsArray.join(', ');
                } else if (errorData.title) {
                    errorMessage = errorData.title;
                }
            } catch {
                errorMessage = error.message || errorMessage;
            }

            showError(errorMessage);
        }
    };

    // ========================================================================
    // ELIMINAR DOCUMENTOS
    // ========================================================================

    const deleteDocuments = async (): Promise<void> => {
        const $checked = $table.find('tbody input.row-check-document:checked');
        const count = $checked.length;

        if (count === 0) return;

        const message = count === 1
            ? '¿Esta seguro de eliminar este documento?'
            : `¿Esta seguro de eliminar ${count} documentos?`;

        (w as any).ALERTS.confirm(
            message,
            'Confirmar Eliminacion',
            async (confirmed: boolean) => {
                if (!confirmed) return;

                try {
                    const promises: Promise<void>[] = [];
                    $checked.each(function () {
                        const recId = parseInt($(this).data('recid'), 10);
                        if (recId) {
                            const url = `${apiBase}/EmployeeDocuments/${recId}`;
                            promises.push(fetchJson(url, { method: 'DELETE' }));
                        }
                    });

                    await Promise.all(promises);
                    (w as any).ALERTS.ok('Documento(s) eliminado(s) correctamente', 'Exito');
                    $table.DataTable().ajax.reload();

                } catch (error) {
                    console.error('Error al eliminar:', error);
                    showError('Error al eliminar documento(s)');
                }
            },
            { type: 'danger' }
        );
    };

    // ========================================================================
    // EVENT HANDLERS
    // ========================================================================

    $(document).on('ifChanged', '#check-all-documents', function (this: HTMLInputElement) {
        const isChecked = $(this).is(':checked');
        $table.find('tbody input.row-check-document').iCheck(isChecked ? 'check' : 'uncheck');
    });

    $(document).on('ifChanged', '.row-check-document', function () {
        const total = $table.find('tbody input.row-check-document').length;
        const checked = $table.find('tbody input.row-check-document:checked').length;

        if (checked === total && total > 0) {
            $('#check-all-documents').iCheck('check');
        } else {
            $('#check-all-documents').iCheck('uncheck');
        }

        updateButtonStates();
    });

    $('#btn-new-document').on('click', () => {
        if (isNew) {
            showError('Debe guardar el empleado antes de agregar documentos');
            return;
        }
        openModal();
    });

    $('#btn-edit-document').on('click', () => {
        const $checked = $table.find('tbody input.row-check-document:checked').first();
        if ($checked.length) {
            const recId = parseInt($checked.data('recid'), 10);
            openModal(recId);
        }
    });

    $('#btn-delete-document').on('click', deleteDocuments);
    $('#btn-save-document').on('click', saveDocument);

    $table.on('click', 'tbody tr', function (e: any) {
        if ($(e.target).is('input.row-check-document') || $(e.target).closest('.icheckbox_flat-green').length) {
            return;
        }

        const $row = $(this);
        const $checkbox = $row.find('input.row-check-document');
        if ($checkbox.length) {
            const isChecked = $checkbox.is(':checked');
            $checkbox.iCheck(isChecked ? 'uncheck' : 'check');
        }
    });

    $table.on('dblclick', 'tbody tr', function () {
        const $row = $(this);
        const $checkbox = $row.find('input.row-check-document');
        const recId = parseInt($checkbox.data('recid'), 10);
        if (recId) {
            $table.find('tbody input.row-check-document').iCheck('uncheck');
            $checkbox.iCheck('check');
            setTimeout(() => openModal(recId), 100);
        }
    });

    // ========================================================================
    // INICIALIZACION
    // ========================================================================

    $(async function () {
        try {
            initializeDataTable();
        } catch (error) {
            console.error('Error en inicializacion de documentos:', error);
        }
    });
})();
