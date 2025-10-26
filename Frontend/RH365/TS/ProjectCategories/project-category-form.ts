// ============================================================================
// Archivo: project-category-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/ProjectCategories/project-category-form.ts
// Descripción: 
//   - Formulario dinámico para Crear/Editar Categorías de Proyecto
//   - Tab General: Campos de negocio en LAYOUT DE 2 COLUMNAS
//   - ComboBox para selección de Proyecto (ProjectRefRecID)
//   - Tab Auditoría: Campos ISO 27001 en 1 columna
//   - Labels a la izquierda de los campos
// ============================================================================

(function () {
    // ========================================================================
    // CONFIGURACIÓN GLOBAL Y CONTEXTO
    // ========================================================================
    const w: any = window;
    const d: Document = document;
    const $: any = w.jQuery || w.$;

    const apiBase: string = w.RH365.urls.apiBase;
    const pageContainer = d.querySelector("#project-category-form-page");

    if (!pageContainer) return;

    const token: string = pageContainer.getAttribute("data-token") || "";
    const dataareaId: string = pageContainer.getAttribute("data-dataarea") || "DAT";
    const userRefRecID: number = parseInt(pageContainer.getAttribute("data-user") || "0", 10);
    const recId: number = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    const isNew: boolean = pageContainer.getAttribute("data-isnew") === "true";

    // ========================================================================
    // INTERFACES Y TIPOS
    // ========================================================================

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
        column?: 'left' | 'right';
        loadOptionsFrom?: string; // URL para cargar opciones dinámicamente
    }

    // ========================================================================
    // DEFINICIÓN DE CAMPOS - TAB GENERAL
    // ========================================================================
    const businessFields: FieldConfig[] = [
        // COLUMNA IZQUIERDA
        {
            field: 'CategoryName',
            label: 'Nombre Categoría',
            type: 'text',
            required: true,
            maxLength: 200,
            placeholder: 'Ej: Desarrollo Software',
            column: 'left'
        },
        {
            field: 'LedgerAccount',
            label: 'Cuenta Contable',
            type: 'text',
            maxLength: 50,
            placeholder: '1.01.001',
            column: 'left'
        },

        // COLUMNA DERECHA
        {
            field: 'ProjectRefRecID',
            label: 'Proyecto',
            type: 'select',
            required: true,
            options: [], // Se cargan dinámicamente
            loadOptionsFrom: 'Projects/enabled',
            column: 'right'
        },
        {
            field: 'ProjectCategoryStatus',
            label: 'Estado',
            type: 'select',
            required: true,
            options: [
                { value: 'true', text: 'Activo' },
                { value: 'false', text: 'Inactivo' }
            ],
            column: 'right'
        }
    ];

    // ========================================================================
    // DEFINICIÓN DE CAMPOS - TAB AUDITORÍA
    // ========================================================================
    const auditFields: FieldConfig[] = [
        {
            field: 'RecID',
            label: 'RecID (Clave Primaria)',
            type: 'number',
            readonly: true,
        },
        {
            field: 'ID',
            label: 'ID Sistema',
            type: 'text',
            readonly: true,
        },
        {
            field: 'DataareaID',
            label: 'Empresa (DataareaID)',
            type: 'text',
            readonly: true,
        },
        {
            field: 'CreatedBy',
            label: 'Creado Por',
            type: 'text',
            readonly: true,
        },
        {
            field: 'CreatedOn',
            label: 'Fecha de Creación',
            type: 'datetime',
            readonly: true,
        },
        {
            field: 'ModifiedBy',
            label: 'Modificado Por',
            type: 'text',
            readonly: true,
        },
        {
            field: 'ModifiedOn',
            label: 'Fecha de Última Modificación',
            type: 'datetime',
            readonly: true,
        }
    ];

    let categoryData: any = null;
    let projectsOptions: { value: string; text: string }[] = [];

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
    // CARGA DE OPCIONES DINÁMICAS (PROYECTOS)
    // ========================================================================

    const loadProjects = async (): Promise<void> => {
        try {
            const url = `${apiBase}/Projects?pageNumber=1&pageSize=100`;
            const response = await fetchJson(url);
            
            // Si viene paginado, extraer Data
            const projects = response?.Data || response || [];

            projectsOptions = projects
                .filter((p: any) => p.ProjectStatus === true) // Solo proyectos activos
                .map((p: any) => ({
                    value: p.RecID.toString(),
                    text: `${p.ProjectCode} - ${p.Name}`
                }));

            // Actualizar opciones del campo ProjectRefRecID
            const projectField = businessFields.find(f => f.field === 'ProjectRefRecID');
            if (projectField) {
                projectField.options = projectsOptions;
            }
        } catch (error) {
            (w as any).ALERTS.error('Error al cargar lista de proyectos', 'Error');
        }
    };

    // ========================================================================
    // RENDERIZADO DE CAMPOS
    // ========================================================================

    const renderField = (config: FieldConfig, value: any, is2Column: boolean = false): string => {
        const fieldId = config.field;
        const fieldName = config.field;

        // Labels SIEMPRE a la izquierda
        const labelClass = is2Column
            ? 'control-label col-md-4 col-sm-4 col-xs-12'
            : 'control-label col-md-3 col-sm-3 col-xs-12';

        const inputContainerClass = is2Column
            ? 'col-md-8 col-sm-8 col-xs-12'
            : 'col-md-6 col-sm-6 col-xs-12';

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
                    displayValue = new Date(displayValue).toLocaleString('es-DO', {
                        year: 'numeric',
                        month: '2-digit',
                        day: '2-digit',
                        hour: '2-digit',
                        minute: '2-digit',
                        second: '2-digit'
                    });
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

    // ========================================================================
    // CARGA DE DATOS
    // ========================================================================

    const loadCategoryData = async (): Promise<void> => {
        // Primero cargar proyectos
        await loadProjects();

        if (isNew) {
            renderBusinessForm({});
            renderAuditForm({});
            return;
        }

        try {
            const url = `${apiBase}/ProjectCategories/${recId}`;
            categoryData = await fetchJson(url);

            renderBusinessForm(categoryData);
            renderAuditForm(categoryData);
        } catch (error) {
            (w as any).ALERTS.error('Error al cargar los datos de la categoría', 'Error');
            renderBusinessForm({});
            renderAuditForm({});
        }
    };

    // ========================================================================
    // RENDERIZADO DE FORMULARIOS
    // ========================================================================

    const renderBusinessForm = (data: any): void => {
        const containerLeft = $('#dynamic-fields-col-left');
        const containerRight = $('#dynamic-fields-col-right');

        containerLeft.empty();
        containerRight.empty();

        businessFields
            .filter(config => config.column === 'left')
            .forEach(config => {
                const value = data[config.field];
                const fieldHtml = renderField(config, value, true);
                containerLeft.append(fieldHtml);
            });

        businessFields
            .filter(config => config.column === 'right')
            .forEach(config => {
                const value = data[config.field];
                const fieldHtml = renderField(config, value, true);
                containerRight.append(fieldHtml);
            });

        if ($.fn.iCheck) {
            $('.flat').iCheck({
                checkboxClass: 'icheckbox_flat-green'
            });
        }
    };

    const renderAuditForm = (data: any): void => {
        const container = $('#audit-fields-container');
        container.empty();

        if (isNew) {
            container.html(`
                <div class="alert alert-warning" role="alert">
                    <i class="fa fa-info-circle"></i>
                    <strong>Modo Creación:</strong> 
                    Los campos de auditoría se generarán automáticamente después de guardar la categoría.
                </div>
            `);
            return;
        }

        auditFields.forEach(config => {
            const value = data[config.field];
            const fieldHtml = renderField(config, value, false);
            container.append(fieldHtml);
        });
    };

    // ========================================================================
    // CAPTURA DE DATOS
    // ========================================================================

    const getFormData = (): any => {
        const formData: any = {};

        businessFields.forEach(config => {
            const field = config.field;
            const $input = $(`#${field}`);

            if (config.readonly) {
                return;
            }

            if ($input.length) {
                if (config.type === 'checkbox') {
                    formData[field] = $input.is(':checked');
                } else if (config.type === 'select' && field === 'ProjectCategoryStatus') {
                    const val = $input.val();
                    formData[field] = val === 'true';
                } else if (config.type === 'select' && field === 'ProjectRefRecID') {
                    const val = $input.val();
                    formData[field] = val ? parseInt(val) : null;
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

    // ========================================================================
    // GUARDADO
    // ========================================================================

    const saveCategory = async (): Promise<void> => {
        const formData = getFormData();

        try {
            const url = isNew ? `${apiBase}/ProjectCategories` : `${apiBase}/ProjectCategories/${recId}`;
            const method = isNew ? 'POST' : 'PUT';

            const payload = {
                CategoryName: formData.CategoryName,
                LedgerAccount: formData.LedgerAccount || null,
                ProjectRefRecID: formData.ProjectRefRecID,
                ProjectCategoryStatus: formData.ProjectCategoryStatus
            };

            console.log('FormData capturado:', formData);
            console.log('Enviando payload:', payload);

            await fetchJson(url, {
                method: method,
                body: JSON.stringify(payload)
            });

            (w as any).ALERTS.ok(
                isNew ? 'Categoría creada exitosamente' : 'Categoría actualizada exitosamente',
                'Éxito'
            );

            setTimeout(() => {
                window.location.href = '/ProjectCategory/LP_ProjectCategories';
            }, 1500);

        } catch (error: any) {
            console.error('Error al guardar:', error);
            let errorMessage = 'Error al guardar la categoría';

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
                } else if (errorData.title) {
                    errorMessage = errorData.title;
                }
            } catch {
                errorMessage = error.message || errorMessage;
            }

            (w as any).ALERTS.error(errorMessage, 'Error');
        }
    };

    // ========================================================================
    // EVENT HANDLERS
    // ========================================================================

    $('#btn-save').on('click', async () => {
        const form = document.getElementById('frm-project-category') as HTMLFormElement;

        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        await saveCategory();
    });

    // ========================================================================
    // INICIALIZACIÓN
    // ========================================================================

    $(async function () {
        try {
            await loadCategoryData();
        } catch (error) {
            (w as any).ALERTS.error('Error al inicializar el formulario', 'Error');
        }
    });
})();
