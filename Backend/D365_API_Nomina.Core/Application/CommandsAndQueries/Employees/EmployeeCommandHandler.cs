using D365_API_Nomina.Core.Application.CommandsAndQueries.EmployeePositions;
using D365_API_Nomina.Core.Application.CommandsAndQueries.LicenseValidations;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.EmployeePositions;
using D365_API_Nomina.Core.Application.Common.Model.Employees;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Employees
{
    public interface IEmployeeCommandHandler :
        ICreateCommandHandler<EmployeeRequest>,
        IDeleteCommandHandler,
        IUpdateCommandHandler<EmployeeRequest>
    {
        public Task<Response<object>> UpdateStatus(string id, bool status, bool isForDGT);
        public Task<Response<object>> UploadImage(EmployeeImageRequest request, string employee);
        public Task<Response<object>> DownloadImage(string employee);
        public Task<Response<object>> AddEmployeetoJob(EmployeePositionRequest model);
        public Task<Response<object>> DismissEmployee(EmployeeRequestDismiss model);
    }

    public class EmployeeCommandHandler : IEmployeeCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IEmployeePositionCommandHandler _employeePosition;
        private readonly ILicenseValidationQueryHandler _queryHandler;
        private readonly ICurrentUserInformation _currentUser;

        public EmployeeCommandHandler(IApplicationDbContext applicationDbContext, IEmployeePositionCommandHandler employeePosition, ILicenseValidationQueryHandler queryHandler, ICurrentUserInformation currentUser)
        {
            _dbContext = applicationDbContext;
            _employeePosition = employeePosition;
            _queryHandler = queryHandler;
            _currentUser = currentUser;
        }

        public async Task<Response<object>> Create(EmployeeRequest model)
        {
            //Validar la cantidad de empleados de la licencia
            string licensekey = _dbContext.Companies.Where(x => x.CompanyId == _currentUser.Company).FirstOrDefault().LicenseKey;

            int controlnumber = _dbContext.Employees.Count() + 1;

            var isLicenseValid = await _queryHandler.ValidateNroControl(licensekey, controlnumber);
            bool isValid = (bool)isLicenseValid.Data;

            if (!isValid)
            {
                return isLicenseValid;
            }

            var response = await _dbContext.Countries.Where(x => x.CountryId == model.Country).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(null)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El país selecionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<Employee>.OnBuild(model, new Employee());

            _dbContext.Employees.Add(entity);
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
                    var response = await _dbContext.Employees.Where(x => x.EmployeeId == item).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");

                    }

                    _dbContext.Employees.Remove(response);
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


        public async Task<Response<object>> Update(string id, EmployeeRequest model)
        {
            var response = await _dbContext.Employees.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El registro seleccionado no existe" },
                    StatusHttp = 404
                };
            }

            var entity = BuildDtoHelper<Employee>.OnBuild(model, response);
            _dbContext.Employees.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }


        public async Task<Response<object>> UpdateStatus(string id, bool status, bool isForDGT)
        {
            var response = await _dbContext.Employees.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Message = "El registro seleccionado no existe"
                };
            }

            response.EmployeeStatus = status;
            _dbContext.Employees.Update(response);
            await _dbContext.SaveChangesAsync();


            //Se inserta en la tabla del historial
            var employeehistory = new EmployeeHistory()
            {
                Type = "NO",
                Description = status? $"Se habilitó el empleado":$"Se deshabilitó el empleado",
                RegisterDate = DateTime.Now,
                EmployeeId = id,
                IsUseDGT = isForDGT
            };

            _dbContext.EmployeeHistories.Add(employeehistory);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito, se agregó al historial del empleado" };

            //return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }


        public async Task<Response<object>> UploadImage(EmployeeImageRequest request, string employee)
        {
            EmployeeImage entity = ConfigureFileRequest(request, employee);

            var imageEmployee = await _dbContext.EmployeeImages.Where(x => x.EmployeeId == employee).FirstOrDefaultAsync();

            if (imageEmployee == null)
            {
                _dbContext.EmployeeImages.Add(entity);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                imageEmployee.Image = entity.Image;
                imageEmployee.Extension = entity.Extension;

                _dbContext.EmployeeImages.Update(imageEmployee);
                await _dbContext.SaveChangesAsync();
            }

            return new Response<object>(true)
            {
                Message = "Imagen cargada correctamente"
            };
        }


        private EmployeeImage ConfigureFileRequest(EmployeeImageRequest request, string employee)
        {
            Image image;
            Image thumb;
            byte[] data;
            ImageFormat imageFormat = null;

            string extension = Path.GetExtension(request.File.FileName);
            if (extension == ".png")
                imageFormat = ImageFormat.Png;

            if (extension == ".jpg")
                imageFormat = ImageFormat.Jpeg;

            using (MemoryStream fs = new MemoryStream())
            {
                request.File.CopyTo(fs);

                image = Image.FromStream(fs);
            }

            using (MemoryStream img = new MemoryStream())
            {
                thumb = image.GetThumbnailImage(165, 155, () => false, IntPtr.Zero);

                thumb.Save(img, imageFormat);
                data = img.ToArray();
            }

            return new EmployeeImage() { Image = data, Extension = Path.GetExtension(request.File.FileName), EmployeeId = employee };
        }


        public async Task<Response<object>> DownloadImage(string employee)
        {
            var image = await _dbContext.EmployeeImages.Where(x => x.EmployeeId == employee).FirstOrDefaultAsync();

            string string64 = string.Empty;

            if (image != null)
            {
                string64 = Convert.ToBase64String(image.Image, 0, image.Image.Length);
            }

            return new Response<object>(string64);
        }


        //Sección de contratar y despedir empleados

        public async Task<Response<object>> AddEmployeetoJob(EmployeePositionRequest model)
        {
            var employee = await _dbContext.Employees.Where(x => x.EmployeeId == model.EmployeeId
                                                            && x.WorkStatus != WorkStatus.Employ).FirstOrDefaultAsync();

            if (employee == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El empleado no existe o ya está contratado." },
                    StatusHttp = 404
                };
            }

            model.SetCallerName("employee");
            var response = await _employeePosition.Create(model);
            if ( response.StatusHttp == 404)
                return response;

            var entity = employee;
            entity.WorkStatus = WorkStatus.Employ;
            entity.StartWorkDate = model.FromDate;
            entity.EndWorkDate = model.ToDate;

            _dbContext.Employees.Update(entity);
            await _dbContext.SaveChangesAsync();

            //Se guarda la información en la tabla del historial
            var employeehistory = new EmployeeHistory()
            {
                Type = "NI",
                Description = "Empleado contratado",
                RegisterDate = model.FromDate,
                EmployeeId = employee.EmployeeId
            };

            _dbContext.EmployeeHistories.Add(employeehistory);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Empleado contratado con éxito." };
        }

        public async Task<Response<object>> DismissEmployee(EmployeeRequestDismiss model)
        {
            var employee = await _dbContext.Employees.Where(x => x.EmployeeId == model.EmployeeId
                                                            && x.WorkStatus != WorkStatus.Dismissed).FirstOrDefaultAsync();

            if (employee == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Errors = new List<string>() { "El empleado no existe o ya fue dado de baja." },
                    StatusHttp = 404
                };
            }

            var employeeposition = await _dbContext.EmployeePositions.Where(x => x.EmployeeId == model.EmployeeId
                                                                            && x.EmployeePositionStatus == true).FirstOrDefaultAsync();
            if (employeeposition != null)
            {
                EmployeePositionStatusRequest a = new EmployeePositionStatusRequest()
                {
                    EmployeeId = model.EmployeeId,
                    ToDate = model.ToDate,
                    PositionId = employeeposition.PositionId
                };

                var response = await _employeePosition.UpdateStatus(a);
                if (response.StatusHttp == 404)
                    return response;
            }

            var entity = employee;
            entity.WorkStatus = WorkStatus.Dismissed;
            entity.EndWorkDate = model.ToDate;
            entity.EmployeeAction = model.EmployeeAction;

            _dbContext.Employees.Update(entity);
            await _dbContext.SaveChangesAsync();


            //Se guarda la información en la tabla del historial
            var employeehistory = new EmployeeHistory()
            {
                Type = "NS",
                Description = "Empleado dado de baja",
                RegisterDate = model.ToDate,
                EmployeeId = employee.EmployeeId
            };

            _dbContext.EmployeeHistories.Add(employeehistory);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Empleado dado de baja con éxito." };
        }
    }

}
