using D365_API_Nomina.Core.Application.CommandsAndQueries.CourseInstructors;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.CourseInstructors;
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
    [Route("api/v2.0/courseinstructors")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class CourseInstructorController : ControllerBase
    {
        private readonly IQueryHandler<CourseInstructorResponse> _QueryHandler;
        private readonly ICourseInstructorCommandHandler _CommandHandler;

        public CourseInstructorController(IQueryHandler<CourseInstructorResponse> queryHandler, ICourseInstructorCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;

        }

        [HttpGet("{courseid}")]
        [AuthorizePrivilege(MenuId = MenuConst.CourseInstructor, View = true)]
        public async Task<ActionResult> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter, string courseid)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, courseid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpGet("{courseid}/{instructorid}")]
        [AuthorizePrivilege(MenuId = MenuConst.CourseEmployee, View = true)]
        public async Task<ActionResult> GetById(string courseid, string instructorid)
        {
            var objectresult = await _QueryHandler.GetId(new string[] { courseid, instructorid });
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost]
        [AuthorizePrivilege(MenuId = MenuConst.CourseInstructor, Edit = true)]
        public async Task<ActionResult> Post([FromBody] CourseInstructorRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpDelete("{courseid}")]
        [AuthorizePrivilege(MenuId = MenuConst.CourseInstructor, Delete = true)]
        public async Task<ActionResult> Delete([FromBody] List<string> ids, string courseid)
        {
            var objectresult = await _CommandHandler.DeleteByCourseId(ids, courseid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPut("{courseid}")]
        [AuthorizePrivilege(MenuId = MenuConst.CourseInstructor, Edit = true)]
        public async Task<ActionResult> Update([FromBody] CourseInstructorRequest model, string courseid)
        {
            var objectresult = await _CommandHandler.Update(courseid, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }

}
