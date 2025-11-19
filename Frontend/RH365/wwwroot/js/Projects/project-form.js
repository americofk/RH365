// ============================================================================
// Archivo: project-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Projects/project-form.ts
// Descripción: 
//   - Formulario dinámico para Crear/Editar Proyectos
//   - Tab General: Campos de negocio en LAYOUT DE 2 COLUMNAS
//   - Tab Auditoría: Campos ISO 27001 en 1 columna
//   - Renderizado separado para cada tab
//   - Validación cliente + servidor
//   - Integración con API REST (/api/Projects)
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
    const pageContainer = d.querySelector("#project-form-page");
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
    // DEFINICIÓN DE CAMPOS - TAB GENERAL (Campos de Negocio en 2 COLUMNAS)
    // ========================================================================
    const businessFields = [
        // COLUMNA IZQUIERDA
        {
            field: 'ProjectCode',
            label: 'Código Proyecto',
            type: 'text',
            required: true,
            maxLength: 50,
            placeholder: 'PRJ-001',
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
            field: 'Observations',
            label: 'Observaciones',
            type: 'textarea',
            maxLength: 500,
            column: 'left'
        },
        // COLUMNA DERECHA
        {
            field: 'LedgerAccount',
            label: 'Cuenta Contable',
            type: 'text',
            maxLength: 50,
            placeholder: '1.01.001',
            column: 'right'
        },
        {
            field: 'ProjectStatus',
            label: 'Estado del Proyecto',
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
    // Variable global para almacenar los datos del proyecto cargados desde el API
    let projectData = null;
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
     * @param is2Column Si es true, ajusta las clases para layout de 2 columnas
     * @returns String con el HTML del campo
     */
    const renderField = (config, value, is2Column = false) => {
        const fieldId = config.field;
        const fieldName = config.field;
        // Labels SIEMPRE a la izquierda
        // Para layout de 2 columnas: label más ancho (col-md-4)
        // Para layout de 1 columna: label estándar (col-md-3)
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
    // CARGA DE DATOS DEL PROYECTO
    // ========================================================================
    /**
     * Carga los datos del proyecto desde el API (solo si es edición).
     * En modo creación, renderiza los formularios vacíos.
     */
    const loadProjectData = () => __awaiter(this, void 0, void 0, function* () {
        if (isNew) {
            // Modo creación: renderizar formularios vacíos
            renderBusinessForm({});
            renderAuditForm({});
            return;
        }
        // Modo edición: cargar datos desde el API
        try {
            const url = `${apiBase}/Projects/${recId}`;
            projectData = yield fetchJson(url);
            // Renderizar ambos formularios con los datos cargados
            renderBusinessForm(projectData);
            renderAuditForm(projectData);
        }
        catch (error) {
            w.ALERTS.error('Error al cargar los datos del proyecto', 'Error');
            // Renderizar formularios vacíos en caso de error
            renderBusinessForm({});
            renderAuditForm({});
        }
    });
    // ========================================================================
    // RENDERIZADO DE FORMULARIOS
    // ========================================================================
    /**
     * Renderiza el formulario de campos de negocio en LAYOUT DE 2 COLUMNAS.
     * Separa los campos según la propiedad 'column' de cada FieldConfig.
     * @param data Datos del proyecto a mostrar
     */
    const renderBusinessForm = (data) => {
        const containerLeft = $('#dynamic-fields-col-left');
        const containerRight = $('#dynamic-fields-col-right');
        containerLeft.empty();
        containerRight.empty();
        // Renderizar campos en columna izquierda
        businessFields
            .filter(config => config.column === 'left')
            .forEach(config => {
            const value = data[config.field];
            const fieldHtml = renderField(config, value, true); // true = layout 2 columnas
            containerLeft.append(fieldHtml);
        });
        // Renderizar campos en columna derecha
        businessFields
            .filter(config => config.column === 'right')
            .forEach(config => {
            const value = data[config.field];
            const fieldHtml = renderField(config, value, true); // true = layout 2 columnas
            containerRight.append(fieldHtml);
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
     * @param data Datos del proyecto a mostrar
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
                    Los campos de auditoría se generarán automáticamente después de guardar el proyecto.
                </div>
            `);
            return;
        }
        // Renderizar SOLO los campos de auditoría
        auditFields.forEach(config => {
            const value = data[config.field];
            const fieldHtml = renderField(config, value, false); // false = layout normal 1 columna
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
                else if (config.type === 'select' && field === 'ProjectStatus') {
                    // Convertir string "true"/"false" a boolean para ProjectStatus
                    const val = $input.val();
                    formData[field] = val === 'true';
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
    // GUARDADO DE PROYECTO
    // ========================================================================
    /**
     * Guarda el proyecto en el API (POST para crear, PUT para actualizar).
     * Muestra alertas de éxito o error y redirige al listado si es exitoso.
     */
    const saveProject = () => __awaiter(this, void 0, void 0, function* () {
        const formData = getFormData();
        try {
            // Determinar URL y método según el modo (crear/editar)
            const url = isNew ? `${apiBase}/Projects` : `${apiBase}/Projects/${recId}`;
            const method = isNew ? 'POST' : 'PUT';
            // Construir payload con los campos necesarios
            const payload = {
                ProjectCode: formData.ProjectCode,
                Name: formData.Name,
                LedgerAccount: formData.LedgerAccount || null,
                ProjectStatus: formData.ProjectStatus,
                Observations: formData.Observations || null
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
            w.ALERTS.ok(isNew ? 'Proyecto creado exitosamente' : 'Proyecto actualizado exitosamente', 'Éxito');
            //// Redirigir al listado después de 1.5 segundos
            //setTimeout(() => {
            //    window.location.href = '/Project/LP_Projects';
            //}, 1500);
        }
        catch (error) {
            console.error('Error al guardar:', error);
            let errorMessage = 'Error al guardar el proyecto';
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
        const form = document.getElementById('frm-project');
        // Validar campos requeridos del formulario
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }
        // Ejecutar guardado
        yield saveProject();
    }));
    // ========================================================================
    // INICIALIZACIÓN
    // ========================================================================
    /**
     * Función de inicialización que se ejecuta cuando el DOM está listo.
     * Carga los datos del proyecto y renderiza los formularios.
     */
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