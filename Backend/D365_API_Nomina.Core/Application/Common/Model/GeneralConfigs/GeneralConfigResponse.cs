using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model.GeneralConfigs
{
    public class GeneralConfigResponse
    {
        public string Email { get; set; }
        public string SMTP { get; set; }
        public string SMTPPort { get; set; }
        public bool IsPassword { get; set; }
    }
}
