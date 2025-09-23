using D365_API_Nomina.Core.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Helper
{
    public static class JWTHelper
    {
        public static string GenerateJwtToken(User _user, AppSettings _configuration, string _companyDefault, bool _isLicenseValid)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.Secret);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, _user.Alias),
                new Claim(ClaimTypes.Email, _user.Email),
                new Claim(ClaimTypes.Actor, _user.ElevationType.ToString()),
                new Claim(ClaimTypes.PostalCode, _companyDefault), //Compañía por defecto
                new Claim(ClaimTypes.SerialNumber, _isLicenseValid.ToString()), //Compañía por defecto
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
