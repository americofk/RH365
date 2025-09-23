using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class EmployeeImageConfiguration : IEntityTypeConfiguration<EmployeeImage>
    {
        public void Configure(EntityTypeBuilder<EmployeeImage> builder)
        {
            builder.HasKey(x => x.EmployeeId);
            builder.Property(x => x.EmployeeId).ValueGeneratedNever();

            builder.Property(x => x.Extension).HasMaxLength(4).IsRequired();

        }
    }
}
