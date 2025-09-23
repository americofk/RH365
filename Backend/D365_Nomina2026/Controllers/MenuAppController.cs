using D365_API_Nomina.Core.Application.CommandsAndQueries.MenusApp;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Domain.Enums;
using D365_API_Nomina.WebUI.Attributes;
using D365_API_Nomina.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0")]
    [Authorize]
    [ApiController]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class MenuAppController : ControllerBase
    {
        private readonly IMenuAppQueryHandler _MenuAppQueryHandler;

        public MenuAppController(IMenuAppQueryHandler menuAppQueryHandler)
        {
            _MenuAppQueryHandler = menuAppQueryHandler;
        }

        [HttpGet("menusapp")]
        public async Task<ActionResult> Get([FromQuery] PaginationFilter paginationFilter)
        {
            return Ok(await _MenuAppQueryHandler.GetAll(paginationFilter));
        }

        [HttpGet("roles")]
        public async Task<ActionResult> GetRoles()
        {
            return Ok(await _MenuAppQueryHandler.GetRoles());
        }
    }
}
