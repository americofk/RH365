using D365_API_Nomina.Core.Application.CommandsAndQueries.LicenseValidations;
using D365_API_Nomina.Core.Application.Common.Helper;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Model.User;
using D365_API_Nomina.Core.Application.Common.Model.Users;
using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.CommandsAndQueries.Login
{
    public interface ILoginCommandHandler
    {
        public Task<Response<object>> Login(LoginRequest _model);
        public Task<Response<object>> RequestChangePassword(string identificator);
        public Task<Response<object>> SendNewPassword(UserChangePasswordRequest model);
    }

    public class LoginCommandHandler: ILoginCommandHandler
    {
        private readonly AppSettings _configuration;
        private readonly IApplicationDbContext dbContext;
        private readonly IEmailServices _emailServices;
        private readonly ILicenseValidationQueryHandler _queryHandler;

        public LoginCommandHandler(IApplicationDbContext applicationDbContext, IOptions<AppSettings> configuration,
            IEmailServices emailServices, ILicenseValidationQueryHandler queryHandler)
        {
            _configuration = configuration.Value;
            dbContext = applicationDbContext;
            _emailServices = emailServices;
            _queryHandler = queryHandler;
        }

        public async Task<Response<object>> Login(LoginRequest _model)
        {
            Response<object> response;

            if (_model.IsValidateUser)
                response = await ValidateUser(_model);
            else
                response = await LoginRequest(_model);

            return response;
        }

        private async Task<Response<object>> LoginRequest(LoginRequest _model)
        {
            var login = await dbContext.Users
                .FirstOrDefaultAsync(x => (x.Email == _model.Email || x.Alias == _model.Email) && x.Password == _model.Password);

            if (login == null)
            {
                return new Response<object>()
                {
                    StatusHttp = 404,
                    Message = "Correo o contraseña inválidas",
                    Errors = new List<string> { "Correo o contraseña inválidas" },
                    Succeeded = false
                };
            }

            List<CompanyForUser> companies;

            if (login.ElevationType == Domain.Enums.AdminType.LocalAdmin)
            {
                companies = await dbContext.Companies
                    .Where(x => x.CompanyStatus == true)
                    .Select(x => new CompanyForUser
                    {
                        CompanyId = x.CompanyId,
                        Name = x.Name,
                        Key = x.LicenseKey

                    }).ToListAsync();
            }
            else
            {
                //Obtener las empresas para los usuario sin roles de administrador
                companies = await dbContext.CompaniesAssignedToUsers.Where(x => x.Alias == login.Alias)
                    .Join(dbContext.Companies,
                        assinged => assinged.CompanyId,
                        companies => companies.CompanyId,
                        (assigned, companies) => new { Company = companies, Assigned = assigned }
                    )
                    .Where(x => x.Company.CompanyStatus == true)
                    .Select(x => new CompanyForUser
                    {
                        CompanyId = x.Company.CompanyId,
                        Name = x.Company.Name,
                        Key = x.Company.LicenseKey

                    }).ToListAsync();

                if (companies == null || companies.Count == 0)
                {
                    return new Response<object>()
                    {
                        StatusHttp = 404,
                        Message = "El usuario no tiene empresas asignadas",
                        Errors = new List<string> { "El usuario no tiene empresas asignadas" },
                        Succeeded = false
                    };
                }
            }

            string companyDefault = string.IsNullOrEmpty(login.CompanyDefaultId) ? companies.First().CompanyId : login.CompanyDefaultId;

            //Validar licencia de la compañía por defecto
            var isLicenseValid = await _queryHandler.ValidateLicense(companies.Find(x => x.CompanyId == companyDefault).Key);

            //Se envía el valor bool que indica si la licencia está habilitada
            string token = JWTHelper.GenerateJwtToken(login, _configuration, companyDefault, (bool)isLicenseValid.Data);
            LoginResponse userResponse = new LoginResponse()
            {
                Token = token,
                Name = $"{login.Name}{(bool)isLicenseValid.Data}" ,
                Alias = login.Alias,
                Avatar = GetImageUser(login.Alias),
                FormatCode = login.FormatCodeId,
                UserCompanies = companies,
                Email = login.Email,
                DefaultCompany = companyDefault
            };

            return new Response<object>(userResponse);
        }


        private async Task<Response<object>> ValidateUser(LoginRequest _model)
        {
            var login = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == _model.Email || x.Alias == _model.Email);

            if (login == null)
            {
                return new Response<object>(false)
                {
                    StatusHttp = 404,
                    Message = "Correo inválido",
                    Errors = new List<string> { "Correo inválido" },
                    Succeeded = false
                };
            }

            return new Response<object>(true);
        }

        public async Task<Response<object>> RequestChangePassword(string identificator)
        {
            var user = await dbContext.Users.Where(x => x.Alias == identificator || x.Email == identificator).FirstOrDefaultAsync();

            if(user == null)
            {
                return new Response<object>(false)
                {
                    StatusHttp = 404,
                    Succeeded = false,
                    Message = "Correo o usuario inválido",
                    Errors = new List<string>() { "Correo o usuario inválido" }
                };
            }

            user.TemporaryPassword = SecurityHelper.MD5(GenerateTokenString());
            user.DateTemporaryPassword = DateTime.Today;

            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();

            string emailResponse = await _emailServices.SendEmail(user.Email, user.TemporaryPassword, user.Name);

            int httpStatus = int.Parse(emailResponse.Substring(0, 3));

            if(httpStatus != 200)
            {
                return new Response<object>(true) { Errors = new List<string>() { emailResponse.Substring(4) }, StatusHttp = httpStatus };
            }
            
            return new Response<object>(true) { Message = emailResponse.Substring(4) , StatusHttp = httpStatus };
        }

        public async Task<Response<object>> SendNewPassword(UserChangePasswordRequest model)
        {
            var user = await dbContext.Users.Where(x => x.Email == model.Email 
                                                    && x.TemporaryPassword == model.TemporaryPassword)
                                            .FirstOrDefaultAsync();

            if (user == null)
            {
                return new Response<object>(false)
                {
                    StatusHttp = 404,
                    Message = "Correo o contraseña temporal inválidos",
                    Errors = new List<string>() { "Correo o contraseña temporal inválidos" },
                    Succeeded = false
                };
            }

            double totaltime = (DateTime.Today - user.DateTemporaryPassword).TotalHours;
            if (totaltime > 12)
            {
                user.TemporaryPassword = null;
                user.DateTemporaryPassword = default;

                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();

                return new Response<object>(false)
                {
                    StatusHttp = 404,
                    Message = "Contraseña temporal vencida, por favor solicite una nueva",
                    Errors = new List<string>() { "Contraseña temporal vencida, por favor solicite una nueva" },
                    Succeeded = false
                };
            }

            //user.Password = SecurityHelper.MD5(model.NewPassword);
            user.Password = model.NewPassword;
            user.TemporaryPassword = null;
            user.DateTemporaryPassword = default;

            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();

            return new Response<object>(true) { Message = "Contraseña cambiada correctamente" };
        }



        private string GetImageUser(string alias)
        {
            var image = dbContext.UserImages.Where(x => x.Alias == alias).FirstOrDefault();

            string string64 = string.Empty;

            if (image != null)
            {
                string64 = Convert.ToBase64String(image.Image, 0, image.Image.Length);
            }

            return string64;
        }

        private string GenerateTokenString()
        {
            Guid newGuid = Guid.NewGuid();
            string token = Convert.ToBase64String(newGuid.ToByteArray());
            token = token.Replace("=", "").Replace("+", "");

            return token;
        }

        //private string GenerateJwtToken(User _user)
        //{
        //    // generate token that is valid for 7 days
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_configuration.Secret);

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, _user.Alias),
        //        new Claim(ClaimTypes.Email, _user.Email),
        //        new Claim(ClaimTypes.Actor, _user.ElevationType.ToString()),
        //        new Claim(ClaimTypes.PostalCode, _user.CompanyDefaultId) //Compañía por defecto
        //    };

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(claims),
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}
    }
}
