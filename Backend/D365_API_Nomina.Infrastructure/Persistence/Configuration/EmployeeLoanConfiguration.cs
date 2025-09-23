using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class EmployeeLoanConfiguration : IEntityTypeConfiguration<EmployeeLoan>
    {
        public void Configure(EntityTypeBuilder<EmployeeLoan> builder)
        {
            builder.HasKey(x => new { x.InternalId, x.EmployeeId });

            builder.Property(x => x.ValidFrom).IsRequired();
            builder.Property(x => x.ValidTo).IsRequired();
            builder.Property(x => x.LoanAmount).IsRequired();

            builder.HasOne<Payroll>()
                .WithMany()
                .HasForeignKey(x => x.PayrollId)
                .IsRequired();

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .IsRequired();

            builder.HasOne<Loan>()
                .WithMany()
                .HasForeignKey(x => x.LoanId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
