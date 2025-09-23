using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Occupations
{
    public class OccupationQueryHandler: IQueryAllWithoutSearchHandler<Occupation>
    {
        private readonly IApplicationDbContext _dbContext;

        public OccupationQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<IEnumerable<Occupation>>> GetAll(PaginationFilter filter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var response = await _dbContext.Occupations
                            .OrderBy(x => x.OccupationId)
                            //.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            //.Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<Occupation>>(response, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
