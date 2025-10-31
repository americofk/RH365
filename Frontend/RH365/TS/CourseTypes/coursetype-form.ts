// ============================================================================
// Archivo: coursetype-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/CourseTypes/coursetype-form.ts
// Descripción: 
//   - Formulario dinámico para Crear/Editar Tipos de Curso
//   - Tab General: Campos de negocio en LAYOUT DE 2 COLUMNAS
//   - Tab Auditoría: Campos ISO 27001 en 1 columna
//   - Renderizado separado para cada tab
//   - Validación cliente + servidor
//   - Integración con API REST (/api/CourseTypes)
//   - Labels a la izquierda de los campos
// Estándar: ISO 27001 - Gestión segura de formularios
// ============================================================================

(function () {
    // ========================================================================
    // CONFIGURACIÓN GLOBAL Y CONTEXTO
    // ========================================================================
    const w: any = window;
    const d: Document = document;
    const $: any = w.jQuery || w.$;

    const apiBase: string = w.RH365.urls.apiBase;
    const pageContainer = d.querySelector("#coursetype-form-page");

    // Si no existe el contenedor, salir
    if (!pageContainer) return;

    // Extraer datos del DOM
    const token: string = pageContainer.getAttribute("data-token") || "";
    const dataareaId: string = pageContainer.getAttribute("data-dataarea") || "DAT";
    const userRefRecID: number = parseInt(pageContainer.getAttribute("data-user") || "0", 10);
    const recId: number = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    const isNew: boolean = pageContainer.getAttribute("data-isnew") === "true";

    // ========================================================================
    // INTERFACES Y TIPOS
    // ========================================================================

    /**
     * Configuración de un campo del formulario.
     * Define cómo se debe renderizar y validar cada campo.
     */
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
            field: 'CourseTypeCode',
            label: 'Código Tipo de Curso',
            type: 'text',
            required: true,
            maxLength: 20,
            placeholder: 'CT-001',
            column: 'left'
        },
        {
            field: 'Name',
            label: 'Nombre',
            type: 'text',
            required: true,
            maxLength: 200,
            placeholder: 'Curso Técnico',
            column: 'left'
        },

        // COLUMNA DERECHA
        {
            field: 'Description',
            label: 'Descripción',
            type: 'textarea',
            maxLength: 500,
            placeholder: 'Descripción del tipo de curso',
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

    // Variable global para almacenar los datos del tipo de curso cargados desde el API
    let courseTypeData: any = null;

    // ========================================================================
    // UTILIDADES - COMUNICACIÓN CON API
    // ========================================================================

    /**
     * Realiza una petición HTTP al API con manejo de autenticación.
     * @param url URL completa del endpoint
     * @param options Opciones adicionales para fetch (method, body, etc.)
     * @returns Promise con la respuesta JSON parseada
     */
    const fetchJson = async (url: string, options?: RequestInit): Promise<any> => {
        const headers: Record<string, string> = {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        };

        // Agregar token de autenticación si existe
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        const response = await fetch(url, { ...options, headers });

        // Si la respuesta no es exitosa, lanzar error con el detalle
        if (!response.ok) {
            const errorData = await response.json().catch(() => ({}));
            throw new Error(JSON.stringify(errorData));
        }

        return response.json();
    };

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
    const renderField = (config: FieldConfig, value: any, is2Column: boolean = false): string => {
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
        let displayValue = value ?? '';

        // Generar input según el tipo de campo
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
    // CARGA DE DATOS DEL TIPO DE CURSO
    // ========================================================================

    /**
     * Carga los datos del tipo de curso desde el API (solo si es edición).
     * En modo creación, renderiza los formularios vacíos.
     */
    const loadCourseTypeData = async (): Promise<void> => {
        if (isNew) {
            // Modo creación: renderizar formularios vacíos
            renderBusinessForm({});
            renderAuditForm({});
            return;
        }

        // Modo edición: cargar datos desde el API
        try {
            const url = `${apiBase}/CourseTypes/${recId}`;
            courseTypeData = await fetchJson(url);

            // Renderizar ambos formularios con los datos cargados
            renderBusinessForm(courseTypeData);
            renderAuditForm(courseTypeData);
        } catch (error) {
            (w as any).ALERTS.error('Error al cargar los datos del tipo de curso', 'Error');

            // Renderizar formularios vacíos en caso de error
            renderBusinessForm({});
            renderAuditForm({});
        }
    };

    // ========================================================================
    // RENDERIZADO DE FORMULARIOS
    // ========================================================================

    /**
     * Renderiza el formulario de campos de negocio en LAYOUT DE 2 COLUMNAS.
     * Separa los campos según la propiedad 'column' de cada FieldConfig.
     * @param data Datos del tipo de curso a mostrar
     */
    const renderBusinessForm = (data: any): void => {
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
     * @param data Datos del tipo de curso a mostrar
     */
    const renderAuditForm = (data: any): void => {
        const container = $('#audit-fields-container');
        container.empty();

        // Si es modo creación, mostrar mensaje informativo
        if (isNew) {
            container.html(`
                <div class="alert alert-warning" role="alert">
                    <i class="fa fa-info-circle"></i>
                    <strong>Modo Creación:</strong> 
                    Los campos de auditoría se generarán automáticamente después de guardar el tipo de curso.
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
    const getFormData = (): any => {
        const formData: any = {};

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
                } else if (config.type === 'number') {
                    // Convertir a número o null
                    const val = $input.val();
                    formData[field] = val ? parseFloat(val) : null;
                } else {
                    // Capturar como string o null
                    const val = $input.val();
                    formData[field] = val || null;
                }
            }
        });

        return formData;
    };

    // ========================================================================
    // GUARDADO DE TIPO DE CURSO
    // ========================================================================

    /**
     * Guarda el tipo de curso en el API (POST para crear, PUT para actualizar).
     * Muestra alertas de éxito o error y redirige al listado si es exitoso.
     */
    const saveCourseType = async (): Promise<void> => {
        const formData = getFormData();

        try {
            // Determinar URL y método según el modo (crear/editar)
            const url = isNew ? `${apiBase}/CourseTypes` : `${apiBase}/CourseTypes/${recId}`;
            const method = isNew ? 'POST' : 'PUT';

            // Construir payload con los campos necesarios
            const payload = {
                CourseTypeCode: formData.CourseTypeCode,
                Name: formData.Name,
                Description: formData.Description || null,
                Observations: formData.Observations || null
            };

            // Debug en consola
            console.log('FormData capturado:', formData);
            console.log('Enviando payload:', payload);

            // Enviar petición al API
            await fetchJson(url, {
                method: method,
                body: JSON.stringify(payload)
            });

            // Mostrar alerta de éxito
            (w as any).ALERTS.ok(
                isNew ? 'Tipo de curso creado exitosamente' : 'Tipo de curso actualizado exitosamente',
                'Éxito'
            );

            // Redirigir al listado después de 1.5 segundos
            setTimeout(() => {
                window.location.href = '/CourseType/LP_CourseTypes';
            }, 1500);

        } catch (error: any) {
            console.error('Error al guardar:', error);
            let errorMessage = 'Error al guardar el tipo de curso';

            // Intentar parsear el mensaje de error del API
            try {
                const errorData = JSON.parse(error.message);

                // Si hay errores de validación, construir mensaje detallado
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
                // Si no se puede parsear, usar el mensaje original
                errorMessage = error.message || errorMessage;
            }

            // Mostrar alerta de error
            (w as any).ALERTS.error(errorMessage, 'Error');
        }
    };

    // ========================================================================
    // EVENT HANDLERS
    // ========================================================================

    /**
     * Manejador del botón Guardar.
     * Valida el formulario y ejecuta el guardado.
     */
    $('#btn-save').on('click', async () => {
        const form = document.getElementById('frm-coursetype') as HTMLFormElement;

        // Validar campos requeridos del formulario
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        // Ejecutar guardado
        await saveCourseType();
    });

    // ========================================================================
    // INICIALIZACIÓN
    // ========================================================================

    /**
     * Función de inicialización que se ejecuta cuando el DOM está listo.
     * Carga los datos del tipo de curso y renderiza los formularios.
     */
    $(async function () {
        try {
            await loadCourseTypeData();
        } catch (error) {
            (w as any).ALERTS.error('Error al inicializar el formulario', 'Error');
        }
    });
})();
