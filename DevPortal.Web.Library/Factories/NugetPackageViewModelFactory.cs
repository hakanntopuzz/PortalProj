using DevPortal.Framework.Abstract;
using DevPortal.NugetManager.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library
{
    public class NugetPackageViewModelFactory : INugetPackageViewModelFactory
    {
        #region ctor

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public NugetPackageViewModelFactory(
            IAuthorizationServiceWrapper authorizationWrapper)
        {
            this.authorizationWrapper = authorizationWrapper;
        }

        #endregion

        bool CheckUserHasAdminPolicy()
        {
            return authorizationWrapper.CheckUserHasAdminPolicy();
        }

        public NugetPackageFolderViewModel CreateNugetPackageFolderViewModel(List<NugetPackageFolder> nugetPackages)
        {
            var isAuthorized = CheckUserHasAdminPolicy();

            return new NugetPackageFolderViewModel
            {
                NugetPackageFolders = nugetPackages,
                IsAuthorized = isAuthorized
            };
        }

        public NugetPackageViewModel CreateNugetPackageViewModel(List<NugetPackage> nugetPackages)
        {
            var isAuthorized = CheckUserHasAdminPolicy();

            return new NugetPackageViewModel
            {
                NugetPackages = nugetPackages,
                IsAuthorized = isAuthorized
            };
        }
    }
}