// ============================================================================
// Archivo: EmployeeWorkCalendarConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employee/EmployeeWorkCalendarConfiguration.cs
// Descripción: Configuración Entity Framework para EmployeeWorkCalendar.
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
    /// Configuración Entity Framework para la entidad EmployeeWorkCalendar.
    /// </summary>
    public class EmployeeWorkCalendarConfiguration : IEntityTypeConfiguration<EmployeeWorkCalendar>
    {
        public void Configure(EntityTypeBuilder<EmployeeWorkCalendar> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EmployeeWorkCalendar");

            // Configuración de propiedades
            builder.Property(e => e.BreakWorkFrom).HasColumnType("time").HasColumnName("BreakWorkFrom");
            builder.Property(e => e.BreakWorkTo).HasColumnType("time").HasColumnName("BreakWorkTo");
            builder.Property(e => e.CalendarDate).HasColumnType("datetime2").HasColumnName("CalendarDate");
            builder.Property(e => e.CalendarDay).HasMaxLength(255).HasColumnName("CalendarDay");
            builder.Property(e => e.EmployeeRefRec).HasColumnName("EmployeeRefRec");
            builder.Property(e => e.EmployeeRefRecID).HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.PayrollProcessRefRecID).HasColumnName("PayrollProcessRefRecID");
            builder.Property(e => e.StatusWorkControl).HasColumnName("StatusWorkControl");
            builder.Property(e => e.TotalHour).HasPrecision(18, 2).HasColumnName("TotalHour");
            builder.Property(e => e.WorkFrom).HasColumnType("time").HasColumnName("WorkFrom");
            builder.Property(e => e.WorkTo).HasColumnType("time").HasColumnName("WorkTo");

            // Configuración de relaciones
            builder.HasOne(e => e.EmployeeRefRec)
                .WithMany()
                .HasForeignKey(e => e.EmployeeRefRecID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
            builder.HasIndex(e => e.EmployeeRefRecID)
                .HasDatabaseName("IX_EmployeeWorkCalendar_EmployeeRefRecID");
        }
    }
}
