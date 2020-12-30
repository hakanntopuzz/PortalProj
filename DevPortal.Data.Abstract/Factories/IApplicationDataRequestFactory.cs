using AB.Data.DapperClient.Model.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract.Factories
{
    public interface IApplicationDataRequestFactory
    {
        IDataRequest GetApplicationsByApplicationGroupId(int applicationGroupId);

        IDataRequest GetApplicationEnvironments(int applicationId);

        IDataRequest GetEnvironmentsDoesNotExistByApplicationId(int applicationId);

        IDataRequest GetApplicationGroups();

        IDataRequest GetApplicationTypes();

        IDataRequest GetApplications();

        IDataRequest GetApplicationsByGroupIdAndName(int applicationGroupId, string applicationName);

        IDataRequest GetApplicationsByGroupId(int applicationGroupId);

        IDataRequest GetApplicationGroupByName(string name);

        IDataRequest GetApplicationsByApplicationName(string applicationName);

        IDataRequest GetApplicationGroupList();

        IDataRequest AddApplicationGroup(ApplicationGroup applicationGroup);

        IDataRequest AddApplicationEnvironment(ApplicationEnvironment applicationEnvironment);

        IDataRequest AddApplication(Application application);

        IDataRequest GetApplication(int applicationId);

        IDataRequest GetSvnRepositories(int applicationId);

        IDataRequest GetJenkinsJobs(int applicationId);

        IDataRequest AddApplicationJenkinsJob(JenkinsJob applicationJenkinsJob);

        IDataRequest DeleteApplicationJenkinsJob(int jenkinsJobId);

        IDataRequest AddApplicationSvnRepository(SvnRepository svnRepository);

        IDataRequest UpdateApplicationSvnRepository(SvnRepository svnRepository);

        IDataRequest DeleteApplicationSvnRepository(int svnRepositoryId);

        IDataRequest GetApplicationSvnRepositoryById(int svnRepositoryId);

        IDataRequest GetApplicationSvnRepositoryUpdateInfo(int repositoryId);

        IDataRequest GetSvnRepositoryTypes();

        IDataRequest GetGeneralSettings();

        IDataRequest UpdateGeneralSettings(GeneralSettings generalSettings);

        IDataRequest GetApplicationEnvironmentById(int applicationEnvironmentId);

        IDataRequest GetApplicationEnvironmentByEnvironmentId(int applicationId, int environmentId);

        IDataRequest GetApplicationEnvironmentUpdateInfo(int environmentId);

        IDataRequest UpdateApplicationEnvironment(ApplicationEnvironment applicationEnvironment);

        IDataRequest GetEnvironments();

        IDataRequest GetEnvironmentById(int id);

        IDataRequest DeleteApplicationEnvironment(int environmentId);

        IDataRequest UpdateApplication(Application application);

        IDataRequest GetApplicationGroupById(int id);

        IDataRequest UpdateApplicationGroup(ApplicationGroup group);

        IDataRequest GetApplicationGroupStatusList();

        IDataRequest GetApplicationStatusList();

        IDataRequest GetJenkinsJobTypes();

        IDataRequest DeleteApplicationGroup(int groupId);

        IDataRequest GetApplicationJenkinsJobById(int jenkinsJobId);

        IDataRequest GetApplicationJenkinsJobUpdateInfo(int jenkinsJobId);

        IDataRequest UpdateApplicationJenkinsJob(JenkinsJob applicationJenkinsJob);

        IDataRequest DeleteApplication(int applicationId);

        IDataRequest GetApplicationByApplicationName(string name);

        IDataRequest GetApplicationCount();

        IDataRequest GetApplicationGroupCount();

        IDataRequest GetApplicationCountByType();

        IDataRequest GetJenkinsJobCountByType();

        IDataRequest GetSonarQubeProjectCountByType();

        IDataRequest GetJenkinsJobCount();

        IDataRequest GetSvnRepositoryCount();

        IDataRequest GetSonarQubeProjectCount();

        IDataRequest GetApplicationRedmineProjectCount();

        IDataRequest GetDatabaseRedmineProjectCount();

        IDataRequest GetNugetPackageCount();

        IDataRequest GetSonarqubeProjects(int applicationId);

        IDataRequest GetLastUpdatedApplications();

        IDataRequest GetApplicationSonarQubeProjectById(int projectId);

        IDataRequest GetApplicationSonarQubeProjectUpdateInfo(int projectId);

        IDataRequest GetSonarQubeProjectTypes();

        IDataRequest AddApplicationSonarQubeProject(SonarqubeProject project);

        IDataRequest DeleteApplicationSonarQubeProject(int projectId);

        IDataRequest UpdateApplicationSonarQubeProject(SonarqubeProject project);

        IDataRequest GetFilteredMenuList(MenuTableParam tableParam);

        IDataRequest GetMenuList();

        IDataRequest AddMenu(MenuModel menuModel);

        IDataRequest GetMenu(int id);

        IDataRequest GetSubMenuList(int id);

        IDataRequest UpdateMenu(MenuModel menuModel);

        IDataRequest DeleteMenu(int menuId);

        IDataRequest GetFilteredApplicationList(int skip, int take, string orderBy, string orderDir, string searchText, int applicationGroupId);

        IDataRequest GetFavouriteRedmineProjects();

        IDataRequest GetFavouriteRedmineWikiPages();

        IDataRequest GetToolPages();

        IDataRequest GetUser(int id);

        IDataRequest GetFilteredUserList(UserSearchFilter searchFilter);

        IDataRequest GetUserStatusList();

        IDataRequest GetUserTypeList();

        IDataRequest AddUser(User user);

        IDataRequest AddUserLogOnLog(UserLogOnLog userLogOnLog);

        IDataRequest GetUser(string svnUserName);

        IDataRequest GetUserByEmailAddress(string emailAddress);

        IDataRequest UpdateUser(User user);

        IDataRequest UpdateOwnUser(User user);

        IDataRequest AddAudit(string tableName, int recordId, string fieldName, string oldValue, string newValue, int modifiedBy);

        IDataRequest GetFilteredAuditList(int skip, int take, string orderBy, string orderDir, string searchText, string tableName, int recordId);

        IDataRequest DeleteUser(int userId);

        IDataRequest GetUserUpdateInfo(int id);

        IDataRequest AddPasswordResetRequest(int userId, string ipAddress, string requestCode);

        IDataRequest CheckPasswordResetRequest(string requestCode);

        IDataRequest UpdatePasswordResetRequest(int userId);

        IDataRequest GetNugetPackages(int applicationId);

        IDataRequest GetApplicationNugetPackageById(int packageId);

        IDataRequest GetApplicationByJenkinsJobName(string name);

        IDataRequest GetPackageUpdateInfo(int id);

        IDataRequest AddApplicationNugetPackage(ApplicationNugetPackage package);

        IDataRequest UpdateApplicationNugetPackage(ApplicationNugetPackage nugetPackage);

        IDataRequest GetApplicationNugetPackageByName(string packageName);

        IDataRequest DeleteApplicationNugetPackage(int packageId);

        IDataRequest GetApplicationGroupUpdateInfo(int applicationGroupId);

        IDataRequest GetApplicationUpdateInfo(int applicationId);

        IDataRequest GetEnvironmentByName(string name);

        IDataRequest AddEnvironment(Environment environment);

        IDataRequest GetEnvironmentUpdateInfo(int id);

        IDataRequest UpdateEnvironment(Environment environment);

        IDataRequest DeleteEnvironment(int environmentId);

        IDataRequest DeleteExternalDependency(ExternalDependency externalDependency);

        IDataRequest GetApplicationEnvironmentCountByEnvironmentId(int environmentId);

        IDataRequest GetExternalDependencyById(int id);

        IDataRequest GetExternalDependencyUpdateInfo(int id);

        IDataRequest GetNugetPackageDependencyUpdateInfo(int id);

        IDataRequest AddExternalDependency(ExternalDependency externalDependency);

        IDataRequest UpdateExternalDependency(ExternalDependency externalDependency);

        IDataRequest GetDatabaseTypes();

        IDataRequest GetDatabaseTypeByName(string name);

        IDataRequest AddDatabaseType(DatabaseType databaseType);

        IDataRequest UpdateDatabaseType(DatabaseType databaseType);

        IDataRequest GetDatabaseTypeById(int id);

        IDataRequest GetDatabaseTypeUpdateInfo(int id);

        IDataRequest DeleteDatabaseType(int databaseTypeId);

        IDataRequest GetDatabaseCountByDatabaseTypeId(int databaseTypeId);

        IDataRequest GetDatabases();

        IDataRequest GetDatabaseGroups();

        IDataRequest GetDatabase(int databaseId);

        IDataRequest GetDatabaseGroupByName(string name);

        IDataRequest AddDatabaseGroup(DatabaseGroup databaseGroup);

        IDataRequest GetDatabaseGroupById(int id);

        IDataRequest GetDatabaseGroupUpdateInfo(int id);

        IDataRequest UpdateDatabaseGroup(DatabaseGroup databaseGroup);

        IDataRequest GetFilteredDatabaseList(int skip, int take, string orderBy, string orderDir, string searchText, int databaseGroupId);

        IDataRequest GetDatabaseUpdateInfo(int databaseId);

        IDataRequest UpdateDatabase(Database database);

        IDataRequest GetDatabaseByDatabaseName(string name);

        IDataRequest AddDatabase(Database database);

        IDataRequest GetDatabaseByDatabaseTypeId(int id);

        IDataRequest DeleteDatabase(int databaseId);

        IDataRequest GetDatabasesByDatabaseGroupId(int groupId);

        IDataRequest GetDatabasesByDatabaseName(string databaseName);

        IDataRequest GetDatabasesByGroupIdAndName(int databaseGroupId, string databaseName);

        IDataRequest GetApplicationsWithLogByApplicationGroup(int applicationGroupId);

        IDataRequest GetMenuGroups();

        IDataRequest GetApplicationBuildVariable(int applicationId);

        IDataRequest BuildTypes { get; }

        IDataRequest GetDatabaseDependenciesByApplicationId(int applicationId);

        IDataRequest GetDatabaseCountByDatabaseGroupId(int databaseGroupId);

        IDataRequest DeleteDatabaseGroup(int databaseGroupId);

        IDataRequest GetApplicationDependenciesByApplicationId(int applicationId);

        IDataRequest GetExternalDependenciesByApplicationId(int applicationId);

        IDataRequest GetMenuUpdateInfo(int id);

        IDataRequest GetDatabaseDependencyById(int id);

        IDataRequest GetDatabaseDependencyUpdateInfo(int id);

        IDataRequest AddDatabaseDependency(DatabaseDependency databaseDependency);

        IDataRequest UpdateDatabaseDependency(DatabaseDependency databaseDependency);

        IDataRequest GetApplicationDependencies(int applicationId);

        IDataRequest GetFullDatabaseDependenciesByApplicationId(int applicationId);

        IDataRequest GetFullExternalDependenciesByApplicationId(int applicationId);

        IDataRequest GetApplicationDependencyById(int id);

        IDataRequest AddApplicationDependency(ApplicationDependency applicationDependency);

        IDataRequest DeleteDatabaseDependency(int databaseDependencyId);

        IDataRequest GetApplicationDependencyUpdateInfo(int applicationId);

        IDataRequest UpdateApplicationDependency(ApplicationDependency applicationDependency);

        IDataRequest DeleteApplicationDependency(int applicationDependencyId);

        IDataRequest GetApplicationTypeByApplicationId(int applicationId);

        IDataRequest GetFavouritePagesByUserId(int userId);

        IDataRequest AddFavouritePage(FavouritePage favouritePage);

        IDataRequest GetFavouritePages(int userId);

        IDataRequest GetLargestFavouritePageOrderByUserId(int userId);

        IDataRequest IsPageInFavourites(int userId, string pageUrl);

        IDataRequest DeleteFavouritePage(int favouriteId);

        IDataRequest GetFavouritePage(int userId, string pageUrl);

        IDataRequest GetFilteredApplicationRedmineProjectList(int skip, int take, string orderBy, string orderDir, string searchText, int applicationGroupId);

        IDataRequest GetFilteredDatabaseRedmineProjectList(int skip, int take, string orderBy, string orderDir, string searchText, int databaseGroupId);

        IDataRequest SortFavouritePages(List<int> pageIdList);

        IDataRequest GetNugetPackageDependenciesByApplicationId(int applicationId);

        IDataRequest GetDatabaseDependencies(int applicationId);

        IDataRequest GetNugetPackageDependenciesById(int id);

        IDataRequest AddNugetPackageDependency(NugetPackageDependency nugetPackageDependency);

        IDataRequest GetExternalDependencies(int applicationId);

        IDataRequest GetNugetPackageDependencies(int applicationId);

        IDataRequest DeleteNugetPackageDependency(int nugetPackageDependencyId);

        IDataRequest GetApplicationBuildSettings(int applicationId);

        IDataRequest AddApplicationBuildSettings(ApplicationBuildSettings buildSettings);

        IDataRequest UpdateApplicationBuildSettings(ApplicationBuildSettings buildSettings);

        IDataRequest GetApplicationBuildSettingsUpdateInfo(int applicationId);
    }
}