using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.CourseEmployees;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.CourseEmployees
{
    public interface ICourseEmployeeCommandHandler :
        ICreateCommandHandler<CourseEmployeeRequest>,
        IUpdateCommandHandler<CourseEmployeeRequest>
    {
        public Task<Response<bool>> DeleteByCourseId(List<string> ids, string courseid);

    }

    public class CourseEmployeeCommandHandler : ICourseEmployeeCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public CourseEmployeeCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> Create(CourseEmployeeRequest model)
        {
            var course = _dbContext.Courses.Where(x => x.CourseId == model.CourseId).FirstOrDefaultAsync();
            if (await course == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { $"El curso asignado no existe - Id {model.CourseId}" },
                    StatusHttp = 404
                };
            }

            var employee = _dbContext.Employees.Where(x => x.EmployeeId == model.EmployeeId).FirstOrDefaultAsync();
            if (await employee == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { $"El empleado asignado no existe - Id {model.EmployeeId}" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<CourseEmployee>.OnBuild(model, new CourseEmployee());

            _dbContext.CourseEmployees.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(entity)
            {
                Message = "Registro creado correctamente"
            };
        }


        public async Task<Response<bool>> DeleteByCourseId(List<string> ids, string courseid)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.CourseEmployees.Where(x => x.EmployeeId == item && x.CourseId == courseid).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");

                    }

                    _dbContext.CourseEmployees.Remove(response);
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


        public async Task<Response<object>> Update(string id, CourseEmployeeRequest model)
        {
            var response = await _dbContext.CourseEmployees.Where(x => x.CourseId == id && x.EmployeeId == model.EmployeeId).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<CourseEmployee>.OnBuild(model, response);
            _dbContext.CourseEmployees.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }


    }

}
