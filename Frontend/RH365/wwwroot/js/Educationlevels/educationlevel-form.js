// ============================================================================
// Archivo: educationlevel-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/EducationLevels/educationlevel-form.ts
// Descripción:
//   - Form Crear/Editar Niveles Educativos (misma UX que Loan).
//   - Auditoría en inputs deshabilitados (gris) usando .form-control[disabled].
//   - Integración con API /api/EducationLevels.
//   - Labels a la izquierda, validación simple.
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
    const apiBase = ((_b = (_a = w.RH365) === null || _a === void 0 ? void 0 : _a.urls) === null || _b === void 0 ? void 0 : _b.apiBase) || "";
    const pageContainer = d.querySelector("#educationlevel-form-page");
    if (!pageContainer)
        return;
    // Contexto
    const token = pageContainer.getAttribute("data-token") || "";
    const dataareaId = pageContainer.getAttribute("data-dataarea") || "DAT";
    const userRefRecID = parseInt(pageContainer.getAttribute("data-user") || "0", 10);
    const recId = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    const isNew = pageContainer.getAttribute("data-isnew") === "true";
    const businessFields = [
        { field: 'EducationLevelCode', label: 'Código Nivel', type: 'text', required: true, maxLength: 40, placeholder: 'Ej: EDU-SEC', helpText: 'Código único del nivel educativo' },
        { field: 'Description', label: 'Descripción', type: 'text', required: true, maxLength: 200, placeholder: 'Ej: Educación Secundaria' }
    ];
    // Orden y etiquetas igual que Loan
    const auditFields = [
        { field: 'RecID', label: 'RecID (Clave Primaria)', type: 'number', readonly: true },
        { field: 'ID', label: 'ID Sistema', type: 'text', readonly: true },
        { field: 'DataareaID', label: 'Empresa (DataareaID)', type: 'text', readonly: true },
        { field: 'CreatedBy', label: 'Creado Por', type: 'text', readonly: true },
        { field: 'CreatedOn', label: 'Fecha de Creación', type: 'datetime', readonly: true },
        { field: 'ModifiedBy', label: 'Modificado Por', type: 'text', readonly: true },
        { field: 'ModifiedOn', label: 'Fecha de Última Modificación', type: 'datetime', readonly: true }
    ];
    let educationLevelData = null;
    // ------------------------------ Utilidades ------------------------------
    const fetchJson = (url, options) => __awaiter(this, void 0, void 0, function* () {
        const headers = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        if (token)
            headers['Authorization'] = `Bearer ${token}`;
        const response = yield fetch(url, Object.assign(Object.assign({}, options), { headers }));
        if (!response.ok) {
            const errorData = yield response.json().catch(() => ({}));
            throw new Error(JSON.stringify(errorData));
        }
        return response.json();
    });
    const pad = (n) => (n < 10 ? `0${n}` : `${n}`);
    const formatDateTime = (iso) => {
        if (!iso)
            return '';
        const dt = new Date(iso);
        if (isNaN(dt.getTime()))
            return '';
        return `${pad(dt.getDate())}/${pad(dt.getMonth() + 1)}/${dt.getFullYear()} ${pad(dt.getHours())}:${pad(dt.getMinutes())}:${pad(dt.getSeconds())}`;
    };
    // ------------------------------ Render genérico -------------------------
    const renderField = (config, value, isTwoCol) => {
        const id = config.field;
        const name = config.field;
        const labelClass = isTwoCol ? 'control-label col-md-4 col-sm-4 col-xs-12'
            : 'control-label col-md-3 col-sm-3 col-xs-12';
        const inputWrap = isTwoCol ? 'col-md-8 col-sm-8 col-xs-12'
            : 'col-md-6 col-sm-6 col-xs-12';
        // === AUDITORÍA READONLY: usar INPUT deshabilitado (gris) como en Loan ===
        if (config.readonly) {
            let display = value !== null && value !== void 0 ? value : '';
            if (config.type === 'datetime')
                display = formatDateTime(value);
            // Para aspecto uniforme, usamos type="text" deshabilitado (evita spinners del number)
            const disabledInput = `<input type="text" class="form-control" value="${display}" disabled readonly>`;
            return `
                <div class="form-group">
                    <label class="${labelClass}">${config.label}</label>
                    <div class="${inputWrap}">
                        ${disabledInput}
                    </div>
                </div>`;
        }
        // === Editables (negocio) ===
        const req = config.required ? 'required' : '';
        let html = '';
        let v = value !== null && value !== void 0 ? value : '';
        switch (config.type) {
            case 'textarea':
                html = `<textarea id="${id}" name="${name}" class="form-control" rows="3" maxlength="${config.maxLength || 500}" ${req}>${v}</textarea>`;
                break;
            case 'select': {
                const opts = (config.options || [])
                    .map(o => `<option value="${o.value}" ${String(v) === o.value ? 'selected' : ''}>${o.text}</option>`).join('');
                html = `<select id="${id}" name="${name}" class="form-control" ${req}>${opts}</select>`;
                break;
            }
            case 'checkbox':
                html = `<input type="checkbox" id="${id}" name="${name}" class="flat" ${v ? 'checked' : ''}>`;
                break;
            case 'datetime':
                html = `<input type="text" id="${id}" name="${name}" class="form-control" value="${formatDateTime(v)}" ${req}>`;
                break;
            case 'date':
                if (typeof v === 'string' && /^\d{4}-\d{2}-\d{2}T/.test(v))
                    v = v.split('T')[0];
                html = `<input type="date" id="${id}" name="${name}" class="form-control" value="${v}" ${req}>`;
                break;
            case 'number':
                html = `<input type="number" step="any" id="${id}" name="${name}" class="form-control" value="${v}" ${req}>`;
                break;
            default: // text
                html = `<input type="text" id="${id}" name="${name}" class="form-control" maxlength="${config.maxLength || 255}" value="${v}" placeholder="${config.placeholder || ''}" ${req}>`;
                break;
        }
        const help = config.helpText ? `<span class="help-block">${config.helpText}</span>` : '';
        const reqMark = config.required ? '<span class="required">*</span>' : '';
        return `
            <div class="form-group">
                <label class="${labelClass}" for="${id}">${config.label} ${reqMark}</label>
                <div class="${inputWrap}">
                    ${html}
                    ${help}
                </div>
            </div>`;
    };
    // ------------------------------ Render secciones ------------------------
    const renderBusinessFields = (data) => {
        const $c = $('#dynamic-fields-container');
        $c.empty();
        $c.append(businessFields.map(f => renderField(f, data[f.field], false)).join(''));
        if ($.fn.iCheck)
            $('.flat').iCheck({ checkboxClass: 'icheckbox_flat-green' });
    };
    const renderAuditFields = (data) => {
        const $c = $('#audit-fields-container');
        $c.empty();
        // Auditoría → 1 columna (mismas clases que Loan), inputs disabled
        $c.append(auditFields.map(f => renderField(f, data[f.field], false)).join(''));
    };
    // ------------------------------ Carga/Guardar ---------------------------
    const loadEducationLevelData = () => __awaiter(this, void 0, void 0, function* () {
        var _a;
        try {
            if (isNew) {
                educationLevelData = {};
                renderBusinessFields({});
                renderAuditFields({});
                return;
            }
            const url = `${apiBase}/EducationLevels/${recId}`;
            const data = yield fetchJson(url);
            educationLevelData = data;
            renderBusinessFields(data);
            renderAuditFields(data);
        }
        catch (err) {
            console.error('Error al cargar:', err);
            (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.error('Error al cargar el nivel educativo', 'Error');
        }
    });
    const getFormData = () => {
        const out = {};
        businessFields.forEach(f => { var _a; const $i = $(`#${f.field}`); if ($i.length)
            out[f.field] = (_a = $i.val()) !== null && _a !== void 0 ? _a : null; });
        return out;
    };
    const saveEducationLevel = () => __awaiter(this, void 0, void 0, function* () {
        var _a, _b, _c;
        const form = d.getElementById('frm-educationlevel');
        if (form && !form.checkValidity()) {
            form.reportValidity();
            return;
        }
        const fd = getFormData();
        const url = isNew ? `${apiBase}/EducationLevels` : `${apiBase}/EducationLevels/${recId}`;
        const method = isNew ? 'POST' : 'PUT';
        const payload = {
            EducationLevelCode: (fd.EducationLevelCode || '').trim(),
            Description: (fd.Description || '').trim()
        };
        if (!payload.EducationLevelCode || !payload.Description) {
            (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.warn('Complete los campos requeridos', 'Validación');
            return;
        }
        if (!isNew) {
            payload.RecID = recId;
            payload.DataareaID = dataareaId;
        }
        try {
            yield fetchJson(url, { method, body: JSON.stringify(payload) });
            (_b = w.ALERTS) === null || _b === void 0 ? void 0 : _b.ok(isNew ? 'Nivel educativo creado' : 'Nivel educativo actualizado', 'Éxito');
            setTimeout(() => { window.location.href = '/EducationLevel/LP_EducationLevels'; }, 1200);
        }
        catch (e) {
            let msg = 'Error al guardar el nivel educativo';
            try {
                const ee = JSON.parse(e.message);
                if (ee === null || ee === void 0 ? void 0 : ee.errors) {
                    const arr = [];
                    for (const k in ee.errors) {
                        const v = ee.errors[k];
                        if (Array.isArray(v))
                            arr.push(...v);
                        else if (v)
                            arr.push(v);
                    }
                    if (arr.length)
                        msg = arr.join(', ');
                }
                else if (ee === null || ee === void 0 ? void 0 : ee.title)
                    msg = ee.title;
                else if (ee === null || ee === void 0 ? void 0 : ee.message)
                    msg = ee.message;
            }
            catch (_d) { }
            (_c = w.ALERTS) === null || _c === void 0 ? void 0 : _c.error(msg, 'Error');
        }
    });
    // ------------------------------ Eventos/Init ----------------------------
    $('#btn-save').on('click', saveEducationLevel);
    $(function () {
        return __awaiter(this, void 0, void 0, function* () {
            yield loadEducationLevelData();
        });
    });
})();
//# sourceMappingURL=educationlevel-form.js.map