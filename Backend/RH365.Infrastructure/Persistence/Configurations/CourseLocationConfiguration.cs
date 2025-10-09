// ============================================================================
// Archivo: CourseLocationConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Training/CourseLocationConfiguration.cs
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public class CourseLocationConfiguration : IEntityTypeConfiguration<CourseLocation>
    {
        public void Configure(EntityTypeBuilder<CourseLocation> builder)
        {
            builder.ToTable("CourseLocations", "dbo");
            builder.HasKey(e => e.RecID);

            builder.Property(e => e.ID).HasMaxLength(50).ValueGeneratedOnAdd();
            builder.Property(e => e.CourseLocationCode).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(300);
            builder.Property(e => e.Observations).HasMaxLength(500);

            builder.Property(e => e.DataareaID).IsRequired().HasMaxLength(10);
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ModifiedBy).HasMaxLength(50);
            builder.Property(e => e.RowVersion).IsRowVersion().IsConcurrencyToken();

            // Ignorar navegación inversa
            builder.Ignore("ClassRooms");

            builder.HasIndex(e => new { e.DataareaID, e.CourseLocationCode })
                   .IsUnique()
                   .HasDatabaseName("UX_CourseLocations_Dataarea_Code");
        }
    }
}