// ============================================================================
// Archivo: EducationLevelResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EducationLevel/EducationLevelResponse.cs
// Descripción:
//   DTO de salida (respuesta) para el recurso "Nivel Educativo".
//   Mantiene la convención de nombres usada en el proyecto (JSON camel/pascal)
//   y contempla los campos estándar de auditoría.
// Notas:
//   - Este modelo vive en el dominio de modelos (Domain.Models.EducationLevel)
//     según la convención de tu solución.
//   - CreatedOn/ModifiedOn se manejan como DateTime (se serializan ISO 8601).
// ============================================================================

using System;

namespace RH365.Core.Domain.Models.EducationLevel
{
    /// <summary>
    /// Respuesta (detalle) de un Nivel Educativo.
    /// </summary>
    public sealed class EducationLevelResponse
    {
        /// <summary>
        /// Identificador interno por secuencia global (PK lógica).
        /// </summary>
        public long RecID { get; set; }

        /// <summary>
        /// Código único del nivel educativo (ej. "EDU-SEC").
        /// </summary>
        public string EducationLevelCode { get; set; } = string.Empty;

        /// <summary>
        /// Descripción legible del nivel educativo (ej. "Secundaria").
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Usuario que creó el registro.
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Fecha de creación (UTC o TZ del sistema, consistente con backend).
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Último usuario que modificó el registro.
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Fecha de última modificación (UTC o TZ del sistema).
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
    }
}
