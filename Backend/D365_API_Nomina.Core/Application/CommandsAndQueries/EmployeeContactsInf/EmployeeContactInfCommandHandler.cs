using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeContactsInf;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeContactsInf
{
    public interface IEmployeeContactInfCommandHandler :
        ICreateCommandHandler<EmployeeContactInfRequest>,
        IUpdateCommandHandler<EmployeeContactInfRequest>
    {
        public Task<Response<bool>> DeleteByEmployeeId(List<string> ids, string employeeid);
    }

    public class EmployeeContactInfCommandHandler : IEmployeeContactInfCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeContactInfCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> Create(EmployeeContactInfRequest model)
        {
            EmployeeContactInf principalEntity = null;


            var contactinf = await _dbContext.EmployeeContactsInf.Where(x => x.EmployeeId == model.EmployeeId)
                .OrderByDescending(x => x.InternalId).FirstOrDefaultAsync();

            var entity = BuildDtoHelper<EmployeeContactInf>.OnBuild(model, new EmployeeContactInf());

            entity.InternalId = contactinf == null ? 0 : contactinf.InternalId + 1;

            if (model.IsPrincipal)
            {
                principalEntity = await _dbContext.EmployeeContactsInf.Where(x => x.EmployeeId == model.EmployeeId
                                                                           && x.IsPrincipal == true
                                                                           && x.ContactType == model.ContactType).FirstOrDefaultAsync();
            }

            //Guarda el nuevo
            _dbContext.EmployeeContactsInf.Add(entity);
            await _dbContext.SaveChangesAsync();

            //Actualizo la entidad que era principal
            if (principalEntity != null)
            {
                principalEntity.IsPrincipal = false;
                _dbContext.EmployeeContactsInf.Update(principalEntity);
                await _dbContext.SaveChangesAsync();
            }

            return new Response<object>(entity)
            {
                Message = "Registro creado correctamente"
            };
        }


        public async Task<Response<bool>> DeleteByEmployeeId(List<string> ids, string employeeid)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.EmployeeContactsInf.Where(x => x.EmployeeId == employeeid && x.InternalId == int.Parse(item)).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");

                    }

                    _dbContext.EmployeeContactsInf.Remove(response);
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


        public async Task<Response<object>> Update(string id, EmployeeContactInfRequest model)
        {
            EmployeeContactInf principalEntity = null;

            var response = await _dbContext.EmployeeContactsInf.Where(x => x.InternalId == int.Parse(id) && x.EmployeeId == model.EmployeeId ).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            if (model.IsPrincipal)
            {
                principalEntity = await _dbContext.EmployeeContactsInf.Where(x => x.EmployeeId == model.EmployeeId
                                                                           && x.IsPrincipal == true
                                                                           && x.ContactType == model.ContactType).FirstOrDefaultAsync();
            }

            var entity = BuildDtoHelper<EmployeeContactInf>.OnBuild(model, response);
            _dbContext.EmployeeContactsInf.Update(entity);
            await _dbContext.SaveChangesAsync();


            //Actualizo la entidad que era principal
            if (principalEntity != null)
            {
                principalEntity.IsPrincipal = false;
                _dbContext.EmployeeContactsInf.Update(principalEntity);
                await _dbContext.SaveChangesAsync();
            }

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }
    }

}
