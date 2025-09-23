using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class EarningCodeConfiguration : IEntityTypeConfiguration<EarningCode>
    {
        public void Configure(EntityTypeBuilder<EarningCode> builder)
        {
            //builder.HasNoKey();
            builder.HasKey(x => x.EarningCodeId);
            builder.Property(x => x.EarningCodeId).HasDefaultValueSql("FORMAT((NEXT VALUE FOR dbo.EarningCodeId),'EC-00000000#')")
                    .HasMaxLength(20);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ProjId).HasMaxLength(20);
            builder.Property(x => x.Department).HasMaxLength(20);
            builder.Property(x => x.LedgerAccount).HasMaxLength(30);
            builder.Property(x => x.ValidFrom).IsRequired();
            builder.Property(x => x.ValidTo).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(200);

            builder.Property(x => x.IsHoliday);
            builder.Property(x => x.WorkFrom);
            builder.Property(x => x.WorkTo);

            //builder.Property(x => x.IndexBase).IsRequired();
            //builder.Property(x => x.MultiplyAmount).IsRequired();
            //builder.Property(x => x.LedgerAccount).IsRequired();
        }
    }
}
