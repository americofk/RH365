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

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.TaxDetails
{
    public class TaxDetailQueryHandler : IQueryHandler<TaxDetail>
    {
        private readonly IApplicationDbContext _dbContext;

        public TaxDetailQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<TaxDetail>>> GetAll(PaginationFilter filter, SearchFilter searchFilter,  object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.TaxDetails
                .Where(x => x.TaxId == (string)queryfilter)
                .AsQueryable();

            SearchFilter<TaxDetail> validSearch = new SearchFilter<TaxDetail>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<TaxDetail>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                .Take(validFilter.PageSize)
                                .ToListAsync();

            return new PagedResponse<IEnumerable<TaxDetail>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        //Condition 0 = Taxid, 1 = InternalId 
        public async Task<Response<TaxDetail>> GetId(object condition)
        {
            string[] a = (string[])condition;
            var response = await _dbContext.TaxDetails
                .Where(x => x.TaxId == a[0] && x.InternalId == int.Parse(a[1]))
                .FirstOrDefaultAsync();

            return new Response<TaxDetail>(response);
        }
    }

}
