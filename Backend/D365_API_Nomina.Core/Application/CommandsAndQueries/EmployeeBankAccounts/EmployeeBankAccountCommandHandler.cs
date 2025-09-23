using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeBankAccounts;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeBankAccounts
{
    public interface IEmployeeBankAccountCommandHandler :
        ICreateCommandHandler<EmployeeBankAccountRequest>,
        IUpdateCommandHandler<EmployeeBankAccountRequest>
    {
        public Task<Response<bool>> DeleteByEmployee(List<string> ids, string employeeid);
    }

    public class EmployeeBankAccountCommandHandler : IEmployeeBankAccountCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeBankAccountCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> Create(EmployeeBankAccountRequest model)
        {
            EmployeeBankAccount principalEntity = null;

            var bankaccount = await _dbContext.EmployeeBankAccounts.Where(x => x.EmployeeId == model.EmployeeId)
                                        .OrderByDescending(x => x.InternalId).FirstOrDefaultAsync();

            var entity = BuildDtoHelper<EmployeeBankAccount>.OnBuild(model, new EmployeeBankAccount());

            entity.InternalId = bankaccount == null ? 0 : bankaccount.InternalId + 1;

            if (model.IsPrincipal)
            {
                principalEntity = await _dbContext.EmployeeBankAccounts.Where(x => x.EmployeeId == model.EmployeeId
                                                                              && x.IsPrincipal == true).FirstOrDefaultAsync();
            }

            //Guardo la nueva dirección
            _dbContext.EmployeeBankAccounts.Add(entity);
            await _dbContext.SaveChangesAsync();

            //Actualizo la entidad que era principal
            if (principalEntity != null)
            {
                principalEntity.IsPrincipal = false;
                _dbContext.EmployeeBankAccounts.Update(principalEntity);
                await _dbContext.SaveChangesAsync();
            }

            return new Response<object>(entity)
            {
                Message = "Registro creado correctamente"
            };
        }


        public async Task<Response<bool>> DeleteByEmployee(List<string> ids, string employeeid)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.EmployeeBankAccounts.Where(x => x.InternalId == int.Parse(item)
                                                                                && x.EmployeeId == employeeid).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");

                    }

                    _dbContext.EmployeeBankAccounts.Remove(response);
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


        public async Task<Response<object>> Update(string id, EmployeeBankAccountRequest model)
        {
            EmployeeBankAccount principalEntity = null;

            var response = await _dbContext.EmployeeBankAccounts.Where(x => x.InternalId == int.Parse(id)
                                                                        && x.EmployeeId == model.EmployeeId).FirstOrDefaultAsync();

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
                principalEntity = await _dbContext.EmployeeBankAccounts.Where(x => x.EmployeeId == model.EmployeeId
                                                                              && x.IsPrincipal == true).FirstOrDefaultAsync();
            }

            var entity = BuildDtoHelper<EmployeeBankAccount>.OnBuild(model, response);
            _dbContext.EmployeeBankAccounts.Update(entity);
            await _dbContext.SaveChangesAsync();

            //Actualizo la entidad que era principal
            if (principalEntity != null)
            {
                principalEntity.IsPrincipal = false;
                _dbContext.EmployeeBankAccounts.Update(principalEntity);
                await _dbContext.SaveChangesAsync();
            }

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }
    }

}
