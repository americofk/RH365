using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeHistories;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeHistories
{
    public class EmployeeHistoryQueryHandler : IQueryAllHandler<EmployeeHistoryResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeHistoryQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<EmployeeHistoryResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.EmployeeHistories
                .OrderBy(x => x.EmployeeHistoryId)
                .AsQueryable();

            SearchFilter<EmployeeHistory> validSearch = new SearchFilter<EmployeeHistory>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<EmployeeHistory>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Where(x => x.EmployeeId == (string)queryfilter)
                            .Select(x => BuildDtoHelper<EmployeeHistoryResponse>.OnBuild(x, new EmployeeHistoryResponse()))
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<EmployeeHistoryResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

    }
}
