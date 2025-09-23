using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class EmployeeBankAccountConfiguration : IEntityTypeConfiguration<EmployeeBankAccount>
    {
        public void Configure(EntityTypeBuilder<EmployeeBankAccount> builder)
        {
            builder.HasKey(x => new { x.EmployeeId, x.InternalId });
            builder.Property(x => x.InternalId).ValueGeneratedNever();

            builder.Property(x => x.BankName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.AccountType);
            builder.Property(x => x.AccountNum).HasMaxLength(30).IsRequired();
            builder.Property(x => x.IsPrincipal);

            builder.Property(x => x.Comment).HasMaxLength(200);
            builder.Property(x => x.Currency).HasMaxLength(5);

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .IsRequired();
        }
    }
}
