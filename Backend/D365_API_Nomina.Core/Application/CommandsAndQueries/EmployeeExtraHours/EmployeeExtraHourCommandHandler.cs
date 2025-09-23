using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeExtraHours;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeExtraHours
{
    public interface IEmployeeExtraHourCommandHandler :
        ICreateCommandHandler<EmployeeExtraHourRequest>,
        IDeleteByParentCommandHandler<EmployeeExtraHourRequestDelete>,
        IUpdateCommandHandler<EmployeeExtraHourRequestUpdate>
    {
    }

    public class EmployeeExtraHourCommandHandler : IEmployeeExtraHourCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeExtraHourCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> Create(EmployeeExtraHourRequest model)
        {
            var response = await _dbContext.EmployeeExtraHours.Where(x => x.EmployeeId == model.EmployeeId
                                                                     && x.WorkedDay == model.WorkedDay
                                                                     && x.EarningCodeId == model.EarningCodeId).FirstOrDefaultAsync();
            if (response != null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro que se intenta crear ya existe" },
                    StatusHttp = 404
                };
            }

            //Buscar el earning code por la versión del mismo, según la fecha
            var earningcode = await _dbContext.EarningCodeVersions.Where(x => x.EarningCodeId == model.EarningCodeId
                                                            && x.IndexBase == IndexBase.Hour
                                                            && x.MultiplyAmount != 0
                                                            && x.ValidFrom <= model.WorkedDay && x.ValidTo >= model.WorkedDay).FirstOrDefaultAsync();

            if (earningcode == null)
            {
                return new Response<object>(null)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El código de hora extra asignado no existe o no tiene configurado el porcentaje a aplicar." },
                    StatusHttp = 404
                };
            }

            //Actualización, cálculo automático de horas extras
            if (earningcode.WorkFrom == default && earningcode.WorkTo == default && earningcode.IsHoliday == false)
            {
                return new Response<object>(null)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El código de hora extra asignado no tiene configuradas las horas de funcionamiento." },
                    StatusHttp = 404
                };
            }

            var salary = await _dbContext.EmployeeEarningCodes
                                .Join(_dbContext.EarningCodes,
                                    eec => eec.EarningCodeId,
                                    ec => ec.EarningCodeId,
                                    (eec, ec) => new { Eec = eec, Ec = ec })
                                .Where(x => x.Eec.EmployeeId == model.EmployeeId
                                    && x.Ec.IsExtraHours == true)
                                .Select(x => new
                                {
                                    Amount = x.Eec.IndexEarningMonthly
                                })
                                .ToListAsync();

            //Se busca la cantidad de horas
            var a = await QuantityHours(model.StartHour, model.EndHour, model.EmployeeId, model.WorkedDay, earningcode.WorkFrom, earningcode.WorkTo, earningcode.IsHoliday);
            if (a.StatusHttp == 404)
            {
                return a;
            }

            decimal quantity = (decimal)a.Data;

            var entity = BuildDtoHelper<EmployeeExtraHour>.OnBuild(model, new EmployeeExtraHour());
            entity.Indice = earningcode.MultiplyAmount / 100;
            entity.Quantity = quantity;

            if (salary != null)
            {
                decimal constans = 23.83M;
                entity.Amount = ((salary.Sum(x => x.Amount) / constans) / 8 * entity.Indice) * quantity;
            }

            _dbContext.EmployeeExtraHours.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(entity)
            {
                Message = "Registro creado correctamente"
            };
        }


        public async Task<Response<bool>> DeleteByParent(List<EmployeeExtraHourRequestDelete> ids, string parentid)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.EmployeeExtraHours.Where(x => x.EarningCodeId == item.EarningCodeId
                                                                             && x.WorkedDay == item.WorkedDay
                                                                             && x.EmployeeId == parentid
                                                                             && x.StatusExtraHour != StatusExtraHour.Pagada).FirstOrDefaultAsync();

                    //if (response == null)
                    //{
                    //    throw new Exception($"El registro seleccionado no existe o está en estado pagado- id {item}");
                    //}

                    var payrollprocess = await _dbContext.PayrollsProcess.Where(x => response.WorkedDay >= x.PeriodStartDate && response.WorkedDay <= x.PeriodEndDate
                                                                                && x.PayrollId == response.PayrollId
                                                                                && x.PayrollProcessStatus != PayrollProcessStatus.Canceled
                                                                                && x.PayrollProcessStatus != PayrollProcessStatus.Paid)
                                                                         .FirstOrDefaultAsync();

                    if (payrollprocess != null)
                    {
                        throw new Exception($"El registro seleccionado no se puede eliminar porque ya fue procesado en una nómina - id {item}");
                    }

                    _dbContext.EmployeeExtraHours.Remove(response);
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


        public async Task<Response<object>> Update(string id, EmployeeExtraHourRequestUpdate model)
        {
            var response = await _dbContext.EmployeeExtraHours.Where(x => x.EmployeeId == id
                                                                     && x.WorkedDay == model.WorkedDay
                                                                     && x.EarningCodeId == model.EarningCodeId
                                                                     && x.StatusExtraHour == StatusExtraHour.Open).FirstOrDefaultAsync();
            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe o no está en estado abierto" },
                    StatusHttp = 404
                };
            }

            var earningcode = await _dbContext.EarningCodes.Where(x => x.EarningCodeId == model.EarningCodeId
                                                && x.IndexBase == IndexBase.Hour
                                                && x.MultiplyAmount != 0).FirstOrDefaultAsync();

            if (earningcode == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El código de hora extra asignado no existe o no tiene configurado el porcentaje a aplicar." },
                    StatusHttp = 404
                };
            }

            //Actualización, cálculo automático de horas extras
            if (earningcode.WorkFrom == default && earningcode.WorkTo == default && earningcode.IsHoliday == false)
            {
                return new Response<object>(null)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El código de hora extra asignado no tiene configuradas las horas de funcionamiento." },
                    StatusHttp = 404
                };
            }

            var salary = await _dbContext.EmployeeEarningCodes
                                .Join(_dbContext.EarningCodes,
                                    eec => eec.EarningCodeId,
                                    ec => ec.EarningCodeId,
                                    (eec, ec) => new { Eec = eec, Ec = ec })
                                .Where(x => x.Eec.EmployeeId == id
                                    && x.Ec.IsExtraHours == true)
                                .Select(x => new
                                {
                                    Amount = x.Eec.IndexEarningMonthly
                                })
                                .ToListAsync();

            //Se busca la cantidad de horas
            var a = await QuantityHours(model.StartHour, model.EndHour, id, model.WorkedDay, earningcode.WorkFrom, earningcode.WorkTo, earningcode.IsHoliday);
            if (a.StatusHttp == 404)
            {
                return a;
            }

            decimal quantity = (decimal)a.Data;

            var entity = BuildDtoHelper<EmployeeExtraHour>.OnBuild(model, response);
            entity.Indice = earningcode.MultiplyAmount / 100;
            entity.Quantity = quantity;

            if (salary != null)
            {
                decimal constans = 23.83M;
                entity.Amount = ((salary.Sum(x => x.Amount) / constans) / 8 * entity.Indice) * quantity;
            }

            _dbContext.EmployeeExtraHours.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }

        //Método para buscar la cantidad de horas
        private async Task<Response<object>> QuantityHours(TimeSpan workFrom, TimeSpan workTo, string employeeid, DateTime workdate, TimeSpan configTimeFrom, TimeSpan configTimeTo, bool isHoliday)
        {
            TimeSpan workfromCalendar;
            TimeSpan worktoCalendar;

            var employee = await _dbContext.Employees.Where(x => x.EmployeeId == employeeid).FirstOrDefaultAsync();

            workfromCalendar = employee.WorkFrom;
            worktoCalendar = employee.WorkTo;

            if (!employee.IsFixedWorkCalendar)//Calendario de trabajo fijo            
            {                
                var calendarwork = await _dbContext.EmployeeWorkCalendars.Where(x => x.EmployeeId == employeeid && x.CalendarDate == workdate)
                                                                    .FirstOrDefaultAsync();

                if (calendarwork == null)
                {
                    return new Response<object>(null)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { "El empleado no tiene calendario de trabajo asignado en la fecha seleccionada" },
                        StatusHttp = 404
                    };
                }
                else
                {
                    workfromCalendar = calendarwork.WorkFrom;
                    worktoCalendar = calendarwork.WorkTo;
                }               
            }

            decimal diffWorkTime = CalcHoursHelper.GetQtyHour(workTo - workFrom);
            decimal diffCalendarTime = CalcHoursHelper.GetQtyHour(worktoCalendar - workfromCalendar);
            decimal diffTotal = 0;

            //Se verifica si es feriado
            if (isHoliday)
            {
                var holidaycalendar = await _dbContext.CalendarHolidays.Where(x => x.CalendarDate == workdate).FirstOrDefaultAsync();
                if (holidaycalendar == null)
                {
                    return new Response<object>(null)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { "El día seleccionado no está configurado como feriado" },
                        StatusHttp = 404
                    };
                }
                else
                {
                    return new Response<object>(Math.Abs(diffWorkTime)) { };
                }
            }

            //Comienza el proceso del cálculo del horario
            //Si el tiempo trabajado es mayor al tiempo del calendario 
            //hay horas extras
            if (diffWorkTime > diffCalendarTime)
            {
                //Se verifica si la configuración es de horario nocturno
                if (configTimeTo < configTimeFrom)
                {
                    var a = await NightHour(configTimeFrom, configTimeTo, workFrom, workTo,workfromCalendar, worktoCalendar);
                    diffTotal = (decimal)a.Data;
                }
                //La configuración no es de horario nocturno
                else
                {
                    var a = await DayHour(configTimeFrom, configTimeTo, workFrom, workTo, workfromCalendar, worktoCalendar);
                    diffTotal = (decimal)a.Data;
                }

                return new Response<object>(diffTotal);
            }            

            return new Response<object>(diffTotal) { };
        }

        //Método de horario nocturno
        private async Task<Response<object>> NightHour(TimeSpan configTimeFrom, TimeSpan configTimeTo,
                                                       TimeSpan workFrom, TimeSpan workTo,
                                                       TimeSpan workfromCalendar, TimeSpan worktoCalendar)
        {
            decimal totalhours = 0;

            //Primer escenario fecha hasta menor a fecha desde 
            if (workTo < workFrom)
            {
                if (workFrom < workfromCalendar)
                {
                    if (workFrom >= configTimeFrom)
                    {
                        totalhours += CalcHoursHelper.GetQtyHour(workfromCalendar - workFrom);
                        if (workTo < worktoCalendar) totalhours = totalhours - CalcHoursHelper.GetQtyHour(worktoCalendar - workTo);
                    }
                    else
                    {
                        totalhours += CalcHoursHelper.GetQtyHour(workfromCalendar - configTimeFrom) < 0 ?0: CalcHoursHelper.GetQtyHour(workfromCalendar - configTimeFrom);
                        //totalhours += CalcHoursHelper.GetQtyHour(configTimeFrom - workfromCalendar);
                        if (workTo < worktoCalendar) totalhours = totalhours - CalcHoursHelper.GetQtyHour(worktoCalendar - workTo);
                    }
                }

                if(workTo > worktoCalendar)
                {
                    if(workTo <= configTimeTo)
                    {
                        totalhours += CalcHoursHelper.GetQtyHour(workTo - worktoCalendar);
                        if (workFrom > workfromCalendar) totalhours = totalhours - CalcHoursHelper.GetQtyHour(workFrom - workfromCalendar);
                    }
                    else
                    {
                        totalhours += CalcHoursHelper.GetQtyHour(configTimeTo - worktoCalendar) < 0 ? 0: CalcHoursHelper.GetQtyHour(configTimeTo - worktoCalendar);
                        if (workFrom > workfromCalendar) totalhours = totalhours - CalcHoursHelper.GetQtyHour(workFrom - workfromCalendar);
                    }                     
                }
            }

            //Segundo escenario fecha hasta mayor a fecha desde
            if (workTo > workFrom)
            {
                if (workFrom < workfromCalendar)
                {
                    if (workfromCalendar > configTimeTo)
                    {
                        totalhours += CalcHoursHelper.GetQtyHour(configTimeTo - workFrom);
                        if (workTo < worktoCalendar) totalhours = totalhours - CalcHoursHelper.GetQtyHour(worktoCalendar - workTo);
                    }
                    else
                    {
                        totalhours += CalcHoursHelper.GetQtyHour(worktoCalendar - workFrom);
                        if (workTo < worktoCalendar) totalhours = totalhours - CalcHoursHelper.GetQtyHour(worktoCalendar - workTo);
                    }
                }

                if (workTo > worktoCalendar)
                {
                    if (worktoCalendar >= configTimeFrom)
                    {
                        totalhours += CalcHoursHelper.GetQtyHour(workTo - worktoCalendar);
                        if (workFrom > workfromCalendar) totalhours = totalhours - CalcHoursHelper.GetQtyHour(workFrom - workfromCalendar);
                    }
                    else
                    {
                        totalhours += CalcHoursHelper.GetQtyHour(workTo - configTimeFrom);
                        if (workFrom > workfromCalendar) totalhours = totalhours - CalcHoursHelper.GetQtyHour(workFrom - workfromCalendar);
                    }
                }
            }

            return new Response<object>(totalhours) { };
        }
        
        //Método de horario diurno
        private async Task<Response<object>> DayHour(TimeSpan configTimeFrom, TimeSpan configTimeTo,
                                                       TimeSpan workFrom, TimeSpan workTo,
                                                       TimeSpan workfromCalendar, TimeSpan worktoCalendar)
        {
            decimal totalhours = 0;

            //Segundo escenario fecha hasta mayor a fecha desde
            if (workTo > workFrom)
            {
                if (workTo > configTimeTo)
                {
                    totalhours += CalcHoursHelper.GetQtyHour(configTimeTo - worktoCalendar) < 0 ? 0 : CalcHoursHelper.GetQtyHour(configTimeTo - worktoCalendar);
                    if (workFrom > workfromCalendar) totalhours = totalhours - CalcHoursHelper.GetQtyHour(workFrom - workfromCalendar);
                }
                else
                {
                    //Se añadió la validación de valores negativos
                    totalhours += CalcHoursHelper.GetQtyHour(workTo - worktoCalendar) < 0 ? 0 : CalcHoursHelper.GetQtyHour(workTo - worktoCalendar);
                    if (workFrom > workfromCalendar) totalhours = totalhours - CalcHoursHelper.GetQtyHour(workFrom - workfromCalendar);
                }


                if (workFrom >= configTimeFrom)
                {
                    totalhours += CalcHoursHelper.GetQtyHour(workfromCalendar - workFrom) < 0 ? 0 : CalcHoursHelper.GetQtyHour(workfromCalendar - workFrom);
                    if (workTo < worktoCalendar) totalhours = totalhours - CalcHoursHelper.GetQtyHour(worktoCalendar - workTo);
                }
                else
                {
                    totalhours += CalcHoursHelper.GetQtyHour(workfromCalendar - configTimeFrom);
                    if (workTo < worktoCalendar) totalhours = totalhours - CalcHoursHelper.GetQtyHour(worktoCalendar - workTo);
                }
            }

            //Primer escenario fecha hasta menor a fecha desde 
            if (workTo < workFrom)
            {
                if (workTo > configTimeFrom)
                {
                    if (worktoCalendar > configTimeFrom)
                    {
                        totalhours += CalcHoursHelper.GetQtyHour(workTo - worktoCalendar);
                        if (workFrom > workfromCalendar) totalhours = totalhours - CalcHoursHelper.GetQtyHour(workFrom - workfromCalendar);
                    }
                    else
                    {
                        totalhours += CalcHoursHelper.GetQtyHour(workTo - configTimeFrom);
                        if (workFrom > workfromCalendar) totalhours = totalhours - CalcHoursHelper.GetQtyHour(workFrom - workfromCalendar);
                    }
                }
                 
                if(workFrom < configTimeTo)
                {
                    if(workfromCalendar > configTimeTo)
                    {
                        totalhours += CalcHoursHelper.GetQtyHour(configTimeTo - workFrom);
                        if (workTo < worktoCalendar) totalhours = totalhours - CalcHoursHelper.GetQtyHour(worktoCalendar - workTo);
                    }
                    else
                    {
                        totalhours += CalcHoursHelper.GetQtyHour(workfromCalendar - workFrom);
                        if (workTo < worktoCalendar) totalhours = totalhours - CalcHoursHelper.GetQtyHour(worktoCalendar - workTo);
                    }                     
                }
            }                      

            return new Response<object>(totalhours) { };
        }

        //Si la diferencia de los horarios de trabajo da negativa es horario de varios días
        //if (diffWorkTime < 0)
        //{

        //    //Si el tiempo trabajado es mayor al tiempo del calendario 
        //    //hay horas extras
        //    if (diffWorkTime > diffCalendarTime)
        //    {
        //        //Si la hora trabajada de inicio es menor a la hora de inicio del calendario
        //        if (workFrom < workfromCalendar)
        //        {
        //            if (configTimeFrom <= workFrom)
        //            {
        //                diffTotal += Math.Abs((workFrom - workfromCalendar).Hours);

        //            }
        //        }

        //        //Si la hora trabajada de salida es mayor a la hora de salida del calendario
        //        if (workTo > worktoCalendar)
        //        {
        //            if (configTimeTo >= workTo)
        //            {
        //                diffTotal += Math.Abs((workTo - worktoCalendar).Hours);
        //            }
        //        }

        //        return new Response<object>(diffTotal);
        //    }
        //}
        //else
        //{
        //    //Si el tiempo trabajado es mayor al tiempo del calendario 
        //    //hay horas extras
        //    if (diffWorkTime > diffCalendarTime)
        //    {
        //        //Si la hora trabajada de inicio es menor a la hora de inicio del calendario
        //        if (workFrom < workfromCalendar)
        //        {
        //            if (configTimeFrom <= workFrom)
        //            {
        //                diffTotal += Math.Abs((workFrom - workfromCalendar).Hours);
        //            }
        //        }

        //        //Si la hora trabajada de salida es mayor a la hora de salida del calendario
        //        if (workTo > worktoCalendar)
        //        {
        //            if (configTimeTo >= workTo)
        //            {
        //                diffTotal += Math.Abs((workTo - worktoCalendar).Hours);
        //            }
        //        }
        //        return new Response<object>(diffTotal);

        //    }
        //}
    }
}
