using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.PayrollsProcess;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.PayrollsProcess
{
    public interface IPayrollProcessQueryHandler: IQueryHandler<PayrollProcessResponse>
    {
        public Task<PagedResponse<IEnumerable<PayrollProcessResponse>>> GetByParent(PaginationFilter filter, string payrollid);
    }

    public class PayrollProcessQueryHandler : IPayrollProcessQueryHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public PayrollProcessQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }


        public async Task<PagedResponse<IEnumerable<PayrollProcessResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.PayrollsProcess
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .AsQueryable();

            SearchFilter<PayrollProcess> validSearch = new SearchFilter<PayrollProcess>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<PayrollProcess>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Take(validFilter.PageSize)
                            .Select(x => BuildDtoHelper<PayrollProcessResponse>.OnBuild(x, new PayrollProcessResponse()))
                            .ToListAsync();

            return new PagedResponse<IEnumerable<PayrollProcessResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }


        public async Task<Response<PayrollProcessResponse>> GetId(object condition)
        {
            var response = await _dbContext.PayrollsProcess
                .Where(x => x.PayrollProcessId == (string)condition)
                .Select(x => BuildDtoHelper<PayrollProcessResponse>.OnBuild(x, new PayrollProcessResponse()))
                .FirstOrDefaultAsync();

            if(response != null)
            {
                var processdetails = await _dbContext.PayrollProcessDetails
                            .Where(x => x.PayrollProcessId == response.PayrollProcessId)
                            .ToListAsync();

                response.PayrollProcessDetails = processdetails;
            }

            var payrollactions = await _dbContext.PayrollProcessActions.Where(x => x.PayrollProcessId == (string)condition)
                .GroupBy(x => x.PayrollActionType)
                .Select(x => new
                {
                    Type = x.Key,
                    Total = x.Sum(x => x.ActionAmount)
                })
                .ToListAsync();

            if(payrollactions != null)
            {
                var a = payrollactions.Find(x => x.Type == PayrollActionType.Deduction);
                response.TotalDeductions = a != null?a.Total:0;

                a = payrollactions.Find(x => x.Type == PayrollActionType.Earning);
                response.TotalEarnings = a != null ? a.Total : 0;

                a = payrollactions.Find(x => x.Type == PayrollActionType.Loan);
                response.TotalLoans = a != null ? a.Total : 0;

                a = payrollactions.Find(x => x.Type == PayrollActionType.Tax);
                response.TotalTaxes = a != null ? a.Total : 0;

                a = payrollactions.Find(x => x.Type == PayrollActionType.ExtraHours);
                response.TotalEarnings += a != null ? a.Total : 0;

                response.Total = response.TotalEarnings - response.TotalDeductions - response.TotalLoans - response.TotalTaxes;
            }

            return new Response<PayrollProcessResponse>(response);
        }


        public async Task<PagedResponse<IEnumerable<PayrollProcessResponse>>> GetByParent(PaginationFilter filter, string payrollid)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var response = await _dbContext.PayrollsProcess
                .OrderBy(x => x.PayrollProcessId)
                .Where(x => x.PayrollId == payrollid && 
                      ( x.PayrollProcessStatus == PayrollProcessStatus.Paid 
                        || x.PayrollProcessStatus == PayrollProcessStatus.Closed
                        || x.PayrollProcessStatus == PayrollProcessStatus.Calculated)
                      )
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)                
                .Take(validFilter.PageSize)
                .Select(x => BuildDtoHelper<PayrollProcessResponse>.OnBuild(x, new PayrollProcessResponse()))
                .ToListAsync();

            return new PagedResponse<IEnumerable<PayrollProcessResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }
    }

}
