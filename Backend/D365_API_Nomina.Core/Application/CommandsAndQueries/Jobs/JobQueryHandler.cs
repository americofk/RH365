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

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Jobs
{
    public class JobQueryHandler : IQueryHandler<Job>
    {
        private readonly IApplicationDbContext _dbContext;

        public JobQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<Job>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.Jobs
                .OrderBy(x => x.JobId)
                .Where(x => x.JobStatus == (bool)queryfilter)
                .AsQueryable();

            SearchFilter<Job> validSearch = new SearchFilter<Job>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<Job>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<Job>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<Job>> GetId(object condition)
        {
            string[] a = (string[])condition;

            var response = await _dbContext.Jobs
                .Where(x => x.JobStatus == bool.Parse(a[0]) && x.JobId == a[1])
                .FirstOrDefaultAsync();

            return new Response<Job>(response);
        }
    }
}
