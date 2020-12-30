using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System;
using System.Collections.Generic;

namespace DevPortal.Data.Repositories
{
    public class DeploymentPackageRepository : IDeploymentPackageRepository
    {
        #region ctor

        readonly ILoggingService loggingService;

        readonly IFileSystem fileSystem;

        readonly IDeploymentPackageFactory deploymentPackageFactory;

        public DeploymentPackageRepository(
            ILoggingService loggingService,
            IFileSystem fileSystem,
            IDeploymentPackageFactory deploymentPackageFactory)
        {
            this.loggingService = loggingService;
            this.fileSystem = fileSystem;
            this.deploymentPackageFactory = deploymentPackageFactory;
        }

        #endregion

        public ICollection<DeploymentPackageFolder> GetDeploymentPackagesFolder(string filePath)
        {
            try
            {
                var directory = fileSystem.GetDirectories(filePath);

                return deploymentPackageFactory.CreateDeploymentPackageFolders(directory);
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(GetDeploymentPackagesFolder), Resources.Resources.Messages.DosyaOkunamadi, ex);

                return deploymentPackageFactory.CreateDeploymentPackageFolders();
            }
        }

        public ICollection<string> GetSubDirectoryFoldersName(string filePath)
        {
            try
            {
                var directory = fileSystem.GetDirectories(filePath);

                return deploymentPackageFactory.GetSubDirectoryFoldersName(directory);
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(GetSubDirectoryFoldersName), Resources.Resources.Messages.DosyaOkunamadi, ex);

                return deploymentPackageFactory.CreateEmptyStringList();
            }
        }
    }
}