using D365_API_Nomina.Core.Application.CommandsAndQueries.CompanyAssignedToUsers;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.CompanyAssignedToUsers;
using D365_API_Nomina.Core.Domain.Enums;
using D365_API_Nomina.WebUI.Attributes;
using D365_API_Nomina.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/companiestouser")]
    [Authorize]
    [ApiController]
    [AuthorizeRole(ElevationTypeRequired = AdminType.LocalAdmin)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class CompanyAssignedToUserController : ControllerBase
    {
        private readonly IQueryAllHandler<CompanyToUserResponse> _QueryHandler;
        private readonly ICompanyToUserCommandHandler _CommandHandler;

        public CompanyAssignedToUserController(IQueryAllHandler<CompanyToUserResponse> queryHandler, ICompanyToUserCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }

        [HttpGet("{alias}")]
        public async Task<ActionResult> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter, string alias)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, alias);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] List<CompanyToUserRequest> models)
        {
            var objectresult = await _CommandHandler.CreateAll(models);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpDelete("{alias}")]
        public async Task<ActionResult> Delete([FromBody] List<string> ids, string alias)
        {
            var objectresult = await _CommandHandler.DeleteByAlias(ids, alias);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
