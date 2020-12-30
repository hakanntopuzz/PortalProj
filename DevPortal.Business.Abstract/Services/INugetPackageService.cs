using System;

namespace DevPortal.Business.Abstract
{
    public interface INugetPackageService
    {
        Uri GetNugetPackageRootUrl();

        Uri GetNugetPackageUrl(string packageName);
    }
}