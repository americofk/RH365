using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.Common.Interface
{
    public interface IHubNotification
    {
        public Task NotificationBatchImport(string messages);
    }
}
