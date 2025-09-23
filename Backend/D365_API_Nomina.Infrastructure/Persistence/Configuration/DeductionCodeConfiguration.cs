using D365_API_Nomina.Core.Domain.Consts;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class DeductionCodeConfiguration : IEntityTypeConfiguration<DeductionCode>
    {
        public void Configure(EntityTypeBuilder<DeductionCode> builder)
        {
            builder.HasKey(x => x.DeductionCodeId);
            builder.Property(x => x.DeductionCodeId).HasDefaultValueSql("FORMAT((NEXT VALUE FOR dbo.DeductionCodeId),'D-00000000#')")
                    .HasMaxLength(20);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ValidFrom).IsRequired();
            builder.Property(x => x.ValidTo).IsRequired();

            builder.Property(x => x.ProjId).HasMaxLength(20);
            builder.Property(x => x.Department).HasMaxLength(20);
            builder.Property(x => x.LedgerAccount).HasMaxLength(30);

            builder.Property(x => x.Ctbution_IndexBase).IsRequired();
            builder.Property(x => x.Ctbution_MultiplyAmount).HasColumnType(ColumnTypeConst.decimaltype).IsRequired();
            builder.Property(x => x.Ctbution_LimitAmount).HasColumnType(ColumnTypeConst.decimaltype).IsRequired();
            builder.Property(x => x.Ctbution_PayFrecuency).IsRequired();
            builder.Property(x => x.Ctbution_LimitPeriod).IsRequired();
            
            builder.Property(x => x.Dduction_IndexBase).IsRequired();
            builder.Property(x => x.Dduction_MultiplyAmount).HasColumnType(ColumnTypeConst.decimaltype).IsRequired();
            builder.Property(x => x.Dduction_LimitAmount).HasColumnType(ColumnTypeConst.decimaltype).IsRequired();
            builder.Property(x => x.Dduction_PayFrecuency).IsRequired();
            builder.Property(x => x.Dduction_LimitPeriod).IsRequired();
        }
    }
}
