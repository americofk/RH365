// ============================================================================
// Archivo: UserGridViewResponse.cs
// Proyecto: RH365.Core
// Ruta: RH365.Core/Domain/Models/UserGridViews/UserGridViewResponse.cs
// Descripción: Response para una vista de usuario
// ============================================================================

using System;
using System.Text.Json.Serialization;

namespace RH365.Core.Domain.Models.UserGridViews
{
    public class UserGridViewResponse
    {
        [JsonPropertyName("RecID")]
        public long RecID { get; set; }

        [JsonPropertyName("UserRefRecID")]
        public long UserRefRecID { get; set; }

        [JsonPropertyName("EntityName")]
        public string EntityName { get; set; }

        [JsonPropertyName("ViewName")]
        public string ViewName { get; set; }

        [JsonPropertyName("IsDefault")]
        public bool IsDefault { get; set; }

        [JsonPropertyName("IsPublic")]
        public bool IsPublic { get; set; }

        [JsonPropertyName("ViewScope")]
        public string ViewScope { get; set; }

        [JsonPropertyName("IsLocked")]
        public bool IsLocked { get; set; }

        [JsonPropertyName("ViewConfig")]
        public string ViewConfig { get; set; }

        [JsonPropertyName("SchemaVersion")]
        public int SchemaVersion { get; set; }

        [JsonPropertyName("DataareaID")]
        public string DataareaID { get; set; }

        [JsonPropertyName("CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonPropertyName("CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }
}