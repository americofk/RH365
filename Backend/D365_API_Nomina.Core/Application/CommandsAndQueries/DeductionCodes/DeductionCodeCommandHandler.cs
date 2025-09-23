using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.StoreServices.DeductionCodes
{
    public interface IDeductionCodeCommandHandler : 
        ICreateCommandHandler<DeductionCodeRequest>,
        IDeleteCommandHandler
    {
        public Task<Response<object>> UpdateStatus(string id, bool status);
        public Task<Response<object>> Update(string id, DeductionCodeRequest model, bool isVersion = false);
        public Task<Response<bool>> DeleteVersion(string id, int internalid);
    }

    public class DeductionCodeCommandHandler : IDeductionCodeCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public DeductionCodeCommandHandler(IApplicationDbContext _dbcontext)
        {
            _dbContext = _dbcontext;
        }

        public async Task<Response<object>> Create(DeductionCodeRequest _model)
        {
            var deductionCode = BuildDtoHelper<DeductionCode>
                    .OnBuild(_model, new DeductionCode());

            _dbContext.DeductionCodes.Add(deductionCode);
            await _dbContext.SaveChangesAsync();

            //Guardamos la versión
            var deductionCodeVersion = BuildDtoHelper<DeductionCodeVersion>
                                        .OnBuild(_model, new DeductionCodeVersion());

            deductionCodeVersion.DeductionCodeId = deductionCode.DeductionCodeId;
            _dbContext.DeductionCodeVersions.Add(deductionCodeVersion);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(deductionCode)
            {
                Message = "Registro creado correctamente"
            };
        }

        public async Task<Response<object>> Update(string id, DeductionCodeRequest model, bool isVersion = false)
        {
            string Message = string.Empty;

            var response = await _dbContext.DeductionCodes.Where(x => x.DeductionCodeId == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            //Validamos las fechas de inicio que suministran
            var validateversion = await _dbContext.DeductionCodeVersions.Where(x => x.DeductionCodeId == id)
                                                                      .OrderByDescending(x => x.ValidTo).Take(2).ToListAsync();

            if (validateversion == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe en la versión" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<DeductionCode>.OnBuild(model, response);

            //Preguntamos si es version para decidir si guardamos uno nuevo o modificamos
            if (isVersion) // true = guardamos uno nuevo
            {
                //Validamos con el primero de la lista
                if (validateversion[0].ValidFrom >= model.ValidFrom)
                {
                    return new Response<object>(false)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { $"La fecha de inicio suministrada no es válida para las versiones anteriores, la fecha debe ser mayor a {validateversion[0].ValidFrom}" },
                        StatusHttp = 404
                    };
                }

                //Actualizamos la versión anterior
                var entityversion = validateversion[0];
                entityversion.ValidTo = model.ValidFrom.AddDays(-1);
                _dbContext.DeductionCodeVersions.Update(entityversion);
                await _dbContext.SaveChangesAsync();

                //Creamos una nueva versión
                var newEntityVersion = BuildDtoHelper<DeductionCodeVersion>.OnBuild(entity, new DeductionCodeVersion());
                _dbContext.DeductionCodeVersions.Add(newEntityVersion);
                await _dbContext.SaveChangesAsync();

                Message = "Versión creada con éxito";
            }
            else
            {
                //Validamos con el segundo de la lista si es que existe
                if (validateversion.Count == 2)
                {
                    if (validateversion[1]?.ValidFrom >= model.ValidFrom)
                    {
                        return new Response<object>(false)
                        {
                            Succeeded = false,
                            Errors = new List<string>() { $"La fecha de fin suministrada no es válida para las versiones anteriores, la fecha debe ser mayor a {validateversion[1].ValidFrom}" },
                            StatusHttp = 404
                        };
                    }
                }

                var payrollprocess = await _dbContext.PayrollProcessActions
                        .Join(_dbContext.PayrollsProcess,
                                action => action.PayrollProcessId,
                                process => process.PayrollProcessId,
                                (action, process) => new { Action = action, Process = process })
                        .Where(x => x.Action.ActionId == id && (x.Action.PayrollActionType == PayrollActionType.Deduction || x.Action.PayrollActionType == PayrollActionType.Contribution)
                                && x.Process.PayrollProcessStatus != PayrollProcessStatus.Canceled
                                && (x.Process.PeriodEndDate >= model.ValidFrom && x.Process.PeriodEndDate <= model.ValidTo))
                        .FirstOrDefaultAsync();

                if (payrollprocess != null)
                {
                    return new Response<object>(string.Empty)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { $"No se puede editar el código de deducción porque está siendo usado en un proceso de nómina. Asegurese de cancelar los procesos de nómina de las fechas correspondientes al código de ganancia o de dejarlos en estado 'Procesado' antes de continuar." },
                        StatusHttp = 404
                    };
                }

                //Actualizamos la versión anterior
                if (validateversion.Count == 2)
                {
                    var entityversion = validateversion[1];
                    entityversion.ValidTo = model.ValidFrom.AddDays(-1);
                    _dbContext.DeductionCodeVersions.Update(entityversion);
                    await _dbContext.SaveChangesAsync();
                }

                //Actualizamos la versión actual
                var entityVersionUpdate = BuildDtoHelper<DeductionCodeVersion>.OnBuild(model, validateversion[0]);
                _dbContext.DeductionCodeVersions.Update(entityVersionUpdate);
                await _dbContext.SaveChangesAsync();

                Message = "Registro actualizado con éxito";
            }

            //Actualizamos la tabla principal
            _dbContext.DeductionCodes.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }

        public async Task<Response<bool>> Delete(List<string> ids)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.DeductionCodes.Where(x => x.DeductionCodeId == item).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");
                    }

                    var employeededuction = await _dbContext.EmployeeDeductionCodes.Where(x => x.DeductionCodeId == item).FirstOrDefaultAsync();

                    if (employeededuction != null)
                    {
                        throw new Exception($"El registro seleccionado no se puede eliminar porque está asignado a un empleado - id {item}");
                    }

                    _dbContext.DeductionCodes.Remove(response);
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

        public async Task<Response<object>> UpdateStatus(string id, bool status)
        {
            var response = await _dbContext.DeductionCodes.Where(x => x.DeductionCodeId == (string)id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Message = "El registro seleccionado no existe"
                };
            }

            response.DeductionStatus = status;
            _dbContext.DeductionCodes.Update(response);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }

        public async Task<Response<bool>> DeleteVersion(string id, int internalid)
        {
            var response = await _dbContext.DeductionCodeVersions.Where(x => x.DeductionCodeId == id && x.InternalId == internalid).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<bool>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { $"El registro seleccionado no existe - id {id.ToLower()} - {internalid}" },
                    StatusHttp = 404
                };
            }

            var versions = await _dbContext.DeductionCodeVersions.Where(x => x.DeductionCodeId == id).OrderByDescending(x => x.InternalId)
                                    .ToListAsync();

            if (versions.First()?.InternalId != internalid)
            {
                return new Response<bool>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { $"No se puede eliminar la versión porque existe una más reciente, por favor elimine las versiones más recientes antes de continuar." },
                    StatusHttp = 404
                };
            }

            if (versions.Count == 1)
            {
                return new Response<bool>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { $"No se puede eliminar la única versión disponible." },
                    StatusHttp = 404
                };
            }

            var payrollprocess = await _dbContext.PayrollProcessActions
                        .Join(_dbContext.PayrollsProcess,
                                action => action.PayrollProcessId,
                                process => process.PayrollProcessId,
                                (action, process) => new { Action = action, Process = process })
                        .Where(x => x.Action.ActionId == id && (x.Action.PayrollActionType == PayrollActionType.Deduction || x.Action.PayrollActionType == PayrollActionType.Contribution)
                                && x.Process.PayrollProcessStatus != PayrollProcessStatus.Canceled
                                && (x.Process.PeriodEndDate >= response.ValidFrom && x.Process.PeriodEndDate <= response.ValidTo))
                        .FirstOrDefaultAsync();

            if (payrollprocess != null)
            {
                return new Response<bool>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { $"No se puede eliminar la versión del código de ganancia porque está siendo usado en un proceso de nómina. Asegurese de cancelar los procesos de nómina de las fechas correspondientes al código de ganancia o de dejarlos en estado 'Procesado' antes de continuar." },
                    StatusHttp = 404
                };
            }

            _dbContext.DeductionCodeVersions.Remove(response);
            await _dbContext.SaveChangesAsync();

            //Actualizamos la version más reciente. El segundo de la lista.
            versions.Remove(versions.First());
            var entityVersionUpdate = versions.First();
            entityVersionUpdate.ValidTo = DateTime.ParseExact("31/12/2132", "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            _dbContext.DeductionCodeVersions.Update(entityVersionUpdate);
            await _dbContext.SaveChangesAsync();

            //Actualizamos la tabla principal
            var principalEntity = await _dbContext.DeductionCodes.Where(x => x.DeductionCodeId == id).FirstOrDefaultAsync();

            var entity = BuildDtoHelper<DeductionCode>.OnBuild(entityVersionUpdate, principalEntity);
            _dbContext.DeductionCodes.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<bool>(true) { Message = "Registros elimando con éxito" };
        }
    }
}
