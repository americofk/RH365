using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeePositions;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeePositions
{
    public interface IEmployeePositionCommandHandler :
        ICreateCommandHandler<EmployeePositionRequest>,
        IDeleteByParentCommandHandler,
        IUpdateCommandHandler<EmployeePositionRequestUpdate>
    {
        public Task<Response<object>> UpdateStatus(EmployeePositionStatusRequest model);
    }

    public class EmployeePositionCommandHandler : IEmployeePositionCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeePositionCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> Create(EmployeePositionRequest model)
        {
            Employee employee;

            if (model.GetCallerName() == null)
            {
                employee = await _dbContext.Employees.Where(x => x.EmployeeId == model.EmployeeId
                                                            && x.WorkStatus == Domain.Enums.WorkStatus.Employ).FirstOrDefaultAsync();                
            }
            else
            {
                employee = await _dbContext.Employees.Where(x => x.EmployeeId == model.EmployeeId
                                                            && x.WorkStatus == Domain.Enums.WorkStatus.Candidate ||
                                                            x.WorkStatus == Domain.Enums.WorkStatus.Dismissed).FirstOrDefaultAsync();
            }

            if (employee == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El empleado no éxiste o no está en estado contratado para asignar un puesto" },
                    StatusHttp = 404
                };
            }


            var response = await _dbContext.EmployeePositions.Where(x => x.EmployeeId == model.EmployeeId 
                                                                    && x.PositionId == model.PositionId).FirstOrDefaultAsync();

            if(response != null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "La posición seleccionada ya existe" },
                    StatusHttp = 404
                };
            }

            response = await _dbContext.EmployeePositions.Where(x => x.EmployeeId == model.EmployeeId
                                                                && (x.ToDate > model.FromDate || x.EmployeePositionStatus == true)).FirstOrDefaultAsync();

            if (response != null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { $"Todavía existe un puesto vigente, deshabilitelo antes de agregar otro puesto. Puesto vigente - {response.PositionId}" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<EmployeePosition>.OnBuild(model, new EmployeePosition());

            _dbContext.EmployeePositions.Add(entity);
            await _dbContext.SaveChangesAsync();

            //Actualizamos la fecha de empleo final del empleado
            var entityUpdate = employee;
            entityUpdate.EndWorkDate = model.ToDate;

            _dbContext.Employees.Update(entityUpdate);
            await _dbContext.SaveChangesAsync();



            //Se guarda la información en la tabla del historial
            var employeehistory = new EmployeeHistory()
            {
                Type = "NC",
                Description = $"Se añadió la posición {model.PositionId}",
                RegisterDate = model.FromDate,
                EmployeeId = model.EmployeeId
            };

            _dbContext.EmployeeHistories.Add(employeehistory);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(entity)
            {
                Message = "Registro creado correctamente"
            };
        }


        public async Task<Response<bool>> DeleteByParent(List<string> ids, string employeeid)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.EmployeePositions.Where(x => x.PositionId == item && x.EmployeeId == employeeid).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");

                    }

                    _dbContext.EmployeePositions.Remove(response);
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


        public async Task<Response<object>> Update(string id, EmployeePositionRequestUpdate model)
        {
            var response = await _dbContext.EmployeePositions.Where(x => x.EmployeeId == id && x.PositionId == model.PositionId && x.EmployeePositionStatus == true).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe o no está vigente, si no está vigente no se puede editar" },
                    StatusHttp = 404
                };
            }

            if (response.FromDate > model.ToDate)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "La fecha desde no puede ser mayor que la fecha hasta" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<EmployeePosition>.OnBuild(model, response);
            _dbContext.EmployeePositions.Update(entity);
            await _dbContext.SaveChangesAsync();


            //Actualizamos la fecha de empleo final del empleado
            var employee = await _dbContext.Employees.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();
            var entityUpdate = employee;
            entityUpdate.EndWorkDate = model.ToDate;

            _dbContext.Employees.Update(entityUpdate);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }


        public async Task<Response<object>> UpdateStatus(EmployeePositionStatusRequest model)
        {
            var response = await _dbContext.EmployeePositions.Where(x => x.PositionId == model.PositionId && x.EmployeeId == model.EmployeeId && x.EmployeePositionStatus == true).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe o no está vigente" },
                    StatusHttp = 404
                };
            }

            if (response.FromDate > model.ToDate)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "La fecha de vencimiento no puede ser menor a la fecha de inicio" },
                    StatusHttp = 404
                };
            }

            response.EmployeePositionStatus = false;
            response.ToDate = model.ToDate;

            _dbContext.EmployeePositions.Update(response);
            await _dbContext.SaveChangesAsync();

            //Actualizamos la fecha de empleo final del empleado
            var employee = await _dbContext.Employees.Where(x => x.EmployeeId == model.EmployeeId).FirstOrDefaultAsync();
            var entityUpdate = employee;
            entityUpdate.EndWorkDate = model.ToDate;

            _dbContext.Employees.Update(entityUpdate);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }

    }

}
