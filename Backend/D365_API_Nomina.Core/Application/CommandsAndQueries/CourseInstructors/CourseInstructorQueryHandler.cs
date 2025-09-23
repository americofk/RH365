using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.CourseInstructors;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.CourseInstructors
{
    public class CourseInstructorQueryHandler : IQueryHandler<CourseInstructorResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public CourseInstructorQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }


        public async Task<PagedResponse<IEnumerable<CourseInstructorResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.CourseInstructors
                                    .OrderBy(x => x.InstructorId)
                                    .Where(x => x.CourseId == (string)queryfilter)
                                    .AsQueryable();

            SearchFilter<CourseInstructor> validSearch = new SearchFilter<CourseInstructor>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<CourseInstructor>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                                .Join(_dbContext.Instructors,
                                    courseinstructor => courseinstructor.InstructorId,
                                    instructor => instructor.InstructorId,
                                    (courseinstructor, instructor) => new { CourseInstructor = courseinstructor, Instructor = instructor })
                                .Join(_dbContext.Courses,
                                    join => join.CourseInstructor.CourseId,
                                    course => course.CourseId,
                                    (join, course) => new { Join = join, Course = course })
                                .Select(x => SetObjectResponse(x.Join.CourseInstructor, x.Join.Instructor, x.Course))
                                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                .Take(validFilter.PageSize)
                                .ToListAsync();

            return new PagedResponse<IEnumerable<CourseInstructorResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }


        private static CourseInstructorResponse SetObjectResponse(CourseInstructor courseInstructor, Instructor instructor, Course course)
        {
            var a = BuildDtoHelper<CourseInstructorResponse>.OnBuild(courseInstructor, new CourseInstructorResponse());
            a.InstructorName = instructor.Name;
            a.CourseName = course.CourseName;
            return a;
        }

        //condition 0 = courseid, 1 = instructor
        public async Task<Response<CourseInstructorResponse>> GetId(object condition)
        {
            string[] a = (string[])condition;

            var response = await _dbContext.CourseInstructors
                .Where(x => x.CourseId == a[0] && x.InstructorId == a[1])
                .Join(_dbContext.Instructors,
                        courseinstructor => courseinstructor.InstructorId,
                        instructor => instructor.InstructorId,
                        (courseinstructor, instructor) => new { CourseInstructor = courseinstructor, Instructor = instructor })
                .Join(_dbContext.Courses,
                        join => join.CourseInstructor.CourseId,
                        course => course.CourseId,
                        (join, course) => new { Join = join, Course = course })
                .Select(x => SetObjectResponse(x.Join.CourseInstructor, x.Join.Instructor, x.Course))
                .FirstOrDefaultAsync();

            return new Response<CourseInstructorResponse>(response);
        }
    }
}
