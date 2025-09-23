using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class CalendarHolidayConfiguration : IEntityTypeConfiguration<CalendarHoliday>
    {
        public void Configure(EntityTypeBuilder<CalendarHoliday> builder)
        {
            builder.HasKey(x => x.CalendarDate);

            builder.Property(x => x.Description).IsRequired().HasMaxLength(100);
        }
    }
}
