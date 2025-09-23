using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.PayCycles;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.StoreServices.PayCycles
{
    public interface IPayCycleCommandHandler : 
        ICreateCommandHandler<PayCycleRequest>,
        IDeleteByParentCommandHandler
    {
        public Task<Response<object>> MarkIsForTax(PayCycleIsForTaxRequest model);
        public Task<Response<object>> MarkIsForTss(PayCycleIsForTssRequest model);
    }

    public class PayCycleCommandHandler : IPayCycleCommandHandler
    {
        private readonly IApplicationDbContext dbContext;

        public PayCycleCommandHandler(IApplicationDbContext _dbcontext)
        {
            dbContext = _dbcontext;
        }

        public async Task<Response<object>> Create(PayCycleRequest _model)
        {
            List<PayCycle> payCycles = null;

            //string messageError = string.Empty;

            var payroll = dbContext.Payrolls.Where(x => x.PayrollId == _model.PayrollId).FirstOrDefault();

            if(payroll == null)
            {
                return new Response<object>()
                {
                    Message = "El parámetro id de nómina suministrado no existe",
                    Succeeded = false,
                    StatusHttp = 404
                };
            }
            else
            {
                //Obtengo de la tabla el registro más alto si existe
                var lastPayCycle = dbContext.PayCycles.Where(x => x.PayrollId == _model.PayrollId)
                                    .OrderByDescending(o => o.PeriodEndDate)
                                    .FirstOrDefault();

                if(lastPayCycle == null) //No hay registros, es la primera generación de los periodos
                {
                    //if (_model.IsFirstPeriod)
                        payCycles = this.GeneratePayCycles(_model, payroll.PayFrecuency, 0, payroll.ValidFrom);
                    //else
                    //    messageError = "Se indicó que no era la primera generación de los periodos, " +
                    //        "pero no existen periodos generados";
                }
                else //Si hay registros generados
                {
                    //if (!_model.IsFirstPeriod) //Se evalua si se solicitó la primera generación de los periodos
                    //{
                        
                        payCycles = this.GeneratePayCycles(_model, 
                            payroll.PayFrecuency, 
                            lastPayCycle.PayCycleId,
                            lastPayCycle.PeriodEndDate.AddDays(1));
                    //}
                    //else
                    //    messageError = "Se indicó que era el primero periodo pero ya existen periodos generados.";
                }
            }

            //using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
            //{

            //}
            //    dbContext.Database.BeginTransaction();
            //if (!string.IsNullOrWhiteSpace(messageError))
            //{
            //    return new Response<object>()
            //    {
            //        Succeeded = false,
            //        Message = messageError,
            //        StatusHttp = 404
            //    };
            //}

            dbContext.PayCycles.AddRange(payCycles);
            await dbContext.SaveChangesAsync();

            //dbContext.Rollback();
            return new Response<object>(payCycles);
        }
        

        private List<PayCycle> GeneratePayCycles(PayCycleRequest _model, PayFrecuency _payFrecuency, 
                                                 int secuence, DateTime _startDate)
        {
            List<PayCycle> payCycles = new List<PayCycle>();

            DateTime startPeriodDate    = _startDate;                 
            DateTime prevEndPeriodDate  = default;

            for (int i = 1; i <= _model.PayCycleQty; i++)
            {
                secuence++; //Secuencia incremental para el id del ciclo de pago

                //Se le suma los días según el tipo de pago
                DateTime endPeriodDate = QtyDays(_payFrecuency, startPeriodDate, i);

                PayCycle payCycle = new PayCycle()
                {
                    PeriodStartDate = i == 1 ? startPeriodDate : prevEndPeriodDate,
                    PeriodEndDate = _payFrecuency == PayFrecuency.BiWeekly ? endPeriodDate : endPeriodDate.AddDays(-1),
                    DefaultPayDate = i == 1 ? startPeriodDate : prevEndPeriodDate,
                    AmountPaidPerPeriod = 0,
                    StatusPeriod = Domain.Enums.StatusPeriod.Open,
                    PayrollId = _model.PayrollId,
                    PayCycleId = secuence
                };

                if (_payFrecuency == PayFrecuency.BiWeekly)
                {
                    startPeriodDate = endPeriodDate.AddDays(1);
                    prevEndPeriodDate = startPeriodDate;
                }
                else
                {
                    prevEndPeriodDate = endPeriodDate;
                }

                payCycles.Add(payCycle);

                ////Se le suma un día a la fecha final para colocar la nueva fecha inicial
                //startPeriodDate = endPeriodDate.AddDays(1);

                ////Se igualan las fechas para sumar de nuevo a la fecha final el ciclo de pago
                //endPeriodDate = startPeriodDate;
            }

            return payCycles;
        }

        private DateTime QtyDays(PayFrecuency  _PayFrecuency, DateTime _StartDate, int _Cycles)
        {
            DateTime EndDate = _StartDate;

            switch (_PayFrecuency)
            {
                case PayFrecuency.Diary:
                    EndDate = _StartDate.AddDays(_Cycles);
                    break;
                case PayFrecuency.Weekly:
                    EndDate = _StartDate.AddDays(7* _Cycles);
                    break;
                case PayFrecuency.TwoWeekly:
                    EndDate = _StartDate.AddDays(14* _Cycles);
                    break;
                case PayFrecuency.BiWeekly:
                    EndDate = _StartDate.Day < 16? 
                        new DateTime(_StartDate.Year, _StartDate.Month, 15): 
                        new DateTime((_StartDate.Month + 1 > 12?_StartDate.Year + 1:_StartDate.Year), (_StartDate.Month + 1 > 12 ? 1 : _StartDate.Month + 1), 1).AddDays(-1);
                    break;
                case PayFrecuency.Monthly:
                    EndDate = _StartDate.AddMonths(_Cycles);
                    break;
                case PayFrecuency.ThreeMonth:
                    EndDate = _StartDate.AddMonths(3* _Cycles);
                    break;
                case PayFrecuency.FourMonth:
                    EndDate = _StartDate.AddMonths(4* _Cycles);
                    break;
                case PayFrecuency.Biannual:
                    EndDate = _StartDate.AddMonths(6* _Cycles);
                    break;
                case PayFrecuency.Yearly:
                    EndDate = _StartDate.AddYears(1);
                    break;
            }

            return EndDate;
        }


        public async Task<Response<bool>> DeleteByParent(List<string> ids, string parentid)
        {
            using var transaction = dbContext.Database.BeginTransaction();

            try
            {
                ids = ids.OrderByDescending(x => x).ToList();

                foreach (var item in ids)
                {
                    var response = await dbContext.PayCycles.Where(x => x.PayCycleId == int.Parse(item) 
                                                                    && x.PayrollId == parentid
                                                                    && x.StatusPeriod == StatusPeriod.Open).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe o no está en un estado válido para eliminar - id {item}");
                    }

                    var paycyclehigh = await dbContext.PayCycles.Where(x => x.PayrollId == parentid)
                        .OrderByDescending(x => x.PayCycleId)
                        .FirstOrDefaultAsync();

                    if (paycyclehigh.PayCycleId == int.Parse(item))
                        dbContext.PayCycles.Remove(response);
                    else
                        throw new Exception($"El registro seleccionado no se puede eliminar, elimine las fechas superiores primero - id {item}");

                    await dbContext.SaveChangesAsync();
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

        public async Task<Response<object>> MarkIsForTax(PayCycleIsForTaxRequest model)
        {
            var response = await dbContext.PayCycles.Where(x => x.PayCycleId == model.PayCycleId && x.PayrollId == model.PayrollId
                                                           && x.StatusPeriod == StatusPeriod.Open).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe o no está en un estado válido para actualizar" },
                    StatusHttp = 404
                };
            }

            var payrollprocess = await dbContext.PayrollsProcess.Where(x => x.PayCycleId == model.PayCycleId && x.PayrollId == model.PayrollId).FirstOrDefaultAsync();

            if (payrollprocess != null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no se puede actualizar por que está seleccionado para un proceso de nómina" },
                    StatusHttp = 404
                };
            }

            var entity = response;
            entity.IsForTax = model.IsForTax;

            dbContext.PayCycles.Update(entity);
            await dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }

        public async Task<Response<object>> MarkIsForTss(PayCycleIsForTssRequest model)
        {
            var response = await dbContext.PayCycles.Where(x => x.PayCycleId == model.PayCycleId && x.PayrollId == model.PayrollId
                                                && x.StatusPeriod == StatusPeriod.Open).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe o no está en un estado válido para actualizar" },
                    StatusHttp = 404
                };
            }

            var payrollprocess = await dbContext.PayrollsProcess.Where(x => x.PayCycleId == model.PayCycleId 
                                                                       && x.PayrollId == model.PayrollId)
                                                                .FirstOrDefaultAsync();

            if (payrollprocess != null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no se puede actualizar porque está seleccionado para un proceso de nómina" },
                    StatusHttp = 404
                };
            }

            var entity = response;
            entity.IsForTss = model.IsForTss;

            dbContext.PayCycles.Update(entity);
            await dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }
    }
}
