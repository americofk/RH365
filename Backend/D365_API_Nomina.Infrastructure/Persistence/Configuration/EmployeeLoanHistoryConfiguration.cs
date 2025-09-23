
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    public class EmployeeLoanHistoryConfiguration : IEntityTypeConfiguration<EmployeeLoanHistory>
    {
        public void Configure(EntityTypeBuilder<EmployeeLoanHistory> builder)
        {
            builder.HasKey(x => new { x.ID, x.EmployeeLoanRefRecID, x.EmployeeRefRecID });

            builder.Property(x => x.PeriodStartDate).IsRequired();
            builder.Property(x => x.PeriodEndDate).IsRequired();
            builder.Property(x => x.LoanAmount).IsRequired();

            builder.HasOne<Payroll>()
                .WithMany()
                .HasForeignKey(x => x.PayrollRefRecID)
                .IsRequired();

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeRefRecID)
                .IsRequired();

            builder.HasOne<Loan>()
                .WithMany()
                .HasForeignKey(x => x.LoanRefRecID)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
