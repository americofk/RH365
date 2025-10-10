// ============================================================================
// Archivo: CreateClassRoomRequest.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/ClassRoom/CreateClassRoomRequest.cs
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.ClassRoom
{
    public class CreateClassRoomRequest
    {
        public string ClassRoomCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public long CourseLocationRefRecID { get; set; }
        public int MaxStudentQty { get; set; }
        public string? Comment { get; set; }
        public TimeOnly AvailableTimeStart { get; set; }
        public TimeOnly AvailableTimeEnd { get; set; }
        public string? Observations { get; set; }
    }
}