// ============================================================================
// Archivo: DeductionCodeResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/DeductionCode/DeductionCodeResponse.cs
// Descripción: 
//   - Response para un código de deducción individual
//   - Cumplimiento ISO 27001: Trazabilidad completa de auditoría
//   - Incluye nombres de entidades relacionadas para mejor UX
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.DeductionCode
{
    /// <summary>
    /// Modelo de respuesta para códigos de deducción
    /// Incluye campos de auditoría y referencias a entidades relacionadas
    /// </summary>
    public class DeductionCodeResponse
    {
        // ====================================================================
        // CAMPOS DE IDENTIFICACIÓN
        // ====================================================================

        /// <summary>
        /// Identificador único del registro (Primary Key)
        /// </summary>
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        /// <summary>
        /// Identificador del sistema generado automáticamente
        /// </summary>
        [JsonPropertyName("ID")]
        public string ID { get; set; }

        /// <summary>
        /// Nombre del código de deducción
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        // ====================================================================
        // RELACIONES CON OTRAS ENTIDADES
        // ====================================================================

        /// <summary>
        /// RecID del proyecto asociado
        /// </summary>
        [JsonPropertyName("ProjectRefRecID")]
        public long? ProjectRefRecID { get; set; }

        /// <summary>
        /// Nombre del proyecto asociado (para visualización)
        /// </summary>
        [JsonPropertyName("ProjectName")]
        public string ProjectName { get; set; }

        /// <summary>
        /// RecID de la categoría de proyecto
        /// </summary>
        [JsonPropertyName("ProjCategoryRefRecID")]
        public long? ProjCategoryRefRecID { get; set; }

        /// <summary>
        /// Nombre de la categoría de proyecto (para visualización)
        /// </summary>
        [JsonPropertyName("ProjCategoryName")]
        public string ProjCategoryName { get; set; }

        /// <summary>
        /// RecID del departamento
        /// </summary>
        [JsonPropertyName("DepartmentRefRecID")]
        public long? DepartmentRefRecID { get; set; }

        /// <summary>
        /// Nombre del departamento (para visualización)
        /// </summary>
        [JsonPropertyName("DepartmentName")]
        public string DepartmentName { get; set; }

        // ====================================================================
        // FECHAS DE VIGENCIA
        // ====================================================================

        /// <summary>
        /// Fecha de inicio de vigencia
        /// </summary>
        [JsonPropertyName("ValidFrom")]
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Fecha de fin de vigencia
        /// </summary>
        [JsonPropertyName("ValidTo")]
        public DateTime ValidTo { get; set; }

        // ====================================================================
        // INFORMACIÓN GENERAL
        // ====================================================================

        /// <summary>
        /// Descripción detallada del código de deducción
        /// </summary>
        [JsonPropertyName("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Cuenta contable asociada
        /// </summary>
        [JsonPropertyName("LedgerAccount")]
        public string LedgerAccount { get; set; }

        /// <summary>
        /// Acción de nómina (0=Ninguna, 1=Contribución, 2=Deducción, 3=Ambas)
        /// </summary>
        [JsonPropertyName("PayrollAction")]
        public int PayrollAction { get; set; }

        // ====================================================================
        // CONFIGURACIÓN DE CONTRIBUCIÓN
        // ====================================================================

        /// <summary>
        /// Índice base para contribución (0=Ninguno, 1=Salario Base, 2=Salario Bruto)
        /// </summary>
        [JsonPropertyName("CtbutionIndexBase")]
        public int? CtbutionIndexBase { get; set; }

        /// <summary>
        /// Monto multiplicador para contribución
        /// </summary>
        [JsonPropertyName("CtbutionMultiplyAmount")]
        public decimal? CtbutionMultiplyAmount { get; set; }

        /// <summary>
        /// Frecuencia de pago de contribución (1=Semanal, 2=Quincenal, 3=Mensual)
        /// </summary>
        [JsonPropertyName("CtbutionPayFrecuency")]
        public int? CtbutionPayFrecuency { get; set; }

        /// <summary>
        /// Periodo límite de contribución (en meses)
        /// </summary>
        [JsonPropertyName("CtbutionLimitPeriod")]
        public int? CtbutionLimitPeriod { get; set; }

        /// <summary>
        /// Monto límite de contribución
        /// </summary>
        [JsonPropertyName("CtbutionLimitAmount")]
        public decimal? CtbutionLimitAmount { get; set; }

        /// <summary>
        /// Monto límite a aplicar de contribución
        /// </summary>
        [JsonPropertyName("CtbutionLimitAmountToApply")]
        public decimal? CtbutionLimitAmountToApply { get; set; }

        // ====================================================================
        // CONFIGURACIÓN DE DEDUCCIÓN
        // ====================================================================

        /// <summary>
        /// Índice base para deducción (0=Ninguno, 1=Salario Base, 2=Salario Bruto)
        /// </summary>
        [JsonPropertyName("DductionIndexBase")]
        public int? DductionIndexBase { get; set; }

        /// <summary>
        /// Monto multiplicador para deducción
        /// </summary>
        [JsonPropertyName("DductionMultiplyAmount")]
        public decimal? DductionMultiplyAmount { get; set; }

        /// <summary>
        /// Frecuencia de pago de deducción (1=Semanal, 2=Quincenal, 3=Mensual)
        /// </summary>
        [JsonPropertyName("DductionPayFrecuency")]
        public int? DductionPayFrecuency { get; set; }

        /// <summary>
        /// Periodo límite de deducción (en meses)
        /// </summary>
        [JsonPropertyName("DductionLimitPeriod")]
        public int? DductionLimitPeriod { get; set; }

        /// <summary>
        /// Monto límite de deducción
        /// </summary>
        [JsonPropertyName("DductionLimitAmount")]
        public decimal? DductionLimitAmount { get; set; }

        /// <summary>
        /// Monto límite a aplicar de deducción
        /// </summary>
        [JsonPropertyName("DductionLimitAmountToApply")]
        public decimal? DductionLimitAmountToApply { get; set; }

        // ====================================================================
        // CONFIGURACIÓN DE CÁLCULOS
        // ====================================================================

        /// <summary>
        /// Indica si se usa para cálculo de impuestos (ISR)
        /// </summary>
        [JsonPropertyName("IsForTaxCalc")]
        public bool IsForTaxCalc { get; set; }

        /// <summary>
        /// Indica si se usa para cálculo de TSS
        /// </summary>
        [JsonPropertyName("IsForTssCalc")]
        public bool IsForTssCalc { get; set; }

        /// <summary>
        /// Estado del código de deducción (Activo/Inactivo)
        /// </summary>
        [JsonPropertyName("DeductionStatus")]
        public bool DeductionStatus { get; set; }

        /// <summary>
        /// Observaciones adicionales
        /// </summary>
        [JsonPropertyName("Observations")]
        public string Observations { get; set; }

        // ====================================================================
        // CAMPOS DE AUDITORÍA (ISO 27001)
        // ====================================================================

        /// <summary>
        /// Identificador de la empresa/área de datos
        /// </summary>
        [JsonPropertyName("DataareaID")]
        public string DataareaID { get; set; }

        /// <summary>
        /// Usuario que creó el registro
        /// </summary>
        [JsonPropertyName("CreatedBy")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Fecha y hora de creación del registro
        /// </summary>
        [JsonPropertyName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Usuario que modificó por última vez el registro
        /// </summary>
        [JsonPropertyName("ModifiedBy")]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Fecha y hora de última modificación
        /// </summary>
        [JsonPropertyName("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Versión del registro para control de concurrencia optimista
        /// </summary>
        [JsonPropertyName("RowVersion")]
        public byte[] RowVersion { get; set; }
    }
}
