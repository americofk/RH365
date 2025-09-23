using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Validation
{
    public class GenericValidation<T>
    {
        public Func<T, Func<T, bool>, string, ValidationResult> ForRule = (property, fn, message) =>
        {
            return fn(property)?new ValidationResult(message):null;
        };
    }
}
