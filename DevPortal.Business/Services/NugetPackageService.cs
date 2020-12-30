using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using System;

namespace DevPortal.Business.Services
{
    public class NugetPackageService : INugetPackageService
    {
        #region ctor

        readonly ISettings settings;

        public NugetPackageService(ISettings settings)
        {
            this.settings = settings;
        }

        #endregion

        public Uri GetNugetPackageRootUrl()
        {
            return new Uri($"{settings.SiteUrl}{"/nuget/packages/"}");
        }

        public Uri GetNugetPackageUrl(string packageName)
        {
            var rootUrl = GetNugetPackageRootUrl();

            return new Uri($"{rootUrl}{packageName}");
        }
    }
}