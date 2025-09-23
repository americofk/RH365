using D365_API_Nomina.Core.Application.CommandsAndQueries.MenuAssignedToUsers;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.MenuAssignedToUsers;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using D365_API_Nomina.WebUI.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/menustouser")]
    [Authorize]
    [ApiController]
    [AuthorizeRole(ElevationTypeRequired = AdminType.LocalAdmin)]
    public class MenuAssignedToUserController : ControllerBase
    {
        private readonly IQueryAllHandler<MenuToUserResponse> _QueryHandler;
        private readonly IMenuToUserCommandHandler _CommandHandler;

        public MenuAssignedToUserController(IQueryAllHandler<MenuToUserResponse> queryHandler, IMenuToUserCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }

        [HttpGet("{alias}")]
        public async Task<ActionResult> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter, string alias)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter, searchFilter, alias);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] List<MenuToUserRequest> models)
        {
            var objectresult = await _CommandHandler.CreateAll(models);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPut("{alias}")]
        public async Task<ActionResult> Put(string  alias, [FromBody] MenuToUserRequest model)
        {
            var objectresult = await _CommandHandler.Update(alias, model);
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