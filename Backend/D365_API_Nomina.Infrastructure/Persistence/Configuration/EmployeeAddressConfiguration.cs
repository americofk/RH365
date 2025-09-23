using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class EmployeeAddressConfiguration : IEntityTypeConfiguration<EmployeeAddress>
    {
        public void Configure(EntityTypeBuilder<EmployeeAddress> builder)
        {
            builder.HasKey(x => new { x.EmployeeId, x.InternalId });
            builder.Property(x => x.InternalId).ValueGeneratedNever();

            builder.Property(x => x.Street).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Home).HasMaxLength(10).IsRequired();
            builder.Property(x => x.Sector).HasMaxLength(50).IsRequired();
            builder.Property(x => x.City).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Province).HasMaxLength(50).IsRequired();
            builder.Property(x => x.ProvinceName).HasMaxLength(50).IsRequired();

            builder.Property(x => x.Comment).HasMaxLength(200);

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .IsRequired();
            
            builder.HasOne<Country>()
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);




        }
    }
}
