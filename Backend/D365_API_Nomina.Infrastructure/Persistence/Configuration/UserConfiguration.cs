using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Alias);
            builder.Property(x => x.Alias).ValueGeneratedNever().IsRequired().HasMaxLength(10);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(200);

            builder.Property(x => x.Password).IsRequired();

            builder.HasOne<FormatCode>().WithMany().HasForeignKey(x => x.FormatCodeId).IsRequired();
            builder.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyDefaultId);
        }
    }
}
