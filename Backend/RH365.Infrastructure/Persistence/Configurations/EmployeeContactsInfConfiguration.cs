// ============================================================================
// Archivo: EmployeeContactsInfConfiguration.cs
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public class EmployeeContactsInfConfiguration : IEntityTypeConfiguration<EmployeeContactsInf>
    {
        public void Configure(EntityTypeBuilder<EmployeeContactsInf> builder)
        {
            builder.ToTable("EmployeeContactsInf", "dbo");
            builder.HasKey(e => e.RecID);

            builder.Property(e => e.ID).HasMaxLength(50).ValueGeneratedOnAdd();
            builder.Property(e => e.EmployeeRefRecID).IsRequired().HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.ContactType).IsRequired();
            builder.Property(e => e.ContactValue).IsRequired().HasMaxLength(200);
            builder.Property(e => e.IsPrincipal).IsRequired();
            builder.Property(e => e.Comment).HasMaxLength(200);
            builder.Property(e => e.Observations).HasMaxLength(500);

            builder.Property(e => e.DataareaID).IsRequired().HasMaxLength(10);
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
            builder.Property(e => e.ModifiedBy).HasMaxLength(50);
            builder.Property(e => e.RowVersion).IsRowVersion().IsConcurrencyToken();

            // FK - Usar el nombre exacto de la propiedad
            builder.HasOne(e => e.EmployeeRefRec)
                   .WithMany()
                   .HasForeignKey(e => e.EmployeeRefRecID)  // Usar la expresión lambda
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(e => e.EmployeeRefRec).AutoInclude(false);

            builder.HasIndex(e => new { e.DataareaID, e.EmployeeRefRecID, e.ContactType })
                   .HasDatabaseName("IX_EmployeeContactsInf_Dataarea_Employee_Type");
        }
    }
}