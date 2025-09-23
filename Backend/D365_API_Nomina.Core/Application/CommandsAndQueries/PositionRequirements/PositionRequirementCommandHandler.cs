using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.PositionRequeriments;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.PositionRequirements
{
    public interface IPositionRequirementCommandHandler :
        ICreateCommandHandler<PositionRequirementRequest>,
        IUpdateCommandHandler<PositionRequirementRequest>
    {
        public Task<Response<bool>> DeleteByPositionId(List<string> ids, string positionid);

    }

    public class PositionRequirementCommandHandler : IPositionRequirementCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public PositionRequirementCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> Create(PositionRequirementRequest model)
        {
            var position = _dbContext.Positions.Where(x => x.PositionId == model.PositionId).FirstOrDefaultAsync();
            if (await position == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { $"La posición asignada no existe - Id {model.PositionId}" },
                    StatusHttp = 404
                };
            }

            var requeriment = _dbContext.PositionRequirements.Where(x => x.PositionId == model.PositionId && x.Name == model.Name.ToUpper()).FirstOrDefaultAsync();
            if (await requeriment != null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { $"El nombre asignado ya existe - Id {model.Name}" },
                    StatusHttp = 404
                };
            }

            model.Name = model.Name.ToUpper();
            var entity = BuildDtoHelper<PositionRequirement>.OnBuild(model, new PositionRequirement());

            _dbContext.PositionRequirements.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(entity)
            {
                Message = "Registro creado correctamente"
            };
        }


        public async Task<Response<bool>> DeleteByPositionId(List<string> ids, string positionid)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.PositionRequirements.Where(x => x.PositionId == positionid &&
                                                                               x.Name == item.ToUpper()).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");

                    }

                    _dbContext.PositionRequirements.Remove(response);
                    await _dbContext.SaveChangesAsync();
                }
                transaction.Commit();
                return new Response<bool>(true) { Message = "Registros elimandos con éxito" };
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return new Response<bool>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { ex.Message }
                };
            }
        }


        public async Task<Response<object>> Update(string id, PositionRequirementRequest model)
        {
            var response = await _dbContext.PositionRequirements.Where(x => x.Name == model.Name.ToUpper() && x.PositionId == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<PositionRequirement>.OnBuild(model, response);
            _dbContext.PositionRequirements.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }
    }

}
