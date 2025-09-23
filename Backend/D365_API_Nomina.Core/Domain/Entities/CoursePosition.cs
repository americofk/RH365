using D365_API_Nomina.Core.Domain.Common;

namespace D365_API_Nomina.Core.Domain.Entities
{
    public class CoursePosition: AuditableCompanyEntity
    {
        public string PositionId { get; set; }
        public string CourseId { get; set; }
        public string Comment { get; set; }
    }
}
