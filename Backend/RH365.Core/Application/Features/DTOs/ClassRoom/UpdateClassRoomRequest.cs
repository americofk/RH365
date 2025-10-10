// ============================================================================
// Archivo: UpdateClassRoomRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/ClassRoom/UpdateClassRoomRequest.cs
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.ClassRoom
{
    public class UpdateClassRoomRequest
    {
        public string? ClassRoomCode { get; set; }
        public string? Name { get; set; }
        public long? CourseLocationRefRecID { get; set; }
        public int? MaxStudentQty { get; set; }
        public string? Comment { get; set; }
        public TimeOnly? AvailableTimeStart { get; set; }
        public TimeOnly? AvailableTimeEnd { get; set; }
        public string? Observations { get; set; }
    }
}