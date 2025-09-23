using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.HasKey(x => x.PositionId);
            builder.Property(x => x.PositionId).HasDefaultValueSql("FORMAT((NEXT VALUE FOR dbo.PositionId),'POS-00000000#')")
                .HasMaxLength(20);

            builder.Property(x => x.PositionName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.PositionStatus);
            builder.Property(x => x.IsVacant);
            builder.Property(x => x.NotifyPositionId).HasMaxLength(20);

            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(200);

            builder.HasOne<Department>()
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<Job>()
                .WithMany()
                .HasForeignKey(x => x.JobId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
