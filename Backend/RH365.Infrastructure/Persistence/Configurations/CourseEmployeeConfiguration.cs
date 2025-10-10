// ============================================================================
// Archivo: CourseEmployeeConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Training/CourseEmployeeConfiguration.cs
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public class CourseEmployeeConfiguration : IEntityTypeConfiguration<CourseEmployee>
    {
        public void Configure(EntityTypeBuilder<CourseEmployee> builder)
        {
            builder.ToTable("CourseEmployees", "dbo");
            builder.HasKey(e => e.RecID);

            builder.Property(e => e.ID).HasMaxLength(50).ValueGeneratedOnAdd();
            builder.Property(e => e.CourseRefRecID).IsRequired().HasColumnName("CourseRefRecID");
            builder.Property(e => e.EmployeeRefRecID).IsRequired().HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.Comment).HasMaxLength(300);
            builder.Property(e => e.Observations).HasMaxLength(500);

            builder.Property(e => e.DataareaID).IsRequired().HasMaxLength(10);
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ModifiedBy).HasMaxLength(50);
            builder.Property(e => e.RowVersion).IsRowVersion().IsConcurrencyToken();

            // FK a Course
            builder.HasOne(e => e.CourseRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.CourseRefRecID)
                   .OnDelete(DeleteBehavior.Cascade);

            // FK a Employee
            builder.HasOne(e => e.EmployeeRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.EmployeeRefRecID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(e => e.CourseRefRec).AutoInclude(false);
            builder.Navigation(e => e.EmployeeRefRec).AutoInclude(false);

            builder.HasIndex(e => new { e.DataareaID, e.CourseRefRecID, e.EmployeeRefRecID })
                   .IsUnique()
                   .HasDatabaseName("UX_CourseEmployees_Dataarea_Course_Employee");
        }
    }
}