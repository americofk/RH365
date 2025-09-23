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

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.ProjCategories
{
    public class ProjCategoryQueryHandler : IQueryHandler<ProjCategory>
    {
        private readonly IApplicationDbContext _dbContext;

        public ProjCategoryQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<ProjCategory>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.ProjCategories
                .OrderBy(x => x.ProjCategoryId)
                .Where(x => x.ProjCategoryStatus == (bool)queryfilter)
                .AsQueryable();

            SearchFilter<ProjCategory> validSearch = new SearchFilter<ProjCategory>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<ProjCategory>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<ProjCategory>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<ProjCategory>> GetId(object condition)
        {
            var response = await _dbContext.ProjCategories
                .Where(x => x.ProjCategoryId == (string)condition)
                .FirstOrDefaultAsync();

            return new Response<ProjCategory>(response);
        }
    }

}
