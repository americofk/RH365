using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class PositionRequirementConfiguration : IEntityTypeConfiguration<PositionRequirement>
    {
        public void Configure(EntityTypeBuilder<PositionRequirement> builder)
        {
            builder.HasKey(x => new { x.Name, x.PositionId });
            builder.Property(x => x.Name).IsRequired().HasMaxLength(30);
            builder.Property(x => x.Detail).IsRequired().HasMaxLength(100);

            //ForeignKey With PositionEntity
            //Without navigation property
            builder.HasOne<Position>()
                .WithMany()
                .HasForeignKey(x => x.PositionId)
                .IsRequired();
        }
    }
}
