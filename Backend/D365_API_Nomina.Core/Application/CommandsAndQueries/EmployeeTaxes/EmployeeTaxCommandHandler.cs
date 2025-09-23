using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeTaxes;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeTaxes
{
    public interface IEmployeeTaxCommandHandler :
        ICreateCommandHandler<EmployeeTaxRequest>,
        IDeleteByParentCommandHandler,
        IUpdateCommandHandler<EmployeeTaxRequest>
    {

    }

    public class EmployeeTaxCommandHandler : IEmployeeTaxCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeTaxCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> Create(EmployeeTaxRequest model)
        {
            var response = await _dbContext.Taxes.Where(x => x.TaxId == model.TaxId).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El código de impuesto no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<EmployeeTax>.OnBuild(model, new EmployeeTax());

            _dbContext.EmployeeTaxes.Add(entity);
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
                    var response = await _dbContext.EmployeeTaxes.Where(x => x.TaxId == item && x.EmployeeId == parentid).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");
                    }

                    _dbContext.EmployeeTaxes.Remove(response);
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


        public async Task<Response<object>> Update(string id, EmployeeTaxRequest model)
        {
            var response = await _dbContext.EmployeeTaxes.Where(x => x.EmployeeId == id && x.TaxId == model.TaxId).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<EmployeeTax>.OnBuild(model, response);
            _dbContext.EmployeeTaxes.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }

    }

}
