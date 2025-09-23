using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using D365_API_Nomina.WebUI.Attributes;
using D365_API_Nomina.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Controllers.v2
{

    [Route("api/v2.0/educationlevels")]
    [Authorize]
    [ApiController]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class EducationLevelController : ControllerBase
    {
        private readonly IQueryAllWithoutSearchHandler<EducationLevel> _queryHandler;

        public EducationLevelController(IQueryAllWithoutSearchHandler<EducationLevel> queryHandler)
        {
            _queryHandler = queryHandler;
        }

        [HttpGet]
        public async Task<ActionResult<Response<string>>> Get([FromQuery] PaginationFilter filter)
        {
            var objectresult = await _queryHandler.GetAll(filter);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
