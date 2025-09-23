using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeExtraHours;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeExtraHours;
using D365_API_Nomina.Core.Domain.Consts;
using D365_API_Nomina.Core.Domain.Enums;
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
    [Route("api/v2.0/employeeextrahours")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class EmployeeExtraHourController : ControllerBase
    {
        private readonly IQueryHandler<EmployeeExtraHourResponse> _QueryHandler;
        private readonly IEmployeeExtraHourCommandHandler _CommandHandler;

        public EmployeeExtraHourController(IQueryHandler<EmployeeExtraHourResponse> queryHandler, IEmployeeExtraHourCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }

        [HttpGet("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeExtraHour, View = true)]
        public async Task<ActionResult> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter, string employeeid)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, employeeid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpGet("{employeeid}/{earningcodeid}/{workedday}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeExtraHour, View = true)]
        public async Task<ActionResult> GetById(string employeeid, DateTime workedday, string earningcodeid)
        {
            var objectresult = await _QueryHandler.GetId(new string[] {employeeid, workedday.ToString(), earningcodeid });
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeExtraHour, Edit = true)]
        public async Task<ActionResult> Post([FromBody] EmployeeExtraHourRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpDelete("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeExtraHour, Delete = true)]
        public async Task<ActionResult> Delete([FromBody] List<EmployeeExtraHourRequestDelete> ids, string employeeid)
        {
            var objectresult = await _CommandHandler.DeleteByParent(ids, employeeid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeExtraHour, Edit = true)]
        public async Task<ActionResult> Update([FromBody] EmployeeExtraHourRequestUpdate model, string employeeid)
        {
            var objectresult = await _CommandHandler.Update(employeeid, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
