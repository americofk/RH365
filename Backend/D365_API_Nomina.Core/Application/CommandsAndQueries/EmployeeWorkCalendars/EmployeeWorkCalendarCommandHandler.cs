using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeWorkCalendars;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeWorkCalendars
{
    public interface IEmployeeWorkCalendarCommandHandler:
        ICreateCommandHandler<EmployeeWorkCalendarRequest>,
        IDeleteByParentCommandHandler<EmployeeWorkCalendarDeleteRequest>,
        IUpdateCommandHandler<EmployeeWorkCalendarRequest>
    {

    }

    public class EmployeeWorkCalendarCommandHandler : IEmployeeWorkCalendarCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeWorkCalendarCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<object>> Create(EmployeeWorkCalendarRequest model)
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

            var employeeWork = await _dbContext.EmployeeWorkCalendars.Where(x => x.EmployeeId == model.EmployeeId)
                                                                     .OrderByDescending(x => x.InternalId)
                                                                     .FirstOrDefaultAsync();

            var entity = BuildDtoHelper<EmployeeWorkCalendar>.OnBuild(model, new EmployeeWorkCalendar());
            entity.InternalId = employeeWork == null ? 1 : employeeWork.InternalId + 1;

            string weekday = spanishdays[(int)model.CalendarDate.DayOfWeek];

            entity.CalendarDay = weekday;

            _dbContext.EmployeeWorkCalendars.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(entity)
            {
                Message = "Registro creado correctamente"
            };
        }

        public async Task<Response<bool>> DeleteByParent(List<EmployeeWorkCalendarDeleteRequest> ids, string parentid)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.EmployeeWorkCalendars.Where(x => x.EmployeeId == parentid
                                                                                && x.InternalId == item.InternalId)
                                                                         .FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe");
                    }

                    _dbContext.EmployeeWorkCalendars.Remove(response);
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

        public async Task<Response<object>> Update(string id, EmployeeWorkCalendarRequest model)
        {
            string[] spanishdays = new[] { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };

            var response = await _dbContext.EmployeeWorkCalendars.Where(x => x.EmployeeId == model.EmployeeId
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

            var entity = BuildDtoHelper<EmployeeWorkCalendar>.OnBuild(model, response);

            _dbContext.EmployeeWorkCalendars.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(entity)
            {
                Message = "Registro actualizado correctamente"
            };
        }
    }
}
