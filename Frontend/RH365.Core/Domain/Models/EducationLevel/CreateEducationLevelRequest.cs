// ============================================================================
// Archivo: CreateEducationLevelRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/EducationLevel/CreateEducationLevelRequest.cs
// Descripción:
//   Modelo de entrada para crear un Nivel Educativo.
//   Incluye solo los campos de negocio editables por el usuario.
// Notas:
//   - Validaciones básicas con DataAnnotations (longitud/obligatorio).
//   - Las columnas de auditoría (CreatedBy/On, etc.) las establece el backend.
// ============================================================================

using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Domain.Models.EducationLevel
{
    /// <summary>
    /// Solicitud para crear un Nivel Educativo.
    /// </summary>
    public sealed class CreateEducationLevelRequest
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
