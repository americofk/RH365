// ============================================================================
// Archivo: grid-views-manager.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/GridViews/grid-views-manager.ts
// Descripción: Gestor de vistas de usuario - Comunicación con API
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
class GridViewsManager {
    constructor(apiBase, token, entityName, userRefRecID, dataareaId) {
        this.currentView = null;
        this.availableViews = [];
        this.apiBase = apiBase;
        this.token = token;
        this.entityName = entityName;
        this.userRefRecID = userRefRecID;
        this.dataareaId = dataareaId;
    }
    initialize() {
        return __awaiter(this, void 0, void 0, function* () {
            yield this.loadAvailableViews();
            const defaultView = yield this.loadDefaultView();
            if (defaultView) {
                this.currentView = defaultView;
                return this.parseViewConfig(defaultView.ViewConfig);
            }
            return [];
        });
    }
    loadDefaultView() {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                const defaultView = this.availableViews.find(v => v.UserRefRecID === this.userRefRecID &&
                    v.IsDefault === true);
                return defaultView || null;
            }
            catch (error) {
                window.ALERTS.error('Error al cargar vista predeterminada', 'Error');
                return null;
            }
        });
    }
    loadAvailableViews() {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                const url = `${this.apiBase}/UserGridViews?entityName=${this.entityName}&dataareaId=${this.dataareaId}`;
                const response = yield fetch(url, {
                    headers: {
                        'Authorization': `Bearer ${this.token}`,
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
                });
                if (!response.ok) {
                    const errorText = yield response.text();
                    throw new Error(`HTTP ${response.status}: ${errorText}`);
                }
                const result = yield response.json();
                let views = [];
                if (result.items && Array.isArray(result.items)) {
                    views = result.items;
                }
                else if (result.Data && Array.isArray(result.Data)) {
                    views = result.Data;
                }
                else if (Array.isArray(result)) {
                    views = result;
                }
                this.availableViews = views.filter(v => v.UserRefRecID === this.userRefRecID || v.IsPublic);
            }
            catch (error) {
                window.ALERTS.error('Error al cargar las vistas disponibles', 'Error');
                this.availableViews = [];
            }
        });
    }
    saveView(viewName_1, columns_1) {
        return __awaiter(this, arguments, void 0, function* (viewName, columns, isDefault = false, isPublic = false) {
            try {
                const viewConfig = { columns };
                const response = yield fetch(`${this.apiBase}/UserGridViews`, {
                    method: 'POST',
                    headers: {
                        'Authorization': `Bearer ${this.token}`,
                        'Content-Type': 'application/json',
                        'Accept': 'application/json'
                    },
                    body: JSON.stringify({
                        UserRefRecID: this.userRefRecID,
                        EntityName: this.entityName,
                        ViewName: viewName,
                        ViewConfig: JSON.stringify(viewConfig),
                        IsDefault: isDefault,
                        IsPublic: isPublic,
                        DataareaID: this.dataareaId,
                        ViewType: 'Grid',
                        ViewScope: isPublic ? 'Public' : 'Private',
                        SchemaVersion: 1,
                        IsLocked: false
                    })
                });
                if (!response.ok) {
                    const errorText = yield response.text();
                    throw new Error(`HTTP ${response.status}: ${errorText}`);
                }
                const savedView = yield response.json();
                this.currentView = savedView;
                yield this.loadAvailableViews();
                window.ALERTS.ok(`Vista "${viewName}" guardada exitosamente`, 'Éxito');
                return true;
            }
            catch (error) {
                window.ALERTS.error('Error al guardar la vista', 'Error');
                return false;
            }
        });
    }
    updateView(columns) {
        return __awaiter(this, void 0, void 0, function* () {
            if (!this.currentView) {
                window.ALERTS.warn('No hay vista seleccionada para actualizar', 'Advertencia');
                return false;
            }
            try {
                const viewConfig = { columns };
                const response = yield fetch(`${this.apiBase}/UserGridViews`, {
                    method: 'PUT',
                    headers: {
                        'Authorization': `Bearer ${this.token}`,
                        'Content-Type': 'application/json',
                        'Accept': 'application/json'
                    },
                    body: JSON.stringify({
                        RecID: this.currentView.RecID,
                        DataareaID: this.dataareaId,
                        UserRefRecID: this.currentView.UserRefRecID,
                        EntityName: this.currentView.EntityName,
                        ViewType: this.currentView.ViewType || 'Grid',
                        ViewScope: this.currentView.ViewScope || 'Private',
                        RoleRefRecID: null,
                        ViewName: this.currentView.ViewName,
                        ViewDescription: null,
                        IsDefault: this.currentView.IsDefault,
                        IsLocked: false,
                        ViewConfig: JSON.stringify(viewConfig),
                        SchemaVersion: 1,
                        Checksum: null,
                        Tags: null,
                        Observations: null,
                        ConcurrencyToken: null
                    })
                });
                if (!response.ok) {
                    const errorText = yield response.text();
                    throw new Error(`HTTP ${response.status}`);
                }
                const updated = yield response.json();
                this.currentView = updated;
                window.ALERTS.ok(`Vista "${this.currentView.ViewName}" actualizada`, 'Éxito');
                return true;
            }
            catch (error) {
                window.ALERTS.error('Error al actualizar la vista', 'Error');
                return false;
            }
        });
    }
    setDefaultView(recId) {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                const url = `${this.apiBase}/UserGridViews/${recId}/set-default?dataareaId=${this.dataareaId}`;
                const response = yield fetch(url, {
                    method: 'POST',
                    headers: {
                        'Authorization': `Bearer ${this.token}`,
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
                });
                if (!response.ok) {
                    const errorText = yield response.text();
                    throw new Error(`HTTP ${response.status}`);
                }
                yield this.loadAvailableViews();
                window.ALERTS.ok('Vista establecida como predeterminada', 'Éxito');
                return true;
            }
            catch (error) {
                window.ALERTS.error('Error al establecer vista predeterminada', 'Error');
                return false;
            }
        });
    }
    deleteView(recId) {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                const url = `${this.apiBase}/UserGridViews/${recId}?dataareaId=${this.dataareaId}`;
                const response = yield fetch(url, {
                    method: 'DELETE',
                    headers: {
                        'Authorization': `Bearer ${this.token}`,
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
                });
                if (!response.ok) {
                    const errorText = yield response.text();
                    throw new Error(`HTTP ${response.status}`);
                }
                yield this.loadAvailableViews();
                if (this.currentView && this.currentView.RecID === recId) {
                    this.currentView = null;
                }
                window.ALERTS.ok('Vista eliminada exitosamente', 'Éxito');
                return true;
            }
            catch (error) {
                window.ALERTS.error('Error al eliminar la vista', 'Error');
                return false;
            }
        });
    }
    loadView(recId) {
        return __awaiter(this, void 0, void 0, function* () {
            const view = this.availableViews.find(v => v.RecID === recId);
            if (!view) {
                window.ALERTS.warn('Vista no encontrada', 'Advertencia');
                return [];
            }
            this.currentView = view;
            return this.parseViewConfig(view.ViewConfig);
        });
    }
    parseViewConfig(configJson) {
        try {
            const config = JSON.parse(configJson);
            return config.columns || [];
        }
        catch (error) {
            window.ALERTS.error('Error al cargar configuración de vista', 'Error');
            return [];
        }
    }
    getAvailableViews() {
        return this.availableViews;
    }
    getCurrentView() {
        return this.currentView;
    }
    getCurrentViewName() {
        return this.currentView ? this.currentView.ViewName : 'Vista por defecto';
    }
    hasCurrentView() {
        return this.currentView !== null;
    }
}
window.GridViewsManager = GridViewsManager;
//# sourceMappingURL=grid-views-manager.js.map