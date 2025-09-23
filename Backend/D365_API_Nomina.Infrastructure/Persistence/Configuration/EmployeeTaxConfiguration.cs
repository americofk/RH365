using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class EmployeeTaxConfiguration : IEntityTypeConfiguration<EmployeeTax>
    {
        public void Configure(EntityTypeBuilder<EmployeeTax> builder)
        {
            builder.HasKey(x => new { x.TaxId, x.EmployeeId, x.PayrollId});

            builder.Property(x => x.ValidFrom).IsRequired();
            builder.Property(x => x.ValidTo).IsRequired();

            builder.HasOne<Payroll>()
                .WithMany()
                .HasForeignKey(x => x.PayrollId)
                .IsRequired();

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .IsRequired();

            builder.HasOne<Tax>()
                .WithMany()
                .HasForeignKey(x => new { x.TaxId, x.InCompany })
                .IsRequired();
        }
    }
}
