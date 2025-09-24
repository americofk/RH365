// ============================================================================
// Archivo: EarningCodeConfiguration.cs
// Proyecto: RH365.Infrastructure
// Ruta: RH365.Infrastructure/Persistence/Configurations/Payroll/EarningCodeConfiguration.cs
// Descripción: Configuración Entity Framework para EarningCode.
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
    /// Configuración Entity Framework para la entidad EarningCode.
    /// </summary>
    public class EarningCodeConfiguration : IEntityTypeConfiguration<EarningCode>
    {
        public void Configure(EntityTypeBuilder<EarningCode> builder)
        {
            // Mapeo a tabla
            builder.ToTable("EarningCode");

            // Configuración de propiedades
            builder.Property(e => e.DepartmentRefRec).HasColumnName("DepartmentRefRec");
            builder.Property(e => e.DepartmentRefRecID).HasColumnName("DepartmentRefRecID");
            builder.Property(e => e.Description).HasMaxLength(500).HasColumnName("Description");
            builder.Property(e => e.EarningCode1).HasMaxLength(50).HasColumnName("EarningCode1");
            builder.Property(e => e.EarningCodeStatus).HasColumnName("EarningCodeStatus");
            builder.Property(e => e.IndexBase).HasColumnName("IndexBase");
            builder.Property(e => e.IsExtraHours).HasColumnName("IsExtraHours");
            builder.Property(e => e.IsHoliday).HasColumnName("IsHoliday");
            builder.Property(e => e.IsIsr).HasColumnName("IsIsr");
            builder.Property(e => e.IsRoyaltyPayroll).HasColumnName("IsRoyaltyPayroll");
            builder.Property(e => e.IsTss).HasColumnName("IsTss");
            builder.Property(e => e.IsUseDgt).HasColumnName("IsUseDgt");
            builder.Property(e => e.LedgerAccount).HasMaxLength(255).HasColumnName("LedgerAccount");
            builder.Property(e => e.MultiplyAmount).HasPrecision(18, 4).HasColumnName("MultiplyAmount");
            builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("Name");
            builder.Property(e => e.ProjId).HasMaxLength(255).HasColumnName("ProjId");
            builder.Property(e => e.ValidFrom).HasColumnType("time").HasColumnName("ValidFrom");
            builder.Property(e => e.ValidTo).HasColumnType("time").HasColumnName("ValidTo");
            builder.Property(e => e.WorkFrom).HasColumnType("time").HasColumnName("WorkFrom");
            builder.Property(e => e.WorkTo).HasColumnType("time").HasColumnName("WorkTo");

            //// Configuración de relaciones
            //builder.HasOne(e => e.DepartmentRefRec)
            //    .WithMany()
            //    .HasForeignKey(e => e.DepartmentRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasMany(e => e.EmployeeEarningCodes)
            //    .WithOne(d => d.EarningCodeRefRec)
            //    .HasForeignKey(d => d.EarningCodeRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
            //builder.HasMany(e => e.EmployeeExtraHours)
            //    .WithOne(d => d.EarningCodeRefRec)
            //    .HasForeignKey(d => d.EarningCodeRefRecID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // Índices
        }
    }
}
