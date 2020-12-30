using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library
{
    public class ApplicationViewModelFactory : IApplicationViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public ApplicationViewModelFactory(
            IBreadCrumbFactory breadCrumbFactory,
            IAuthorizationServiceWrapper authorizationWrapper)
        {
            this.breadCrumbFactory = breadCrumbFactory;
            this.authorizationWrapper = authorizationWrapper;
        }

        #endregion

        bool CheckUserHasAdminDeveloperPolicy()
        {
            return authorizationWrapper.CheckUserHasAdminDeveloperPolicy();
        }

        #region application

        public ApplicationViewModel CreateApplication(Application application)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateApplicationDetailModel(application.Id);
            var favouritePageName = $"{PageNames.ApplicationInformation} - {application.Name}";

            return new ApplicationViewModel
            {
                Application = application,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel,
                FavouritePageName = favouritePageName
            };
        }

        public EditApplicationViewModel CreateEditApplication(
            Application application,
            ICollection<ApplicationGroup> applicationGroups,
            ICollection<ApplicationType> applicationTypes,
            ICollection<ApplicationStatus> applicationStatusList)
        {
            return new EditApplicationViewModel
            {
                ApplicationGroups = applicationGroups,
                ApplicationTypes = applicationTypes,
                ApplicationStatusList = applicationStatusList,
                Application = application,
                BreadCrumbViewModel = breadCrumbFactory.CreateApplicationEditModel(application.Id)
            };
        }

        public ApplicationsViewModel CreateApplicationsViewModel(ICollection<ApplicationGroup> applicationGroups, ICollection<ApplicationListItem> applications)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateApplicationsModel();

            return new ApplicationsViewModel
            {
                ApplicationGroups = applicationGroups,
                Applications = applications,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        public AddApplicationViewModel CreateApplicationAddViewModel(
            ICollection<ApplicationGroup> applicationGroups,
            ICollection<ApplicationType> applicationTypes,
            ICollection<ApplicationStatus> applicationStatus)
        {
            return new AddApplicationViewModel
            {
                ApplicationGroups = applicationGroups,
                ApplicationTypes = applicationTypes,
                ApplicationStatus = applicationStatus,
                BreadCrumbViewModel = breadCrumbFactory.CreateApplicationAddModel()
            };
        }

        public ApplicationsViewModel CreateApplicationsViewModelWithFilter(
            ICollection<ApplicationGroup> applicationGroups,
            ICollection<ApplicationListItem> applications,
            int applicationGroupId,
            string applicationName)
        {
            return new ApplicationsViewModel
            {
                ApplicationGroups = applicationGroups,
                Applications = applications,
                ApplicationGroupId = applicationGroupId,
                ApplicationName = applicationName,
                BreadCrumbViewModel = breadCrumbFactory.CreateApplicationsModel()
            };
        }

        public ApplicationEnvironment CreateApplicationEnvironment(ApplicationEnvironment application)
        {
            return new ApplicationEnvironment
            {
                ApplicationId = application.ApplicationId,
                EnvironmentId = application.EnvironmentId,
                HasLog = application.HasLog,
                LogFilePath = application.LogFilePath,
                PhysicalPath = application.PhysicalPath,
                Url = application.Url
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

        public ApplicationListModel CreateApplicationListModel(IEnumerable<Application> applicationList)
        {
            return new ApplicationListModel
            {
                data = applicationList
            };
        }

        public ApplicationFullModel CreateApplicationFullModel(Application application,
            ICollection<ApplicationEnvironment> applicationEnvironments,
            ICollection<SvnRepository> svnRepositories,
            ICollection<JenkinsJob> jenkinsJobs,
            ICollection<SonarqubeProject> sonarqubeProjects,
            ICollection<ApplicationNugetPackage> applicationNugetPackages)
        {
            return new ApplicationFullModel
            {
                Application = application,
                ApplicationEnvironments = applicationEnvironments,
                ApplicationNugetPackages = applicationNugetPackages,
                JenkinsJobs = jenkinsJobs,
                SonarqubeProjects = sonarqubeProjects,
                SvnRepositories = svnRepositories
            };
        }

        #endregion
    }
}