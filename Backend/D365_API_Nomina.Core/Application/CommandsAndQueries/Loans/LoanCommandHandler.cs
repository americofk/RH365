using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.Loans;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Loans
{
    public interface ILoanCommandHandler :
        ICreateCommandHandler<LoanRequest>,
        IDeleteCommandHandler,
        IUpdateCommandHandler<LoanRequest>
    {
        public Task<Response<object>> UpdateStatus(string id, bool status);
    }

    public class LoanCommandHandler : ILoanCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public LoanCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> Create(LoanRequest model)
        {
            var entity = BuildDtoHelper<Loan>.OnBuild(model, new Loan());

            _dbContext.Loans.Add(entity);
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
                    var response = await _dbContext.Loans.Where(x => x.LoanId == item).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");
                    }

                    var employeeloans = await _dbContext.EmployeeLoans.Where(x => x.LoanId == item).FirstOrDefaultAsync();

                    if (employeeloans != null)
                    {
                        throw new Exception($"El registro seleccionado no se puede eliminar porque está asignado a un empleado - id {item}");
                    }

                    _dbContext.Loans.Remove(response);
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


        public async Task<Response<object>> Update(string id, LoanRequest model)
        {
            var response = await _dbContext.Loans.Where(x => x.LoanId == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<Loan>.OnBuild(model, response);
            _dbContext.Loans.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }


        public async Task<Response<object>> UpdateStatus(string id, bool status)
        {
            var response = await _dbContext.Loans.Where(x => x.LoanId == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Message = "El registro seleccionado no existe"
                };
            }

            response.LoanStatus = status;
            _dbContext.Loans.Update(response);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }
    }

}
