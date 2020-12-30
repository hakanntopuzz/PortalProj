using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library
{
    public class ApplicationSonarQubeProjectViewModelFactory : IApplicationSonarQubeProjectViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public ApplicationSonarQubeProjectViewModelFactory(
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

        #region  application sonarQube project

        public ApplicationSonarQubeProjectViewModel CreateApplicationSonarQubeProjectViewModel(Application application, ICollection<SonarQubeProjectType> projectTypeList, string projectUrl)
        {
            return new ApplicationSonarQubeProjectViewModel
            {
                ApplicationSonarQubeProject = new SonarqubeProject
                {
                    ApplicationId = application.Id,
                    ApplicationName = application.Name
                },
                SonarQubeProjectTypeList = projectTypeList,
                SonarQubeProjectUrl = projectUrl,
                BreadCrumbViewModel = breadCrumbFactory.CreateNewSonarQubeProjectModel(application.Id)
            };
        }

        public ApplicationSonarQubeProjectViewModel CreateApplicationSonarQubeProjectViewModel(Application application, SonarqubeProject project, ICollection<SonarQubeProjectType> projectTypeList, string projectUrl)
        {
            if (application == null || project == null)
            {
                return null;
            }

            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateDetailSonarQubeProjectModel(application.Id);
            var favouritePageName = $"{application.Name} - SonarQube Projesi - {project.SonarqubeProjectName}";

            var model = new ApplicationSonarQubeProjectViewModel
            {
                ApplicationSonarQubeProject = project,
                SonarQubeProjectTypeList = projectTypeList,
                SonarQubeProjectUrl = projectUrl,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel,
                FavouritePageName = favouritePageName
            };

            model.ApplicationSonarQubeProject.ApplicationName = application.Name;

            return model;
        }

        public ApplicationSonarQubeProjectViewModel CreateEditApplicationSonarQubeProjectViewModel(Application application, SonarqubeProject project, ICollection<SonarQubeProjectType> projectTypeList, string projectUrl)
        {
            var model = new ApplicationSonarQubeProjectViewModel
            {
                ApplicationSonarQubeProject = project,
                SonarQubeProjectTypeList = projectTypeList,
                SonarQubeProjectUrl = projectUrl,
                BreadCrumbViewModel = breadCrumbFactory.CreateEditSonarQubeProjectModel(project.SonarqubeProjectId, application.Id)
            };
            model.ApplicationSonarQubeProject.ApplicationName = application.Name;

            return model;
        }

        #endregion
    }
}