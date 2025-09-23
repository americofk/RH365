using D365_API_Nomina.Core.Application.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.Common.Interface
{
    public interface IQueryByIdHandler<T>
    {
        public Task<Response<T>> GetId(string id);
    }
}
