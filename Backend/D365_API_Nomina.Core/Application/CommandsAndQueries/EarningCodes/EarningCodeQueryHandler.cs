using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.StoreServices.EarningCodes
{
    public interface IEarningCodeQueryHandler: IQueryHandler<EarningCode>
    {
        public Task<PagedResponse<IEnumerable<EarningCode>>> GetAllHours(PaginationFilter filter, object queryFilter = null);
        public Task<PagedResponse<IEnumerable<EarningCode>>> GetAllEarnings(PaginationFilter filter, object queryFilter = null);
    }

    public class EarningCodeQueryHandler : IEarningCodeQueryHandler
    {
        private readonly IApplicationDbContext dbContext;
        public EarningCodeQueryHandler(IApplicationDbContext _applicationDbContext)
        {
            dbContext = _applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<EarningCode>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryFilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = dbContext.EarningCodes
                .OrderBy(x => x.EarningCodeId)
                .Where(x => x.EarningCodeStatus == (bool)queryFilter)
                .AsQueryable();

            SearchFilter<EarningCode> validSearch = new SearchFilter<EarningCode>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<EarningCode>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<EarningCode>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<PagedResponse<IEnumerable<EarningCode>>> GetAllHours(PaginationFilter filter, object queryFilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var response = await dbContext.EarningCodes
                .OrderBy(x => x.EarningCodeId)
                .Where(x => x.EarningCodeStatus == (bool)queryFilter && x.IndexBase == IndexBase.Hour && x.MultiplyAmount != 0)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

            return new PagedResponse<IEnumerable<EarningCode>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<PagedResponse<IEnumerable<EarningCode>>> GetAllEarnings(PaginationFilter filter, object queryFilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var response = await dbContext.EarningCodes
                .OrderBy(x => x.EarningCodeId)
                .Where(x => x.EarningCodeStatus == (bool)queryFilter && x.IndexBase == IndexBase.FixedAmount)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

            return new PagedResponse<IEnumerable<EarningCode>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<EarningCode>> GetId(object condition)
        {
            var response = await dbContext.EarningCodes
                .Where(x => x.EarningCodeId == (string)condition)
                .FirstOrDefaultAsync();

            return new Response<EarningCode>(response);
        }
    }
}
