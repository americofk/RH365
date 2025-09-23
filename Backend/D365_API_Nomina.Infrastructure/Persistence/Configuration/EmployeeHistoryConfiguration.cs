using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class EmployeeHistoryConfiguration : IEntityTypeConfiguration<EmployeeHistory>
    {
        public void Configure(EntityTypeBuilder<EmployeeHistory> builder)
        {
            builder.HasKey(x => x.EmployeeHistoryId);
            builder.Property(x => x.EmployeeHistoryId).HasDefaultValueSql("FORMAT((NEXT VALUE FOR dbo.EmployeeHistoryId),'EH-00000000#')")
                .HasMaxLength(20);

            builder.Property(x => x.RegisterDate).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Type).HasMaxLength(5).IsRequired();

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .IsRequired();
        }
    }
}
