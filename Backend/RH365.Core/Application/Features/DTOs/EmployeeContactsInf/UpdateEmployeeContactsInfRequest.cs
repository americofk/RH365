// ============================================================================
// Archivo: UpdateEmployeeContactsInfRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeContactsInf/UpdateEmployeeContactsInfRequest.cs
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.EmployeeContactsInf
{
    public class UpdateEmployeeContactsInfRequest
    {
        public long? EmployeeRefRecID { get; set; }
        public int? ContactType { get; set; }
        public string? ContactValue { get; set; }
        public bool? IsPrincipal { get; set; }
        public string? Comment { get; set; }
    }
}