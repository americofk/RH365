// ============================================================================
// Archivo: BaseEntity.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Common/BaseEntity.cs
// Descripción: Clase base para todas las entidades del dominio.
//   - Define la clave primaria RecID
//   - Base para herencia de entidades auditables
// ============================================================================

using System.ComponentModel.DataAnnotations;

namespace RH365.Core.Domain.Common
{
    /// <summary>
    /// Clase base para todas las entidades del sistema.
    /// Provee la clave primaria requerida por la arquitectura.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Clave primaria global del sistema.
        /// Utiliza la secuencia dbo.RecId iniciando en 2020450.
        /// ISO 27001: Identificación única e inmutable de registros.
        /// </summary>
        [Key]
        public long RecID { get; set; }
    }
}