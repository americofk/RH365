// ============================================================================
// Archivo: occupation-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Occupations/occupation-form.ts
// Descripción: 
//   - Formulario dinámico para Crear/Editar Ocupaciones
//   - Tab General: Campos de negocio en LAYOUT DE 1 COLUMNA
//   - Tab Auditoría: Campos ISO 27001 en 1 columna
//   - Renderizado separado para cada tab
//   - Validación cliente + servidor
//   - Integración con API REST (/api/Occupations)
//   - Labels a la izquierda de los campos
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
    const pageContainer = d.querySelector("#occupation-form-page");
    // Si no existe el contenedor, salir
    if (!pageContainer)
        return;
    // Extraer datos del DOM
    const token = pageContainer.getAttribute("data-token") || "";
    const dataareaId = pageContainer.getAttribute("data-dataarea") || "DAT";
    const userRefRecID = parseInt(pageContainer.getAttribute("data-user") || "0", 10);
    const recId = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    const isNew = pageContainer.getAttribute("data-isnew") === "true";
    // ========================================================================
    // DEFINICIÓN DE CAMPOS - TAB GENERAL (Campos de Negocio en 1 COLUMNA)
    // ========================================================================
    const businessFields = [
        {
            field: 'OccupationCode',
            label: 'Código de Ocupación',
            type: 'text',
            required: true,
            maxLength: 50,
            placeholder: 'OC-001'
        },
        {
            field: 'Description',
            label: 'Descripción',
            type: 'textarea',
            required: true,
            maxLength: 500
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
    // Variable global para almacenar los datos de la ocupación cargados desde el API
    let occupationData = null;
    // ========================================================================
    // UTILIDADES - COMUNICACIÓN CON API
    // ========================================================================
    /**
     * Realiza una petición HTTP al API con manejo de autenticación.
     * @param url URL completa del endpoint
     * @param options Opciones adicionales para fetch (method, body, etc.)
     * @returns Promise con la respuesta JSON parseada
     */
    const fetchJson = (url, options) => __awaiter(this, void 0, void 0, function* () {
        const headers = {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        };
        // Agregar token de autenticación si existe
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }
        const response = yield fetch(url, Object.assign(Object.assign({}, options), { headers }));
        // Si la respuesta no es exitosa, lanzar error con el detalle
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
     * Labels siempre a la izquierda del campo.
     * @param config Configuración del campo
     * @param value Valor actual del campo
     * @returns String con el HTML del campo
     */
    const renderField = (config, value) => {
        const fieldId = config.field;
        const fieldName = config.field;
        // Labels a la izquierda - layout de 1 columna
        const labelClass = 'control-label col-md-3 col-sm-3 col-xs-12';
        const inputContainerClass = 'col-md-6 col-sm-6 col-xs-12';
        const requiredMark = config.required ? '<span class="required">*</span>' : '';
        const readonlyAttr = config.readonly ? 'readonly' : '';
        const requiredAttr = config.required ? 'required' : '';
        let inputHtml = '';
        let displayValue = value !== null && value !== void 0 ? value : '';
        // Generar input según el tipo de campo
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
                // Formatear datetime para visualización
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
                // Extraer solo la fecha (YYYY-MM-DD) si viene datetime
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
        // Agregar texto de ayuda si existe
        const helpTextHtml = config.helpText ? `<span class="help-block">${config.helpText}</span>` : '';
        // Retornar el HTML completo del form-group
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
    // CARGA DE DATOS DE LA OCUPACIÓN
    // ========================================================================
    /**
     * Carga los datos de la ocupación desde el API (solo si es edición).
     * En modo creación, renderiza los formularios vacíos.
     */
    const loadOccupationData = () => __awaiter(this, void 0, void 0, function* () {
        if (isNew) {
            // Modo creación: renderizar formularios vacíos
            renderBusinessForm({});
            renderAuditForm({});
            return;
        }
        // Modo edición: cargar datos desde el API
        try {
            const url = `${apiBase}/Occupations/${recId}`;
            occupationData = yield fetchJson(url);
            // Renderizar ambos formularios con los datos cargados
            renderBusinessForm(occupationData);
            renderAuditForm(occupationData);
        }
        catch (error) {
            w.ALERTS.error('Error al cargar los datos de la ocupación', 'Error');
            // Renderizar formularios vacíos en caso de error
            renderBusinessForm({});
            renderAuditForm({});
        }
    });
    // ========================================================================
    // RENDERIZADO DE FORMULARIOS
    // ========================================================================
    /**
     * Renderiza el formulario de campos de negocio en LAYOUT DE 1 COLUMNA.
     * @param data Datos de la ocupación a mostrar
     */
    const renderBusinessForm = (data) => {
        const container = $('#dynamic-fields-container');
        container.empty();
        // Renderizar todos los campos de negocio
        businessFields.forEach(config => {
            const value = data[config.field];
            const fieldHtml = renderField(config, value);
            container.append(fieldHtml);
        });
        // Inicializar iCheck para checkboxes si existe el plugin
        if ($.fn.iCheck) {
            $('.flat').iCheck({
                checkboxClass: 'icheckbox_flat-green'
            });
        }
    };
    /**
     * Renderiza el formulario de campos de auditoría (Tab Auditoría).
     * SOLO renderiza los campos definidos en auditFields.
     * @param data Datos de la ocupación a mostrar
     */
    const renderAuditForm = (data) => {
        const container = $('#audit-fields-container');
        container.empty();
        // Si es modo creación, mostrar mensaje informativo
        if (isNew) {
            container.html(`
                <div class="alert alert-warning" role="alert">
                    <i class="fa fa-info-circle"></i>
                    <strong>Modo Creación:</strong> 
                    Los campos de auditoría se generarán automáticamente después de guardar la ocupación.
                </div>
            `);
            return;
        }
        // Renderizar SOLO los campos de auditoría
        auditFields.forEach(config => {
            const value = data[config.field];
            const fieldHtml = renderField(config, value);
            container.append(fieldHtml);
        });
    };
    // ========================================================================
    // CAPTURA DE DATOS DEL FORMULARIO
    // ========================================================================
    /**
     * Obtiene los datos del formulario de negocio para enviar al API.
     * SOLO captura campos editables del Tab General (businessFields).
     * @returns Objeto con los datos del formulario
     */
    const getFormData = () => {
        const formData = {};
        // Iterar SOLO sobre businessFields (no auditFields)
        businessFields.forEach(config => {
            const field = config.field;
            const $input = $(`#${field}`);
            // Saltar campos readonly
            if (config.readonly) {
                return;
            }
            if ($input.length) {
                if (config.type === 'checkbox') {
                    // Capturar valor booleano de checkbox
                    formData[field] = $input.is(':checked');
                }
                else if (config.type === 'number') {
                    // Convertir a número o null
                    const val = $input.val();
                    formData[field] = val ? parseFloat(val) : null;
                }
                else {
                    // Capturar como string o null
                    const val = $input.val();
                    formData[field] = val || null;
                }
            }
        });
        return formData;
    };
    // ========================================================================
    // GUARDADO DE OCUPACIÓN
    // ========================================================================
    /**
     * Guarda la ocupación en el API (POST para crear, PUT para actualizar).
     * Muestra alertas de éxito o error y redirige al listado si es exitoso.
     */
    const saveOccupation = () => __awaiter(this, void 0, void 0, function* () {
        const formData = getFormData();
        try {
            // Determinar URL y método según el modo (crear/editar)
            const url = isNew ? `${apiBase}/Occupations` : `${apiBase}/Occupations/${recId}`;
            const method = isNew ? 'POST' : 'PUT';
            // Construir payload con los campos necesarios
            const payload = {
                OccupationCode: formData.OccupationCode,
                Description: formData.Description
            };
            // Debug en consola
            console.log('FormData capturado:', formData);
            console.log('Enviando payload:', payload);
            // Enviar petición al API
            yield fetchJson(url, {
                method: method,
                body: JSON.stringify(payload)
            });
            // Mostrar alerta de éxito
            w.ALERTS.ok(isNew ? 'Ocupación creada exitosamente' : 'Ocupación actualizada exitosamente', 'Éxito');
            // Redirigir al listado después de 1.5 segundos
            setTimeout(() => {
                window.location.href = '/Occupation/LP_Occupations';
            }, 1500);
        }
        catch (error) {
            console.error('Error al guardar:', error);
            let errorMessage = 'Error al guardar la ocupación';
            // Intentar parsear el mensaje de error del API
            try {
                const errorData = JSON.parse(error.message);
                // Si hay errores de validación, construir mensaje detallado
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
                // Si no se puede parsear, usar el mensaje original
                errorMessage = error.message || errorMessage;
            }
            // Mostrar alerta de error
            w.ALERTS.error(errorMessage, 'Error');
        }
    });
    // ========================================================================
    // EVENT HANDLERS
    // ========================================================================
    /**
     * Manejador del botón Guardar.
     * Valida el formulario y ejecuta el guardado.
     */
    $('#btn-save').on('click', () => __awaiter(this, void 0, void 0, function* () {
        const form = document.getElementById('frm-occupation');
        // Validar campos requeridos del formulario
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }
        // Ejecutar guardado
        yield saveOccupation();
    }));
    // ========================================================================
    // INICIALIZACIÓN
    // ========================================================================
    /**
     * Función de inicialización que se ejecuta cuando el DOM está listo.
     * Carga los datos de la ocupación y renderiza los formularios.
     */
    $(function () {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                yield loadOccupationData();
            }
            catch (error) {
                w.ALERTS.error('Error al inicializar el formulario', 'Error');
            }
        });
    });
})();
//# sourceMappingURL=occupation-form.js.map