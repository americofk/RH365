using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    public class ReportConfigConfiguration : IEntityTypeConfiguration<ReportConfig>
    {
        public void Configure(EntityTypeBuilder<ReportConfig> builder)
        {
            builder.HasKey(x => x.InternalId);

            builder.Property(x => x.Salary).HasMaxLength(20);
            builder.Property(x => x.SFS).HasMaxLength(20);
            builder.Property(x => x.LoanCooperative).HasMaxLength(20);
            builder.Property(x => x.DeductionCooperative).HasMaxLength(20);
            builder.Property(x => x.Comission).HasMaxLength(20);
            builder.Property(x => x.AFP).HasMaxLength(20);
        }
    }
}
