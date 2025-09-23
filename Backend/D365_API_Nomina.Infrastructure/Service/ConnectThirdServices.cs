using D365_API_Nomina.Core.Application.Common.Interface;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Infrastructure.Service
{
    public class ConnectThirdServices : IConnectThirdServices
    {
        public async Task<HttpResponseMessage> CallAsync(string url, object body, HttpMethod method, IFormFile file = null)
        {
            HttpClient client = new HttpClient();
            MultipartFormDataContent form = null;
            HttpRequestMessage message = new HttpRequestMessage(method, url);

            if (body != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(body), UnicodeEncoding.UTF8, "application/json");
            }

            if (file != null)
            {
                byte[] data;
                using (MemoryStream ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    data = ms.ToArray();
                }

                form = new MultipartFormDataContent();
                form.Add(new ByteArrayContent(data), "file", file.FileName);

            }

            return await client.SendAsync(message);
        }
    }
}
