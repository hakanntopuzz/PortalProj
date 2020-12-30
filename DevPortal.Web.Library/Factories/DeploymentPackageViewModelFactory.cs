using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;

namespace DevPortal.Web.Library
{
    public class DeploymentPackageViewModelFactory : IDeploymentPackageViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        public DeploymentPackageViewModelFactory(
            IBreadCrumbFactory breadCrumbFactory)
        {
            this.breadCrumbFactory = breadCrumbFactory;
        }

        #endregion

        public DeploymentPackageViewModel CreateDeploymentPackageViewModel()
        {
            var breadCrumbViewModel = breadCrumbFactory.CreateDeploymentPackageModel();

            return new DeploymentPackageViewModel
            {
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        public DeploymentPackageViewModel CreateDatabaseDeploymentPackageViewModel()
        {
            var breadCrumbViewModel = breadCrumbFactory.CreateDatabaseDeploymentPackageModel();

            return new DeploymentPackageViewModel
            {
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }
    }
}