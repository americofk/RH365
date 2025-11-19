// ============================================================================
// Archivo: loan-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Loans/loan-form.ts
// Descripción: 
//   - Formulario dinámico para Crear/Editar Préstamos
//   - Tab General: Campos de negocio en LAYOUT DE 2 COLUMNAS
//   - Tab Auditoría: Campos ISO 27001 en 1 columna
//   - Renderizado separado para cada tab
//   - Validación cliente + servidor
//   - Integración con API REST (/api/Loans)
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
    const w = window;
    const d = document;
    const $ = w.jQuery || w.$;
    const apiBase = w.RH365.urls.apiBase;
    const pageContainer = d.querySelector("#loan-form-page");
    if (!pageContainer)
        return;
    const token = pageContainer.getAttribute("data-token") || "";
    const dataareaId = pageContainer.getAttribute("data-dataarea") || "DAT";
    const userRefRecID = parseInt(pageContainer.getAttribute("data-user") || "0", 10);
    const recId = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    const isNew = pageContainer.getAttribute("data-isnew") === "true";
    const businessFields = [
        // COLUMNA IZQUIERDA
        {
            field: 'LoanCode',
            label: 'Código Préstamo',
            type: 'text',
            required: true,
            maxLength: 50,
            placeholder: 'LOAN-001',
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
            field: 'Description',
            label: 'Descripción',
            type: 'textarea',
            maxLength: 500,
            column: 'left'
        },
        {
            field: 'LedgerAccount',
            label: 'Cuenta Contable',
            type: 'text',
            maxLength: 50,
            placeholder: '1.01.001',
            column: 'left'
        },
        // COLUMNA DERECHA
        {
            field: 'ValidFrom',
            label: 'Válido Desde',
            type: 'date',
            column: 'right'
        },
        {
            field: 'ValidTo',
            label: 'Válido Hasta',
            type: 'date',
            column: 'right'
        },
        {
            field: 'MultiplyAmount',
            label: 'Monto Multiplicador',
            type: 'number',
            column: 'right'
        },
        {
            field: 'PayFrecuency',
            label: 'Frecuencia de Pago',
            type: 'number',
            column: 'right'
        },
        {
            field: 'IndexBase',
            label: 'Base de Índice',
            type: 'number',
            column: 'right'
        },
        {
            field: 'LoanStatus',
            label: 'Estado del Préstamo',
            type: 'select',
            required: true,
            options: [
                { value: 'true', text: 'Activo' },
                { value: 'false', text: 'Inactivo' }
            ],
            column: 'right'
        }
    ];
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
    let loanData = null;
    const fetchJson = (url_1, ...args_1) => __awaiter(this, [url_1, ...args_1], void 0, function* (url, options = {}) {
        const headers = {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        };
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }
        const response = yield fetch(url, Object.assign(Object.assign({}, options), { headers }));
        if (!response.ok) {
            throw new Error(`HTTP ${response.status} @ ${url}`);
        }
        return response.json();
    });
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
                inputHtml = `<input type="number" step="any" id="${fieldId}" name="${fieldName}" class="form-control" value="${displayValue}" ${readonlyAttr} ${requiredAttr}>`;
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
    const loadLoanData = () => __awaiter(this, void 0, void 0, function* () {
        try {
            if (isNew) {
                renderBusinessTab({});
                renderAuditTab({});
                return;
            }
            const url = `${apiBase}/Loans/${recId}`;
            const data = yield fetchJson(url);
            loanData = data;
            renderBusinessTab(data);
            renderAuditTab(data);
        }
        catch (error) {
            w.ALERTS.error('Error al cargar el préstamo', 'Error');
        }
    });
    const renderBusinessTab = (data) => {
        const leftFields = businessFields.filter(f => f.column === 'left');
        const rightFields = businessFields.filter(f => f.column === 'right');
        const leftHtml = leftFields.map(config => {
            const value = data[config.field];
            return renderField(config, value, true);
        }).join('');
        const rightHtml = rightFields.map(config => {
            const value = data[config.field];
            return renderField(config, value, true);
        }).join('');
        d.getElementById('dynamic-fields-col-left').innerHTML = leftHtml;
        d.getElementById('dynamic-fields-col-right').innerHTML = rightHtml;
        if ($.fn.iCheck) {
            $('.flat').iCheck({ checkboxClass: 'icheckbox_flat-green' });
        }
    };
    const renderAuditTab = (data) => {
        const auditHtml = auditFields.map(config => {
            const value = data[config.field];
            return renderField(config, value, false);
        }).join('');
        d.getElementById('audit-fields-container').innerHTML = auditHtml;
    };
    const saveLoan = () => __awaiter(this, void 0, void 0, function* () {
        try {
            const formData = {
                LoanCode: $('#LoanCode').val().trim(),
                Name: $('#Name').val().trim(),
                Description: $('#Description').val().trim() || null,
                ValidFrom: $('#ValidFrom').val() || null,
                ValidTo: $('#ValidTo').val() || null,
                MultiplyAmount: parseFloat($('#MultiplyAmount').val()) || 0,
                LedgerAccount: $('#LedgerAccount').val().trim() || null,
                PayFrecuency: parseInt($('#PayFrecuency').val()) || 0,
                IndexBase: parseFloat($('#IndexBase').val()) || 0,
                LoanStatus: $('#LoanStatus').val() === 'true',
                DataareaID: dataareaId
            };
            if (!formData.LoanCode || !formData.Name) {
                w.ALERTS.warn('Complete los campos requeridos', 'Validación');
                return;
            }
            let url = `${apiBase}/Loans`;
            let method = 'POST';
            let payload = formData;
            if (!isNew) {
                url = `${apiBase}/Loans/${recId}`;
                method = 'PUT';
                delete payload.DataareaID;
            }
            yield fetchJson(url, {
                method,
                body: JSON.stringify(payload)
            });
            w.ALERTS.ok('Préstamo guardado correctamente', 'Éxito');
            //setTimeout(() => {
            //    window.location.href = '/Loan/LP_Loans';
            //}, 1500);
        }
        catch (error) {
            w.ALERTS.error('Error al guardar el préstamo', 'Error');
        }
    });
    $('#btn-save').on('click', saveLoan);
    $(function () {
        return __awaiter(this, void 0, void 0, function* () {
            yield loadLoanData();
        });
    });
})();
//# sourceMappingURL=loan-form.js.map