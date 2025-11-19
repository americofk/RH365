// ============================================================================
// Archivo: position-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Positions/position-form.ts
// Descripción: 
//   - Formulario dinámico para Crear/Editar Posiciones
//   - Tab General: Campos de negocio en LAYOUT DE 2 COLUMNAS
//   - Tab Auditoría: Campos ISO 27001 en 1 columna
//   - Renderizado separado para cada tab
//   - Validación cliente + servidor
//   - Integración con API REST (/api/Positions)
// Estándar: ISO 27001 - Control A.14.2.5 (Principios de ingeniería de sistemas seguros)
// ============================================================================

(function () {
    // ========================================================================
    // CONFIGURACIÓN GLOBAL Y CONTEXTO
    // ========================================================================
    const w: any = window;
    const d: Document = document;
    const $: any = w.jQuery || w.$;

    const apiBase: string = w.RH365.urls.apiBase;
    const pageContainer = d.querySelector("#position-form-page");

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
    }

    // ========================================================================
    // DEFINICIÓN DE CAMPOS - TAB GENERAL (Campos de Negocio en 2 COLUMNAS)
    // ========================================================================
    const businessFields: FieldConfig[] = [
        // COLUMNA IZQUIERDA
        {
            field: 'PositionCode',
            label: 'Código Posición',
            type: 'text',
            required: true,
            maxLength: 20,
            placeholder: 'POS-001',
            column: 'left'
        },
        {
            field: 'PositionName',
            label: 'Nombre Posición',
            type: 'text',
            required: true,
            maxLength: 50,
            column: 'left'
        },
        {
            field: 'Description',
            label: 'Descripción',
            type: 'textarea',
            maxLength: 200,
            column: 'left'
        },
        {
            field: 'Observations',
            label: 'Observaciones',
            type: 'textarea',
            maxLength: 500,
            column: 'left'
        },

        // COLUMNA DERECHA
        {
            field: 'DepartmentRefRecID',
            label: 'Departamento',
            type: 'select',
            required: true,
            options: [],
            column: 'right'
        },
        {
            field: 'JobRefRecID',
            label: 'Puesto',
            type: 'select',
            required: true,
            options: [],
            column: 'right'
        },
        {
            field: 'StartDate',
            label: 'Fecha Inicio',
            type: 'date',
            required: true,
            column: 'right'
        },
        {
            field: 'EndDate',
            label: 'Fecha Fin',
            type: 'date',
            column: 'right'
        },
        {
            field: 'IsVacant',
            label: 'Vacante',
            type: 'select',
            required: true,
            options: [
                { value: 'true', text: 'Sí' },
                { value: 'false', text: 'No' }
            ],
            column: 'right'
        },
        {
            field: 'PositionStatus',
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
    // DEFINICIÓN DE CAMPOS - TAB AUDITORÍA (SOLO ISO 27001)
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

    let positionData: any = null;

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
    // CARGA DE CATÁLOGOS (DEPARTMENTS Y JOBS)
    // ========================================================================

    /**
     * Cargar departamentos desde el API y poblar el campo DepartmentRefRecID
     */
    const loadDepartments = async (): Promise<void> => {
        try {
            const url = `${apiBase}/Departments?pageNumber=1&pageSize=100`;
            const response = await fetchJson(url);

            // Manejar tanto Array directo como Object con Data
            let departmentsArray: any[] = [];
            if (Array.isArray(response)) {
                departmentsArray = response;
            } else if (response?.Data && Array.isArray(response.Data)) {
                departmentsArray = response.Data;
            }

            if (departmentsArray.length > 0) {
                const departmentField = businessFields.find(f => f.field === 'DepartmentRefRecID');
                if (departmentField) {
                    departmentField.options = departmentsArray.map((dept: any) => ({
                        value: dept.RecID.toString(),
                        text: `${dept.DepartmentCode} - ${dept.Name}`
                    }));
                }
            }
        } catch (error) {
            console.error('Error cargando departamentos:', error);
            (w as any).ALERTS.warn('No se pudieron cargar los departamentos', 'Advertencia');
        }
    };

    /**
     * Cargar puestos (jobs) desde el API y poblar el campo JobRefRecID
     */
    const loadJobs = async (): Promise<void> => {
        try {
            const url = `${apiBase}/Jobs?pageNumber=1&pageSize=100`;
            const response = await fetchJson(url);

            // Manejar tanto Array directo como Object con Data
            let jobsArray: any[] = [];
            if (Array.isArray(response)) {
                jobsArray = response;
            } else if (response?.Data && Array.isArray(response.Data)) {
                jobsArray = response.Data;
            }

            if (jobsArray.length > 0) {
                const jobField = businessFields.find(f => f.field === 'JobRefRecID');
                if (jobField) {
                    jobField.options = jobsArray.map((job: any) => ({
                        value: job.RecID.toString(),
                        text: `${job.JobCode} - ${job.Name}`
                    }));
                }
            }
        } catch (error) {
            console.error('Error cargando jobs:', error);
            (w as any).ALERTS.warn('No se pudieron cargar los puestos', 'Advertencia');
        }
    };

    // ========================================================================
    // RENDERIZADO DE CAMPOS
    // ========================================================================

    const renderField = (config: FieldConfig, value: any, is2Column: boolean = false): string => {
        const fieldId = config.field;
        const fieldName = config.field;

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
                let optionsHtml = '';

                if (config.field === 'DepartmentRefRecID' || config.field === 'JobRefRecID') {
                    optionsHtml = '<option value="">-- Seleccione --</option>';
                    optionsHtml += options.map(opt => {
                        const isSelected = displayValue && displayValue.toString() === opt.value;
                        return `<option value="${opt.value}" ${isSelected ? 'selected' : ''}>${opt.text}</option>`;
                    }).join('');
                } else {
                    optionsHtml = options.map(opt =>
                        `<option value="${opt.value}" ${displayValue.toString() === opt.value ? 'selected' : ''}>${opt.text}</option>`
                    ).join('');
                }

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

            default:
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
    // CARGA DE DATOS DE LA POSICIÓN
    // ========================================================================

    const loadPositionData = async (): Promise<void> => {
        await loadDepartments();
        await loadJobs();

        if (isNew) {
            renderBusinessForm({});
            renderAuditForm({});
            return;
        }

        try {
            const url = `${apiBase}/Positions/${recId}`;
            positionData = await fetchJson(url);

            renderBusinessForm(positionData);
            renderAuditForm(positionData);
        } catch (error) {
            console.error('Error al cargar posición:', error);
            (w as any).ALERTS.error('Error al cargar los datos de la posición', 'Error');

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
                    Los campos de auditoría se generarán automáticamente después de guardar la posición.
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
    // CAPTURA DE DATOS DEL FORMULARIO
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
                } else if (config.type === 'select' && (field === 'PositionStatus' || field === 'IsVacant')) {
                    const val = $input.val();
                    formData[field] = val === 'true';
                } else if (config.type === 'select' && (field === 'DepartmentRefRecID' || field === 'JobRefRecID')) {
                    const val = $input.val();
                    formData[field] = val ? parseInt(val, 10) : null;
                } else if (config.type === 'number') {
                    const val = $input.val();
                    formData[field] = val ? parseFloat(val) : null;
                } else if (config.type === 'date') {
                    const val = $input.val();
                    formData[field] = val || null;
                } else {
                    const val = $input.val();
                    formData[field] = val || null;
                }
            }
        });

        return formData;
    };

    // ========================================================================
    // GUARDADO DE POSICIÓN
    // ========================================================================

    const savePosition = async (): Promise<void> => {
        const formData = getFormData();

        try {
            const url = isNew ? `${apiBase}/Positions` : `${apiBase}/Positions/${recId}`;
            const method = isNew ? 'POST' : 'PUT';

            const payload = {
                PositionCode: formData.PositionCode,
                PositionName: formData.PositionName,
                IsVacant: formData.IsVacant,
                DepartmentRefRecID: formData.DepartmentRefRecID,
                JobRefRecID: formData.JobRefRecID,
                NotifyPositionRefRecID: formData.NotifyPositionRefRecID || null, // ← CORREGIDO: null en lugar de 0
                PositionStatus: formData.PositionStatus,
                StartDate: formData.StartDate,
                EndDate: formData.EndDate || null,
                Description: formData.Description || null,
                Observations: formData.Observations || null
            };

            await fetchJson(url, {
                method: method,
                body: JSON.stringify(payload)
            });

            (w as any).ALERTS.ok(
                isNew ? 'Posición creada exitosamente' : 'Posición actualizada exitosamente',
                'Éxito'
            );

            //setTimeout(() => {
            //    window.location.href = '/Position/LP_Positions';
            //}, 1500);

        } catch (error: any) {
            console.error('Error al guardar:', error);
            let errorMessage = 'Error al guardar la posición';

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
        const form = document.getElementById('frm-position') as HTMLFormElement;

        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        await savePosition();
    });

    // ========================================================================
    // INICIALIZACIÓN
    // ========================================================================

    $(async function () {
        try {
            await loadPositionData();
        } catch (error) {
            (w as any).ALERTS.error('Error al inicializar el formulario', 'Error');
        }
    });
})();