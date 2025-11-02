// ============================================================================
// Archivo: PositionConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Organization/PositionConfiguration.cs
// Descripción: Configuración Entity Framework para la entidad Position.
//   - Configuración explícita para prevenir shadow properties
// ============================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RH365.Core.Domain.Entities;

namespace RH365.Infrastructure.Persistence.Configurations.Organization
{
    public sealed class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("Positions", "dbo");

            // Configuración del ID generado por BD
            builder.Property(e => e.ID)
                .HasMaxLength(50)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd()
                .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

            // Configuración de propiedades
            builder.Property(e => e.PositionCode)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("PositionCode");

            builder.Property(e => e.PositionName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("PositionName");

            builder.Property(e => e.IsVacant)
                .IsRequired()
                .HasColumnName("IsVacant");

            builder.Property(e => e.DepartmentRefRecID)
                .HasColumnName("DepartmentRefRecID");

            builder.Property(e => e.JobRefRecID)
                .HasColumnName("JobRefRecID");

            builder.Property(e => e.NotifyPositionRefRecID)
                .HasColumnName("NotifyPositionRefRecID");

            builder.Property(e => e.PositionStatus)
                .IsRequired()
                .HasColumnName("PositionStatus");

            builder.Property(e => e.StartDate)
                .IsRequired()
                .HasColumnName("StartDate");

            builder.Property(e => e.EndDate)
                .HasColumnName("EndDate");

            builder.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("Description");

            builder.Property(e => e.Observations)
                .HasMaxLength(500);

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

            // CRÍTICO: Ignorar navegaciones para evitar shadow properties
            builder.Ignore(e => e.Department);
            builder.Ignore(e => e.Job);
            builder.Ignore(e => e.NotifyPosition);
            builder.Ignore(e => e.EmployeePositions);
            builder.Ignore(e => e.InverseNotifyPosition);
            builder.Ignore(e => e.PositionRequirements);

            // Índice único
            builder.HasIndex(e => new { e.PositionCode, e.DataareaID })
                .IsUnique()
                .HasDatabaseName("UX_Positions_PositionCode_DataareaID");

            // Índices para búsquedas
            builder.HasIndex(e => e.DepartmentRefRecID)
                .HasDatabaseName("IX_Positions_DepartmentRefRecID");

            builder.HasIndex(e => e.JobRefRecID)
                .HasDatabaseName("IX_Positions_JobRefRecID");
        }
    }
}