// ============================================================================
// Archivo: payroll-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Payrolls/payroll-form.ts
// Descripción: 
//   - Formulario dinámico para Crear/Editar Nóminas
//   - Tab General: Campos de negocio en LAYOUT DE 2 COLUMNAS
//   - Tab Auditoría: Campos ISO 27001 en 1 columna
//   - Renderizado separado para cada tab
//   - Validación cliente + servidor
//   - Integración con API REST (/api/Payrolls)
//   - Labels a la izquierda de los campos
// ISO 27001: Formulario con validación y trazabilidad de cambios
// ============================================================================

(function () {
    // ========================================================================
    // CONFIGURACIÓN GLOBAL Y CONTEXTO
    // ========================================================================
    const w: any = window;
    const d: Document = document;
    const $: any = w.jQuery || w.$;

    const apiBase: string = w.RH365.urls.apiBase;
    const pageContainer = d.querySelector("#payroll-form-page");

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
            field: 'Name',
            label: 'Nombre de Nómina',
            type: 'text',
            required: true,
            maxLength: 200,
            placeholder: 'Nómina Quincenal',
            column: 'left'
        },
        {
            field: 'PayFrecuency',
            label: 'Frecuencia de Pago',
            type: 'select',
            required: true,
            options: [
                { value: '0', text: 'Diario' },
                { value: '1', text: 'Semanal' },
                { value: '2', text: 'Bisemanal' },
                { value: '3', text: 'Quincenal' },
                { value: '4', text: 'Mensual' },
                { value: '5', text: 'Trimestral' },
                { value: '6', text: 'Cuatrimestral' },
                { value: '7', text: 'Semestral' },
                { value: '8', text: 'Anual' }
            ],
            column: 'left'
        },
        {
            field: 'ValidFrom',
            label: 'Válido Desde',
            type: 'date',
            required: true,
            column: 'left'
        },
        {
            field: 'ValidTo',
            label: 'Válido Hasta',
            type: 'date',
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
            required: true,
            options: [{ value: '', text: '-- Cargando monedas... --' }],
            column: 'right'
        },
        {
            field: 'BankSecuence',
            label: 'Secuencia Bancaria',
            type: 'number',
            placeholder: '1',
            column: 'right'
        },
        {
            field: 'IsRoyaltyPayroll',
            label: 'Nómina de Regalías',
            type: 'checkbox',
            column: 'right'
        },
        {
            field: 'IsForHourPayroll',
            label: 'Nómina por Hora',
            type: 'checkbox',
            column: 'right'
        },
        {
            field: 'PayrollStatus',
            label: 'Estado de la Nómina',
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

    let payrollData: any = null;

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
    // CARGA DE DATOS RELACIONADOS (MONEDAS)
    // ========================================================================

    const loadCurrencies = async (): Promise<void> => {
        try {
            const url = `${apiBase}/Currencies?skip=0&take=100`;
            const response = await fetchJson(url);

            const currencyField = businessFields.find(f => f.field === 'CurrencyRefRecID');
            
            if (!currencyField) {
                console.error('Campo CurrencyRefRecID no encontrado');
                return;
            }

            // Manejar tanto array directo como objeto con Data
            let currencies = [];
            if (Array.isArray(response)) {
                currencies = response;
            } else if (response?.Data && Array.isArray(response.Data)) {
                currencies = response.Data;
            }

            if (currencies.length > 0) {
                console.log('Monedas cargadas:', currencies.length);
                currencyField.options = currencies.map((currency: any) => ({
                    value: currency.RecID.toString(),
                    text: `${currency.CurrencyCode} - ${currency.Name}`
                }));
            } else {
                console.warn('No se encontraron monedas en el API');
                currencyField.options = [{ value: '', text: '-- Seleccione una moneda --' }];
            }
        } catch (error) {
            console.error('Error al cargar monedas:', error);
            const currencyField = businessFields.find(f => f.field === 'CurrencyRefRecID');
            if (currencyField) {
                currencyField.options = [{ value: '', text: '-- Error al cargar monedas --' }];
            }
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
    // CARGA DE DATOS DE LA NÓMINA
    // ========================================================================

    const loadPayrollData = async (): Promise<void> => {
        // Primero cargar las monedas
        await loadCurrencies();

        if (isNew) {
            renderBusinessForm({});
            renderAuditForm({});
            return;
        }

        try {
            const url = `${apiBase}/Payrolls/${recId}`;
            payrollData = await fetchJson(url);

            renderBusinessForm(payrollData);
            renderAuditForm(payrollData);
        } catch (error) {
            (w as any).ALERTS.error('Error al cargar los datos de la nómina', 'Error');

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
                    Los campos de auditoría se generarán automáticamente después de guardar la nómina.
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
                } else if (config.type === 'select' && field === 'PayrollStatus') {
                    const val = $input.val();
                    formData[field] = val === 'true';
                } else if (config.type === 'select' && field === 'PayFrecuency') {
                    const val = $input.val();
                    formData[field] = val ? parseInt(val) : null;
                } else if (config.type === 'number' || field === 'CurrencyRefRecID' || field === 'BankSecuence') {
                    const val = $input.val();
                    formData[field] = val ? parseInt(val) : null;
                } else if (config.type === 'date') {
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
    // GUARDADO DE NÓMINA
    // ========================================================================

    const savePayroll = async (): Promise<void> => {
        const formData = getFormData();

        try {
            const url = isNew ? `${apiBase}/Payrolls` : `${apiBase}/Payrolls/${recId}`;
            const method = isNew ? 'POST' : 'PUT';

            const payload = {
                Name: formData.Name,
                PayFrecuency: formData.PayFrecuency,
                ValidFrom: formData.ValidFrom,
                ValidTo: formData.ValidTo,
                Description: formData.Description || null,
                IsRoyaltyPayroll: formData.IsRoyaltyPayroll || false,
                IsForHourPayroll: formData.IsForHourPayroll || false,
                BankSecuence: formData.BankSecuence || 1,
                CurrencyRefRecID: formData.CurrencyRefRecID,
                PayrollStatus: formData.PayrollStatus,
                Observations: formData.Observations || null
            };

            console.log('FormData capturado:', formData);
            console.log('Enviando payload:', payload);

            await fetchJson(url, {
                method: method,
                body: JSON.stringify(payload)
            });

            (w as any).ALERTS.ok(
                isNew ? 'Nómina creada exitosamente' : 'Nómina actualizada exitosamente',
                'Éxito'
            );

            setTimeout(() => {
                window.location.href = '/Payroll/LP_Payrolls';
            }, 1500);

        } catch (error: any) {
            console.error('Error al guardar:', error);
            let errorMessage = 'Error al guardar la nómina';

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
        const form = document.getElementById('frm-payroll') as HTMLFormElement;

        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        await savePayroll();
    });

    // ========================================================================
    // INICIALIZACIÓN
    // ========================================================================

    $(async function () {
        try {
            await loadPayrollData();
        } catch (error) {
            (w as any).ALERTS.error('Error al inicializar el formulario', 'Error');
        }
    });
})();
