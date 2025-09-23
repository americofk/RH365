using System.ComponentModel.DataAnnotations;

namespace D365_API_Nomina.Core.Application.Common.Model.Projects
{
    public class ProjectRequestUpdate
    {
        [Required(ErrorMessage = "El nombre no puede estar vacío")]
        public string Name { get; set; }

        public string LedgerAccount { get; set; }
    }
}
