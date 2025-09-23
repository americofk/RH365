using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class CompaniesAssignedToUserConfiguration : IEntityTypeConfiguration<CompaniesAssignedToUser>
    {
        public void Configure(EntityTypeBuilder<CompaniesAssignedToUser> builder)
        {
            builder.HasKey(x => new { x.Alias, x.CompanyId });

            builder.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId);

            builder.HasOne<User>().WithMany().HasForeignKey(x => x.Alias);
        }
    }
}
