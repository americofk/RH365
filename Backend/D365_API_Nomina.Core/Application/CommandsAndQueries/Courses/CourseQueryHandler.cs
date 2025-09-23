using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.Course;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Courses
{
    public class CourseQueryHandler : IQueryHandler<CourseResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public CourseQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<CourseResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.Courses
                .OrderBy(x => x.CourseId)
                .AsQueryable();

            SearchFilter<Course> validSearch = new SearchFilter<Course>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<Course>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Join(_dbContext.CourseTypes,
                                course => course.CourseTypeId,
                                coursetype => coursetype.CourseTypeId,
                                (course, coursetype) => new { Course = course, CourseType = coursetype })
                            .Join(_dbContext.ClassRooms,
                                course => course.Course.ClassRoomId,
                                classroom => classroom.ClassRoomId,
                                (course, classroom) => new { Course = course, ClassRoom = classroom })
                            .Select(x => SetObjectResponse(x.Course.Course, x.ClassRoom, x.Course.CourseType))
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<CourseResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        private static CourseResponse SetObjectResponse(Course course, ClassRoom classRoom, CourseType courseType)
        {
            var a = BuildDtoHelper<CourseResponse>.OnBuild(course, new CourseResponse());
            a.ClassRoomName = classRoom.Name;
            a.CourseTypeName = courseType.Name;
            return a;
        }

        public async Task<Response<CourseResponse>> GetId(object condition)
        {
            var response = await _dbContext.Courses
                .Join(_dbContext.CourseTypes,
                    course => course.CourseTypeId,
                    coursetype => coursetype.CourseTypeId,
                    (course, coursetype) => new { Course = course, CourseType = coursetype })
                .Join(_dbContext.ClassRooms,
                    course => course.Course.ClassRoomId,
                    classroom => classroom.ClassRoomId,
                    (course, classroom) => new { Course = course, ClassRoom = classroom })
                .Where(x=> x.Course.Course.CourseId == (string)condition)
                .Select(x => SetObjectResponse(x.Course.Course, x.ClassRoom, x.Course.CourseType))
                .FirstOrDefaultAsync();

            return new Response<CourseResponse>(response);
        }
    }

}
