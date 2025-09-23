using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class PayrollProcessDetailConfiguration : IEntityTypeConfiguration<PayrollProcessDetail>
    {
        public void Configure(EntityTypeBuilder<PayrollProcessDetail> builder)
        {
            //builder.HasKey(x => new { x.ProcessDetailId, x.PayrollProcessId });

            //builder.Property(x => x.ProcessDetailId).HasDefaultValueSql("FORMAT((NEXT VALUE FOR dbo.ProcessDetailsId),'00000000#')")
            //    .HasMaxLength(20);

            builder.HasKey(x => new { x.PayrollProcessId, x.EmployeeId });

            builder.Property(x => x.TotalAmount);
            builder.Property(x => x.TotalTaxAmount);
            builder.Property(x => x.TotalTssAmount);
            builder.Property(x => x.PayMethod);
            builder.Property(x => x.BankAccount).HasMaxLength(30);
            builder.Property(x => x.BankName).HasMaxLength(100);
            builder.Property(x => x.Document).HasMaxLength(30);
            builder.Property(x => x.DepartmentName).HasMaxLength(60);
            builder.Property(x => x.EmployeeName).HasMaxLength(50);
            builder.Property(x => x.PayrollProcessStatus);

            builder.HasOne<PayrollProcess>()
                .WithMany()
                .HasForeignKey(x => x.PayrollProcessId);

            builder.HasOne<Department>()
                .WithMany()
                .HasForeignKey(x => x.DepartmentId);
            
            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeId);
        }
    }
}
