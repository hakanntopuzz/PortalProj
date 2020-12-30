using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.NugetManager.Model;
using DevPortal.Resources.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Data.Repositories
{
    public class NugetServerRepository : INugetServerRepository
    {
        #region ctor

        readonly INugetServerApiClientWrapper apiClient;

        readonly ILoggingService loggingService;

        readonly IFileSystemService fileSystemService;

        readonly INugetServerFactory nugetServerFactory;

        readonly IApplicationDataRequestFactory applicationDataRequestFactory;

        public NugetServerRepository(
            INugetServerApiClientWrapper apiClient,
            ILoggingService loggingService,
            IFileSystemService fileSystemService,
            INugetServerFactory nugetServerFactory,
            IApplicationDataRequestFactory applicationDataRequestFactory)
        {
            this.apiClient = apiClient;
            this.loggingService = loggingService;
            this.fileSystemService = fileSystemService;
            this.nugetServerFactory = nugetServerFactory;
            this.applicationDataRequestFactory = applicationDataRequestFactory;
        }

        #endregion

        public string GetPackages(int skip)
        {
            try
            {
                return apiClient.Get<string>($"Packages?$skip={skip}");
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(GetPackages), "Nuget sunucusundan paketlerin çekilmesi sırasında beklenmeyen bir hata oluştu.", ex);

                return null;
            }
        }

        public ICollection<NugetPackageFolder> GetNugetPackagesFolder(string filePath)
        {
            try
            {
                var directory = fileSystemService.GetDirectories(filePath).ToList();

                return nugetServerFactory.CreateNugetPackageFolders(directory);
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(GetNugetPackagesFolder), Messages.DosyaOkunamadi, ex);

                return nugetServerFactory.CreateNugetPackageFolders();
            }
        }

        public ICollection<string> GetFileList(string filePath)
        {
            try
            {
                var directory = fileSystemService.GetFilePathList(filePath).ToList();
                var files = nugetServerFactory.GetSubDirectoryFoldersName(directory);

                if (files.Count >= 1)
                {
                    return files;
                }
                else
                {
                    return new List<string>();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(GetFileList), Messages.DosyaOkunamadi, ex);

                return nugetServerFactory.CreateEmptyStringList();
            }
        }

        public ICollection<string> GetSubDirectoryFoldersName(string filePath)
        {
            try
            {
                var directory = fileSystemService.GetDirectories(filePath).ToList();

                return nugetServerFactory.GetSubDirectoryFoldersName(directory);
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(GetSubDirectoryFoldersName), Messages.DosyaOkunamadi, ex);

                return nugetServerFactory.CreateEmptyStringList();
            }
        }

        public Model.LogFileModel GetDownloadFileContents(string path)
        {
            return fileSystemService.GetDownloadFileContents(path);
        }

        #region  get nuspec template

        public Model.LogFileModel GetNuspecTemplate(string path)
        {
            try
            {
                return fileSystemService.GetFileContent(path);
            }
            catch (Exception ex)
            {
                var methodName = $"GetNuspecTemplate";
                var message = "Dosya okunamadı.";

                loggingService.LogError(methodName, message, ex);

                return null;
            }
        }

        #endregion
    }
}