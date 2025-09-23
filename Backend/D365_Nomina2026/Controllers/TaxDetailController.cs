using D365_API_Nomina.Core.Application.CommandsAndQueries.TaxDetails;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model.TaxDetails;
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
    [Route("api/v2.0/taxdetails")]
    [ApiController]
    [Authorize]
    [AuthorizeRole(ElevationTypeRequired = AdminType.User)]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class TaxDetailController : ControllerBase
    {
        private readonly IQueryHandler<TaxDetail> _QueryHandler;
        private readonly ITaxDetailCommandHandler _CommandHandler;

        public TaxDetailController(IQueryHandler<TaxDetail> queryHandler, ITaxDetailCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }

        [HttpGet("{id}")]
        [AuthorizePrivilege(MenuId = MenuConst.TaxEnabled, View = true)]
        public async Task<ActionResult> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SearchFilter searchFilter, string id)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter, searchFilter, id);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpGet("{taxid}/{internalid}")]
        [AuthorizePrivilege(MenuId = MenuConst.TaxEnabled, View = true)]
        public async Task<ActionResult> GetById(string taxid, int internalid)
        {
            var objectresult = await _QueryHandler.GetId(new string[]{taxid, internalid.ToString()});
            return StatusCode(objectresult.StatusHttp, objectresult);
        }

        [HttpPost]
        [AuthorizePrivilege(MenuId = MenuConst.TaxEnabled, Edit = true)]
        public async Task<ActionResult> Post([FromBody] TaxDetailRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpDelete("{taxid}")]
        [AuthorizePrivilege(MenuId = MenuConst.TaxEnabled, Delete = true)]
        public async Task<ActionResult> Delete([FromBody] List<string> ids, string taxid)
        {
            var objectresult = await _CommandHandler.DeleteByParent(ids, taxid);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("{internalid}")]
        [AuthorizePrivilege(MenuId = MenuConst.TaxEnabled, Edit = true)]
        public async Task<ActionResult> Update([FromBody] TaxDetailRequest model, int internalid)
        {
            var objectresult = await _CommandHandler.Update(internalid.ToString(), model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }

}
