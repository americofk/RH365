using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    public class EmployeeContactInfConfiguration : IEntityTypeConfiguration<EmployeeContactsInf>
    {
        public void Configure(EntityTypeBuilder<EmployeeContactsInf> builder)
        {
            builder.HasKey(x => new { x.ID });
            builder.Property(x => x.ID).ValueGeneratedNever();
            builder.Property(x => x.ContactType);
            builder.Property(x => x.IsPrincipal);
            builder.Property(x => x.Comment).HasMaxLength(200);

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.ID)
                .IsRequired();
        }
    }
}
