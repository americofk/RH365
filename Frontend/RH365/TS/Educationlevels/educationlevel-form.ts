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

(function () {
    const w: any = window;
    const d: Document = document;
    const $: any = w.jQuery || w.$;

    const apiBase: string = w.RH365?.urls?.apiBase || "";
    const pageContainer = d.querySelector("#educationlevel-form-page");
    if (!pageContainer) return;

    // Contexto
    const token: string = pageContainer.getAttribute("data-token") || "";
    const dataareaId: string = pageContainer.getAttribute("data-dataarea") || "DAT";
    const userRefRecID: number = parseInt(pageContainer.getAttribute("data-user") || "0", 10);
    const recId: number = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    const isNew: boolean = pageContainer.getAttribute("data-isnew") === "true";

    // ----------------------------- Definiciones ------------------------------
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
    }

    const businessFields: FieldConfig[] = [
        { field: 'EducationLevelCode', label: 'Código Nivel', type: 'text', required: true, maxLength: 40, placeholder: 'Ej: EDU-SEC', helpText: 'Código único del nivel educativo' },
        { field: 'Description', label: 'Descripción', type: 'text', required: true, maxLength: 200, placeholder: 'Ej: Educación Secundaria' }
    ];

    // Orden y etiquetas igual que Loan
    const auditFields: FieldConfig[] = [
        { field: 'RecID', label: 'RecID (Clave Primaria)', type: 'number', readonly: true },
        { field: 'ID', label: 'ID Sistema', type: 'text', readonly: true },
        { field: 'DataareaID', label: 'Empresa (DataareaID)', type: 'text', readonly: true },
        { field: 'CreatedBy', label: 'Creado Por', type: 'text', readonly: true },
        { field: 'CreatedOn', label: 'Fecha de Creación', type: 'datetime', readonly: true },
        { field: 'ModifiedBy', label: 'Modificado Por', type: 'text', readonly: true },
        { field: 'ModifiedOn', label: 'Fecha de Última Modificación', type: 'datetime', readonly: true }
    ];

    let educationLevelData: any = null;

    // ------------------------------ Utilidades ------------------------------
    const fetchJson = async (url: string, options?: RequestInit): Promise<any> => {
        const headers: Record<string, string> = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        if (token) headers['Authorization'] = `Bearer ${token}`;
        const response = await fetch(url, { ...options, headers });
        if (!response.ok) {
            const errorData = await response.json().catch(() => ({}));
            throw new Error(JSON.stringify(errorData));
        }
        return response.json();
    };

    const pad = (n: number) => (n < 10 ? `0${n}` : `${n}`);
    const formatDateTime = (iso?: string): string => {
        if (!iso) return '';
        const dt = new Date(iso);
        if (isNaN(dt.getTime())) return '';
        return `${pad(dt.getDate())}/${pad(dt.getMonth() + 1)}/${dt.getFullYear()} ${pad(dt.getHours())}:${pad(dt.getMinutes())}:${pad(dt.getSeconds())}`;
    };

    // ------------------------------ Render genérico -------------------------
    const renderField = (config: FieldConfig, value: any, isTwoCol: boolean): string => {
        const id = config.field;
        const name = config.field;

        const labelClass = isTwoCol ? 'control-label col-md-4 col-sm-4 col-xs-12'
            : 'control-label col-md-3 col-sm-3 col-xs-12';
        const inputWrap = isTwoCol ? 'col-md-8 col-sm-8 col-xs-12'
            : 'col-md-6 col-sm-6 col-xs-12';

        // === AUDITORÍA READONLY: usar INPUT deshabilitado (gris) como en Loan ===
        if (config.readonly) {
            let display = value ?? '';
            if (config.type === 'datetime') display = formatDateTime(value);
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
        let v = value ?? '';

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
                if (typeof v === 'string' && /^\d{4}-\d{2}-\d{2}T/.test(v)) v = v.split('T')[0];
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
    const renderBusinessFields = (data: any) => {
        const $c = $('#dynamic-fields-container');
        $c.empty();
        $c.append(businessFields.map(f => renderField(f, data[f.field], false)).join(''));
        if ($.fn.iCheck) $('.flat').iCheck({ checkboxClass: 'icheckbox_flat-green' });
    };

    const renderAuditFields = (data: any) => {
        const $c = $('#audit-fields-container');
        $c.empty();
        // Auditoría → 1 columna (mismas clases que Loan), inputs disabled
        $c.append(auditFields.map(f => renderField(f, data[f.field], false)).join(''));
    };

    // ------------------------------ Carga/Guardar ---------------------------
    const loadEducationLevelData = async () => {
        try {
            if (isNew) {
                educationLevelData = {};
                renderBusinessFields({});
                renderAuditFields({});
                return;
            }
            const url = `${apiBase}/EducationLevels/${recId}`;
            const data = await fetchJson(url);
            educationLevelData = data;
            renderBusinessFields(data);
            renderAuditFields(data);
        } catch (err) {
            console.error('Error al cargar:', err);
            (w as any).ALERTS?.error('Error al cargar el nivel educativo', 'Error');
        }
    };

    const getFormData = () => {
        const out: any = {};
        businessFields.forEach(f => { const $i = $(`#${f.field}`); if ($i.length) out[f.field] = ($i.val() as string) ?? null; });
        return out;
    };

    const saveEducationLevel = async () => {
        const form = d.getElementById('frm-educationlevel') as HTMLFormElement | null;
        if (form && !form.checkValidity()) { form.reportValidity(); return; }

        const fd = getFormData();
        const url = isNew ? `${apiBase}/EducationLevels` : `${apiBase}/EducationLevels/${recId}`;
        const method = isNew ? 'POST' : 'PUT';

        const payload: any = {
            EducationLevelCode: (fd.EducationLevelCode || '').trim(),
            Description: (fd.Description || '').trim()
        };
        if (!payload.EducationLevelCode || !payload.Description) {
            (w as any).ALERTS?.warn('Complete los campos requeridos', 'Validación'); return;
        }
        if (!isNew) { payload.RecID = recId; payload.DataareaID = dataareaId; }

        try {
            await fetchJson(url, { method, body: JSON.stringify(payload) });
            (w as any).ALERTS?.ok(isNew ? 'Nivel educativo creado' : 'Nivel educativo actualizado', 'Éxito');
            /*setTimeout(() => { window.location.href = '/EducationLevel/LP_EducationLevels'; }, 1200);*/
        } catch (e: any) {
            let msg = 'Error al guardar el nivel educativo';
            try {
                const ee = JSON.parse(e.message);
                if (ee?.errors) {
                    const arr: string[] = [];
                    for (const k in ee.errors) { const v = ee.errors[k]; if (Array.isArray(v)) arr.push(...v); else if (v) arr.push(v); }
                    if (arr.length) msg = arr.join(', ');
                } else if (ee?.title) msg = ee.title; else if (ee?.message) msg = ee.message;
            } catch { }
            (w as any).ALERTS?.error(msg, 'Error');
        }
    };

    // ------------------------------ Eventos/Init ----------------------------
    $('#btn-save').on('click', saveEducationLevel);

    $(async function () {
        await loadEducationLevelData();
    });
})();
