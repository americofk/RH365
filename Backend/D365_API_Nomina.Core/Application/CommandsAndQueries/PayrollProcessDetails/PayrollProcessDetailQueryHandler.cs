using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.PayrollProcessDetails
{
    public class PayrollProcessDetailQueryHandler : IQueryHandler<PayrollProcessDetail>
    {
        private readonly IApplicationDbContext _dbContext;

        public PayrollProcessDetailQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<PayrollProcessDetail>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.PayrollProcessDetails
                .OrderBy(x => x.EmployeeId)
                .Where(x => x.PayrollProcessId == (string)queryfilter)
                .AsQueryable();

            SearchFilter<PayrollProcessDetail> validSearch = new SearchFilter<PayrollProcessDetail>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<PayrollProcessDetail>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<PayrollProcessDetail>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        //condition 0 = payrollprocessid, 1 = employee
        public async Task<Response<PayrollProcessDetail>> GetId(object condition)
        {
            string[] a = (string[])condition;

            var response = await _dbContext.PayrollProcessDetails
                .Where(x => x.PayrollProcessId == a[0] && 
                       x.EmployeeId == a[1])
                .FirstOrDefaultAsync();

            return new Response<PayrollProcessDetail>(response);
        }
    }

}
