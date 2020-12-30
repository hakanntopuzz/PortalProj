using AB.Data.DapperClient.Extensions;
using AB.Data.DapperClient.Model;
using AB.Data.DapperClient.Model.Abstract;
using DevPortal.Data.Abstract.Infrastructure;
using DevPortal.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Data.Factories
{
    public class DapperParameterCollectionFactory : IParameterCollectionFactory
    {
        public IParameterCollection ParameterCollectionWithApplicationGroupId(int applicationGroupId)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.ApplicationGroupId, applicationGroupId);

            return parameters;
        }

        public IParameterCollection ParameterCollectionWithApplicationId(int applicationId)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.ApplicationId, applicationId);

            return parameters;
        }

        public IParameterCollection ParameterCollectionWithApplicationGroupIdAndName(int applicationGroupId, string applicationName)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.ApplicationGroupId, applicationGroupId);
            parameters.Add(ParameterNames.Name, applicationName);

            return parameters;
        }

        public IParameterCollection ParameterCollectionWithName(string name)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Name, name);

            return parameters;
        }

        public IParameterCollection CreateAddApplicationGroupParameters(ApplicationGroup applicationGroup)
        {
            if (applicationGroup == null)
            {
                return EmptyParameterCollection();
            }

            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.ApplicationGroupName, applicationGroup.Name);
            parameters.Add(ParameterNames.StatusId, applicationGroup.StatusId);
            parameters.Add(ParameterNames.CreatedBy, applicationGroup.RecordUpdateInfo.CreatedBy);
            parameters.Add(ParameterNames.ModifiedBy, applicationGroup.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateAddApplicationParameters(Application application)
        {
            if (application == null)
            {
                return EmptyParameterCollection();
            }

            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Name, application.Name);
            parameters.Add(ParameterNames.ApplicationGroupId, application.ApplicationGroupId);
            parameters.Add(ParameterNames.StatusId, application.StatusId);
            parameters.Add(ParameterNames.ApplicationTypeId, application.ApplicationTypeId);
            parameters.Add(ParameterNames.RedmineProjectName, application.RedmineProjectName);
            parameters.Add(ParameterNames.Description, application.Description);
            parameters.Add(ParameterNames.CreatedBy, application.RecordUpdateInfo.CreatedBy);
            parameters.Add(ParameterNames.ModifiedBy, application.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        static IParameterCollection EmptyParameterCollection()
        {
            return new DapperParameterCollection();
        }

        public IParameterCollection CreateGeneralSettingsParameters(GeneralSettings generalSettings)
        {
            if (generalSettings == null)
            {
                return EmptyParameterCollection();
            }

            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.RedmineUrl, generalSettings.RedmineUrl);
            parameters.Add(ParameterNames.SvnUrl, generalSettings.SvnUrl);
            parameters.Add(ParameterNames.JenkinsUrl, generalSettings.JenkinsUrl);
            parameters.Add(ParameterNames.SonarQubeUrl, generalSettings.SonarQubeUrl);
            parameters.Add(ParameterNames.NugetUrl, generalSettings.NugetUrl);
            parameters.Add(ParameterNames.NugetApiKey, generalSettings.NugetApiKey);
            parameters.Add(ParameterNames.NugetPackageArchiveFolderPath, generalSettings.NugetPackageArchiveFolderPath);
            parameters.Add(ParameterNames.ApplicationVersionPackageProdFolderPath, generalSettings.ApplicationVersionPackageProdFolderPath);
            parameters.Add(ParameterNames.ApplicationVersionPackagePreProdFolderPath, generalSettings.ApplicationVersionPackagePreProdFolderPath);
            parameters.Add(ParameterNames.DatabaseDeploymentPackageProdFolderPath, generalSettings.DatabaseDeploymentPackageProdFolderPath);
            parameters.Add(ParameterNames.DatabaseDeploymentPackagePreProdFolderPath, generalSettings.DatabaseDeploymentPackagePreProdFolderPath);
            parameters.Add(ParameterNames.ModifiedBy, generalSettings.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateAddApplicationEnvironmentParameters(ApplicationEnvironment applicationEnvironment)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.ApplicationId, applicationEnvironment.ApplicationId);
            parameters.Add(ParameterNames.EnvironmentId, applicationEnvironment.EnvironmentId);
            parameters.Add(ParameterNames.HasLog, applicationEnvironment.HasLog);
            parameters.Add(ParameterNames.Url, applicationEnvironment.Url);
            parameters.Add(ParameterNames.PhysicalPath, applicationEnvironment.PhysicalPath);
            parameters.Add(ParameterNames.LogFilePath, applicationEnvironment.LogFilePath);
            parameters.Add(ParameterNames.CreatedBy, applicationEnvironment.RecordUpdateInfo.CreatedBy);
            parameters.Add(ParameterNames.ModifiedBy, applicationEnvironment.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateAddApplicationJenkinsJobParameters(JenkinsJob applicationJenkinsJob)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Name, applicationJenkinsJob.JenkinsJobName);
            parameters.Add(ParameterNames.JobTypeId, applicationJenkinsJob.JenkinsJobTypeId);
            parameters.Add(ParameterNames.ApplicationId, applicationJenkinsJob.ApplicationId);
            parameters.Add(ParameterNames.CreatedBy, applicationJenkinsJob.RecordUpdateInfo.CreatedBy);
            parameters.Add(ParameterNames.ModifiedBy, applicationJenkinsJob.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateUpdateApplicationJenkinsJobParameters(JenkinsJob applicationJenkinsJob)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, applicationJenkinsJob.JenkinsJobId);
            parameters.Add(ParameterNames.Name, applicationJenkinsJob.JenkinsJobName);
            parameters.Add(ParameterNames.JobTypeId, applicationJenkinsJob.JenkinsJobTypeId);
            parameters.Add(ParameterNames.ApplicationId, applicationJenkinsJob.ApplicationId);
            parameters.Add(ParameterNames.ModifiedBy, applicationJenkinsJob.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateAddApplicationSvnRepositoryParameters(SvnRepository svnRepository)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Name, svnRepository.Name);
            parameters.Add(ParameterNames.RepositoryTypeId, svnRepository.SvnRepositoryTypeId);
            parameters.Add(ParameterNames.ApplicationId, svnRepository.ApplicationId);
            parameters.Add(ParameterNames.CreatedBy, svnRepository.RecordUpdateInfo.CreatedBy);
            parameters.Add(ParameterNames.ModifiedBy, svnRepository.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateUpdateApplicationSvnRepositoryParameters(SvnRepository svnRepository)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, svnRepository.Id);
            parameters.Add(ParameterNames.Name, svnRepository.Name);
            parameters.Add(ParameterNames.RepositoryTypeId, svnRepository.SvnRepositoryTypeId);
            parameters.Add(ParameterNames.ModifiedBy, svnRepository.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateGetApplicationSvnRepositoryByIdParameters(int svnRepositoryId)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.SvnRepositoryId, svnRepositoryId);

            return parameters;
        }

        public IParameterCollection ParameterCollectionWithApplicationEnvironmentId(int applicationEnvironmentId)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.ApplicationEnvironmentId, applicationEnvironmentId);

            return parameters;
        }

        public IParameterCollection ParameterCollectionWithApplicationAndEnvironmentId(int applicationId, int environmentId)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.ApplicationId, applicationId);
            parameters.Add(ParameterNames.EnvironmentId, environmentId);

            return parameters;
        }

        public IParameterCollection ParameterCollectionWithId(int id)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, id);

            return parameters;
        }

        public IParameterCollection ParameterCollectionWithApplicationJenkinsJobId(int jenkinsJobId)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.ApplicationJenkinsJobId, jenkinsJobId);

            return parameters;
        }

        public IParameterCollection CreateUpdateApplicationEnvironmentParameters(ApplicationEnvironment applicationEnvironment)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, applicationEnvironment.Id);
            parameters.Add(ParameterNames.ApplicationId, applicationEnvironment.ApplicationId);
            parameters.Add(ParameterNames.EnvironmentId, applicationEnvironment.EnvironmentId);
            parameters.Add(ParameterNames.HasLog, applicationEnvironment.HasLog);
            parameters.Add(ParameterNames.Url, applicationEnvironment.Url);
            parameters.Add(ParameterNames.PhysicalPath, applicationEnvironment.PhysicalPath);
            parameters.Add(ParameterNames.LogFilePath, applicationEnvironment.LogFilePath);
            parameters.Add(ParameterNames.ModifiedBy, applicationEnvironment.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateGetApplicationGroupByNameParameters(string name)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.ApplicationGroupName, name);

            return parameters;
        }

        public IParameterCollection CreateUpdateApplicationParameters(Application application)
        {
            if (application == null)
            {
                return EmptyParameterCollection();
            }

            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, application.Id);
            parameters.Add(ParameterNames.Name, application.Name);
            parameters.Add(ParameterNames.ApplicationGroupId, application.ApplicationGroupId);
            parameters.Add(ParameterNames.StatusId, application.StatusId);
            parameters.Add(ParameterNames.ApplicationTypeId, application.ApplicationTypeId);
            parameters.Add(ParameterNames.RedmineProjectName, application.RedmineProjectName);
            parameters.Add(ParameterNames.Description, application.Description);
            parameters.Add(ParameterNames.ModifiedBy, application.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateGetApplicationGroupByIdParameters(int id)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.ApplicationGroupId, id);

            return parameters;
        }

        public IParameterCollection CreateUpdateApplicationGroupParameters(ApplicationGroup group)
        {
            if (group == null)
            {
                return EmptyParameterCollection();
            }

            var parameters = new DapperParameterCollection();
            parameters.Add(ParameterNames.Id, group.Id);
            parameters.Add(ParameterNames.Name, group.Name);
            parameters.Add(ParameterNames.StatusId, group.StatusId);
            parameters.Add(ParameterNames.ModifiedBy, group.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateParameterCollectionWithId(int id)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, id);

            return parameters;
        }

        public IParameterCollection CreateAddApplicationSonarQubeProjectParameters(SonarqubeProject project)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Name, project.SonarqubeProjectName);
            parameters.Add(ParameterNames.ProjectTypeId, project.SonarqubeProjectTypeId);
            parameters.Add(ParameterNames.ApplicationId, project.ApplicationId);
            parameters.Add(ParameterNames.CreatedBy, project.RecordUpdateInfo.CreatedBy);
            parameters.Add(ParameterNames.ModifiedBy, project.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateUpdateApplicationSonarQubeProjectParameters(SonarqubeProject project)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, project.SonarqubeProjectId);
            parameters.Add(ParameterNames.Name, project.SonarqubeProjectName);
            parameters.Add(ParameterNames.ProjectTypeId, project.SonarqubeProjectTypeId);
            parameters.Add(ParameterNames.ApplicationId, project.ApplicationId);
            parameters.Add(ParameterNames.ModifiedBy, project.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateGetFilteredMenuListParameters(MenuTableParam tableParam)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.DisplayLength, tableParam.length);
            parameters.Add(ParameterNames.DisplayStart, tableParam.start);
            parameters.Add(ParameterNames.SortCol, tableParam.SortColumn);
            parameters.Add(ParameterNames.SortDir, tableParam.order.FirstOrDefault()?.dir);
            parameters.Add(ParameterNames.SearchValue, tableParam.SearchText);

            return parameters;
        }

        public IParameterCollection CreateGetMenuListParameters()
        {
            return new DapperParameterCollection();
        }

        public IParameterCollection CreateAddMenuParameters(MenuModel menuModel)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Name, menuModel.Name);
            parameters.Add(ParameterNames.ParentId, menuModel.ParentId);
            parameters.Add(ParameterNames.Link, menuModel.Link);
            parameters.Add(ParameterNames.Order, menuModel.Order);
            parameters.Add(ParameterNames.Group, menuModel.MenuGroupId);
            parameters.Add(ParameterNames.Icon, menuModel.Icon);
            parameters.Add(ParameterNames.Description, menuModel.Description);
            parameters.Add(ParameterNames.CreatedBy, menuModel.RecordUpdateInfo.CreatedBy);
            parameters.Add(ParameterNames.ModifiedBy, menuModel.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateUpdateMenuParameters(int id, string name, int? parentId, string link, int? order, int? group, string description, bool isDeleted)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, id);
            parameters.Add(ParameterNames.Name, name);
            parameters.Add(ParameterNames.ParentId, parentId);
            parameters.Add(ParameterNames.Link, link);
            parameters.Add(ParameterNames.Order, order);
            parameters.Add(ParameterNames.Group, group);
            parameters.Add(ParameterNames.Description, description);
            parameters.Add(ParameterNames.IsDeleted, isDeleted);

            return parameters;
        }

        public IParameterCollection CreateUpdateMenuParameters(MenuModel menuModel)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, menuModel.Id);
            parameters.Add(ParameterNames.Name, menuModel.Name);
            parameters.Add(ParameterNames.ParentId, menuModel.ParentId);
            parameters.Add(ParameterNames.Link, menuModel.Link);
            parameters.Add(ParameterNames.Order, menuModel.Order);
            parameters.Add(ParameterNames.Group, menuModel.MenuGroupId);
            parameters.Add(ParameterNames.Icon, menuModel.Icon);
            parameters.Add(ParameterNames.Description, menuModel.Description);
            parameters.Add(ParameterNames.ModifiedBy, menuModel.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateGetFilteredApplicationListParameters(int skip, int take, string orderBy, string orderDir, string searchText, int applicationGroupId)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.DisplayStart, skip);
            parameters.Add(ParameterNames.DisplayLength, take);
            parameters.Add(ParameterNames.SortCol, orderBy);
            parameters.Add(ParameterNames.SortDir, orderDir);
            parameters.Add(ParameterNames.SearchValue, searchText);
            parameters.Add(ParameterNames.ApplicationGroupId, applicationGroupId);

            return parameters;
        }

        public IParameterCollection CreateGetFilteredUserListParameters(UserSearchFilter searchFilter)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.DisplayLength, searchFilter.length);
            parameters.Add(ParameterNames.DisplayStart, searchFilter.start);
            parameters.Add(ParameterNames.SortCol, searchFilter.SortColumn);
            parameters.Add(ParameterNames.SortDir, searchFilter.order.FirstOrDefault()?.dir);
            parameters.Add(ParameterNames.SearchValue, searchFilter.SearchText);

            return parameters;
        }

        public IParameterCollection CreateAddUserParameters(User user)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.SecureId, user.SecureId);
            parameters.Add(ParameterNames.FirstName, user.FirstName);
            parameters.Add(ParameterNames.LastName, user.LastName);
            parameters.Add(ParameterNames.SvnUserName, user.SvnUserName);
            parameters.Add(ParameterNames.EmailAddress, user.EmailAddress);
            parameters.Add(ParameterNames.PasswordHash, user.PasswordHash);
            parameters.Add(ParameterNames.UserStatusId, user.UserStatusId);
            parameters.Add(ParameterNames.UserTypeId, user.UserTypeId);
            parameters.Add(ParameterNames.CreatedBy, user.RecordUpdateInfo.CreatedBy);

            return parameters;
        }

        public IParameterCollection CreateAddUserLogOnLogParameters(UserLogOnLog userLogOnLog)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.UserId, userLogOnLog.UserId);
            parameters.Add(ParameterNames.IpAddress, userLogOnLog.IpAddress);
            parameters.Add(ParameterNames.EmailAddress, userLogOnLog.EmailAddress);
            parameters.Add(ParameterNames.BrowserInfo, userLogOnLog.BrowserInfo);
            parameters.Add(ParameterNames.BrowserName, userLogOnLog.BrowserName);
            parameters.Add(ParameterNames.BrowserVersion, userLogOnLog.BrowserVersion);

            return parameters;
        }

        public IParameterCollection CreateGetUserParameters(string svnUserName)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.SvnUserName, svnUserName);

            return parameters;
        }

        public IParameterCollection CreateGetUserByEmailAddresParameters(string emailAddress)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.EmailAddress, emailAddress);

            return parameters;
        }

        public IParameterCollection CreateUpdateUserParameters(User user)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, user.Id);
            parameters.Add(ParameterNames.FirstName, user.FirstName);
            parameters.Add(ParameterNames.LastName, user.LastName);
            parameters.Add(ParameterNames.SvnUserName, user.SvnUserName);
            parameters.Add(ParameterNames.EmailAddress, user.EmailAddress);
            parameters.Add(ParameterNames.UserStatusId, user.UserStatusId);
            parameters.Add(ParameterNames.UserTypeId, user.UserTypeId);
            parameters.Add(ParameterNames.ModifiedBy, user.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateUpdateOwnUserParameters(User user)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.FirstName, user.FirstName);
            parameters.Add(ParameterNames.LastName, user.LastName);
            parameters.Add(ParameterNames.SvnUserName, user.SvnUserName);
            parameters.Add(ParameterNames.UserId, user.Id);
            parameters.Add(ParameterNames.PasswordHash, user.PasswordHash);

            return parameters;
        }

        public IParameterCollection CreateAddAuditParameters(string tableName, int recordId, string fieldName, string oldValue, string newValue, int modifiedBy)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.TableName, tableName);
            parameters.Add(ParameterNames.RecordId, recordId);
            parameters.Add(ParameterNames.FieldName, fieldName);
            parameters.Add(ParameterNames.OldValue, oldValue);
            parameters.Add(ParameterNames.NewValue, newValue);
            parameters.Add(ParameterNames.ModifiedBy, modifiedBy);

            return parameters;
        }

        public IParameterCollection CreateGetAuditListParameters(int skip, int take, string orderBy, string orderDir, string searchText, string tableName, int recordId)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.DisplayLength, take);
            parameters.Add(ParameterNames.DisplayStart, skip);
            parameters.Add(ParameterNames.SortCol, orderBy);
            parameters.Add(ParameterNames.SortDir, orderDir);
            parameters.Add(ParameterNames.SearchValue, searchText);
            parameters.Add(ParameterNames.TableName, tableName);
            parameters.Add(ParameterNames.RecordId, recordId);

            return parameters;
        }

        public IParameterCollection CreatePasswordResetParameters(int userId, string ipAddress, string requestCode)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.UserId, userId);
            parameters.Add(ParameterNames.IpAddress, ipAddress);
            parameters.Add(ParameterNames.RequestCode, requestCode);

            return parameters;
        }

        public IParameterCollection CreateCheckPasswordResetParameters(string requestCode)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.RequestCode, requestCode);

            return parameters;
        }

        public IParameterCollection CreateParameterCollectionWithUserIdParameters(int userId)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.UserId, userId);

            return parameters;
        }

        public IParameterCollection CreateGetApplicationByJenkinsJobNameParameters(string name)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.JenkinsJobName, name);

            return parameters;
        }

        public IParameterCollection CreateAddApplicationNugetPackageParameters(ApplicationNugetPackage package)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Name, package.NugetPackageName);
            parameters.Add(ParameterNames.ApplicationId, package.ApplicationId);
            parameters.Add(ParameterNames.CreatedBy, package.RecordUpdateInfo.CreatedBy);
            parameters.Add(ParameterNames.ModifiedBy, package.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateUpdateApplicationNugetPackageParameters(ApplicationNugetPackage nugetPackage)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, nugetPackage.NugetPackageId);
            parameters.Add(ParameterNames.Name, nugetPackage.NugetPackageName);
            parameters.Add(ParameterNames.ModifiedBy, nugetPackage.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateGetApplicationNugetPackageByNameParameters(string packageName)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.NugetPackageName, packageName);

            return parameters;
        }

        public IParameterCollection CreateGetEnvironmentByNameParameters(string name)
        {
            return ParameterCollectionWithName(name);
        }

        public IParameterCollection CreateAddEnvironmentParameters(Environment environment)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Name, environment.Name);
            parameters.Add(ParameterNames.CreatedBy, environment.RecordUpdateInfo.CreatedBy);
            parameters.Add(ParameterNames.ModifiedBy, environment.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateUpdateEnvironmentParameters(Environment environment)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, environment.Id);
            parameters.Add(ParameterNames.Name, environment.Name);
            parameters.Add(ParameterNames.ModifiedBy, environment.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateDeleteEnvironmentParameters(int environmentId)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.EnvironmentId, environmentId);

            return parameters;
        }

        public IParameterCollection ParameterCollectionWithEnvironmentId(int environmentId)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.EnvironmentId, environmentId);

            return parameters;
        }

        public IParameterCollection CreateGetExternalDependentByIdParameters(int id)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.ExternalDependencyId, id);

            return parameters;
        }

        public IParameterCollection CreateDeleteExternalDependencyParameters(ExternalDependency externalDependency)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.ExternalDependencyId, externalDependency.Id);
            parameters.Add(ParameterNames.ModifiedBy, externalDependency.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateGetFilteredDatabaseListParameters(int skip, int take, string orderBy, string orderDir, string searchText, int databaseGroupId)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.DisplayStart, skip);
            parameters.Add(ParameterNames.DisplayLength, take);
            parameters.Add(ParameterNames.SortCol, orderBy);
            parameters.Add(ParameterNames.SortDir, orderDir);
            parameters.Add(ParameterNames.SearchValue, searchText);
            parameters.Add(ParameterNames.DatabaseGroupId, databaseGroupId);

            return parameters;
        }

        public IParameterCollection ParameterCollectionWithDatabaseId(int databaseId)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.DatabaseId, databaseId);

            return parameters;
        }

        public IParameterCollection CreateGetDatabaseTypeByNameParameters(string name)
        {
            return ParameterCollectionWithName(name);
        }

        public IParameterCollection CreateAddDatabaseTypeParameters(DatabaseType databaseType)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Name, databaseType.Name);
            parameters.Add(ParameterNames.CreatedBy, databaseType.RecordUpdateInfo.CreatedBy);
            parameters.Add(ParameterNames.ModifiedBy, databaseType.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateUpdateDatabaseTypeParameters(DatabaseType databaseType)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, databaseType.Id);
            parameters.Add(ParameterNames.Name, databaseType.Name);
            parameters.Add(ParameterNames.ModifiedBy, databaseType.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateGetDatabaseGroupByNameParameters(string name)
        {
            return ParameterCollectionWithName(name);
        }

        public IParameterCollection CreateAddDatabaseGroupParameters(DatabaseGroup databaseGroup)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Name, databaseGroup.Name);
            parameters.Add(ParameterNames.CreatedBy, databaseGroup.RecordUpdateInfo.CreatedBy);
            parameters.Add(ParameterNames.ModifiedBy, databaseGroup.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateUpdateDatabaseGroupParameters(DatabaseGroup databaseGroup)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, databaseGroup.Id);
            parameters.Add(ParameterNames.Name, databaseGroup.Name);
            parameters.Add(ParameterNames.ModifiedBy, databaseGroup.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateAddExternalDependencyParameters(ExternalDependency externalDependency)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Name, externalDependency.Name);
            parameters.Add(ParameterNames.Description, externalDependency.Description);
            parameters.Add(ParameterNames.ApplicationId, externalDependency.ApplicationId);
            parameters.Add(ParameterNames.CreatedBy, externalDependency.RecordUpdateInfo.CreatedBy);

            return parameters;
        }

        public IParameterCollection CreateUpdateExternalDependencyParameters(ExternalDependency externalDependency)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, externalDependency.Id);
            parameters.Add(ParameterNames.Name, externalDependency.Name);
            parameters.Add(ParameterNames.Description, externalDependency.Description);
            parameters.Add(ParameterNames.ModifiedBy, externalDependency.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateUpdateDatabaseParameters(Database database)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, database.Id);
            parameters.Add(ParameterNames.Name, database.Name);
            parameters.Add(ParameterNames.DatabaseGroupId, database.DatabaseGroupId);
            parameters.Add(ParameterNames.DatabaseTypeId, database.DatabaseTypeId);
            parameters.Add(ParameterNames.RedmineProjectName, database.RedmineProjectName);
            parameters.Add(ParameterNames.Description, database.Description);
            parameters.Add(ParameterNames.ModifiedBy, database.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateGetDatabaseByDatabaseNameParameters(string name)
        {
            return ParameterCollectionWithName(name);
        }

        public IParameterCollection CreateAddDatabaseParameters(Database database)
        {
            if (database == null)
            {
                return EmptyParameterCollection();
            }

            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Name, database.Name);
            parameters.Add(ParameterNames.Description, database.Description);
            parameters.Add(ParameterNames.DatabaseGroupId, database.DatabaseGroupId);
            parameters.Add(ParameterNames.DatabaseTypeId, database.DatabaseTypeId);
            parameters.Add(ParameterNames.RedmineProjectName, database.RedmineProjectName);
            parameters.Add(ParameterNames.CreatedBy, database.RecordUpdateInfo.CreatedBy);
            parameters.Add(ParameterNames.ModifiedBy, database.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection ParameterCollectionWithDatabaseGroupId(int databaseGroupId)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.DatabaseGroupId, databaseGroupId);

            return parameters;
        }

        public IParameterCollection ParameterCollectionWithDatabaseGroupIdAndName(int databaseGroupId, string databaseName)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.DatabaseGroupId, databaseGroupId);
            parameters.Add(ParameterNames.Name, databaseName);

            return parameters;
        }

        public IParameterCollection CreateAddDatabaseDependencyParameters(DatabaseDependency databaseDependency)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.DependentApplicationId, databaseDependency.ApplicationId);
            parameters.Add(ParameterNames.DatabaseId, databaseDependency.DatabaseId);
            parameters.Add(ParameterNames.Description, databaseDependency.Description);
            parameters.Add(ParameterNames.CreatedBy, databaseDependency.RecordUpdateInfo.CreatedBy);
            parameters.Add(ParameterNames.ModifiedBy, databaseDependency.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateUpdateDatabaseDependencyParameters(DatabaseDependency databaseDependency)
        {
            if (databaseDependency == null)
            {
                return EmptyParameterCollection();
            }

            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, databaseDependency.Id);
            parameters.Add(ParameterNames.Description, databaseDependency.Description);
            parameters.Add(ParameterNames.ModifiedBy, databaseDependency.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateAddApplicationDependencyParameters(ApplicationDependency applicationDependency)
        {
            if (applicationDependency == null)
            {
                return EmptyParameterCollection();
            }

            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.DependentApplicationId, applicationDependency.DependentApplicationId);
            parameters.Add(ParameterNames.DependedApplicationId, applicationDependency.DependedApplicationId);
            parameters.Add(ParameterNames.Description, applicationDependency.Description);
            parameters.Add(ParameterNames.CreatedBy, applicationDependency.RecordUpdateInfo.CreatedBy);
            parameters.Add(ParameterNames.ModifiedBy, applicationDependency.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateUpdateApplicationDependencyParameters(ApplicationDependency applicationDependency)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, applicationDependency.Id);
            parameters.Add(ParameterNames.Description, applicationDependency.Description);
            parameters.Add(ParameterNames.ModifiedBy, applicationDependency.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateAddFavoritePageParameters(FavouritePage favouritePage)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.UserId, favouritePage.UserId);
            parameters.Add(ParameterNames.PageName, favouritePage.PageName);
            parameters.Add(ParameterNames.PageUrl, favouritePage.PageUrl);
            parameters.Add(ParameterNames.Order, favouritePage.Order);

            return parameters;
        }

        public IParameterCollection CreateParameterCollectionWithUserIdAndPageUrlParameters(int userId, string pageUrl)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.UserId, userId);
            parameters.Add(ParameterNames.PageUrl, pageUrl);

            return parameters;
        }

        public IParameterCollection CreateGetFilteredApplicationRedmineProjectListParameters(int skip, int take, string orderBy, string orderDir, string searchText, int applicationGroupId)
        {
            return CreateGetFilteredApplicationListParameters(skip, take, orderBy, orderDir, searchText, applicationGroupId);
        }

        public IParameterCollection CreateGetFilteredDatabaseRedmineProjectListParameters(int skip, int take, string orderBy, string orderDir, string searchText, int databaseGroupId)
        {
            return CreateGetFilteredDatabaseListParameters(skip, take, orderBy, orderDir, searchText, databaseGroupId);
        }

        public IParameterCollection CreateParameterCollectionWithPageIdList(List<int> pageIdList)
        {
            var parameters = new DapperParameterCollection();
            parameters.Add(ParameterNames.PageIdList, pageIdList.AsTableValuedParameter(SqlTableTypeNames.IdCollection));

            return parameters;
        }

        public IParameterCollection CreateAddNugetPackageDependencyParameters(NugetPackageDependency nugetPackageDependency)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.NugetPackageName, nugetPackageDependency.NugetPackageName);
            parameters.Add(ParameterNames.DependentApplicationId, nugetPackageDependency.DependentApplicationId);
            parameters.Add(ParameterNames.CreatedBy, nugetPackageDependency.RecordUpdateInfo.CreatedBy);
            parameters.Add(ParameterNames.ModifiedBy, nugetPackageDependency.RecordUpdateInfo.ModifiedBy);

            return parameters;
        }

        public IParameterCollection CreateAddApplicationBuildSettingsParameters(ApplicationBuildSettings buildSettings)
        {
            var parameters = new DapperParameterCollection();
            CreateBuildSettingsParametersCore(buildSettings, parameters);
            parameters.Add(ParameterNames.CreatedBy, buildSettings.RecordUpdateInfo.CreatedBy);

            return parameters;
        }

        public IParameterCollection CreateUpdateApplicationBuildSettingsParameters(ApplicationBuildSettings buildSettings)
        {
            var parameters = new DapperParameterCollection();

            parameters.Add(ParameterNames.Id, buildSettings.Id);
            CreateBuildSettingsParametersCore(buildSettings, parameters);

            return parameters;
        }

        static void CreateBuildSettingsParametersCore(ApplicationBuildSettings buildSettings, DapperParameterCollection parameters)
        {
            parameters.Add(ParameterNames.ApplicationId, buildSettings.ApplicationId);
            parameters.Add(ParameterNames.Workspace, buildSettings.Workspace);
            parameters.Add(ParameterNames.SolutionName, buildSettings.SolutionName);
            parameters.Add(ParameterNames.ProjectName, buildSettings.ProjectName);
            parameters.Add(ParameterNames.DeployPath, buildSettings.DeployPath);
            parameters.Add(ParameterNames.DevPublishProfileName, buildSettings.DevPublishProfileName);
            parameters.Add(ParameterNames.TestPublishProfileName, buildSettings.TestPublishProfileName);
            parameters.Add(ParameterNames.PreProdPublishProfileName, buildSettings.PreProdPublishProfileName);
            parameters.Add(ParameterNames.ProdPublishProfileName, buildSettings.ProdPublishProfileName);
            parameters.Add(ParameterNames.DevRemoteAddress, buildSettings.DevRemoteAddress);
            parameters.Add(ParameterNames.TestRemoteAddress, buildSettings.TestRemoteAddress);
            parameters.Add(ParameterNames.PreProdRemoteAddress, buildSettings.PreProdRemoteAddress);
            parameters.Add(ParameterNames.ProdRemoteAddress, buildSettings.ProdRemoteAddress);
            parameters.Add(ParameterNames.ModifiedBy, buildSettings.RecordUpdateInfo.ModifiedBy);
        }
    }
}