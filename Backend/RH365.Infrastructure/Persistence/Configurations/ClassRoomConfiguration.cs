// ============================================================================
// Archivo: ClassRoomConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Training/ClassRoomConfiguration.cs
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public class ClassRoomConfiguration : IEntityTypeConfiguration<ClassRoom>
    {
        public void Configure(EntityTypeBuilder<ClassRoom> builder)
        {
            builder.ToTable("ClassRooms", "dbo");
            builder.HasKey(e => e.RecID);

            builder.Property(e => e.ID).HasMaxLength(50).ValueGeneratedOnAdd();
            builder.Property(e => e.ClassRoomCode).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
            builder.Property(e => e.CourseLocationRefRecID).IsRequired().HasColumnName("CourseLocationRefRecID");
            builder.Property(e => e.MaxStudentQty).IsRequired();
            builder.Property(e => e.Comment).HasMaxLength(100);
            builder.Property(e => e.AvailableTimeStart).HasColumnType("time").IsRequired();
            builder.Property(e => e.AvailableTimeEnd).HasColumnType("time").IsRequired();
            builder.Property(e => e.Observations).HasMaxLength(500);

            builder.Property(e => e.DataareaID).IsRequired().HasMaxLength(10);
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ModifiedBy).HasMaxLength(50);
            builder.Property(e => e.RowVersion).IsRowVersion().IsConcurrencyToken();

            // FK a CourseLocation
            builder.HasOne(e => e.CourseLocationRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.CourseLocationRefRecID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(e => e.CourseLocationRefRec).AutoInclude(false);

            // Ignorar navegación inversa
            builder.Ignore("Courses");

            builder.HasIndex(e => new { e.ClassRoomCode, e.DataareaID })
                   .IsUnique()
                   .HasDatabaseName("IX_ClassRoom_ClassRoomCode_DataareaID");

            builder.HasIndex(e => e.CourseLocationRefRecID)
                   .HasDatabaseName("IX_ClassRoom_CourseLocationRefRecID");
        }
    }
}