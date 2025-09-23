using D365_API_Nomina.Core.Application.CommandsAndQueries.ProjCategories;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.ProjCategories;
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
    [Route("api/v2.0/projcategories")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class ProjCategoryController : ControllerBase
    {
        private readonly IQueryHandler<ProjCategory> _QueryHandler;
        private readonly IProjCategoryCommandHandler _CommandHandler;

        public ProjCategoryController(IQueryHandler<ProjCategory> queryHandler, IProjCategoryCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }

        [HttpGet("enabled")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjCategoryEnabled, View = true)]
        public async Task<ActionResult> GetEnabled([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, true);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        //[HttpGet("projcategorys/disabled")]
        //[AuthorizePrivilege(MenuId = MenuConst.ProjCategoryDisabled, View = true)]
        //public async Task<ActionResult> GetDisabled([FromQuery] PaginationFilter paginationFilter)
        //{
        //    var objectresult = await _QueryHandler.GetAll(paginationFilter, false);
        //    return StatusCode(objectresult.StatusHttp, objectresult);

        //}

        [HttpGet("enabled/{projcategoryid}")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjCategoryEnabled, View = true)]
        public async Task<ActionResult> GetById(string projcategoryid)
        {
            var objectresult = await _QueryHandler.GetId(projcategoryid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPost("enabled")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjCategoryEnabled, Edit = true)]
        public async Task<ActionResult> Post([FromBody] ProjCategoryRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpDelete("enabled")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjCategoryEnabled, Delete = true)]
        public async Task<ActionResult> Delete([FromBody] List<string> ids)
        {
            var objectresult = await _CommandHandler.Delete(ids);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("enabled/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjCategoryEnabled, Edit = true)]
        public async Task<ActionResult> Update([FromBody] ProjCategoryRequestUpdate model, string id)
        {
            var objectresult = await _CommandHandler.Update(id, model);
            return StatusCode(objectresult.StatusHttp, objectresult);

        }


        [HttpPut("enabled/updatestatus/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjCategoryEnabled, Edit = true)]
        public async Task<ActionResult> UpdateStatus(string id)
        {
            var objectresult = await _CommandHandler.UpdateStatus(id, false);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }









        [HttpGet("disabled")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjCategoryDisabled, View = true)]
        public async Task<ActionResult> GetDisabled([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, false);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpDelete("disabled")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjCategoryDisabled, Delete = true)]
        public async Task<ActionResult> DeleteDisabled([FromBody] List<string> ids)
        {
            var objectresult = await _CommandHandler.Delete(ids);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPut("disabled/updatestatus/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.ProjCategoryDisabled, Edit = true)]
        public async Task<ActionResult> UpdateStatusDisabled(string id)
        {
            var objectresult = await _CommandHandler.UpdateStatus(id, true);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }

}
