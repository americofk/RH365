// ============================================================================
// Archivo: grid-columns-manager.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/GridViews/grid-columns-manager.ts
// Descripci�n: Gestor de columnas - UI y eventos (modales, drag & drop)
// ============================================================================
class GridColumnsManager {
    constructor(allColumns, visibleColumns, onColumnsChanged) {
        this.allColumns = allColumns;
        this.visibleColumns = visibleColumns;
        this.onColumnsChanged = onColumnsChanged;
    }
    /**
     * Mostrar modal para gestionar columnas
     */
    showColumnsModal() {
        const modal = $('#modal-manage-columns');
        const columnsList = $('#columns-list');
        // Limpiar lista
        columnsList.empty();
        // Crear items de columnas ordenados
        const sortedColumns = [...this.allColumns].sort((a, b) => {
            const indexA = this.visibleColumns.indexOf(a);
            const indexB = this.visibleColumns.indexOf(b);
            if (indexA === -1 && indexB === -1)
                return 0;
            if (indexA === -1)
                return 1;
            if (indexB === -1)
                return -1;
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
        modal.modal('show');
    }
    /**
     * Inicializar drag & drop con jQuery UI
     */
    initializeSortable(container) {
        if (typeof $.ui === 'undefined') {
            console.warn('jQuery UI no est� cargado. Drag & drop no disponible.');
            return;
        }
        container.sortable({
            handle: '.drag-handle',
            cursor: 'move',
            opacity: 0.6,
            update: () => {
                console.log('Orden de columnas actualizado');
            }
        });
    }
    /**
     * Aplicar cambios de columnas
     */
    applyColumns() {
        const newVisibleColumns = [];
        const columnConfigs = [];
        $('#columns-list .column-item').each((index, element) => {
            const $item = $(element);
            const field = $item.data('field');
            const isChecked = $item.find('.column-checkbox').is(':checked');
            const config = {
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
    formatColumnName(field) {
        const translations = {
            'RecID': 'ID Registro',
            'ID': 'ID Sistema',
            'ProjectCode': 'C�digo Proyecto',
            'Name': 'Nombre',
            'LedgerAccount': 'Cuenta Contable',
            'ProjectStatus': 'Estado',
            'DataareaID': 'Empresa',
            'CreatedBy': 'Creado Por',
            'CreatedOn': 'Fecha Creaci�n',
            'ModifiedBy': 'Modificado Por',
            'ModifiedOn': 'Fecha Modificaci�n',
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
    getVisibleColumns() {
        return this.visibleColumns;
    }
    /**
     * Obtener todas las columnas
     */
    getAllColumns() {
        return this.allColumns;
    }
    /**
     * Convertir columnas actuales a ColumnConfig
     */
    getCurrentColumnConfig() {
        return this.visibleColumns.map((field, index) => ({
            field: field,
            visible: true,
            order: index
        }));
    }
    /**
     * Aplicar configuraci�n de columnas desde vista guardada
     */
    applyColumnConfig(config) {
        if (!config || config.length === 0)
            return;
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
    resetToDefault(defaultColumns) {
        this.visibleColumns = [...defaultColumns];
        this.onColumnsChanged(defaultColumns);
    }
}
// Exportar para uso global
window.GridColumnsManager = GridColumnsManager;
//# sourceMappingURL=grid-columns-manager.js.map