using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class GeneralConfigConfiguration : IEntityTypeConfiguration<GeneralConfig>
    {
        public void Configure(EntityTypeBuilder<GeneralConfig> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email).IsRequired().HasMaxLength(200);
            builder.Property(x => x.EmailPassword).IsRequired();
            builder.Property(x => x.SMTP).IsRequired().HasMaxLength(50);
            builder.Property(x => x.SMTPPort).IsRequired().HasMaxLength(10);
        }
    }
}
