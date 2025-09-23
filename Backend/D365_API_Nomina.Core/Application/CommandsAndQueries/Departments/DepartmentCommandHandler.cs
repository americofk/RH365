using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.Departments;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Departments
{
    public interface IDepartmentCommandHandler : ICreateCommandHandler<DepartmentRequest>, IDeleteCommandHandler, IUpdateCommandHandler<DepartmentRequest>
    {
        public Task<Response<object>> UpdateStatus(string id, bool status);
    }

    public class DepartmentCommandHandler : IDepartmentCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;


        public DepartmentCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }


        public async Task<Response<object>> Create(DepartmentRequest model)
        {
            var entity = BuildDtoHelper<Department>.OnBuild(model, new Department());

            _dbContext.Departments.Add(entity);
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
                    
                    var response = await _dbContext.Departments.Where(x => x.DepartmentId == item).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");
                    }

                    var position = await _dbContext.Positions.Where(x => x.DepartmentId == item).FirstOrDefaultAsync();

                    if (position != null)
                    {
                        throw new Exception($"El registro seleccionado no se puede eliminar porque está asociado a un puesto - id {item}");
                    }


                    var loan = await _dbContext.Loans.Where(x => x.DepartmentId == item).FirstOrDefaultAsync();

                    if (loan != null)
                    {
                        throw new Exception($"El registro seleccionado no se puede eliminar porque está asociado a un préstamo - id {item}");
                    }


                    var earningCode = await _dbContext.EarningCodes.Where(x => x.Department == item).FirstOrDefaultAsync();

                    if (earningCode != null)
                    {
                        throw new Exception($"El registro seleccionado no se puede eliminar porque está asociado a un código de ganancias - id {item}");
                    }


                    var deductionCode = await _dbContext.DeductionCodes.Where(x => x.Department == item).FirstOrDefaultAsync();

                    if (deductionCode != null)
                    {
                        throw new Exception($"El registro seleccionado no se puede eliminar porque está asociado a un código de deducciones - id {item}");
                    }


                    var tax = await _dbContext.Taxes.Where(x => x.DepartmentId == item).FirstOrDefaultAsync();

                    if (tax != null)
                    {
                        throw new Exception($"El registro seleccionado no se puede eliminar porque está asociado a un código de impuesto - id {item}");
                    }


                    await ValidateEntity(item);

                    _dbContext.Departments.Remove(response);
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
                    Errors = new List<string>() { ex.Message },
                    StatusHttp = 404
                };
            }
        }


        public async Task<Response<object>> Update(string id, DepartmentRequest model)
        {
            var response = await _dbContext.Departments.Where(x => x.DepartmentId == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<Department>.OnBuild(model, response);
            _dbContext.Departments.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }


        public async Task<Response<object>> UpdateStatus(string id, bool status)
        {
            var response = await _dbContext.Departments.Where(x => x.DepartmentId == (string)id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Message = "El registro seleccionado no existe"
                };
            }

            response.DepartamentStatus = status;
            _dbContext.Departments.Update(response);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }

        private async Task<bool> ValidateEntity(string departmentid)
        {
            var earning = await _dbContext.EarningCodes.Where(x => x.Department == departmentid)
                .Select(x => x.EarningCodeId).FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(earning))
            {
                throw new Exception($"El registro seleccionado no se puede eliminar porque está asociado a un código de ganancia - id {departmentid}");
            }

            var deduction = await _dbContext.DeductionCodes.Where(x => x.Department == departmentid)
                .Select(x => x.DeductionCodeId).FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(deduction))
            {
                throw new Exception($"El registro seleccionado no se puede eliminar porque está asociado a un código de deducción - id {departmentid}");
            }

            var loan = await _dbContext.Loans.Where(x => x.DepartmentId == departmentid)
                .Select(x => x.LoanId).FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(loan))
            {
                throw new Exception($"El registro seleccionado no se puede eliminar porque está asociado a un préstamo - id {departmentid}");
            }
            
            var tax = await _dbContext.Taxes.Where(x => x.DepartmentId == departmentid)
                .Select(x => x.TaxId).FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(tax))
            {
                throw new Exception($"El registro seleccionado no se puede eliminar porque está asociado a un código de impuesto - id {departmentid}");
            }
            
            var position = await _dbContext.Positions.Where(x => x.DepartmentId == departmentid)
                .Select(x => x.PositionId).FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(position))
            {
                throw new Exception($"El registro seleccionado no se puede eliminar porque está asociado a una posición - id {departmentid}");
            }

            return true;
        }

        //public async Task<Response<object>> Delete(string id)
        //{
        //    //Validar si el departamento tiene empleados asociados para no permitir eliminar
        //    var response = await _dbContext.Departments.Where(x => x.DepartmentId == (string)id).FirstOrDefaultAsync();

        //    if (response == null)
        //    {
        //        return new Response<object>(false)
        //        {
        //            Succeeded = false,
        //            Message = "El registro seleccionado no existe"
        //        };
        //    }

        //    _dbContext.Departments.Remove(response);
        //    await _dbContext.SaveChangesAsync();
        //    return new Response<object>(true) { Message = "Registro elimando con éxito"};
        //}

    }
}
