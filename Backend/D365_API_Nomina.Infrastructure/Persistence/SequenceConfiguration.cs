using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Infrastructure.Persistence.Configuration
{
    public static class SequenceConfiguration
    {
        public static void ConfigureSequences(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("PayrollId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);

            modelBuilder.HasSequence<int>("EarningCodeId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);

            modelBuilder.HasSequence<int>("DeductionCodeId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);

            modelBuilder.HasSequence<int>("DepartmentId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);

            modelBuilder.HasSequence<int>("JobId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);

            modelBuilder.HasSequence<int>("PositionId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);

            modelBuilder.HasSequence<int>("ClassRoomId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);

            modelBuilder.HasSequence<int>("CourseLocationId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);            
            
            modelBuilder.HasSequence<int>("CourseTypeId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);

            modelBuilder.HasSequence<int>("IntructorId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);
            
            modelBuilder.HasSequence<int>("CourseId")
                .HasMax(999999)
                .HasMin(1)
                .StartsAt(1);
            
            modelBuilder.HasSequence<int>("MenuId")
                .HasMax(9999)
                .HasMin(1)
                .StartsAt(1);

            modelBuilder.HasSequence<int>("EmployeeId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);

            modelBuilder.HasSequence<int>("ProjId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);
            
            modelBuilder.HasSequence<int>("ProjCategoryId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);
            
            modelBuilder.HasSequence<int>("LoanId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);
            
            modelBuilder.HasSequence<int>("PayrollProcessId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);
            
            modelBuilder.HasSequence<int>("ProcessDetailsId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);
            
            modelBuilder.HasSequence<int>("EmployeeHistoryId")
                .HasMax(999999999)
                .HasMin(1)
                .StartsAt(1);
        }
    }
}
