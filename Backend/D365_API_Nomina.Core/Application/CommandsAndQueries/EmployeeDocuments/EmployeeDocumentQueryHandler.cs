using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeDocuments;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeDocuments
{
    public class EmployeeDocumentQueryHandler : IQueryHandler<EmployeeDocumentResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeDocumentQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<PagedResponse<IEnumerable<EmployeeDocumentResponse>>> GetAll(PaginationFilter filter, SearchFilter searchFilter, object queryfilter = null)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var tempResponse = _dbContext.EmployeeDocuments
                .OrderBy(x => x.InternalId)
                .Where(x => x.EmployeeId == (string)queryfilter)
                .AsQueryable();

            SearchFilter<EmployeeDocument> validSearch = new SearchFilter<EmployeeDocument>(searchFilter.PropertyName, searchFilter.PropertyValue);
            if (validSearch.IsValid())
            {
                var lambda = GenericSearchHelper<EmployeeDocument>.GetLambdaExpession(validSearch);

                tempResponse = tempResponse.Where(lambda)
                                           .AsQueryable();
            }

            var response = await tempResponse
                            .Select(x => SetObjectResponse(x))
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToListAsync();

            return new PagedResponse<IEnumerable<EmployeeDocumentResponse>>(response, validFilter.PageNumber, validFilter.PageSize);
        }

        private static EmployeeDocumentResponse SetObjectResponse(EmployeeDocument employeedocuments)
        {
            var a = BuildDtoHelper<EmployeeDocumentResponse>.OnBuild(employeedocuments, new EmployeeDocumentResponse());
            a.HasAttach = employeedocuments.FileAttach == null ? false : true;
            return a;
        }

        public async Task<Response<EmployeeDocumentResponse>> GetId(object condition)
        {
            string[] a = (string[])condition;

            var response = await _dbContext.EmployeeDocuments
                .Where(x => x.EmployeeId == a[0] && x.InternalId == int.Parse(a[1]))
                .Select(x => SetObjectResponse(x))
                .FirstOrDefaultAsync();

            return new Response<EmployeeDocumentResponse>(response);
        }
    }
}
