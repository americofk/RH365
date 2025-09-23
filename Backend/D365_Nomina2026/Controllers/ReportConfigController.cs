using D365_API_Nomina.Core.Application.CommandsAndQueries.Reports;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.Reports;
using D365_API_Nomina.Core.Domain.Consts;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using D365_API_Nomina.WebUI.Attributes;
using D365_API_Nomina.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/reportconfig")]
    [Authorize]
    [ApiController]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class ReportConfigController : ControllerBase
    {
        private readonly IQueryAllHandler<ReportConfig> _QueryHandler;
        private readonly ICreateCommandHandler<ReportConfigRequest> _CommandHandler;

        public ReportConfigController(IQueryAllHandler<ReportConfig> QueryHandler, ICreateCommandHandler<ReportConfigRequest> CommandHandler)
        {
            _QueryHandler = QueryHandler;
            _CommandHandler = CommandHandler;
        }

        [HttpGet]
        [AuthorizePrivilege(MenuId = MenuConst.ReportConfig, View = true)]
        public async Task<ActionResult<PagedResponse<ReportConfig>>> Get()
        {
            var objectresult = await _QueryHandler.GetAll(null, null);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost]
        [AuthorizePrivilege(MenuId = MenuConst.ReportConfig, Edit = true)]
        public async Task<ActionResult<Response<Payroll>>> Post([FromBody] ReportConfigRequest _model)
        {
            var objectresult = await _CommandHandler.Create(_model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
