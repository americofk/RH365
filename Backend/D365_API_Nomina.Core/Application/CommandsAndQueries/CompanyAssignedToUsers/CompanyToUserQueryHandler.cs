using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.CompanyAssignedToUsers;
using D365_API_Nomina.Core.Application.Common.Model.MenuAssignedToUsers;
using D365_API_Nomina.Core.Application.Common.Model.User;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.CompanyAssignedToUsers
{
    public class CompanyToUserQueryHandler : IQueryAllHandler<CompanyToUserResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public CompanyToUserQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<CompanyToUserResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryFilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var user = await _dbContext.Users.Where(x => x.Alias == (string)queryFilter).FirstOrDefaultAsync();

            List<CompanyToUserResponse> response = null;
            IQueryable<CompaniesAssignedToUser> tmpResponse;
            IQueryable<Company> tmpResponseCmpy;

            if (user.ElevationType == Domain.Enums.AdminType.LocalAdmin)
            {
                tmpResponseCmpy = _dbContext.Companies
                                            .AsQueryable();

                SearchFilter<Company> validSearch = new SearchFilter<Company>(searchFilter.PropertyName, searchFilter.PropertyValue);
                if (validSearch.IsValid())
                {
                    var lambda = GenericSearchHelper<Company>.GetLambdaExpession(validSearch);

                    tmpResponseCmpy = tmpResponseCmpy.Where(lambda)
                                               .AsQueryable();
                }

                response = await tmpResponseCmpy.Select(x => SetObjectResponse(x)).ToListAsync();
            }
            else
            {
                tmpResponse =  _dbContext.CompaniesAssignedToUsers
                                    .OrderBy(x => x.CompanyId)
                                    .Where(x => x.Alias == (string)queryFilter)
                                    .AsQueryable();

                SearchFilter<CompaniesAssignedToUser> validSearch = new SearchFilter<CompaniesAssignedToUser>(searchFilter.PropertyName, searchFilter.PropertyValue);
                if (validSearch.IsValid())
                {
                    var lambda = GenericSearchHelper<CompaniesAssignedToUser>.GetLambdaExpession(validSearch);

                    tmpResponse = tmpResponse.Where(lambda)
                                               .AsQueryable();
                }

                response = await tmpResponse
                    .Join(_dbContext.Companies,
                        assigned => assigned.CompanyId,
                        company => company.CompanyId,
                        (assigned, company) => new { Assigned = assigned, Company = company })
                    .Select(x => SetObjectResponse(x.Assigned, x.Company))
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            }

            return new PagedResponse<IEnumerable<CompanyToUserResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        private static CompanyToUserResponse SetObjectResponse(CompaniesAssignedToUser companyToUser, Company company)
        {
            var a = BuildDtoHelper<CompanyToUserResponse>.OnBuild(companyToUser, new CompanyToUserResponse());
            a.CompanyName = company.Name;
            return a;
        }

        private static CompanyToUserResponse SetObjectResponse(Company company)
        {
            var a = BuildDtoHelper<CompanyToUserResponse>.OnBuild(company, new CompanyToUserResponse());
            a.CompanyName = company.Name;
            return a;
        }
    }
}
