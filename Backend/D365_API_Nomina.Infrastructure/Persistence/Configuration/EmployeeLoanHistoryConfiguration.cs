
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class EmployeeLoanHistoryConfiguration : IEntityTypeConfiguration<EmployeeLoanHistory>
    {
        public void Configure(EntityTypeBuilder<EmployeeLoanHistory> builder)
        {
            builder.HasKey(x => new { x.InternalId, x.ParentInternalId, x.EmployeeId});

            builder.Property(x => x.PeriodStartDate).IsRequired();
            builder.Property(x => x.PeriodEndDate).IsRequired();
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
