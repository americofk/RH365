// ============================================================================
// Archivo: EmployeeDocumentConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Employees/EmployeeDocumentConfiguration.cs
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations
{
    public class EmployeeDocumentConfiguration : IEntityTypeConfiguration<EmployeeDocument>
    {
        public void Configure(EntityTypeBuilder<EmployeeDocument> builder)
        {
            builder.ToTable("EmployeeDocuments", "dbo");
            builder.HasKey(e => e.RecID);

            builder.Property(e => e.ID).HasMaxLength(50).ValueGeneratedOnAdd();
            builder.Property(e => e.EmployeeRefRecID).IsRequired().HasColumnName("EmployeeRefRecID");
            builder.Property(e => e.DocumentType).IsRequired();
            builder.Property(e => e.DocumentNumber).IsRequired().HasMaxLength(30);
            builder.Property(e => e.DueDate).IsRequired();
            builder.Property(e => e.FileAttach).HasColumnType("varbinary(max)");
            builder.Property(e => e.IsPrincipal).IsRequired();
            builder.Property(e => e.Comment).HasMaxLength(200);
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

            builder.HasIndex(e => new { e.DataareaID, e.EmployeeRefRecID, e.DocumentType })
                   .HasDatabaseName("IX_EmployeeDocuments_Dataarea_Employee_Type");
        }
    }
}