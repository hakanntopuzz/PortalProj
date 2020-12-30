using DevPortal.NugetManager.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface INugetPackageViewModelFactory
    {
        NugetPackageFolderViewModel CreateNugetPackageFolderViewModel(List<NugetPackageFolder> nugetPackages);

        NugetPackageViewModel CreateNugetPackageViewModel(List<NugetPackage> nugetPackages);
    }
}