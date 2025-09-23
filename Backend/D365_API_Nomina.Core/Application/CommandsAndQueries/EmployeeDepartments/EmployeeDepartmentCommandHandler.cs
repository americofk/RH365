using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeDeparments;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeDepartments
{
    public interface IEmployeeDepartmentCommandHandler :
        ICreateCommandHandler<EmployeeDepartmentRequest>,
        IDeleteByParentCommandHandler,
        IUpdateCommandHandler<EmployeeDepartmentRequest>
    {
        public Task<Response<object>> UpdateStatus(string id, bool status, string employeeid);
    }

    public class EmployeeDepartmentCommandHandler : IEmployeeDepartmentCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeDepartmentCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> Create(EmployeeDepartmentRequest model)
        {
            var entity = BuildDtoHelper<EmployeeDepartment>.OnBuild(model, new EmployeeDepartment());

            _dbContext.EmployeeDepartments.Add(entity);
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
                    var response = await _dbContext.EmployeeDepartments.Where(x => x.DepartmentId == item).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");

                    }

                    _dbContext.EmployeeDepartments.Remove(response);
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


        public async Task<Response<object>> Update(string id, EmployeeDepartmentRequest model)
        {
            var response = await _dbContext.EmployeeDepartments.Where(x => x.EmployeeId == id && x.DepartmentId == model.DepartmentId).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<EmployeeDepartment>.OnBuild(model, response);
            _dbContext.EmployeeDepartments.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }


        public async Task<Response<object>> UpdateStatus(string id, bool status, string employeeid)
        {
            var response = await _dbContext.EmployeeDepartments.Where(x => x.EmployeeId == id && x.DepartmentId == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Message = "El registro seleccionado no existe"
                };
            }

            response.EmployeeDepartmentStatus = status;
            _dbContext.EmployeeDepartments.Update(response);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }
    }

}
