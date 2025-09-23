using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class UserImageConfiguration : IEntityTypeConfiguration<UserImage>
    {
        public void Configure(EntityTypeBuilder<UserImage> builder)
        {
            builder.HasKey(x => x.Alias);
            builder.Property(x => x.Alias).ValueGeneratedNever();

            builder.Property(x => x.Extension).HasMaxLength(4).IsRequired();

            //builder.HasOne<User>().WithOne().HasForeignKey("Alias").IsRequired();
        }
    }
}
