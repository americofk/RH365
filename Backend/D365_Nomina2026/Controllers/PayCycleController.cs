using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.PayCycles;
using D365_API_Nomina.Core.Application.StoreServices.PayCycles;
using D365_API_Nomina.Core.Domain.Consts;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using D365_API_Nomina.WebUI.Attributes;
using D365_API_Nomina.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/paycycle")]
    [Authorize]
    [ApiController]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class PayCycleController : ControllerBase
    {
        private readonly IPayCycleCommandHandler _commandHandler;
        private readonly IQueryHandler<PayCycleResponse> _queryHandler;

        public PayCycleController(IPayCycleCommandHandler commandHandler, IQueryHandler<PayCycleResponse> queryHandler)
        {
            _commandHandler = commandHandler;
            _queryHandler = queryHandler;
        }

        [HttpGet("{payrollid}")]
        [AuthorizePrivilege(MenuId = MenuConst.PayCycle, View = true)]
        public async Task<ActionResult<Response<string>>> Get([FromQuery] PaginationFilter filter, [FromQuery] SearchFilter searchFilter, string payrollid)
        {
            var objectresult = await _queryHandler.GetAll(filter,searchFilter, payrollid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost]
        [AuthorizePrivilege(MenuId = MenuConst.PayCycle, Edit = true)]
        public async Task<ActionResult<Response<string>>> Post([FromQuery] PayCycleRequest createPayCycleCommand)
        {
            var objectresult= await _commandHandler.Create(createPayCycleCommand);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpDelete("{payrollid}")]
        [AuthorizePrivilege(MenuId = MenuConst.PayCycle, Delete = true)]
        public async Task<ActionResult> Delete([FromBody] List<string> ids, string payrollid)
        {
            var objectresult = await _commandHandler.DeleteByParent(ids, payrollid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost("markisfortax")]
        [AuthorizePrivilege(MenuId = MenuConst.PayCycle, Edit = true)]
        public async Task<ActionResult> MarkIsForTax([FromBody] PayCycleIsForTaxRequest model)
        {
            var objectresult = await _commandHandler.MarkIsForTax(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpPost("markisfortss")]
        [AuthorizePrivilege(MenuId = MenuConst.PayCycle, Edit = true)]
        public async Task<ActionResult> MarkIsForTss([FromBody] PayCycleIsForTssRequest model)
        {
            var objectresult = await _commandHandler.MarkIsForTss(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
