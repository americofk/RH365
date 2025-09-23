using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class EmployeeWorkCalendarConfiguration : IEntityTypeConfiguration<EmployeeWorkCalendar>
    {
        public void Configure(EntityTypeBuilder<EmployeeWorkCalendar> builder)
        {
            builder.HasKey(x => new { x.InternalId, x.EmployeeId});
            builder.Property(x => x.InternalId).ValueGeneratedNever();

            builder.Property(x => x.CalendarDate).IsRequired();
            builder.Property(x => x.CalendarDay).HasMaxLength(30).IsRequired();

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
