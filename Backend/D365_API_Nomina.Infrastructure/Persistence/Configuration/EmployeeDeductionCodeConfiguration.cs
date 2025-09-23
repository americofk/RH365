using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    public class EmployeeDeductionCodeConfiguration : IEntityTypeConfiguration<EmployeeDeductionCode>
    {
        public void Configure(EntityTypeBuilder<EmployeeDeductionCode> builder)
        {
            builder.HasKey(x => new { x.DeductionCodeRefRecID, x.EmployeeRefRecID, x.PayrollRefRecID });

            builder.Property(x => x.FromDate).IsRequired();
            builder.Property(x => x.ToDate).IsRequired();
            //builder.Property(x => x.IndexDeduction).IsRequired();
            //builder.Property(x => x.PercentContribution).IsRequired();
            //builder.Property(x => x.PercentDeduction).IsRequired();

            builder.Property(x => x.Comment)
                .HasMaxLength(200);

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeRefRecID)
                .IsRequired();

            builder.HasOne<Payroll>()
                .WithMany()
                .HasForeignKey(x => x.PayrollRefRecID)
                .IsRequired();

            builder.HasOne<DeductionCode>()
                .WithMany()
                .HasForeignKey(x => x.DeductionCodeRefRecID)
                .IsRequired();
        }
    }
}
