using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library
{
    public class ApplicationEnvironmentViewModelFactory : IApplicationEnvironmentViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public ApplicationEnvironmentViewModelFactory(
            IBreadCrumbFactory breadCrumbFactory,
            IAuthorizationServiceWrapper authorizationWrapper)
        {
            this.breadCrumbFactory = breadCrumbFactory;
            this.authorizationWrapper = authorizationWrapper;
        }

        #endregion

        #region private methods

        bool CheckUserHasAdminDeveloperPolicy()
        {
            return authorizationWrapper.CheckUserHasAdminDeveloperPolicy();
        }

        #endregion

        #region application environment

        public ApplicationEnvironment CreateApplicationEnvironment(ApplicationEnvironment applicationEnvironment)
        {
            return new ApplicationEnvironment
            {
                ApplicationId = applicationEnvironment.ApplicationId,
                EnvironmentId = applicationEnvironment.EnvironmentId,
                HasLog = applicationEnvironment.HasLog,
                LogFilePath = applicationEnvironment.LogFilePath,
                PhysicalPath = applicationEnvironment.PhysicalPath,
                Url = applicationEnvironment.Url
            };
        }

        public ApplicationEnvironment CreateApplicationEnvironment(int applicationId, string applicationName)
        {
            return new ApplicationEnvironment
            {
                ApplicationId = applicationId,
                ApplicationName = applicationName
            };
        }

        public ApplicationEnvironmentViewModel CreateApplicationEnvironmentDetailViewModel(ApplicationEnvironment applicationEnvironment)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateApplicationEnvironmentDetailModel(applicationEnvironment.ApplicationId);
            var favouritePageName = $"{applicationEnvironment.ApplicationName} - Uygulama Ortamı - {applicationEnvironment.EnvironmentName}";

            return new ApplicationEnvironmentViewModel
            {
                ApplicationEnvironment = applicationEnvironment,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel,
                FavouritePageName = favouritePageName
            };
        }

        public ApplicationEnvironmentViewModel CreateApplicationEnvironmentViewModel(ApplicationEnvironment applicationEnvironment)
        {
            return new ApplicationEnvironmentViewModel
            {
                ApplicationEnvironment = applicationEnvironment,
            };
        }

        public ApplicationEnvironmentViewModel CreateApplicationEnvironmentViewModel(ApplicationEnvironmentViewModel model, ICollection<DevPortal.Model.Environment> environments)
        {
            return new ApplicationEnvironmentViewModel
            {
                ApplicationEnvironment = model.ApplicationEnvironment,
                EnvironmentList = environments,
                BreadCrumbViewModel = breadCrumbFactory.CreateApplicationEnvironmentModel(model.ApplicationEnvironment.ApplicationId),
            };
        }

        public ApplicationEnvironmentViewModel CreateEditApplicationEnvironmentViewModel(ApplicationEnvironment applicationEnvironment, ICollection<DevPortal.Model.Environment> environments)
        {
            return new ApplicationEnvironmentViewModel
            {
                ApplicationEnvironment = applicationEnvironment,
                EnvironmentList = environments,
                BreadCrumbViewModel = breadCrumbFactory.CreateApplicationEnvironmentEditModel(applicationEnvironment.Id, applicationEnvironment.ApplicationId),
            };
        }

        #endregion
    }
}