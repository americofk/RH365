using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeLoans;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeTaxes;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeTaxes
{
    public class EmployeeTaxQueryHandler : IQueryHandler<EmployeeTaxResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeTaxQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<EmployeeTaxResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.EmployeeTaxes
                .OrderBy(x => x.TaxId)
                .Where(x => x.EmployeeId == (string)queryfilter)
                .AsQueryable();

            SearchFilter<EmployeeTax> validSearch = new SearchFilter<EmployeeTax>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<EmployeeTax>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                                .Join(_dbContext.Payrolls,
                                    et => et.PayrollId,
                                    payroll => payroll.PayrollId,
                                    (et, payroll) => new { Et = et, Payroll = payroll })
                                .Select(x => SetObjectResponse(x.Et, x.Payroll))
                                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                .Take(validFilter.PageSize)
                                .ToListAsync();

            return new PagedResponse<IEnumerable<EmployeeTaxResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        private static EmployeeTaxResponse SetObjectResponse(EmployeeTax employeeTax, Payroll payroll)
        {
            var a = BuildDtoHelper<EmployeeTaxResponse>.OnBuild(employeeTax, new EmployeeTaxResponse());
            a.PayrollName = payroll.Name;
            return a;
        }

        //Condition EmployeeId = 0, Tax = 1
        public async Task<Response<EmployeeTaxResponse>> GetId(object condition)
        {
            string[] a = (string[])condition;

            var response = await _dbContext.EmployeeTaxes
                .Where(x => x.EmployeeId == a[0] && x.TaxId == a[1])
                .Join(_dbContext.Payrolls,
                    et => et.PayrollId,
                    payroll => payroll.PayrollId,
                    (et, payroll) => new { Et = et, Payroll = payroll })
                .Select(x => SetObjectResponse(x.Et, x.Payroll))
                .FirstOrDefaultAsync();

            return new Response<EmployeeTaxResponse>(response);
        }
    }

}
