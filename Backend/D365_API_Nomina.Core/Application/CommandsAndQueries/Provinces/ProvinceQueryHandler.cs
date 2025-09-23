using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.Provinces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Provinces
{
    public class ProvinceQueryHandler: IQueryAllHandler<ProvinceResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public ProvinceQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<ProvinceResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            List<ProvinceResponse> response = await _dbContext.Provinces
                                     .OrderBy(x => x.ProvinceId)
                                     .Select(x => BuildDtoHelper<ProvinceResponse>.OnBuild(x, new ProvinceResponse()))
                                     .ToListAsync();

            return new PagedResponse<IEnumerable<ProvinceResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
