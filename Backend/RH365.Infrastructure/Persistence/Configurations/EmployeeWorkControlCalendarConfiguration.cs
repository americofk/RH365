// ============================================================================
// Archivo: EmployeeWorkControlCalendarConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employees/EmployeeWorkControlCalendarConfiguration.cs
// Descripción:
//   - Configuración EF Core para EmployeeWorkControlCalendar -> dbo.EmployeeWorkControlCalendars
//   - Mapeo completo de FKs con .HasColumnName() explícito
//   - Cumple auditoría ISO 27001
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Employees
{
    /// <summary>EF Configuration para <see cref="EmployeeWorkControlCalendar"/>.</summary>
    public class EmployeeWorkControlCalendarConfiguration : IEntityTypeConfiguration<EmployeeWorkControlCalendar>
    {
        public void Configure(EntityTypeBuilder<EmployeeWorkControlCalendar> builder)
        {
            // Tabla
            builder.ToTable("EmployeeWorkControlCalendars", "dbo");

            // PK
            builder.HasKey(e => e.RecID);

            // ID legible
            builder.Property(e => e.ID)
                   .HasMaxLength(50)
                   .ValueGeneratedOnAdd();

            // FK con .HasColumnName() explícito
            builder.Property(e => e.EmployeeRefRecID)
                   .IsRequired()
                   .HasColumnName("EmployeeRefRecID");

            builder.Property(e => e.PayrollProcessRefRecID)
                   .HasColumnName("PayrollProcessRefRecID");

            // Propiedades obligatorias
            builder.Property(e => e.CalendarDate)
                   .IsRequired();

            builder.Property(e => e.CalendarDay)
                   .IsRequired()
                   .HasMaxLength(30);

            builder.Property(e => e.WorkFrom)
                   .IsRequired();

            builder.Property(e => e.WorkTo)
                   .IsRequired();

            builder.Property(e => e.BreakWorkFrom)
                   .IsRequired();

            builder.Property(e => e.BreakWorkTo)
                   .IsRequired();

            builder.Property(e => e.TotalHour)
                   .IsRequired()
                   .HasColumnType("decimal(32,16)");

            builder.Property(e => e.StatusWorkControl)
                   .IsRequired();

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

            // Relación FK
            builder.HasOne(e => e.EmployeeRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.EmployeeRefRecID)
                   .HasConstraintName("FK_EmployeeWorkControlCalendars_Employees")
                   .OnDelete(DeleteBehavior.Restrict);

            // Navegación con AutoInclude(false)
            builder.Navigation(e => e.EmployeeRefRec).AutoInclude(false);

            // Índices
            builder.HasIndex(e => e.EmployeeRefRecID)
                   .HasDatabaseName("IX_EmployeeWorkControlCalendars_EmployeeRefRecID");

            builder.HasIndex(e => e.CalendarDate)
                   .HasDatabaseName("IX_EmployeeWorkControlCalendars_CalendarDate");

            builder.HasIndex(e => new { e.EmployeeRefRecID, e.CalendarDate })
                   .HasDatabaseName("IX_EmployeeWorkControlCalendars_Employee_Date");
        }
    }
}