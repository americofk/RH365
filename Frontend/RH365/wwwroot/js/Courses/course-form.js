// ============================================================================
// Archivo: course-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Courses/course-form.ts
// Descripción: 
//   - Formulario dinámico para Crear/Editar Cursos
//   - Tab General: Campos de negocio en LAYOUT DE 2 COLUMNAS
//   - Tab Auditoría: Campos ISO 27001 en 1 columna
//   - Renderizado separado para cada tab
//   - Validación cliente + servidor
//   - Integración con API REST (/api/Courses)
// Estándar: ISO 27001 - Validación y seguridad de datos
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
    const pageContainer = d.querySelector("#course-form-page");
    if (!pageContainer)
        return;
    const token = pageContainer.getAttribute("data-token") || "";
    const dataareaId = pageContainer.getAttribute("data-dataarea") || "DAT";
    const userRefRecID = parseInt(pageContainer.getAttribute("data-user") || "0", 10);
    const recId = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    const isNew = pageContainer.getAttribute("data-isnew") === "true";
    let courseData = null;
    let courseTypesMap = new Map();
    let classRoomsMap = new Map();
    // ========================================================================
    // DEFINICIÓN DE CAMPOS - TAB GENERAL (2 COLUMNAS)
    // ========================================================================
    const businessFields = [
        // COLUMNA IZQUIERDA
        {
            field: 'CourseCode',
            label: 'Código Curso',
            type: 'text',
            required: true,
            maxLength: 50,
            placeholder: 'TECH-2025-001',
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
            field: 'CourseTypeRefRecID',
            label: 'Tipo de Curso',
            type: 'select',
            required: true,
            options: [],
            column: 'left'
        },
        {
            field: 'ClassRoomRefRecID',
            label: 'Salón',
            type: 'select',
            required: false,
            options: [],
            column: 'left'
        },
        {
            field: 'Description',
            label: 'Descripción',
            type: 'textarea',
            maxLength: 500,
            column: 'left'
        },
        {
            field: 'StartDate',
            label: 'Fecha Inicio',
            type: 'datetime',
            required: true,
            column: 'left'
        },
        {
            field: 'EndDate',
            label: 'Fecha Fin',
            type: 'datetime',
            required: true,
            column: 'left'
        },
        {
            field: 'MinStudents',
            label: 'Mínimo Estudiantes',
            type: 'number',
            required: true,
            column: 'left'
        },
        {
            field: 'MaxStudents',
            label: 'Máximo Estudiantes',
            type: 'number',
            required: true,
            column: 'left'
        },
        // COLUMNA DERECHA
        {
            field: 'IsMatrixTraining',
            label: 'Matriz de Formación',
            type: 'checkbox',
            column: 'right'
        },
        {
            field: 'InternalExternal',
            label: 'Tipo',
            type: 'select',
            required: true,
            options: [
                { value: '0', text: 'Interno' },
                { value: '1', text: 'Externo' }
            ],
            column: 'right'
        },
        {
            field: 'Periodicity',
            label: 'Periodicidad',
            type: 'number',
            required: true,
            column: 'right'
        },
        {
            field: 'QtySessions',
            label: 'Cantidad de Sesiones',
            type: 'number',
            required: true,
            column: 'right'
        },
        {
            field: 'Objetives',
            label: 'Objetivos',
            type: 'textarea',
            maxLength: 1000,
            column: 'right'
        },
        {
            field: 'Topics',
            label: 'Temas',
            type: 'textarea',
            maxLength: 1000,
            column: 'right'
        },
        {
            field: 'CourseStatus',
            label: 'Estado del Curso',
            type: 'select',
            required: true,
            options: [
                { value: '0', text: 'Borrador' },
                { value: '1', text: 'Planificado' },
                { value: '2', text: 'En Curso' },
                { value: '3', text: 'Finalizado' },
                { value: '4', text: 'Cancelado' }
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
    // DEFINICIÓN DE CAMPOS - TAB AUDITORÍA (ISO 27001)
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
    // ========================================================================
    // UTILIDADES - COMUNICACIÓN CON API
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
    // CARGA DE DATOS DE REFERENCIA
    // ========================================================================
    const loadCourseTypes = () => __awaiter(this, void 0, void 0, function* () {
        try {
            const url = `${apiBase}/CourseTypes?pageNumber=1&pageSize=1000`;
            const response = yield fetchJson(url);
            let courseTypes = [];
            if (Array.isArray(response)) {
                courseTypes = response;
            }
            else if ((response === null || response === void 0 ? void 0 : response.Data) && Array.isArray(response.Data)) {
                courseTypes = response.Data;
            }
            courseTypes.forEach((type) => {
                if (type.RecID && type.Name) {
                    courseTypesMap.set(type.RecID, type.Name);
                }
            });
            const courseTypeField = businessFields.find(f => f.field === 'CourseTypeRefRecID');
            if (courseTypeField) {
                courseTypeField.options = [
                    { value: '', text: '-- Seleccione --' },
                    ...Array.from(courseTypesMap.entries()).map(([recId, name]) => ({
                        value: recId.toString(),
                        text: name
                    }))
                ];
            }
        }
        catch (error) {
            w.ALERTS.warn('No se pudieron cargar los tipos de curso', 'Advertencia');
            throw error;
        }
    });
    const loadClassRooms = () => __awaiter(this, void 0, void 0, function* () {
        try {
            const url = `${apiBase}/ClassRooms?pageNumber=1&pageSize=1000`;
            const response = yield fetchJson(url);
            let classRooms = [];
            if (Array.isArray(response)) {
                classRooms = response;
            }
            else if ((response === null || response === void 0 ? void 0 : response.Data) && Array.isArray(response.Data)) {
                classRooms = response.Data;
            }
            classRooms.forEach((room) => {
                if (room.RecID && room.Name) {
                    classRoomsMap.set(room.RecID, room.Name);
                }
            });
            const classRoomField = businessFields.find(f => f.field === 'ClassRoomRefRecID');
            if (classRoomField) {
                classRoomField.options = [
                    { value: '', text: '-- Seleccione --' },
                    ...Array.from(classRoomsMap.entries()).map(([recId, name]) => ({
                        value: recId.toString(),
                        text: name
                    }))
                ];
            }
        }
        catch (error) {
            w.ALERTS.warn('No se pudieron cargar los salones', 'Advertencia');
            throw error;
        }
    });
    // ========================================================================
    // RENDERIZADO DE CAMPOS
    // ========================================================================
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
                    displayValue = displayValue.slice(0, 16);
                }
                if (readonlyAttr) {
                    if (displayValue) {
                        displayValue = new Date(displayValue).toLocaleString('es-DO', {
                            year: 'numeric',
                            month: '2-digit',
                            day: '2-digit',
                            hour: '2-digit',
                            minute: '2-digit'
                        });
                    }
                    inputHtml = `<input type="text" id="${fieldId}" name="${fieldName}" class="form-control" value="${displayValue}" ${readonlyAttr} ${requiredAttr}>`;
                }
                else {
                    inputHtml = `<input type="datetime-local" id="${fieldId}" name="${fieldName}" class="form-control" value="${displayValue}" ${readonlyAttr} ${requiredAttr}>`;
                }
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
    // CARGA DE DATOS DEL CURSO
    // ========================================================================
    const loadCourseData = () => __awaiter(this, void 0, void 0, function* () {
        if (isNew) {
            renderBusinessForm({});
            renderAuditForm({});
            return;
        }
        try {
            const url = `${apiBase}/Courses/${recId}`;
            courseData = yield fetchJson(url);
            renderBusinessForm(courseData);
            renderAuditForm(courseData);
        }
        catch (error) {
            w.ALERTS.error('No se pudieron cargar los datos del curso', 'Error');
            renderBusinessForm({});
            renderAuditForm({});
        }
    });
    // ========================================================================
    // RENDERIZADO DE FORMULARIOS
    // ========================================================================
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
    const renderAuditForm = (data) => {
        const container = $('#audit-fields-container');
        container.empty();
        if (isNew) {
            container.html(`
                <div class="alert alert-warning" role="alert">
                    <i class="fa fa-info-circle"></i>
                    <strong>Modo Creación:</strong> 
                    Los campos de auditoría se generarán automáticamente después de guardar el curso.
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
                else if (config.type === 'select') {
                    const val = $input.val();
                    if (field === 'InternalExternal' || field === 'CourseStatus') {
                        formData[field] = val ? parseInt(val) : 0;
                    }
                    else if (field === 'CourseTypeRefRecID' || field === 'ClassRoomRefRecID') {
                        formData[field] = val ? parseInt(val) : null;
                    }
                    else {
                        formData[field] = val || null;
                    }
                }
                else if (config.type === 'number') {
                    const val = $input.val();
                    formData[field] = val ? parseInt(val) : null;
                }
                else if (config.type === 'datetime') {
                    const val = $input.val();
                    if (val) {
                        formData[field] = new Date(val).toISOString();
                    }
                    else {
                        formData[field] = null;
                    }
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
    // GUARDADO DE CURSO
    // ========================================================================
    const saveCourse = () => __awaiter(this, void 0, void 0, function* () {
        const formData = getFormData();
        try {
            const url = isNew ? `${apiBase}/Courses` : `${apiBase}/Courses/${recId}`;
            const method = isNew ? 'POST' : 'PUT';
            const payload = {
                CourseCode: formData.CourseCode,
                Name: formData.Name,
                CourseTypeRefRecID: formData.CourseTypeRefRecID,
                ClassRoomRefRecID: formData.ClassRoomRefRecID,
                Description: formData.Description || null,
                StartDate: formData.StartDate,
                EndDate: formData.EndDate,
                IsMatrixTraining: formData.IsMatrixTraining,
                InternalExternal: formData.InternalExternal,
                MinStudents: formData.MinStudents,
                MaxStudents: formData.MaxStudents,
                Periodicity: formData.Periodicity,
                QtySessions: formData.QtySessions,
                Objetives: formData.Objetives || null,
                Topics: formData.Topics || null,
                CourseStatus: formData.CourseStatus,
                Observations: formData.Observations || null
            };
            yield fetchJson(url, {
                method: method,
                body: JSON.stringify(payload)
            });
            w.ALERTS.ok(isNew
                ? 'El curso ha sido creado exitosamente'
                : 'El curso ha sido actualizado exitosamente', 'Éxito');
            setTimeout(() => {
                window.location.href = '/Course/LP_Courses';
            }, 1500);
        }
        catch (error) {
            let errorMessage = 'No se pudo guardar el curso';
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
                    errorMessage = errorsArray.join('. ');
                }
                else if (errorData.title) {
                    errorMessage = errorData.title;
                }
            }
            catch (_a) {
                errorMessage = error.message || errorMessage;
            }
            w.ALERTS.error(errorMessage, 'Error al Guardar');
        }
    });
    // ========================================================================
    // EVENT HANDLERS
    // ========================================================================
    $('#btn-save').on('click', () => __awaiter(this, void 0, void 0, function* () {
        const form = document.getElementById('frm-course');
        if (!form.checkValidity()) {
            w.ALERTS.warn('Por favor complete todos los campos requeridos', 'Validación');
            form.reportValidity();
            return;
        }
        yield saveCourse();
    }));
    // ========================================================================
    // INICIALIZACIÓN
    // ========================================================================
    $(function () {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                yield Promise.all([
                    loadCourseTypes(),
                    loadClassRooms()
                ]);
                yield loadCourseData();
            }
            catch (error) {
                w.ALERTS.error('Error al inicializar el formulario de curso', 'Error de Inicialización');
            }
        });
    });
})();
//# sourceMappingURL=course-form.js.map