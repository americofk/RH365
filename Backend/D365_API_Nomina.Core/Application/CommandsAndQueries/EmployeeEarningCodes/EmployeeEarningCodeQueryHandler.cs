using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeEarningCodes;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeEarningCodes
{
    public class EmployeeEarningCodeQueryHandler : IQueryHandler<EmployeeEarningCodeResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeEarningCodeQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<EmployeeEarningCodeResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.EmployeeEarningCodes
                .OrderBy(x => x.EarningCodeId)
                .Where(x => x.EmployeeId == (string)queryfilter)
                .AsQueryable();

            SearchFilter<EmployeeEarningCode> validSearch = new SearchFilter<EmployeeEarningCode>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<EmployeeEarningCode>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Join(_dbContext.EarningCodes,
                                employeeearning => employeeearning.EarningCodeId,
                                earning => earning.EarningCodeId,
                                (employeeearning, earning) => new { EmployeeEarning = employeeearning, Earning = earning })
                            .Join(_dbContext.Payrolls,
                                join => join.EmployeeEarning.PayrollId,
                                payroll => payroll.PayrollId,
                                (join, payroll) => new { Join = join, Payroll = payroll })
                            .Select(x => SetObjectResponse(x.Join.EmployeeEarning, x.Join.Earning, x.Payroll))
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<EmployeeEarningCodeResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<EmployeeEarningCodeResponse>> GetId(object condition)
        {
            string[] a = (string[])condition;

            var response = await _dbContext.EmployeeEarningCodes
                .Where(x => x.EmployeeId == a[0] && x.InternalId == int.Parse(a[1]))
                .Join(_dbContext.EarningCodes,
                    employeeearning => employeeearning.EarningCodeId,
                    earning => earning.EarningCodeId,
                    (employeeearning, earning) => new { EmployeeEarning = employeeearning, Earning = earning })
                .Join(_dbContext.Payrolls,
                    join => join.EmployeeEarning.PayrollId,
                    payroll => payroll.PayrollId,
                    (join, payroll) => new { Join = join, Payroll = payroll })
                .Select(x => SetObjectResponse(x.Join.EmployeeEarning, x.Join.Earning, x.Payroll))
                .FirstOrDefaultAsync();

            return new Response<EmployeeEarningCodeResponse>(response);
        }

        private static EmployeeEarningCodeResponse SetObjectResponse(EmployeeEarningCode employeeEarningCode, EarningCode earningCode, Payroll payroll)
        {
            var a = BuildDtoHelper<EmployeeEarningCodeResponse>.OnBuild(employeeEarningCode, new EmployeeEarningCodeResponse());
            a.EarningName = earningCode.Name;
            a.PayrollName = payroll.Name;

            return a;
        }
    }

}
