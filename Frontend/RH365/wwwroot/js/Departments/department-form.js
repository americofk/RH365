// ============================================================================
// Archivo: department-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Departments/department-form.ts
// Descripción: 
//   - Formulario dinámico para Crear/Editar Departamentos
//   - Tab General: Campos de negocio en LAYOUT DE 2 COLUMNAS
//   - Tab Auditoría: Campos ISO 27001 en 1 columna
//   - Renderizado separado para cada tab
//   - Validación cliente + servidor
//   - Integración con API REST (/api/Departments)
//   - Labels a la izquierda de los campos
// Estándar: ISO 27001 - Validación y trazabilidad de datos
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
    // CONFIGURACIÓN GLOBAL Y CONTEXTO
    // ========================================================================
    const w = window;
    const d = document;
    const $ = w.jQuery || w.$;
    const apiBase = w.RH365.urls.apiBase;
    const pageContainer = d.querySelector("#department-form-page");
    if (!pageContainer)
        return;
    const token = pageContainer.getAttribute("data-token") || "";
    const dataareaId = pageContainer.getAttribute("data-dataarea") || "DAT";
    const userRefRecID = parseInt(pageContainer.getAttribute("data-user") || "0", 10);
    const recId = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    const isNew = pageContainer.getAttribute("data-isnew") === "true";
    // ========================================================================
    // DEFINICIÓN DE CAMPOS - TAB GENERAL (Campos de Negocio en 2 COLUMNAS)
    // ========================================================================
    const businessFields = [
        // COLUMNA IZQUIERDA
        {
            field: 'DepartmentCode',
            label: 'Código Departamento',
            type: 'text',
            required: true,
            maxLength: 50,
            placeholder: 'DEPT-001',
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
            field: 'QtyWorkers',
            label: 'Cantidad de Trabajadores',
            type: 'number',
            placeholder: '0',
            column: 'left'
        },
        {
            field: 'Description',
            label: 'Descripción',
            type: 'textarea',
            maxLength: 500,
            placeholder: 'Descripción del departamento...',
            column: 'left'
        },
        {
            field: 'Observations',
            label: 'Observaciones',
            type: 'textarea',
            maxLength: 500,
            placeholder: 'Observaciones adicionales...',
            column: 'left'
        },
        // COLUMNA DERECHA
        {
            field: 'StartDate',
            label: 'Fecha de Inicio',
            type: 'date',
            column: 'right'
        },
        {
            field: 'EndDate',
            label: 'Fecha de Finalización',
            type: 'date',
            column: 'right'
        },
        {
            field: 'DepartmentStatus',
            label: 'Estado del Departamento',
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
    const auditFields = [
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
            field: 'QtyWorkers',
            label: 'Cantidad de Trabajadores',
            type: 'number',
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
    let departmentData = null;
    // ========================================================================
    // UTILIDADES - COMUNICACIÓN CON API
    // ========================================================================
    /**
     * Realiza petición HTTP al API con autenticación.
     */
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
    // RENDERIZADO DE CAMPOS
    // ========================================================================
    /**
     * Genera el HTML de un campo según su configuración.
     */
    const renderField = (config, value, is2Column = false) => {
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
    // CARGA DE DATOS DEL DEPARTAMENTO
    // ========================================================================
    /**
     * Carga los datos del departamento desde el API.
     */
    const loadDepartmentData = () => __awaiter(this, void 0, void 0, function* () {
        if (isNew) {
            renderBusinessForm({});
            renderAuditForm({});
            return;
        }
        try {
            const url = `${apiBase}/Departments/${recId}`;
            departmentData = yield fetchJson(url);
            renderBusinessForm(departmentData);
            renderAuditForm(departmentData);
        }
        catch (error) {
            w.ALERTS.error('Error al cargar los datos del departamento', 'Error');
            renderBusinessForm({});
            renderAuditForm({});
        }
    });
    // ========================================================================
    // RENDERIZADO DE FORMULARIOS
    // ========================================================================
    /**
     * Renderiza el formulario de campos de negocio en LAYOUT DE 2 COLUMNAS.
     */
    const renderBusinessForm = (data) => {
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
    /**
     * Renderiza el formulario de campos de auditoría (Tab Auditoría).
     */
    const renderAuditForm = (data) => {
        const container = $('#audit-fields-container');
        container.empty();
        if (isNew) {
            container.html(`
                <div class="alert alert-warning" role="alert">
                    <i class="fa fa-info-circle"></i>
                    <strong>Modo Creación:</strong> 
                    Los campos de auditoría se generarán automáticamente después de guardar el departamento.
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
    /**
     * Obtiene los datos del formulario de negocio para enviar al API.
     */
    const getFormData = () => {
        const formData = {};
        businessFields.forEach(config => {
            const field = config.field;
            const $input = $(`#${field}`);
            if (config.readonly) {
                return;
            }
            if ($input.length) {
                if (config.type === 'checkbox') {
                    formData[field] = $input.is(':checked');
                }
                else if (config.type === 'select' && field === 'DepartmentStatus') {
                    const val = $input.val();
                    formData[field] = val === 'true';
                }
                else if (config.type === 'number') {
                    const val = $input.val();
                    formData[field] = val ? parseInt(val) : null;
                }
                else if (config.type === 'date') {
                    const val = $input.val();
                    formData[field] = val || null;
                }
                else {
                    const val = $input.val();
                    formData[field] = val || null;
                }
            }
        });
        return formData;
    };
    // ========================================================================
    // GUARDADO DE DEPARTAMENTO
    // ========================================================================
    /**
     * Guarda el departamento en el API (POST para crear, PUT para actualizar).
     */
    const saveDepartment = () => __awaiter(this, void 0, void 0, function* () {
        const formData = getFormData();
        try {
            const url = isNew ? `${apiBase}/Departments` : `${apiBase}/Departments/${recId}`;
            const method = isNew ? 'POST' : 'PUT';
            const payload = {
                DepartmentCode: formData.DepartmentCode,
                Name: formData.Name,
                QtyWorkers: formData.QtyWorkers || null,
                StartDate: formData.StartDate || null,
                EndDate: formData.EndDate || null,
                Description: formData.Description || null,
                DepartmentStatus: formData.DepartmentStatus,
                Observations: formData.Observations || null
            };
            console.log('FormData capturado:', formData);
            console.log('Enviando payload:', payload);
            yield fetchJson(url, {
                method: method,
                body: JSON.stringify(payload)
            });
            w.ALERTS.ok(isNew ? 'Departamento creado exitosamente' : 'Departamento actualizado exitosamente', 'Éxito');
            setTimeout(() => {
                window.location.href = '/Department/LP_Departments';
            }, 1500);
        }
        catch (error) {
            console.error('Error al guardar:', error);
            let errorMessage = 'Error al guardar el departamento';
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
    // EVENT HANDLERS
    // ========================================================================
    /**
     * Manejador del botón Guardar.
     */
    $('#btn-save').on('click', () => __awaiter(this, void 0, void 0, function* () {
        const form = document.getElementById('frm-department');
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }
        yield saveDepartment();
    }));
    // ========================================================================
    // INICIALIZACIÓN
    // ========================================================================
    /**
     * Inicialización cuando el DOM está listo.
     */
    $(function () {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                yield loadDepartmentData();
            }
            catch (error) {
                w.ALERTS.error('Error al inicializar el formulario', 'Error');
            }
        });
    });
})();
//# sourceMappingURL=department-form.js.map