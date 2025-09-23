using D365_API_Nomina.Core.Application.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Core.Application.Common.Interface
{
    public interface IDeleteByParentCommandHandler
    {
        public Task<Response<bool>> DeleteByParent(List<string> ids, string parentid);
    }
    
    public interface IDeleteByParentCommandHandler<T>
    {
        public Task<Response<bool>> DeleteByParent(List<T> ids, string parentid);
    }
}
