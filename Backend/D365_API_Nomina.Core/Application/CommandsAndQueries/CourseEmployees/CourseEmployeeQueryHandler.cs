using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.CourseEmployees;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.CourseEmployees
{
    public class CourseEmployeeQueryHandler : IQueryHandler<CourseEmployeeResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public CourseEmployeeQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        //Queryfilter = courseid
        public async Task<PagedResponse<IEnumerable<CourseEmployeeResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.CourseEmployees
                .OrderBy(x => x.CourseId)
                .Where(x => x.CourseId == (string)queryfilter)
                .AsQueryable();

            SearchFilter<CourseEmployee> validSearch = new SearchFilter<CourseEmployee>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<CourseEmployee>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Join(_dbContext.Employees,
                                courseemployees => courseemployees.EmployeeId,
                                employee => employee.EmployeeId,
                                (courseemployees, employee) => new { CourseEmployees = courseemployees, Employee = employee })
                            .Join(_dbContext.EmployeePositions,
                                join => join.Employee.EmployeeId,
                                employeeposition => employeeposition.EmployeeId,
                                (join, employeeposition) => new { Join = join, EmployeePosition = employeeposition })
                            .Join(_dbContext.Positions,
                                joinPosi => joinPosi.EmployeePosition.PositionId,
                                position => position.PositionId,
                                (joinPosi, position) => new { JoinPosi = joinPosi, Position = position })
                            .Join(_dbContext.Departments,
                                joinDepa => joinDepa.Position.DepartmentId,
                                department => department.DepartmentId,
                                (joinDepa, department) => new { JoinDepa = joinDepa, Department = department })
                            .Where(x => x.JoinDepa.JoinPosi.EmployeePosition.EmployeePositionStatus == true)
                            .Select(x => SetObjectResponse(x.JoinDepa.JoinPosi.Join.CourseEmployees, x.JoinDepa.JoinPosi.Join.Employee, x.Department))
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<CourseEmployeeResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }


        private static CourseEmployeeResponse SetObjectResponse(CourseEmployee courseEmployee, Employee employee, Department department)
        {
            var a = BuildDtoHelper<CourseEmployeeResponse>.OnBuild(courseEmployee, new CourseEmployeeResponse());
            a.EmployeeName = $"{employee.Name} {employee.LastName}";
            a.DepartmentName = department.Name;
            //a.CourseName = course.CourseName;
            return a;
        }


        //Condition 0 = Courseid, 1 = EmployeeId
        public async Task<Response<CourseEmployeeResponse>> GetId(object condition)
        {
            string[] filter = (string[])condition;

            var response = await _dbContext.CourseEmployees
                .Where(x => x.CourseId == filter[0] && x.EmployeeId == filter[1])
                .Join(_dbContext.Employees,
                    courseemployees => courseemployees.EmployeeId,
                    employee => employee.EmployeeId,
                    (courseemployees, employee) => new { CourseEmployees = courseemployees, Employee = employee })
                .Join(_dbContext.EmployeePositions,
                    join => join.Employee.EmployeeId,
                    employeeposition => employeeposition.EmployeeId,
                    (join, employeeposition) => new { Join = join, EmployeePosition = employeeposition })
                .Join(_dbContext.Positions,
                    joinPosi => joinPosi.EmployeePosition.PositionId,
                    position => position.PositionId,
                    (joinPosi, position) => new { JoinPosi = joinPosi, Position = position })
                .Join(_dbContext.Departments,
                    joinDepa => joinDepa.Position.DepartmentId,
                    department => department.DepartmentId,
                    (joinDepa, department) => new { JoinDepa = joinDepa, Department = department })
                .Where(x => x.JoinDepa.JoinPosi.EmployeePosition.EmployeePositionStatus == true)
                .Select(x => SetObjectResponse(x.JoinDepa.JoinPosi.Join.CourseEmployees, x.JoinDepa.JoinPosi.Join.Employee, x.Department))
                .FirstOrDefaultAsync();

            return new Response<CourseEmployeeResponse>(response);
        }
    }

}
