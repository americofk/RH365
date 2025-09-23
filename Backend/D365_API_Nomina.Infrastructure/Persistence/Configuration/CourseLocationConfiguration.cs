using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class CourseLocationConfiguration : IEntityTypeConfiguration<CourseLocation>
    {
        public void Configure(EntityTypeBuilder<CourseLocation> builder)
        {
            builder.HasKey(x => x.CourseLocationId);
            builder.Property(x => x.CourseLocationId).HasDefaultValueSql("FORMAT((NEXT VALUE FOR dbo.CourseLocationId),'CLT-00000000#')")
                .HasMaxLength(20);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Phone).HasMaxLength(20);
            builder.Property(x => x.Mail).HasMaxLength(100);
            builder.Property(x => x.Address).IsRequired().HasMaxLength(500);
            builder.Property(x => x.ContactName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Comment).HasMaxLength(100);
        }
    }
}
