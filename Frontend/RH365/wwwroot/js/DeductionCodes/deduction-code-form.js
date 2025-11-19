// ============================================================================
// Archivo: deduction-code-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/DeductionCodes/deduction-code-form.ts
// Descripción: 
//   - Formulario dinámico para Crear/Editar Códigos de Deducción
//   - Tab General: 2 columnas con SELECT para RefRecID
//   - Lógica condicional según Acción de Nómina
//   - Cumplimiento ISO 27001
// ============================================================================
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
(function () {
    // ========================================================================
    // CONFIGURACIÓN GLOBAL
    // ========================================================================
    const w = window;
    const d = document;
    const $ = w.jQuery || w.$;
    const apiBase = w.RH365.urls.apiBase;
    const pageContainer = d.querySelector("#deduction-code-form-page");
    if (!pageContainer)
        return;
    const token = pageContainer.getAttribute("data-token") || "";
    const dataareaId = pageContainer.getAttribute("data-dataarea") || "DAT";
    const userRefRecID = parseInt(pageContainer.getAttribute("data-user") || "0", 10);
    const recId = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    const isNew = pageContainer.getAttribute("data-isnew") === "true";
    let deductionCodeData = null;
    let projectOptions = [];
    let categoryOptions = [];
    let departmentOptions = [];
    // ========================================================================
    // CARGA DE OPCIONES
    // ========================================================================
    const loadProjectOptions = () => __awaiter(this, void 0, void 0, function* () {
        try {
            const url = `${apiBase}/Projects?pageNumber=1&pageSize=100`;
            const response = yield fetchJson(url);
            const data = Array.isArray(response) ? response : ((response === null || response === void 0 ? void 0 : response.Data) || (response === null || response === void 0 ? void 0 : response.data) || []);
            projectOptions = data.map((item) => {
                var _a;
                return ({
                    value: ((_a = item.RecID) === null || _a === void 0 ? void 0 : _a.toString()) || '',
                    text: `${item.ProjectCode || ''} - ${item.Name || ''}`
                });
            });
        }
        catch (error) {
            console.error('Error cargando proyectos:', error);
            projectOptions = [];
        }
    });
    const loadCategoryOptions = (projectRecID) => __awaiter(this, void 0, void 0, function* () {
        try {
            const url = `${apiBase}/ProjectCategories?pageNumber=1&pageSize=100`;
            const response = yield fetchJson(url);
            const data = Array.isArray(response) ? response : ((response === null || response === void 0 ? void 0 : response.Data) || (response === null || response === void 0 ? void 0 : response.data) || []);
            let filteredData = data;
            if (projectRecID) {
                filteredData = data.filter((item) => item.ProjectRefRecID === projectRecID ||
                    item.ProjectRecID === projectRecID ||
                    item.ProjectId === projectRecID);
            }
            categoryOptions = filteredData.map((item) => {
                var _a;
                return ({
                    value: ((_a = item.RecID) === null || _a === void 0 ? void 0 : _a.toString()) || '',
                    text: item.Name || item.CategoryName || item.CategoryCode || ''
                });
            });
        }
        catch (error) {
            console.error('Error cargando categorías:', error);
            categoryOptions = [];
        }
    });
    const loadDepartmentOptions = () => __awaiter(this, void 0, void 0, function* () {
        try {
            const url = `${apiBase}/Departments?pageNumber=1&pageSize=100`;
            const response = yield fetchJson(url);
            const data = Array.isArray(response) ? response : ((response === null || response === void 0 ? void 0 : response.Data) || (response === null || response === void 0 ? void 0 : response.data) || []);
            departmentOptions = data.map((item) => {
                var _a;
                return ({
                    value: ((_a = item.RecID) === null || _a === void 0 ? void 0 : _a.toString()) || '',
                    text: item.Name || item.DepartmentName || item.DepartmentCode || ''
                });
            });
        }
        catch (error) {
            console.error('Error cargando departamentos:', error);
            departmentOptions = [];
        }
    });
    // ========================================================================
    // ACTUALIZAR DROPDOWN DE CATEGORÍAS
    // ========================================================================
    const updateCategoryDropdown = (projectRecID) => __awaiter(this, void 0, void 0, function* () {
        const currentCategoryValue = $('#ProjCategoryRefRecID').val();
        yield loadCategoryOptions(projectRecID);
        const generalFields = getGeneralFields();
        const categoryField = generalFields.find(f => f.field === 'ProjCategoryRefRecID');
        if (categoryField) {
            const options = categoryField.options || [];
            const optionsHtml = options.map(opt => `<option value="${opt.value}">${opt.text}</option>`).join('');
            const $categorySelect = $('#ProjCategoryRefRecID');
            $categorySelect.html(optionsHtml);
            const valueExists = options.some(opt => opt.value === currentCategoryValue);
            if (valueExists && currentCategoryValue) {
                $categorySelect.val(currentCategoryValue);
            }
            else {
                $categorySelect.val('');
            }
        }
    });
    // ========================================================================
    // DEFINICIÓN DE CAMPOS - TAB GENERAL
    // ========================================================================
    const getGeneralFields = () => [
        // COLUMNA IZQUIERDA
        {
            field: 'Name',
            label: 'Nombre',
            type: 'text',
            required: true,
            maxLength: 200,
            placeholder: 'Seguro Médico',
            column: 'left'
        },
        {
            field: 'ProjectRefRecID',
            label: 'Proyecto',
            type: 'select',
            options: [{ value: '', text: '-- Seleccione --' }, ...projectOptions],
            helpText: 'Proyecto asociado (opcional)',
            column: 'left'
        },
        {
            field: 'ProjCategoryRefRecID',
            label: 'Categoría Proyecto',
            type: 'select',
            options: [{ value: '', text: '-- Seleccione --' }, ...categoryOptions],
            helpText: 'Categoría del proyecto (opcional)',
            column: 'left'
        },
        {
            field: 'DepartmentRefRecID',
            label: 'Departamento',
            type: 'select',
            options: [{ value: '', text: '-- Seleccione --' }, ...departmentOptions],
            helpText: 'Departamento asociado (opcional)',
            column: 'left'
        },
        {
            field: 'LedgerAccount',
            label: 'Cuenta Contable',
            type: 'text',
            maxLength: 50,
            placeholder: '2101-001',
            column: 'left'
        },
        {
            field: 'Description',
            label: 'Descripción',
            type: 'textarea',
            maxLength: 500,
            placeholder: 'Descripción detallada',
            helpText: 'Máximo 500 caracteres',
            column: 'left'
        },
        // COLUMNA DERECHA
        {
            field: 'ValidFrom',
            label: 'Válido Desde',
            type: 'date',
            required: true,
            column: 'right'
        },
        {
            field: 'ValidTo',
            label: 'Válido Hasta',
            type: 'date',
            required: true,
            column: 'right'
        },
        {
            field: 'PayrollAction',
            label: 'Acción de Nómina',
            type: 'select',
            required: true,
            options: [
                { value: '0', text: 'Ninguna' },
                { value: '1', text: 'Contribución' },
                { value: '2', text: 'Deducción' },
                { value: '3', text: 'Ambas' }
            ],
            column: 'right'
        },
        {
            field: 'DeductionStatus',
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
            field: 'IsForTaxCalc',
            label: 'Usar para Cálculo ISR',
            type: 'checkbox',
            column: 'right'
        },
        {
            field: 'IsForTssCalc',
            label: 'Usar para Cálculo TSS',
            type: 'checkbox',
            column: 'right'
        },
        {
            field: 'Observations',
            label: 'Observaciones',
            type: 'textarea',
            maxLength: 1000,
            placeholder: 'Observaciones adicionales',
            helpText: 'Máximo 1000 caracteres',
            column: 'right'
        }
    ];
    // ========================================================================
    // CAMPOS CONTRIBUCIÓN
    // ========================================================================
    const contributionFields = [
        {
            field: 'CtbutionIndexBase',
            label: 'Base de Cálculo',
            type: 'select',
            options: [
                { value: '0', text: 'Ninguno' },
                { value: '1', text: 'Salario Base' },
                { value: '2', text: 'Salario Bruto' }
            ]
        },
        {
            field: 'CtbutionMultiplyAmount',
            label: 'Monto Multiplicador',
            type: 'decimal',
            min: 0,
            step: '0.01',
            placeholder: '150.50'
        },
        {
            field: 'CtbutionPayFrecuency',
            label: 'Frecuencia de Pago',
            type: 'select',
            options: [
                { value: '0', text: 'No aplica' },
                { value: '1', text: 'Semanal' },
                { value: '2', text: 'Quincenal' },
                { value: '3', text: 'Mensual' }
            ]
        },
        {
            field: 'CtbutionLimitPeriod',
            label: 'Periodo Límite (meses)',
            type: 'number',
            min: 0,
            max: 120,
            placeholder: '12'
        },
        {
            field: 'CtbutionLimitAmount',
            label: 'Monto Límite',
            type: 'decimal',
            min: 0,
            step: '0.01',
            placeholder: '5000.00'
        },
        {
            field: 'CtbutionLimitAmountToApply',
            label: 'Monto Límite a Aplicar',
            type: 'decimal',
            min: 0,
            step: '0.01',
            placeholder: '4500.00'
        }
    ];
    // ========================================================================
    // CAMPOS DEDUCCIÓN
    // ========================================================================
    const deductionFields = [
        {
            field: 'DductionIndexBase',
            label: 'Base de Cálculo',
            type: 'select',
            options: [
                { value: '0', text: 'Ninguno' },
                { value: '1', text: 'Salario Base' },
                { value: '2', text: 'Salario Bruto' }
            ]
        },
        {
            field: 'DductionMultiplyAmount',
            label: 'Monto Multiplicador',
            type: 'decimal',
            min: 0,
            step: '0.01',
            placeholder: '200.75'
        },
        {
            field: 'DductionPayFrecuency',
            label: 'Frecuencia de Pago',
            type: 'select',
            options: [
                { value: '0', text: 'No aplica' },
                { value: '1', text: 'Semanal' },
                { value: '2', text: 'Quincenal' },
                { value: '3', text: 'Mensual' }
            ]
        },
        {
            field: 'DductionLimitPeriod',
            label: 'Periodo Límite (meses)',
            type: 'number',
            min: 0,
            max: 120,
            placeholder: '12'
        },
        {
            field: 'DductionLimitAmount',
            label: 'Monto Límite',
            type: 'decimal',
            min: 0,
            step: '0.01',
            placeholder: '6000.00'
        },
        {
            field: 'DductionLimitAmountToApply',
            label: 'Monto Límite a Aplicar',
            type: 'decimal',
            min: 0,
            step: '0.01',
            placeholder: '5500.00'
        }
    ];
    // ========================================================================
    // CAMPOS AUDITORÍA
    // ========================================================================
    const auditFields = [
        {
            field: 'RecID',
            label: 'RecID (Clave Primaria)',
            type: 'number',
            readonly: true
        },
        {
            field: 'ID',
            label: 'ID Sistema',
            type: 'text',
            readonly: true
        },
        {
            field: 'DataareaID',
            label: 'Empresa (DataareaID)',
            type: 'text',
            readonly: true
        },
        {
            field: 'CreatedBy',
            label: 'Creado Por',
            type: 'text',
            readonly: true
        },
        {
            field: 'CreatedOn',
            label: 'Fecha de Creación',
            type: 'datetime',
            readonly: true
        },
        {
            field: 'ModifiedBy',
            label: 'Modificado Por',
            type: 'text',
            readonly: true
        },
        {
            field: 'ModifiedOn',
            label: 'Fecha de Última Modificación',
            type: 'datetime',
            readonly: true
        }
    ];
    // ========================================================================
    // UTILIDADES
    // ========================================================================
    const fetchJson = (url, options) => __awaiter(this, void 0, void 0, function* () {
        const headers = {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        };
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }
        const response = yield fetch(url, Object.assign(Object.assign({}, options), { headers }));
        if (!response.ok) {
            const errorData = yield response.json().catch(() => ({}));
            throw new Error(JSON.stringify(errorData));
        }
        return response.json();
    });
    // ========================================================================
    // RENDERIZADO
    // ========================================================================
    const renderField = (config, value, is2Column = false) => {
        const fieldId = config.field;
        const fieldName = config.field;
        const labelClass = is2Column
            ? 'control-label col-md-4 col-sm-4 col-xs-12'
            : 'control-label col-md-3 col-sm-3 col-xs-12';
        const inputContainerClass = is2Column
            ? 'col-md-8 col-sm-8 col-xs-12'
            : 'control-label col-md-6 col-sm-6 col-xs-12';
        const requiredMark = config.required ? '<span class="required">*</span>' : '';
        const readonlyAttr = config.readonly ? 'readonly' : '';
        const requiredAttr = config.required ? 'required' : '';
        let inputHtml = '';
        let displayValue = value !== null && value !== void 0 ? value : '';
        switch (config.type) {
            case 'textarea':
                inputHtml = `<textarea id="${fieldId}" name="${fieldName}" class="form-control" rows="3" maxlength="${config.maxLength || 500}" ${readonlyAttr} ${requiredAttr}>${displayValue}</textarea>`;
                break;
            case 'select':
                const options = config.options || [];
                const displayValueStr = displayValue != null ? displayValue.toString() : '';
                const optionsHtml = options.map(opt => `<option value="${opt.value}" ${displayValueStr === opt.value ? 'selected' : ''}>${opt.text}</option>`).join('');
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
            case 'decimal':
                inputHtml = `<input type="number" id="${fieldId}" name="${fieldName}" class="form-control" step="${config.step || '0.01'}" min="${config.min || 0}" value="${displayValue}" placeholder="${config.placeholder || ''}" ${readonlyAttr} ${requiredAttr}>`;
                break;
            case 'number':
                inputHtml = `<input type="number" id="${fieldId}" name="${fieldName}" class="form-control" min="${config.min || 0}" max="${config.max || ''}" value="${displayValue}" placeholder="${config.placeholder || ''}" ${readonlyAttr} ${requiredAttr}>`;
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
    // LÓGICA CONDICIONAL
    // ========================================================================
    const clearFieldValue = ($field, config) => {
        if (config.type === 'select') {
            $field.val($field.find('option:first').val());
        }
        else if (config.type === 'checkbox') {
            if ($.fn.iCheck) {
                $field.iCheck('uncheck');
            }
            else {
                $field.prop('checked', false);
            }
        }
        else {
            $field.val('');
        }
    };
    const toggleContributionFields = (enable) => {
        contributionFields.forEach(config => {
            const $field = $(`#${config.field}`);
            if ($field.length) {
                if (enable) {
                    $field.prop('disabled', false).prop('readonly', false);
                    $field.closest('.form-group').removeClass('disabled-field');
                }
                else {
                    clearFieldValue($field, config);
                    $field.prop('disabled', true);
                    $field.closest('.form-group').addClass('disabled-field');
                }
            }
        });
    };
    const toggleDeductionFields = (enable) => {
        deductionFields.forEach(config => {
            const $field = $(`#${config.field}`);
            if ($field.length) {
                if (enable) {
                    $field.prop('disabled', false).prop('readonly', false);
                    $field.closest('.form-group').removeClass('disabled-field');
                }
                else {
                    clearFieldValue($field, config);
                    $field.prop('disabled', true);
                    $field.closest('.form-group').addClass('disabled-field');
                }
            }
        });
    };
    const applyPayrollActionLogic = () => {
        const payrollAction = parseInt($('#PayrollAction').val() || '0', 10);
        switch (payrollAction) {
            case 0:
                toggleContributionFields(false);
                toggleDeductionFields(false);
                break;
            case 1:
                toggleContributionFields(true);
                toggleDeductionFields(false);
                break;
            case 2:
                toggleContributionFields(false);
                toggleDeductionFields(true);
                break;
            case 3:
                toggleContributionFields(true);
                toggleDeductionFields(true);
                break;
            default:
                toggleContributionFields(false);
                toggleDeductionFields(false);
                break;
        }
    };
    // ========================================================================
    // CARGA Y RENDERIZADO
    // ========================================================================
    const loadDeductionCodeData = () => __awaiter(this, void 0, void 0, function* () {
        yield Promise.all([
            loadProjectOptions(),
            loadCategoryOptions(),
            loadDepartmentOptions()
        ]);
        if (isNew) {
            renderGeneralForm({});
            renderContributionForm({});
            renderDeductionForm({});
            renderAuditForm({});
            applyPayrollActionLogic();
            return;
        }
        try {
            const url = `${apiBase}/DeductionCodes/${recId}`;
            deductionCodeData = yield fetchJson(url);
            renderGeneralForm(deductionCodeData);
            renderContributionForm(deductionCodeData);
            renderDeductionForm(deductionCodeData);
            renderAuditForm(deductionCodeData);
            applyPayrollActionLogic();
        }
        catch (error) {
            w.ALERTS.error('Error al cargar los datos', 'Error');
            renderGeneralForm({});
            renderContributionForm({});
            renderDeductionForm({});
            renderAuditForm({});
            applyPayrollActionLogic();
        }
    });
    const renderGeneralForm = (data) => {
        const containerLeft = $('#dynamic-fields-col-left');
        const containerRight = $('#dynamic-fields-col-right');
        containerLeft.empty();
        containerRight.empty();
        const generalFields = getGeneralFields();
        generalFields
            .filter(config => config.column === 'left')
            .forEach(config => {
            const value = data[config.field];
            const fieldHtml = renderField(config, value, true);
            containerLeft.append(fieldHtml);
        });
        generalFields
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
        // Event handler para cascada Proyecto -> Categoría
        $('#ProjectRefRecID').on('change', function () {
            return __awaiter(this, void 0, void 0, function* () {
                const projectRecID = $(this).val();
                const projectId = projectRecID && projectRecID !== '' ? parseInt(projectRecID, 10) : undefined;
                yield updateCategoryDropdown(projectId);
            });
        });
        // Inicializar cascada si hay proyecto seleccionado
        const initialProjectId = $('#ProjectRefRecID').val();
        if (initialProjectId && initialProjectId !== '') {
            const projectId = parseInt(initialProjectId, 10);
            updateCategoryDropdown(projectId);
        }
        $('#PayrollAction').on('change', function () {
            applyPayrollActionLogic();
        });
    };
    const renderContributionForm = (data) => {
        const container = $('#contribution-fields-container');
        container.empty();
        contributionFields.forEach(config => {
            const value = data[config.field];
            const fieldHtml = renderField(config, value, false);
            container.append(fieldHtml);
        });
    };
    const renderDeductionForm = (data) => {
        const container = $('#deduction-fields-container');
        container.empty();
        deductionFields.forEach(config => {
            const value = data[config.field];
            const fieldHtml = renderField(config, value, false);
            container.append(fieldHtml);
        });
    };
    const renderAuditForm = (data) => {
        const container = $('#audit-fields-container');
        container.empty();
        if (isNew) {
            container.html(`
                <div class="alert alert-warning" role="alert">
                    <i class="fa fa-info-circle"></i>
                    <strong>Modo Creación:</strong> 
                    Los campos de auditoría se generarán automáticamente.
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
    // CAPTURA Y GUARDADO
    // ========================================================================
    const getFormData = () => {
        const formData = {};
        const generalFields = getGeneralFields();
        const allEditableFields = [
            ...generalFields.filter(f => !f.readonly),
            ...contributionFields,
            ...deductionFields
        ];
        allEditableFields.forEach(config => {
            const field = config.field;
            const $input = $(`#${field}`);
            if ($input.length) {
                if ($input.prop('disabled')) {
                    formData[field] = null;
                    return;
                }
                if (config.type === 'checkbox') {
                    formData[field] = $input.is(':checked');
                }
                else if (field === 'DeductionStatus') {
                    const val = $input.val();
                    formData[field] = val === 'true';
                }
                else if (config.type === 'select' && (field.includes('RefRecID') || field.includes('RecID'))) {
                    const val = $input.val();
                    formData[field] = val && val !== '' ? parseInt(val, 10) : null;
                }
                else if (config.type === 'number') {
                    const val = $input.val();
                    formData[field] = val ? parseInt(val, 10) : null;
                }
                else if (config.type === 'decimal') {
                    const val = $input.val();
                    formData[field] = val ? parseFloat(val) : null;
                }
                else {
                    const val = $input.val();
                    formData[field] = val || null;
                }
            }
        });
        return formData;
    };
    const saveDeductionCode = () => __awaiter(this, void 0, void 0, function* () {
        const formData = getFormData();
        try {
            const url = isNew ? `${apiBase}/DeductionCodes` : `${apiBase}/DeductionCodes/${recId}`;
            const method = isNew ? 'POST' : 'PUT';
            yield fetchJson(url, {
                method: method,
                body: JSON.stringify(formData)
            });
            w.ALERTS.ok(isNew ? 'Creado exitosamente' : 'Actualizado exitosamente', 'Éxito');
            //setTimeout(() => {
            //    window.location.href = '/DeductionCode/LP_DeductionCodes';
            //}, 1500);
        }
        catch (error) {
            console.error('Error al guardar:', error);
            let errorMessage = 'Error al guardar';
            try {
                const errorData = JSON.parse(error.message);
                if (errorData.errors) {
                    const errorsArray = [];
                    for (const key in errorData.errors) {
                        if (errorData.errors.hasOwnProperty(key)) {
                            const errList = errorData.errors[key];
                            if (Array.isArray(errList)) {
                                errorsArray.push(...errList);
                            }
                            else {
                                errorsArray.push(errList);
                            }
                        }
                    }
                    errorMessage = errorsArray.join(', ');
                }
                else if (errorData.title) {
                    errorMessage = errorData.title;
                }
            }
            catch (_a) {
                errorMessage = error.message || errorMessage;
            }
            w.ALERTS.error(errorMessage, 'Error');
        }
    });
    // ========================================================================
    // EVENTOS
    // ========================================================================
    $('#btn-save').on('click', () => __awaiter(this, void 0, void 0, function* () {
        const form = document.getElementById('frm-deduction-code');
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }
        yield saveDeductionCode();
    }));
    // ========================================================================
    // INICIALIZACIÓN
    // ========================================================================
    $(function () {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                yield loadDeductionCodeData();
            }
            catch (error) {
                w.ALERTS.error('Error al inicializar', 'Error');
            }
        });
    });
    const style = document.createElement('style');
    style.textContent = `
        .disabled-field {
            opacity: 0.6;
        }
        .disabled-field label {
            color: #999;
        }
        .disabled-field input[disabled],
        .disabled-field select[disabled],
        .disabled-field textarea[disabled] {
            background-color: #f5f5f5;
            cursor: not-allowed;
        }
    `;
    document.head.appendChild(style);
})();
//# sourceMappingURL=deduction-code-form.js.map