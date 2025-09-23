using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.CourseInstructors;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.CourseInstructors
{
    public interface ICourseInstructorCommandHandler :
        ICreateCommandHandler<CourseInstructorRequest>,
        IUpdateCommandHandler<CourseInstructorRequest>
    {
        public Task<Response<bool>> DeleteByCourseId(List<string> ids, string courseid);
    }

    public class CourseInstructorCommandHandler : ICourseInstructorCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public CourseInstructorCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> Create(CourseInstructorRequest model)
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

            var instructor = _dbContext.Instructors.Where(x => x.InstructorId == model.InstructorId).FirstOrDefaultAsync();
            if (await instructor == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { $"El instructor asignado no existe - Id {model.InstructorId}" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<CourseInstructor>.OnBuild(model, new CourseInstructor());

            _dbContext.CourseInstructors.Add(entity);
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
                    var response = await _dbContext.CourseInstructors.Where(x => x.InstructorId == item 
                                        && x.CourseId == courseid).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");

                    }

                    _dbContext.CourseInstructors.Remove(response);
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


        public async Task<Response<object>> Update(string id, CourseInstructorRequest model)
        {
            var response = await _dbContext.CourseInstructors.Where(x => x.CourseId == id && x.InstructorId == model.InstructorId).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<CourseInstructor>.OnBuild(model, response);
            _dbContext.CourseInstructors.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }

    }

}
