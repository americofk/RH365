using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.ProjCategories
{
    public class ProjCategoryRequestUpdate
    {
        [Required(ErrorMessage = "El nombre no puede estar vacío")]
        public string CategoryName { get; set; }

        public string LedgerAccount { get; set; }
    }
}
