using D365_API_Nomina.Core.Application.CommandsAndQueries.CalendarHolidays;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.CalendarHolidays;
using D365_API_Nomina.Core.Domain.Consts;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using D365_API_Nomina.WebUI.Attributes;
using D365_API_Nomina.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/calendarholidays")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class CalendarHolidayController : ControllerBase
    {
        private readonly IQueryAllHandler<CalendarHoliday> _QueryHandler;
        private readonly ICalendarHolidayCommandHandler _CommandHandler;

        public CalendarHolidayController(IQueryAllHandler<CalendarHoliday> queryHandler, ICalendarHolidayCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }

        [HttpGet]
        [AuthorizePrivilege(MenuId = MenuConst.CalendarHoliday, View = true)]
        public async Task<ActionResult> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter, searchFilter, true);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost]
        [AuthorizePrivilege(MenuId = MenuConst.CalendarHoliday, Edit = true)]
        public async Task<ActionResult> Post([FromBody] CalendarHolidayRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpDelete]
        [AuthorizePrivilege(MenuId = MenuConst.CalendarHoliday, Delete = true)]
        public async Task<ActionResult> Delete([FromBody] List<CalendarHolidayDeleteRequest> ids)
        {
            var objectresult = await _CommandHandler.DeleteByParent(ids,"");
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPut("{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.CalendarHoliday, Edit = true)]
        public async Task<ActionResult> Update([FromBody] CalendarHolidayRequest model)
        {
            var objectresult = await _CommandHandler.Update("", model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
