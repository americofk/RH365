using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(x => x.CourseId);
            builder.Property(x => x.CourseId).HasDefaultValueSql("FORMAT((NEXT VALUE FOR dbo.CourseId),'CO-00000000#')")
                .HasMaxLength(20);

            builder.Property(x => x.CourseName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.IsMatrixTraining);
            builder.Property(x => x.InternalExternal).IsRequired();
            builder.Property(x => x.StartDateTime).IsRequired();
            builder.Property(x => x.EndDateTime).IsRequired();
            builder.Property(x => x.MinStudents).IsRequired();
            builder.Property(x => x.MaxStudents).IsRequired();
            builder.Property(x => x.Periodicity);
            builder.Property(x => x.QtySessions);
            builder.Property(x => x.CourseParentId).HasMaxLength(20);
            builder.Property(x => x.URLDocuments).HasMaxLength(1000);

            builder.Property(x => x.Description)
                .HasMaxLength(300);

            builder.Property(x => x.Objetives)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.Topics)
                .HasMaxLength(1000)
                .IsRequired();


            builder.HasOne<CourseType>()
                .WithMany()
                .HasForeignKey(x => x.CourseTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            //builder.HasOne<Course>().
            //    .WithMany()
            //    .HasForeignKey(x => x.CourseParentId)
            //    .OnDelete(DeleteBehavior.NoAction);                

            //builder.HasOne<CourseLocation>()
            //    .WithMany()
            //    .HasForeignKey(x => x.CourseLocationId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<ClassRoom>()
                .WithMany()
                .HasForeignKey(x => x.ClassRoomId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
