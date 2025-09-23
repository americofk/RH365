using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class FormatCodeConfiguration : IEntityTypeConfiguration<FormatCode>
    {
        public void Configure(EntityTypeBuilder<FormatCode> builder)
        {
            builder.HasKey(x => x.FormatCodeId);
            builder.Property(x => x.FormatCodeId).ValueGeneratedNever();

            builder.Property(x => x.FormatCodeId).IsRequired().HasMaxLength(5);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(15);

        }
    }
}
