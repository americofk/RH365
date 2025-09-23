using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeExtraHours;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeExtraHours
{
    public class EmployeeExtraHourQueryHandler : IQueryHandler<EmployeeExtraHourResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeExtraHourQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<EmployeeExtraHourResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.EmployeeExtraHours
                .OrderBy(x => x.WorkedDay)
                .Where(x => x.EmployeeId == (string)queryfilter)
                .AsQueryable();

            SearchFilter<EmployeeExtraHour> validSearch = new SearchFilter<EmployeeExtraHour>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<EmployeeExtraHour>.GetLambdaExpession(validSearch);

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

            return new PagedResponse<IEnumerable<EmployeeExtraHourResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }


        public async Task<Response<EmployeeExtraHourResponse>> GetId(object condition)
        {
            string[] a = (string[])condition;
            var response = await _dbContext.EmployeeExtraHours
                .OrderBy(x => x.WorkedDay)
                .Where(x => x.EmployeeId == a[0] && x.WorkedDay == DateTime.Parse(a[1]) && x.EarningCodeId == a[2])
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

            return new Response<EmployeeExtraHourResponse>(response);
        }


        private static EmployeeExtraHourResponse SetObjectResponse(EmployeeExtraHour employeeExtraHour, EarningCode earningCode, Payroll payroll)
        {
            var a = BuildDtoHelper<EmployeeExtraHourResponse>.OnBuild(employeeExtraHour, new EmployeeExtraHourResponse());
            a.EarningCodeName = earningCode.Name;
            a.PayrollName = payroll.Name;

            return a;
        }
    }
}
