using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.Users;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Users
{
    public class UserQueryHandler : IQueryHandler<UserResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public UserQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<UserResponse>>> GetAll(PaginationFilter filter, 
                                                                           SearchFilter searchFilter, 
                                                                           object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.Users
                .OrderBy(x => x.Alias)
                .AsQueryable();

            SearchFilter<User> validSearch = new SearchFilter<User>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<User>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                .Take(validFilter.PageSize)
                                .Select(x => BuildDtoHelper<UserResponse>.OnBuild(x, new UserResponse()))
                                .ToListAsync();

            return new PagedResponse<IEnumerable<UserResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<UserResponse>> GetId(object condition)
        {

            var response = await _dbContext.Users
                .Where(x=> x.Alias == (string)condition)
                .Select(x => BuildDtoHelper<UserResponse>.OnBuild(x, new UserResponse()))
                .FirstOrDefaultAsync();

            return new Response<UserResponse>(response);
        }
    }
}
