using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.CalendarHolidays;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.CalendarHolidays
{
    public interface ICalendarHolidayCommandHandler:
        ICreateCommandHandler<CalendarHolidayRequest>,
        IDeleteByParentCommandHandler<CalendarHolidayDeleteRequest>,
        IUpdateCommandHandler<CalendarHolidayRequest>
    {

    }

    public class CalendarHolidayCommandHandler : ICalendarHolidayCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public CalendarHolidayCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<object>> Create(CalendarHolidayRequest model)
        {
            var entity = BuildDtoHelper<CalendarHoliday>.OnBuild(model, new CalendarHoliday());

            _dbContext.CalendarHolidays.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(entity)
            {
                Message = "Registro creado correctamente"
            };
        }

        public async Task<Response<bool>> DeleteByParent(List<CalendarHolidayDeleteRequest> ids, string parentid)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.CalendarHolidays.Where(x => x.CalendarDate == item.CalendarDate).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item.CalendarDate}");
                    }

                    _dbContext.CalendarHolidays.Remove(response);
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

        //Parametro id no se usa en este proceso
        public async Task<Response<object>> Update(string id, CalendarHolidayRequest model)
        {
            var response = await _dbContext.CalendarHolidays.Where(x => x.CalendarDate == model.CalendarDate).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<CalendarHoliday>.OnBuild(model, response);
            _dbContext.CalendarHolidays.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }
    }
}
