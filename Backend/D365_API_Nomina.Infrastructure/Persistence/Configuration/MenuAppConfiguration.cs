using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class MenuAppConfiguration : IEntityTypeConfiguration<MenuApp>
    {
        public void Configure(EntityTypeBuilder<MenuApp> builder)
        {
            builder.HasKey(x => x.MenuId);
            builder.Property(x => x.MenuId)
                .HasDefaultValueSql("FORMAT((NEXT VALUE FOR dbo.MenuId),'MENU-000#')")
                .HasMaxLength(20);

            builder.Property(x => x.MenuName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Icon).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Action).HasMaxLength(100);

            builder.HasOne<MenuApp>()
                .WithMany()
                .HasForeignKey(x => x.MenuFather);
        }
    }
}
