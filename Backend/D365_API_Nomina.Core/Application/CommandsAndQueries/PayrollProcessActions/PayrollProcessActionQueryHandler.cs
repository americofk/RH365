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
    public class PayrollProcessActionQueryHandler : IQueryAllHandler<PayrollProcessAction>
    {
        private readonly IApplicationDbContext _dbContext;

        public PayrollProcessActionQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        //queryFilter 0 - EmployeeId; 1 - PayrollProcessId
        public async Task<PagedResponse<IEnumerable<PayrollProcessAction>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            string[] a = (string[])queryfilter;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.PayrollProcessActions
                .OrderBy(x => x.PayrollActionType)
                .Where(x => x.EmployeeId == a[0] && x.PayrollProcessId == a[1])
                .AsQueryable();

            SearchFilter<PayrollProcessAction> validSearch = new SearchFilter<PayrollProcessAction>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<PayrollProcessAction>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse.ToListAsync();

            //.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            //.Take(validFilter.PageSize)
            return new PagedResponse<IEnumerable<PayrollProcessAction>>(response, validFilter.PageNumber, validFilter.PageSize);
        }
    }

}
