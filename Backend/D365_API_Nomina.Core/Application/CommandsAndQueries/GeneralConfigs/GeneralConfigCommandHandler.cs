using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.GeneralConfigs;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.GeneralConfigs
{
    public interface IGeneralConfigCommandHandler:
        ICreateCommandHandler<GeneralConfigRequest>
    {

    }

    public class GeneralConfigCommandHandler : IGeneralConfigCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public GeneralConfigCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<object>> Create(GeneralConfigRequest model)
        {
            var response = await _dbContext.GeneralConfigs.FirstOrDefaultAsync();
            
            if(response == null)
            {
                var entity = BuildDtoHelper<GeneralConfig>.OnBuild(model, new GeneralConfig());

                _dbContext.GeneralConfigs.Add(entity);
                await _dbContext.SaveChangesAsync();

                return new Response<object>(entity)
                {
                    Message = "Registro creado correctamente"
                };
            }
            else
            {
                var entity = BuildDtoHelper<GeneralConfig>.OnBuild(model, response);

                _dbContext.GeneralConfigs.Update(entity);
                await _dbContext.SaveChangesAsync();

                return new Response<object>(entity)
                {
                    Message = "Registro actualizado correctamente"
                };
            }
        }
    }
}
