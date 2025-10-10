// ============================================================================
// Archivo: ClassRoomDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/ClassRoom/ClassRoomDto.cs
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.ClassRoom
{
    public class ClassRoomDto
    {
        public int RecID { get; set; }
        public string? ID { get; set; }
        public string ClassRoomCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public long CourseLocationRefRecID { get; set; }
        public int MaxStudentQty { get; set; }
        public string? Comment { get; set; }
        public TimeOnly AvailableTimeStart { get; set; }
        public TimeOnly AvailableTimeEnd { get; set; }
        public string? Observations { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}