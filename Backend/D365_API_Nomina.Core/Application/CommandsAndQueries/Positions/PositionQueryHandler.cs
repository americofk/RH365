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

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Positions
{
    public class PositionQueryHandler : IQueryHandler<Position>
    {
        private readonly IApplicationDbContext _dbContext;

        public PositionQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        //queryFilter is array 0 = PositonStatus, 1 = IsVacants
        public async Task<PagedResponse<IEnumerable<Position>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            bool[] filters = (bool[])queryfilter;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.Positions
                .OrderBy(x => x.PositionId)
                .Where(x => x.PositionStatus == filters[0] && x.IsVacant == filters[1])
                .AsQueryable();

            SearchFilter<Position> validSearch = new SearchFilter<Position>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<Position>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<Position>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<Position>> GetId(object condition)
        {
            var response = await _dbContext.Positions
                .Where(x => x.PositionId == (string)condition)
                .FirstOrDefaultAsync();

            return new Response<Position>(response);
        }
    }

}
