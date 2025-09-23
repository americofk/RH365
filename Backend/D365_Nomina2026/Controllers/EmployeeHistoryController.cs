using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeHistories;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeHistories;
using D365_API_Nomina.Core.Domain.Consts;
using D365_API_Nomina.Core.Domain.Enums;
using D365_API_Nomina.WebUI.Attributes;
using D365_API_Nomina.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/employeehistories")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class EmployeeHistoryController : ControllerBase
    {
        private readonly IQueryAllHandler<EmployeeHistoryResponse> _QueryHandler;
        private readonly IEmployeeHistoryCommandHandler _CommandHandler;

        public EmployeeHistoryController(IQueryAllHandler<EmployeeHistoryResponse> queryHandler, IEmployeeHistoryCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }

        [HttpGet("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeHistory, View = true)]
        public async Task<ActionResult> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter, string employeeid)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter, searchFilter, employeeid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpDelete("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeHistory, Delete = true)]
        public async Task<ActionResult> Delete([FromBody] List<EmployeeHistoryDeleteRequest> ids, string employeeid)
        {
            var objectresult = await _CommandHandler.DeleteByParent(ids, employeeid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPut("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeHistory, Edit = true)]
        public async Task<ActionResult> Update([FromBody] EmployeeHistoryUpdateRequest model, string employeeid)
        {
            var objectresult = await _CommandHandler.Update(employeeid, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost("markisfordgt")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeHistory, Edit = true)]
        public async Task<ActionResult> MarkIsForDGT([FromBody] EmployeeHistoryIsForDGTRequest model)
        {
            var objectresult = await _CommandHandler.MarkIsForDGT(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
