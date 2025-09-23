using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.Positions;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Positions
{
    public interface IPositionCommandHandler :
        IDeleteCommandHandler,
        IUpdateCommandHandler<PositionRequest>
    {
        public Task<Response<object>> UpdateStatus(string id, bool status);
        public Task<Response<object>> UpdateVacants(string id, bool status);
        public Task<Response<object>> Create(PositionRequest model, bool isVacant = false);
    }

    public class PositionCommandHandler : IPositionCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public PositionCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> Create(PositionRequest model, bool isVacant)
        {
            var a = await ValidatPositionInformation(model);
            if (a != null)
                return a;

            var entity = BuildDtoHelper<Position>.OnBuild(model, new Position());
            entity.IsVacant = isVacant;
            _dbContext.Positions.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(entity)
            {
                Message = "Registro creado correctamente"
            };
        }

        private async Task<Response<object>> ValidatPositionInformation(PositionRequest model)
        {
            if (!string.IsNullOrEmpty(model.NotifyPositionId))
            {
                var notifyPosition = _dbContext.Positions.Where(x => x.PositionId == model.NotifyPositionId).FirstOrDefaultAsync();

                if (await notifyPosition == null)
                {
                    return new Response<object>(false)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { "El puesto a notificar seleccionado no existe" },
                        StatusHttp = 404
                    };
                }
            }

            var department = _dbContext.Departments.Where(x => x.DepartmentId == model.DepartmentId).FirstOrDefaultAsync();

            if (await department == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El departamento seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var job = _dbContext.Jobs.Where(x => x.JobId == model.JobId).FirstOrDefaultAsync();

            if (await job == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El cargo seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            return null;
        }

        public async Task<Response<bool>> Delete(List<string> ids)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.Positions.Where(x => x.PositionId == item).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");

                    }

                    _dbContext.Positions.Remove(response);
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


        public async Task<Response<object>> Update(string id, PositionRequest model)
        {
            var response = await _dbContext.Positions.Where(x => x.PositionId == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<Position>.OnBuild(model, response);
            _dbContext.Positions.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }


        public async Task<Response<object>> UpdateStatus(string id, bool status)
        {
            var response = await _dbContext.Positions.Where(x => x.PositionId == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Message = "El registro seleccionado no existe"
                };
            }

            response.PositionStatus = status;
            _dbContext.Positions.Update(response);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }

        public async Task<Response<object>> UpdateVacants(string id, bool isVacants)
        {
            var response = await _dbContext.Positions.Where(x => x.PositionId == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Message = "El registro seleccionado no existe"
                };
            }

            response.IsVacant = isVacants;
            _dbContext.Positions.Update(response);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }
    }

}
