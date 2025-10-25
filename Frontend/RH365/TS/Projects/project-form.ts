// ============================================================================
// Archivo: project-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Projects/project-form.ts
// Descripción: Formulario dinámico para Crear/Editar Proyectos
// ============================================================================

(function () {
    const w: any = window;
    const d: Document = document;
    const $: any = w.jQuery || w.$;

    const apiBase: string = (w.RH365?.urls?.apiBase) || "http://localhost:9595/api";
    const pageContainer = d.querySelector("#project-form-page");

    if (!pageContainer) return;

    const token: string = pageContainer.getAttribute("data-token") || "";
    const dataareaId: string = pageContainer.getAttribute("data-dataarea") || "DAT";
    const userRefRecID: number = parseInt(pageContainer.getAttribute("data-user") || "0", 10);
    const recId: number = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    const isNew: boolean = pageContainer.getAttribute("data-isnew") === "true";

    // Configuración de campos (similar a columnas de DataTable)
    interface FieldConfig {
        field: string;
        label: string;
        type: 'text' | 'number' | 'date' | 'datetime' | 'textarea' | 'select' | 'checkbox';
        required?: boolean;
        maxLength?: number;
        options?: { value: string; text: string }[];
        placeholder?: string;
        helpText?: string;
        readonly?: boolean;
    }

    const fieldDefinitions: FieldConfig[] = [
        { field: 'ID', label: 'ID Sistema', type: 'text', readonly: true },
        { field: 'ProjectCode', label: 'Código Proyecto', type: 'text', required: true, maxLength: 50, placeholder: 'PRJ-001' },
        { field: 'Name', label: 'Nombre', type: 'text', required: true, maxLength: 200 },
        { field: 'LedgerAccount', label: 'Cuenta Contable', type: 'text', maxLength: 50 },
        {
            field: 'ProjectStatus',
            label: 'Estado',
            type: 'select',
            required: true,
            options: [
                { value: 'true', text: 'Activo' },
                { value: 'false', text: 'Inactivo' }
            ]
        },
        { field: 'CreatedOn', label: 'Fecha Creación', type: 'datetime', readonly: true },
        { field: 'CreatedBy', label: 'Creado Por', type: 'text', readonly: true },
        { field: 'ModifiedOn', label: 'Última Modificación', type: 'datetime', readonly: true },
        { field: 'ModifiedBy', label: 'Modificado Por', type: 'text', readonly: true },
        { field: 'Observations', label: 'Observaciones', type: 'textarea', maxLength: 500 }
    ];

    let projectData: any = null;

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

    const renderField = (config: FieldConfig, value: any): string => {
        const fieldId = config.field;
        const fieldName = config.field;
        const labelClass = 'control-label col-md-3 col-sm-3 col-xs-12';
        const inputContainerClass = 'col-md-6 col-sm-6 col-xs-12';
        const requiredMark = config.required ? '<span class="required">*</span>' : '';
        const readonlyAttr = config.readonly ? 'readonly' : '';
        const requiredAttr = config.required ? 'required' : '';

        let inputHtml = '';
        let displayValue = value ?? '';

        switch (config.type) {
            case 'textarea':
                inputHtml = `<textarea id="${fieldId}" name="${fieldName}" class="form-control" rows="3" maxlength="${config.maxLength || 500}" ${readonlyAttr} ${requiredAttr}>${displayValue}</textarea>`;
                break;

            case 'select':
                const options = config.options || [];
                const optionsHtml = options.map(opt =>
                    `<option value="${opt.value}" ${displayValue.toString() === opt.value ? 'selected' : ''}>${opt.text}</option>`
                ).join('');
                inputHtml = `<select id="${fieldId}" name="${fieldName}" class="form-control" ${readonlyAttr ? 'disabled' : ''} ${requiredAttr}>${optionsHtml}</select>`;
                break;

            case 'checkbox':
                const checked = displayValue === true || displayValue === 'true' ? 'checked' : '';
                inputHtml = `<input type="checkbox" id="${fieldId}" name="${fieldName}" class="flat" ${checked} ${readonlyAttr ? 'disabled' : ''}>`;
                break;

            case 'datetime':
                if (displayValue && typeof displayValue === 'string' && /^\d{4}-\d{2}-\d{2}T/.test(displayValue)) {
                    displayValue = new Date(displayValue).toLocaleString('es-DO');
                }
                inputHtml = `<input type="text" id="${fieldId}" name="${fieldName}" class="form-control" value="${displayValue}" ${readonlyAttr} ${requiredAttr}>`;
                break;

            case 'date':
                if (displayValue && typeof displayValue === 'string' && /^\d{4}-\d{2}-\d{2}T/.test(displayValue)) {
                    displayValue = displayValue.split('T')[0];
                }
                inputHtml = `<input type="date" id="${fieldId}" name="${fieldName}" class="form-control" value="${displayValue}" ${readonlyAttr} ${requiredAttr}>`;
                break;

            case 'number':
                inputHtml = `<input type="number" id="${fieldId}" name="${fieldName}" class="form-control" value="${displayValue}" ${readonlyAttr} ${requiredAttr}>`;
                break;

            default: // text
                inputHtml = `<input type="text" id="${fieldId}" name="${fieldName}" class="form-control" maxlength="${config.maxLength || 255}" value="${displayValue}" placeholder="${config.placeholder || ''}" ${readonlyAttr} ${requiredAttr}>`;
                break;
        }

        const helpTextHtml = config.helpText ? `<span class="help-block">${config.helpText}</span>` : '';

        return `
            <div class="form-group">
                <label class="${labelClass}" for="${fieldId}">
                    ${config.label} ${requiredMark}
                </label>
                <div class="${inputContainerClass}">
                    ${inputHtml}
                    ${helpTextHtml}
                </div>
            </div>
        `;
    };

    const loadProjectData = async (): Promise<void> => {
        if (isNew) {
            renderForm({});
            return;
        }

        try {
            const url = `${apiBase}/Projects/${recId}`;
            projectData = await fetchJson(url);
            renderForm(projectData);
        } catch (error) {
            (w as any).ALERTS.error('Error al cargar los datos del proyecto', 'Error');
            renderForm({});
        }
    };

    const renderForm = (data: any): void => {
        const container = $('#dynamic-fields-container');
        container.empty();

        fieldDefinitions.forEach(config => {
            const value = data[config.field];
            const fieldHtml = renderField(config, value);
            container.append(fieldHtml);
        });

        // Inicializar iCheck para checkboxes
        if ($.fn.iCheck) {
            $('.flat').iCheck({
                checkboxClass: 'icheckbox_flat-green'
            });
        }
    };

    const getFormData = (): any => {
        const formData: any = {};

        fieldDefinitions.forEach(config => {
            const field = config.field;
            const $input = $(`#${field}`);

            // Saltar campos readonly (auditoría)
            if (config.readonly) {
                return;
            }

            if ($input.length) {
                if (config.type === 'checkbox') {
                    formData[field] = $input.is(':checked');
                } else if (config.type === 'select' && field === 'ProjectStatus') {
                    // Convertir string "true"/"false" a boolean
                    const val = $input.val();
                    formData[field] = val === 'true';
                } else if (config.type === 'number') {
                    const val = $input.val();
                    formData[field] = val ? parseFloat(val) : null;
                } else {
                    const val = $input.val();
                    formData[field] = val || null;
                }
            }
        });

        return formData;
    };

    const saveProject = async (): Promise<void> => {
        const formData = getFormData();

        try {
            const url = isNew ? `${apiBase}/Projects` : `${apiBase}/Projects/${recId}`;
            const method = isNew ? 'POST' : 'PUT';

            // Payload DIRECTO sin wrapper request
            const payload = {
                ProjectCode: formData.ProjectCode,
                Name: formData.Name,
                LedgerAccount: formData.LedgerAccount || null,
                ProjectStatus: formData.ProjectStatus,
                Observations: formData.Observations || null
            };

            console.log('FormData capturado:', formData); // Debug
            console.log('Enviando payload:', payload); // Debug

            await fetchJson(url, {
                method: method,
                body: JSON.stringify(payload)
            });

            (w as any).ALERTS.ok(
                isNew ? 'Proyecto creado exitosamente' : 'Proyecto actualizado exitosamente',
                'Éxito'
            );

            setTimeout(() => {
                window.location.href = '/Project/LP_Projects';
            }, 1500);

        } catch (error: any) {
            console.error('Error al guardar:', error);
            let errorMessage = 'Error al guardar el proyecto';

            try {
                const errorData = JSON.parse(error.message);
                if (errorData.errors) {
                    const errorsArray: string[] = [];
                    for (const key in errorData.errors) {
                        if (errorData.errors.hasOwnProperty(key)) {
                            const errList = errorData.errors[key];
                            if (Array.isArray(errList)) {
                                for (let i = 0; i < errList.length; i++) {
                                    errorsArray.push(errList[i]);
                                }
                            } else {
                                errorsArray.push(errList);
                            }
                        }
                    }
                    errorMessage = errorsArray.join(', ');
                }
            } catch {
                errorMessage = error.message || errorMessage;
            }

            (w as any).ALERTS.error(errorMessage, 'Error');
        }
    };

    $('#btn-save').on('click', async () => {
        const form = document.getElementById('frm-project') as HTMLFormElement;
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }
        await saveProject();
    });

    // Inicializar
    $(async function () {
        try {
            await loadProjectData();
        } catch (error) {
            (w as any).ALERTS.error('Error al inicializar el formulario', 'Error');
        }
    });
})();