using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(x => x.DepartmentId);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(60);
            builder.Property(x => x.QtyWorkers).IsRequired();
            //builder.Property(x => x.ResponsibleId).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(100);
            builder.Property(x => x.DepartamentStatus).IsRequired();

            builder.Property(x => x.DepartmentId).HasDefaultValueSql("FORMAT((NEXT VALUE FOR dbo.DepartmentId),'DPT-00000000#')")
                    .HasMaxLength(20);
            
        }
    }
}
