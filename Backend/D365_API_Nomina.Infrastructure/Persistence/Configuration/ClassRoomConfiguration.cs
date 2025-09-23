using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class ClassRoomConfiguration : IEntityTypeConfiguration<ClassRoom>
    {
        public void Configure(EntityTypeBuilder<ClassRoom> builder)
        {
            builder.HasKey(x =>x.ID);
            builder.Property(x => x.ID).HasDefaultValueSql("FORMAT((NEXT VALUE FOR dbo.ClassRoomId),'CR-00000000#')")
                .HasMaxLength(20);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.MaxStudentQty).IsRequired();
            builder.Property(x => x.Comment).HasMaxLength(100);
            builder.Property(x => x.AvailableTimeStart).IsRequired();
            builder.Property(x => x.AvailableTimeEnd).IsRequired();

            builder.HasOne<CourseLocation>()
                .WithMany()
                .HasForeignKey(x => x.CourseLocationRefRecID)
                .IsRequired();
        }
    }
}
