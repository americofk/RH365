using D365_API_Nomina.Core.Application.CommandsAndQueries.CourseTypes;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.CourseTypes;
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


namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/coursetypes")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class CourseTypeController : ControllerBase
    {
        private readonly IQueryHandler<CourseType> _QueryHandler;
        private readonly ICourseTypeCommandHandler _CommandHandler;

        public CourseTypeController(IQueryHandler<CourseType> queryHandler, ICourseTypeCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }

        [HttpGet]
        [AuthorizePrivilege(MenuId = MenuConst.CourseType, View = true)]
        public async Task<ActionResult<PagedResponse<CourseType>>> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, true);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpGet("{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.CourseType, View = true)]
        public async Task<ActionResult<PagedResponse<CourseType>>> GetById(string id)
        {
            var objectresult = await _QueryHandler.GetId(id);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost]
        [AuthorizePrivilege(MenuId = MenuConst.CourseType, Edit = true)]
        public async Task<ActionResult<Response<CourseType>>> Post([FromBody] CourseTypeRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpDelete]
        [AuthorizePrivilege(MenuId = MenuConst.CourseType, Delete = true)]
        public async Task<ActionResult<Response<bool>>> Delete([FromBody] List<string> ids)
        {
            var objectresult = await _CommandHandler.Delete(ids);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.CourseType, Edit = true)]
        public async Task<ActionResult<Response<bool>>> Update([FromBody] CourseTypeRequest model, string id)
        {
            var objectresult = await _CommandHandler.Update(id, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
