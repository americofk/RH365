using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.Common.Interface
{
    public interface IEmailServices
    {
        public Task<string> SendEmail(string email, string temporaryPassword, string username);
        public Task<string> SendEmailFile(string email, string bodyemail, DateTime reportdate);
    }
}
