using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeDeparments;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeLoans;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeLoans
{
    public class EmployeeLoanQueryHandler : IQueryHandler<EmployeeLoanResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeLoanQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<EmployeeLoanResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.EmployeeLoans
                .OrderBy(x => x.LoanId)
                .Where(x => x.EmployeeId == (string)queryfilter)
                .AsQueryable();

            SearchFilter<EmployeeLoan> validSearch = new SearchFilter<EmployeeLoan>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<EmployeeLoan>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                                .Join(_dbContext.Loans,
                                    el => el.LoanId,
                                    l => l.LoanId,
                                    (el,l) => new {El = el, L = l})
                                .Join(_dbContext.Payrolls,
                                    join => join.El.PayrollId,
                                    payroll => payroll.PayrollId,
                                    (join, payroll) => new { Join = join, Payroll = payroll })
                                .Select(x => SetObjectResponse(x.Join.El, x.Join.L, x.Payroll))
                                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                .Take(validFilter.PageSize)
                                .ToListAsync();

            return new PagedResponse<IEnumerable<EmployeeLoanResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }


        private static EmployeeLoanResponse SetObjectResponse(EmployeeLoan employeeLoan, Loan loan, Payroll payroll)
        {
            var a = BuildDtoHelper<EmployeeLoanResponse>.OnBuild(employeeLoan, new EmployeeLoanResponse());
            a.LoanName = loan.Name;
            a.PayrollName = payroll.Name;
            return a;
        }


        public async Task<Response<EmployeeLoanResponse>> GetId(object condition)
        {
            string[] a = (string[])condition;

            var response = await _dbContext.EmployeeLoans
                //.Where(x => x.EmployeeId == a[0] && x.LoanId == a[1])
                .Where(x => x.EmployeeId == a[0] && x.InternalId == int.Parse(a[1]))
                .Join(_dbContext.Loans,
                    el => el.LoanId,
                    l => l.LoanId,
                    (el, l) => new { El = el, L = l })
                .Join(_dbContext.Payrolls,
                    join => join.El.PayrollId,
                    payroll => payroll.PayrollId,
                    (join, payroll) => new { Join = join, Payroll = payroll })
                .Select(x => SetObjectResponse(x.Join.El, x.Join.L, x.Payroll))
                .FirstOrDefaultAsync();

            return new Response<EmployeeLoanResponse>(response);
        }
    }
}
