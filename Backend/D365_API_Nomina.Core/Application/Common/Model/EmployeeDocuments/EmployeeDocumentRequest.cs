using D365_API_Nomina.Core.Application.Common.Validation;
using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.EmployeeDocuments
{
    public class EmployeeDocumentRequest : GenericValidation<EmployeeDocumentRequest>, IValidatableObject
    {
        public string EmployeeId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime DueDate { get; set; }
        public string Comment { get; set; }
        public bool IsPrincipal { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>()
            {
                ForRule(this, x => string.IsNullOrWhiteSpace(x.DocumentNumber), "El número de documento no puede estar vacío"),
                ForRule(this, x => !Enum.IsDefined(typeof(DocumentType), x.DocumentType), "El tipo de documento suministrado no existe"),
                ForRule(this, x => string.IsNullOrEmpty(x.EmployeeId), "El empleado no puede estar vacío"),
                ForRule(this, x => x.DueDate == default, "La fecha de vencimiento no puede estar vacía")
            };

            return validationResults;
        }
    }
}
