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

namespace D365_API_Nomina.Core.Application.StoreServices.DeductionCodes
{
    public class DeductionCodeQueryHandler : IQueryHandler<DeductionCode>
    {
        private readonly IApplicationDbContext _dbContext;
        public DeductionCodeQueryHandler(IApplicationDbContext _applicationDbContext)
        {
            _dbContext = _applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<DeductionCode>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryFilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.DeductionCodes
                .OrderBy(x => x.DeductionCodeId)
                .Where(x => x.DeductionStatus == (bool)queryFilter)
                .AsQueryable();

            SearchFilter<DeductionCode> validSearch = new SearchFilter<DeductionCode>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<DeductionCode>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<DeductionCode>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<DeductionCode>> GetId(object condition)
        {
            var response = await _dbContext.DeductionCodes
                .Where(x => x.DeductionCodeId == (string)condition)
                .FirstOrDefaultAsync();

            return new Response<DeductionCode>(response);
        }
    }
}
