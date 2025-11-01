// ============================================================================
// Archivo: UpdateDepartmentRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/Department/UpdateDepartmentRequest.cs
// Descripción: Request para actualizar un departamento
// Estándar: ISO 27001 - Validación de datos de actualización
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.Department
{
    /// <summary>
    /// Representa la solicitud para actualizar un departamento existente.
    /// Contiene únicamente los campos editables por el usuario.
    /// </summary>
    public class UpdateDepartmentRequest
    {
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
        public int? QtyWorkers { get; set; }

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
        public bool? DepartmentStatus { get; set; }

        /// <summary>
        /// Observaciones adicionales
        /// </summary>
        [JsonPropertyName("Observations")]
        public string Observations { get; set; }
    }
}
