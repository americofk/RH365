using D365_API_Nomina.Core.Domain.Consts;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class BatchHistoryConfiguration : IEntityTypeConfiguration<BatchHistory>
    {
        public void Configure(EntityTypeBuilder<BatchHistory> builder)
        {
            builder.HasKey(x => x.InternalId);
            builder.Property(x => x.Information).HasColumnType(ColumnTypeConst.maxvarchartype);
        }
    }
}
