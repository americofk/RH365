using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.Taxes;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Taxes
{
    public interface ITaxCommandHandler :
        ICreateCommandHandler<TaxRequest>,
        IDeleteCommandHandler,
        IUpdateCommandHandler<TaxRequest>
    {
        public Task<Response<object>> UpdateStatus(string id, bool status);
    }

    public class TaxCommandHandler : ITaxCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserInformation _currentUser;

        public TaxCommandHandler(IApplicationDbContext applicationDbContext, ICurrentUserInformation currentUser)
        {
            _dbContext = applicationDbContext;
            _currentUser = currentUser;
        }


        public async Task<Response<object>> Create(TaxRequest model)
        {
            var response = await _dbContext.Taxes.Where(x => x.TaxId == model.TaxId).FirstOrDefaultAsync();

            if (response != null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El id de impuesto ya existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<Tax>.OnBuild(model, new Tax());
            entity.InCompany = _currentUser.Company;

            _dbContext.Taxes.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(entity)
            {
                Message = "Registro creado correctamente"
            };
        }


        public async Task<Response<bool>> Delete(List<string> ids)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.Taxes.Where(x => x.TaxId == item).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");
                    }

                    _dbContext.Taxes.Remove(response);
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


        public async Task<Response<object>> Update(string id, TaxRequest model)
        {
            var response = await _dbContext.Taxes.Where(x => x.TaxId == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<Tax>.OnBuild(model, response);
            _dbContext.Taxes.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }


        public async Task<Response<object>> UpdateStatus(string id, bool status)
        {
            var response = await _dbContext.Taxes.Where(x => x.TaxId == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Message = "El registro seleccionado no existe"
                };
            }

            response.TaxStatus = status;
            _dbContext.Taxes.Update(response);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }
    }

}
