using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeEarningCodes;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeEarningCodes
{
    public interface IEmployeeEarningCodeCommandHandler :
        ICreateCommandHandler<EmployeeEarningCodeRequest>,
        IDeleteByParentCommandHandler,
        IUpdateCommandHandler<EmployeeEarningCodeRequest>
    {
    }

    public class EmployeeEarningCodeCommandHandler : IEmployeeEarningCodeCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeEarningCodeCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> Create(EmployeeEarningCodeRequest model)
        {
            var earningcode = await _dbContext.EarningCodes.Where(x => x.EarningCodeId == model.EarningCodeId).FirstOrDefaultAsync();

            if (earningcode == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El código de ganancia seleccionado no existe" },
                    StatusHttp = 404
                };
            }



            var response = await _dbContext.EmployeeEarningCodes.Where(x => x.EmployeeId == model.EmployeeId).OrderByDescending(x => x.InternalId).FirstOrDefaultAsync();

            var entity = BuildDtoHelper<EmployeeEarningCode>.OnBuild(model, new EmployeeEarningCode());

            entity.InternalId = response == null ? 1 : response.InternalId + 1;

            //if (response != null)
            //{
            //    return new Response<object>(false)
            //    {
            //        Succeeded = false,
            //        Errors = new List<string>() { "El registro seleccionado ya existe" },
            //        StatusHttp = 404
            //    };
            //}

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

            //var entity = BuildDtoHelper<EmployeeEarningCode>.OnBuild(model, new EmployeeEarningCode());
            entity.PayFrecuency = payroll.PayFrecuency;
            entity.IsUseDGT = earningcode.IsUseDGT;

            entity.IndexEarning = model.IndexEarning;
            entity.IndexEarningMonthly = model.IndexEarningMonthly;

            if (model.IndexEarning != model.IndexEarningMonthly)
            {
                entity.IndexEarning = CalcAmountForMonth(payroll.PayFrecuency, model.IndexEarningMonthly);
            }
            //entity.IndexEarning = CalcAmountForMonth(payroll.PayFrecuency, model.IndexEarningMonthly);

            decimal constans = 23.83M;
            entity.IndexEarningDiary = entity.IndexEarningMonthly / constans;
            entity.IndexEarningHour = entity.IndexEarningDiary / 8;


            _dbContext.EmployeeEarningCodes.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(entity)
            {
                Message = "Registro creado correctamente"
            };
        }

        //parentid - employeeid
        public async Task<Response<bool>> DeleteByParent(List<string> ids, string parentid)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.EmployeeEarningCodes.Where(x => x.InternalId == int.Parse(item) && x.EmployeeId == parentid).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");
                    }
                     
                    var payrollprocess = await _dbContext.PayrollsProcess
                                                            .Join(_dbContext.PayrollProcessActions,
                                                                pp => pp.PayrollProcessId,
                                                                pa => pa.PayrollProcessId,
                                                                (pp,pa)=> new {Pp=pp, Pa=pa })
                                                            .Where(x => (response.FromDate <= x.Pp.PeriodEndDate && response.ToDate >= x.Pp.PeriodEndDate)
                                                                && x.Pp.PayrollId == response.PayrollId
                                                                && x.Pp.PayrollProcessStatus != PayrollProcessStatus.Canceled
                                                                && x.Pa.ActionId == response.EarningCodeId
                                                                && x.Pa.EmployeeId == parentid).FirstOrDefaultAsync();
                    
                    //Proceso anterior
                    //var payrollprocess = await _dbContext.PayrollsProcess
                                                            //.Join(_dbContext.PayrollProcessActions,
                                                            //    pp => pp.PayrollProcessId,
                                                            //    pa => pa.PayrollProcessId,
                                                            //    (pp,pa)=> new {Pp=pp, Pa=pa })
                                                            //.Where(x => (response.FromDate <= x.Pp.PeriodEndDate || response.ToDate >= x.Pp.PeriodEndDate)
                                                            //    && x.Pp.PayrollId == response.PayrollId
                                                            //    && x.Pp.PayrollProcessStatus != PayrollProcessStatus.Canceled
                                                            //    && x.Pa.ActionId == response.EarningCodeId
                                                            //    && x.Pa.EmployeeId == parentid).FirstOrDefaultAsync();

                    if (payrollprocess != null)
                    {
                        throw new Exception($"El registro seleccionado no se puede eliminar porque ya fue procesado en una nómina - id {item}");
                    }

                    _dbContext.EmployeeEarningCodes.Remove(response);
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


        public async Task<Response<object>> Update(string id, EmployeeEarningCodeRequest model)
        {
            decimal currentAmount = 0;

            var response = await _dbContext.EmployeeEarningCodes.Where(x => x.EmployeeId == id && x.InternalId == model.internalid).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            if (response.EarningCodeId != model.EarningCodeId || response.PayrollId != model.PayrollId)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "No puede cambiar el código de ganancia, ni la nómina asociada" },
                    StatusHttp = 404
                };
            }


            var earningCode = await _dbContext.EarningCodes.Where(x => x.EarningCodeId == model.EarningCodeId).FirstOrDefaultAsync();

            if (earningCode == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El código de ganancia seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            currentAmount = response.IndexEarningMonthly;

            var entity = BuildDtoHelper<EmployeeEarningCode>.OnBuild(model, response);
            entity.IsUseDGT = earningCode.IsUseDGT;

            //Calculo de los montos
            var payroll = await _dbContext.Payrolls.Where(x => x.PayrollId == model.PayrollId).FirstOrDefaultAsync();

            entity.IndexEarningMonthly = model.IndexEarningMonthly;
            entity.IndexEarning = CalcAmountForMonth(payroll.PayFrecuency, model.IndexEarningMonthly);

            decimal constans = 23.83M;
            entity.IndexEarningDiary = entity.IndexEarningMonthly / constans;
            entity.IndexEarningHour = entity.IndexEarningDiary / 8;

            _dbContext.EmployeeEarningCodes.Update(entity);
            await _dbContext.SaveChangesAsync();

            //return new Response<object>(false)
            //{
            //    Succeeded = false,
            //    Errors = new List<string>() { $"{earningCode.IsUseDGT} - {response.IndexEarningMonthly} - {model.IndexEarningMonthly} - {model.IsForDGT}"  },
            //    StatusHttp = 404
            //};

            //Se guarda la información en la tabla del historial
            if (earningCode.IsUseDGT &&
               currentAmount != model.IndexEarningMonthly
               && model.IsForDGT)
            {
                var employeehistory = new EmployeeHistory()
                {
                    Type = "NC",
                    Description = $"Se cambió el salario de {currentAmount} a {model.IndexEarningMonthly}",
                    RegisterDate = DateTime.Now,
                    EmployeeId = model.EmployeeId,
                    IsUseDGT = model.IsForDGT
                };

                _dbContext.EmployeeHistories.Add(employeehistory);
                await _dbContext.SaveChangesAsync();

                return new Response<object>(true) { Message = "Registro actualizado con éxito, se agregó al historial del empleado" };
            }

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }


        //Función para obtener el número de periodos para completar el mes
        private decimal CalcAmountForMonth(PayFrecuency _PayFrecuency, decimal amount)
        {
            decimal newamount = 0;

            switch (_PayFrecuency)
            {
                case PayFrecuency.Diary:
                    newamount = amount / 30;
                    break;
                case PayFrecuency.Weekly:
                    newamount = amount / 4;
                    break;
                case PayFrecuency.TwoWeekly:
                    newamount = amount / 2;
                    break;
                case PayFrecuency.BiWeekly:
                    newamount = amount / 2;
                    break;
                case PayFrecuency.Monthly:
                    newamount = 1;
                    break;
                case PayFrecuency.ThreeMonth:
                    newamount = amount * 3;
                    break;
                case PayFrecuency.FourMonth:
                    newamount = amount * 4;
                    break;
                case PayFrecuency.Biannual:
                    newamount = amount * 6;
                    break;
                case PayFrecuency.Yearly:
                    newamount = amount * 12;
                    break;
            }

            return newamount;
        }
    }

}
