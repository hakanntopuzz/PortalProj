using DevPortal.Web.Library.Model;

namespace DevPortal.Web.Library.Abstract
{
    public interface IBreadCrumbFactory
    {
        #region application

        BreadCrumbViewModel CreateApplicationsModel();

        BreadCrumbViewModel CreateApplicationAddModel();

        BreadCrumbViewModel CreateApplicationEditModel(int applicationId);

        BreadCrumbViewModel CreateApplicationDetailModel(int applicationId);

        BreadCrumbViewModel CreateNewSvnRepositoryModel(int applicationId);

        BreadCrumbViewModel CreateEditSvnRepositoryModel(int applicationId, int svnRepositoryId);

        BreadCrumbViewModel CreateDetailSvnRepositoryModel(int applicationId);

        BreadCrumbViewModel CreateNewJenkinsJobModel(int applicationId);

        BreadCrumbViewModel CreateDetailJenkinsJobModel(int applicationId);

        BreadCrumbViewModel CreateEditJenkinsJobModel(int jenkinsJobId, int applicationId);

        BreadCrumbViewModel CreateDetailSonarQubeProjectModel(int applicationId);

        BreadCrumbViewModel CreateNewSonarQubeProjectModel(int applicationId);

        BreadCrumbViewModel CreateEditSonarQubeProjectModel(int projectId, int applicationId);

        BreadCrumbViewModel CreateDetailNugetPackageModel(int applicationId);

        BreadCrumbViewModel CreateNewNugetPackageModel(int applicationId);

        BreadCrumbViewModel CreateEditNugetPackageModel(int applicationId, int nugetPackageId);

        #endregion

        #region application group

        BreadCrumbViewModel CreateApplicationGroupsModel();

        BreadCrumbViewModel CreateApplicationGroupAddModel();

        BreadCrumbViewModel CreateApplicationGroupEditModel();

        BreadCrumbViewModel CreateApplicationGroupDetailModel();

        #endregion

        #region application environment

        BreadCrumbViewModel CreateApplicationEnvironmentModel(int applicationId);

        BreadCrumbViewModel CreateApplicationEnvironmentDetailModel(int applicationId);

        BreadCrumbViewModel CreateApplicationEnvironmentEditModel(int applicationEnvironmentId, int applicationId);

        #endregion

        #region menu

        BreadCrumbViewModel CreateMenuListModel();

        BreadCrumbViewModel CreateAddMenuModel();

        BreadCrumbViewModel CreateDetailMenuModel();

        BreadCrumbViewModel CreateEditMenuModel(int id);

        #endregion

        #region log

        BreadCrumbViewModel CreateLogListModel();

        BreadCrumbViewModel CreateLogDetailModel(string physicalPath);

        #endregion

        #region general settings

        BreadCrumbViewModel CreateGeneralSettingsModel();

        #endregion

        #region svn admin

        BreadCrumbViewModel CreateSvnAdminListModel();

        BreadCrumbViewModel CreateSvnRepositoryFolderModel();

        #endregion

        #region security

        BreadCrumbViewModel CreateGuidModel();

        BreadCrumbViewModel CreateHashModel();

        BreadCrumbViewModel CreatePasswordModel();

        #endregion

        #region user

        BreadCrumbViewModel CreateUserListModel();

        BreadCrumbViewModel CreateUserAddModel();

        BreadCrumbViewModel CreateDetailUserModel();

        BreadCrumbViewModel CreateEditUserModel(int id);

        BreadCrumbViewModel CreateChangePasswordModel();

        BreadCrumbViewModel CreateResetPasswordModel();

        BreadCrumbViewModel CreateForgotPasswordModel();

        BreadCrumbViewModel CreateUserProfileModel();

        #endregion

        #region environments

        BreadCrumbViewModel CreateEnvironmentsModel();

        BreadCrumbViewModel CreateEnvironmentDetailModel(int environmentId);

        BreadCrumbViewModel CreateEnvironmentEditModel(int environmentId);

        BreadCrumbViewModel CreateEnvironmentAddModel();

        #endregion

        #region database types

        BreadCrumbViewModel CreateDatabaseTypesModel();

        BreadCrumbViewModel CreateDatabaseTypeAddModel();

        BreadCrumbViewModel CreateDatabaseTypeDetailModel(int id);

        BreadCrumbViewModel CreateDatabaseTypeEditModel(int id);

        #endregion

        #region database

        BreadCrumbViewModel CreateDatabasesModel();

        BreadCrumbViewModel CreateDatabaseDetailModel(int databaseId);

        BreadCrumbViewModel CreateDatabaseEditModel(int databaseId);

        BreadCrumbViewModel CreateDatabaseAddModel();

        #endregion

        #region database groups

        BreadCrumbViewModel CreateDatabaseGroupsModel();

        BreadCrumbViewModel CreateDatabaseGroupAddModel();

        BreadCrumbViewModel CreateDatabaseGroupDetailModel(int id);

        BreadCrumbViewModel CreateDatabaseGroupEditModel(int id);

        #endregion

        #region external dependency

        BreadCrumbViewModel CreateExternalDependencyDetailModel(int applicationId);

        BreadCrumbViewModel CreateExternalDependencyAddModel(int applicationId);

        BreadCrumbViewModel CreateExternalDependencyEditModel(int applicationId, int externalDependencyId);

        #endregion

        #region database dependency

        BreadCrumbViewModel CreateDatabaseDependencyDetailModel(int applicationId);

        BreadCrumbViewModel CreateDatabaseDependencyAddModel(int applicationId);

        BreadCrumbViewModel CreateDatabaseDependencyEditModel(int applicationId, int databaseDependencyId);

        #endregion

        #region application dependency

        BreadCrumbViewModel CreateApplicationDependencyAddModel(int applicationId);

        BreadCrumbViewModel CreateApplicationDependencyDetailModel(int applicationId);

        BreadCrumbViewModel CreateApplicationDependencyEditModel(int applicationId, int applicationDependencyId);

        #endregion

        #region favourite pages

        BreadCrumbViewModel CreateFavouritePagesModel();

        #endregion

        #region redmine

        BreadCrumbViewModel CreateApplicationRedmineProjectsModel();

        BreadCrumbViewModel CreateDatabaseRedmineProjectsModel();

        #endregion

        #region nuget package dependency

        BreadCrumbViewModel CreateNugetPackageDependencyDetailModel(int applicationId);

        BreadCrumbViewModel CreateNugetPackageDependencyAddModel(int applicationId);

        #endregion

        BreadCrumbViewModel CreateApplicationBuildSettingsModel(int applicationId);

        BreadCrumbViewModel CreateDeploymentPackageModel();

        BreadCrumbViewModel CreateDatabaseDeploymentPackageModel();
    }
}