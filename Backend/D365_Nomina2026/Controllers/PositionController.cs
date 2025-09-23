using D365_API_Nomina.Core.Application.CommandsAndQueries.Positions;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.Positions;
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
    [Route("api/v2.0/positions")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class PositionController : ControllerBase
    {
        private readonly IQueryHandler<Position> _QueryHandler;
        private readonly IPositionCommandHandler _CommandHandler;

        public PositionController(IQueryHandler<Position> queryHandler, IPositionCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }


        [HttpGet("enabled")]
        [AuthorizePrivilege(MenuId = MenuConst.PositionEnabled, View = true)]
        public async Task<ActionResult> GetEnabled([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, new bool[] { true, false });
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpGet("enabled/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.PositionEnabled, View = true)]
        public async Task<ActionResult> GetById(string id)
        {
            var objectresult = await _QueryHandler.GetId(id);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost("enabled")]
        [AuthorizePrivilege(MenuId = MenuConst.PositionEnabled, Edit = true)]
        public async Task<ActionResult> Post([FromBody] PositionRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpDelete("enabled")]
        [AuthorizePrivilege(MenuId = MenuConst.PositionEnabled, Delete = true)]
        public async Task<ActionResult> DeleteEnabled([FromBody] List<string> ids)
        {
            var objectresult = await _CommandHandler.Delete(ids);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPut("enabled/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.PositionEnabled, Edit = true)]
        public async Task<ActionResult> Update([FromBody] PositionRequest model, string id)
        {
            var objectresult = await _CommandHandler.Update(id, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPut("enabled/updatestatus/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.PositionEnabled, Edit = true)]
        public async Task<ActionResult> UpdateStatusEnabled(string id)
        {
            var objectresult = await _CommandHandler.UpdateStatus(id, false);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }



        [HttpGet("disabled")]
        [AuthorizePrivilege(MenuId = MenuConst.PositionDisabled, View = true)]
        public async Task<ActionResult> GetDisabled([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, new bool[] { false, false });
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpDelete("disabled")]
        [AuthorizePrivilege(MenuId = MenuConst.PositionDisabled, Delete = true)]
        public async Task<ActionResult> DeleteDisabled([FromBody] List<string> ids)
        {
            var objectresult = await _CommandHandler.Delete(ids);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPut("disabled/updatestatus/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.PositionDisabled, Edit = true)]
        public async Task<ActionResult> UpdateStatusDisabled(string id)
        {
            var objectresult = await _CommandHandler.UpdateStatus(id, true);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }





        #region Vacants
        [HttpGet("vacants")]
        [AuthorizePrivilege(MenuId = MenuConst.PositionVacant, View = true)]
        public async Task<ActionResult> GetVacants([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter,searchFilter, new bool[] { true, true });
            return StatusCode(objectresult.StatusHttp, objectresult);

        }

        [HttpPut("vacants/updatetovacants/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.PositionVacant, Edit = true)]
        public async Task<ActionResult> UpdateToVacant(string id, bool isVacants)
        {
            var objectresult = await _CommandHandler.UpdateVacants(id, isVacants);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpDelete("vacants")]
        [AuthorizePrivilege(MenuId = MenuConst.PositionVacant, Delete = true)]
        public async Task<ActionResult> DeleteVacants([FromBody] List<string> ids)
        {
            var objectresult = await _CommandHandler.Delete(ids);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost("vacants")]
        [AuthorizePrivilege(MenuId = MenuConst.PositionVacant, Edit = true)]
        public async Task<ActionResult> PostVacants([FromBody] PositionRequest model)
        {
            var objectresult = await _CommandHandler.Create(model, true);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPut("vacants/{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.PositionVacant, Edit = true)]
        public async Task<ActionResult> UpdateVacant([FromBody] PositionRequest model, string id)
        {
            var objectresult = await _CommandHandler.Update(id, model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        #endregion

    }

}
