using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Loans
{
    public class LoanQueryHandler : IQueryHandler<Loan>
    //public class LoanQueryHandler : IQueryWithSearchHandler<Loan>
    {
        private readonly IApplicationDbContext _dbContext;

        public LoanQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<Loan>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse =  _dbContext.Loans                
                .Where(x => x.LoanStatus == (bool)queryfilter)
                .AsQueryable();

            SearchFilter<Loan> validSearch = new SearchFilter<Loan>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<Loan>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                         .Take(validFilter.PageSize)
                                         .ToListAsync();

            return new PagedResponse<IEnumerable<Loan>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<Loan>> GetId(object condition)
        {
            var response = await _dbContext.Loans
                .Where(x => x.LoanId == (string)condition)
                .FirstOrDefaultAsync();

            return new Response<Loan>(response);
        }
    }
}
