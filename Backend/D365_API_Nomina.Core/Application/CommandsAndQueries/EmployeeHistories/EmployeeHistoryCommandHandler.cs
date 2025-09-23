using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeHistories;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeeHistories
{
    public interface IEmployeeHistoryCommandHandler : 
        IDeleteByParentCommandHandler<EmployeeHistoryDeleteRequest>,
        IUpdateCommandHandler<EmployeeHistoryUpdateRequest>
    {
        public Task<Response<object>> MarkIsForDGT(EmployeeHistoryIsForDGTRequest model);

    }

    public class EmployeeHistoryCommandHandler : IEmployeeHistoryCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public EmployeeHistoryCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<bool>> DeleteByParent(List<EmployeeHistoryDeleteRequest> ids, string parentid)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.EmployeeHistories.Where(x => x.EmployeeHistoryId == item.EmployeeHistoryId && x.EmployeeId == parentid).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");
                    }

                    _dbContext.EmployeeHistories.Remove(response);
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
                    Errors = new List<string>() { ex.Message },
                    StatusHttp = 404
                };
            }
        }

        public async Task<Response<object>> Update(string id, EmployeeHistoryUpdateRequest model)
        {
            var response = await _dbContext.EmployeeHistories.Where(x => x.EmployeeHistoryId == model.EmployeeHistoryId && x.EmployeeId == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = response;
            entity.RegisterDate = model.RegisterDate;

            _dbContext.EmployeeHistories.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }

        public async Task<Response<object>> MarkIsForDGT(EmployeeHistoryIsForDGTRequest model)
        {
            var response = await _dbContext.EmployeeHistories.Where(x => x.EmployeeHistoryId == model.EmployeeHistoryId).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = response;
            entity.IsUseDGT = model.IsUseDGT;

            _dbContext.EmployeeHistories.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }
    }
}
