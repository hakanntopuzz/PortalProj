using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library
{
    public class ApplicationJenkinsJobViewModelFactory : IApplicationJenkinsJobViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public ApplicationJenkinsJobViewModelFactory(
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

        #region application jenkins jobs

        public ApplicationJenkinsJobViewModel CreateApplicationJenkinsJobViewModel(Application application, ICollection<JenkinsJobType> jenkinsJobTypeList, string jenkinsJobUrl)
        {
            return new ApplicationJenkinsJobViewModel
            {
                ApplicationJenkinsJob = new JenkinsJob
                {
                    ApplicationId = application.Id,
                    ApplicationName = application.Name
                },
                JenkinsJobTypeList = jenkinsJobTypeList,
                JenkinsJobUrl = jenkinsJobUrl,
                BreadCrumbViewModel = breadCrumbFactory.CreateNewJenkinsJobModel(application.Id)
            };
        }

        public ApplicationJenkinsJobViewModel CreateApplicationJenkinsJobViewModel(Application application, JenkinsJob jenkinsJob, ICollection<JenkinsJobType> jenkinsJobTypeList, string jenkinsJobUrl)
        {
            var model = new ApplicationJenkinsJobViewModel
            {
                ApplicationJenkinsJob = jenkinsJob,
                JenkinsJobTypeList = jenkinsJobTypeList,
                JenkinsJobUrl = jenkinsJobUrl,
                BreadCrumbViewModel = breadCrumbFactory.CreateNewJenkinsJobModel(application.Id)
            };
            model.ApplicationJenkinsJob.ApplicationName = application.Name;

            return model;
        }

        public ApplicationJenkinsJobViewModel CreateDetailApplicationJenkinsJobViewModel(Application application, JenkinsJob jenkinsJob, ICollection<JenkinsJobType> jenkinsJobTypeList, string jenkinsJobUrl)
        {
            if (application == null || jenkinsJob == null)
            {
                return null;
            }

            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateDetailJenkinsJobModel(application.Id);
            var favouritePageName = $"{application.Name} - Jenkins Görevi - {jenkinsJob.JenkinsJobName}";

            var model = new ApplicationJenkinsJobViewModel
            {
                ApplicationJenkinsJob = jenkinsJob,
                JenkinsJobTypeList = jenkinsJobTypeList,
                JenkinsJobUrl = jenkinsJobUrl,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel,
                FavouritePageName = favouritePageName
            };

            model.ApplicationJenkinsJob.ApplicationName = application.Name;

            return model;
        }

        public ApplicationJenkinsJobViewModel CreateEditApplicationJenkinsJobViewModel(Application application, JenkinsJob jenkinsJob, ICollection<JenkinsJobType> jenkinsJobTypeList, string jenkinsJobUrl)
        {
            var model = new ApplicationJenkinsJobViewModel
            {
                ApplicationJenkinsJob = jenkinsJob,
                JenkinsJobTypeList = jenkinsJobTypeList,
                JenkinsJobUrl = jenkinsJobUrl,
                BreadCrumbViewModel = breadCrumbFactory.CreateEditJenkinsJobModel(jenkinsJob.JenkinsJobId, application.Id)
            };
            model.ApplicationJenkinsJob.ApplicationName = application.Name;

            return model;
        }

        #endregion
    }
}