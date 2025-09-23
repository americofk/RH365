using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.CalendarHolidays;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.CalendarHolidays
{
    public class CalendarHolidayQueryHandler : IQueryAllHandler<CalendarHoliday>
    {
        private readonly IApplicationDbContext _dbContext;

        public CalendarHolidayQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<IEnumerable<CalendarHoliday>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var response = await _dbContext.CalendarHolidays
                                        .OrderByDescending(x => x.CalendarDate)
                                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                        .Take(validFilter.PageSize)
                                        .ToListAsync();                            

            return new PagedResponse<IEnumerable<CalendarHoliday>>(response, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
