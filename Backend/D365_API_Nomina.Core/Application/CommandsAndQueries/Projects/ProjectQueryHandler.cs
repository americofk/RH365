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

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Projects
{
    public class ProjectQueryHandler : IQueryHandler<Project>
    {
        private readonly IApplicationDbContext _dbContext;

        public ProjectQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<Project>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.Projects
                .OrderBy(x => x.ProjId)
                .Where(x => x.ProjStatus == (bool)queryfilter)
                .AsQueryable();                

            SearchFilter<Project> validSearch = new SearchFilter<Project>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<Project>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }
            var response = await tempResponse
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<Project>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<Project>> GetId(object condition)
        {
            var response = await _dbContext.Projects
                .Where(x=> x.ProjId == (string)condition)
                .FirstOrDefaultAsync();

            return new Response<Project>(response);
        }
    }

}
