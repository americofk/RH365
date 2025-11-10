// ============================================================================
// Archivo: position-requirements.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Positions/position-requirements.ts
// Descripción:
//   - Gestión de Requisitos de Posición (línea por línea)
//   - CRUD completo de requisitos
//   - Integración con API REST (/api/PositionRequirements)
//   - Filtrado en cliente por PositionRefRecID
//   - Tab independiente dentro del formulario de posición
// Estándar: ISO 27001 - Control A.12.1.1 (Procedimientos operacionales documentados)
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
    const pageContainer = d.querySelector("#position-form-page");
    if (!pageContainer)
        return;
    const apiBase = w.RH365.urls.apiBase;
    const token = pageContainer.getAttribute("data-token") || "";
    const positionRecID = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    const isNewPosition = pageContainer.getAttribute("data-isnew") === "true";
    // ========================================================================
    // ESTADO
    // ========================================================================
    let requirements = [];
    let editingRequirementRecID = null;
    // ========================================================================
    // COMUNICACIÓN CON API
    // ========================================================================
    /**
     * Realiza peticiones HTTP al API con manejo de autenticación
     */
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
        // Si es DELETE y retorna 204 No Content, no intenta parsear JSON
        if (response.status === 204) {
            return null;
        }
        return response.json();
    });
    // ========================================================================
    // CARGA DE REQUISITOS (CON FILTRADO EN FRONTEND)
    // ========================================================================
    /**
     * Cargar TODOS los requisitos desde el API y filtrar por PositionRefRecID
     */
    const loadRequirements = () => __awaiter(this, void 0, void 0, function* () {
        if (isNewPosition) {
            requirements = [];
            renderRequirementsTab();
            return;
        }
        try {
            // Traer TODOS los requisitos (ajusta el take según necesites)
            const url = `${apiBase}/PositionRequirements?skip=0&take=1000`;
            const response = yield fetchJson(url);
            // Filtrar solo los requisitos de esta posición
            if (Array.isArray(response)) {
                // Si el endpoint retorna array directo
                requirements = response.filter((req) => req.PositionRefRecID === positionRecID);
            }
            else if ((response === null || response === void 0 ? void 0 : response.Data) && Array.isArray(response.Data)) {
                // Si el endpoint retorna objeto con propiedad Data (paginado)
                requirements = response.Data.filter((req) => req.PositionRefRecID === positionRecID);
            }
            else {
                console.warn('Formato de respuesta inesperado:', response);
                requirements = [];
            }
            renderRequirementsTab();
        }
        catch (error) {
            console.error('Error al cargar requisitos:', error);
            requirements = [];
            renderRequirementsTab();
        }
    });
    // ========================================================================
    // RENDERIZADO
    // ========================================================================
    /**
     * Escapa caracteres HTML para prevenir XSS
     */
    const escapeHtml = (text) => {
        if (!text)
            return '';
        const map = {
            '&': '&amp;',
            '<': '&lt;',
            '>': '&gt;',
            '"': '&quot;',
            "'": '&#039;'
        };
        return String(text).replace(/[&<>"']/g, m => map[m]);
    };
    /**
     * Renderizar el tab de requisitos con la tabla o mensajes informativos
     */
    const renderRequirementsTab = () => {
        const container = $('#requirements-container');
        container.empty();
        // Modo creación: no permite agregar requisitos hasta guardar la posición
        if (isNewPosition) {
            container.html(`
                <div class="alert alert-warning" role="alert">
                    <i class="fa fa-info-circle"></i>
                    <strong>Modo Creación:</strong> 
                    Primero debe guardar la posición para poder agregar requisitos.
                </div>
            `);
            $('#btn-add-requirement').prop('disabled', true);
            return;
        }
        // Habilitar botón de agregar requisito en modo edición
        $('#btn-add-requirement').prop('disabled', false);
        // Si no hay requisitos, mostrar mensaje informativo
        if (requirements.length === 0) {
            container.html(`
                <div class="alert alert-info" role="alert">
                    <i class="fa fa-lightbulb-o"></i>
                    No hay requisitos registrados. Haga clic en <strong>"Agregar Requisito"</strong> para comenzar.
                </div>
            `);
            return;
        }
        // Renderizar tabla con requisitos existentes
        const tableHtml = `
            <div class="table-responsive">
                <table class="table table-striped table-bordered table-hover">
                    <thead>
                        <tr>
                            <th style="width: 20%;">Nombre</th>
                            <th style="width: 30%;">Detalle</th>
                            <th style="width: 35%;">Observaciones</th>
                            <th style="width: 15%;" class="text-center">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${requirements.map(req => `
                            <tr data-recid="${req.RecID}">
                                <td>${escapeHtml(req.Name || '')}</td>
                                <td>${escapeHtml(req.Detail || '')}</td>
                                <td>${escapeHtml(req.Observations || '')}</td>
                                <td class="text-center">
                                    <button type="button" 
                                            class="btn btn-xs btn-primary btn-edit-requirement" 
                                            data-recid="${req.RecID}" 
                                            title="Editar">
                                        <i class="fa fa-pencil"></i>
                                    </button>
                                    <button type="button" 
                                            class="btn btn-xs btn-danger btn-delete-requirement" 
                                            data-recid="${req.RecID}" 
                                            title="Eliminar">
                                        <i class="fa fa-trash"></i>
                                    </button>
                                </td>
                            </tr>
                        `).join('')}
                    </tbody>
                </table>
            </div>
        `;
        container.html(tableHtml);
    };
    // ========================================================================
    // MODAL - AGREGAR/EDITAR REQUISITO
    // ========================================================================
    /**
     * Abrir modal en modo AGREGAR (nuevo requisito)
     */
    const openAddRequirementModal = () => {
        editingRequirementRecID = null;
        $('#requirement-name').val('');
        $('#requirement-detail').val('');
        $('#requirement-observations').val('');
        $('#modal-requirement-title').text('Agregar Requisito');
        $('#modal-requirement').modal('show');
    };
    /**
     * Abrir modal en modo EDITAR (requisito existente)
     */
    const openEditRequirementModal = (recID) => {
        const requirement = requirements.find(r => r.RecID === recID);
        if (!requirement) {
            w.ALERTS.error('Requisito no encontrado', 'Error');
            return;
        }
        editingRequirementRecID = recID;
        $('#requirement-name').val(requirement.Name);
        $('#requirement-detail').val(requirement.Detail);
        $('#requirement-observations').val(requirement.Observations || '');
        $('#modal-requirement-title').text('Editar Requisito');
        $('#modal-requirement').modal('show');
    };
    // ========================================================================
    // CRUD - CREAR REQUISITO
    // ========================================================================
    /**
     * Crear nuevo requisito en el API
     */
    const createRequirement = () => __awaiter(this, void 0, void 0, function* () {
        const name = $('#requirement-name').val().trim();
        const detail = $('#requirement-detail').val().trim();
        const observations = $('#requirement-observations').val().trim();
        // Validaciones
        if (!name) {
            w.ALERTS.warn('El campo "Nombre" es obligatorio', 'Validación');
            return;
        }
        if (!detail) {
            w.ALERTS.warn('El campo "Detalle" es obligatorio', 'Validación');
            return;
        }
        try {
            const payload = {
                Name: name,
                Detail: detail,
                PositionRefRecID: positionRecID,
                Observations: observations || null
            };
            const url = `${apiBase}/PositionRequirements`;
            const newRequirement = yield fetchJson(url, {
                method: 'POST',
                body: JSON.stringify(payload)
            });
            // Agregar al array local
            requirements.push(newRequirement);
            renderRequirementsTab();
            $('#modal-requirement').modal('hide');
            w.ALERTS.ok('Requisito agregado exitosamente', 'Éxito');
        }
        catch (error) {
            console.error('Error al crear requisito:', error);
            let errorMessage = 'Error al crear el requisito';
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
                    errorMessage = errorsArray.join(', ');
                }
                else if (errorData.title) {
                    errorMessage = errorData.title;
                }
            }
            catch (_a) {
                errorMessage = error.message || errorMessage;
            }
            w.ALERTS.error(errorMessage, 'Error');
        }
    });
    // ========================================================================
    // CRUD - ACTUALIZAR REQUISITO
    // ========================================================================
    /**
     * Actualizar requisito existente en el API
     */
    const updateRequirement = () => __awaiter(this, void 0, void 0, function* () {
        if (!editingRequirementRecID) {
            w.ALERTS.error('No hay requisito seleccionado para editar', 'Error');
            return;
        }
        const name = $('#requirement-name').val().trim();
        const detail = $('#requirement-detail').val().trim();
        const observations = $('#requirement-observations').val().trim();
        // Validaciones
        if (!name) {
            w.ALERTS.warn('El campo "Nombre" es obligatorio', 'Validación');
            return;
        }
        if (!detail) {
            w.ALERTS.warn('El campo "Detalle" es obligatorio', 'Validación');
            return;
        }
        try {
            const payload = {
                Name: name,
                Detail: detail,
                PositionRefRecID: positionRecID,
                Observations: observations || null
            };
            const url = `${apiBase}/PositionRequirements/${editingRequirementRecID}`;
            const updatedRequirement = yield fetchJson(url, {
                method: 'PUT',
                body: JSON.stringify(payload)
            });
            // Actualizar en el array local
            const index = requirements.findIndex(r => r.RecID === editingRequirementRecID);
            if (index !== -1) {
                requirements[index] = updatedRequirement;
            }
            renderRequirementsTab();
            $('#modal-requirement').modal('hide');
            w.ALERTS.ok('Requisito actualizado exitosamente', 'Éxito');
        }
        catch (error) {
            console.error('Error al actualizar requisito:', error);
            let errorMessage = 'Error al actualizar el requisito';
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
                    errorMessage = errorsArray.join(', ');
                }
                else if (errorData.title) {
                    errorMessage = errorData.title;
                }
            }
            catch (_a) {
                errorMessage = error.message || errorMessage;
            }
            w.ALERTS.error(errorMessage, 'Error');
        }
    });
    // ========================================================================
    // CRUD - ELIMINAR REQUISITO
    // ========================================================================
    /**
     * Eliminar requisito del API con confirmación
     */
    const deleteRequirement = (recID) => {
        w.ALERTS.confirm('¿Está seguro de eliminar este requisito?', 'Confirmar Eliminación', (confirmed) => __awaiter(this, void 0, void 0, function* () {
            if (!confirmed)
                return;
            try {
                const url = `${apiBase}/PositionRequirements/${recID}`;
                yield fetchJson(url, { method: 'DELETE' });
                // Eliminar del array local
                requirements = requirements.filter(r => r.RecID !== recID);
                renderRequirementsTab();
                w.ALERTS.ok('Requisito eliminado exitosamente', 'Éxito');
            }
            catch (error) {
                console.error('Error al eliminar requisito:', error);
                let errorMessage = 'Error al eliminar el requisito';
                try {
                    const errorData = JSON.parse(error.message);
                    if (errorData.message) {
                        errorMessage = errorData.message;
                    }
                    else if (errorData.title) {
                        errorMessage = errorData.title;
                    }
                }
                catch (_a) {
                    errorMessage = error.message || errorMessage;
                }
                w.ALERTS.error(errorMessage, 'Error');
            }
        }), { type: 'danger' });
    };
    // ========================================================================
    // EVENT HANDLERS
    // ========================================================================
    /**
     * Botón "Agregar Requisito"
     */
    $('#btn-add-requirement').on('click', () => {
        openAddRequirementModal();
    });
    /**
     * Botones "Editar" en la tabla (delegación de eventos)
     */
    $(document).on('click', '.btn-edit-requirement', function () {
        const recID = parseInt($(this).data('recid'), 10);
        openEditRequirementModal(recID);
    });
    /**
     * Botones "Eliminar" en la tabla (delegación de eventos)
     */
    $(document).on('click', '.btn-delete-requirement', function () {
        const recID = parseInt($(this).data('recid'), 10);
        deleteRequirement(recID);
    });
    /**
     * Botón "Guardar" en el modal
     */
    $('#btn-save-requirement').on('click', () => __awaiter(this, void 0, void 0, function* () {
        if (editingRequirementRecID) {
            yield updateRequirement();
        }
        else {
            yield createRequirement();
        }
    }));
    // ========================================================================
    // INICIALIZACIÓN
    // ========================================================================
    /**
     * Cargar requisitos al iniciar el módulo
     */
    $(function () {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                yield loadRequirements();
            }
            catch (error) {
                console.error('Error al inicializar módulo de requisitos:', error);
                w.ALERTS.error('Error al inicializar requisitos', 'Error');
            }
        });
    });
})();
//# sourceMappingURL=position-requirements.js.map