using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeWorkCalendars;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeWorkCalendars
{
    public class EmployeeWorkCalendarQueryHandler: IQueryHandler<EmployeeWorkCalendarResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeWorkCalendarQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<IEnumerable<EmployeeWorkCalendarResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.EmployeeWorkCalendars
                .OrderByDescending(x => x.CalendarDate)
                .Where(x => x.EmployeeId == (string)queryfilter)
                .AsQueryable();

            SearchFilter<EmployeeWorkCalendar> validSearch = new SearchFilter<EmployeeWorkCalendar>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<EmployeeWorkCalendar>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Select(x => SetObjectResponse(x))
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<EmployeeWorkCalendarResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<EmployeeWorkCalendarResponse>> GetId(object condition)
        {
            string[] a = (string[])condition;

            var response = await _dbContext.EmployeeWorkCalendars
                .Where(x => x.InternalId == int.Parse(a[1]) && x.EmployeeId == a[0])
                .Select(x => SetObjectResponse(x))
                .FirstOrDefaultAsync();

            return new Response<EmployeeWorkCalendarResponse>(response);
        }

        private static EmployeeWorkCalendarResponse SetObjectResponse(EmployeeWorkCalendar employeeWorkCalendar)
        {
            var a = BuildDtoHelper<EmployeeWorkCalendarResponse>.OnBuild(employeeWorkCalendar, new EmployeeWorkCalendarResponse());
            return a;
        }
    }
}
