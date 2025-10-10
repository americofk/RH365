// ============================================================================
// Archivo: CourseConfiguration.cs
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses", "dbo");
            builder.HasKey(e => e.RecID);

            builder.Property(e => e.ID).HasMaxLength(50).ValueGeneratedOnAdd();
            builder.Property(e => e.CourseCode).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.CourseTypeRefRecID).IsRequired().HasColumnName("CourseTypeRefRecID");

            // Configurar ClassRoomRefRecID sin navegación
            builder.Property(e => e.ClassRoomRefRecID).HasColumnName("ClassRoomRefRecID");

            builder.Property(e => e.Description).HasMaxLength(200);
            builder.Property(e => e.StartDate).IsRequired();
            builder.Property(e => e.EndDate).IsRequired();
            builder.Property(e => e.IsMatrixTraining).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.InternalExternal).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.CourseParentId).HasMaxLength(20);
            builder.Property(e => e.MinStudents).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.MaxStudents).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.Periodicity).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.QtySessions).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.Objetives).IsRequired().HasMaxLength(1000);
            builder.Property(e => e.Topics).IsRequired().HasMaxLength(1000);
            builder.Property(e => e.CourseStatus).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.UrlDocuments).HasMaxLength(1000).HasColumnName("URLDocuments");
            builder.Property(e => e.Observations).HasMaxLength(500);

            builder.Property(e => e.DataareaID).IsRequired().HasMaxLength(10);
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ModifiedBy).HasMaxLength(50);
            builder.Property(e => e.RowVersion).IsRowVersion().IsConcurrencyToken();

            // FK a CourseType
            builder.HasOne(e => e.CourseTypeRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.CourseTypeRefRecID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(e => e.CourseTypeRefRec).AutoInclude(false);

            // Ignorar navegaciones inversas y ClassRoom
            builder.Ignore("CourseEmployees");
            builder.Ignore("CourseInstructors");
            builder.Ignore("ClassRoomRefRec");
            builder.Ignore("ClassRoom");

            builder.HasIndex(e => new { e.DataareaID, e.CourseCode })
                   .IsUnique()
                   .HasDatabaseName("UX_Courses_Dataarea_Code");
        }
    }
}