using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    public class EmployeeDocumentConfiguration : IEntityTypeConfiguration<EmployeeDocument>
    {
        public void Configure(EntityTypeBuilder<EmployeeDocument> builder)
        {
            builder.HasKey(x => new { x.EmployeeRefRecID, x.ID });
            builder.Property(x => x.ID).ValueGeneratedNever();

            builder.Property(x => x.DocumentType).HasMaxLength(100).IsRequired();
            builder.Property(x => x.DueDate).IsRequired();
            builder.Property(x => x.DocumentNumber).HasMaxLength(30).IsRequired();
            builder.Property(x => x.FileAttach);

            builder.Property(x => x.Comment).HasMaxLength(200);
            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeRefRecID)
                .IsRequired();
        }
    }
}
