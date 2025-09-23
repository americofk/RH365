using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.HasKey(x => x.LoanId);
            builder.Property(x => x.LoanId).HasDefaultValueSql("FORMAT((NEXT VALUE FOR dbo.LoanId),'LO-00000000#')")
                    .HasMaxLength(20);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.ValidFrom).IsRequired();
            builder.Property(x => x.ValidTo).IsRequired();
            builder.Property(x => x.LedgerAccount).HasMaxLength(30);
            builder.Property(x => x.Description).HasMaxLength(200);

            builder.Property(x => x.DepartmentId).HasMaxLength(20);
            builder.Property(x => x.ProjId).HasMaxLength(20);
            builder.Property(x => x.ProjCategoryId).HasMaxLength(20);         

        }
    }
}
