using RH365.Core.Domain.Const;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RH365.Core.CORE.Domain.Models
{
    public class ValidateLogin
    {
        [MaxLength(100)]
        [RegularExpression("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessage = ErrorMsg.Email)]
        [Required(ErrorMessage = "Correo" + ErrorMsg.Emptym)]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsValidateUser { get; set; }
    }
}
