// ============================================================================
// Archivo: tax-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Taxes/tax-form.ts
// Descripción: 
//   - Formulario dinámico para Crear/Editar Impuestos
//   - Tab General: Campos de negocio en LAYOUT DE 2 COLUMNAS
//   - Tab Auditoría: Campos ISO 27001 en 1 columna
//   - Renderizado separado para cada tab
//   - Validación cliente + servidor
//   - Integración con API REST (/api/Taxis)
//   - Labels a la izquierda de los campos
//   - Integración con tax-details.ts para Tab Detalles
// Estándar: ISO 27001 - Integridad y validación de datos
// ============================================================================

(function () {
    // ========================================================================
    // CONFIGURACIÓN GLOBAL Y CONTEXTO
    // ========================================================================
    const w: any = window;
    const d: Document = document;
    const $: any = w.jQuery || w.$;

    const apiBase: string = w.RH365.urls.apiBase;
    const pageContainer = d.querySelector("#tax-form-page");

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
        step?: string;
    }

    let taxData: any = null;

    // Variables para almacenar opciones de los selects
    let currencyOptions: { value: string; text: string }[] = [];
    let projectOptions: { value: string; text: string }[] = [];
    let projectCategoryOptions: { value: string; text: string }[] = [];
    let departmentOptions: { value: string; text: string }[] = [];

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
    // CARGA DE OPCIONES PARA SELECTS
    // ========================================================================

    const loadCurrencyOptions = async (): Promise<void> => {
        try {
            const url = `${apiBase}/Currencies?skip=0&take=100`;
            const response = await fetchJson(url);
            const data = Array.isArray(response) ? response : (response?.Data || response?.data || []);
            
            currencyOptions = data.map((item: any) => ({
                value: item.RecID?.toString() || '',
                text: `${item.CurrencyCode || ''} - ${item.Name || ''}`
            }));
        } catch (error) {
            console.error('Error cargando monedas:', error);
            currencyOptions = [];
        }
    };

    const loadProjectOptions = async (): Promise<void> => {
        try {
            const url = `${apiBase}/Projects?skip=0&take=100`;
            const response = await fetchJson(url);
            const data = Array.isArray(response) ? response : (response?.Data || response?.data || []);
            
            projectOptions = data.map((item: any) => ({
                value: item.RecID?.toString() || '',
                text: `${item.ProjectCode || ''} - ${item.Name || ''}`
            }));
        } catch (error) {
            console.error('Error cargando proyectos:', error);
            projectOptions = [];
        }
    };

    const loadProjectCategoryOptions = async (projectRecID?: number): Promise<void> => {
        try {
            let url = `${apiBase}/ProjectCategories?skip=0&take=100`;
            
            if (projectRecID) {
                url += `&projectId=${projectRecID}`;
            }
            
            const response = await fetchJson(url);
            const data = Array.isArray(response) ? response : (response?.Data || response?.data || []);
            
            let filteredData = data;
            if (projectRecID) {
                filteredData = data.filter((item: any) => 
                    item.ProjectRefRecID === projectRecID || 
                    item.ProjectRecID === projectRecID ||
                    item.ProjectId === projectRecID
                );
            }
            
            projectCategoryOptions = filteredData.map((item: any) => ({
                value: item.RecID?.toString() || '',
                text: item.Name || item.CategoryName || item.CategoryCode || ''
            }));
        } catch (error) {
            console.error('Error cargando categorías de proyecto:', error);
            projectCategoryOptions = [];
        }
    };

    const loadDepartmentOptions = async (): Promise<void> => {
        try {
            const url = `${apiBase}/Departments?skip=0&take=100`;
            const response = await fetchJson(url);
            const data = Array.isArray(response) ? response : (response?.Data || response?.data || []);
            
            departmentOptions = data.map((item: any) => ({
                value: item.RecID?.toString() || '',
                text: `${item.DepartmentCode || ''} - ${item.Name || ''}`
            }));
        } catch (error) {
            console.error('Error cargando departamentos:', error);
            departmentOptions = [];
        }
    };

    // ========================================================================
    // DEFINICIÓN DE CAMPOS - TAB GENERAL (Campos de Negocio en 2 COLUMNAS)
    // ========================================================================
    const getBusinessFields = (): FieldConfig[] => [
        // COLUMNA IZQUIERDA
        {
            field: 'TaxCode',
            label: 'Código Impuesto',
            type: 'text',
            required: true,
            maxLength: 50,
            placeholder: 'ISR',
            column: 'left'
        },
        {
            field: 'Name',
            label: 'Nombre',
            type: 'text',
            required: true,
            maxLength: 200,
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
        {
            field: 'ValidFrom',
            label: 'Válido Desde',
            type: 'datetime',
            required: true,
            column: 'left'
        },
        {
            field: 'ValidTo',
            label: 'Válido Hasta',
            type: 'datetime',
            required: true,
            column: 'left'
        },
        {
            field: 'Description',
            label: 'Descripción',
            type: 'textarea',
            maxLength: 500,
            column: 'left'
        },

        // COLUMNA DERECHA
        {
            field: 'CurrencyRefRecID',
            label: 'Moneda',
            type: 'select',
            options: [{ value: '', text: '-- Seleccione --' }, ...currencyOptions],
            column: 'right'
        },
        {
            field: 'ProjectRefRecID',
            label: 'Proyecto',
            type: 'select',
            options: [{ value: '', text: '-- Seleccione --' }, ...projectOptions],
            column: 'right'
        },
        {
            field: 'ProjectCategoryRefRecID',
            label: 'Categoría Proyecto',
            type: 'select',
            options: [{ value: '', text: '-- Seleccione --' }, ...projectCategoryOptions],
            column: 'right'
        },
        {
            field: 'DepartmentRefRecID',
            label: 'Departamento',
            type: 'select',
            options: [{ value: '', text: '-- Seleccione --' }, ...departmentOptions],
            column: 'right'
        },
        {
            field: 'TaxStatus',
            label: 'Estado del Impuesto',
            type: 'select',
            required: true,
            options: [
                { value: 'true', text: 'Activo' },
                { value: 'false', text: 'Inactivo' }
            ],
            column: 'right'
        },
        {
            field: 'Observations',
            label: 'Observaciones',
            type: 'textarea',
            maxLength: 500,
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
                const displayValueStr = displayValue != null ? displayValue.toString() : '';
                const optionsHtml = options.map(opt =>
                    `<option value="${opt.value}" ${displayValueStr === opt.value ? 'selected' : ''}>${opt.text}</option>`
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

            case 'number':
                const step = config.step || '1';
                inputHtml = `<input type="number" id="${fieldId}" name="${fieldName}" class="form-control" value="${displayValue}" step="${step}" ${readonlyAttr} ${requiredAttr}>`;
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
    // FUNCIONES AUXILIARES
    // ========================================================================

    const updateProjectCategoryDropdown = async (projectRecID?: number): Promise<void> => {
        const currentCategoryValue = $('#ProjectCategoryRefRecID').val();
        
        await loadProjectCategoryOptions(projectRecID);
        
        const businessFields = getBusinessFields();
        const categoryField = businessFields.find(f => f.field === 'ProjectCategoryRefRecID');
        
        if (categoryField) {
            const options = categoryField.options || [];
            const optionsHtml = options.map(opt =>
                `<option value="${opt.value}">${opt.text}</option>`
            ).join('');
            
            const $categorySelect = $('#ProjectCategoryRefRecID');
            $categorySelect.html(optionsHtml);
            
            const valueExists = options.some(opt => opt.value === currentCategoryValue);
            if (valueExists && currentCategoryValue) {
                $categorySelect.val(currentCategoryValue);
            } else {
                $categorySelect.val('');
            }
        }
    };

    // ========================================================================
    // CARGA DE DATOS DEL IMPUESTO
    // ========================================================================

    const loadTaxData = async (): Promise<void> => {
        await Promise.all([
            loadCurrencyOptions(),
            loadProjectOptions(),
            loadProjectCategoryOptions(),
            loadDepartmentOptions()
        ]);

        if (isNew) {
            renderBusinessForm({});
            renderAuditForm({});
            return;
        }

        try {
            const url = `${apiBase}/Taxis/${recId}`;
            taxData = await fetchJson(url);

            renderBusinessForm(taxData);
            renderAuditForm(taxData);
            
            // Inicializar módulo de TaxDetails
            if ((w as any).TaxDetailsModule) {
                await (w as any).TaxDetailsModule.initialize(recId, token);
            }
        } catch (error) {
            (w as any).ALERTS.error('Error al cargar los datos del impuesto', 'Error');

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

        const businessFields = getBusinessFields();

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

        $('#ProjectRefRecID').on('change', async function() {
            const projectRecID = $(this).val();
            const projectId = projectRecID && projectRecID !== '' ? parseInt(projectRecID as string, 10) : undefined;
            await updateProjectCategoryDropdown(projectId);
        });

        const initialProjectId = $('#ProjectRefRecID').val();
        if (initialProjectId && initialProjectId !== '') {
            const projectId = parseInt(initialProjectId as string, 10);
            updateProjectCategoryDropdown(projectId);
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
                    Los campos de auditoría se generarán automáticamente después de guardar el impuesto.
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

        const businessFields = getBusinessFields();

        businessFields.forEach(config => {
            const field = config.field;
            const $input = $(`#${field}`);

            if (config.readonly) {
                return;
            }

            if ($input.length) {
                if (config.type === 'checkbox') {
                    formData[field] = $input.is(':checked');
                } else if (config.type === 'select' && field === 'TaxStatus') {
                    const val = $input.val();
                    formData[field] = val === 'true';
                } else if (config.type === 'select' && (field.includes('RefRecID') || field.includes('RecID'))) {
                    const val = $input.val();
                    formData[field] = val && val !== '' ? parseInt(val, 10) : null;
                } else if (config.type === 'number') {
                    const val = $input.val();
                    formData[field] = val ? parseFloat(val) : null;
                } else if (config.type === 'datetime') {
                    const val = $input.val();
                    formData[field] = val ? new Date(val).toISOString() : null;
                } else {
                    const val = $input.val();
                    formData[field] = val || null;
                }
            }
        });

        return formData;
    };

    // ========================================================================
    // GUARDADO DE IMPUESTO
    // ========================================================================

    const saveTax = async (): Promise<void> => {
        const formData = getFormData();

        try {
            const url = isNew ? `${apiBase}/Taxis` : `${apiBase}/Taxis/${recId}`;
            const method = isNew ? 'POST' : 'PUT';

            const payload = {
                TaxCode: formData.TaxCode,
                Name: formData.Name,
                LedgerAccount: formData.LedgerAccount || null,
                ValidFrom: formData.ValidFrom,
                ValidTo: formData.ValidTo,
                CurrencyRefRecID: formData.CurrencyRefRecID || null,
                Description: formData.Description || null,
                ProjectRefRecID: formData.ProjectRefRecID || null,
                ProjectCategoryRefRecID: formData.ProjectCategoryRefRecID || null,
                DepartmentRefRecID: formData.DepartmentRefRecID || null,
                TaxStatus: formData.TaxStatus,
                Observations: formData.Observations || null
            };

            console.log('FormData capturado:', formData);
            console.log('Enviando payload:', payload);

            await fetchJson(url, {
                method: method,
                body: JSON.stringify(payload)
            });

            (w as any).ALERTS.ok(
                isNew ? 'Impuesto creado exitosamente' : 'Impuesto actualizado exitosamente',
                'Éxito'
            );

            setTimeout(() => {
                window.location.href = '/Tax/LP_Taxes';
            }, 1500);

        } catch (error: any) {
            console.error('Error al guardar:', error);
            let errorMessage = 'Error al guardar el impuesto';

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
        const form = document.getElementById('frm-tax') as HTMLFormElement;

        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        await saveTax();
    });

    // ========================================================================
    // INICIALIZACIÓN
    // ========================================================================

    $(async function () {
        try {
            await loadTaxData();
        } catch (error) {
            (w as any).ALERTS.error('Error al inicializar el formulario', 'Error');
        }
    });
})();
