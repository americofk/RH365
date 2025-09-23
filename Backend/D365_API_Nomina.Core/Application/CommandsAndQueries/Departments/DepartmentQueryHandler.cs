using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Departments
{
    public class DepartmentQueryHandler : IQueryHandler<Department>
    {
        private readonly IApplicationDbContext _dbContext;

        public DepartmentQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }


        public async Task<PagedResponse<IEnumerable<Department>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryFilter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.Departments
                                .OrderBy(x => x.DepartmentId)
                                .Where(x => x.DepartamentStatus == (bool)queryFilter)
                                .AsQueryable();

            SearchFilter<Department> validSearch = new SearchFilter<Department>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<Department>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<Department>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<Department>> GetId(object condition)
        {
            var response = await _dbContext.Departments
                .Where(x => x.DepartmentId == (string)condition)
                .FirstOrDefaultAsync();

            return new Response<Department>(response);
        }


        //public async Task<Response<Department>> GetById(object id)
        //{
        //    var response = await _dbContext.Departments.Where(x => x.DepartmentId == (string)id).FirstOrDefaultAsync();

        //    if (response == null)
        //    {
        //        return new Response<Department>()
        //        {
        //            Succeeded = false,
        //            Message = "El registro seleccionado no existe"
        //        };
        //    }
        //    return new Response<Department>(response);
        //}
    }
}
