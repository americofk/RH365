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
        console.log('GridViewsManager constructor:');
        console.log('  - apiBase:', apiBase);
        console.log('  - token:', token ? '✓ Presente' : '❌ VACÍO');
        console.log('  - entityName:', entityName);
        console.log('  - userRefRecID:', userRefRecID);
        console.log('  - dataareaId:', dataareaId);
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
                if (defaultView) {
                    console.log(`✓ Vista por defecto encontrada: "${defaultView.ViewName}"`);
                    return defaultView;
                }
                console.log('ℹ No hay vista por defecto configurada');
                return null;
            }
            catch (error) {
                console.error('Error cargando vista por defecto:', error);
                return null;
            }
        });
    }
    loadAvailableViews() {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                const url = `${this.apiBase}/UserGridViews?entityName=${this.entityName}&dataareaId=${this.dataareaId}`;
                console.log('Cargando vistas desde:', url);
                const response = yield fetch(url, {
                    headers: {
                        'Authorization': `Bearer ${this.token}`,
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
                });
                if (!response.ok) {
                    const errorText = yield response.text();
                    console.error('Error en respuesta:', errorText);
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
                else {
                    console.warn('Formato de respuesta inesperado:', result);
                    views = [];
                }
                console.log(`✓ ${views.length} vistas recibidas del API`);
                this.availableViews = views.filter(v => v.UserRefRecID === this.userRefRecID || v.IsPublic);
                console.log(`✓ ${this.availableViews.length} vistas disponibles para el usuario`);
                if (this.availableViews.length > 0) {
                    this.availableViews.forEach(v => {
                        console.log(`  - ${v.ViewName}${v.IsDefault ? ' (Predeterminada)' : ''}`);
                    });
                }
            }
            catch (error) {
                console.error('Error cargando vistas disponibles:', error);
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
                    console.error('Error guardando vista:', errorText);
                    throw new Error(`HTTP ${response.status}: ${errorText}`);
                }
                const savedView = yield response.json();
                this.currentView = savedView;
                yield this.loadAvailableViews();
                console.log(`✓ Vista "${viewName}" guardada exitosamente`);
                return true;
            }
            catch (error) {
                console.error('Error guardando vista:', error);
                alert('Error al guardar la vista. Por favor intenta nuevamente.');
                return false;
            }
        });
    }
    updateView(columns) {
        return __awaiter(this, void 0, void 0, function* () {
            if (!this.currentView) {
                console.warn('No hay vista actual para actualizar');
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
                    console.error('Error actualizando vista:', errorText);
                    throw new Error(`HTTP ${response.status}`);
                }
                const updated = yield response.json();
                this.currentView = updated;
                console.log(`✓ Vista "${this.currentView.ViewName}" actualizada`);
                return true;
            }
            catch (error) {
                console.error('Error actualizando vista:', error);
                alert('Error al actualizar la vista.');
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
                    console.error('Error estableciendo vista por defecto:', errorText);
                    throw new Error(`HTTP ${response.status}`);
                }
                yield this.loadAvailableViews();
                console.log('✓ Vista establecida como predeterminada');
                return true;
            }
            catch (error) {
                console.error('Error estableciendo vista por defecto:', error);
                alert('Error al establecer vista predeterminada.');
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
                    console.error('Error eliminando vista:', errorText);
                    throw new Error(`HTTP ${response.status}`);
                }
                yield this.loadAvailableViews();
                if (this.currentView && this.currentView.RecID === recId) {
                    this.currentView = null;
                }
                console.log('✓ Vista eliminada exitosamente');
                return true;
            }
            catch (error) {
                console.error('Error eliminando vista:', error);
                alert('Error al eliminar la vista.');
                return false;
            }
        });
    }
    loadView(recId) {
        return __awaiter(this, void 0, void 0, function* () {
            const view = this.availableViews.find(v => v.RecID === recId);
            if (!view) {
                console.warn(`Vista con RecID ${recId} no encontrada`);
                return [];
            }
            this.currentView = view;
            console.log(`✓ Vista "${view.ViewName}" cargada`);
            return this.parseViewConfig(view.ViewConfig);
        });
    }
    parseViewConfig(configJson) {
        try {
            const config = JSON.parse(configJson);
            return config.columns || [];
        }
        catch (error) {
            console.error('Error parseando configuración de vista:', error);
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