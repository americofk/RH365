using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    public class EmployeeImageConfiguration : IEntityTypeConfiguration<EmployeeImage>
    {
        public void Configure(EntityTypeBuilder<EmployeeImage> builder)
        {
            builder.HasKey(x => x.EmployeeRefRecID);
            builder.Property(x => x.EmployeeRefRecID).ValueGeneratedNever();
            builder.Property(x => x.Extension).HasMaxLength(4).IsRequired();

        }
    }
}
