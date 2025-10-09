// ============================================================================
// Archivo: CourseTypeDto.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Application/Features/DTOs/CourseType/CourseTypeDto.cs
// ============================================================================
using System;

namespace RH365.Core.Application.Features.DTOs.CourseType
{
    public class CourseTypeDto
    {
        public long RecID { get; set; }
        public string? ID { get; set; }
        public string CourseTypeCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Observations { get; set; }
        public string? DataareaID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}