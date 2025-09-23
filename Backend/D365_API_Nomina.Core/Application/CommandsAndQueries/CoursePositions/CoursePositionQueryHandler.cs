using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.CoursePositons;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.CoursePositions
{
    public class CoursePositionQueryHandler : IQueryHandler<CoursePositionResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public CoursePositionQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<CoursePositionResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.CoursePositions
                .OrderBy(x => x.PositionId)
                .Where(x => x.CourseId == (string)queryfilter)
                .AsQueryable();

            SearchFilter<CoursePosition> validSearch = new SearchFilter<CoursePosition>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<CoursePosition>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Join(_dbContext.Positions,
                                courseposition => courseposition.PositionId,
                                position => position.PositionId,
                                (courseposition, position) => new { CoursePosition = courseposition, Position = position })
                            .Join(_dbContext.Departments,
                                join => join.Position.DepartmentId,
                                department => department.DepartmentId,
                                (join, department) => new { Join = join, Department = department })
                            .Select(x => SetObjectResponse(x.Join.CoursePosition, x.Join.Position, x.Department))
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<CoursePositionResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        private static CoursePositionResponse SetObjectResponse(CoursePosition coursePosition, Position position, Department department)
        {
            var a = BuildDtoHelper<CoursePositionResponse>.OnBuild(coursePosition, new CoursePositionResponse());
            a.PositionName = position.PositionName;
            a.DepartmentName = department.Name;
            return a;
        }

        //Condition 0 = courseid, 1 = Positionid
        public async Task<Response<CoursePositionResponse>> GetId(object condition)
        {
            string[] a = (string[])condition;

            var response = await _dbContext.CoursePositions
                .Where(x => x.CourseId == a[0] && x.PositionId == a[1])
                .Join(_dbContext.Positions,
                    courseposition => courseposition.PositionId,
                    position => position.PositionId,
                    (courseposition, position) => new { CoursePosition = courseposition, Position = position })
                .Join(_dbContext.Departments,
                    join => join.Position.DepartmentId,
                    department => department.DepartmentId,
                    (join, department) => new { Join = join, Department = department })
                .Select(x => SetObjectResponse(x.Join.CoursePosition, x.Join.Position, x.Department))
                .FirstOrDefaultAsync();

            return new Response<CoursePositionResponse>(response);
        }
    }

}
