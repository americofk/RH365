// ============================================================================
// Archivo: tax-details.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Taxes/tax-details.ts
// Descripción: 
//   - Gestión completa de TaxDetails (Detalles de Impuesto)
//   - CRUD independiente: Crear, Leer, Actualizar, Eliminar
//   - Renderizado de tabla de detalles
//   - Modal para agregar/editar detalles
//   - Filtrado por TaxRefRecID
// Estándar: ISO 27001 - Gestión de datos relacionados
// ============================================================================

(function () {
    const w: any = window;
    const d: Document = document;
    const $: any = w.jQuery || w.$;

    const apiBase: string = w.RH365.urls.apiBase;

    // Variables globales del módulo
    let taxDetails: any[] = [];
    let editingDetailId: number | null = null;
    let currentTaxRecID: number = 0;
    let authToken: string = "";

    // ========================================================================
    // UTILIDADES
    // ========================================================================

    /**
     * Realiza una petición HTTP al API
     */
    const fetchJson = async (url: string, options?: RequestInit): Promise<any> => {
        const headers: Record<string, string> = {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        };

        if (authToken) {
            headers['Authorization'] = `Bearer ${authToken}`;
        }

        const response = await fetch(url, { ...options, headers });

        if (!response.ok) {
            const errorData = await response.json().catch(() => ({}));
            throw new Error(JSON.stringify(errorData));
        }

        return response.json();
    };

    /**
     * Formatea números con separadores de miles
     */
    const formatNumber = (value: number): string => {
        return value.toLocaleString('es-DO', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    };

    // ========================================================================
    // CARGA Y RENDERIZADO DE DATOS
    // ========================================================================

    /**
     * Carga los detalles de impuesto desde el API
     */
    const loadTaxDetails = async (): Promise<void> => {
        try {
            const url = `${apiBase}/TaxDetails?skip=0&take=1000`;
            const response = await fetchJson(url);
            
            // Manejar diferentes formatos de respuesta
            let allDetails: any[] = [];
            if (Array.isArray(response)) {
                allDetails = response;
            } else if (response?.Data && Array.isArray(response.Data)) {
                allDetails = response.Data;
            } else if (response?.data && Array.isArray(response.data)) {
                allDetails = response.data;
            }

            // Filtrar por TaxRefRecID
            taxDetails = allDetails.filter((detail: any) => detail.TaxRefRecID === currentTaxRecID);
            
            renderTaxDetailsTable();
        } catch (error) {
            console.error('Error cargando detalles de impuesto:', error);
            taxDetails = [];
            renderTaxDetailsTable();
        }
    };

    /**
     * Renderiza la tabla de detalles de impuesto
     */
    const renderTaxDetailsTable = (): void => {
        const container = $('#tax-details-table-body');
        container.empty();

        if (taxDetails.length === 0) {
            container.html(`
                <tr>
                    <td colspan="8" class="text-center text-muted">
                        <i class="fa fa-info-circle"></i> No hay detalles registrados
                    </td>
                </tr>
            `);
            return;
        }

        taxDetails.forEach((detail: any) => {
            const row = `
                <tr data-recid="${detail.RecID}">
                    <td class="text-center">${detail.ID || ''}</td>
                    <td class="text-right">${formatNumber(detail.AnnualAmountHigher || 0)}</td>
                    <td class="text-right">${formatNumber(detail.AnnualAmountNotExceed || 0)}</td>
                    <td class="text-right">${formatNumber(detail.Percent || 0)}%</td>
                    <td class="text-right">${formatNumber(detail.FixedAmount || 0)}</td>
                    <td class="text-right">${formatNumber(detail.ApplicableScale || 0)}</td>
                    <td>${detail.Observations || ''}</td>
                    <td class="text-center">
                        <button type="button" class="btn btn-xs btn-warning btn-edit-detail" data-recid="${detail.RecID}">
                            <i class="fa fa-pencil"></i>
                        </button>
                        <button type="button" class="btn btn-xs btn-danger btn-delete-detail" data-recid="${detail.RecID}">
                            <i class="fa fa-trash"></i>
                        </button>
                    </td>
                </tr>
            `;
            container.append(row);
        });

        // Agregar event listeners
        $('.btn-edit-detail').off('click').on('click', function() {
            const detailRecId = parseInt($(this).data('recid'), 10);
            editTaxDetail(detailRecId);
        });

        $('.btn-delete-detail').off('click').on('click', function() {
            const detailRecId = parseInt($(this).data('recid'), 10);
            deleteTaxDetail(detailRecId);
        });
    };

    // ========================================================================
    // OPERACIONES CRUD
    // ========================================================================

    /**
     * Abre el modal para crear un nuevo detalle
     */
    const openNewDetailModal = (): void => {
        editingDetailId = null;
        
        // Limpiar formulario
        $('#detail-AnnualAmountHigher').val('0');
        $('#detail-AnnualAmountNotExceed').val('0');
        $('#detail-Percent').val('0');
        $('#detail-FixedAmount').val('0');
        $('#detail-ApplicableScale').val('0');
        $('#detail-Observations').val('');

        $('#modal-detail-title').text('Nuevo Detalle');
        ($ as any)('#modal-tax-detail').modal('show');
    };

    /**
     * Edita un detalle existente
     */
    const editTaxDetail = (detailRecId: number): void => {
        const detail = taxDetails.find(d => d.RecID === detailRecId);
        if (!detail) return;

        editingDetailId = detailRecId;

        // Cargar datos en el formulario
        $('#detail-AnnualAmountHigher').val(detail.AnnualAmountHigher || 0);
        $('#detail-AnnualAmountNotExceed').val(detail.AnnualAmountNotExceed || 0);
        $('#detail-Percent').val(detail.Percent || 0);
        $('#detail-FixedAmount').val(detail.FixedAmount || 0);
        $('#detail-ApplicableScale').val(detail.ApplicableScale || 0);
        $('#detail-Observations').val(detail.Observations || '');

        $('#modal-detail-title').text('Editar Detalle');
        ($ as any)('#modal-tax-detail').modal('show');
    };

    /**
     * Guarda un detalle (crear o actualizar)
     */
    const saveTaxDetail = async (): Promise<void> => {
        const formData = {
            TaxRefRecID: currentTaxRecID,
            AnnualAmountHigher: parseFloat($('#detail-AnnualAmountHigher').val() as string) || 0,
            AnnualAmountNotExceed: parseFloat($('#detail-AnnualAmountNotExceed').val() as string) || 0,
            Percent: parseFloat($('#detail-Percent').val() as string) || 0,
            FixedAmount: parseFloat($('#detail-FixedAmount').val() as string) || 0,
            ApplicableScale: parseFloat($('#detail-ApplicableScale').val() as string) || 0,
            Observations: $('#detail-Observations').val() as string || null
        };

        try {
            if (editingDetailId) {
                // Actualizar
                const url = `${apiBase}/TaxDetails/${editingDetailId}`;
                await fetchJson(url, {
                    method: 'PUT',
                    body: JSON.stringify(formData)
                });
                (w as any).ALERTS.ok('Detalle actualizado exitosamente', 'Éxito');
            } else {
                // Crear
                const url = `${apiBase}/TaxDetails`;
                await fetchJson(url, {
                    method: 'POST',
                    body: JSON.stringify(formData)
                });
                (w as any).ALERTS.ok('Detalle creado exitosamente', 'Éxito');
            }

            ($ as any)('#modal-tax-detail').modal('hide');
            await loadTaxDetails();
        } catch (error: any) {
            console.error('Error al guardar detalle:', error);
            
            let errorMessage = 'Error al guardar el detalle';
            try {
                const errorData = JSON.parse(error.message);
                if (errorData.errors) {
                    const errorsArray: string[] = [];
                    for (const key in errorData.errors) {
                        if (errorData.errors.hasOwnProperty(key)) {
                            const errList = errorData.errors[key];
                            if (Array.isArray(errList)) {
                                errorsArray.push(...errList);
                            } else {
                                errorsArray.push(errList);
                            }
                        }
                    }
                    errorMessage = errorsArray.join(', ');
                } else if (errorData.title) {
                    errorMessage = errorData.title;
                }
            } catch {
                errorMessage = error.message || errorMessage;
            }
            
            (w as any).ALERTS.error(errorMessage, 'Error');
        }
    };

    /**
     * Elimina un detalle
     */
    const deleteTaxDetail = async (detailRecId: number): Promise<void> => {
        (w as any).ALERTS.confirm(
            '¿Está seguro de eliminar este detalle?',
            'Confirmar Eliminación',
            async (confirmed: boolean) => {
                if (!confirmed) return;

                try {
                    const url = `${apiBase}/TaxDetails/${detailRecId}`;
                    await fetchJson(url, { method: 'DELETE' });
                    
                    (w as any).ALERTS.ok('Detalle eliminado exitosamente', 'Éxito');
                    await loadTaxDetails();
                } catch (error) {
                    console.error('Error al eliminar detalle:', error);
                    (w as any).ALERTS.error('Error al eliminar el detalle', 'Error');
                }
            },
            { type: 'danger' }
        );
    };

    // ========================================================================
    // EVENT HANDLERS
    // ========================================================================

    /**
     * Manejador del botón Nuevo Detalle
     */
    $('#btn-new-detail').on('click', () => {
        openNewDetailModal();
    });

    /**
     * Manejador del botón Guardar Detalle en el modal
     */
    $('#btn-save-detail').on('click', async () => {
        await saveTaxDetail();
    });

    // ========================================================================
    // API PÚBLICA DEL MÓDULO
    // ========================================================================

    /**
     * Inicializa el módulo de TaxDetails
     * @param taxRecID RecID del impuesto padre
     * @param token Token de autenticación
     */
    const initialize = async (taxRecID: number, token: string): Promise<void> => {
        currentTaxRecID = taxRecID;
        authToken = token;

        if (taxRecID && taxRecID > 0) {
            await loadTaxDetails();
        } else {
            // Modo creación - mostrar mensaje
            $('#tax-details-table-body').html(`
                <tr>
                    <td colspan="8" class="text-center text-warning">
                        <i class="fa fa-info-circle"></i>
                        <strong>Modo Creación:</strong> 
                        Debe guardar el impuesto primero antes de agregar detalles.
                    </td>
                </tr>
            `);
            
            // Deshabilitar botón de nuevo detalle
            $('#btn-new-detail').prop('disabled', true);
        }
    };

    /**
     * Recarga los detalles desde el API
     */
    const refresh = async (): Promise<void> => {
        if (currentTaxRecID && currentTaxRecID > 0) {
            await loadTaxDetails();
        }
    };

    // Exportar funciones públicas al objeto global
    (w as any).TaxDetailsModule = {
        initialize,
        refresh
    };

})();
