using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeAddress;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeesAddress
{
    public interface IEmployeeAddressCommandHandler :
        ICreateCommandHandler<EmployeeAddressRequest>,
        IUpdateCommandHandler<EmployeeAddressRequest>
    {
        public Task<Response<bool>> DeleteByEmployeeId(List<string> ids, string employeeid);

    }

    public class EmployeeAddressCommandHandler : IEmployeeAddressCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeAddressCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<Response<object>> Create(EmployeeAddressRequest model)
        {
            EmployeeAddress principalEntity = null;

            var address = await _dbContext.EmployeesAddress.Where(x => x.EmployeeId == model.EmployeeId).OrderByDescending(x =>x.InternalId).FirstOrDefaultAsync();
            var entity = BuildDtoHelper<EmployeeAddress>.OnBuild(model, new EmployeeAddress());

            entity.InternalId = address == null ? 1 : address.InternalId + 1;

            if(model.IsPrincipal)
            {
                principalEntity = await _dbContext.EmployeesAddress.Where(x => x.EmployeeId == model.EmployeeId
                                                                           && x.IsPrincipal == true).FirstOrDefaultAsync();
            }

            var province = await _dbContext.Provinces.Where(x => x.ProvinceId == model.Province).FirstOrDefaultAsync();

            if (province == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "La provincia no existe" },
                    StatusHttp = (int)HttpStatusCode.NotFound
                };
            }

            entity.ProvinceName = province.Name;

            //Guardo la nueva dirección
            _dbContext.EmployeesAddress.Add(entity);
            await _dbContext.SaveChangesAsync();

            //Actualizo la entidad que era principal
            if(principalEntity != null)
            {
                principalEntity.IsPrincipal = false;
                _dbContext.EmployeesAddress.Update(principalEntity);
                await _dbContext.SaveChangesAsync();
            }

            return new Response<object>(entity)
            {
                Message = "Registro creado correctamente"
            };
        }


        public async Task<Response<bool>> DeleteByEmployeeId(List<string> ids, string employeeid)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.EmployeesAddress.Where(x => x.EmployeeId == employeeid 
                                            && x.InternalId == int.Parse(item)).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");

                    }

                    _dbContext.EmployeesAddress.Remove(response);
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


        public async Task<Response<object>> Update(string id, EmployeeAddressRequest model)
        {
            EmployeeAddress principalEntity = null;

            var response = await _dbContext.EmployeesAddress.Where(x => x.InternalId == int.Parse(id)
                                    && x.EmployeeId == model.EmployeeId).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = (int)HttpStatusCode.NotFound
                };
            }

            if (model.IsPrincipal)
            {
                principalEntity = await _dbContext.EmployeesAddress.Where(x => x.EmployeeId == model.EmployeeId
                                                                           && x.IsPrincipal == true).FirstOrDefaultAsync();
            }            

            var entity = BuildDtoHelper<EmployeeAddress>.OnBuild(model, response);

            var province = await _dbContext.Provinces.Where(x => x.ProvinceId == model.Province).FirstOrDefaultAsync();

            if (province == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "La provincia no existe" },
                    StatusHttp = (int)HttpStatusCode.NotFound
                };
            }
            entity.ProvinceName = province.Name;

            _dbContext.EmployeesAddress.Update(entity);
            await _dbContext.SaveChangesAsync();

            //Actualizo la entidad que era principal
            if (principalEntity != null)
            {
                principalEntity.IsPrincipal = false;
                _dbContext.EmployeesAddress.Update(principalEntity);
                await _dbContext.SaveChangesAsync();
            }

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }
    }

}
