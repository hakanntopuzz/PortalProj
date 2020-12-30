using DevPortal.Model;
using DevPortal.Web.Library.Model;

namespace DevPortal.Web.Library.Abstract
{
    public interface IApplicationNugetPackageViewModelFactory
    {
        #region application nuget packages

        ApplicationNugetPackageViewModel CreateDetailApplicationNugetPackageViewModel(Application application, ApplicationNugetPackage applicationNugetPackage, string nugetPackageUrl);

        ApplicationNugetPackageViewModel CreateApplicationNugetPackageViewModel(Application application);

        ApplicationNugetPackageViewModel CreateEditApplicationNugetPackageViewModel(Application application, ApplicationNugetPackage applicationNugetPackage, string nugetPackageUrl);

        #endregion
    }
}