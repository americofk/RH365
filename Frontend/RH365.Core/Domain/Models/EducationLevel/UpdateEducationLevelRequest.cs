// ============================================================================
// Archivo: UpdateEducationLevelRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EducationLevel/UpdateEducationLevelRequest.cs
// Descripción:
//   Modelo de entrada para actualizar un Nivel Educativo (PUT /{recId}).
//   Incluye campos de negocio editables; el RecID viaja en la ruta.
// Notas:
//   - Validaciones con DataAnnotations.
//   - Auditoría (ModifiedBy/On) la gestiona el backend automáticamente.
// ============================================================================

using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Domain.Models.EducationLevel
{
    /// <summary>
    /// Solicitud para actualizar un Nivel Educativo existente.
    /// </summary>
    public sealed class UpdateEducationLevelRequest
    {
        /// <summary>
        /// Código único del nivel educativo (ej.: "EDU-SEC").
        /// </summary>
        [Required(ErrorMessage = "El código de nivel educativo es obligatorio.")]
        [MaxLength(40, ErrorMessage = "El código no debe exceder 40 caracteres.")]
        public string EducationLevelCode { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del nivel educativo (ej.: "Secundaria").
        /// </summary>
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [MaxLength(200, ErrorMessage = "La descripción no debe exceder 200 caracteres.")]
        public string Description { get; set; } = string.Empty;
    }
}
