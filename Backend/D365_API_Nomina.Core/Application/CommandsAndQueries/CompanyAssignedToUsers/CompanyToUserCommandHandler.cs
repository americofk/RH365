using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.CompanyAssignedToUsers;
using D365_API_Nomina.Core.Application.Common.Model.MenuAssignedToUsers;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.CompanyAssignedToUsers
{
    public interface ICompanyToUserCommandHandler
    {
        public Task<Response<object>> CreateAll(List<CompanyToUserRequest> request);

        public Task<Response<bool>> DeleteByAlias(List<string> ids, string alias);
    }

    public class CompanyToUserCommandHandler : ICompanyToUserCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public CompanyToUserCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> CreateAll(List<CompanyToUserRequest> request)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var model in request)
                {
                    var companyAssigned = _dbContext.CompaniesAssignedToUsers.Where(x => x.Alias == model.Alias && x.CompanyId == model.CompanyId).FirstOrDefaultAsync();

                    if (await companyAssigned == null)
                    {
                        var user = _dbContext.Users.Where(x => x.Alias == model.Alias).FirstOrDefaultAsync();
                        if (await user == null)
                        {
                            throw new Exception($"El usuario asignado no existe - Id {model.Alias}");
                        }

                        var company = _dbContext.Companies.Where(x => x.CompanyId == model.CompanyId).FirstOrDefaultAsync();
                        if (await company == null)
                        {
                            throw new Exception($"La empresa asignada no existe - Id {model.CompanyId}");
                        }

                        var entity = BuildDtoHelper<CompaniesAssignedToUser>.OnBuild(model, new CompaniesAssignedToUser());

                        _dbContext.CompaniesAssignedToUsers.Add(entity);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                transaction.Commit();
                return new Response<object>(true)
                {
                    Message = "Registros creados correctamente"
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { ex.Message }
                };
            }
        }

        public async Task<Response<bool>> DeleteByAlias(List<string> ids, string alias)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.CompaniesAssignedToUsers.Where(x => x.Alias == alias && x.CompanyId == item).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado de usuario y compañía no existe - Usuario {alias}, Compañía {item}");

                    }

                    _dbContext.CompaniesAssignedToUsers.Remove(response);
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
    }
}
