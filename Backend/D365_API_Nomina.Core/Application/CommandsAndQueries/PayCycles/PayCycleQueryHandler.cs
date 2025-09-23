using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.PayCycles;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.PayCycles
{
    public class PayCycleQueryHandler : IQueryHandler<PayCycleResponse>
    {
        private readonly IApplicationDbContext dbContext;

        public PayCycleQueryHandler(IApplicationDbContext _applicationDbContext)
        {
            dbContext = _applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<PayCycleResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = dbContext.PayCycles
                .OrderBy(x => x.PayCycleId)
                .Where(x => x.PayrollId == (string)queryfilter)
                .AsQueryable();

            SearchFilter<PayCycle> validSearch = new SearchFilter<PayCycle>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<PayCycle>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Join(dbContext.Payrolls,
                                c => c.PayrollId,
                                p => p.PayrollId,
                                (c, p) => new { C = c, P = p })
                            .Select(x => SetObjectResponse(x.C, x.P))
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<PayCycleResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public Task<Response<PayCycleResponse>> GetId(object condition)
        {
            throw new NotImplementedException();
        }

        private static PayCycleResponse SetObjectResponse(PayCycle paycycle, Payroll payroll)
        {
            var a = BuildDtoHelper<PayCycleResponse>.OnBuild(paycycle, new PayCycleResponse());
            a.PayFrecuency = payroll.PayFrecuency;

            return a;
        }
    }
}
