using D365_API_Nomina.Core.Application.CommandsAndQueries.CoursePositions;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.CoursePositons;
using D365_API_Nomina.Core.Domain.Consts;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using D365_API_Nomina.WebUI.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/coursepositions")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    public class CoursePositionController : ControllerBase
    {
        private readonly IQueryHandler<CoursePositionResponse> _QueryHandler;
        private readonly ICoursePositionCommandHandler _CommandHandler;

        public CoursePositionController(IQueryHandler<CoursePositionResponse> queryHandler, ICoursePositionCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }

        [HttpGet("{courseid}")]
        [AuthorizePrivilege(MenuId = MenuConst.CoursePosition, View = true)]
        public async Task<ActionResult> Get([FromQuery] PaginationFilter paginationFilter, string courseid, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter, searchFilter, courseid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpGet("{courseid}/{positionid}")]
        [AuthorizePrivilege(MenuId = MenuConst.CoursePosition, View = true)]
        public async Task<ActionResult> GetById(string courseid, string positionid)
        {
            var objectresult = await _QueryHandler.GetId(new string[] { courseid, positionid });
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPost]
        [AuthorizePrivilege(MenuId = MenuConst.CoursePosition, Edit = true)]
        public async Task<ActionResult> Post([FromBody] CoursePositionRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpDelete("{courseid}")]
        [AuthorizePrivilege(MenuId = MenuConst.CoursePosition, Delete = true)]
        public async Task<ActionResult> Delete([FromBody] List<string> ids, string courseid)
        {
            var objectresult = await _CommandHandler.DeleteByCourseId(ids, courseid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("{courseid}")]
        [AuthorizePrivilege(MenuId = MenuConst.CoursePosition, Edit = true)]
        public async Task<ActionResult> Update([FromBody] CoursePositionRequest model, string courseid)
        {
            var objectresult = await _CommandHandler.Update(courseid, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
