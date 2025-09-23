using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    public class EmployeeTaxConfiguration : IEntityTypeConfiguration<EmployeeTaxis>
    {
        public void Configure(EntityTypeBuilder<EmployeeTaxis> builder)
        {
            builder.HasKey(x => new { x.ID, x.EmployeeRefRecID, x.PayrollRefRecID});

            builder.Property(x => x.ValidFrom).IsRequired();
            builder.Property(x => x.ValidTo).IsRequired();

            builder.HasOne<Payroll>()
                .WithMany()
                .HasForeignKey(x => x.PayrollRefRecID)
                .IsRequired();

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeRefRecID)
                .IsRequired();

            builder.HasOne<Taxis>()
                .WithMany()
                .HasForeignKey(x => new { x.ID, x.DataareaID })
                .IsRequired();
        }
    }
}
