using D365_API_Nomina.Core.Application.CommandsAndQueries.GeneralConfigs;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.GeneralConfigs;
using D365_API_Nomina.Core.Domain.Consts;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.WebUI.Attributes;
using D365_API_Nomina.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/generalconfigs")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class GeneralConfigController : ControllerBase
    {
        private readonly IQueryByIdHandler<GeneralConfigResponse> _QueryHandler;
        private readonly IGeneralConfigCommandHandler _commandHandler;

        public GeneralConfigController(IQueryByIdHandler<GeneralConfigResponse> queryHandler, IGeneralConfigCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _commandHandler = commandHandler;
        }

        [HttpGet]
        [AuthorizePrivilege(MenuId = MenuConst.GeneralConfig, View = true)]
        public async Task<ActionResult> Get()
        {
            var objectresult = await _QueryHandler.GetId("");
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneralConfigRequest _model)
        {
            var objectresult = await _commandHandler.Create(_model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
