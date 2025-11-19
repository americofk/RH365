// ============================================================================
// Archivo: earning-code-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/EarningCodes/earning-code-form.ts
// Descripción: 
//   - Formulario dinámico para Crear/Editar Códigos de Nómina
//   - Tab General: Campos de negocio en LAYOUT DE 2 COLUMNAS
//   - Tab Auditoría: Campos ISO 27001 en 1 columna
//   - Renderizado separado para cada tab
//   - Validación cliente + servidor
//   - Integración con API REST (/api/EarningCodes)
//   - Carga de entidades relacionadas (Proyectos, Departamentos)
// Estándar: ISO 27001 - Gestión segura de información de nómina
// ============================================================================

(function () {
    const w: any = window;
    const d: Document = document;
    const $: any = w.jQuery || w.$;

    const apiBase: string = w.RH365.urls.apiBase;
    const pageContainer = d.querySelector("#earning-code-form-page");

    if (!pageContainer) return;

    const token: string = pageContainer.getAttribute("data-token") || "";
    const dataareaId: string = pageContainer.getAttribute("data-dataarea") || "DAT";
    const userRefRecID: number = parseInt(pageContainer.getAttribute("data-user") || "0", 10);
    const recId: number = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    const isNew: boolean = pageContainer.getAttribute("data-isnew") === "true";

    interface FieldConfig {
        field: string;
        label: string;
        type: 'text' | 'number' | 'date' | 'datetime' | 'time' | 'textarea' | 'select' | 'checkbox';
        required?: boolean;
        maxLength?: number;
        options?: { value: string; text: string }[];
        placeholder?: string;
        helpText?: string;
        readonly?: boolean;
        column?: 'left' | 'right';
        step?: string;
        min?: number;
        max?: number;
    }

    const businessFields: FieldConfig[] = [
        // COLUMNA IZQUIERDA
        {
            field: 'Name',
            label: 'Nombre',
            type: 'text',
            required: true,
            maxLength: 200,
            placeholder: 'Salario Base',
            column: 'left'
        },
        {
            field: 'Description',
            label: 'Descripción',
            type: 'textarea',
            maxLength: 500,
            placeholder: 'Descripción del código de nómina',
            column: 'left'
        },
        {
            field: 'ValidFrom',
            label: 'Vigencia Desde',
            type: 'datetime',
            required: true,
            column: 'left'
        },
        {
            field: 'ValidTo',
            label: 'Vigencia Hasta',
            type: 'datetime',
            required: true,
            column: 'left'
        },
        {
            field: 'LedgerAccount',
            label: 'Cuenta Contable',
            type: 'text',
            maxLength: 50,
            placeholder: '5101-001',
            column: 'left'
        },
        {
            field: 'IndexBase',
            label: 'Índice Base',
            type: 'select',
            required: true,
            options: [
                { value: '0', text: 'Hora' },
                { value: '1', text: 'Período de pago' },
                { value: '2', text: 'Mensual' },
                { value: '3', text: 'Anual' },
                { value: '4', text: 'Monto fijo' },
                { value: '5', text: 'Retroactivo' },
                { value: '6', text: 'Índice salarial estándar' },
                { value: '7', text: 'Porcentaje de ganancia' },
                { value: '8', text: 'Horas de trabajo' }
            ],
            column: 'left'
        },
        {
            field: 'MultiplyAmount',
            label: 'Multiplicador',
            type: 'number',
            required: true,
            step: '0.01',
            min: 0,
            column: 'left'
        },
        {
            field: 'IsTSS',
            label: 'Aplica TSS',
            type: 'checkbox',
            column: 'left'
        },
        {
            field: 'IsISR',
            label: 'Aplica ISR',
            type: 'checkbox',
            column: 'left'
        },
        {
            field: 'IsExtraHours',
            label: 'Horas Extra',
            type: 'checkbox',
            column: 'left'
        },
        {
            field: 'IsRoyaltyPayroll',
            label: 'Regalía Pascual',
            type: 'checkbox',
            column: 'left'
        },

        // COLUMNA DERECHA
        {
            field: 'EarningCodeStatus',
            label: 'Estado',
            type: 'select',
            required: true,
            options: [
                { value: 'true', text: 'Activo' },
                { value: 'false', text: 'Inactivo' }
            ],
            column: 'right'
        },
        {
            field: 'ProjectRefRecID',
            label: 'Proyecto',
            type: 'select',
            options: [{ value: '', text: '-- Seleccione --' }],
            column: 'right'
        },
        {
            field: 'DepartmentRefRecID',
            label: 'Departamento',
            type: 'select',
            options: [{ value: '', text: '-- Seleccione --' }],
            column: 'right'
        },
        {
            field: 'ProjCategoryRefRecID',
            label: 'Categoría de Proyecto',
            type: 'select',
            options: [{ value: '', text: '-- Seleccione --' }],
            column: 'right'
        },
        {
            field: 'WorkFrom',
            label: 'Hora de Inicio',
            type: 'time',
            placeholder: '08:00',
            column: 'right'
        },
        {
            field: 'WorkTo',
            label: 'Hora de Fin',
            type: 'time',
            placeholder: '17:00',
            column: 'right'
        },
        {
            field: 'Observations',
            label: 'Observaciones',
            type: 'textarea',
            maxLength: 500,
            column: 'right'
        },

        // Checkboxes finales - Columna derecha
        {
            field: 'IsUseDGT',
            label: 'Usa DGT',
            type: 'checkbox',
            column: 'right'
        },
        {
            field: 'IsHoliday',
            label: 'Es Feriado',
            type: 'checkbox',
            column: 'right'
        }
    ];

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

    let earningCodeData: any = null;
    let projectsOptions: any[] = [];
    let departmentsOptions: any[] = [];
    let categoriesOptions: any[] = [];

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
                inputHtml = `<textarea id="${fieldId}" name="${fieldName}" class="form-control" rows="3" maxlength="${config.maxLength || 500}" ${readonlyAttr} ${requiredAttr} placeholder="${config.placeholder || ''}">${displayValue}</textarea>`;
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
                    displayValue = displayValue.substring(0, 16);
                }
                inputHtml = `<input type="datetime-local" id="${fieldId}" name="${fieldName}" class="form-control" value="${displayValue}" ${readonlyAttr} ${requiredAttr}>`;
                break;

            case 'date':
                if (displayValue && typeof displayValue === 'string' && /^\d{4}-\d{2}-\d{2}T/.test(displayValue)) {
                    displayValue = displayValue.split('T')[0];
                }
                inputHtml = `<input type="date" id="${fieldId}" name="${fieldName}" class="form-control" value="${displayValue}" ${readonlyAttr} ${requiredAttr}>`;
                break;

            case 'time':
                if (displayValue && typeof displayValue === 'string' && /^\d{2}:\d{2}:\d{2}/.test(displayValue)) {
                    displayValue = displayValue.substring(0, 5);
                }
                inputHtml = `<input type="time" id="${fieldId}" name="${fieldName}" class="form-control" value="${displayValue}" ${readonlyAttr} ${requiredAttr} placeholder="${config.placeholder || ''}">`;
                break;

            case 'number':
                const stepAttr = config.step ? `step="${config.step}"` : '';
                const minAttr = config.min !== undefined ? `min="${config.min}"` : '';
                const maxAttr = config.max !== undefined ? `max="${config.max}"` : '';
                inputHtml = `<input type="number" id="${fieldId}" name="${fieldName}" class="form-control" value="${displayValue}" ${stepAttr} ${minAttr} ${maxAttr} ${readonlyAttr} ${requiredAttr}>`;
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

    const loadRelatedEntities = async (): Promise<void> => {
        try {
            const [projectsResp, departmentsResp] = await Promise.all([
                fetchJson(`${apiBase}/Projects?skip=0&take=1000`),
                fetchJson(`${apiBase}/Departments?skip=0&take=1000`)
            ]);

            // Manejar respuesta de proyectos (array directo o con propiedad Data)
            if (Array.isArray(projectsResp)) {
                projectsOptions = projectsResp.map((p: any) => ({
                    value: p.RecID.toString(),
                    text: p.Name || p.ProjectCode
                }));
            } else if (projectsResp?.Data) {
                projectsOptions = projectsResp.Data.map((p: any) => ({
                    value: p.RecID.toString(),
                    text: p.Name || p.ProjectCode
                }));
            }

            // Manejar respuesta de departamentos (array directo o con propiedad Data)
            if (Array.isArray(departmentsResp)) {
                departmentsOptions = departmentsResp.map((d: any) => ({
                    value: d.RecID.toString(),
                    text: d.Name || d.DepartmentCode
                }));
            } else if (departmentsResp?.Data) {
                departmentsOptions = departmentsResp.Data.map((d: any) => ({
                    value: d.RecID.toString(),
                    text: d.Name || d.DepartmentCode
                }));
            }

            // Actualizar opciones de Proyecto
            const projectField = businessFields.find(f => f.field === 'ProjectRefRecID');
            if (projectField && projectField.options) {
                projectField.options = [
                    { value: '', text: '-- Seleccione --' },
                    ...projectsOptions
                ];
            }

            // Actualizar opciones de Departamento
            const deptField = businessFields.find(f => f.field === 'DepartmentRefRecID');
            if (deptField && deptField.options) {
                deptField.options = [
                    { value: '', text: '-- Seleccione --' },
                    ...departmentsOptions
                ];
            }

            console.log('Proyectos cargados:', projectsOptions.length);
            console.log('Departamentos cargados:', departmentsOptions.length);

            // Cargar categorías inicial (sin filtro)
            await loadCategoryOptions();

        } catch (error) {
            console.error('Error cargando entidades relacionadas:', error);
        }
    };

    const loadCategoryOptions = async (projectRecID?: number): Promise<void> => {
        try {
            const url = `${apiBase}/ProjectCategories?skip=0&take=1000`;
            const response = await fetchJson(url);
            const data = Array.isArray(response) ? response : (response?.Data || response?.data || []);
            
            let filteredData = data;
            if (projectRecID) {
                // Filtrar categorías por proyecto usando diferentes nombres de campo posibles
                filteredData = data.filter((item: any) => 
                    item.ProjectRefRecID === projectRecID || 
                    item.ProjectRecID === projectRecID ||
                    item.ProjectId === projectRecID
                );
            }
            
            categoriesOptions = filteredData.map((item: any) => ({
                value: item.RecID?.toString() || '',
                text: item.Name || item.CategoryName || item.CategoryCode || ''
            }));

            console.log(`Categorías cargadas${projectRecID ? ' (filtradas)' : ''}:`, categoriesOptions.length);

        } catch (error) {
            console.error('Error cargando categorías:', error);
            categoriesOptions = [];
        }
    };

    const updateCategoryDropdown = async (projectRecID?: number): Promise<void> => {
        const currentCategoryValue = $('#ProjCategoryRefRecID').val();
        
        await loadCategoryOptions(projectRecID);
        
        const categoryField = businessFields.find(f => f.field === 'ProjCategoryRefRecID');
        
        if (categoryField) {
            categoryField.options = [
                { value: '', text: '-- Seleccione --' },
                ...categoriesOptions
            ];

            const optionsHtml = categoryField.options.map(opt =>
                `<option value="${opt.value}">${opt.text}</option>`
            ).join('');
            
            const $categorySelect = $('#ProjCategoryRefRecID');
            $categorySelect.html(optionsHtml);
            
            // Mantener el valor si existe en las nuevas opciones
            const valueExists = categoriesOptions.some(opt => opt.value === currentCategoryValue);
            if (valueExists && currentCategoryValue) {
                $categorySelect.val(currentCategoryValue);
            } else {
                $categorySelect.val('');
            }
        }
    };

    const loadEarningCodeData = async (): Promise<void> => {
        await loadRelatedEntities();

        if (isNew) {
            renderBusinessForm({});
            renderAuditForm({});
            return;
        }

        try {
            const url = `${apiBase}/EarningCodes/${recId}`;
            earningCodeData = await fetchJson(url);

            renderBusinessForm(earningCodeData);
            renderAuditForm(earningCodeData);
        } catch (error) {
            (w as any).ALERTS.error('Error al cargar los datos del código de nómina', 'Error');
            renderBusinessForm({});
            renderAuditForm({});
        }
    };

    const renderBusinessForm = (data: any): void => {
        const containerLeft = $('#dynamic-fields-col-left');
        const containerRight = $('#dynamic-fields-col-right');

        containerLeft.empty();
        containerRight.empty();

        // Primera parte - Campos principales columna izquierda
        const leftMainFields = businessFields.filter(config => 
            config.column === 'left' && config.type !== 'checkbox'
        );
        leftMainFields.forEach(config => {
            const value = data[config.field];
            const fieldHtml = renderField(config, value, true);
            containerLeft.append(fieldHtml);
        });

        // Primera parte - Campos principales columna derecha
        const rightMainFields = businessFields.filter(config => 
            config.column === 'right' && config.type !== 'checkbox'
        );
        rightMainFields.forEach(config => {
            const value = data[config.field];
            const fieldHtml = renderField(config, value, true);
            containerRight.append(fieldHtml);
        });

        // Separador visual
        containerLeft.append('<div class="ln_solid" style="margin: 20px 0;"></div>');
        containerRight.append('<div class="ln_solid" style="margin: 20px 0;"></div>');

        // Segunda parte - Checkboxes columna izquierda
        const leftCheckboxFields = businessFields.filter(config => 
            config.column === 'left' && config.type === 'checkbox'
        );
        leftCheckboxFields.forEach(config => {
            const value = data[config.field];
            const fieldHtml = renderField(config, value, true);
            containerLeft.append(fieldHtml);
        });

        // Segunda parte - Checkboxes columna derecha
        const rightCheckboxFields = businessFields.filter(config => 
            config.column === 'right' && config.type === 'checkbox'
        );
        rightCheckboxFields.forEach(config => {
            const value = data[config.field];
            const fieldHtml = renderField(config, value, true);
            containerRight.append(fieldHtml);
        });

        if ($.fn.iCheck) {
            $('.flat').iCheck({
                checkboxClass: 'icheckbox_flat-green'
            });
        }

        // Event handler para cascada Proyecto → Categoría
        $('#ProjectRefRecID').on('change', async function() {
            const projectRecID = $(this).val();
            const projectId = projectRecID && projectRecID !== '' ? parseInt(projectRecID as string, 10) : undefined;
            await updateCategoryDropdown(projectId);
        });

        // Inicializar cascada si hay proyecto seleccionado
        const initialProjectId = $('#ProjectRefRecID').val();
        if (initialProjectId && initialProjectId !== '') {
            const projectId = parseInt(initialProjectId as string, 10);
            updateCategoryDropdown(projectId);
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
                    Los campos de auditoría se generarán automáticamente después de guardar el código de nómina.
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
                } else if (config.type === 'select' && field === 'EarningCodeStatus') {
                    const val = $input.val();
                    formData[field] = val === 'true';
                } else if (config.type === 'select' && (field === 'ProjectRefRecID' || field === 'DepartmentRefRecID' || field === 'ProjCategoryRefRecID')) {
                    const val = $input.val();
                    formData[field] = val && val !== '' ? parseInt(val, 10) : null;
                } else if (config.type === 'select' && field === 'IndexBase') {
                    const val = $input.val();
                    formData[field] = val !== '' ? parseInt(val, 10) : 0;
                } else if (config.type === 'number') {
                    const val = $input.val();
                    formData[field] = val ? parseFloat(val) : null;
                } else if (config.type === 'datetime') {
                    const val = $input.val();
                    formData[field] = val ? new Date(val).toISOString() : null;
                } else if (config.type === 'time') {
                    const val = $input.val();
                    formData[field] = val ? `${val}:00` : null;
                } else {
                    const val = $input.val();
                    formData[field] = val || null;
                }
            }
        });

        return formData;
    };

    const saveEarningCode = async (): Promise<void> => {
        const formData = getFormData();

        try {
            const url = isNew ? `${apiBase}/EarningCodes` : `${apiBase}/EarningCodes/${recId}`;
            const method = isNew ? 'POST' : 'PUT';

            const payload = {
                Name: formData.Name,
                Description: formData.Description,
                IsTSS: formData.IsTSS || false,
                IsISR: formData.IsISR || false,
                ProjectRefRecID: formData.ProjectRefRecID,
                ProjCategoryRefRecID: formData.ProjCategoryRefRecID,
                ValidFrom: formData.ValidFrom,
                ValidTo: formData.ValidTo,
                IndexBase: formData.IndexBase,
                MultiplyAmount: formData.MultiplyAmount,
                LedgerAccount: formData.LedgerAccount,
                DepartmentRefRecID: formData.DepartmentRefRecID,
                EarningCodeStatus: formData.EarningCodeStatus,
                IsExtraHours: formData.IsExtraHours || false,
                IsRoyaltyPayroll: formData.IsRoyaltyPayroll || false,
                IsUseDGT: formData.IsUseDGT || false,
                IsHoliday: formData.IsHoliday || false,
                WorkFrom: formData.WorkFrom,
                WorkTo: formData.WorkTo,
                Observations: formData.Observations
            };

            console.log('FormData capturado:', formData);
            console.log('Enviando payload:', payload);

            await fetchJson(url, {
                method: method,
                body: JSON.stringify(payload)
            });

            (w as any).ALERTS.ok(
                isNew ? 'Código de nómina creado exitosamente' : 'Código de nómina actualizado exitosamente',
                'Éxito'
            );

            //setTimeout(() => {
            //    window.location.href = '/EarningCode/LP_EarningCodes';
            //}, 1500);

        } catch (error: any) {
            console.error('Error al guardar:', error);
            let errorMessage = 'Error al guardar el código de nómina';

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

    $('#btn-save').on('click', async () => {
        const form = document.getElementById('frm-earning-code') as HTMLFormElement;

        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        await saveEarningCode();
    });

    $(async function () {
        try {
            await loadEarningCodeData();
        } catch (error) {
            (w as any).ALERTS.error('Error al inicializar el formulario', 'Error');
        }
    });
})();
