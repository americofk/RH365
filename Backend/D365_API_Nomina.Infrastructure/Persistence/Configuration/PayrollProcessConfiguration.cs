using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class PayrollProcessConfiguration : IEntityTypeConfiguration<PayrollProcess>
    {
        public void Configure(EntityTypeBuilder<PayrollProcess> builder)
        {
            builder.HasKey(x => x.PayrollProcessId);
            builder.Property(x => x.PayrollProcessId).HasDefaultValueSql("FORMAT((NEXT VALUE FOR dbo.PayrollProcessId),'PPAY-00000000#')")
                .HasMaxLength(20);

            builder.Property(x => x.Description).HasMaxLength(200);
            builder.Property(x => x.PaymentDate);
            builder.Property(x => x.EmployeeQuantity);
            builder.Property(x => x.EmployeeQuantityForPay);
            builder.Property(x => x.PeriodStartDate).IsRequired();
            builder.Property(x => x.PeriodEndDate).IsRequired();
            builder.Property(x => x.PayrollProcessStatus);
            builder.Property(x => x.ProjId).HasMaxLength(20);
            builder.Property(x => x.ProjCategoryId).HasMaxLength(20);
            builder.Property(x => x.PayCycleId);

            builder.HasOne<Payroll>()
                .WithMany()
                .HasForeignKey(x => x.PayrollId)
                .OnDelete(DeleteBehavior.NoAction);

            ////Revisar esta llave foranea, estar seguro de la implementación
            //builder.HasOne<PayCycle>()
            //    .WithMany()
            //    .HasForeignKey(x => x.PayCycleId)
            //    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
