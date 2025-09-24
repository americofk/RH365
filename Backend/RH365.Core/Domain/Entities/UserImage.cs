// ============================================================================
// Archivo: UserImage.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Entities/Security/UserImage.cs
// Descripción: Entidad que representa las imágenes de perfil de los usuarios.
//   - Hereda de AuditableCompanyEntity para cumplir con ISO 27001
//   - Permite almacenar fotos de usuario y su extensión de archivo
// ============================================================================

using RH365.Core.Domain.Common;
using System;

namespace RH365.Core.Domain.Entities
{
    /// <summary>
    /// Representa la imagen de perfil de un usuario.
    /// </summary>
    public class UserImage : AuditableCompanyEntity
    {
        /// <summary>
        /// FK al usuario al que pertenece la imagen.
        /// </summary>
        public long UserRefRecID { get; set; }

        /// <summary>
        /// Imagen del usuario en formato binario.
        /// </summary>
        public byte[]? Image { get; set; }

        /// <summary>
        /// Extensión del archivo de imagen (ej. jpg, png).
        /// </summary>
        public string Extension { get; set; } = null!;

        // -----------------------------
        // Propiedades de navegación
        // -----------------------------

        /// <summary>
        /// Usuario propietario de la imagen.
        /// </summary>
        public virtual User UserRefRec { get; set; } = null!;
    }
}
