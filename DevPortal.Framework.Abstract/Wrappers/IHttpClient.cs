using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevPortal.Framework.Abstract
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(Uri uri);

        Task<string> GetJsonStringAsync(Uri uri);

        Task<string> GetStringAsync(Uri uri);

        string DownloadString(Uri uri);
    }
}