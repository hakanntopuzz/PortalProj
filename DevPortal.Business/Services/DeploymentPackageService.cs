using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Business.Services
{
    public class DeploymentPackageService : IDeploymentPackageService
    {
        #region ctor

        readonly IGeneralSettingsService generalSettingsService;

        readonly IDeploymentPackageRepository deploymentPackageRepository;

        public DeploymentPackageService(IGeneralSettingsService generalSettingsService,
            IDeploymentPackageRepository deploymentPackageRepository)
        {
            this.generalSettingsService = generalSettingsService;
            this.deploymentPackageRepository = deploymentPackageRepository;
        }

        #endregion

        #region deployment version

        public ICollection<DeploymentPackageFolder> GetApplicationsByEnvironmentId(int environmentId)
        {
            var packagesPath = GetDeploymentPackagesPathByEnvironmentId(environmentId);
            var deploymentPackages = deploymentPackageRepository.GetDeploymentPackagesFolder(packagesPath);

            return deploymentPackages.ToList();
        }

        public ICollection<DeploymentPackageFolder> GetDatabaseDeploymentPackages(int environmentId)
        {
            var packagesPath = @"P:\Aktarım\Veritabanı\PreProd";
            var deploymentPackages = deploymentPackageRepository.GetDeploymentPackagesFolder(packagesPath);

            return deploymentPackages.ToList();
        }

        private string GetDeploymentPackagesPathByEnvironmentId(int environmentId)
        {
            string path = string.Empty;
            if (environmentId == (int)Environments.PreProd)
            {
                path = generalSettingsService.GetApplicationVersionPackagePreProdFolderPath();
            }
            else if (environmentId == (int)Environments.Prod)
            {
                path = generalSettingsService.GetApplicationVersionPackageProdFolderPath();
            }

            path = path.Replace(@"\\", @"\");

            return path;
        }

        #endregion
    }
}