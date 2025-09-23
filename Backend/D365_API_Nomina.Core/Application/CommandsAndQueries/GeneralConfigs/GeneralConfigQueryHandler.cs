using D365_API_Nomina.Core.Application.Common.Filter;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.GeneralConfigs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.GeneralConfigs
{
    public class GeneralConfigQueryHandler : IQueryByIdHandler<GeneralConfigResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public GeneralConfigQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<GeneralConfigResponse>> GetId(string id)
        {
            var response = await _dbContext.GeneralConfigs
                .Select(x => new GeneralConfigResponse(){
                    SMTP = x.SMTP,
                    SMTPPort = x.SMTPPort,
                    IsPassword = string.IsNullOrEmpty(x.EmailPassword) ? false : true,
                    Email = x.Email
                })
                .FirstOrDefaultAsync();

            return new Response<GeneralConfigResponse>(response);
        }
    }
}
