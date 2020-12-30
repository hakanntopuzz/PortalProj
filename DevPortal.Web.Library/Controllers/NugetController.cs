using DevPortal.Model;
using DevPortal.NugetManager.Business.Abstract;
using DevPortal.NugetManager.Business.Abstract.Service;
using DevPortal.NugetManager.Model;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Controllers
{
    [ApiController]
    public class NugetController : ControllerBase
    {
        #region ctor

        readonly INugetService nugetService;

        readonly INugetArchiveService nugetArchiveService;

        readonly INuspecService nuspecService;

        readonly IHostEnvironment hostEnvironment;

        public NugetController(
                    INugetService nugetService,
                    INugetArchiveService nugetArchiveService,
                    INuspecService nuspecService,
                    IHostEnvironment hostEnvironment)
        {
            this.nugetService = nugetService;
            this.nugetArchiveService = nugetArchiveService;
            this.nuspecService = nuspecService;
            this.hostEnvironment = hostEnvironment;
        }

        #endregion

        #region get filtered nuget packages

        [HttpGet, Route("/get-filtered-nuget-packages")]
        public FilteredNugetPackages GetFilteredNugetPackages(int skip, int take, string searchString, int orderBy)
        {
            return nugetService.GetFilteredNugetPackages(skip, take, searchString, orderBy);
        }

        #endregion

        #region get nuget packages by title

        [HttpGet, Route("/get-nuget-packages-by-title")]
        public NugetPackageViewModel GetNugetPackagesByTitle(string packageTitle)
        {
            return nugetService.GetNugetPackagesByTitle(packageTitle);
        }

        #endregion

        #region get filtered archive nuget packages

        [HttpGet, Route("/get-filtered-archive-nuget-packages")]
        public List<NugetPackageFolder> GetFilteredFromArchivedNugetPackages(string searchString)
        {
            return nugetArchiveService.GetFilteredFromArchivedNugetPackages(searchString);
        }

        #endregion

        #region get filtered old nuget packages

        [HttpGet, Route("/get-filtered-old-nuget-packages")]
        public NugetPackageFolderViewModel GetFilteredFromOldNugetPackages(string searchString)
        {
            return nugetService.GetFilteredOldNugetPackages(searchString);
        }

        #endregion

        #region nupkg download

        [Route("/download-nuget-packages")]
        public LogFileModel NupkgDownload(string path, string fileName)
        {
            return nugetArchiveService.GetNugetPackagesNupkgFileContents(path, fileName);
        }

        #endregion

        #region nuspec download

        [Route("/download-nuspec-template")]
        public LogFileModel NuspecDownload(string path, string fileName)
        {
            string filePath = GetNuspecFilePath(path);
            return nuspecService.GetNuspecFileContents(filePath, fileName);
        }

        #endregion

        #region get nuspec template

        [Route("/get-nuspec-template")]
        public LogFileModel GetNuspecTemplate(string path)
        {
            string filePath = GetNuspecFilePath(path);
            return nugetService.GetNuspecTemplate(filePath);
        }

        #endregion

        #region get nuspec file path

        string GetNuspecFilePath(string path)
        {
            return string.Concat(hostEnvironment.ContentRootPath, "/wwwroot", path);
        }

        #endregion

        #region archive package

        [HttpGet, Route("/add-archive-nupkg-file")]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public bool ArchivePackage(string title, string versionItem)
        {
            return nugetArchiveService.CreatePackageFolder(title, versionItem);
        }

        #endregion

        #region export nuget package

        [Route("/export-nuget-packages")]
        public byte[] ExportNugetPackages(string searchString, int totalCount)
        {
            var nugetPackageList = GetFilteredNugetPackages(0, totalCount, searchString, 0);
            var result = nugetService.ExportNugetPackagesAsCsv(nugetPackageList.NugetPackages);
            return result.Value;
        }

        #endregion

        #region move nuget packages to old nuget package

        [Route("/move-nuget-packages-to-old-packages-file")]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public bool MoveNugetPackageToOldPackagesFile(string title)
        {
            return nugetService.MoveNugetPackageToOldPackagesFile(title);
        }

        #endregion

        #region move old nuget package  to nuget package

        [Route("/move-old-package-to-nuget-packages-file")]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public bool MoveOldPackageToNugetPackagesFile(string title)
        {
            return nugetService.MoveOldPackageToNugetPackagesFile(title);
        }

        #endregion

        #region upload new nuget package

        [Route("/upload-new-nuget-package")]
        [PolicyBasedAuthorize(Policy.AdminDeveloper)]
        public bool UploadNewNugetPackage(string fileName)
        {
            return nugetService.UploadNewNugetPackage(fileName);
        }

        #endregion
    }
}