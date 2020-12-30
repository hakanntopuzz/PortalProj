using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Factories
{
    public class BreadCrumbFactory : IBreadCrumbFactory
    {
        #region ctor

        readonly IUrlHelper urlHelper;

        public BreadCrumbFactory(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        #endregion

        static BreadCrumbViewModel CreateBreadCrumbViewModel(string moduleName, List<BreadCrumbModel> pages)
        {
            return new BreadCrumbViewModel
            {
                ModuleName = moduleName,
                PageList = pages
            };
        }

        #region application

        public BreadCrumbViewModel CreateApplicationsModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications,GetApplicationUrl())
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateApplicationAddModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.NewApplication, GetAddApplicationUrl())
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateApplicationEditModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var applicationEditUrl = GetApplicationEditUrl(applicationId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.ApplicationUpdate, applicationEditUrl)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateApplicationDetailModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        #endregion

        #region application group

        public BreadCrumbViewModel CreateApplicationGroupsModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.ApplicationGroups,GetApplicationGroupUrl())
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateApplicationGroupAddModel()
        {
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.ApplicationGroups, GetApplicationGroupUrl()),
                BreadCrumbModel.Create(PageNames.NewApplicationGroup, GetAddApplicationGroupUrl())
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateApplicationGroupEditModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.ApplicationGroups, GetApplicationGroupUrl()),
                BreadCrumbModel.Create(PageNames.DetailApplicationGroup, GetDetailApplicationGroupUrl()),
                BreadCrumbModel.Create(PageNames.EditApplicationGroup)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateApplicationGroupDetailModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.ApplicationGroups, GetApplicationGroupUrl()),
                BreadCrumbModel.Create(PageNames.DetailApplicationGroup, GetDetailApplicationGroupUrl())
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        #endregion

        #region application environment

        public BreadCrumbViewModel CreateApplicationEnvironmentModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var addapplicationEnvironmentUrl = urlHelper.Action(ApplicationEnvironmentControllerActionNames.Add, ControllerNames.ApplicationEnvironment, new { applicationId = applicationId });
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications,GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation,applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.NewApplicationEnvironment,addapplicationEnvironmentUrl)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateApplicationEnvironmentDetailModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.ApplicationEnvironmentInformations)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateApplicationEnvironmentEditModel(int applicationEnvironmentId, int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var aapplicationEnvironmentDetailUrl = urlHelper.Action(ApplicationEnvironmentControllerActionNames.Detail, ControllerNames.ApplicationEnvironment, applicationEnvironmentId);
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications,GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation,applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.ApplicationEnvironmentInformations,aapplicationEnvironmentDetailUrl),
                BreadCrumbModel.Create(PageNames.ApplicationEnvironmentUpdate)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        #endregion

        #region application svn

        public BreadCrumbViewModel CreateNewSvnRepositoryModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.NewSVNRepository)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateDetailSvnRepositoryModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications,  GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.SVNRepositoryInformation)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateEditSvnRepositoryModel(int applicationId, int svnRepositoryId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var applicationSvnDetailUrl = urlHelper.Action(ApplicationSvnControllerActionNames.Detail, ControllerNames.ApplicationSvn, svnRepositoryId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.SVNRepositoryInformation,applicationSvnDetailUrl),
                BreadCrumbModel.Create(PageNames.SVNRepositoryUpdate)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        #endregion

        #region application jenkins

        public BreadCrumbViewModel CreateNewJenkinsJobModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.NewJenkinsQuest)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateDetailJenkinsJobModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.JenkinsTaskInformation)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateEditJenkinsJobModel(int jenkinsJobId, int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var applicationJenkinsJobDetailUrl = urlHelper.Action(ApplicationControllerActionNames.Detail, ControllerNames.ApplicationJenkinsJob, jenkinsJobId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.JenkinsTaskInformation, applicationJenkinsJobDetailUrl),
                BreadCrumbModel.Create(PageNames.JenkinsTaskUpdate)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        #endregion

        #region application sonarqube

        public BreadCrumbViewModel CreateNewSonarQubeProjectModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.NewSonarQubeProject)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateDetailSonarQubeProjectModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.SonarQubeProjectInformation)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateEditSonarQubeProjectModel(int projectId, int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var projectDetailUrl = urlHelper.Action(ApplicationSonarqubeProjectControllerActionNames.Detail, ControllerNames.ApplicationSonarqubeProject, projectId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.SonarQubeProjectInformation, projectDetailUrl),
                BreadCrumbModel.Create(PageNames.SonarQubeProjectUpdate)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        #endregion

        #region menu

        public BreadCrumbViewModel CreateMenuListModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Menu,GetMenuUrl())
            };

            return CreateBreadCrumbViewModel(ModuleNames.Management, pages);
        }

        public BreadCrumbViewModel CreateAddMenuModel()
        {
            var addMenuUrl = urlHelper.Action(ApplicationControllerActionNames.Add, ControllerNames.Menu);

            var pages = new List<BreadCrumbModel>
            {
                 BreadCrumbModel.Create(PageNames.Menu,GetMenuUrl()),
                 BreadCrumbModel.Create(PageNames.NewMenu,addMenuUrl)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Management, pages);
        }

        public BreadCrumbViewModel CreateDetailMenuModel()
        {
            var detailMenuUrl = urlHelper.Action(ApplicationControllerActionNames.Detail, ControllerNames.Menu);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Menu,GetMenuUrl()),
                BreadCrumbModel.Create(PageNames.MenuInformation, detailMenuUrl)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Management, pages);
        }

        public BreadCrumbViewModel CreateEditMenuModel(int id)
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Menu,GetMenuUrl()),
                BreadCrumbModel.Create(PageNames.MenuInformation,urlHelper.Action(ApplicationControllerActionNames.Detail, ControllerNames.Menu, id)),
                BreadCrumbModel.Create(PageNames.UpdateMenu,urlHelper.Action(ApplicationControllerActionNames.Edit, ControllerNames.Menu, id))
            };

            return CreateBreadCrumbViewModel(ModuleNames.Management, pages);
        }

        #endregion

        #region log

        public BreadCrumbViewModel CreateLogListModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.LogList,GetLogListUrl())
            };

            return CreateBreadCrumbViewModel(ModuleNames.LogManagement, pages);
        }

        public BreadCrumbViewModel CreateLogDetailModel(string physicalPath)
        {
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.LogList,GetLogListUrl()),
                BreadCrumbModel.Create(PageNames.LogInformation,urlHelper.Action(ApplicationControllerActionNames.LogDetails, ControllerNames.Log, new { path = physicalPath }))
            };

            return CreateBreadCrumbViewModel(ModuleNames.LogManagement, pages);
        }

        #endregion

        #region general settings

        public BreadCrumbViewModel CreateGeneralSettingsModel()
        {
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.GeneralSettings,urlHelper.Action(ApplicationControllerActionNames.Index, ControllerNames.GeneralSettings))
            };

            return CreateBreadCrumbViewModel(ModuleNames.Management, pages);
        }

        #endregion

        #region svn admin

        public BreadCrumbViewModel CreateSvnAdminListModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.SvnRepositories, urlHelper.Action(SvnAdminControllerActionNames.Index, ControllerNames.SvnAdmin))
            };

            return CreateBreadCrumbViewModel(ModuleNames.SVN, pages);
        }

        public BreadCrumbViewModel CreateSvnRepositoryFolderModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.SvnRepositories, urlHelper.Action(SvnAdminControllerActionNames.Index, ControllerNames.SvnAdmin)),
                BreadCrumbModel.Create(PageNames.SvnRepositoryFolder, urlHelper.Action(SvnAdminControllerActionNames.CreateSvnRepositoryFolder, ControllerNames.SvnAdmin))
            };

            return CreateBreadCrumbViewModel(ModuleNames.SVN, pages);
        }

        #endregion

        #region Security

        public BreadCrumbViewModel CreateGuidModel()
        {
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.CreateNewGuid,urlHelper.Action(SecurityControllerActionNames.Guid, ControllerNames.Security))
            };

            return CreateBreadCrumbViewModel(ModuleNames.Security, pages);
        }

        public BreadCrumbViewModel CreateHashModel()
        {
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.CreateHash)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Security, pages);
        }

        public BreadCrumbViewModel CreatePasswordModel()
        {
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.CreateNewPassword,urlHelper.Action(SecurityControllerActionNames.Password, ControllerNames.Security))
            };

            return CreateBreadCrumbViewModel(ModuleNames.Security, pages);
        }

        #endregion

        #region user

        public BreadCrumbViewModel CreateUserListModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.User,GetUserUrl()),
            };

            return CreateBreadCrumbViewModel(ModuleNames.UserManagement, pages);
        }

        public BreadCrumbViewModel CreateUserAddModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.User,GetUserUrl()),
                BreadCrumbModel.Create(PageNames.NewUser)
            };

            return CreateBreadCrumbViewModel(ModuleNames.UserManagement, pages);
        }

        public BreadCrumbViewModel CreateDetailUserModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.User,GetUserUrl()),
                BreadCrumbModel.Create(PageNames.UserInformation)
            };

            return CreateBreadCrumbViewModel(ModuleNames.UserManagement, pages);
        }

        public BreadCrumbViewModel CreateEditUserModel(int id)
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.User,GetUserUrl()),
                BreadCrumbModel.Create(PageNames.UserInformation,urlHelper.Action(UserControllerActionNames.Detail, ControllerNames.User, id)),
                BreadCrumbModel.Create(PageNames.UpdateUser,urlHelper.Action(UserControllerActionNames.Edit, ControllerNames.User, id))
            };

            return CreateBreadCrumbViewModel(ModuleNames.Management, pages);
        }

        public BreadCrumbViewModel CreateUserProfileModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.UpdateOwnUser)
            };

            return CreateBreadCrumbViewModel(ModuleNames.User, pages);
        }

        public BreadCrumbViewModel CreateChangePasswordModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.ChangePassword)
            };

            return CreateBreadCrumbViewModel(ModuleNames.User, pages);
        }

        public BreadCrumbViewModel CreateResetPasswordModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.ResetPassword)
            };

            return CreateBreadCrumbViewModel(ModuleNames.User, pages);
        }

        public BreadCrumbViewModel CreateForgotPasswordModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.ForgotPassword)
            };

            return CreateBreadCrumbViewModel(ModuleNames.User, pages);
        }

        #endregion

        #region application nuget package

        public BreadCrumbViewModel CreateDetailNugetPackageModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.NugetPackageInformation)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateNewNugetPackageModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.NewNugetPackage)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateEditNugetPackageModel(int applicationId, int nugetPackageId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var applicationNugetPackageDetailUrl = GetApplicationNugetPackageDetailUrl(nugetPackageId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.NugetPackage, applicationNugetPackageDetailUrl),
                BreadCrumbModel.Create(PageNames.NugetPackageUpdate)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        #endregion

        #region environment

        public BreadCrumbViewModel CreateEnvironmentsModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                    BreadCrumbModel.Create(PageNames.Environments,GetEnvironmentUrl())
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateEnvironmentDetailModel(int environmentId)
        {
            var environmentDetailUrl = GetEnvironmentDetailUrl(environmentId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Environments, GetEnvironmentUrl()),
                BreadCrumbModel.Create(PageNames.EnvironmentDetail, environmentDetailUrl)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateEnvironmentEditModel(int environmentId)
        {
            var environmentDetailUrl = GetEnvironmentDetailUrl(environmentId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Environments, GetEnvironmentUrl()),
                BreadCrumbModel.Create(PageNames.EnvironmentDetail, environmentDetailUrl),
                BreadCrumbModel.Create(PageNames.EnvironmentEdit)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateEnvironmentAddModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Environments, GetEnvironmentUrl()),
                BreadCrumbModel.Create(PageNames.NewEnvironment)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        #endregion

        #region database type

        public BreadCrumbViewModel CreateDatabaseTypesModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                    BreadCrumbModel.Create(PageNames.DatabaseTypes,GetDatabaseTypeUrl()),
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateDatabaseTypeAddModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                    BreadCrumbModel.Create(PageNames.DatabaseTypes,GetDatabaseTypeUrl()),
                    BreadCrumbModel.Create(PageNames.NewDatabaseType)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateDatabaseTypeDetailModel(int id)
        {
            var databaseTypeDetailUrl = GeDatabaseTypeDetailUrl(id);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.DatabaseTypes, GetDatabaseTypeUrl()),
                BreadCrumbModel.Create(PageNames.DatabaseTypeDetail, databaseTypeDetailUrl)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateDatabaseTypeEditModel(int id)
        {
            var databaseTypeDetailUrl = GeDatabaseTypeDetailUrl(id);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.DatabaseTypes, GetDatabaseTypeUrl()),
                BreadCrumbModel.Create(PageNames.DatabaseTypeDetail, databaseTypeDetailUrl),
                BreadCrumbModel.Create(PageNames.DatabaseTypeEdit)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        #endregion

        #region database

        public BreadCrumbViewModel CreateDatabasesModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Databases,GetDatabaseUrl())
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateDatabaseDetailModel(int databaseId)
        {
            var databaseDetailUrl = GetDatabaseDetailUrl(databaseId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Databases, GetDatabaseUrl()),
                BreadCrumbModel.Create(PageNames.DatabaseInformation, databaseDetailUrl)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateDatabaseEditModel(int databaseId)
        {
            var databaseDetailUrl = GetDatabaseDetailUrl(databaseId);
            var databaseEditUrl = GetDatabaseEditUrl(databaseId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Databases, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.DatabaseInformation, databaseDetailUrl),
                BreadCrumbModel.Create(PageNames.DatabaseUpdate, databaseEditUrl)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateDatabaseAddModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Databases,GetDatabaseUrl()),
                BreadCrumbModel.Create(PageNames.NewDatabase)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        #endregion

        #region database group

        public BreadCrumbViewModel CreateDatabaseGroupsModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                    BreadCrumbModel.Create(PageNames.DatabaseGroups,GetDatabaseGroupUrl())
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateDatabaseGroupAddModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                    BreadCrumbModel.Create(PageNames.DatabaseGroups,GetDatabaseGroupUrl()),
                    BreadCrumbModel.Create(PageNames.NewDatabaseGroup)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateDatabaseGroupDetailModel(int id)
        {
            var databaseGroupDetailUrl = GeDatabaseGroupDetailUrl(id);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.DatabaseGroups, GetDatabaseGroupUrl()),
                BreadCrumbModel.Create(PageNames.DatabaseGroupDetail, databaseGroupDetailUrl)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateDatabaseGroupEditModel(int id)
        {
            var databaseGroupDetailUrl = GeDatabaseGroupDetailUrl(id);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.DatabaseGroups, GetDatabaseGroupUrl()),
                BreadCrumbModel.Create(PageNames.DatabaseGroupDetail, databaseGroupDetailUrl),
                BreadCrumbModel.Create(PageNames.DatabaseGroupEdit)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        #endregion

        #region external dependency

        public BreadCrumbViewModel CreateExternalDependencyDetailModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.ExternalDependencyDetail)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateExternalDependencyAddModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.ExternalDependencyAdd)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateExternalDependencyEditModel(int applicationId, int externalDependencyId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var externalDependencyDetailUrl = GetExternalDependencyDetailUrl(externalDependencyId);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.ExternalDependencyDetail, externalDependencyDetailUrl),
                BreadCrumbModel.Create(PageNames.ExternalDependencyEdit)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        #endregion

        #region database dependency

        public BreadCrumbViewModel CreateDatabaseDependencyDetailModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.DatabaseDependencyDetail)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateDatabaseDependencyAddModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel>
            {
                    BreadCrumbModel.Create(PageNames.Applications,GetApplicationUrl()),
                    BreadCrumbModel.Create(PageNames.ApplicationInformation,applicationDetailUrl),
                    BreadCrumbModel.Create(PageNames.NewDatabaseDependency)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateDatabaseDependencyEditModel(int applicationId, int databaseDependencyId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var databaseDependencyDetailUrl = GetDatabaseDependencyDetailUrl(databaseDependencyId);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.DatabaseDependencyDetail, databaseDependencyDetailUrl),
                BreadCrumbModel.Create(PageNames.DatabaseDependencyEdit)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        #endregion

        #region application dependency

        public BreadCrumbViewModel CreateApplicationDependencyAddModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel>
            {
                    BreadCrumbModel.Create(PageNames.Applications,GetApplicationUrl()),
                    BreadCrumbModel.Create(PageNames.ApplicationInformation,applicationDetailUrl),
                    BreadCrumbModel.Create(PageNames.NewApplicationDependency)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateApplicationDependencyDetailModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.ApplicationDependencyDetail)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateApplicationDependencyEditModel(int applicationId, int applicationDependencyId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var applicationDependencyDetailUrl = GetApplicationDependencyDetailUrl(applicationDependencyId);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.ApplicationDependencyDetail, applicationDependencyDetailUrl),
                BreadCrumbModel.Create(PageNames.ApplicationDependencyEdit)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        #endregion

        #region nuget package depency

        public BreadCrumbViewModel CreateNugetPackageDependencyDetailModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.NugetPackageDependency)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        public BreadCrumbViewModel CreateNugetPackageDependencyAddModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel>
            {
                    BreadCrumbModel.Create(PageNames.Applications,GetApplicationUrl()),
                    BreadCrumbModel.Create(PageNames.ApplicationInformation,applicationDetailUrl),
                    BreadCrumbModel.Create(PageNames.NewNugetPackageDependency)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        #endregion

        #region favourite pages

        public BreadCrumbViewModel CreateFavouritePagesModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.FavouritePages)
            };

            return CreateBreadCrumbViewModel(ModuleNames.User, pages);
        }

        #endregion

        #region redmine

        public BreadCrumbViewModel CreateApplicationRedmineProjectsModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.ApplicationRedmineProjects, GetApplicationRedmineProjectsUrl())
            };

            return CreateBreadCrumbViewModel(ModuleNames.Redmine, pages);
        }

        public BreadCrumbViewModel CreateDatabaseRedmineProjectsModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.DatabaseRedmineProjects, GetDatabaseRedmineProjectsUrl())
            };

            return CreateBreadCrumbViewModel(ModuleNames.Redmine, pages);
        }

        #endregion

        public BreadCrumbViewModel CreateApplicationBuildSettingsModel(int applicationId)
        {
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.ApplicationBuildSettings)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Application, pages);
        }

        #region helper methods

        string GetApplicationUrl()
        {
            return urlHelper.Action(ApplicationControllerActionNames.Index, ControllerNames.Application);
        }

        string GetAddApplicationUrl()
        {
            return urlHelper.Action(ApplicationControllerActionNames.Add, ControllerNames.Application);
        }

        string GetApplicationDetailUrl(int applicationId)
        {
            return urlHelper.Action(ApplicationControllerActionNames.Detail, ControllerNames.Application, applicationId);
        }

        string GetApplicationNugetPackageDetailUrl(int nugetPackageId)
        {
            return urlHelper.Action(ApplicationNugetPackageControllerActionNames.Detail, ControllerNames.ApplicationNugetPackage, nugetPackageId);
        }

        string GetApplicationEditUrl(int applicationId)
        {
            return urlHelper.Action(ApplicationControllerActionNames.Edit, ControllerNames.Application, applicationId);
        }

        string GetApplicationGroupUrl()
        {
            return urlHelper.Action(ApplicationGroupControllerActionNames.Index, ControllerNames.ApplicationGroup);
        }

        string GetAddApplicationGroupUrl()
        {
            return urlHelper.Action(ApplicationGroupControllerActionNames.Add, ControllerNames.ApplicationGroup);
        }

        string GetDetailApplicationGroupUrl()
        {
            return urlHelper.Action(ApplicationControllerActionNames.Detail, ControllerNames.ApplicationGroup);
        }

        string GetMenuUrl()
        {
            return urlHelper.Action(ApplicationControllerActionNames.Index, ControllerNames.Menu);
        }

        string GetLogListUrl()
        {
            return urlHelper.Action(ApplicationControllerActionNames.Index, ControllerNames.Log);
        }

        string GetUserUrl()
        {
            return urlHelper.Action(UserControllerActionNames.Index, ControllerNames.User);
        }

        string GetEnvironmentUrl()
        {
            return urlHelper.Action(EnvironmentControllerActionNames.Index, ControllerNames.Environment);
        }

        string GetEnvironmentDetailUrl(int environmentId)
        {
            return urlHelper.Action(EnvironmentControllerActionNames.Detail, ControllerNames.Environment, environmentId);
        }

        string GetDatabaseTypeUrl()
        {
            return urlHelper.Action(DatabaseTypeControllerActionNames.Index, ControllerNames.DatabaseType);
        }

        string GeDatabaseTypeDetailUrl(int databaseTypeId)
        {
            return urlHelper.Action(DatabaseTypeControllerActionNames.Detail, ControllerNames.DatabaseType, databaseTypeId);
        }

        string GetDatabaseDetailUrl(int databaseId)
        {
            return urlHelper.Action(DatabaseControllerActionNames.Detail, ControllerNames.Database, databaseId);
        }

        string GetDatabaseUrl()
        {
            return urlHelper.Action(DatabaseControllerActionNames.Index, ControllerNames.Database);
        }

        string GetDatabaseGroupUrl()
        {
            return urlHelper.Action(DatabaseGroupControllerActionNames.Index, ControllerNames.DatabaseGroup);
        }

        string GeDatabaseGroupDetailUrl(int databaseGroupId)
        {
            return urlHelper.Action(DatabaseGroupControllerActionNames.Detail, ControllerNames.DatabaseGroup, databaseGroupId);
        }

        string GetDatabaseEditUrl(int databaseId)
        {
            return urlHelper.Action(DatabaseControllerActionNames.Edit, ControllerNames.Database, databaseId);
        }

        string GetExternalDependencyDetailUrl(int externalDependencyId)
        {
            return urlHelper.Action(ExternalDependencyControllerActionNames.Detail, ControllerNames.ExternalDependency, externalDependencyId);
        }

        string GetDatabaseDependencyDetailUrl(int databaseDependencyId)
        {
            return urlHelper.Action(DatabaseDependencyControllerActionNames.Detail, ControllerNames.DatabaseDependency, databaseDependencyId);
        }

        string GetApplicationDependencyDetailUrl(int applicationDependencyId)
        {
            return urlHelper.Action(ApplicationDependencyControllerActionNames.Detail, ControllerNames.ApplicationDependency, applicationDependencyId);
        }

        string GetApplicationRedmineProjectsUrl()
        {
            return urlHelper.Action(RedmineControllerActionNames.Index, ControllerNames.Redmine);
        }

        string GetDatabaseRedmineProjectsUrl()
        {
            return urlHelper.Action(RedmineControllerActionNames.DatabaseProjects, ControllerNames.Redmine);
        }

        string GetApplicationDeploymentPackagesUrl()
        {
            return urlHelper.Action(DeploymentPackageControllerActionNames.Index, ControllerNames.DeploymentPackage);
        }

        #endregion

        #region redmine

        public BreadCrumbViewModel CreateDeploymentPackageModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.ApplicationDeploymentPackages, GetApplicationDeploymentPackagesUrl())
            };

            return CreateBreadCrumbViewModel(ModuleNames.Deployment, pages);
        }

        public BreadCrumbViewModel CreateDatabaseDeploymentPackageModel()
        {
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.DatabaseDeploymentPackages)
            };

            return CreateBreadCrumbViewModel(ModuleNames.Deployment, pages);
        }

        #endregion
    }
}