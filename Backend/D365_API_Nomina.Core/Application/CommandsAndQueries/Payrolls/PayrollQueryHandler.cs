using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.PayCycles;
using D365_API_Nomina.Core.Application.Common.Model.Payrolls;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.StoreServices.Payrolls
{
    public class PayrollQueryHandler : IQueryHandler<PayrollResponse>
    {
        private readonly IApplicationDbContext dbContext;
        private readonly IQueryHandler<PayCycleResponse> _queryHandler;

        public PayrollQueryHandler(IApplicationDbContext _applicationDbContext, IQueryHandler<PayCycleResponse> queryHandler)
        {
            dbContext = _applicationDbContext;
            _queryHandler = queryHandler;
        }


        public async Task<PagedResponse<IEnumerable<PayrollResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryFilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = dbContext.Payrolls
                .OrderBy(x => x.PayrollId)
                .Where(x => x.PayrollStatus == (bool)queryFilter)
                .AsQueryable();

            SearchFilter<Payroll> validSearch = new SearchFilter<Payroll>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<Payroll>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }
            var response = await tempResponse
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .Select(x => BuildDtoHelper<PayrollResponse>.OnBuild(x, new PayrollResponse()))
                            .ToListAsync();

            return new PagedResponse<IEnumerable<PayrollResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<PayrollResponse>> GetId(object condition)
        {
            string[] a = (string[])condition;

            var response = await dbContext.Payrolls
                .OrderBy(x => x.PayrollId)
                .Where(x => x.PayrollId == a[0])
                .Select(x => BuildDtoHelper<PayrollResponse>.OnBuild(x, new PayrollResponse()))
                .FirstOrDefaultAsync();

            //true añade la lista de ciclos
            if (bool.Parse(a[1]))
            {
                if (response != null)
                {
                    var payCycles = await _queryHandler.GetAll(new PaginationFilter(), new SearchFilter(), response.PayrollId) ;
                    response.PayCycles = (List<PayCycle>)payCycles.Data;
                }
            }

            return new Response<PayrollResponse>(response);
        }
    }
}
