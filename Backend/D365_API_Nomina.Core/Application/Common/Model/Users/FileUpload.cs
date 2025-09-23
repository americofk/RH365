using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.Users
{
    public class FileUpload
    {
        public byte[] Image { get; set; }
        public string Extension { get; set; }
        public string Alias { get; set; }
    }
}
