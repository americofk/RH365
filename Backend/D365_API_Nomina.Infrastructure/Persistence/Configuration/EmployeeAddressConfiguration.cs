using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    public class EmployeeAddressConfiguration : IEntityTypeConfiguration<EmployeesAddress>
    {
        public void Configure(EntityTypeBuilder<EmployeesAddress> builder)
        {
            builder.HasKey(x => new { x.ID});
            builder.Property(x => x.Street).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Home).HasMaxLength(10).IsRequired();
            builder.Property(x => x.Sector).HasMaxLength(50).IsRequired();
            builder.Property(x => x.City).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Province).HasMaxLength(50).IsRequired();
            builder.Property(x => x.ProvinceName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Comment).HasMaxLength(200);

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.ID)
                .IsRequired();
            
            builder.HasOne<Country>()
                .WithMany()
                .HasForeignKey(x => x.CountryRefRecID)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
