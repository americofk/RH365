using D365_API_Nomina.Core.Application.Common.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeDocuments
{
    public class EmplDocFileRequest : GenericValidation<EmplDocFileRequest>, IValidatableObject
    {
        public IFormFile File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<string> extensions = new List<string>() { ".pdf"};

            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                //ForRule(this, x => string.IsNullOrWhiteSpace(x.Alias), "El alias no puede estar vacío"),
                ForRule(this, x => File.Length == 0 || File.Length > 1048576, "El tamaño del archivo debe ser mayor a 0 y menor a 1 mb"),
                ForRule(this, x => !extensions.Contains(Path.GetExtension(x.File.FileName).ToLower()), "La extensión del archivo debe ser pdf"),
                //ForRule(this, x => !File.ContentType.Contains("png") && !File.ContentType.Contains("jpg"), "La extensión del archivo debe ser png o jpg")
            };

            return validationResults;
        }
    }
}
