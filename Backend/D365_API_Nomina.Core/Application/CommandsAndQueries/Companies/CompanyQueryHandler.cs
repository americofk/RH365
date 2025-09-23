using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.Companies;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Companies
{
    public interface ICompanyQueryHandler: IQueryHandler<CompanyResponse>
    {
        public Task<PagedResponse<IEnumerable<CompanyResponse>>> GetAll(PaginationFilter filter);
    }

    public class CompanyQueryHandler : ICompanyQueryHandler
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserInformation _currentUserInformation;

        public CompanyQueryHandler(IApplicationDbContext applicationDbContext, ICurrentUserInformation currentUserInformation)
        {
            _dbContext = applicationDbContext;
            _currentUserInformation = currentUserInformation;
        }

        public async Task<PagedResponse<IEnumerable<CompanyResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var adminType = (AdminType)Enum.Parse(typeof(AdminType), _currentUserInformation.ElevationType);

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            IQueryable<Company> tempResponse;
            List<CompanyResponse> response;

            if (adminType == AdminType.LocalAdmin)
            {
                tempResponse =  _dbContext.Companies
                                        .OrderBy(x => x.CompanyId)
                                        .AsQueryable();

                SearchFilter<Company> validSearch = new SearchFilter<Company>(searchFilter.PropertyName, searchFilter.PropertyValue);
                if (validSearch.IsValid())
                {
                    var lambda = GenericSearchHelper<Company>.GetLambdaExpession(validSearch);

                    tempResponse = tempResponse.Where(lambda)
                                               .AsQueryable();
                }

                response = await tempResponse
                            .Where(x => x.CompanyStatus == true)
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .Select(x => BuildDtoHelper<CompanyResponse>.OnBuild(x, new CompanyResponse()))
                            .ToListAsync();
            }
            else
            {
                response = new List<CompanyResponse>();
            }

            return new PagedResponse<IEnumerable<CompanyResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<CompanyResponse>> GetId(object condition)
        {
            var response = await _dbContext.Companies
                .Where(x => x.CompanyId == (string)condition)
                .Select(x => new CompanyResponse()
                {
                    Name = x.Name,
                    CompanyId = x.CompanyId
                })
                .FirstOrDefaultAsync();

            return new Response<CompanyResponse>(response);
        }

        public async Task<PagedResponse<IEnumerable<CompanyResponse>>> GetAll(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            List<CompanyResponse> response = await _dbContext.Companies
                                     .OrderBy(x => x.CompanyId)
                                     .Where( x => x.LicenseKey != null)
                                     .Select(x => BuildDtoHelper<CompanyResponse>.OnBuild(x, new CompanyResponse()))
                                     .ToListAsync();

            return new PagedResponse<IEnumerable<CompanyResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
