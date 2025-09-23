using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeAddress;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeesAddress
{
    public class EmployeeAddressQueryHandler : IQueryHandler<EmployeeAddressResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeAddressQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<EmployeeAddressResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.EmployeesAddress
                .Where(x => x.EmployeeId == (string)queryfilter)
                .AsQueryable();

            SearchFilter<EmployeeAddress> validSearch = new SearchFilter<EmployeeAddress>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<EmployeeAddress>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Join(_dbContext.Provinces,
                                    employeeaddress => employeeaddress.Province,
                                    province => province.ProvinceId,
                                    (employeeaddress, province) => new { Employeeaddress = employeeaddress, Province = province })
                            .Select(x => SetObjectResponse(x.Employeeaddress, x.Province))
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<EmployeeAddressResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<EmployeeAddressResponse>> GetId(object condition)
        {
            string[] a = (string[])condition;

            var response = await _dbContext.EmployeesAddress
                            .Where(x => x.EmployeeId == a[0] && x.InternalId == int.Parse( a[1]))
                            .Join(_dbContext.Provinces,
                                    employeeaddress => employeeaddress.Province,
                                    province => province.ProvinceId,
                                    (employeeaddress, province) => new { Employeeaddress = employeeaddress, Province = province })
                            .Select(x => SetObjectResponse(x.Employeeaddress, x.Province))
                            .FirstOrDefaultAsync();

            return new Response<EmployeeAddressResponse>(response);
        }

        private static EmployeeAddressResponse SetObjectResponse(EmployeeAddress employeeAddress, Province province)
        {
            var a = BuildDtoHelper<EmployeeAddressResponse>.OnBuild(employeeAddress, new EmployeeAddressResponse());
            a.ProvinceName = province.Name;

            return a;
        }
    }

}
