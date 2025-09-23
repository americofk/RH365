using D365_API_Nomina.Core.Application.CommandsAndQueries.Companies;
using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Model.Companies;
using D365_API_Nomina.Core.Domain.Enums;
using D365_API_Nomina.WebUI.Attributes;
using D365_API_Nomina.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace D365_API_Nomina.WebUI.Controllers.v2
{
    [Route("api/v2.0/config/companies")]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class CompanyByConfigController : ControllerBase
    {
        private readonly ICompanyQueryHandler _QueryHandler;
        private readonly ICompanyCommandHandler _CommandHandler;

        public CompanyByConfigController(ICompanyQueryHandler queryHandler, ICompanyCommandHandler commandHandler)
        {
            _QueryHandler = queryHandler;
            _CommandHandler = commandHandler;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PaginationFilter paginationFilter)
        {
            var objectresult = await _QueryHandler.GetAll(paginationFilter);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
        
        [HttpPost]
        public async Task<ActionResult> Post(CompanyRequest model)
        {
            var objectresult = await _CommandHandler.Create(model);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }


        [HttpPut("{companyid}")]
        public async Task<ActionResult> Update(string companyid, [FromQuery] bool status)
        {
            var objectresult = await _CommandHandler.UpdateStatus(companyid, status);
            return StatusCode(objectresult.StatusHttp, objectresult);
        }
    }
}
