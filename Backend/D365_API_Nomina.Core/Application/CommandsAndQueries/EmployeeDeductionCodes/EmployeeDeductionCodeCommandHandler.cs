using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeDeductionCodes;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeDeductionCodes
{
    public interface IEmployeeDeductionCodeCommandHandler :
        ICreateCommandHandler<EmployeeDeductionCodeRequest>,
        IDeleteByParentCommandHandler,
        IUpdateCommandHandler<EmployeeDeductionCodeRequestUpdate>
    {
        
    }

    public class EmployeeDeductionCodeCommandHandler : IEmployeeDeductionCodeCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeDeductionCodeCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> Create(EmployeeDeductionCodeRequest model)
        {
            var payroll = await _dbContext.Payrolls.Where(x => x.PayrollId == model.PayrollId).FirstOrDefaultAsync();

            if (payroll == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "La nómina asociada no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<EmployeeDeductionCode>.OnBuild(model, new EmployeeDeductionCode());

            entity.PayFrecuency = payroll.PayFrecuency;

            _dbContext.EmployeeDeductionCodes.Add(entity);
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
                    var response = await _dbContext.EmployeeDeductionCodes.Where(x => x.DeductionCodeId == item && x.EmployeeId == parentid).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");
                    }

                    var payrollprocess = await _dbContext.PayrollsProcess
                                                            .Join(_dbContext.PayrollProcessActions,
                                                                pp => pp.PayrollProcessId,
                                                                pa => pa.PayrollProcessId,
                                                                (pp, pa) => new { Pp = pp, Pa = pa })
                                                            .Where(x => (response.FromDate <= x.Pp.PeriodEndDate || response.ToDate >= x.Pp.PeriodEndDate)
                                                                && x.Pp.PayrollId == response.PayrollId
                                                                && x.Pa.ActionId == item
                                                                && x.Pa.EmployeeId == parentid 
                                                                && x.Pp.PayrollProcessStatus != PayrollProcessStatus.Canceled).FirstOrDefaultAsync();

                    if (payrollprocess != null)
                    {
                        throw new Exception($"El registro seleccionado no se puede eliminar porque ya fue procesado en una nómina - id {item}");
                    }

                    _dbContext.EmployeeDeductionCodes.Remove(response);
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

        public async Task<Response<object>> Update(string id, EmployeeDeductionCodeRequestUpdate model)
        {
            var response = await _dbContext.EmployeeDeductionCodes.Where(x => x.EmployeeId == id && x.DeductionCodeId == model.DeductionCodeId).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            //Validar fecha al actualizar
            if (response.FromDate > model.ToDate)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "La fecha desde no puede ser mayor a la fecha hasta" },
                    StatusHttp = 404
                };
            }

            if (response.DeductionCodeId != model.DeductionCodeId || response.PayrollId != model.PayrollId)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "No puede cambiar el código de descuento, ni la nómina asociada" },
                    StatusHttp = 404
                };
            }
            var entity = BuildDtoHelper<EmployeeDeductionCode>.OnBuild(model, response);
            _dbContext.EmployeeDeductionCodes.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }
    }

}
