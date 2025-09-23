using D365_API_Nomina.Core.Domain.Consts;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class EmployeeExtraHourConfiguration : IEntityTypeConfiguration<EmployeeExtraHour>
    {
        public void Configure(EntityTypeBuilder<EmployeeExtraHour> builder)
        {
            builder.HasKey(x => new { x.EmployeeId, x.EarningCodeId, x.WorkedDay });

            builder.Property(x => x.StartHour).IsRequired();
            builder.Property(x => x.EndHour).IsRequired();
            //builder.Property(x => x.TotalExtraHour).IsRequired();
            //builder.Property(x => x.TotalHour).IsRequired();
            //builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.Quantity).HasColumnType(ColumnTypeConst.decimaltype).IsRequired();
            builder.Property(x => x.Indice).IsRequired();

            builder.Property(x => x.Comment)
                .HasMaxLength(200);

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .IsRequired();

            builder.HasOne<Payroll>()
                .WithMany()
                .HasForeignKey(x => x.PayrollId)
                .IsRequired();

            builder.HasOne<EarningCode>()
                .WithMany()
                .HasForeignKey(x => x.EarningCodeId)
                .IsRequired();
        }
    }
}
