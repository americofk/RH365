// ============================================================================
// Archivo: UpdateDeductionCodeRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/DeductionCode/UpdateDeductionCodeRequest.cs
// Descripción: 
//   - Request para actualizar un código de deducción existente
//   - Cumplimiento ISO 27001: Validación de modificaciones
//   - Todos los campos son opcionales para permitir actualizaciones parciales
// ============================================================================

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.DeductionCode
{
    /// <summary>
    /// Modelo de request para actualización de códigos de deducción
    /// Permite actualizaciones parciales de campos
    /// </summary>
    public class UpdateDeductionCodeRequest
    {
        /// <summary>
        /// Nombre del código de deducción
        /// </summary>
        [JsonPropertyName("Name")]
        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string Name { get; set; }

        /// <summary>
        /// Referencia al proyecto asociado
        /// </summary>
        [JsonPropertyName("ProjectRefRecID")]
        public long? ProjectRefRecID { get; set; }

        /// <summary>
        /// Referencia a la categoría de proyecto
        /// </summary>
        [JsonPropertyName("ProjCategoryRefRecID")]
        public long? ProjCategoryRefRecID { get; set; }

        /// <summary>
        /// Fecha de inicio de vigencia
        /// </summary>
        [JsonPropertyName("ValidFrom")]
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// Fecha de fin de vigencia
        /// </summary>
        [JsonPropertyName("ValidTo")]
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// Descripción detallada del código de deducción
        /// </summary>
        [JsonPropertyName("Description")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Description { get; set; }

        /// <summary>
        /// Cuenta contable asociada
        /// </summary>
        [JsonPropertyName("LedgerAccount")]
        [StringLength(50, ErrorMessage = "La cuenta contable no puede exceder 50 caracteres")]
        public string LedgerAccount { get; set; }

        /// <summary>
        /// Referencia al departamento
        /// </summary>
        [JsonPropertyName("DepartmentRefRecID")]
        public long? DepartmentRefRecID { get; set; }

        /// <summary>
        /// Acción de nómina (0=Ninguna, 1=Contribución, 2=Deducción, 3=Ambas)
        /// </summary>
        [JsonPropertyName("PayrollAction")]
        [Range(0, 3, ErrorMessage = "La acción de nómina debe estar entre 0 y 3")]
        public int? PayrollAction { get; set; }

        // ====================================================================
        // CONFIGURACIÓN DE CONTRIBUCIÓN
        // ====================================================================

        /// <summary>
        /// Índice base para contribución (0=Ninguno, 1=Salario Base, 2=Salario Bruto)
        /// </summary>
        [JsonPropertyName("CtbutionIndexBase")]
        [Range(0, 2, ErrorMessage = "El índice base de contribución debe estar entre 0 y 2")]
        public int? CtbutionIndexBase { get; set; }

        /// <summary>
        /// Monto multiplicador para contribución
        /// </summary>
        [JsonPropertyName("CtbutionMultiplyAmount")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto multiplicador debe ser positivo")]
        public decimal? CtbutionMultiplyAmount { get; set; }

        /// <summary>
        /// Frecuencia de pago de contribución (1=Semanal, 2=Quincenal, 3=Mensual)
        /// </summary>
        [JsonPropertyName("CtbutionPayFrecuency")]
        [Range(0, 3, ErrorMessage = "La frecuencia de pago debe estar entre 0 y 3")]
        public int? CtbutionPayFrecuency { get; set; }

        /// <summary>
        /// Periodo límite de contribución (en meses)
        /// </summary>
        [JsonPropertyName("CtbutionLimitPeriod")]
        [Range(0, 120, ErrorMessage = "El periodo límite debe estar entre 0 y 120 meses")]
        public int? CtbutionLimitPeriod { get; set; }

        /// <summary>
        /// Monto límite de contribución
        /// </summary>
        [JsonPropertyName("CtbutionLimitAmount")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto límite debe ser positivo")]
        public decimal? CtbutionLimitAmount { get; set; }

        /// <summary>
        /// Monto límite a aplicar de contribución
        /// </summary>
        [JsonPropertyName("CtbutionLimitAmountToApply")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto a aplicar debe ser positivo")]
        public decimal? CtbutionLimitAmountToApply { get; set; }

        // ====================================================================
        // CONFIGURACIÓN DE DEDUCCIÓN
        // ====================================================================

        /// <summary>
        /// Índice base para deducción (0=Ninguno, 1=Salario Base, 2=Salario Bruto)
        /// </summary>
        [JsonPropertyName("DductionIndexBase")]
        [Range(0, 2, ErrorMessage = "El índice base de deducción debe estar entre 0 y 2")]
        public int? DductionIndexBase { get; set; }

        /// <summary>
        /// Monto multiplicador para deducción
        /// </summary>
        [JsonPropertyName("DductionMultiplyAmount")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto multiplicador debe ser positivo")]
        public decimal? DductionMultiplyAmount { get; set; }

        /// <summary>
        /// Frecuencia de pago de deducción (1=Semanal, 2=Quincenal, 3=Mensual)
        /// </summary>
        [JsonPropertyName("DductionPayFrecuency")]
        [Range(0, 3, ErrorMessage = "La frecuencia de pago debe estar entre 0 y 3")]
        public int? DductionPayFrecuency { get; set; }

        /// <summary>
        /// Periodo límite de deducción (en meses)
        /// </summary>
        [JsonPropertyName("DductionLimitPeriod")]
        [Range(0, 120, ErrorMessage = "El periodo límite debe estar entre 0 y 120 meses")]
        public int? DductionLimitPeriod { get; set; }

        /// <summary>
        /// Monto límite de deducción
        /// </summary>
        [JsonPropertyName("DductionLimitAmount")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto límite debe ser positivo")]
        public decimal? DductionLimitAmount { get; set; }

        /// <summary>
        /// Monto límite a aplicar de deducción
        /// </summary>
        [JsonPropertyName("DductionLimitAmountToApply")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto a aplicar debe ser positivo")]
        public decimal? DductionLimitAmountToApply { get; set; }

        // ====================================================================
        // CONFIGURACIÓN DE CÁLCULOS
        // ====================================================================

        /// <summary>
        /// Indica si se usa para cálculo de impuestos (ISR)
        /// </summary>
        [JsonPropertyName("IsForTaxCalc")]
        public bool? IsForTaxCalc { get; set; }

        /// <summary>
        /// Indica si se usa para cálculo de TSS
        /// </summary>
        [JsonPropertyName("IsForTssCalc")]
        public bool? IsForTssCalc { get; set; }

        /// <summary>
        /// Estado del código de deducción (Activo/Inactivo)
        /// </summary>
        [JsonPropertyName("DeductionStatus")]
        public bool? DeductionStatus { get; set; }

        /// <summary>
        /// Observaciones adicionales
        /// </summary>
        [JsonPropertyName("Observations")]
        [StringLength(1000, ErrorMessage = "Las observaciones no pueden exceder 1000 caracteres")]
        public string Observations { get; set; }
    }
}
