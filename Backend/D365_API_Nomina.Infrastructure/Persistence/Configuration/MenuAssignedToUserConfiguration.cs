using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class MenuAssignedToUserConfiguration : IEntityTypeConfiguration<MenuAssignedToUser>
    {
        public void Configure(EntityTypeBuilder<MenuAssignedToUser> builder)
        {
            builder.HasKey(x => new { x.Alias, x.MenuId });

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.Alias)
                .IsRequired();

            builder.HasOne<MenuApp>()
                .WithMany()
                .HasForeignKey(x => x.MenuId)
                .IsRequired();
        }
    }
}
