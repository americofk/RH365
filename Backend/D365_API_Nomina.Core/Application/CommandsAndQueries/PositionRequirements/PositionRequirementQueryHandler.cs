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

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.PositionRequirements
{
    public class PositionRequirementQueryHandler : IQueryHandler<PositionRequirement>
    {
        private readonly IApplicationDbContext _dbContext;

        public PositionRequirementQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<PositionRequirement>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.PositionRequirements
                .OrderBy(x => x.PositionId)
                .Where(x => x.PositionId == (string)queryfilter)
                .AsQueryable();

            SearchFilter<PositionRequirement> validSearch = new SearchFilter<PositionRequirement>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<PositionRequirement>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<PositionRequirement>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<PositionRequirement>> GetId(object condition)
        {
            string[] a = (string[])condition;

            var response = await _dbContext.PositionRequirements
                .Where(x => x.PositionId == a[0] && x.Name == a[1])
                .FirstOrDefaultAsync();

            return new Response<PositionRequirement>(response);
        }
    }

}
