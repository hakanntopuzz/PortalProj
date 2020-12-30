using DevPortal.Web.Library.Model;

namespace DevPortal.Web.Library.Abstract
{
    public interface IDeploymentPackageViewModelFactory
    {
        DeploymentPackageViewModel CreateDeploymentPackageViewModel();

        DeploymentPackageViewModel CreateDatabaseDeploymentPackageViewModel();
    }
}