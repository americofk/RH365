using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(x => x.EmployeeId).HasDefaultValueSql("FORMAT((NEXT VALUE FOR dbo.EmployeeId),'EMP-00000000#')")
                .HasMaxLength(20);

            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Property(x => x.LastName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.PersonalTreatment).HasMaxLength(50);
            builder.Property(x => x.BirthDate).IsRequired();
            builder.Property(x => x.Gender).IsRequired();
            builder.Property(x => x.Age).IsRequired();
            builder.Property(x => x.DependentsNumbers);
            builder.Property(x => x.MaritalStatus).IsRequired();
            builder.Property(x => x.NSS).IsRequired().HasMaxLength(20);
            builder.Property(x => x.ARS).IsRequired().HasMaxLength(20);
            builder.Property(x => x.AFP).IsRequired().HasMaxLength(20);

            builder.Property(x => x.AdmissionDate);
            builder.Property(x => x.EmployeeType).IsRequired();
            builder.Property(x => x.HomeOffice).IsRequired();
            builder.Property(x => x.OwnCar).IsRequired();
            builder.Property(x => x.HasDisability).IsRequired();

            builder.Property(x => x.WorkFrom);
            builder.Property(x => x.WorkTo);
            builder.Property(x => x.BreakWorkFrom);
            builder.Property(x => x.BreakWorkTo);

            builder.Property(x => x.Nationality).HasMaxLength(5);
            builder.Property(x => x.LocationId).HasMaxLength(10);


            builder.Property(x => x.IsFixedWorkCalendar);

            //builder.Property(x => x.Image);

            builder.HasOne<Country>()
                .WithMany()
                .HasForeignKey(x => x.Country)
                .IsRequired();
            
            builder.HasOne<Occupation>()
                .WithMany()
                .HasForeignKey(x => x.OccupationId)
                .IsRequired();
            
            builder.HasOne<EducationLevel>()
                .WithMany()
                .HasForeignKey(x => x.EducationLevelId)
                .IsRequired();
            
            builder.HasOne<DisabilityType>()
                .WithMany()
                .HasForeignKey(x => x.DisabilityTypeId)
                .IsRequired();

        }
    }
}
