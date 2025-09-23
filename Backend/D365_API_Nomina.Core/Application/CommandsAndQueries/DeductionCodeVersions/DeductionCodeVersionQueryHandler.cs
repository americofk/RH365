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
    public class DeductionCodeVersionQueryHandler : IQueryHandler<DeductionCodeVersion>
    {
        private readonly IApplicationDbContext _dbContext;
        public DeductionCodeVersionQueryHandler(IApplicationDbContext _applicationDbContext)
        {
            _dbContext = _applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<DeductionCodeVersion>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryFilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.DeductionCodeVersions
                                    .OrderBy(x => x.DeductionCodeId)
                                    .Where(x => x.DeductionCodeId == (string)queryFilter)
                                    .AsQueryable();

            SearchFilter<DeductionCodeVersion> validSearch = new SearchFilter<DeductionCodeVersion>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<DeductionCodeVersion>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                .Take(validFilter.PageSize)
                                .ToListAsync();

            return new PagedResponse<IEnumerable<DeductionCodeVersion>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<DeductionCodeVersion>> GetId(object condition)
        {
            string[] a = (string[])condition;

            var response = await _dbContext.DeductionCodeVersions
                .Where(x => x.DeductionCodeId == a[0] && x.InternalId == int.Parse(a[1]))
                .FirstOrDefaultAsync();

            return new Response<DeductionCodeVersion>(response);
        }
    }
}
