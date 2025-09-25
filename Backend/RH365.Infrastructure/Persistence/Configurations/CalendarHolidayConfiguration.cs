// ============================================================================
// Archivo: CalendarHolidayConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/General/CalendarHolidayConfiguration.cs
// Descripción: Configuración Entity Framework para CalendarHoliday.
//   - Mapeo de propiedades y relaciones
//   - Índices y restricciones de base de datos
//   - Cumplimiento ISO 27001
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración Entity Framework para la entidad CalendarHoliday.
    /// </summary>
    public class CalendarHolidayConfiguration : IEntityTypeConfiguration<CalendarHoliday>
    {
        public void Configure(EntityTypeBuilder<CalendarHoliday> builder)
        {
            // Mapeo a tabla
            builder.ToTable("CalendarHoliday");

            // Configuración de propiedades
            builder.Property(e => e.CalendarDate).HasColumnType("date").HasColumnName("CalendarDate");
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");

            // Índices
        }
    }
}
