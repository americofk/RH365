// ============================================================================
// Archivo: EmployeeWorkCalendarConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employees/EmployeeWorkCalendarConfiguration.cs
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public class EmployeeWorkCalendarConfiguration : IEntityTypeConfiguration<EmployeeWorkCalendar>
    {
        public void Configure(EntityTypeBuilder<EmployeeWorkCalendar> builder)
        {
            builder.ToTable("EmployeeWorkCalendars", "dbo");
            builder.HasKey(e => e.RecID);

            builder.Property(e => e.ID).HasMaxLength(50).ValueGeneratedOnAdd();
            builder.Property(e => e.EmployeeRefRecID).IsRequired().HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.CalendarDate).IsRequired();
            builder.Property(e => e.CalendarDay).IsRequired().HasMaxLength(30);
            builder.Property(e => e.WorkFrom).HasColumnType("time").IsRequired();
            builder.Property(e => e.WorkTo).HasColumnType("time").IsRequired();
            builder.Property(e => e.BreakWorkFrom).HasColumnType("time").IsRequired();
            builder.Property(e => e.BreakWorkTo).HasColumnType("time").IsRequired();
            builder.Property(e => e.TotalHour).HasPrecision(32, 16).IsRequired();
            builder.Property(e => e.StatusWorkControl).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.PayrollProcessRefRecID);
            builder.Property(e => e.Observations).HasMaxLength(500);

            builder.Property(e => e.DataareaID).IsRequired().HasMaxLength(10);
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ModifiedBy).HasMaxLength(50);
            builder.Property(e => e.RowVersion).IsRowVersion().IsConcurrencyToken();

            // FK a Employee
            builder.HasOne(e => e.EmployeeRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.EmployeeRefRecID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(e => e.EmployeeRefRec).AutoInclude(false);

            builder.HasIndex(e => new { e.DataareaID, e.EmployeeRefRecID, e.CalendarDate })
                   .HasDatabaseName("IX_EmployeeWorkCalendars_Dataarea_Employee_Date");
        }
    }
}