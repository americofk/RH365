using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RH365.Infrastructure.Services
{
    public class ServiceConnect
    {
        public static async Task<HttpResponseMessage> connectservice(string token, string url, object body, HttpMethod method, IFormFile file = null)
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
                //form.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                //form.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                //form.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary",Guid.NewGuid().ToString()));
                form.Add(new ByteArrayContent(data), "file", file.FileName);

                //message.Content = form;
                if (!string.IsNullOrEmpty(token))
                    //form.Headers.Add("Authorization", $"Bearer {token}");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await client.PostAsync(url, form);

            }

            if (!string.IsNullOrEmpty(token))
                message.Headers.Add("Authorization", $"Bearer {token}");

            return await client.SendAsync(message);
        }
        
        public static async Task<Stream> connect(string token, string url)
        {
            HttpClient client = new HttpClient();

            //if (!string.IsNullOrEmpty(token))
            //    message.Headers.Add("Authorization", $"Bearer {token}");

            return await client.GetStreamAsync(url).ConfigureAwait(false);
        }
    }
}
