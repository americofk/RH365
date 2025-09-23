using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class DisabilityTypeConfiguration : IEntityTypeConfiguration<DisabilityType>
    {
        public void Configure(EntityTypeBuilder<DisabilityType> builder)
        {
            builder.HasKey(x => x.DisabilityTypeId);
            builder.Property(x => x.DisabilityTypeId).HasMaxLength(20).IsRequired();

            builder.Property(x => x.Description).HasMaxLength(200);
        }
    }
}
