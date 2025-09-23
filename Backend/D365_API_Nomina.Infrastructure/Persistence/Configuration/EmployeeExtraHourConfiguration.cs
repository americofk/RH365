
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    public class EmployeeExtraHourConfiguration : IEntityTypeConfiguration<EmployeeExtraHour>
    {
        public void Configure(EntityTypeBuilder<EmployeeExtraHour> builder)
        {
            builder.HasKey(x => new { x.EmployeeRefRecID, x.EarningCodeRefRecID, x.WorkedDay });

            builder.Property(x => x.StartHour).IsRequired();
            builder.Property(x => x.EndHour).IsRequired();
            builder.Property(x => x.Indice).IsRequired();

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

            builder.HasOne<EarningCode>()
                .WithMany()
                .HasForeignKey(x => x.EarningCodeRefRecID)
                .IsRequired();
        }
    }
}
