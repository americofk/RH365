using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeWorkControlCalendars;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeWorkControlCalendars;
using D365_API_Nomina.Core.Domain.Consts;
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
    [Route("api/v2.0/employeeworkcontrolcalendars")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class EmployeeWorkControlCalendarController : ControllerBase
    {
        private readonly IEmployeeWorkControlCalendarCommandHandler _CommandHandler;
        private readonly IQueryHandler<EmployeeWorkControlCalendarResponse> _QueryHandler;

        public EmployeeWorkControlCalendarController(IEmployeeWorkControlCalendarCommandHandler commandHandler, IQueryHandler<EmployeeWorkControlCalendarResponse> queryHandler)
        {
            _CommandHandler = commandHandler;
            _QueryHandler = queryHandler;
        }


        [HttpGet("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeWorkCalendar, View = true)]
        public async Task<ActionResult> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter, string employeeid)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter, searchFilter, employeeid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpGet]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeWorkCalendar, View = true)]
        public async Task<ActionResult> GetById([FromQuery] string employeeid, [FromQuery] int workedday)
        {
            var objectresult = await _QueryHandler.GetId(new string[] { employeeid, workedday.ToString() });
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPost]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeWorkCalendar, Edit = true)]
        public async Task<ActionResult> Post([FromBody] EmployeeWorkControlCalendarRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpDelete("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeWorkCalendar, Delete = true)]
        public async Task<ActionResult> Delete([FromBody] List<EmployeeWorkControlCalendarDeleteRequest> ids, string employeeid)
        {
            var objectresult = await _CommandHandler.DeleteByParent(ids, employeeid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EmployeeWorkCalendar, Edit = true)]
        public async Task<ActionResult> Update([FromBody] EmployeeWorkControlCalendarRequest model, string employeeid)
        {
            var objectresult = await _CommandHandler.Update(employeeid, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

    }
}
