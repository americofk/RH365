// ============================================================================
// Archivo: position-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Positions/position-form.ts
// Descripción: 
//   - Formulario dinámico para Crear/Editar Posiciones
//   - Tab General: Campos de negocio en LAYOUT DE 2 COLUMNAS
//   - Tab Auditoría: Campos ISO 27001 en 1 columna
//   - Renderizado separado para cada tab
//   - Validación cliente + servidor
//   - Integración con API REST (/api/Positions)
// Estándar: ISO 27001 - Control A.14.2.5 (Principios de ingeniería de sistemas seguros)
// ============================================================================
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g = Object.create((typeof Iterator === "function" ? Iterator : Object).prototype);
    return g.next = verb(0), g["throw"] = verb(1), g["return"] = verb(2), typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (g && (g = 0, op[0] && (_ = 0)), _) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
(function () {
    var _this = this;
    // ========================================================================
    // CONFIGURACIÓN GLOBAL Y CONTEXTO
    // ========================================================================
    var w = window;
    var d = document;
    var $ = w.jQuery || w.$;
    var apiBase = w.RH365.urls.apiBase;
    var pageContainer = d.querySelector("#position-form-page");
    if (!pageContainer)
        return;
    var token = pageContainer.getAttribute("data-token") || "";
    var dataareaId = pageContainer.getAttribute("data-dataarea") || "DAT";
    var userRefRecID = parseInt(pageContainer.getAttribute("data-user") || "0", 10);
    var recId = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    var isNew = pageContainer.getAttribute("data-isnew") === "true";
    // ========================================================================
    // DEFINICIÓN DE CAMPOS - TAB GENERAL (Campos de Negocio en 2 COLUMNAS)
    // ========================================================================
    var businessFields = [
        // COLUMNA IZQUIERDA
        {
            field: 'PositionCode',
            label: 'Código Posición',
            type: 'text',
            required: true,
            maxLength: 20,
            placeholder: 'POS-001',
            column: 'left'
        },
        {
            field: 'PositionName',
            label: 'Nombre Posición',
            type: 'text',
            required: true,
            maxLength: 50,
            column: 'left'
        },
        {
            field: 'Description',
            label: 'Descripción',
            type: 'textarea',
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
            field: 'DepartmentRefRecID',
            label: 'Departamento',
            type: 'select',
            required: true,
            options: [],
            column: 'right'
        },
        {
            field: 'JobRefRecID',
            label: 'Puesto',
            type: 'select',
            required: true,
            options: [],
            column: 'right'
        },
        {
            field: 'StartDate',
            label: 'Fecha Inicio',
            type: 'date',
            required: true,
            column: 'right'
        },
        {
            field: 'EndDate',
            label: 'Fecha Fin',
            type: 'date',
            column: 'right'
        },
        {
            field: 'IsVacant',
            label: 'Vacante',
            type: 'select',
            required: true,
            options: [
                { value: 'true', text: 'Sí' },
                { value: 'false', text: 'No' }
            ],
            column: 'right'
        },
        {
            field: 'PositionStatus',
            label: 'Estado',
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
    var auditFields = [
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
    var positionData = null;
    // ========================================================================
    // UTILIDADES - COMUNICACIÓN CON API
    // ========================================================================
    var fetchJson = function (url, options) { return __awaiter(_this, void 0, void 0, function () {
        var headers, response, errorData;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0:
                    headers = {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    };
                    if (token) {
                        headers['Authorization'] = "Bearer ".concat(token);
                    }
                    return [4 /*yield*/, fetch(url, __assign(__assign({}, options), { headers: headers }))];
                case 1:
                    response = _a.sent();
                    if (!!response.ok) return [3 /*break*/, 3];
                    return [4 /*yield*/, response.json().catch(function () { return ({}); })];
                case 2:
                    errorData = _a.sent();
                    throw new Error(JSON.stringify(errorData));
                case 3: return [2 /*return*/, response.json()];
            }
        });
    }); };
    // ========================================================================
    // CARGA DE CATÁLOGOS (DEPARTMENTS Y JOBS)
    // ========================================================================
    /**
     * Cargar departamentos desde el API y poblar el campo DepartmentRefRecID
     */
    var loadDepartments = function () { return __awaiter(_this, void 0, void 0, function () {
        var url, response, departmentsArray, departmentField, error_1;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0:
                    _a.trys.push([0, 2, , 3]);
                    url = "".concat(apiBase, "/Departments?pageNumber=1&pageSize=100");
                    return [4 /*yield*/, fetchJson(url)];
                case 1:
                    response = _a.sent();
                    departmentsArray = [];
                    if (Array.isArray(response)) {
                        departmentsArray = response;
                    }
                    else if ((response === null || response === void 0 ? void 0 : response.Data) && Array.isArray(response.Data)) {
                        departmentsArray = response.Data;
                    }
                    if (departmentsArray.length > 0) {
                        departmentField = businessFields.find(function (f) { return f.field === 'DepartmentRefRecID'; });
                        if (departmentField) {
                            departmentField.options = departmentsArray.map(function (dept) { return ({
                                value: dept.RecID.toString(),
                                text: "".concat(dept.DepartmentCode, " - ").concat(dept.Name)
                            }); });
                        }
                    }
                    return [3 /*break*/, 3];
                case 2:
                    error_1 = _a.sent();
                    console.error('Error cargando departamentos:', error_1);
                    w.ALERTS.warn('No se pudieron cargar los departamentos', 'Advertencia');
                    return [3 /*break*/, 3];
                case 3: return [2 /*return*/];
            }
        });
    }); };
    /**
     * Cargar puestos (jobs) desde el API y poblar el campo JobRefRecID
     */
    var loadJobs = function () { return __awaiter(_this, void 0, void 0, function () {
        var url, response, jobsArray, jobField, error_2;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0:
                    _a.trys.push([0, 2, , 3]);
                    url = "".concat(apiBase, "/Jobs?pageNumber=1&pageSize=100");
                    return [4 /*yield*/, fetchJson(url)];
                case 1:
                    response = _a.sent();
                    jobsArray = [];
                    if (Array.isArray(response)) {
                        jobsArray = response;
                    }
                    else if ((response === null || response === void 0 ? void 0 : response.Data) && Array.isArray(response.Data)) {
                        jobsArray = response.Data;
                    }
                    if (jobsArray.length > 0) {
                        jobField = businessFields.find(function (f) { return f.field === 'JobRefRecID'; });
                        if (jobField) {
                            jobField.options = jobsArray.map(function (job) { return ({
                                value: job.RecID.toString(),
                                text: "".concat(job.JobCode, " - ").concat(job.Name)
                            }); });
                        }
                    }
                    return [3 /*break*/, 3];
                case 2:
                    error_2 = _a.sent();
                    console.error('Error cargando jobs:', error_2);
                    w.ALERTS.warn('No se pudieron cargar los puestos', 'Advertencia');
                    return [3 /*break*/, 3];
                case 3: return [2 /*return*/];
            }
        });
    }); };
    // ========================================================================
    // RENDERIZADO DE CAMPOS
    // ========================================================================
    var renderField = function (config, value, is2Column) {
        if (is2Column === void 0) { is2Column = false; }
        var fieldId = config.field;
        var fieldName = config.field;
        var labelClass = is2Column
            ? 'control-label col-md-4 col-sm-4 col-xs-12'
            : 'control-label col-md-3 col-sm-3 col-xs-12';
        var inputContainerClass = is2Column
            ? 'col-md-8 col-sm-8 col-xs-12'
            : 'col-md-6 col-sm-6 col-xs-12';
        var requiredMark = config.required ? '<span class="required">*</span>' : '';
        var readonlyAttr = config.readonly ? 'readonly' : '';
        var requiredAttr = config.required ? 'required' : '';
        var inputHtml = '';
        var displayValue = value !== null && value !== void 0 ? value : '';
        switch (config.type) {
            case 'textarea':
                inputHtml = "<textarea id=\"".concat(fieldId, "\" name=\"").concat(fieldName, "\" class=\"form-control\" rows=\"3\" maxlength=\"").concat(config.maxLength || 500, "\" ").concat(readonlyAttr, " ").concat(requiredAttr, ">").concat(displayValue, "</textarea>");
                break;
            case 'select':
                var options = config.options || [];
                var optionsHtml = '';
                if (config.field === 'DepartmentRefRecID' || config.field === 'JobRefRecID') {
                    optionsHtml = '<option value="">-- Seleccione --</option>';
                    optionsHtml += options.map(function (opt) {
                        var isSelected = displayValue && displayValue.toString() === opt.value;
                        return "<option value=\"".concat(opt.value, "\" ").concat(isSelected ? 'selected' : '', ">").concat(opt.text, "</option>");
                    }).join('');
                }
                else {
                    optionsHtml = options.map(function (opt) {
                        return "<option value=\"".concat(opt.value, "\" ").concat(displayValue.toString() === opt.value ? 'selected' : '', ">").concat(opt.text, "</option>");
                    }).join('');
                }
                inputHtml = "<select id=\"".concat(fieldId, "\" name=\"").concat(fieldName, "\" class=\"form-control\" ").concat(readonlyAttr ? 'disabled' : '', " ").concat(requiredAttr, ">").concat(optionsHtml, "</select>");
                break;
            case 'checkbox':
                var checked = displayValue === true || displayValue === 'true' ? 'checked' : '';
                inputHtml = "<input type=\"checkbox\" id=\"".concat(fieldId, "\" name=\"").concat(fieldName, "\" class=\"flat\" ").concat(checked, " ").concat(readonlyAttr ? 'disabled' : '', ">");
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
                inputHtml = "<input type=\"text\" id=\"".concat(fieldId, "\" name=\"").concat(fieldName, "\" class=\"form-control\" value=\"").concat(displayValue, "\" ").concat(readonlyAttr, " ").concat(requiredAttr, ">");
                break;
            case 'date':
                if (displayValue && typeof displayValue === 'string' && /^\d{4}-\d{2}-\d{2}T/.test(displayValue)) {
                    displayValue = displayValue.split('T')[0];
                }
                inputHtml = "<input type=\"date\" id=\"".concat(fieldId, "\" name=\"").concat(fieldName, "\" class=\"form-control\" value=\"").concat(displayValue, "\" ").concat(readonlyAttr, " ").concat(requiredAttr, ">");
                break;
            case 'number':
                inputHtml = "<input type=\"number\" id=\"".concat(fieldId, "\" name=\"").concat(fieldName, "\" class=\"form-control\" value=\"").concat(displayValue, "\" ").concat(readonlyAttr, " ").concat(requiredAttr, ">");
                break;
            default:
                inputHtml = "<input type=\"text\" id=\"".concat(fieldId, "\" name=\"").concat(fieldName, "\" class=\"form-control\" maxlength=\"").concat(config.maxLength || 255, "\" value=\"").concat(displayValue, "\" placeholder=\"").concat(config.placeholder || '', "\" ").concat(readonlyAttr, " ").concat(requiredAttr, ">");
                break;
        }
        var helpTextHtml = config.helpText ? "<span class=\"help-block\">".concat(config.helpText, "</span>") : '';
        return "\n            <div class=\"form-group\">\n                <label class=\"".concat(labelClass, "\" for=\"").concat(fieldId, "\">\n                    ").concat(config.label, " ").concat(requiredMark, "\n                </label>\n                <div class=\"").concat(inputContainerClass, "\">\n                    ").concat(inputHtml, "\n                    ").concat(helpTextHtml, "\n                </div>\n            </div>\n        ");
    };
    // ========================================================================
    // CARGA DE DATOS DE LA POSICIÓN
    // ========================================================================
    var loadPositionData = function () { return __awaiter(_this, void 0, void 0, function () {
        var url, error_3;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, loadDepartments()];
                case 1:
                    _a.sent();
                    return [4 /*yield*/, loadJobs()];
                case 2:
                    _a.sent();
                    if (isNew) {
                        renderBusinessForm({});
                        renderAuditForm({});
                        return [2 /*return*/];
                    }
                    _a.label = 3;
                case 3:
                    _a.trys.push([3, 5, , 6]);
                    url = "".concat(apiBase, "/Positions/").concat(recId);
                    return [4 /*yield*/, fetchJson(url)];
                case 4:
                    positionData = _a.sent();
                    renderBusinessForm(positionData);
                    renderAuditForm(positionData);
                    return [3 /*break*/, 6];
                case 5:
                    error_3 = _a.sent();
                    console.error('Error al cargar posición:', error_3);
                    w.ALERTS.error('Error al cargar los datos de la posición', 'Error');
                    renderBusinessForm({});
                    renderAuditForm({});
                    return [3 /*break*/, 6];
                case 6: return [2 /*return*/];
            }
        });
    }); };
    // ========================================================================
    // RENDERIZADO DE FORMULARIOS
    // ========================================================================
    var renderBusinessForm = function (data) {
        var containerLeft = $('#dynamic-fields-col-left');
        var containerRight = $('#dynamic-fields-col-right');
        containerLeft.empty();
        containerRight.empty();
        businessFields
            .filter(function (config) { return config.column === 'left'; })
            .forEach(function (config) {
            var value = data[config.field];
            var fieldHtml = renderField(config, value, true);
            containerLeft.append(fieldHtml);
        });
        businessFields
            .filter(function (config) { return config.column === 'right'; })
            .forEach(function (config) {
            var value = data[config.field];
            var fieldHtml = renderField(config, value, true);
            containerRight.append(fieldHtml);
        });
        if ($.fn.iCheck) {
            $('.flat').iCheck({
                checkboxClass: 'icheckbox_flat-green'
            });
        }
    };
    var renderAuditForm = function (data) {
        var container = $('#audit-fields-container');
        container.empty();
        if (isNew) {
            container.html("\n                <div class=\"alert alert-warning\" role=\"alert\">\n                    <i class=\"fa fa-info-circle\"></i>\n                    <strong>Modo Creaci\u00F3n:</strong> \n                    Los campos de auditor\u00EDa se generar\u00E1n autom\u00E1ticamente despu\u00E9s de guardar la posici\u00F3n.\n                </div>\n            ");
            return;
        }
        auditFields.forEach(function (config) {
            var value = data[config.field];
            var fieldHtml = renderField(config, value, false);
            container.append(fieldHtml);
        });
    };
    // ========================================================================
    // CAPTURA DE DATOS DEL FORMULARIO
    // ========================================================================
    var getFormData = function () {
        var formData = {};
        businessFields.forEach(function (config) {
            var field = config.field;
            var $input = $("#".concat(field));
            if (config.readonly) {
                return;
            }
            if ($input.length) {
                if (config.type === 'checkbox') {
                    formData[field] = $input.is(':checked');
                }
                else if (config.type === 'select' && (field === 'PositionStatus' || field === 'IsVacant')) {
                    var val = $input.val();
                    formData[field] = val === 'true';
                }
                else if (config.type === 'select' && (field === 'DepartmentRefRecID' || field === 'JobRefRecID')) {
                    var val = $input.val();
                    formData[field] = val ? parseInt(val, 10) : null;
                }
                else if (config.type === 'number') {
                    var val = $input.val();
                    formData[field] = val ? parseFloat(val) : null;
                }
                else if (config.type === 'date') {
                    var val = $input.val();
                    formData[field] = val || null;
                }
                else {
                    var val = $input.val();
                    formData[field] = val || null;
                }
            }
        });
        return formData;
    };
    // ========================================================================
    // GUARDADO DE POSICIÓN
    // ========================================================================
    var savePosition = function () { return __awaiter(_this, void 0, void 0, function () {
        var formData, url, method, payload, error_4, errorMessage, errorData, errorsArray, key, errList, i;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0:
                    formData = getFormData();
                    _a.label = 1;
                case 1:
                    _a.trys.push([1, 3, , 4]);
                    url = isNew ? "".concat(apiBase, "/Positions") : "".concat(apiBase, "/Positions/").concat(recId);
                    method = isNew ? 'POST' : 'PUT';
                    payload = {
                        PositionCode: formData.PositionCode,
                        PositionName: formData.PositionName,
                        IsVacant: formData.IsVacant,
                        DepartmentRefRecID: formData.DepartmentRefRecID,
                        JobRefRecID: formData.JobRefRecID,
                        NotifyPositionRefRecID: formData.NotifyPositionRefRecID || null, // ← CORREGIDO: null en lugar de 0
                        PositionStatus: formData.PositionStatus,
                        StartDate: formData.StartDate,
                        EndDate: formData.EndDate || null,
                        Description: formData.Description || null,
                        Observations: formData.Observations || null
                    };
                    return [4 /*yield*/, fetchJson(url, {
                            method: method,
                            body: JSON.stringify(payload)
                        })];
                case 2:
                    _a.sent();
                    w.ALERTS.ok(isNew ? 'Posición creada exitosamente' : 'Posición actualizada exitosamente', 'Éxito');
                    setTimeout(function () {
                        window.location.href = '/Position/LP_Positions';
                    }, 1500);
                    return [3 /*break*/, 4];
                case 3:
                    error_4 = _a.sent();
                    console.error('Error al guardar:', error_4);
                    errorMessage = 'Error al guardar la posición';
                    try {
                        errorData = JSON.parse(error_4.message);
                        if (errorData.errors) {
                            errorsArray = [];
                            for (key in errorData.errors) {
                                if (errorData.errors.hasOwnProperty(key)) {
                                    errList = errorData.errors[key];
                                    if (Array.isArray(errList)) {
                                        for (i = 0; i < errList.length; i++) {
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
                    catch (_b) {
                        errorMessage = error_4.message || errorMessage;
                    }
                    w.ALERTS.error(errorMessage, 'Error');
                    return [3 /*break*/, 4];
                case 4: return [2 /*return*/];
            }
        });
    }); };
    // ========================================================================
    // EVENT HANDLERS
    // ========================================================================
    $('#btn-save').on('click', function () { return __awaiter(_this, void 0, void 0, function () {
        var form;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0:
                    form = document.getElementById('frm-position');
                    if (!form.checkValidity()) {
                        form.reportValidity();
                        return [2 /*return*/];
                    }
                    return [4 /*yield*/, savePosition()];
                case 1:
                    _a.sent();
                    return [2 /*return*/];
            }
        });
    }); });
    // ========================================================================
    // INICIALIZACIÓN
    // ========================================================================
    $(function () {
        return __awaiter(this, void 0, void 0, function () {
            var error_5;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, 2, , 3]);
                        return [4 /*yield*/, loadPositionData()];
                    case 1:
                        _a.sent();
                        return [3 /*break*/, 3];
                    case 2:
                        error_5 = _a.sent();
                        w.ALERTS.error('Error al inicializar el formulario', 'Error');
                        return [3 /*break*/, 3];
                    case 3: return [2 /*return*/];
                }
            });
        });
    });
})();
