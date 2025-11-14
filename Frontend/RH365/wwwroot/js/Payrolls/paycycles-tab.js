// ============================================================================
// Archivo: paycycles-tab.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Payrolls/paycycles-tab.ts
// Descripción: 
//   - Gestión de Ciclos de Pago (PayCycles) dentro del formulario de Nómina
//   - CRUD completo con tabla dinámica
//   - Filtrado por PayrollRefRecID
//   - Modal para crear/editar
//   - Bloqueo total si estado es Pagado (2) o Registrado (3)
// ISO 27001: Control de ciclos de pago con validación y trazabilidad
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
    console.log('🔄 Inicializando paycycles-tab.ts...');
    const apiBase = (_b = (_a = w.RH365) === null || _a === void 0 ? void 0 : _a.urls) === null || _b === void 0 ? void 0 : _b.apiBase;
    if (!apiBase) {
        console.error('❌ API Base URL no está definida');
        return;
    }
    const pageContainer = d.querySelector("#payroll-form-page");
    if (!pageContainer) {
        console.error('❌ Contenedor #payroll-form-page no encontrado');
        return;
    }
    const token = pageContainer.getAttribute("data-token") || "";
    const recId = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    const isNew = pageContainer.getAttribute("data-isnew") === "true";
    console.log('📋 Configuración:', { token: token ? '✓' : '✗', recId, isNew });
    let payCyclesData = [];
    let currentEditingId = null;
    // Mapa de estados de período según GlobalsEnum
    const statusPeriodMap = {
        0: 'Abierto',
        1: 'Procesado',
        2: 'Pagado',
        3: 'Registrado'
    };
    // Estados que no permiten edición
    const LOCKED_STATUSES = [2, 3]; // Pagado y Registrado
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
        console.log('🌐 Fetch:', { url, method: (options === null || options === void 0 ? void 0 : options.method) || 'GET' });
        const response = yield fetch(url, Object.assign(Object.assign({}, options), { headers }));
        if (!response.ok) {
            const errorData = yield response.json().catch(() => ({}));
            console.error('❌ Error en fetch:', errorData);
            throw new Error(JSON.stringify(errorData));
        }
        return response.json();
    });
    // ========================================================================
    // UTILIDADES - VALIDACIONES
    // ========================================================================
    const isStatusLocked = (status) => {
        return LOCKED_STATUSES.includes(status);
    };
    const getStatusName = (status) => {
        return statusPeriodMap[status] || 'Desconocido';
    };
    // ========================================================================
    // CARGA DE DATOS
    // ========================================================================
    const loadPayCycles = () => __awaiter(this, void 0, void 0, function* () {
        var _a;
        console.log('🔄 Cargando ciclos de pago...', { isNew, recId });
        if (isNew || recId === 0) {
            console.log('ℹ️ Modo creación - mostrando estado vacío');
            showEmptyState();
            return;
        }
        try {
            const url = `${apiBase}/PayCycles?skip=0&take=100`;
            console.log('📡 Llamando API:', url);
            const response = yield fetchJson(url);
            console.log('✅ Respuesta del API:', response);
            let allCycles = [];
            if (Array.isArray(response)) {
                allCycles = response;
            }
            else if ((response === null || response === void 0 ? void 0 : response.Data) && Array.isArray(response.Data)) {
                allCycles = response.Data;
            }
            else {
                console.warn('⚠️ Respuesta inesperada del API:', response);
            }
            // Filtrar por PayrollRefRecID
            payCyclesData = allCycles.filter((cycle) => {
                console.log(`🔍 Comparando: cycle.PayrollRefRecID=${cycle.PayrollRefRecID} con recId=${recId}`);
                return cycle.PayrollRefRecID === recId;
            });
            console.log(`✅ ${payCyclesData.length} ciclos de pago cargados para nómina ${recId}`);
            renderTable();
        }
        catch (error) {
            console.error('❌ Error al cargar ciclos de pago:', error);
            (_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.error('Error al cargar ciclos de pago', 'Error');
            showEmptyState();
        }
    });
    // ========================================================================
    // RENDERIZADO
    // ========================================================================
    const showEmptyState = () => {
        console.log('📄 Mostrando estado vacío');
        const emptyHtml = isNew
            ? `<div class="alert alert-warning text-center">
                <i class="fa fa-info-circle"></i>
                <strong>Modo Creación:</strong> Guarda la nómina primero para agregar ciclos de pago.
            </div>`
            : `<div class="alert alert-info text-center">
                <i class="fa fa-info-circle"></i>
                No hay ciclos de pago registrados. Haz clic en "Nuevo Ciclo" para agregar uno.
            </div>`;
        const container = $('#paycycles-container');
        if (container.length) {
            container.html(emptyHtml);
            console.log('✅ Estado vacío renderizado');
        }
        else {
            console.error('❌ Contenedor #paycycles-container no encontrado');
        }
        const btnNew = $('#btn-new-cycle');
        if (btnNew.length) {
            btnNew.prop('disabled', isNew);
            console.log('✅ Botón nuevo ciclo configurado:', { disabled: isNew });
        }
    };
    const formatDate = (dateString) => {
        if (!dateString)
            return '';
        const date = new Date(dateString);
        if (isNaN(date.getTime()))
            return '';
        return date.toLocaleDateString('es-DO', { day: '2-digit', month: '2-digit', year: 'numeric' });
    };
    const formatCurrency = (value) => {
        if (value == null)
            return '$0.00';
        return new Intl.NumberFormat('es-DO', {
            style: 'currency',
            currency: 'DOP'
        }).format(value);
    };
    const getStatusLabel = (status) => {
        const labels = {
            0: '<span class="label label-success">Abierto</span>',
            1: '<span class="label label-info">Procesado</span>',
            2: '<span class="label label-warning">Pagado</span>',
            3: '<span class="label label-default">Registrado</span>'
        };
        return labels[status] || '<span class="label label-default">Desconocido</span>';
    };
    const renderTable = () => {
        console.log('🎨 Renderizando tabla con', payCyclesData.length, 'ciclos');
        if (payCyclesData.length === 0) {
            showEmptyState();
            return;
        }
        const tableRows = payCyclesData.map(cycle => {
            const isLocked = isStatusLocked(cycle.StatusPeriod);
            const editBtnClass = isLocked ? 'btn-default disabled' : 'btn-primary';
            const editBtnTitle = isLocked ? 'Ver (Solo Lectura)' : 'Editar';
            const editBtnIcon = isLocked ? 'fa-eye' : 'fa-pencil';
            return `
            <tr data-recid="${cycle.RecID}">
                <td class="text-center">
                    <input type="checkbox" class="flat cycle-check" data-recid="${cycle.RecID}"/>
                </td>
                <td>${cycle.ID || ''}</td>
                <td>${formatDate(cycle.PeriodStartDate)}</td>
                <td>${formatDate(cycle.PeriodEndDate)}</td>
                <td>${formatDate(cycle.PayDate)}</td>
                <td>${formatCurrency(cycle.AmountPaidPerPeriod)}</td>
                <td>${getStatusLabel(cycle.StatusPeriod)}</td>
                <td class="text-center">
                    ${cycle.IsForTax ? '<i class="fa fa-check text-success"></i>' : '<i class="fa fa-times text-muted"></i>'}
                </td>
                <td class="text-center">
                    ${cycle.IsForTss ? '<i class="fa fa-check text-success"></i>' : '<i class="fa fa-times text-muted"></i>'}
                </td>
                <td class="text-center">
                    <button type="button" class="btn btn-xs ${editBtnClass} btn-edit-cycle" 
                            data-recid="${cycle.RecID}" 
                            data-status="${cycle.StatusPeriod}"
                            title="${editBtnTitle}">
                        <i class="fa ${editBtnIcon}"></i>
                    </button>
                    <button type="button" class="btn btn-xs btn-danger btn-delete-cycle" 
                            data-recid="${cycle.RecID}"
                            data-status="${cycle.StatusPeriod}" 
                            title="Eliminar"
                            ${isLocked ? 'disabled' : ''}>
                        <i class="fa fa-trash"></i>
                    </button>
                </td>
            </tr>
        `;
        }).join('');
        const tableHtml = `
            <div class="table-responsive">
                <table class="table table-striped table-bordered">
                    <thead>
                        <tr>
                            <th style="width:40px;"><input type="checkbox" id="check-all-cycles" class="flat"/></th>
                            <th>ID</th>
                            <th>Inicio Período</th>
                            <th>Fin Período</th>
                            <th>Fecha de Pago</th>
                            <th>Monto</th>
                            <th>Estado</th>
                            <th style="width:80px;">Para Impuestos</th>
                            <th style="width:80px;">Para TSS</th>
                            <th style="width:100px;">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${tableRows}
                    </tbody>
                </table>
            </div>
        `;
        const container = $('#paycycles-container');
        if (container.length) {
            container.html(tableHtml);
            console.log('✅ Tabla renderizada');
        }
        const btnNew = $('#btn-new-cycle');
        if (btnNew.length) {
            btnNew.prop('disabled', false);
        }
        // Inicializar iCheck
        if ($.fn.iCheck) {
            $('.flat').iCheck({ checkboxClass: 'icheckbox_flat-green' });
            console.log('✅ iCheck inicializado');
        }
        updateButtonStates();
    };
    // ========================================================================
    // MODAL - CREAR/EDITAR
    // ========================================================================
    const openModal = (cycleId) => {
        console.log('📝 Abriendo modal:', { cycleId });
        currentEditingId = cycleId || null;
        const isEdit = cycleId !== undefined;
        // Resetear formulario
        const form = document.getElementById('frm-paycycle');
        if (form) {
            form.reset();
        }
        // Habilitar todos los campos por defecto
        $('#PeriodStartDate, #PeriodEndDate, #DefaultPayDate, #PayDate, #AmountPaidPerPeriod, #StatusPeriod, #ObservationsCycle').prop('readonly', false).prop('disabled', false);
        $('#IsForTax, #IsForTss').iCheck('enable');
        let isStatusLockedFlag = false;
        if (isEdit) {
            const cycle = payCyclesData.find(c => c.RecID === cycleId);
            if (cycle) {
                console.log('✏️ Cargando datos del ciclo:', cycle);
                isStatusLockedFlag = isStatusLocked(cycle.StatusPeriod);
                $('#PeriodStartDate').val(cycle.PeriodStartDate ? cycle.PeriodStartDate.split('T')[0] : '');
                $('#PeriodEndDate').val(cycle.PeriodEndDate ? cycle.PeriodEndDate.split('T')[0] : '');
                $('#DefaultPayDate').val(cycle.DefaultPayDate ? cycle.DefaultPayDate.split('T')[0] : '');
                $('#PayDate').val(cycle.PayDate ? cycle.PayDate.split('T')[0] : '');
                $('#AmountPaidPerPeriod').val(cycle.AmountPaidPerPeriod || 0);
                $('#StatusPeriod').val(cycle.StatusPeriod.toString());
                $('#IsForTax').prop('checked', cycle.IsForTax);
                $('#IsForTss').prop('checked', cycle.IsForTss);
                $('#ObservationsCycle').val(cycle.Observations || '');
                if (isStatusLockedFlag) {
                    // ESTADO PAGADO O REGISTRADO: Bloquear TODO
                    console.log('🔒 Estado PAGADO/REGISTRADO - Bloqueando TODOS los campos');
                    $('#PeriodStartDate, #PeriodEndDate, #DefaultPayDate, #PayDate').prop('readonly', true);
                    $('#AmountPaidPerPeriod').prop('readonly', true);
                    $('#StatusPeriod').prop('disabled', true);
                    $('#ObservationsCycle').prop('readonly', true);
                    $('#IsForTax, #IsForTss').iCheck('disable');
                    $('#modal-paycycle-title').html(`<i class="fa fa-eye"></i> Ver Ciclo de Pago - ${getStatusName(cycle.StatusPeriod)} (Solo Lectura)`);
                    $('#btn-save-cycle').hide();
                    // Mostrar alerta informativa
                    const alertHtml = `
                        <div class="alert alert-warning" role="alert" id="locked-alert">
                            <i class="fa fa-lock"></i> 
                            <strong>Solo Lectura:</strong> Este ciclo está en estado <strong>${getStatusName(cycle.StatusPeriod)}</strong> y no puede ser modificado.
                        </div>
                    `;
                    $('#frm-paycycle').prepend(alertHtml);
                }
                else {
                    // ESTADO ABIERTO O PROCESADO: Bloquear campos excepto IsForTax e IsForTss
                    console.log('🔓 Estado ABIERTO/PROCESADO - Bloqueando campos (excepto Para Impuestos y Para TSS)');
                    $('#PeriodStartDate, #PeriodEndDate, #DefaultPayDate, #PayDate').prop('readonly', true);
                    $('#AmountPaidPerPeriod').prop('readonly', true);
                    $('#StatusPeriod').prop('disabled', true);
                    $('#ObservationsCycle').prop('readonly', true);
                    $('#modal-paycycle-title').html('<i class="fa fa-pencil"></i> Editar Ciclo de Pago');
                    $('#btn-save-cycle').show();
                }
            }
        }
        else {
            $('#modal-paycycle-title').html('<i class="fa fa-plus"></i> Nuevo Ciclo de Pago');
            $('#btn-save-cycle').show();
        }
        // Actualizar iCheck
        if ($.fn.iCheck) {
            $('#IsForTax, #IsForTss').iCheck('update');
        }
        // Abrir modal
        $('#modal-paycycle').modal('show');
        console.log('✅ Modal abierto');
    };
    // ========================================================================
    // GUARDADO
    // ========================================================================
    const saveCycle = () => __awaiter(this, void 0, void 0, function* () {
        var _a, _b;
        console.log('💾 Guardando ciclo...');
        const form = document.getElementById('frm-paycycle');
        if (!form.checkValidity()) {
            form.reportValidity();
            console.warn('⚠️ Formulario inválido');
            return;
        }
        try {
            let payload;
            if (currentEditingId) {
                // MODO EDICIÓN: Solo enviar campos editables
                payload = {
                    IsForTax: $('#IsForTax').is(':checked'),
                    IsForTss: $('#IsForTss').is(':checked')
                };
                console.log('✏️ Modo edición - solo enviando IsForTax e IsForTss');
            }
            else {
                // MODO CREACIÓN: Enviar todos los campos
                payload = {
                    PayrollRefRecID: recId,
                    PeriodStartDate: $('#PeriodStartDate').val() ? new Date($('#PeriodStartDate').val()).toISOString() : null,
                    PeriodEndDate: $('#PeriodEndDate').val() ? new Date($('#PeriodEndDate').val()).toISOString() : null,
                    DefaultPayDate: $('#DefaultPayDate').val() ? new Date($('#DefaultPayDate').val()).toISOString() : null,
                    PayDate: $('#PayDate').val() ? new Date($('#PayDate').val()).toISOString() : null,
                    AmountPaidPerPeriod: parseFloat($('#AmountPaidPerPeriod').val()) || 0,
                    StatusPeriod: parseInt($('#StatusPeriod').val()),
                    IsForTax: $('#IsForTax').is(':checked'),
                    IsForTss: $('#IsForTss').is(':checked'),
                    Observations: $('#ObservationsCycle').val() || null
                };
                console.log('➕ Modo creación - enviando todos los campos');
            }
            console.log('📤 Payload a enviar:', payload);
            const url = currentEditingId
                ? `${apiBase}/PayCycles/${currentEditingId}`
                : `${apiBase}/PayCycles`;
            const method = currentEditingId ? 'PUT' : 'POST';
            yield fetchJson(url, {
                method: method,
                body: JSON.stringify(payload)
            });
            console.log('✅ Ciclo guardado exitosamente');
            if ((_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.ok) {
                w.ALERTS.ok(currentEditingId ? 'Ciclo actualizado exitosamente' : 'Ciclo creado exitosamente', 'Éxito');
            }
            $('#modal-paycycle').modal('hide');
            yield loadPayCycles();
        }
        catch (error) {
            console.error('❌ Error al guardar ciclo:', error);
            let errorMessage = 'Error al guardar el ciclo de pago';
            try {
                const errorData = JSON.parse(error.message);
                if (errorData.errors) {
                    const errorsArray = [];
                    for (const key in errorData.errors) {
                        if (errorData.errors.hasOwnProperty(key)) {
                            errorsArray.push(...errorData.errors[key]);
                        }
                    }
                    errorMessage = errorsArray.join(', ');
                }
                else if (errorData.title) {
                    errorMessage = errorData.title;
                }
            }
            catch (_c) {
                errorMessage = error.message || errorMessage;
            }
            if ((_b = w.ALERTS) === null || _b === void 0 ? void 0 : _b.error) {
                w.ALERTS.error(errorMessage, 'Error');
            }
        }
    });
    // ========================================================================
    // ELIMINACIÓN
    // ========================================================================
    const deleteCycle = (cycleId, cycleStatus) => __awaiter(this, void 0, void 0, function* () {
        var _a, _b;
        console.log('🗑️ Intentando eliminar ciclo:', cycleId);
        // Verificar si el estado está bloqueado
        if (isStatusLocked(cycleStatus)) {
            console.warn('⚠️ No se puede eliminar - Estado bloqueado:', getStatusName(cycleStatus));
            if ((_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.warn) {
                w.ALERTS.warn(`No se puede eliminar un ciclo en estado ${getStatusName(cycleStatus)}`, 'Operación no permitida');
            }
            return;
        }
        if (!((_b = w.ALERTS) === null || _b === void 0 ? void 0 : _b.confirm)) {
            console.error('❌ ALERTS.confirm no disponible');
            return;
        }
        w.ALERTS.confirm('¿Está seguro de eliminar este ciclo de pago?', 'Confirmar Eliminación', (confirmed) => __awaiter(this, void 0, void 0, function* () {
            var _a, _b;
            if (!confirmed) {
                console.log('❌ Eliminación cancelada por el usuario');
                return;
            }
            try {
                const url = `${apiBase}/PayCycles/${cycleId}`;
                yield fetchJson(url, { method: 'DELETE' });
                console.log('✅ Ciclo eliminado exitosamente');
                if ((_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.ok) {
                    w.ALERTS.ok('Ciclo eliminado exitosamente', 'Éxito');
                }
                yield loadPayCycles();
            }
            catch (error) {
                console.error('❌ Error al eliminar ciclo:', error);
                if ((_b = w.ALERTS) === null || _b === void 0 ? void 0 : _b.error) {
                    w.ALERTS.error('Error al eliminar el ciclo de pago', 'Error');
                }
            }
        }), { type: 'danger' });
    });
    const deleteSelectedCycles = () => __awaiter(this, void 0, void 0, function* () {
        var _a, _b;
        const $checked = $('.cycle-check:checked');
        const count = $checked.length;
        console.log('🗑️ Intentando eliminar', count, 'ciclos seleccionados');
        if (count === 0)
            return;
        // Verificar si alguno tiene estado bloqueado
        let hasLockedCycles = false;
        $checked.each(function () {
            const $row = $(this).closest('tr');
            const recId = $(this).data('recid');
            const cycle = payCyclesData.find(c => c.RecID === recId);
            if (cycle && isStatusLocked(cycle.StatusPeriod)) {
                hasLockedCycles = true;
            }
        });
        if (hasLockedCycles) {
            if ((_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.warn) {
                w.ALERTS.warn('Algunos ciclos seleccionados están en estado Pagado o Registrado y no pueden ser eliminados', 'Operación no permitida');
            }
            return;
        }
        const message = count === 1
            ? '¿Está seguro de eliminar este ciclo de pago?'
            : `¿Está seguro de eliminar ${count} ciclos de pago?`;
        if (!((_b = w.ALERTS) === null || _b === void 0 ? void 0 : _b.confirm)) {
            console.error('❌ ALERTS.confirm no disponible');
            return;
        }
        w.ALERTS.confirm(message, 'Confirmar Eliminación', (confirmed) => __awaiter(this, void 0, void 0, function* () {
            var _a, _b;
            if (!confirmed)
                return;
            try {
                const promises = [];
                $checked.each(function () {
                    const cycleId = $(this).data('recid');
                    if (cycleId) {
                        const url = `${apiBase}/PayCycles/${cycleId}`;
                        promises.push(fetchJson(url, { method: 'DELETE' }));
                    }
                });
                yield Promise.all(promises);
                console.log('✅ Ciclos eliminados exitosamente');
                if ((_a = w.ALERTS) === null || _a === void 0 ? void 0 : _a.ok) {
                    w.ALERTS.ok('Ciclo(s) eliminado(s) exitosamente', 'Éxito');
                }
                yield loadPayCycles();
            }
            catch (error) {
                console.error('❌ Error al eliminar ciclos:', error);
                if ((_b = w.ALERTS) === null || _b === void 0 ? void 0 : _b.error) {
                    w.ALERTS.error('Error al eliminar ciclo(s) de pago', 'Error');
                }
            }
        }), { type: 'danger' });
    });
    // ========================================================================
    // ESTADOS DE BOTONES
    // ========================================================================
    const updateButtonStates = () => {
        const checkedCount = $('.cycle-check:checked').length;
        const btnDelete = $('#btn-delete-cycles');
        if (btnDelete.length) {
            btnDelete.prop('disabled', checkedCount === 0);
        }
    };
    // ========================================================================
    // EVENT HANDLERS
    // ========================================================================
    const setupEventHandlers = () => {
        console.log('🔧 Configurando event handlers...');
        // Botón nuevo ciclo
        $('#btn-new-cycle').off('click').on('click', function () {
            console.log('➕ Click en btn-new-cycle');
            openModal();
        });
        // Botón guardar ciclo
        $('#btn-save-cycle').off('click').on('click', function () {
            return __awaiter(this, void 0, void 0, function* () {
                console.log('💾 Click en btn-save-cycle');
                yield saveCycle();
            });
        });
        // Botón eliminar seleccionados
        $('#btn-delete-cycles').off('click').on('click', function () {
            return __awaiter(this, void 0, void 0, function* () {
                console.log('🗑️ Click en btn-delete-cycles');
                yield deleteSelectedCycles();
            });
        });
        // Botón editar/ver ciclo (delegado)
        $(document).off('click', '.btn-edit-cycle').on('click', '.btn-edit-cycle', function () {
            const cycleId = $(this).data('recid');
            console.log('✏️ Click en btn-edit-cycle:', cycleId);
            openModal(cycleId);
        });
        // Botón eliminar ciclo (delegado)
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
        // Limpiar alerta al cerrar modal
        $('#modal-paycycle').on('hidden.bs.modal', function () {
            $('#locked-alert').remove();
        });
        console.log('✅ Event handlers configurados');
    };
    // ========================================================================
    // INICIALIZACIÓN
    // ========================================================================
    const initialize = () => __awaiter(this, void 0, void 0, function* () {
        console.log('🚀 Inicializando módulo PayCycles...');
        try {
            setupEventHandlers();
            yield loadPayCycles();
            console.log('✅ Módulo PayCycles inicializado correctamente');
        }
        catch (error) {
            console.error('❌ Error al inicializar PayCycles:', error);
            showEmptyState();
        }
    });
    // Ejecutar al cargar el DOM
    $(function () {
        return __awaiter(this, void 0, void 0, function* () {
            console.log('📄 DOM listo - ejecutando initialize()');
            yield initialize();
        });
    });
    console.log('✅ paycycles-tab.ts cargado');
})();
//# sourceMappingURL=paycycles-tab.js.map