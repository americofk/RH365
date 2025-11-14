// ============================================================================
// Archivo: paycycles-tab.ts
// Proyecto: RH365.WebMVC
// Ruta: wwwroot/js/Payrolls/paycycles-tab.ts
// Descripción:
//   - Tab de Ciclos de Pago dentro del formulario de Payroll
//   - Generación masiva de ciclos con un solo clic
//   - CRUD individual de ciclos
//   - Eliminación múltiple de ciclos seleccionados
//   - Validaciones de estado (solo Open se puede editar/eliminar)
//   - Soporte para ISR y TSS
// ISO 27001: Gestión de ciclos con trazabilidad completa
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
    const apiBase = ((_b = (_a = w.RH365) === null || _a === void 0 ? void 0 : _a.urls) === null || _b === void 0 ? void 0 : _b.apiBase) || '/api';
    // Variables globales
    let currentPayrollId = 0;
    let payCycles = [];
    // ========================================================================
    // INICIALIZACIÓN DE PAYROLL ID
    // ========================================================================
    /**
     * Obtiene el PayrollId desde el DOM
     */
    const getPayrollIdFromDOM = () => {
        const pageContainer = d.querySelector("#payroll-form-page");
        if (!pageContainer) {
            console.warn('⚠️ No se encontró #payroll-form-page');
            return 0;
        }
        const recIdAttr = pageContainer.getAttribute("data-recid");
        const recId = parseInt(recIdAttr || "0", 10);
        console.log('📊 PayrollId desde DOM:', recId);
        return recId;
    };
    // ========================================================================
    // FUNCIÓN PARA CARGAR CICLOS DE PAGO
    // ========================================================================
    const loadPayCycles = () => __awaiter(this, void 0, void 0, function* () {
        var _a;
        console.log('📊 loadPayCycles() - Iniciando carga...');
        console.log('📊 Current Payroll ID:', currentPayrollId);
        // Si no hay PayrollId, mostrar estado vacío
        if (!currentPayrollId || currentPayrollId === 0) {
            console.log('⚠️ No hay Payroll ID, mostrando estado para nuevo registro');
            showNewPayrollState();
            return;
        }
        try {
            // Llamar al API para obtener TODOS los ciclos
            const url = `${apiBase}/PayCycles?skip=0&take=500`;
            console.log('🌐 Fetching:', url);
            const response = yield fetch(url, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            });
            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }
            const allCycles = yield response.json();
            console.log('📦 Total ciclos recibidos:', allCycles.length);
            // Filtrar solo los ciclos de este Payroll (PascalCase)
            payCycles = allCycles.filter((c) => {
                const match = c.PayrollRefRecID === currentPayrollId;
                if (match) {
                    console.log('✓ Ciclo matched:', c.ID, 'PayrollRef:', c.PayrollRefRecID);
                }
                return match;
            });
            console.log(`✅ ${payCycles.length} ciclos filtrados para Payroll ${currentPayrollId}`);
            renderPayCyclesTable();
            updateCycleCount(payCycles.length);
            updateButtonStates();
        }
        catch (error) {
            console.error('❌ Error al cargar ciclos:', error);
            (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.error('Error al cargar los ciclos de pago', 'Error');
            showEmptyState();
        }
    });
    // ========================================================================
    // RENDERIZADO DE TABLA
    // ========================================================================
    const renderPayCyclesTable = () => {
        const tbody = $('#paycycles-tbody');
        tbody.empty();
        if (payCycles.length === 0) {
            showEmptyState();
            return;
        }
        payCycles.forEach(cycle => {
            const statusBadge = getStatusBadge(cycle.StatusPeriod);
            const isLocked = cycle.StatusPeriod === 2 || cycle.StatusPeriod === 3; // Paid or Registered
            const row = `
                <tr data-recid="${cycle.RecID}">
                    <td class="text-center">
                        <input type="checkbox" 
                               class="flat cycle-check" 
                               data-recid="${cycle.RecID}"
                               data-status="${cycle.StatusPeriod}"
                               ${isLocked ? 'disabled' : ''}>
                    </td>
                    <td>${cycle.ID || '-'}</td>
                    <td>${formatDate(cycle.PeriodStartDate)}</td>
                    <td>${formatDate(cycle.PeriodEndDate)}</td>
                    <td>${formatDate(cycle.PayDate)}</td>
                    <td class="text-right">${formatCurrency(cycle.AmountPaidPerPeriod)}</td>
                    <td class="text-center">${statusBadge}</td>
                    <td class="text-center">
                        <input type="checkbox" 
                               class="flat" 
                               ${cycle.IsForTax ? 'checked' : ''} 
                               disabled>
                    </td>
                    <td class="text-center">
                        <input type="checkbox" 
                               class="flat" 
                               ${cycle.IsForTss ? 'checked' : ''} 
                               disabled>
                    </td>
                    <td class="text-center">
                        <button type="button" 
                                class="btn btn-xs btn-primary btn-edit-cycle" 
                                data-recid="${cycle.RecID}"
                                data-status="${cycle.StatusPeriod}"
                                ${isLocked ? 'disabled' : ''}>
                            <i class="fa fa-pencil"></i>
                        </button>
                        <button type="button" 
                                class="btn btn-xs btn-danger btn-delete-cycle" 
                                data-recid="${cycle.RecID}"
                                data-status="${cycle.StatusPeriod}"
                                ${isLocked ? 'disabled' : ''}>
                            <i class="fa fa-trash"></i>
                        </button>
                    </td>
                </tr>
            `;
            tbody.append(row);
        });
        // Re-inicializar iCheck
        if ($.fn.iCheck) {
            $('.flat').iCheck({
                checkboxClass: 'icheckbox_flat-green'
            });
        }
    };
    const showEmptyState = () => {
        const tbody = $('#paycycles-tbody');
        tbody.html(`
            <tr>
                <td colspan="10" class="text-center text-muted" style="padding: 40px;">
                    <i class="fa fa-calendar-o fa-3x" style="opacity: 0.3;"></i>
                    <p style="margin-top: 15px; font-size: 16px;">No hay ciclos de pago</p>
                    <p class="text-muted" style="font-size: 13px;">
                        Use el botón <strong>"Generar"</strong> para crear ciclos automáticamente
                    </p>
                </td>
            </tr>
        `);
        updateCycleCount(0);
    };
    const showNewPayrollState = () => {
        const tbody = $('#paycycles-tbody');
        tbody.html(`
            <tr>
                <td colspan="10" class="text-center" style="padding: 40px;">
                    <i class="fa fa-info-circle fa-3x text-info" style="opacity: 0.5;"></i>
                    <p style="margin-top: 15px; font-size: 16px; color: #31708f;">
                        <strong>Guarde primero la nómina</strong>
                    </p>
                    <p class="text-muted" style="font-size: 13px;">
                        Después de guardar podrá generar los ciclos de pago
                    </p>
                </td>
            </tr>
        `);
        updateCycleCount(0);
    };
    // ========================================================================
    // ACTUALIZACIÓN DE CONTADOR
    // ========================================================================
    const updateCycleCount = (count) => {
        $('#paycycles-count').text(`${count} ciclo${count !== 1 ? 's' : ''}`);
        const badge = $('#paycycles-count-badge');
        if (count > 0) {
            badge.text(count).show();
        }
        else {
            badge.hide();
        }
    };
    // ========================================================================
    // GENERACIÓN MASIVA DE CICLOS
    // ========================================================================
    /**
     * Genera múltiples ciclos de pago llamando al endpoint del API.
     */
    const generatePayCycles = () => __awaiter(this, void 0, void 0, function* () {
        var _a, _b, _c, _d, _e;
        console.log('🎯 generatePayCycles() llamado');
        const quantity = parseInt($('#paycycle-quantity').val(), 10);
        console.log('📊 Cantidad ingresada:', quantity);
        console.log('📊 Payroll ID actual:', currentPayrollId);
        // Validar cantidad
        if (!quantity || quantity < 1 || quantity > 100) {
            console.warn('⚠️ Cantidad inválida:', quantity);
            (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.warn('Por favor ingrese una cantidad entre 1 y 100', 'Validación');
            return;
        }
        if (!currentPayrollId || currentPayrollId === 0) {
            console.error('❌ Payroll ID inválido:', currentPayrollId);
            (_b = w.ALERTS) === null || _b === void 0 ? void 0 : _b.error('Debe guardar la nómina antes de generar ciclos', 'Error');
            return;
        }
        console.log(`🚀 Generando ${quantity} ciclos para Payroll ${currentPayrollId}`);
        console.log(`🌐 API Base: ${apiBase}`);
        try {
            // Mostrar loading
            (_c = w.ALERTS) === null || _c === void 0 ? void 0 : _c.info('Generando ciclos...', 'Procesando');
            const url = `${apiBase}/PayCycles/generate`;
            console.log('🌐 POST:', url);
            // Payload con PascalCase
            const payload = {
                PayrollRefRecID: currentPayrollId,
                Quantity: quantity
            };
            console.log('📤 Payload:', payload);
            const response = yield fetch(url, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(payload)
            });
            if (!response.ok) {
                const errorData = yield response.json().catch(() => ({}));
                console.error('❌ Error del API:', errorData);
                throw new Error(errorData.error || errorData.message || errorData.title || `HTTP ${response.status}`);
            }
            const result = yield response.json();
            console.log('✅ Respuesta del API:', result);
            // Mostrar mensaje de éxito
            const message = result.message || `Se generaron ${result.count || quantity} ciclo(s) exitosamente`;
            (_d = w.ALERTS) === null || _d === void 0 ? void 0 : _d.ok(message, 'Éxito');
            // Recargar la tabla
            yield loadPayCycles();
            // Limpiar input
            $('#paycycle-quantity').val('12');
        }
        catch (error) {
            console.error('❌ Error al generar ciclos:', error);
            (_e = w.ALERTS) === null || _e === void 0 ? void 0 : _e.error(error.message || 'Error al generar los ciclos de pago', 'Error');
        }
    });
    // ========================================================================
    // CRUD INDIVIDUAL DE CICLOS
    // ========================================================================
    const createPayCycle = (data) => __awaiter(this, void 0, void 0, function* () {
        var _a, _b;
        try {
            const url = `${apiBase}/PayCycles`;
            const response = yield fetch(url, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(Object.assign(Object.assign({}, data), { PayrollRefRecID: currentPayrollId }))
            });
            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }
            (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.ok('Ciclo creado exitosamente', 'Éxito');
            yield loadPayCycles();
            $('#modal-paycycle').modal('hide');
        }
        catch (error) {
            console.error('Error al crear ciclo:', error);
            (_b = w.ALERTS) === null || _b === void 0 ? void 0 : _b.error('Error al crear el ciclo de pago', 'Error');
        }
    });
    const updatePayCycle = (recId, data) => __awaiter(this, void 0, void 0, function* () {
        var _a, _b;
        try {
            const url = `${apiBase}/PayCycles/${recId}`;
            const response = yield fetch(url, {
                method: 'PUT',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });
            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }
            (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.ok('Ciclo actualizado exitosamente', 'Éxito');
            yield loadPayCycles();
            $('#modal-paycycle').modal('hide');
        }
        catch (error) {
            console.error('Error al actualizar ciclo:', error);
            (_b = w.ALERTS) === null || _b === void 0 ? void 0 : _b.error('Error al actualizar el ciclo de pago', 'Error');
        }
    });
    const deleteCycle = (recId, status) => __awaiter(this, void 0, void 0, function* () {
        var _a, _b;
        // Validar que solo se puedan eliminar ciclos Open
        if (status === 2 || status === 3) {
            (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.error('No se pueden eliminar ciclos Pagados o Registrados', 'Validación');
            return;
        }
        (_b = w.ALERTS) === null || _b === void 0 ? void 0 : _b.confirm('¿Está seguro de eliminar este ciclo de pago?', 'Confirmar Eliminación', (confirmed) => __awaiter(this, void 0, void 0, function* () {
            var _a, _b;
            if (!confirmed)
                return;
            try {
                const url = `${apiBase}/PayCycles/${recId}`;
                const response = yield fetch(url, {
                    method: 'DELETE',
                    headers: {
                        'Accept': 'application/json'
                    }
                });
                if (!response.ok) {
                    throw new Error(`HTTP ${response.status}`);
                }
                (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.ok('Ciclo eliminado exitosamente', 'Éxito');
                yield loadPayCycles();
            }
            catch (error) {
                console.error('Error al eliminar ciclo:', error);
                (_b = w.ALERTS) === null || _b === void 0 ? void 0 : _b.error('Error al eliminar el ciclo de pago', 'Error');
            }
        }), { type: 'danger' });
    });
    // ========================================================================
    // ✅ NUEVA FUNCIÓN: ELIMINAR MÚLTIPLES CICLOS SELECCIONADOS
    // ========================================================================
    const deleteSelectedCycles = () => __awaiter(this, void 0, void 0, function* () {
        var _a, _b;
        console.log('🗑️ deleteSelectedCycles() llamado');
        // Obtener todos los checkboxes marcados
        const $checked = $('.cycle-check:checked');
        const count = $checked.length;
        if (count === 0) {
            console.warn('⚠️ No hay ciclos seleccionados');
            return;
        }
        console.log(`📊 Ciclos seleccionados: ${count}`);
        // Validar que ninguno esté Pagado o Registrado
        let hasLockedCycles = false;
        $checked.each(function () {
            const status = parseInt($(this).data('status'), 10);
            if (status === 2 || status === 3) {
                hasLockedCycles = true;
                return false; // break
            }
        });
        if (hasLockedCycles) {
            (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.error('No se pueden eliminar ciclos Pagados o Registrados. Deseleccione esos ciclos e intente nuevamente.', 'Validación');
            return;
        }
        // Confirmar eliminación
        const message = count === 1
            ? '¿Está seguro de eliminar este ciclo?'
            : `¿Está seguro de eliminar ${count} ciclos?`;
        (_b = w.ALERTS) === null || _b === void 0 ? void 0 : _b.confirm(message, 'Confirmar Eliminación', (confirmed) => __awaiter(this, void 0, void 0, function* () {
            var _a, _b;
            if (!confirmed)
                return;
            try {
                console.log('🚀 Eliminando ciclos...');
                // Eliminar cada ciclo
                const promises = [];
                $checked.each(function () {
                    const recId = $(this).data('recid');
                    if (recId) {
                        const url = `${apiBase}/PayCycles/${recId}`;
                        const promise = fetch(url, {
                            method: 'DELETE',
                            headers: { 'Accept': 'application/json' }
                        }).then(response => {
                            if (!response.ok) {
                                throw new Error(`Error al eliminar ciclo ${recId}`);
                            }
                        });
                        promises.push(promise);
                    }
                });
                yield Promise.all(promises);
                console.log('✅ Todos los ciclos eliminados');
                (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.ok(`Se eliminaron ${count} ciclo(s) exitosamente`, 'Éxito');
                // Recargar la tabla
                yield loadPayCycles();
            }
            catch (error) {
                console.error('❌ Error al eliminar ciclos:', error);
                (_b = w.ALERTS) === null || _b === void 0 ? void 0 : _b.error('Error al eliminar algunos ciclos', 'Error');
                yield loadPayCycles(); // Recargar de todas formas
            }
        }), { type: 'danger' });
    });
    // ========================================================================
    // UTILIDADES
    // ========================================================================
    const getStatusBadge = (status) => {
        const statusMap = {
            0: '<span class="label label-info">Abierto</span>',
            1: '<span class="label label-warning">Procesado</span>',
            2: '<span class="label label-success">Pagado</span>',
            3: '<span class="label label-primary">Registrado</span>'
        };
        return statusMap[status] || '<span class="label label-default">Desconocido</span>';
    };
    const formatDate = (dateStr) => {
        if (!dateStr)
            return '-';
        const date = new Date(dateStr);
        return date.toLocaleDateString('es-DO', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit'
        });
    };
    const formatCurrency = (amount) => {
        return new Intl.NumberFormat('es-DO', {
            style: 'currency',
            currency: 'DOP'
        }).format(amount || 0);
    };
    const updateButtonStates = () => {
        const checkedCount = $('.cycle-check:checked').length;
        $('#btn-delete-cycles').prop('disabled', checkedCount === 0);
    };
    // ========================================================================
    // EVENT HANDLERS
    // ========================================================================
    const setupEventHandlers = () => {
        console.log('🔧 Configurando event handlers de PayCycles');
        // Botón: Generar Ciclos
        $(document).off('click', '#btn-generate-cycles').on('click', '#btn-generate-cycles', function () {
            return __awaiter(this, void 0, void 0, function* () {
                console.log('🚀 Click en btn-generate-cycles');
                yield generatePayCycles();
            });
        });
        // Botón: Nuevo Ciclo (manual)
        $(document).off('click', '#btn-new-cycle').on('click', '#btn-new-cycle', function () {
            var _a;
            console.log('➕ Click en btn-new-cycle');
            if (!currentPayrollId || currentPayrollId === 0) {
                (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.error('Debe guardar la nómina antes de crear ciclos', 'Error');
                return;
            }
            // Limpiar formulario y abrir modal
            const form = $('#paycycle-form')[0];
            if (form)
                form.reset();
            $('#paycycle-recid').val('0');
            // Inicializar checkboxes ISR y TSS como desmarcados
            $('#paycycle-is-for-tax').iCheck('uncheck');
            $('#paycycle-is-for-tss').iCheck('uncheck');
            $('#modal-paycycle-title').text('Nuevo Ciclo de Pago');
            $('#modal-paycycle').modal('show');
        });
        // Botón: Editar Ciclo
        $(document).off('click', '.btn-edit-cycle').on('click', '.btn-edit-cycle', function () {
            return __awaiter(this, void 0, void 0, function* () {
                var _a, _b;
                if ($(this).prop('disabled')) {
                    console.warn('⚠️ Botón editar deshabilitado');
                    return;
                }
                const recId = $(this).data('recid');
                const cycleStatus = $(this).data('status');
                console.log('✏️ Click en btn-edit-cycle:', { recId, cycleStatus });
                // Validar que solo se puedan editar ciclos Open
                if (cycleStatus === 2 || cycleStatus === 3) {
                    (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.warning('No se pueden editar ciclos Pagados o Registrados', 'Validación');
                    return;
                }
                // Buscar el ciclo (PascalCase)
                const cycle = payCycles.find(c => c.RecID === recId);
                if (!cycle) {
                    (_b = w.ALERTS) === null || _b === void 0 ? void 0 : _b.error('Ciclo no encontrado', 'Error');
                    return;
                }
                // Llenar formulario (PascalCase)
                $('#paycycle-recid').val(cycle.RecID);
                $('#paycycle-start-date').val(cycle.PeriodStartDate.split('T')[0]);
                $('#paycycle-end-date').val(cycle.PeriodEndDate.split('T')[0]);
                $('#paycycle-pay-date').val(cycle.PayDate.split('T')[0]);
                $('#paycycle-amount').val(cycle.AmountPaidPerPeriod);
                $('#paycycle-observations').val(cycle.Observations || '');
                // Llenar checkboxes ISR y TSS
                if (cycle.IsForTax) {
                    $('#paycycle-is-for-tax').iCheck('check');
                }
                else {
                    $('#paycycle-is-for-tax').iCheck('uncheck');
                }
                if (cycle.IsForTss) {
                    $('#paycycle-is-for-tss').iCheck('check');
                }
                else {
                    $('#paycycle-is-for-tss').iCheck('uncheck');
                }
                $('#modal-paycycle-title').text('Editar Ciclo de Pago');
                $('#modal-paycycle').modal('show');
            });
        });
        // Botón: Eliminar Ciclo Individual
        $(document).off('click', '.btn-delete-cycle').on('click', '.btn-delete-cycle', function () {
            if ($(this).prop('disabled')) {
                console.warn('⚠️ Botón eliminar deshabilitado');
                return;
            }
            const cycleId = $(this).data('recid');
            const cycleStatus = $(this).data('status');
            console.log('🗑️ Click en btn-delete-cycle:', { cycleId, cycleStatus });
            deleteCycle(cycleId, cycleStatus);
        });
        // ✅ NUEVO: Botón Eliminar Seleccionados
        $(document).off('click', '#btn-delete-cycles').on('click', '#btn-delete-cycles', function () {
            return __awaiter(this, void 0, void 0, function* () {
                console.log('🗑️ Click en btn-delete-cycles');
                yield deleteSelectedCycles();
            });
        });
        // Check all cycles
        $(document).off('ifChanged', '#check-all-cycles').on('ifChanged', '#check-all-cycles', function () {
            const isChecked = $(this).is(':checked');
            console.log('☑️ Check all cycles:', isChecked);
            $('.cycle-check').iCheck(isChecked ? 'check' : 'uncheck');
        });
        // Individual cycle check
        $(document).off('ifChanged', '.cycle-check').on('ifChanged', '.cycle-check', function () {
            const total = $('.cycle-check').length;
            const checked = $('.cycle-check:checked').length;
            if (checked === total && total > 0) {
                $('#check-all-cycles').iCheck('check');
            }
            else {
                $('#check-all-cycles').iCheck('uncheck');
            }
            updateButtonStates();
        });
        // Guardar ciclo (desde modal) - Con ISR y TSS
        $(document).off('click', '#btn-save-paycycle').on('click', '#btn-save-paycycle', function () {
            return __awaiter(this, void 0, void 0, function* () {
                const recId = parseInt($('#paycycle-recid').val(), 10);
                const isNew = !recId || recId === 0;
                // Capturar valores de ISR y TSS
                const data = {
                    PeriodStartDate: $('#paycycle-start-date').val(),
                    PeriodEndDate: $('#paycycle-end-date').val(),
                    PayDate: $('#paycycle-pay-date').val(),
                    DefaultPayDate: $('#paycycle-pay-date').val(),
                    AmountPaidPerPeriod: parseFloat($('#paycycle-amount').val()) || 0,
                    StatusPeriod: 0, // Open
                    IsForTax: $('#paycycle-is-for-tax').is(':checked'),
                    IsForTss: $('#paycycle-is-for-tss').is(':checked'),
                    Observations: $('#paycycle-observations').val()
                };
                console.log('💾 Guardando ciclo:', data);
                if (isNew) {
                    yield createPayCycle(data);
                }
                else {
                    yield updatePayCycle(recId, data);
                }
            });
        });
        // Al cambiar de tab a "Ciclos de Pago", recargar si es necesario
        $('a[href="#tab_content2"]').on('shown.bs.tab', function () {
            console.log('📑 Tab Ciclos de Pago activado');
            const payrollIdFromDOM = getPayrollIdFromDOM();
            if (payrollIdFromDOM !== currentPayrollId) {
                console.log('🔄 PayrollId cambió, recargando...');
                currentPayrollId = payrollIdFromDOM;
                loadPayCycles();
            }
        });
        console.log('✅ Event handlers configurados');
    };
    // ========================================================================
    // INICIALIZACIÓN PÚBLICA
    // ========================================================================
    /**
     * Inicializa el módulo de ciclos de pago.
     * Debe ser llamado desde payroll-form.ts después de guardar el Payroll.
     */
    w.PayCyclesTab = {
        init: function (payrollId) {
            return __awaiter(this, void 0, void 0, function* () {
                console.log('🚀 PayCyclesTab.init() - Payroll ID:', payrollId);
                currentPayrollId = payrollId;
                setupEventHandlers();
                yield loadPayCycles();
            });
        },
        refresh: function () {
            return __awaiter(this, void 0, void 0, function* () {
                console.log('🔄 PayCyclesTab.refresh()');
                yield loadPayCycles();
            });
        },
        setPayrollId: function (payrollId) {
            console.log('📝 PayCyclesTab.setPayrollId():', payrollId);
            currentPayrollId = payrollId;
        }
    };
    // ========================================================================
    // AUTO-INICIALIZACIÓN
    // ========================================================================
    /**
     * Se ejecuta automáticamente al cargar el DOM.
     */
    $(function () {
        return __awaiter(this, void 0, void 0, function* () {
            console.log('📄 DOM ready - paycycles-tab.ts');
            // Obtener PayrollId desde el DOM
            currentPayrollId = getPayrollIdFromDOM();
            console.log('📊 PayrollId inicial:', currentPayrollId);
            // Configurar event handlers
            setupEventHandlers();
            // Cargar ciclos si estamos en modo edición
            if (currentPayrollId > 0) {
                console.log('✅ Modo edición detectado, cargando ciclos...');
                yield loadPayCycles();
            }
            else {
                console.log('ℹ️ Modo creación detectado, mostrando mensaje');
                showNewPayrollState();
            }
        });
    });
    console.log('✅ paycycles-tab.ts cargado y listo');
})();
//# sourceMappingURL=paycycles-tab.js.map