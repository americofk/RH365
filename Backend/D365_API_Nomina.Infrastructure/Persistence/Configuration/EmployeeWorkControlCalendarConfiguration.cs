using DC365_PayrollHR.Core.Domain.Consts;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    public class EmployeeWorkControlCalendarConfiguration : IEntityTypeConfiguration<EmployeeWorkControlCalendar>
    {
        public void Configure(EntityTypeBuilder<EmployeeWorkControlCalendar> builder)
        {
            builder.HasKey(x => new { x.InternalId, x.EmployeeId });
            builder.Property(x => x.InternalId).ValueGeneratedNever();

            builder.Property(x => x.CalendarDate).IsRequired();
            builder.Property(x => x.CalendarDay).HasMaxLength(30).IsRequired();

            builder.Property(x => x.TotalHour).HasColumnType(ColumnTypeConst.decimaltype);

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
