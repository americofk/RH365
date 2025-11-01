// ============================================================================
// Archivo: DepartmentResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Department/DepartmentResponse.cs
// Descripción: Response para un departamento individual
// Estándar: ISO 27001 - Trazabilidad completa de auditoría
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Department
{
    /// <summary>
    /// Representa la respuesta de un departamento individual desde el API.
    /// Incluye campos de auditoría según ISO 27001 para trazabilidad completa.
    /// </summary>
    public class DepartmentResponse
    {
        /// <summary>
        /// Identificador único del registro (Clave Primaria)
        /// </summary>
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        /// <summary>
        /// Identificador del sistema
        /// </summary>
        [JsonPropertyName("ID")]
        public string ID { get; set; }

        /// <summary>
        /// Código único del departamento
        /// </summary>
        [JsonPropertyName("DepartmentCode")]
        public string DepartmentCode { get; set; }

        /// <summary>
        /// Nombre del departamento
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Cantidad de trabajadores en el departamento
        /// </summary>
        [JsonPropertyName("QtyWorkers")]
        public int QtyWorkers { get; set; }

        /// <summary>
        /// Fecha de inicio del departamento
        /// </summary>
        [JsonPropertyName("StartDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Fecha de finalización del departamento
        /// </summary>
        [JsonPropertyName("EndDate")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Descripción del departamento
        /// </summary>
        [JsonPropertyName("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Estado del departamento (activo/inactivo)
        /// </summary>
        [JsonPropertyName("DepartmentStatus")]
        public bool DepartmentStatus { get; set; }

        /// <summary>
        /// Observaciones adicionales
        /// </summary>
        [JsonPropertyName("Observations")]
        public string Observations { get; set; }

        /// <summary>
        /// Identificador de la empresa (DataareaID)
        /// </summary>
        [JsonPropertyName("DataareaID")]
        public string DataareaID { get; set; }

        /// <summary>
        /// Usuario que creó el registro
        /// </summary>
        [JsonPropertyName("CreatedBy")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        [JsonPropertyName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Usuario que modificó el registro por última vez
        /// </summary>
        [JsonPropertyName("ModifiedBy")]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Fecha de última modificación
        /// </summary>
        [JsonPropertyName("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Versión del registro para control de concurrencia
        /// </summary>
        [JsonPropertyName("RowVersion")]
        public byte[] RowVersion { get; set; }
    }
}
