// ============================================================================
// Archivo: CoursePosition.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Training/CoursePosition.cs
// Descripción: Relación que asocia cursos con posiciones de trabajo específicas.
//   - Many-to-many entre Course y Position
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
// ============================================================================

using RH365.Core.Domain.Common;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa la relación entre un curso y una posición de trabajo.
    /// </summary>
    public class CoursePosition : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al curso asociado.
        /// </summary>
        public long CourseRefRecID { get; set; }

        /// <summary>
        /// FK a la posición de trabajo asociada.
        /// </summary>
        public long PositionRefRecID { get; set; }

        /// <summary>
        /// Comentario adicional sobre la relación curso–posición.
        /// </summary>
        public string? Comment { get; set; }
    }
}
