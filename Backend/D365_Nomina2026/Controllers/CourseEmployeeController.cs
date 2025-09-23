using D365_API_Nomina.Core.Application.CommandsAndQueries.CourseEmployees;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.CourseEmployees;
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
    [Route("api/v2.0/courseemployees")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class CourseEmployeeController : ControllerBase
    {
        private readonly IQueryHandler<CourseEmployeeResponse> _QueryHandler;
        private readonly ICourseEmployeeCommandHandler _CommandHandler;

        public CourseEmployeeController(IQueryHandler<CourseEmployeeResponse> queryHandler, ICourseEmployeeCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }

        [HttpGet("{courseid}")]
        [AuthorizePrivilege(MenuId = MenuConst.CourseEmployee, View = true)]
        public async Task<ActionResult> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter, string courseid)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, courseid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpGet("{courseid}/{employeeid}")]
        [AuthorizePrivilege(MenuId = MenuConst.CourseEmployee, View = true)]
        public async Task<ActionResult> GetById(string courseid, string employeeid)
        {
            var objectresult = await _QueryHandler.GetId(new string[]{ courseid, employeeid });
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost]
        [AuthorizePrivilege(MenuId = MenuConst.CourseEmployee, Edit = true)]
        public async Task<ActionResult> Post([FromBody] CourseEmployeeRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpDelete("{courseid}")]
        [AuthorizePrivilege(MenuId = MenuConst.CourseEmployee, Delete = true)]
        public async Task<ActionResult> Delete([FromBody] List<string> ids, string courseid)
        {
            var objectresult = await _CommandHandler.DeleteByCourseId(ids, courseid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPut("{courseid}")]
        [AuthorizePrivilege(MenuId = MenuConst.CourseInstructor, Edit = true)]
        public async Task<ActionResult> Update([FromBody] CourseEmployeeRequest model, string courseid)
        {
            var objectresult = await _CommandHandler.Update(courseid, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
