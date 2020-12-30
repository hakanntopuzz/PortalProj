using Ab.Data.ApiClient;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using System;

namespace DevPortal.Data.Wrappers
{
    public class NugetServerApiClientWrapper : ApiClientWrapper, INugetServerApiClientWrapper
    {
        #region ctor

        readonly string baseUri;

        public NugetServerApiClientWrapper(IGeneralSettingsService service)
        {
            var serverUrl = service.GetNugetServerUrl();
            baseUri = serverUrl.ToString();
        }

        #endregion

        protected override Uri BaseUri => new Uri(baseUri);
    }
}