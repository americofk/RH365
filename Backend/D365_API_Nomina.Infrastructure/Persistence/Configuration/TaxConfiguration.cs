using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class TaxConfiguration : IEntityTypeConfiguration<Tax>
    {
        public void Configure(EntityTypeBuilder<Tax> builder)
        {
            builder.HasKey(x => new { x.TaxId, x.DataareaID });
            builder.Property(x => x.TaxId)
                .HasMaxLength(20);

            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.LedgerAccount).HasMaxLength(30);
            builder.Property(x => x.ValidFrom).IsRequired();
            builder.Property(x => x.ValidTo).IsRequired();
            builder.Property(x => x.Currency).HasMaxLength(5).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(200);
            builder.Property(x => x.LimitPeriod).HasMaxLength(20);


            builder.Property(x => x.ProjId).HasMaxLength(20);
            builder.Property(x => x.ProjCategory).HasMaxLength(20);
            builder.Property(x => x.DepartmentId).HasMaxLength(20);



        }
    }
}
