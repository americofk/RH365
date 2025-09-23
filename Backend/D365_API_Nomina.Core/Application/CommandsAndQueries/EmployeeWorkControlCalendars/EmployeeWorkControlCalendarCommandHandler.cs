using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeWorkControlCalendars;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeWorkControlCalendars
{
    public interface IEmployeeWorkControlCalendarCommandHandler:
        ICreateCommandHandler<EmployeeWorkControlCalendarRequest>,
        IUpdateCommandHandler<EmployeeWorkControlCalendarRequest>,
        IDeleteByParentCommandHandler<EmployeeWorkControlCalendarDeleteRequest>
    {

    }

    public class EmployeeWorkControlCalendarCommandHandler : IEmployeeWorkControlCalendarCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeWorkControlCalendarCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<object>> Create(EmployeeWorkControlCalendarRequest model)
        {
            string[] spanishdays = new[] { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };

            var employees = await _dbContext.Employees.Where(x => x.EmployeeId == model.EmployeeId).FirstOrDefaultAsync();

            if (employees == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El empleado seleccionado no existe." },
                    StatusHttp = 404
                };
            }

            var employeeWork = await _dbContext.EmployeeWorkControlCalendars.Where(x => x.EmployeeId == model.EmployeeId)
                                                                     .OrderByDescending(x => x.InternalId)
                                                                     .FirstOrDefaultAsync();

            var entity = BuildDtoHelper<EmployeeWorkControlCalendar>.OnBuild(model, new EmployeeWorkControlCalendar());
            entity.InternalId = employeeWork == null ? 1 : employeeWork.InternalId + 1;

            string weekday = spanishdays[(int)model.CalendarDate.DayOfWeek];

            entity.CalendarDay = weekday;
            entity.TotalHour = (decimal)(Math.Abs((model.WorkTo - model.WorkFrom).TotalHours) - Math.Abs((model.BreakWorkFrom - model.BreakWorkTo).TotalHours));

            _dbContext.EmployeeWorkControlCalendars.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(entity)
            {
                Message = "Registro creado correctamente"
            };
        }

        public async Task<Response<bool>> DeleteByParent(List<EmployeeWorkControlCalendarDeleteRequest> ids, string parentid)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.EmployeeWorkControlCalendars.Where(x => x.EmployeeId == parentid
                                                                                && x.InternalId == item.InternalId)
                                                                         .FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe");
                    }

                    _dbContext.EmployeeWorkControlCalendars.Remove(response);
                    await _dbContext.SaveChangesAsync();
                }

                transaction.Commit();
                return new Response<bool>(true) { Message = "Registros eliminados con éxito" };
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

        public async Task<Response<object>> Update(string id, EmployeeWorkControlCalendarRequest model)
        {
            string[] spanishdays = new[] { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };

            var response = await _dbContext.EmployeeWorkControlCalendars.Where(x => x.EmployeeId == model.EmployeeId
                                                                        && x.InternalId == int.Parse(id)).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe." },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<EmployeeWorkControlCalendar>.OnBuild(model, response);
            entity.TotalHour = (decimal)(Math.Abs((model.WorkTo - model.WorkFrom).TotalHours) - Math.Abs((model.BreakWorkFrom - model.BreakWorkTo).TotalHours));

            _dbContext.EmployeeWorkControlCalendars.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(entity)
            {
                Message = "Registro actualizado correctamente"
            };
        }
    }
}
