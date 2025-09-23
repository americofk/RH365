using D365_API_Nomina.Core.Application.Common.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Users
{
    public class UserImageRequest : GenericValidation<UserImageRequest>, IValidatableObject
    {
        public IFormFile File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<string> extensions = new List<string>(){ ".png", ".jpg" };
            List<string> mimeTypes = new List<string>(){ "image/png", "image/jpeg" };

            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                //ForRule(this, x => string.IsNullOrWhiteSpace(x.Alias), "El alias no puede estar vacío"),
                ForRule(this, x => x.File.Length == 0 || x.File.Length > 1048576, "El tamaño del archivo debe ser mayor a 0 y menor a 1 mb"),
                ForRule(this, x => !extensions.Contains(Path.GetExtension(x.File.FileName).ToLower()), "La extensión del archivo debe ser png o jpg"),
                //ForRule(this, x => !mimeTypes.Contains(x.File.ContentType), $"La extensión del archivo debe contener png o jpg - {File.ContentType}")
            };

            return validationResults;
        }
    }
}
