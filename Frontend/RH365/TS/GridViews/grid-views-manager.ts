// ============================================================================
// Archivo: grid-views-manager.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/GridViews/grid-views-manager.ts
// Descripción: Gestor de vistas de usuario - Comunicación con API
// ============================================================================

interface ColumnConfig {
    field: string;
    visible: boolean;
    order: number;
    width?: number;
}

interface ViewConfig {
    columns: ColumnConfig[];
    pageSize?: number;
}

interface UserGridView {
    RecID: number;
    EntityName: string;
    ViewName: string;
    ViewType?: string;
    ViewScope?: string;
    IsDefault: boolean;
    IsPublic: boolean;
    ViewConfig: string;
    UserRefRecID: number;
    CreatedBy: string;
    CreatedOn: string;
}

class GridViewsManager {
    private apiBase: string;
    private token: string;
    private entityName: string;
    private userRefRecID: number;
    private dataareaId: string;
    private currentView: UserGridView | null = null;
    private availableViews: UserGridView[] = [];

    constructor(apiBase: string, token: string, entityName: string, userRefRecID: number, dataareaId: string) {
        this.apiBase = apiBase;
        this.token = token;
        this.entityName = entityName;
        this.userRefRecID = userRefRecID;
        this.dataareaId = dataareaId;
    }

    async initialize(): Promise<ColumnConfig[]> {
        await this.loadAvailableViews();
        const defaultView = await this.loadDefaultView();
        if (defaultView) {
            this.currentView = defaultView;
            return this.parseViewConfig(defaultView.ViewConfig);
        }
        return [];
    }

    private async loadDefaultView(): Promise<UserGridView | null> {
        try {
            const defaultView = this.availableViews.find(v =>
                v.UserRefRecID === this.userRefRecID &&
                v.IsDefault === true
            );
            return defaultView || null;
        } catch (error) {
            (window as any).ALERTS.error('Error al cargar vista predeterminada', 'Error');
            return null;
        }
    }

    private async loadAvailableViews(): Promise<void> {
        try {
            const url = `${this.apiBase}/UserGridViews?entityName=${this.entityName}&dataareaId=${this.dataareaId}`;

            const response = await fetch(url, {
                headers: {
                    'Authorization': `Bearer ${this.token}`,
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(`HTTP ${response.status}: ${errorText}`);
            }

            const result = await response.json();
            let views: UserGridView[] = [];

            if (result.items && Array.isArray(result.items)) {
                views = result.items;
            } else if (result.Data && Array.isArray(result.Data)) {
                views = result.Data;
            } else if (Array.isArray(result)) {
                views = result;
            }

            this.availableViews = views.filter(v =>
                v.UserRefRecID === this.userRefRecID || v.IsPublic
            );

        } catch (error) {
            (window as any).ALERTS.error('Error al cargar las vistas disponibles', 'Error');
            this.availableViews = [];
        }
    }

    async saveView(viewName: string, columns: ColumnConfig[], isDefault: boolean = false, isPublic: boolean = false): Promise<boolean> {
        try {
            const viewConfig: ViewConfig = { columns };
            const response = await fetch(`${this.apiBase}/UserGridViews`, {
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
                const errorText = await response.text();
                throw new Error(`HTTP ${response.status}: ${errorText}`);
            }

            const savedView = await response.json();
            this.currentView = savedView;
            await this.loadAvailableViews();

            (window as any).ALERTS.ok(`Vista "${viewName}" guardada exitosamente`, 'Éxito');
            return true;

        } catch (error) {
            (window as any).ALERTS.error('Error al guardar la vista', 'Error');
            return false;
        }
    }

    async updateView(columns: ColumnConfig[]): Promise<boolean> {
        if (!this.currentView) {
            (window as any).ALERTS.warn('No hay vista seleccionada para actualizar', 'Advertencia');
            return false;
        }

        try {
            const viewConfig: ViewConfig = { columns };
            const response = await fetch(`${this.apiBase}/UserGridViews`, {
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
                const errorText = await response.text();
                throw new Error(`HTTP ${response.status}`);
            }

            const updated = await response.json();
            this.currentView = updated;

            (window as any).ALERTS.ok(`Vista "${this.currentView.ViewName}" actualizada`, 'Éxito');
            return true;

        } catch (error) {
            (window as any).ALERTS.error('Error al actualizar la vista', 'Error');
            return false;
        }
    }

    async setDefaultView(recId: number): Promise<boolean> {
        try {
            const url = `${this.apiBase}/UserGridViews/${recId}/set-default?dataareaId=${this.dataareaId}`;
            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${this.token}`,
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(`HTTP ${response.status}`);
            }

            await this.loadAvailableViews();
            (window as any).ALERTS.ok('Vista establecida como predeterminada', 'Éxito');
            return true;

        } catch (error) {
            (window as any).ALERTS.error('Error al establecer vista predeterminada', 'Error');
            return false;
        }
    }

    async deleteView(recId: number): Promise<boolean> {
        try {
            const url = `${this.apiBase}/UserGridViews/${recId}?dataareaId=${this.dataareaId}`;
            const response = await fetch(url, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${this.token}`,
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(`HTTP ${response.status}`);
            }

            await this.loadAvailableViews();

            if (this.currentView && this.currentView.RecID === recId) {
                this.currentView = null;
            }

            (window as any).ALERTS.ok('Vista eliminada exitosamente', 'Éxito');
            return true;

        } catch (error) {
            (window as any).ALERTS.error('Error al eliminar la vista', 'Error');
            return false;
        }
    }

    async loadView(recId: number): Promise<ColumnConfig[]> {
        const view = this.availableViews.find(v => v.RecID === recId);

        if (!view) {
            (window as any).ALERTS.warn('Vista no encontrada', 'Advertencia');
            return [];
        }

        this.currentView = view;
        return this.parseViewConfig(view.ViewConfig);
    }

    private parseViewConfig(configJson: string): ColumnConfig[] {
        try {
            const config: ViewConfig = JSON.parse(configJson);
            return config.columns || [];
        } catch (error) {
            (window as any).ALERTS.error('Error al cargar configuración de vista', 'Error');
            return [];
        }
    }

    getAvailableViews(): UserGridView[] {
        return this.availableViews;
    }

    getCurrentView(): UserGridView | null {
        return this.currentView;
    }

    getCurrentViewName(): string {
        return this.currentView ? this.currentView.ViewName : 'Vista por defecto';
    }

    hasCurrentView(): boolean {
        return this.currentView !== null;
    }
}

(window as any).GridViewsManager = GridViewsManager;