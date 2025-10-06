// ============================================================================
// Archivo: CalendarHolidayConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Calendar/CalendarHolidayConfiguration.cs
// Descripción:
//   - Configuración EF Core para CalendarHoliday -> dbo.CalendarHolidays
//   - ID generado por DEFAULT en BD
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Calendar
{
    /// <summary>EF Configuration para <see cref="CalendarHoliday"/>.</summary>
    public class CalendarHolidayConfiguration : IEntityTypeConfiguration<CalendarHoliday>
    {
        public void Configure(EntityTypeBuilder<CalendarHoliday> builder)
        {
            builder.ToTable("CalendarHolidays", "dbo");
            builder.HasKey(e => e.RecID);

            // ID legible generado en BD
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // Campos obligatorios
            builder.Property(e => e.CalendarDate)
                   .IsRequired();

            builder.Property(e => e.Description)
                   .IsRequired()
                   .HasMaxLength(100);

            // Campos opcionales
            builder.Property(e => e.Observations)
                   .HasMaxLength(500);

            // Auditoría ISO 27001
            builder.Property(e => e.DataareaID)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(e => e.CreatedBy)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.ModifiedBy)
                   .HasMaxLength(50);

            builder.Property(e => e.RowVersion)
                   .IsRowVersion()
                   .IsConcurrencyToken();

            // Índice único por empresa y fecha
            builder.HasIndex(e => new { e.DataareaID, e.CalendarDate })
                   .IsUnique()
                   .HasDatabaseName("UX_CalendarHolidays_Dataarea_Date");
        }
    }
}