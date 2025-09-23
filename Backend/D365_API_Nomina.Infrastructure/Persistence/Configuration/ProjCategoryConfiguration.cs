using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    public class ProjCategoryConfiguration : IEntityTypeConfiguration<ProjCategory>
    {
        public void Configure(EntityTypeBuilder<ProjCategory> builder)
        {
            builder.HasKey(x => x.ProjCategoryId);
            builder.Property(x => x.ProjCategoryId)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValueSql("FORMAT((NEXT VALUE FOR dbo.ProjCategoryId),'PRJC-00000000#')");

            builder.Property(x => x.CategoryName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.LedgerAccount).HasMaxLength(20);
        }
    }
}
