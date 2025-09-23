using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.TaxDetails;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.TaxDetails
{
    public interface ITaxDetailCommandHandler :
        ICreateCommandHandler<TaxDetailRequest>,
        IDeleteByParentCommandHandler,
        IUpdateCommandHandler<TaxDetailRequest>
    {
    }

    public class TaxDetailCommandHandler : ITaxDetailCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public TaxDetailCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> Create(TaxDetailRequest model)
        {
            var response = await _dbContext.Taxes.Where(x => x.TaxId == model.TaxId).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var taxdetails = await _dbContext.TaxDetails.Where(x => x.TaxId == model.TaxId).OrderByDescending(x => x.InternalId).FirstOrDefaultAsync();
            var entity = BuildDtoHelper<TaxDetail>.OnBuild(model, new TaxDetail());

            entity.InternalId = taxdetails == null ? 1 : taxdetails.InternalId + 1;
            entity.DataareaID = response.DataareaID;

            _dbContext.TaxDetails.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(entity)
            {
                Message = "Registro creado correctamente"
            };
        }

        public async Task<Response<bool>> DeleteByParent(List<string> ids, string parentid)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.TaxDetails.Where(x => x.InternalId == int.Parse(item) && x.TaxId == parentid).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");
                    }

                    _dbContext.TaxDetails.Remove(response);
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

        public async Task<Response<object>> Update(string id, TaxDetailRequest model)
        {
            var response = await _dbContext.TaxDetails.Where(x => x.InternalId == int.Parse(id) && x.TaxId == model.TaxId).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<TaxDetail>.OnBuild(model, response);
            _dbContext.TaxDetails.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }

    }

}
