using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Model
{
    public class Response <T>
    {
        public Response()
        {
        }
        public Response(T data)
        {
            StatusHttp = 200;
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string Message { get; set; }
        public int StatusHttp { get; set; }

    }
}
