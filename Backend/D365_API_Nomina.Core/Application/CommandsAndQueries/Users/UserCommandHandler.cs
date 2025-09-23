using D365_API_Nomina.Core.Application.CommandsAndQueries.LicenseValidations;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.Users;
using D365_API_Nomina.Core.Domain.Entities;
using D365_API_Nomina.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Users
{
    public interface IUserCommandHandler : ICreateCommandHandler<UserRequest>, IUpdateCommandHandler<UserRequestUpdate>
        , IDeleteCommandHandler
    {
        public Task<Response<object>> UpdateOptions(string id, UserOptionsRequestUpdate model);
        public Task<Response<object>> UploadUserImage(UserImageRequest request, string alias);
        public Task<Response<object>> DownloadUserImage(string alias);        
        public Task<Response<object>> ChangeCompanyUsed(string companyid);        
    }

    public class UserCommandHandler : IUserCommandHandler
    {
        private readonly AppSettings _configuration;
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserInformation _currentUserInformation;
        private readonly ILicenseValidationQueryHandler _queryHandler;

        public UserCommandHandler(IApplicationDbContext applicationDbContext, ICurrentUserInformation currentUserInformation, IOptions<AppSettings> configuration, ILicenseValidationQueryHandler queryHandler)
        {
            _dbContext = applicationDbContext;
            _currentUserInformation = currentUserInformation;
            _configuration = configuration.Value;
            _queryHandler = queryHandler;
        }

        public async Task<Response<object>> Create(UserRequest model)
        {
            var a = await ValidateUserInformation(model.FormatCodeId, model.CompanyDefaultId, model.Alias);
            if (a != null)
                return a;
            
            var newUSer = await _dbContext.Users.Where(x => x.Alias == model.Alias || x.Email == model.Email).FirstOrDefaultAsync();
            if (newUSer != null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    StatusHttp = 404,
                    Errors = new List<string>() { "El alias o el correo ya existe." }
                };
            }

            var entity = BuildDtoHelper<User>.OnBuild(model, new User());
            entity.Password = "";
            _dbContext.Users.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(entity)
            {
                Message = "Registro creado correctamente"
            };
        }

        public async Task<Response<object>> Update(string id, UserRequestUpdate model)
        {
            var response = await _dbContext.Users.Where(x => x.Alias == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Message = "El registro seleccionado no existe"
                };
            }

            var a = await ValidateUserInformation(model.FormatCodeId, model.CompanyDefaultId, id);
            if (a != null)
                return a;

            var entity = BuildDtoHelper<User>.OnBuild(model, response);
            _dbContext.Users.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }

        public async Task<Response<object>> UpdateOptions(string id, UserOptionsRequestUpdate model)
        {
            var response = await _dbContext.Users.Where(x => x.Alias == id).FirstOrDefaultAsync();

            if (response == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    Message = "El registro seleccionado no existe"
                };
            }

            var a = await ValidateUserInformation(model.FormatCodeId, model.CompanyDefaultId, id);
            if (a != null)
                return a;

            var entity = BuildDtoHelper<User>.OnBuild(model, response);
            _dbContext.Users.Update(entity);
            await _dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Registro actualizado con éxito" };
        }

        public async  Task<Response<object>> UploadUserImage(UserImageRequest request, string alias)
        {
            UserImage entity = ConfigureFileRequest(request, alias);

            var imageUser = await _dbContext.UserImages.Where(x => x.Alias == alias).FirstOrDefaultAsync();

            if(imageUser == null)
            {
                _dbContext.UserImages.Add(entity);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                imageUser.Image = entity.Image;
                imageUser.Extension = entity.Extension;

                _dbContext.UserImages.Update(imageUser);
                await _dbContext.SaveChangesAsync();
            }

            return new Response<object>(true)
            {
                Message = "Imagen cargada correctamente"
            };
        }

        public async Task<Response<object>> DownloadUserImage(string alias)
        {
            var image = await _dbContext.UserImages.Where(x => x.Alias == alias).FirstOrDefaultAsync();

            string string64 = string.Empty;

            if(image != null)
            {
                string64 = Convert.ToBase64String(image.Image, 0, image.Image.Length);
            }

            return new Response<object>(string64);
        }

        public async Task<Response<bool>> Delete(List<string> ids)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                foreach (var item in ids)
                {
                    var response = await _dbContext.Users.Where(x => x.Alias == item).FirstOrDefaultAsync();

                    if (response == null)
                    {
                        throw new Exception($"El registro seleccionado no existe - id {item}");

                    }

                    _dbContext.Users.Remove(response);
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

        //Servicio para preparar las imágenes para guardarlas en la base de datos
        private UserImage ConfigureFileRequest(UserImageRequest request, string alias)
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

            return new UserImage() { Image = data, Extension = Path.GetExtension(request.File.FileName), Alias = alias };
        }

        private async Task<Response<object>> ValidateUserInformation(string FormatCodeId, string CompanyDefaultId, string alias)
        {           

            var formatCode = _dbContext.FormatCodes.Where(x => x.FormatCodeId == FormatCodeId).FirstOrDefaultAsync();
            if (await formatCode == null)
            {
                return new Response<object>(false)
                {
                    Succeeded = false,
                    StatusHttp = 404,
                    Errors = new List<string>() { "El código de formato no existe" }
                };
            }

            if (!string.IsNullOrEmpty(CompanyDefaultId))
            {
                var user = await _dbContext.Users.Where(x => x.Alias == alias).FirstOrDefaultAsync();
                if (user.ElevationType == Domain.Enums.AdminType.LocalAdmin)
                {
                    var companyForUser = _dbContext.Companies.Where(x => x.CompanyId == CompanyDefaultId).FirstOrDefaultAsync();
                    if (await companyForUser == null)
                    {
                        return new Response<object>(false)
                        {
                            Succeeded = false,
                            StatusHttp = 404,
                            Errors = new List<string>() { "La empresa asignada por defecto no éxiste o no es válida" }
                        };
                    }
                }
                else
                {
                    var companyForUser = _dbContext.CompaniesAssignedToUsers.Where(x => x.CompanyId == CompanyDefaultId).FirstOrDefaultAsync();
                    if (await companyForUser == null)
                    {
                        return new Response<object>(false)
                        {
                            Succeeded = false,
                            StatusHttp = 404,
                            Errors = new List<string>() { "La empresa asignada por defecto no éxiste o no es válida" }
                        };
                    }
                }
            }

            return null;
        }


        public async Task<Response<object>> ChangeCompanyUsed(string companyid)
        {
            var user = await _dbContext.Users.Where(x => x.Alias == _currentUserInformation.Alias).FirstOrDefaultAsync();

            if (user == null)
            {
                return new Response<object>()
                {
                    Succeeded = false,
                    Errors = new List<string> { "El registro seleccionado no existe" }
                };
            }
            var company = await _dbContext.Companies.Where(x => x.CompanyId == companyid).FirstOrDefaultAsync();

            if(company == null)
            {
                return new Response<object>()
                {
                    Succeeded = false,
                    Errors = new List<string> { "El id de la compañía no existe" }
                };
            }

            var adminType = (AdminType)Enum.Parse(typeof(AdminType), _currentUserInformation.ElevationType);
            if (adminType != AdminType.LocalAdmin)
            {
                var companyUser = await _dbContext.CompaniesAssignedToUsers.Where(x => x.Alias == _currentUserInformation.Alias
                                                                              && x.CompanyId == companyid).FirstOrDefaultAsync();
                if (companyUser == null)
                {
                    return new Response<object>()
                    {
                        Succeeded = false,
                        Errors = new List<string> { "El usuario no tiene asignada la empresa que quiere cambiar" }
                    };
                }
            }

            user.CompanyDefaultId = companyid;

            //Validar licencia de la compañía por defecto
            var isLicenseValid = await _queryHandler.ValidateLicense(company.LicenseKey);

            string token = JWTHelper.GenerateJwtToken(user, _configuration, companyid, (bool)isLicenseValid.Data);

            return new Response<object>(
                new { 
                    Token = token, 
                    Name = company.Name, 
                    CompanyId = company.CompanyId 
                });

        }
    }
}
