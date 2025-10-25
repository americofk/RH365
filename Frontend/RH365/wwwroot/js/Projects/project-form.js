// ============================================================================
// Archivo: project-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Projects/project-form.ts
// Descripci�n: Formulario din�mico para Crear/Editar Proyectos
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
    var _a, _b;
    const w = window;
    const d = document;
    const $ = w.jQuery || w.$;
    const apiBase = ((_b = (_a = w.RH365) === null || _a === void 0 ? void 0 : _a.urls) === null || _b === void 0 ? void 0 : _b.apiBase) || "http://localhost:9595/api";
    const pageContainer = d.querySelector("#project-form-page");
    if (!pageContainer)
        return;
    const token = pageContainer.getAttribute("data-token") || "";
    const dataareaId = pageContainer.getAttribute("data-dataarea") || "DAT";
    const userRefRecID = parseInt(pageContainer.getAttribute("data-user") || "0", 10);
    const recId = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    const isNew = pageContainer.getAttribute("data-isnew") === "true";
    const fieldDefinitions = [
        { field: 'ID', label: 'ID Sistema', type: 'text', readonly: true },
        { field: 'ProjectCode', label: 'C�digo Proyecto', type: 'text', required: true, maxLength: 50, placeholder: 'PRJ-001' },
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
        { field: 'CreatedOn', label: 'Fecha Creaci�n', type: 'datetime', readonly: true },
        { field: 'CreatedBy', label: 'Creado Por', type: 'text', readonly: true },
        { field: 'ModifiedOn', label: '�ltima Modificaci�n', type: 'datetime', readonly: true },
        { field: 'ModifiedBy', label: 'Modificado Por', type: 'text', readonly: true },
        { field: 'Observations', label: 'Observaciones', type: 'textarea', maxLength: 500 }
    ];
    let projectData = null;
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
    const renderField = (config, value) => {
        const fieldId = config.field;
        const fieldName = config.field;
        const labelClass = 'control-label col-md-3 col-sm-3 col-xs-12';
        const inputContainerClass = 'col-md-6 col-sm-6 col-xs-12';
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
                const optionsHtml = options.map(opt => `<option value="${opt.value}" ${displayValue.toString() === opt.value ? 'selected' : ''}>${opt.text}</option>`).join('');
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
    const loadProjectData = () => __awaiter(this, void 0, void 0, function* () {
        if (isNew) {
            renderForm({});
            return;
        }
        try {
            const url = `${apiBase}/Projects/${recId}`;
            projectData = yield fetchJson(url);
            renderForm(projectData);
        }
        catch (error) {
            w.ALERTS.error('Error al cargar los datos del proyecto', 'Error');
            renderForm({});
        }
    });
    const renderForm = (data) => {
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
    const getFormData = () => {
        const formData = {};
        fieldDefinitions.forEach(config => {
            const field = config.field;
            const $input = $(`#${field}`);
            // Saltar campos readonly (auditor�a)
            if (config.readonly) {
                return;
            }
            if ($input.length) {
                if (config.type === 'checkbox') {
                    formData[field] = $input.is(':checked');
                }
                else if (config.type === 'select' && field === 'ProjectStatus') {
                    // Convertir string "true"/"false" a boolean
                    const val = $input.val();
                    formData[field] = val === 'true';
                }
                else if (config.type === 'number') {
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
    const saveProject = () => __awaiter(this, void 0, void 0, function* () {
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
            yield fetchJson(url, {
                method: method,
                body: JSON.stringify(payload)
            });
            w.ALERTS.ok(isNew ? 'Proyecto creado exitosamente' : 'Proyecto actualizado exitosamente', '�xito');
            setTimeout(() => {
                window.location.href = '/Project/LP_Projects';
            }, 1500);
        }
        catch (error) {
            console.error('Error al guardar:', error);
            let errorMessage = 'Error al guardar el proyecto';
            try {
                const errorData = JSON.parse(error.message);
                if (errorData.errors) {
                    const errorsArray = [];
                    for (const key in errorData.errors) {
                        if (errorData.errors.hasOwnProperty(key)) {
                            const errList = errorData.errors[key];
                            if (Array.isArray(errList)) {
                                for (let i = 0; i < errList.length; i++) {
                                    errorsArray.push(errList[i]);
                                }
                            }
                            else {
                                errorsArray.push(errList);
                            }
                        }
                    }
                    errorMessage = errorsArray.join(', ');
                }
            }
            catch (_a) {
                errorMessage = error.message || errorMessage;
            }
            w.ALERTS.error(errorMessage, 'Error');
        }
    });
    $('#btn-save').on('click', () => __awaiter(this, void 0, void 0, function* () {
        const form = document.getElementById('frm-project');
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }
        yield saveProject();
    }));
    // Inicializar
    $(function () {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                yield loadProjectData();
            }
            catch (error) {
                w.ALERTS.error('Error al inicializar el formulario', 'Error');
            }
        });
    });
})();
//# sourceMappingURL=project-form.js.map