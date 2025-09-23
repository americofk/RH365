using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.Batchs;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeBankAccounts;
using D365_API_Nomina.Core.Application.Common.Model.EmployeeWorkCalendars;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Batchs
{
    public interface IImportBatchDataCommandHandler : IDeleteCommandHandler
    {
        public Task<Response<object>> Create(List<BatchEmployeeRequest> models, string partialRoute, string[] session);
        public Task<Response<object>> Create(List<BatchAddressEmployeeRequest> models, string partialRoute, string[] session);
        public Task<Response<object>> Create(List<BatchContactInfoEmployeeRequest> models, string partialRoute, string[] session);
        public Task<Response<object>> Create(List<BatchBankAccountEmployeeRequest> models, string partialRoute, string[] session);
        public Task<Response<object>> Create(List<BatchDocumentEmployeeRequest> models, string partialRoute, string[] session);
        public Task<Response<object>> Create(List<BatchTaxEmployeeRequest> models, string partialRoute, string[] session);
        public Task<Response<object>> Create(List<BatchExtraHoursEmployeeRequest> models, string partialRoute, string[] session);
        public Task<Response<object>> Create(List<BatchEarningCodeEmployeeRequest> models, string partialRoute, string[] session);
        public Task<Response<object>> Create(List<BatchLoanEmployeeRequest> models, string partialRoute, string[] session);
        public Task<Response<object>> Create(List<BatchDeductionCodeEmployeeRequest> models, string partialRoute, string[] session);
        public Task<Response<object>> Create(List<BatchCourseRequest> models, string partialRoute, string[] session);
        public Task<Response<object>> Create(List<BatchEmployeeWorkCalendarRequest> models, string partialRoute, string[] session);
        public Task<Response<object>> Create(List<BatchEmployeeWorkControlCalendarRequest> models, string partialRoute, string[] session);
    }

    public class ImportBatchDataCommandHandler : IImportBatchDataCommandHandler
    {
        private IApplicationDbContext _NewDbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IApplicationDbContext _dbContext;

        public ImportBatchDataCommandHandler(IServiceScopeFactory serviceScopeFactory, IApplicationDbContext dbContext)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _dbContext = dbContext;
        }


        private async Task<int> CreateHistory(BatchEntity entity, string[] session)
        {
            var newEntity = new BatchHistory()
            {
                BatchEntity = entity,
                StartDate = DateTime.Now,

                DataareaID = session[0],
                CreatedBy = session[1],
                CreatedOn = DateTime.Now
            };

            _NewDbContext.BatchHistories.Add(newEntity);
            await _NewDbContext.SaveChangesAsync();

            return newEntity.InternalId;
        }

        private async Task UpdateHistory(int internalid, bool iserror, string information, string[] session)
        {
            var response = await _NewDbContext.BatchHistories.Where(x => x.InternalId == internalid
                                                                    && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

            response.IsError = iserror;
            response.Information = information;
            response.IsFinished = true;
            response.EndDate = DateTime.Now;

            _NewDbContext.BatchHistories.Update(response);
            await _NewDbContext.SaveChangesAsync();
        }

        //Empleados prospectos o contratados
        public async Task<Response<object>> Create(List<BatchEmployeeRequest> models, string partialRoute, string[] session)
        {
            bool isError = false;
            int internalid = 0;
            string information = string.Empty;
            int count = 0;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _NewDbContext = scope.ServiceProvider.GetService<IApplicationDbContext>();

                internalid = await CreateHistory(BatchEntity.Employee, session);

                using var transaction = _NewDbContext.Database.BeginTransaction();

                try
                {
                    foreach (var item in models)
                    {
                        count++;
                        var country = await _NewDbContext.Countries.Where(x => x.CountryId == item.Country).FirstOrDefaultAsync();

                        if (country == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.Name} - El país selecionado no existe. |";
                        }

                        if (!string.IsNullOrEmpty(item.PositionId))
                        {
                            var position = await _NewDbContext.Positions.Where(x => x.PositionId == item.PositionId && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                            if (position == null)
                            {
                                isError = true;
                                information += $"Empleado: {item.Name} - La posición seleccionada no existe: {item.PositionId}. |";
                            }
                        }

                        if (!isError)
                        {
                            var entity = BuildDtoHelper<Employee>.OnBuild(item, new Employee());

                            entity.DataareaID = session[0];
                            entity.CreatedBy = session[1];
                            entity.CreatedOn = DateTime.Now;
                            entity.Nationality = country.NationalityCode;

                            if (!string.IsNullOrEmpty(item.PositionId))
                            {
                                entity.WorkStatus = WorkStatus.Employ;
                                entity.StartWorkDate = item.PositionFromDate;
                                entity.EndWorkDate = item.PositionToDate;
                            }

                            _NewDbContext.Employees.Add(entity);
                            await _NewDbContext.SaveChangesAsync();



                            if (!string.IsNullOrEmpty(item.PositionId))
                            {
                                //Sección posición de empleado
                                var employeeposition = new EmployeePosition()
                                {
                                    PositionId = item.PositionId,
                                    ToDate = item.PositionToDate,
                                    FromDate = item.PositionFromDate,
                                    EmployeeId = entity.EmployeeId,
                                    DataareaID = session[0],
                                    CreatedBy = session[1],
                                    CreatedOn = DateTime.Now
                                };

                                _NewDbContext.EmployeePositions.Add(employeeposition);
                                await _NewDbContext.SaveChangesAsync();


                                //Se guarda la información en la tabla del historial
                                var employeehistory = new EmployeeHistory()
                                {
                                    Type = "NI",
                                    Description = "Empleado contratado",
                                    RegisterDate = item.PositionFromDate,
                                    EmployeeId = entity.EmployeeId
                                };

                                _NewDbContext.EmployeeHistories.Add(employeehistory);
                                await _NewDbContext.SaveChangesAsync();
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(information))
                    {
                        information = $"Se crearon {count} registro/s correctamente";
                    }

                    await UpdateHistory(internalid, isError, information, session);

                    transaction.Commit();

                    return new Response<object>()
                    {
                        Succeeded = true,
                        Message = "Operacion completada, vea la información para saber los detalles de la solicitud",
                        StatusHttp = 200,
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    await UpdateHistory(internalid, true, ex.Message, session);

                    string path = Path.Combine(partialRoute, @$"BatchLog\EmployeeProcess-{internalid}.txt");
                    using (FileStream fs = File.Create(path))
                    {
                        byte[] content = new UTF8Encoding(true).GetBytes(ex.Message);
                        fs.Write(content, 0, content.Length);
                    }

                    return new Response<object>(string.Empty)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { ex.Message },
                        StatusHttp = 404
                    };
                }
            }
        }

        //Direcciones de empleados
        public async Task<Response<object>> Create(List<BatchAddressEmployeeRequest> models, string partialRoute, string[] session)
        {
            bool isError = false;
            int internalid = 0;
            string information = string.Empty;
            int count = 0;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _NewDbContext = scope.ServiceProvider.GetService<IApplicationDbContext>();

                internalid = await CreateHistory(BatchEntity.EmployeeAdress, session);

                using var transaction = _NewDbContext.Database.BeginTransaction();

                try
                {
                    foreach (var item in models)
                    {
                        EmployeeAddress principalEntity = null;

                        count++;

                        //Validaciones
                        var response = await _NewDbContext.Employees.Where(x => x.EmployeeId == item.EmployeeId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();
                        var Province = await _NewDbContext.Provinces.Where(x => x.ProvinceId == item.Province).FirstOrDefaultAsync();

                        if (Province == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de provincia asignado no existe. |";
                        }
                        else
                        {
                            item.ProvinceName = Province.Name;
                        }

                        if (response == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de empleado asignado no existe. |";
                        }

                        var country = await _NewDbContext.Countries.Where(x => x.CountryId == item.CountryId).FirstOrDefaultAsync();

                        if (country == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El país selecionado no existe. |";
                        }
                        //Validaciones                        

                        if (!isError)
                        {
                            var address = await _NewDbContext.EmployeesAddress.Where(x => x.EmployeeId == item.EmployeeId
                                                                                     && x.DataareaID == session[0]).IgnoreQueryFilters().OrderByDescending(x => x.InternalId).FirstOrDefaultAsync();

                            var entity = BuildDtoHelper<EmployeeAddress>.OnBuild(item, new EmployeeAddress());

                            entity.InternalId = address == null ? 1 : address.InternalId + 1;

                            if (entity.IsPrincipal)
                            {
                                principalEntity = await _NewDbContext.EmployeesAddress.Where(x => x.EmployeeId == item.EmployeeId
                                                                                             && x.IsPrincipal == true
                                                                                             && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();
                            }

                            //Guardo la nueva dirección
                            entity.DataareaID = session[0];
                            entity.CreatedBy = session[1];
                            entity.CreatedOn = DateTime.Now;

                            _NewDbContext.EmployeesAddress.Add(entity);
                            await _NewDbContext.SaveChangesAsync();

                            //Actualizo la entidad que era principal
                            if (principalEntity != null)
                            {
                                principalEntity.IsPrincipal = false;
                                _NewDbContext.EmployeesAddress.Update(principalEntity);
                                await _NewDbContext.SaveChangesAsync();
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(information))
                    {
                        information = $"Se crearon {count} registro/s correctamente";
                    }

                    await UpdateHistory(internalid, isError, information, session);

                    transaction.Commit();

                    return new Response<object>()
                    {
                        Succeeded = true,
                        Message = "Operacion completada, vea la información para saber los detalles de la solicitud",
                        StatusHttp = 200,
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    await UpdateHistory(internalid, true, ex.Message, session);

                    string path = Path.Combine(partialRoute, @$"BatchLog\EmployeeAddressProcess-{internalid}.txt");
                    using (FileStream fs = File.Create(path))
                    {
                        byte[] content = new UTF8Encoding(true).GetBytes(ex.Message);
                        fs.Write(content, 0, content.Length);
                    }

                    return new Response<object>(string.Empty)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { ex.Message },
                        StatusHttp = 404
                    };
                }
            }
        }

        //Contactos de empleados
        public async Task<Response<object>> Create(List<BatchContactInfoEmployeeRequest> models, string partialRoute, string[] session)
        {
            bool isError = false;
            int internalid = 0;
            string information = string.Empty;
            int count = 0;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _NewDbContext = scope.ServiceProvider.GetService<IApplicationDbContext>();

                internalid = await CreateHistory(BatchEntity.EmployeeContactInfo, session);

                using var transaction = _NewDbContext.Database.BeginTransaction();

                try
                {
                    foreach (var item in models)
                    {
                        EmployeeContactInf principalEntity = null;

                        count++;

                        //Validaciones
                        var response = await _NewDbContext.Employees.Where(x => x.EmployeeId == item.EmployeeId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (response == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de empleado asignado no existe. |";
                        }
                        //Validaciones                        

                        if (!isError)
                        {
                            var contactinf = await _NewDbContext.EmployeeContactsInf.Where(x => x.EmployeeId == item.EmployeeId && x.DataareaID == session[0])
                                                                                    .IgnoreQueryFilters()
                                                                                    .OrderByDescending(x => x.InternalId).FirstOrDefaultAsync();

                            var entity = BuildDtoHelper<EmployeeContactInf>.OnBuild(item, new EmployeeContactInf());

                            entity.InternalId = contactinf == null ? 1 : contactinf.InternalId + 1;

                            if (item.IsPrincipal)
                            {
                                principalEntity = await _NewDbContext.EmployeeContactsInf.Where(x => x.EmployeeId == item.EmployeeId
                                                                                               && x.IsPrincipal == true
                                                                                               && x.ContactType == item.ContactType
                                                                                               && x.DataareaID == session[0])
                                                                                         .IgnoreQueryFilters()
                                                                                         .FirstOrDefaultAsync();
                            }

                            //Guardo la nueva dirección
                            entity.DataareaID = session[0];
                            entity.CreatedBy = session[1];
                            entity.CreatedOn = DateTime.Now;

                            _NewDbContext.EmployeeContactsInf.Add(entity);
                            await _NewDbContext.SaveChangesAsync();

                            //Actualizo la entidad que era principal
                            if (principalEntity != null)
                            {
                                principalEntity.IsPrincipal = false;
                                _NewDbContext.EmployeeContactsInf.Update(principalEntity);
                                await _NewDbContext.SaveChangesAsync();
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(information))
                    {
                        information = $"Se crearon {count} registro/s correctamente";
                    }

                    await UpdateHistory(internalid, isError, information, session);

                    transaction.Commit();

                    return new Response<object>()
                    {
                        Succeeded = true,
                        Message = "Operacion completada, vea la información para saber los detalles de la solicitud",
                        StatusHttp = 200,
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    await UpdateHistory(internalid, true, ex.Message, session);

                    string path = Path.Combine(partialRoute, @$"BatchLog\EmployeeContctInfoProcess-{internalid}.txt");
                    using (FileStream fs = File.Create(path))
                    {
                        byte[] content = new UTF8Encoding(true).GetBytes(ex.Message);
                        fs.Write(content, 0, content.Length);
                    }

                    return new Response<object>(string.Empty)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { ex.Message },
                        StatusHttp = 404
                    };
                }
            }
        }

        //Bancos de empleados
        public async Task<Response<object>> Create(List<BatchBankAccountEmployeeRequest> models, string partialRoute, string[] session)
        {
            bool isError = false;
            int internalid = 0;
            string information = string.Empty;
            int count = 0;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _NewDbContext = scope.ServiceProvider.GetService<IApplicationDbContext>();

                internalid = await CreateHistory(BatchEntity.EmployeeBanks, session);

                using var transaction = _NewDbContext.Database.BeginTransaction();

                try
                {
                    foreach (var item in models)
                    {
                        EmployeeBankAccount principalEntity = null;

                        count++;

                        //Validaciones
                        var response = await _NewDbContext.Employees.Where(x => x.EmployeeId == item.EmployeeId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (response == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de empleado asignado no existe. |";
                        }
                        //Validaciones                        

                        if (!isError)
                        {
                            var contactinf = await _NewDbContext.EmployeeBankAccounts.Where(x => x.EmployeeId == item.EmployeeId && x.DataareaID == session[0])
                                                                                    .IgnoreQueryFilters()
                                                                                    .OrderByDescending(x => x.InternalId).FirstOrDefaultAsync();

                            var entity = BuildDtoHelper<EmployeeBankAccount>.OnBuild(item, new EmployeeBankAccount());

                            entity.InternalId = contactinf == null ? 1 : contactinf.InternalId + 1;

                            if (item.IsPrincipal)
                            {
                                principalEntity = await _NewDbContext.EmployeeBankAccounts.Where(x => x.EmployeeId == item.EmployeeId
                                                                                               && x.IsPrincipal == true
                                                                                               && x.AccountType == item.AccountType
                                                                                               && x.DataareaID == session[0])
                                                                                         .IgnoreQueryFilters()
                                                                                         .FirstOrDefaultAsync();
                            }

                            //Guardo la nueva dirección
                            entity.DataareaID = session[0];
                            entity.CreatedBy = session[1];
                            entity.CreatedOn = DateTime.Now;

                            _NewDbContext.EmployeeBankAccounts.Add(entity);
                            await _NewDbContext.SaveChangesAsync();

                            //Actualizo la entidad que era principal
                            if (principalEntity != null)
                            {
                                principalEntity.IsPrincipal = false;
                                _NewDbContext.EmployeeBankAccounts.Update(principalEntity);
                                await _NewDbContext.SaveChangesAsync();
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(information))
                    {
                        information = $"Se crearon {count} registro/s correctamente";
                    }

                    await UpdateHistory(internalid, isError, information, session);

                    transaction.Commit();

                    return new Response<object>()
                    {
                        Succeeded = true,
                        Message = "Operacion completada, vea la información para saber los detalles de la solicitud",
                        StatusHttp = 200,
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    await UpdateHistory(internalid, true, ex.Message, session);

                    string path = Path.Combine(partialRoute, @$"BatchLog\EmployeeBankAccountProcess-{internalid}.txt");
                    using (FileStream fs = File.Create(path))
                    {
                        byte[] content = new UTF8Encoding(true).GetBytes(ex.Message);
                        fs.Write(content, 0, content.Length);
                    }

                    return new Response<object>(string.Empty)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { ex.Message },
                        StatusHttp = 404
                    };
                }
            }
        }

        //Documentos de empleados
        public async Task<Response<object>> Create(List<BatchDocumentEmployeeRequest> models, string partialRoute, string[] session)
        {
            bool isError = false;
            int internalid = 0;
            string information = string.Empty;
            int count = 0;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _NewDbContext = scope.ServiceProvider.GetService<IApplicationDbContext>();

                internalid = await CreateHistory(BatchEntity.EmployeeDocument, session);

                using var transaction = _NewDbContext.Database.BeginTransaction();

                try
                {
                    foreach (var item in models)
                    {
                        EmployeeDocument principalEntity = null;

                        count++;

                        //Validaciones
                        var response = await _NewDbContext.Employees.Where(x => x.EmployeeId == item.EmployeeId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (response == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de empleado asignado no existe. |";
                        }
                        //Validaciones                        

                        if (!isError)
                        {
                            var document = await _NewDbContext.EmployeeDocuments.Where(x => x.EmployeeId == item.EmployeeId && x.DataareaID == session[0])
                                                                                .IgnoreQueryFilters()
                                                                                .OrderByDescending(x => x.InternalId)
                                                                                .FirstOrDefaultAsync();

                            var entity = BuildDtoHelper<EmployeeDocument>.OnBuild(item, new EmployeeDocument());

                            entity.InternalId = document == null ? 1 : document.InternalId + 1;

                            if (item.IsPrincipal)
                            {
                                principalEntity = await _NewDbContext.EmployeeDocuments.Where(x => x.EmployeeId == item.EmployeeId
                                                                                               && x.IsPrincipal == true
                                                                                               && x.DocumentType == item.DocumentType
                                                                                               && x.DataareaID == session[0])
                                                                                        .IgnoreQueryFilters()
                                                                                        .FirstOrDefaultAsync();
                            }

                            //Guardo la nueva dirección
                            entity.DataareaID = session[0];
                            entity.CreatedBy = session[1];
                            entity.CreatedOn = DateTime.Now;

                            _NewDbContext.EmployeeDocuments.Add(entity);
                            await _NewDbContext.SaveChangesAsync();

                            //Actualizo la entidad que era principal
                            if (principalEntity != null)
                            {
                                principalEntity.IsPrincipal = false;
                                _NewDbContext.EmployeeDocuments.Update(principalEntity);
                                await _NewDbContext.SaveChangesAsync();
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(information))
                    {
                        information = $"Se crearon {count} registro/s correctamente";
                    }

                    await UpdateHistory(internalid, isError, information, session);

                    transaction.Commit();

                    return new Response<object>()
                    {
                        Succeeded = true,
                        Message = "Operacion completada, vea la información para saber los detalles de la solicitud",
                        StatusHttp = 200,
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    await UpdateHistory(internalid, true, ex.Message, session);

                    string path = Path.Combine(partialRoute, @$"BatchLog\EmployeeDocumentProcess-{internalid}.txt");
                    using (FileStream fs = File.Create(path))
                    {
                        byte[] content = new UTF8Encoding(true).GetBytes(ex.Message);
                        fs.Write(content, 0, content.Length);
                    }

                    return new Response<object>(string.Empty)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { ex.Message },
                        StatusHttp = 404
                    };
                }
            }
        }

        //Códigos de impuesto
        public async Task<Response<object>> Create(List<BatchTaxEmployeeRequest> models, string partialRoute, string[] session)
        {
            bool isError = false;
            int internalid = 0;
            string information = string.Empty;
            int count = 0;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _NewDbContext = scope.ServiceProvider.GetService<IApplicationDbContext>();

                internalid = await CreateHistory(BatchEntity.EmployeeTax, session);

                using var transaction = _NewDbContext.Database.BeginTransaction();

                try
                {
                    foreach (var item in models)
                    {
                        count++;

                        //Validaciones
                        var employee = await _NewDbContext.Employees.Where(x => x.EmployeeId == item.EmployeeId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (employee == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de empleado asignado no existe. |";
                        }

                        var tax = await _NewDbContext.Taxes.Where(x => x.TaxId == item.TaxId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (tax == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de impuesto no existe. |";
                        }

                        var payroll = await _NewDbContext.Payrolls.Where(x => x.PayrollId == item.PayrollId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (payroll == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de nómina no existe. |";
                        }

                        if (!isError)
                        {
                            var existTaxEmployee = await _NewDbContext.EmployeeTaxes.Where(x => x.TaxId == item.TaxId
                                                                               && x.EmployeeId == item.EmployeeId
                                                                               && x.PayrollId == item.PayrollId
                                                                               && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                            if (existTaxEmployee != null)
                            {
                                isError = true;
                                information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de impuesto ({item.TaxId}) ya fue asignado a esa nómina ({item.PayrollId}). |";
                            }
                        }
                        //Validaciones                       


                        if (!isError)
                        {
                            var entity = BuildDtoHelper<EmployeeTax>.OnBuild(item, new EmployeeTax());

                            //Guardo el nuevo código de impuesto
                            entity.DataareaID = session[0];
                            entity.CreatedBy = session[1];
                            entity.CreatedOn = DateTime.Now;

                            _NewDbContext.EmployeeTaxes.Add(entity);
                            await _NewDbContext.SaveChangesAsync();
                        }
                    }

                    if (string.IsNullOrEmpty(information))
                    {
                        information = $"Se crearon {count} registro/s correctamente";
                    }

                    await UpdateHistory(internalid, isError, information, session);

                    transaction.Commit();

                    return new Response<object>()
                    {
                        Succeeded = true,
                        Message = "Operacion completada, vea la información para saber los detalles de la solicitud",
                        StatusHttp = 200,
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    await UpdateHistory(internalid, true, ex.Message, session);

                    string path = Path.Combine(partialRoute, @$"BatchLog\EmployeeDocumentProcess-{internalid}.txt");
                    using (FileStream fs = File.Create(path))
                    {
                        byte[] content = new UTF8Encoding(true).GetBytes(ex.Message);
                        fs.Write(content, 0, content.Length);
                    }

                    return new Response<object>(string.Empty)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { ex.Message },
                        StatusHttp = 404
                    };
                }
            }
        }

        //Horas Extras
        public async Task<Response<object>> Create(List<BatchExtraHoursEmployeeRequest> models, string partialRoute, string[] session)
        {
            bool isError = false;
            int internalid = 0;
            string information = string.Empty;
            int count = 0;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _NewDbContext = scope.ServiceProvider.GetService<IApplicationDbContext>();

                internalid = await CreateHistory(BatchEntity.EmployeeExtraHours, session);

                using var transaction = _NewDbContext.Database.BeginTransaction();

                try
                {
                    foreach (var item in models)
                    {
                        count++;

                        //Validaciones
                        var employee = await _NewDbContext.Employees.Where(x => x.EmployeeId == item.EmployeeId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (employee == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de empleado asignado no existe. |";
                        }

                        var earningcode = await _NewDbContext.EarningCodeVersions.Where(x => x.EarningCodeId == item.EarningCodeId
                                                                                        && x.IndexBase == IndexBase.Hour
                                                                                        && x.MultiplyAmount != 0
                                                                                        && x.ValidFrom <= item.WorkedDay && x.ValidTo >= item.WorkedDay
                                                                                        && x.DataareaID == session[0])
                                                                                 .IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (earningcode == null)
                        {
                            return new Response<object>(null)
                            {
                                Succeeded = false,
                                Errors = new List<string>() { $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de hora extra asignado no existe o no tiene configurado el porcentaje a aplicar." },
                                StatusHttp = 404
                            };
                        }

                        var payroll = await _NewDbContext.Payrolls.Where(x => x.PayrollId == item.PayrollId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (payroll == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de nómina no existe. |";
                        }

                        if (!isError)
                        {
                            var existExtraHoursEmployee = await _NewDbContext.EmployeeExtraHours.Where(x => x.EmployeeId == item.EmployeeId
                                                                     && x.WorkedDay == item.WorkedDay
                                                                     && x.EarningCodeId == item.EarningCodeId
                                                                     && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                            if (existExtraHoursEmployee != null)
                            {
                                isError = true;
                                information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de hora extra ({item.EarningCodeId}) ya fue asignado a la nómina ({item.PayrollId}). |";
                            }
                        }
                        //Validaciones                       


                        if (!isError)
                        {
                            var salary = await _NewDbContext.EmployeeEarningCodes
                                .Join(_NewDbContext.EarningCodes,
                                    eec => eec.EarningCodeId,
                                    ec => ec.EarningCodeId,
                                    (eec, ec) => new { Eec = eec, Ec = ec })
                                .Where(x => x.Eec.EmployeeId == item.EmployeeId
                                    && x.Ec.IsExtraHours == true
                                    && x.Ec.DataareaID == session[0])
                                .Select(x => new
                                {
                                    Amount = x.Eec.IndexEarningMonthly
                                })
                                .ToListAsync();

                            if (salary != null)
                            {
                                var entity = BuildDtoHelper<EmployeeExtraHour>.OnBuild(item, new EmployeeExtraHour());
                                entity.Indice = earningcode.MultiplyAmount / 100;
                                if (salary != null)
                                {
                                    decimal constans = 23.83M;
                                    entity.Amount = ((salary.Sum(x => x.Amount) / constans) / 8 * entity.Indice) * item.Quantity;
                                }
                                //Guardo el nuevo código de horas extras
                                entity.DataareaID = session[0];
                                entity.CreatedBy = session[1];
                                entity.CreatedOn = DateTime.Now;

                                _NewDbContext.EmployeeExtraHours.Add(entity);
                                await _NewDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - Empleado no tiene salario del cual calcular las horas extras. |";
                            }

                        }
                    }

                    if (string.IsNullOrEmpty(information))
                    {
                        information = $"Se crearon {count} registro/s correctamente";
                    }

                    await UpdateHistory(internalid, isError, information, session);

                    transaction.Commit();

                    return new Response<object>()
                    {
                        Succeeded = true,
                        Message = "Operacion completada, vea la información para saber los detalles de la solicitud",
                        StatusHttp = 200,
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    await UpdateHistory(internalid, true, ex.Message, session);

                    string path = Path.Combine(partialRoute, @$"BatchLog\EmployeeExtraHoursProcess-{internalid}.txt");
                    using (FileStream fs = File.Create(path))
                    {
                        byte[] content = new UTF8Encoding(true).GetBytes(ex.Message);
                        fs.Write(content, 0, content.Length);
                    }

                    return new Response<object>(string.Empty)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { ex.Message },
                        StatusHttp = 404
                    };
                }
            }
        }

        //Códigos de ganancia
        public async Task<Response<object>> Create(List<BatchEarningCodeEmployeeRequest> models, string partialRoute, string[] session)
        {
            bool isError = false;
            int internalid = 0;
            string information = string.Empty;
            int count = 0;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _NewDbContext = scope.ServiceProvider.GetService<IApplicationDbContext>();

                internalid = await CreateHistory(BatchEntity.EmployeeEarningCode, session);

                using var transaction = _NewDbContext.Database.BeginTransaction();

                try
                {
                    foreach (var item in models)
                    {
                        count++;

                        //Validaciones
                        var employee = await _NewDbContext.Employees.Where(x => x.EmployeeId == item.EmployeeId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (employee == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de empleado asignado no existe. |";
                        }

                        var earningcode = await _NewDbContext.EarningCodes.Where(x => x.EarningCodeId == item.EarningCodeId
                                                                                        && x.IndexBase == IndexBase.FixedAmount
                                                                                        && x.DataareaID == session[0])
                                                                                 .IgnoreQueryFilters()
                                                                                 .FirstOrDefaultAsync();

                        if (earningcode == null)
                        {
                            isError = true;
                            information = $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de ganancia ({item.EarningCodeId}) asignado no existe.";
                        }

                        var payroll = await _NewDbContext.Payrolls.Where(x => x.PayrollId == item.PayrollId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (payroll == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de nómina ({item.PayrollId}) no existe. |";
                        }

                        if (!isError)
                        {
                            var paycycle = await _NewDbContext.PayCycles.Where(x => x.PayrollId == item.PayrollId
                                                                               && x.PayCycleId == item.StartPeriodForPaid
                                                                               && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                            if (paycycle == null)
                            {
                                isError = true;
                                information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El ciclo de pago ({item.StartPeriodForPaid}) para la nómina ({item.PayrollId}) no existe. |";
                            }

                            //var existEarningCodeEmployee = await _NewDbContext.EmployeeEarningCodes.Where(x => x.EmployeeId == item.EmployeeId
                            //                                         && x.EarningCodeId == item.EarningCodeId
                            //                                         && x.PayrollId == item.PayrollId
                            //                                         && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                            //if (existEarningCodeEmployee != null)
                            //{
                            //    isError = true;
                            //    information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de ganancia ({item.EarningCodeId}) ya fue asignado a la nómina ({item.PayrollId}). |";
                            //}
                        }
                        //Validaciones                       


                        if (!isError)
                        {
                            var response = await _NewDbContext.EmployeeEarningCodes.Where(x => x.EmployeeId == item.EmployeeId
                                                                                          && x.DataareaID == session[0])
                                                                                   .OrderByDescending(x => x.InternalId)
                                                                                   .IgnoreQueryFilters()
                                                                                   .FirstOrDefaultAsync();

                            //Guardo el nuevo código de ganancia
                            var entity = BuildDtoHelper<EmployeeEarningCode>.OnBuild(item, new EmployeeEarningCode());
                            entity.InternalId = response == null ? 1 : response.InternalId + 1;

                            entity.DataareaID = session[0];
                            entity.CreatedBy = session[1];
                            entity.CreatedOn = DateTime.Now;

                            entity.PayFrecuency = payroll.PayFrecuency;

                            entity.IndexEarningMonthly = item.IndexEarningMonthly;
                            entity.IndexEarning = item.IndexEarning;

                            if(item.IndexEarning != item.IndexEarningMonthly)
                            { 
                                entity.IndexEarning = CalcAmountForMonth(payroll.PayFrecuency, item.IndexEarningMonthly);                            
                            }

                            decimal constans = 23.83M;
                            entity.IndexEarningDiary = entity.IndexEarningMonthly / constans;
                            entity.IndexEarningHour = entity.IndexEarningDiary / 8;

                            _NewDbContext.EmployeeEarningCodes.Add(entity);
                            await _NewDbContext.SaveChangesAsync();
                        }
                    }

                    if (string.IsNullOrEmpty(information))
                    {
                        information = $"Se crearon {count} registro/s correctamente";
                    }

                    await UpdateHistory(internalid, isError, information, session);

                    transaction.Commit();

                    return new Response<object>()
                    {
                        Succeeded = true,
                        Message = "Operacion completada, vea la información para saber los detalles de la solicitud",
                        StatusHttp = 200,
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    await UpdateHistory(internalid, true, ex.Message, session);

                    string path = Path.Combine(partialRoute, @$"BatchLog\EmployeeEarningCodesProcess-{internalid}.txt");
                    using (FileStream fs = File.Create(path))
                    {
                        byte[] content = new UTF8Encoding(true).GetBytes(ex.Message);
                        fs.Write(content, 0, content.Length);
                    }

                    return new Response<object>(string.Empty)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { ex.Message },
                        StatusHttp = 404
                    };
                }
            }
        }

        //Prestamos
        public async Task<Response<object>> Create(List<BatchLoanEmployeeRequest> models, string partialRoute, string[] session)
        {
            bool isError = false;
            int internalid = 0;
            string information = string.Empty;
            int count = 0;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _NewDbContext = scope.ServiceProvider.GetService<IApplicationDbContext>();

                internalid = await CreateHistory(BatchEntity.EmployeeLoans, session);

                using var transaction = _NewDbContext.Database.BeginTransaction();

                try
                {
                    foreach (var item in models)
                    {
                        count++;

                        //Validaciones
                        var employee = await _NewDbContext.Employees.Where(x => x.EmployeeId == item.EmployeeId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (employee == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de empleado asignado no existe. |";
                        }

                        var loan = await _NewDbContext.Loans.Where(x => x.LoanId == item.LoanId
                                                                        && x.DataareaID == session[0])
                                                                   .IgnoreQueryFilters()
                                                                   .FirstOrDefaultAsync();

                        if (loan == null)
                        {
                            return new Response<object>(null)
                            {
                                Succeeded = false,
                                Errors = new List<string>() { $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de préstamo ({item.LoanId}) asignado no existe." },
                                StatusHttp = 404
                            };
                        }

                        var payroll = await _NewDbContext.Payrolls.Where(x => x.PayrollId == item.PayrollId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (payroll == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de nómina ({item.PayrollId}) no existe. |";
                        }

                        if (!isError)
                        {
                            var paycycle = await _NewDbContext.PayCycles.Where(x => x.PayrollId == item.PayrollId
                                                                               && x.PayCycleId == item.StartPeriodForPaid
                                                                               && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                            if (paycycle == null)
                            {
                                isError = true;
                                information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El ciclo de pago ({item.StartPeriodForPaid}) para la nómina ({item.PayrollId}) no existe. |";
                            }

                            var existLoanEmployee = await _NewDbContext.EmployeeLoans.Where(x => x.EmployeeId == item.EmployeeId
                                                                     && x.LoanId == item.LoanId
                                                                     && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                            if (existLoanEmployee != null)
                            {
                                isError = true;
                                information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El préstamo ({item.LoanId}) ya fue asignado a la nómina ({item.PayrollId}). |";
                            }
                        }
                        //Validaciones                       


                        if (!isError)
                        {
                            //Guardo el nuevo prestamo
                            var entity = BuildDtoHelper<EmployeeLoan>.OnBuild(item, new EmployeeLoan());

                            entity.DataareaID = session[0];
                            entity.CreatedBy = session[1];
                            entity.CreatedOn = DateTime.Now;

                            _NewDbContext.EmployeeLoans.Add(entity);
                            await _NewDbContext.SaveChangesAsync();
                        }
                    }

                    if (string.IsNullOrEmpty(information))
                    {
                        information = $"Se crearon {count} registro/s correctamente";
                    }

                    await UpdateHistory(internalid, isError, information, session);

                    transaction.Commit();

                    return new Response<object>()
                    {
                        Succeeded = true,
                        Message = "Operacion completada, vea la información para saber los detalles de la solicitud",
                        StatusHttp = 200,
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    await UpdateHistory(internalid, true, ex.Message, session);

                    string path = Path.Combine(partialRoute, @$"BatchLog\EmployeeLoansProcess-{internalid}.txt");
                    using (FileStream fs = File.Create(path))
                    {
                        byte[] content = new UTF8Encoding(true).GetBytes(ex.Message);
                        fs.Write(content, 0, content.Length);
                    }

                    return new Response<object>(string.Empty)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { ex.Message },
                        StatusHttp = 404
                    };
                }
            }
        }

        //Deducciones
        public async Task<Response<object>> Create(List<BatchDeductionCodeEmployeeRequest> models, string partialRoute, string[] session)
        {
            bool isError = false;
            int internalid = 0;
            string information = string.Empty;
            int count = 0;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _NewDbContext = scope.ServiceProvider.GetService<IApplicationDbContext>();

                internalid = await CreateHistory(BatchEntity.EmployeeDeductionCode, session);

                using var transaction = _NewDbContext.Database.BeginTransaction();

                try
                {
                    foreach (var item in models)
                    {
                        count++;

                        //Validaciones
                        var employee = await _NewDbContext.Employees.Where(x => x.EmployeeId == item.EmployeeId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (employee == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de empleado asignado no existe. |";
                        }

                        var deductioncode = await _NewDbContext.DeductionCodes.Where(x => x.DeductionCodeId == item.DeductionCodeId
                                                                        && x.DataareaID == session[0])
                                                                    .IgnoreQueryFilters()
                                                                    .FirstOrDefaultAsync();

                        if (deductioncode == null)
                        {
                            return new Response<object>(null)
                            {
                                Succeeded = false,
                                Errors = new List<string>() { $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de deducción ({item.DeductionCodeId}) asignado no existe." },
                                StatusHttp = 404
                            };
                        }

                        var payroll = await _NewDbContext.Payrolls.Where(x => x.PayrollId == item.PayrollId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (payroll == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de nómina ({item.PayrollId}) no existe. |";
                        }

                        if (!isError)
                        {
                            //Modificación de validación para deducciones por frecuencia de pago
                            var paycycle = await _NewDbContext.PayCycles.Where(x => x.PayrollId == item.PayrollId
                                                                               && x.PayCycleId == item.StartPeriodForPaid
                                                                               && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                            if (paycycle == null)
                            {
                                isError = true;
                                information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El ciclo de pago ({item.StartPeriodForPaid}) para la nómina ({item.PayrollId}) no existe. |";
                            }

                            //Modificación de validación para deducciones por frecuencia de pago

                            var existDeductionCodeEmployee = await _NewDbContext.EmployeeDeductionCodes.Where(x => x.EmployeeId == item.EmployeeId
                                                                                                  && x.DeductionCodeId == item.DeductionCodeId
                                                                                                  && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                            if (existDeductionCodeEmployee != null)
                            {
                                isError = true;
                                information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de deducción ({item.DeductionCodeId}) ya fue asignado a la nómina ({item.PayrollId}). |";
                            }
                        }
                        //Validaciones                       


                        if (!isError)
                        {
                            //Guardo el nuevo código de deducción
                            var entity = BuildDtoHelper<EmployeeDeductionCode>.OnBuild(item, new EmployeeDeductionCode());

                            entity.DataareaID = session[0];
                            entity.CreatedBy = session[1];
                            entity.CreatedOn = DateTime.Now;

                            //Modificación para agregar el periodo de pago de la nómina
                            entity.PayFrecuency = payroll.PayFrecuency;
                            //Modificación para agregar el periodo de pago de la nómina

                            _NewDbContext.EmployeeDeductionCodes.Add(entity);
                            await _NewDbContext.SaveChangesAsync();
                        }
                    }

                    if (string.IsNullOrEmpty(information))
                    {
                        information = $"Se crearon {count} registro/s correctamente";
                    }

                    await UpdateHistory(internalid, isError, information, session);

                    transaction.Commit();

                    return new Response<object>()
                    {
                        Succeeded = true,
                        Message = "Operacion completada, vea la información para saber los detalles de la solicitud",
                        StatusHttp = 200,
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return new Response<object>(string.Empty)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { ex.Message }
                    };
                }
            }
        }

        //Cursos
        public async Task<Response<object>> Create(List<BatchCourseRequest> models, string partialRoute, string[] session)
        {
            bool isError = false;
            int internalid = 0;
            string information = string.Empty;
            int count = 0;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _NewDbContext = scope.ServiceProvider.GetService<IApplicationDbContext>();

                internalid = await CreateHistory(BatchEntity.EmployeeDeductionCode, session);

                using var transaction = _NewDbContext.Database.BeginTransaction();

                try
                {
                    foreach (var item in models)
                    {
                        count++;

                        //Validaciones
                        var courseType = await _NewDbContext.CourseTypes.Where(x => x.CourseTypeId == item.CourseTypeId
                                                                               && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (courseType == null)
                        {
                            isError = true;
                            information += $"Course: {item.CourseName} - El tipo de curso asignado no existe. |";
                        }

                        var classroom = await _NewDbContext.ClassRooms.Where(x => x.ClassRoomId == item.ClassRoomId
                                                                             && x.DataareaID == session[0])
                                                                      .IgnoreQueryFilters()
                                                                      .FirstOrDefaultAsync();

                        if (classroom == null)
                        {
                            isError = true;
                            information += $"Course: {item.CourseName} - El salón de clase asignado no existe.|";
                        }


                        if (!string.IsNullOrEmpty(item.ClassRoomId))
                        {
                            var parentCourse = await _NewDbContext.Courses.Where(x => x.CourseId == item.CourseParentId
                                                                               && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                            if (parentCourse == null)
                            {
                                isError = true;
                                information += $"Course: {item.CourseName} - El curso padre asignado no existe.|";
                            }
                        }
                        //Validaciones                       


                        if (!isError)
                        {
                            //Guardo el nuevo código de curso
                            var entity = BuildDtoHelper<Course>.OnBuild(item, new Course());

                            entity.DataareaID = session[0];
                            entity.CreatedBy = session[1];
                            entity.CreatedOn = DateTime.Now;

                            _NewDbContext.Courses.Add(entity);
                            await _NewDbContext.SaveChangesAsync();
                        }
                    }

                    if (string.IsNullOrEmpty(information))
                    {
                        information = $"Se crearon {count} registro/s correctamente";
                    }

                    await UpdateHistory(internalid, isError, information, session);

                    transaction.Commit();

                    return new Response<object>()
                    {
                        Succeeded = true,
                        Message = "Operacion completada, vea la información para saber los detalles de la solicitud",
                        StatusHttp = 200,
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return new Response<object>(string.Empty)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { ex.Message }
                    };
                }
            }
        }


        public async Task<Response<bool>> Delete(List<string> ids)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.BatchHistories.Where(x => x.InternalId == int.Parse(item)).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");
                    }

                    _dbContext.BatchHistories.Remove(response);
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

        //Calendario de trabajo para empleados
        public async Task<Response<object>> Create(List<BatchEmployeeWorkCalendarRequest> models, string partialRoute, string[] session)
        {
            bool isError = false;
            int internalid = 0;
            string information = string.Empty;
            int count = 0;

            string[] spanishdays = new[] { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _NewDbContext = scope.ServiceProvider.GetService<IApplicationDbContext>();

                internalid = await CreateHistory(BatchEntity.EmployeeAdress, session);

                using var transaction = _NewDbContext.Database.BeginTransaction();

                try
                {
                    foreach (var item in models)
                    {
                        count++;

                        //Validaciones
                        var response = await _NewDbContext.Employees.Where(x => x.EmployeeId == item.EmployeeId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (response == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de empleado asignado no existe. |";
                        }

                        var workcalendar = await _NewDbContext.EmployeeWorkCalendars.Where(x => x.CalendarDate == item.CalendarDate &&
                                                                                            x.EmployeeId == item.EmployeeId &&
                                                                                            x.DataareaID == session[0]).FirstOrDefaultAsync();

                        if (workcalendar != null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - La fecha seleccionada ya existe. |";
                        }
                        //Validaciones                        

                        if (!isError)
                        {
                            var employeeWorkCalendar = await _NewDbContext.EmployeeWorkCalendars.Where(x => x.EmployeeId == item.EmployeeId
                                                                                     && x.DataareaID == session[0]).IgnoreQueryFilters().OrderByDescending(x => x.InternalId).FirstOrDefaultAsync();

                            var entity = BuildDtoHelper<EmployeeWorkCalendar>.OnBuild(item, new EmployeeWorkCalendar());

                            entity.InternalId = employeeWorkCalendar == null ? 1 : employeeWorkCalendar.InternalId + 1;

                            //Guardo la nueva fecha
                            entity.DataareaID = session[0];
                            entity.CreatedBy = session[1];
                            entity.CreatedOn = DateTime.Now;

                            string weekday = spanishdays[(int)item.CalendarDate.DayOfWeek];

                            entity.CalendarDay = weekday;

                            _NewDbContext.EmployeeWorkCalendars.Add(entity);
                            await _NewDbContext.SaveChangesAsync();
                        }
                    }

                    if (string.IsNullOrEmpty(information))
                    {
                        information = $"Se crearon {count} registro/s correctamente";
                    }

                    await UpdateHistory(internalid, isError, information, session);

                    transaction.Commit();

                    return new Response<object>()
                    {
                        Succeeded = true,
                        Message = "Operacion completada, vea la información para saber los detalles de la solicitud",
                        StatusHttp = 200,
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    await UpdateHistory(internalid, true, ex.Message, session);

                    string path = Path.Combine(partialRoute, @$"BatchLog\EmployeeWorkCalendarProcess-{internalid}.txt");
                    using (FileStream fs = File.Create(path))
                    {
                        byte[] content = new UTF8Encoding(true).GetBytes(ex.Message);
                        fs.Write(content, 0, content.Length);
                    }

                    return new Response<object>(string.Empty)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { ex.Message },
                        StatusHttp = 404
                    };
                }
            }
        }


        //Función para obtener el número de periodos para completar el mes
        private decimal CalcAmountForMonth(PayFrecuency _PayFrecuency, decimal amount)
        {
            decimal newamount = 0;

            switch (_PayFrecuency)
            {
                case PayFrecuency.Diary:
                    newamount = amount / 30;
                    break;
                case PayFrecuency.Weekly:
                    newamount = amount / 4;
                    break;
                case PayFrecuency.TwoWeekly:
                    newamount = amount / 2;
                    break;
                case PayFrecuency.BiWeekly:
                    newamount = amount / 2;
                    break;
                case PayFrecuency.Monthly:
                    newamount = 1;
                    break;
                case PayFrecuency.ThreeMonth:
                    newamount = amount * 3;
                    break;
                case PayFrecuency.FourMonth:
                    newamount = amount * 4;
                    break;
                case PayFrecuency.Biannual:
                    newamount = amount * 6;
                    break;
                case PayFrecuency.Yearly:
                    newamount = amount * 12;
                    break;
            }

            return newamount;
        }


        //Control de asistencia para empleados
        public async Task<Response<object>> Create(List<BatchEmployeeWorkControlCalendarRequest> models, string partialRoute, string[] session)
        {
            bool isError = false;
            int internalid = 0;
            string information = string.Empty;
            int count = 0;

            string[] spanishdays = new[] { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _NewDbContext = scope.ServiceProvider.GetService<IApplicationDbContext>();

                internalid = await CreateHistory(BatchEntity.EmployeeAdress, session);

                using var transaction = _NewDbContext.Database.BeginTransaction();

                try
                {
                    foreach (var item in models)
                    {
                        count++;

                        //Validaciones
                        var response = await _NewDbContext.Employees.Where(x => x.EmployeeId == item.EmployeeId
                                                                           && x.DataareaID == session[0]).IgnoreQueryFilters().FirstOrDefaultAsync();

                        if (response == null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - El código de empleado asignado no existe. |";
                        }

                        var workcalendar = await _NewDbContext.EmployeeWorkControlCalendars.Where(x => x.CalendarDate == item.CalendarDate &&
                                                                                            x.EmployeeId == item.EmployeeId &&
                                                                                            x.DataareaID == session[0]).FirstOrDefaultAsync();

                        if (workcalendar != null)
                        {
                            isError = true;
                            information += $"Empleado: {item.EmployeeId} {item.EmployeeName} - La fecha seleccionada ya existe. |";
                        }
                        //Validaciones                        

                        if (!isError)
                        {
                            var employeeWorkCalendar = await _NewDbContext.EmployeeWorkControlCalendars.Where(x => x.EmployeeId == item.EmployeeId
                                                                                     && x.DataareaID == session[0]).IgnoreQueryFilters().OrderByDescending(x => x.InternalId).FirstOrDefaultAsync();

                            var entity = BuildDtoHelper<EmployeeWorkControlCalendar>.OnBuild(item, new EmployeeWorkControlCalendar());

                            entity.InternalId = employeeWorkCalendar == null ? 1 : employeeWorkCalendar.InternalId + 1;

                            //Guardo la nueva fecha
                            entity.DataareaID = session[0];
                            entity.CreatedBy = session[1];
                            entity.CreatedOn = DateTime.Now;

                            string weekday = spanishdays[(int)item.CalendarDate.DayOfWeek];

                            entity.CalendarDay = weekday;
                            entity.TotalHour = (decimal)(Math.Abs((item.WorkTo - item.WorkFrom).TotalHours) - Math.Abs((item.BreakWorkFrom - item.BreakWorkTo).TotalHours)); entity.TotalHour = (decimal)(Math.Abs((item.WorkTo - item.WorkFrom).TotalHours) - Math.Abs((item.BreakWorkFrom - item.BreakWorkTo).TotalHours));
                            
                            _NewDbContext.EmployeeWorkControlCalendars.Add(entity);
                            await _NewDbContext.SaveChangesAsync();
                        }
                    }

                    if (string.IsNullOrEmpty(information))
                    {
                        information = $"Se crearon {count} registro/s correctamente";
                    }

                    await UpdateHistory(internalid, isError, information, session);

                    transaction.Commit();

                    return new Response<object>()
                    {
                        Succeeded = true,
                        Message = "Operacion completada, vea la información para saber los detalles de la solicitud",
                        StatusHttp = 200,
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    await UpdateHistory(internalid, true, ex.Message, session);

                    string path = Path.Combine(partialRoute, @$"BatchLog\EmployeeWorkCalendarProcess-{internalid}.txt");
                    using (FileStream fs = File.Create(path))
                    {
                        byte[] content = new UTF8Encoding(true).GetBytes(ex.Message);
                        fs.Write(content, 0, content.Length);
                    }

                    return new Response<object>(string.Empty)
                    {
                        Succeeded = false,
                        Errors = new List<string>() { ex.Message },
                        StatusHttp = 404
                    };
                }
            }
        }
    }
}
