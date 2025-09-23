using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class PayrollProcessActionConfiguration : IEntityTypeConfiguration<PayrollProcessAction>
    {
        public void Configure(EntityTypeBuilder<PayrollProcessAction> builder)
        {
            builder.HasKey(x => new { x.InternalId, x.PayrollProcessId, x.EmployeeId });

            builder.Property(x => x.InternalId).IsRequired()
                .ValueGeneratedNever();

            builder.Property(x => x.PayrollActionType);
            builder.Property(x => x.ActionName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.ActionId).HasMaxLength(25).IsRequired();
            builder.Property(x => x.ActionAmount).IsRequired();
            builder.Property(x => x.ApplyTax).IsRequired();
            builder.Property(x => x.ApplyTSS).IsRequired();

            builder.HasOne<PayrollProcess>()
                .WithMany()
                .HasForeignKey(x => x.PayrollProcessId)
                .IsRequired();

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .IsRequired();

        }
    }
}
