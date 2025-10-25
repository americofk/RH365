// ============================================================================
// Archivo: grid-columns-manager.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/GridViews/grid-columns-manager.ts
// Descripción: Gestor de columnas - UI y eventos (modales, drag & drop)
// ============================================================================

interface ColumnConfig {
    field: string;
    visible: boolean;
    order: number;
    width?: number;
}

class GridColumnsManager {
    private allColumns: string[];
    private visibleColumns: string[];
    private onColumnsChanged: (columns: string[]) => void;

    constructor(allColumns: string[], visibleColumns: string[], onColumnsChanged: (columns: string[]) => void) {
        this.allColumns = allColumns;
        this.visibleColumns = visibleColumns;
        this.onColumnsChanged = onColumnsChanged;
    }

    /**
     * Mostrar modal para gestionar columnas
     */
    showColumnsModal(): void {
        const modal = $('#modal-manage-columns');
        const columnsList = $('#columns-list');

        // Limpiar lista
        columnsList.empty();

        // Crear items de columnas ordenados
        const sortedColumns = [...this.allColumns].sort((a, b) => {
            const indexA = this.visibleColumns.indexOf(a);
            const indexB = this.visibleColumns.indexOf(b);

            if (indexA === -1 && indexB === -1) return 0;
            if (indexA === -1) return 1;
            if (indexB === -1) return -1;

            return indexA - indexB;
        });

        sortedColumns.forEach(column => {
            const isVisible = this.visibleColumns.includes(column);
            const columnName = this.formatColumnName(column);

            const item = $(`
                <div class="column-item" data-field="${column}">
                    <span class="drag-handle">
                        <i class="fa fa-bars"></i>
                    </span>
                    <input type="checkbox" class="column-checkbox" ${isVisible ? 'checked' : ''}>
                    <span class="column-name">${columnName}</span>
                </div>
            `);

            columnsList.append(item);
        });

        // Inicializar sortable (drag & drop)
        this.initializeSortable(columnsList);

        // Mostrar modal
        (modal as any).modal('show');
    }

    /**
     * Inicializar drag & drop con jQuery UI
     */
    private initializeSortable(container: JQuery): void {
        if (typeof ($ as any).ui === 'undefined') {
            (window as any).ALERTS.warn('jQuery UI no está disponible. Drag & drop deshabilitado.', 'Advertencia');
            return;
        }

        container.sortable({
            handle: '.drag-handle',
            cursor: 'move',
            opacity: 0.6
        });
    }

    /**
     * Aplicar cambios de columnas
     */
    applyColumns(): ColumnConfig[] {
        const newVisibleColumns: string[] = [];
        const columnConfigs: ColumnConfig[] = [];

        $('#columns-list .column-item').each((index, element) => {
            const $item = $(element);
            const field = $item.data('field');
            const isChecked = $item.find('.column-checkbox').is(':checked');

            const config: ColumnConfig = {
                field: field,
                visible: isChecked,
                order: index
            };

            columnConfigs.push(config);

            if (isChecked) {
                newVisibleColumns.push(field);
            }
        });

        this.visibleColumns = newVisibleColumns;

        // Notificar cambio
        this.onColumnsChanged(newVisibleColumns);

        return columnConfigs;
    }

    /**
     * Formatear nombre de columna para mostrar
     */
    private formatColumnName(field: string): string {
        const translations: Record<string, string> = {
            'RecID': 'ID Registro',
            'ID': 'ID Sistema',
            'ProjectCode': 'Código Proyecto',
            'Name': 'Nombre',
            'LedgerAccount': 'Cuenta Contable',
            'ProjectStatus': 'Estado',
            'DataareaID': 'Empresa',
            'CreatedBy': 'Creado Por',
            'CreatedOn': 'Fecha Creación',
            'ModifiedBy': 'Modificado Por',
            'ModifiedOn': 'Fecha Modificación',
            'Observations': 'Observaciones'
        };

        return translations[field] || field
            .replace(/([a-z])([A-Z])/g, '$1 $2')
            .replace(/_/g, ' ')
            .replace(/^./, (c) => c.toUpperCase());
    }

    /**
     * Obtener columnas visibles actuales
     */
    getVisibleColumns(): string[] {
        return this.visibleColumns;
    }

    /**
     * Obtener todas las columnas
     */
    getAllColumns(): string[] {
        return this.allColumns;
    }

    /**
     * Convertir columnas actuales a ColumnConfig
     */
    getCurrentColumnConfig(): ColumnConfig[] {
        return this.visibleColumns.map((field, index) => ({
            field: field,
            visible: true,
            order: index
        }));
    }

    /**
     * Aplicar configuración de columnas desde vista guardada
     */
    applyColumnConfig(config: ColumnConfig[]): void {
        if (!config || config.length === 0) return;

        // Ordenar por orden y filtrar visibles
        const sortedVisible = config
            .filter(c => c.visible)
            .sort((a, b) => a.order - b.order)
            .map(c => c.field);

        if (sortedVisible.length > 0) {
            this.visibleColumns = sortedVisible;
            this.onColumnsChanged(sortedVisible);
        }
    }

    /**
     * Restablecer a columnas por defecto
     */
    resetToDefault(defaultColumns: string[]): void {
        this.visibleColumns = [...defaultColumns];
        this.onColumnsChanged(defaultColumns);
    }
}

// Exportar para uso global
(window as any).GridColumnsManager = GridColumnsManager;