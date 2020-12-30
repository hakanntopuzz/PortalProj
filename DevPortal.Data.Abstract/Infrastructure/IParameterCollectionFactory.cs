using AB.Data.DapperClient.Model.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract.Infrastructure
{
    //TODO: ParameterCollection ile başlayan metotlar arayüzde olmamalı. Private olarak kullanılmalı. Bunlar yerine Create ile başlayan, istemci metodun adıyla devam eden metotlar oluşturulmalı.
    public interface IParameterCollectionFactory
    {
        IParameterCollection ParameterCollectionWithApplicationId(int applicationId);

        IParameterCollection ParameterCollectionWithApplicationGroupId(int applicationGroupId);

        IParameterCollection ParameterCollectionWithApplicationGroupIdAndName(int applicationGroupId, string applicationName);

        IParameterCollection ParameterCollectionWithName(string applicationName);

        IParameterCollection CreateAddApplicationGroupParameters(ApplicationGroup applicationGroup);

        IParameterCollection CreateAddApplicationParameters(Application application);

        IParameterCollection CreateGeneralSettingsParameters(GeneralSettings generalSettings);

        IParameterCollection CreateAddApplicationEnvironmentParameters(ApplicationEnvironment applicationEnvironment);

        IParameterCollection ParameterCollectionWithApplicationEnvironmentId(int applicationEnvironmentId);

        IParameterCollection ParameterCollectionWithApplicationAndEnvironmentId(int applicationId, int environmentId);

        IParameterCollection ParameterCollectionWithId(int id);

        IParameterCollection CreateUpdateApplicationEnvironmentParameters(ApplicationEnvironment applicationEnvironment);

        IParameterCollection CreateGetApplicationGroupByNameParameters(string name);

        IParameterCollection CreateUpdateApplicationParameters(Application application);

        IParameterCollection CreateGetApplicationGroupByIdParameters(int id);

        IParameterCollection CreateUpdateApplicationGroupParameters(ApplicationGroup group);

        IParameterCollection CreateAddApplicationJenkinsJobParameters(JenkinsJob applicationJenkinsJob);

        IParameterCollection ParameterCollectionWithApplicationJenkinsJobId(int jenkinsJobId);

        IParameterCollection CreateAddApplicationSvnRepositoryParameters(SvnRepository svnRepository);

        IParameterCollection CreateUpdateApplicationJenkinsJobParameters(JenkinsJob applicationJenkinsJob);

        IParameterCollection CreateGetApplicationSvnRepositoryByIdParameters(int svnRepositoryId);

        IParameterCollection CreateUpdateApplicationSvnRepositoryParameters(SvnRepository svnRepository);

        IParameterCollection CreateParameterCollectionWithId(int id);

        IParameterCollection CreateAddApplicationSonarQubeProjectParameters(SonarqubeProject project);

        IParameterCollection CreateUpdateApplicationSonarQubeProjectParameters(SonarqubeProject project);

        IParameterCollection CreateGetFilteredMenuListParameters(MenuTableParam tableParam);

        IParameterCollection CreateGetMenuListParameters();

        IParameterCollection CreateUpdateMenuParameters(int id, string name, int? parentId, string link, int? order, int? group, string description, bool isDeleted);

        IParameterCollection CreateUpdateMenuParameters(MenuModel menuModel);

        IParameterCollection CreateGetFilteredApplicationListParameters(int skip, int take, string orderBy, string orderDir, string searchText, int applicationGroupId);

        IParameterCollection CreateAddMenuParameters(MenuModel menuModel);

        IParameterCollection CreateGetFilteredUserListParameters(UserSearchFilter searchFilter);

        IParameterCollection CreateAddUserParameters(User user);

        IParameterCollection CreateGetUserParameters(string svnUserName);

        IParameterCollection CreateGetUserByEmailAddresParameters(string emailAddress);

        IParameterCollection CreateAddUserLogOnLogParameters(UserLogOnLog userLogOnLog);

        IParameterCollection CreateUpdateUserParameters(User user);

        IParameterCollection CreateUpdateOwnUserParameters(User user);

        IParameterCollection CreateAddAuditParameters(string tableName, int recordId, string fieldName, string oldValue, string newValue, int modifiedBy);

        IParameterCollection CreateGetAuditListParameters(int skip, int take, string orderBy, string orderDir, string searchText, string tableName, int recordId);

        IParameterCollection CreatePasswordResetParameters(int userId, string ipAddress, string requestCode);

        IParameterCollection CreateCheckPasswordResetParameters(string requestCode);

        IParameterCollection CreateParameterCollectionWithUserIdParameters(int userId);

        IParameterCollection CreateGetApplicationByJenkinsJobNameParameters(string name);

        IParameterCollection CreateAddApplicationNugetPackageParameters(ApplicationNugetPackage package);

        IParameterCollection CreateUpdateApplicationNugetPackageParameters(ApplicationNugetPackage nugetPackage);

        IParameterCollection CreateGetApplicationNugetPackageByNameParameters(string packageName);

        IParameterCollection CreateGetEnvironmentByNameParameters(string name);

        IParameterCollection CreateAddEnvironmentParameters(Environment environment);

        IParameterCollection CreateUpdateEnvironmentParameters(Environment environment);

        IParameterCollection CreateDeleteEnvironmentParameters(int environmentId);

        IParameterCollection ParameterCollectionWithEnvironmentId(int environmentId);

        IParameterCollection CreateGetExternalDependentByIdParameters(int id);

        IParameterCollection CreateDeleteExternalDependencyParameters(ExternalDependency externalDependency);

        IParameterCollection CreateGetDatabaseTypeByNameParameters(string name);

        IParameterCollection CreateAddDatabaseTypeParameters(DatabaseType databaseType);

        IParameterCollection CreateAddDatabaseGroupParameters(DatabaseGroup databaseGroup);

        IParameterCollection CreateUpdateDatabaseTypeParameters(DatabaseType databaseType);

        IParameterCollection CreateGetDatabaseGroupByNameParameters(string name);

        IParameterCollection CreateUpdateDatabaseGroupParameters(DatabaseGroup databaseGroup);

        IParameterCollection CreateGetFilteredDatabaseListParameters(int skip, int take, string orderBy, string orderDir, string searchText, int databaseGroupId);

        IParameterCollection ParameterCollectionWithDatabaseId(int databaseId);

        IParameterCollection CreateUpdateDatabaseParameters(Database database);

        IParameterCollection CreateGetDatabaseByDatabaseNameParameters(string name);

        IParameterCollection CreateAddDatabaseParameters(Database database);

        IParameterCollection CreateAddExternalDependencyParameters(ExternalDependency externalDependency);

        IParameterCollection CreateUpdateExternalDependencyParameters(ExternalDependency externalDependency);

        IParameterCollection ParameterCollectionWithDatabaseGroupId(int databaseGroupId);

        IParameterCollection ParameterCollectionWithDatabaseGroupIdAndName(int databaseGroupId, string databaseName);

        IParameterCollection CreateAddDatabaseDependencyParameters(DatabaseDependency databaseDependency);

        IParameterCollection CreateUpdateDatabaseDependencyParameters(DatabaseDependency databaseDependency);

        IParameterCollection CreateAddApplicationDependencyParameters(ApplicationDependency applicationDependency);

        IParameterCollection CreateUpdateApplicationDependencyParameters(ApplicationDependency applicationDependency);

        IParameterCollection CreateAddFavoritePageParameters(FavouritePage favouritePage);

        IParameterCollection CreateParameterCollectionWithUserIdAndPageUrlParameters(int userId, string pageUrl);

        IParameterCollection CreateGetFilteredApplicationRedmineProjectListParameters(int skip, int take, string orderBy, string orderDir, string searchText, int applicationGroupId);

        IParameterCollection CreateGetFilteredDatabaseRedmineProjectListParameters(int skip, int take, string orderBy, string orderDir, string searchText, int databaseGroupId);

        IParameterCollection CreateParameterCollectionWithPageIdList(List<int> pageIdList);

        IParameterCollection CreateAddNugetPackageDependencyParameters(NugetPackageDependency nugetPackageDependency);

        IParameterCollection CreateAddApplicationBuildSettingsParameters(ApplicationBuildSettings buildSettings);

        IParameterCollection CreateUpdateApplicationBuildSettingsParameters(ApplicationBuildSettings buildSettings);
    }
}