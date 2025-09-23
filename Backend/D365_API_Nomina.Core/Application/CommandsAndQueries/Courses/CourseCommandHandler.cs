using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.Course;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Courses
{
    public interface ICourseCommandHandler :
        ICreateCommandHandler<CourseRequest>,
        IDeleteCommandHandler,
        IUpdateCommandHandler<CourseRequest>
    {

    }

    public class CourseCommandHandler : ICourseCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public CourseCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> Create(CourseRequest model)
        {
            if(!string.IsNullOrEmpty(model.CourseParentId))
            {
                var courseParentId =  _dbContext.Courses.Where(x => x.CourseId == model.CourseParentId).FirstOrDefaultAsync();

                if(await courseParentId == null)
                {
                    return new Response<object>(false)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { "El curso padre seleccionado no existe" },
                        StatusHttp = 404
                    };
                }
            }

            var classRoomId = _dbContext.ClassRooms.Where(x => x.ClassRoomId == model.ClassRoomId).FirstOrDefaultAsync();

            if (await classRoomId == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El salón de curso seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var courseType = _dbContext.CourseTypes.Where(x => x.CourseTypeId == model.CourseTypeId).FirstOrDefaultAsync();

            if (await courseType == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El tipo de curso seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<Course>.OnBuild(model, new Course());

            _dbContext.Courses.Add(entity);
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
                    var response = await _dbContext.Courses.Where(x => x.CourseId == item 
                                            && x.CourseStatus == Domain.Enums.CourseStatus.Created).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe o su estado es distinto de creado- id {item}");

                    }

                    _dbContext.Courses.Remove(response);
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


        public async Task<Response<object>> Update(string id, CourseRequest model)
        {
            var response = await _dbContext.Courses.Where(x => x.CourseId == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<Course>.OnBuild(model, response);
            _dbContext.Courses.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }
    }

}
