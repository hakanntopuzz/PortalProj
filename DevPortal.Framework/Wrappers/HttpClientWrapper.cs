using DevPortal.Framework.Abstract;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DevPortal.Framework.Wrappers
{
    public class HttpClientWrapper : IHttpClient
    {
        public async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetAsync(uri);
            }
        }

        public async Task<string> GetJsonStringAsync(Uri uri)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(uri);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> GetStringAsync(Uri uri)
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetStringAsync(uri);
            }
        }

        public string DownloadString(Uri uri)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(uri);
            }
        }
    }
}