// ============================================================================
// Archivo: CreateEmployeeContactsInfRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/EmployeeContactsInf/CreateEmployeeContactsInfRequest.cs
// ============================================================================
namespace RH365.Core.Application.Features.DTOs.EmployeeContactsInf
{
    public class CreateEmployeeContactsInfRequest
    {
        public long EmployeeRefRecID { get; set; }
        public int ContactType { get; set; }
        public string ContactValue { get; set; } = null!;
        public bool IsPrincipal { get; set; }
        public string? Comment { get; set; }
    }
}