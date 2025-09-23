using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.StoreServices.EarningCodes;
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
    [Route("api/v2.0/earningcodes")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class EarningCodeController : ControllerBase
    {
        private readonly IEarningCodeQueryHandler _queryHandler;
        private IEarningCodeCommandHandler _commandHandler;
        private IQueryAllHandler<EarningCodeVersion> _queryVersionHandler;

        public EarningCodeController(IEarningCodeQueryHandler queryHandler, IEarningCodeCommandHandler commandHandler, IQueryAllHandler<EarningCodeVersion> queryVersionHandler)
        {
            _queryHandler = queryHandler;
            _commandHandler = commandHandler;
            _queryVersionHandler = queryVersionHandler;
        }

        [HttpGet("enabled")]
        [AuthorizePrivilege(MenuId = MenuConst.EarningCodeEnabled, View = true)]
        public async Task<ActionResult> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter, [FromQuery] bool versions = false, [FromQuery] string id = "")
        {
            if (!versions)
            {
                var objectresult = await _queryHandler.GetAll(paginationFilter,searchFilter, true);
                return StatusCode(objectresult.StatusHttp, objectresult);
            }
            else
            {
                var objectresult = await _queryVersionHandler.GetAll(paginationFilter,searchFilter, id); 
                return StatusCode(objectresult.StatusHttp, objectresult);
            }
        }
        
        [HttpGet("enabled/hours")]
        [AuthorizePrivilege(MenuId = MenuConst.EarningCodeEnabled, View = true)]
        public async Task<ActionResult> GetHours([FromQuery] PaginationFilter paginationFilter)
        {
            var objectresult = await _queryHandler.GetAllHours(paginationFilter, true);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpGet("enabled/earnings")]
        [AuthorizePrivilege(MenuId = MenuConst.EarningCodeEnabled, View = true)]
        public async Task<ActionResult> GetEarnings([FromQuery] PaginationFilter paginationFilter)
        {
            var objectresult = await _queryHandler.GetAllEarnings(paginationFilter, true);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpGet("enabled/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.EarningCodeEnabled, View = true)]
        public async Task<ActionResult> GetById(string id)
        {
            var objectresult = await _queryHandler.GetId(id);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost("enabled")]
        public async Task<ActionResult> Post([FromBody] EarningCodeRequest _model)
        {
            var objectresult = await _commandHandler.Create(_model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPut("enabled/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.EarningCodeEnabled, Edit = true)]
        public async Task<ActionResult> Update([FromBody] EarningCodeRequest model, string id, [FromQuery] bool isVersion = false)
        {
            var objectresult = await _commandHandler.Update(id, model, isVersion);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpDelete("enabled")]
        [AuthorizePrivilege(MenuId = MenuConst.EarningCodeEnabled, Delete = true)]
        public async Task<ActionResult> Delete([FromBody] List<string> ids)
        {
            var objectresult = await _commandHandler.Delete(ids);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPut("enabled/updatestatus/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.LoanEnabled, Edit = true)]
        public async Task<ActionResult> UpdateStatusEnabled(string id)
        {
            var objectresult = await _commandHandler.UpdateStatus(id, false);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        //Eliminar versiones
        [HttpDelete("version/{id}/{internalid}")]
        [AuthorizePrivilege(MenuId = MenuConst.EarningCodeEnabled, Delete = true)]
        public async Task<ActionResult> Delete(string id, int internalid)
        {
            var objectresult = await _commandHandler.DeleteVersion(id, internalid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }





        [HttpGet("disabled")]
        [AuthorizePrivilege(MenuId = MenuConst.EarningCodeEnabled, View = true)]
        public async Task<ActionResult> GetDisabled([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter, [FromQuery] bool versions = false, [FromQuery] string id = "")
        {
            if (!versions)
            {
                var objectresult = await _queryHandler.GetAll(paginationFilter,searchFilter, false);
                return StatusCode(objectresult.StatusHttp, objectresult);
            }
            else
            {
                var objectresult = await _queryVersionHandler.GetAll(paginationFilter,searchFilter, id);
                return StatusCode(objectresult.StatusHttp, objectresult);
            }
        }

        //[HttpGet("enabled/{id}")]
        //[AuthorizePrivilege(MenuId = MenuConst.EarningCodeEnabled, View = true)]
        //public async Task<ActionResult> GetById(string id)
        //{
        //    var objectresult = await _queryHandler.GetId(id);
        //    return StatusCode(objectresult.StatusHttp, objectresult);
        //}

        [HttpDelete("disabled")]
        [AuthorizePrivilege(MenuId = MenuConst.EarningCodeEnabled, Delete = true)]
        public async Task<ActionResult> DeleteDisabled([FromBody] List<string> ids)
        {
            var objectresult = await _commandHandler.Delete(ids);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPut("disabled/updatestatus/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.LoanEnabled, Edit = true)]
        public async Task<ActionResult> UpdateStatusDisabled(string id)
        {
            var objectresult = await _commandHandler.UpdateStatus(id, true);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
