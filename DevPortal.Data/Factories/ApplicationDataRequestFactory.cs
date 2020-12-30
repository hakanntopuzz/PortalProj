using AB.Data.DapperClient.Model;
using AB.Data.DapperClient.Model.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Abstract.Infrastructure;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Factories
{
    public class ApplicationDataRequestFactory : IApplicationDataRequestFactory
    {
        #region ctor

        readonly IParameterCollectionFactory parameterCollectionFactory;

        public ApplicationDataRequestFactory(IParameterCollectionFactory parameterCollectionFactory)
        {
            this.parameterCollectionFactory = parameterCollectionFactory;
        }

        #endregion

        #region get split on parameters

        static string GetSplitOnParameters(params string[] fieldNames)
        {
            return string.Join(",", fieldNames);
        }

        #endregion

        public IDataRequest GetApplicationEnvironments(int applicationId)
        {
            var procedureName = ProcedureNames.GetApplicationEnvironments;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationGroups()
        {
            var procedureName = ProcedureNames.GetApplicationGroups;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetApplicationsWithLogByApplicationGroup(int applicationGroupId)
        {
            var procedureName = ProcedureNames.GetApplicationsWithLogByApplicationGroup;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationGroupId(applicationGroupId);
            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationTypes()
        {
            var procedureName = ProcedureNames.GetApplicationTypes;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetApplicationsByApplicationGroupId(int applicationGroupId)
        {
            var procedureName = ProcedureNames.GetApplicationsByApplicationGroupId;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationGroupId(applicationGroupId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddApplication(Application application)
        {
            var procedureName = ProcedureNames.AddApplication;
            var parameters = parameterCollectionFactory.CreateAddApplicationParameters(application);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplications()
        {
            var procedureName = ProcedureNames.GetApplications;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetApplicationsByGroupIdAndName(int applicationGroupId, string applicationName)
        {
            var procedureName = ProcedureNames.GetApplicationsByGroupIdAndName;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationGroupIdAndName(applicationGroupId, applicationName);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationsByGroupId(int applicationGroupId)
        {
            var procedureName = ProcedureNames.GetApplicationsByGroupId;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationGroupId(applicationGroupId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationsByApplicationName(string applicationName)
        {
            var procedureName = ProcedureNames.GetApplicationsByApplicationName;
            var parameters = parameterCollectionFactory.ParameterCollectionWithName(applicationName);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationGroupList()
        {
            var procedureName = ProcedureNames.GetApplicationGroupList;

            string splitOnParameters = GetSplitOnParameters(ColumnNames.ModifiedDate);

            return DapperDataRequest.Create(procedureName, splitOnParameters: splitOnParameters);
        }

        public IDataRequest AddApplicationGroup(ApplicationGroup applicationGroup)
        {
            var procedureName = ProcedureNames.AddApplicationGroup;
            var parameters = parameterCollectionFactory.CreateAddApplicationGroupParameters(applicationGroup);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationGroupByName(string name)
        {
            var procedureName = ProcedureNames.GetApplicationGroupByName;
            var parameters = parameterCollectionFactory.CreateGetApplicationGroupByNameParameters(name);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplication(int applicationId)
        {
            var procedureName = ProcedureNames.GetApplication;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetSvnRepositories(int applicationId)
        {
            var procedureName = ProcedureNames.GetSvnRepositories;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetJenkinsJobs(int applicationId)
        {
            var procedureName = ProcedureNames.GetJenkinsJobs;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationJenkinsJobById(int jenkinsJobId)
        {
            var procedureName = ProcedureNames.GetApplicationJenkinsJobById;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationJenkinsJobId(jenkinsJobId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationJenkinsJobUpdateInfo(int jenkinsJobId)
        {
            var procedureName = ProcedureNames.GetApplicationJenkinsJobUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(jenkinsJobId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetJenkinsJobTypes()
        {
            var procedureName = ProcedureNames.GetJenkinsJobTypes;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest UpdateApplicationJenkinsJob(JenkinsJob applicationJenkinsJob)
        {
            var procedureName = ProcedureNames.UpdateApplicationJenkinsJob;
            var parameters = parameterCollectionFactory.CreateUpdateApplicationJenkinsJobParameters(applicationJenkinsJob);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetGeneralSettings()
        {
            var procedureName = ProcedureNames.GetGeneralSettings;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest UpdateGeneralSettings(GeneralSettings generalSettings)
        {
            var procedureName = ProcedureNames.UpdateGeneralSettings;
            var parameters = parameterCollectionFactory.CreateGeneralSettingsParameters(generalSettings);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddApplicationEnvironment(ApplicationEnvironment applicationEnvironment)
        {
            var procedureName = ProcedureNames.AddApplicationEnvironment;
            var parameters = parameterCollectionFactory.CreateAddApplicationEnvironmentParameters(applicationEnvironment);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddApplicationJenkinsJob(JenkinsJob applicationJenkinsJob)
        {
            var procedureName = ProcedureNames.AddApplicationJenkinsJob;
            var parameters = parameterCollectionFactory.CreateAddApplicationJenkinsJobParameters(applicationJenkinsJob);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest DeleteApplicationJenkinsJob(int jenkinsJobId)
        {
            var procedureName = ProcedureNames.DeleteApplicationJenkinsJob;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationJenkinsJobId(jenkinsJobId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddApplicationSvnRepository(SvnRepository svnRepository)
        {
            var procedureName = ProcedureNames.AddApplicationSvnRepository;
            var parameters = parameterCollectionFactory.CreateAddApplicationSvnRepositoryParameters(svnRepository);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateApplicationSvnRepository(SvnRepository svnRepository)
        {
            var procedureName = ProcedureNames.UpdateApplicationSvnRepository;
            var parameters = parameterCollectionFactory.CreateUpdateApplicationSvnRepositoryParameters(svnRepository);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest DeleteApplicationSvnRepository(int svnRepositoryId)
        {
            var procedureName = ProcedureNames.DeleteApplicationSvnRepository;
            var parameters = parameterCollectionFactory.CreateGetApplicationSvnRepositoryByIdParameters(svnRepositoryId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationSvnRepositoryById(int svnRepositoryId)
        {
            var procedureName = ProcedureNames.GetApplicationSvnRepositoryById;
            var parameters = parameterCollectionFactory.CreateGetApplicationSvnRepositoryByIdParameters(svnRepositoryId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationSvnRepositoryUpdateInfo(int repositoryId)
        {
            var procedureName = ProcedureNames.GetApplicationSvnRepositoryUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(repositoryId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetSvnRepositoryTypes()
        {
            var procedureName = ProcedureNames.GetSvnRepositoryTypes;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetEnvironmentsDoesNotExistByApplicationId(int applicationId)
        {
            var procedureName = ProcedureNames.GetEnvironmentsDoesNotExistByApplicationId;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationEnvironmentById(int applicationEnvironmentId)
        {
            var procedureName = ProcedureNames.GetApplicationEnvironmentById;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationEnvironmentId(applicationEnvironmentId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationEnvironmentByEnvironmentId(int applicationId, int environmentId)
        {
            var procedureName = ProcedureNames.GetApplicationEnvironmentByEnvironmentId;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationAndEnvironmentId(applicationId, environmentId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationEnvironmentUpdateInfo(int environmentId)
        {
            var procedureName = ProcedureNames.GetApplicationEnvironmentUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(environmentId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateApplicationEnvironment(ApplicationEnvironment applicationEnvironment)
        {
            var procedureName = ProcedureNames.UpdateApplicationEnvironment;
            var parameters = parameterCollectionFactory.CreateUpdateApplicationEnvironmentParameters(applicationEnvironment);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetEnvironments()
        {
            var procedureName = ProcedureNames.GetEnvironments;

            string splitOnParameters = GetSplitOnParameters(ColumnNames.ModifiedDate);

            return DapperDataRequest.Create(procedureName, splitOnParameters: splitOnParameters);
        }

        public IDataRequest DeleteApplicationEnvironment(int environmentId)
        {
            var procedureName = ProcedureNames.DeleteApplicationEnvironment;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationEnvironmentId(environmentId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateApplication(Application application)
        {
            var procedureName = ProcedureNames.UpdateApplication;
            var parameters = parameterCollectionFactory.CreateUpdateApplicationParameters(application);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationGroupById(int id)
        {
            var procedureName = ProcedureNames.GetApplicationGroupById;
            var parameters = parameterCollectionFactory.CreateGetApplicationGroupByIdParameters(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateApplicationGroup(ApplicationGroup group)
        {
            var procedureName = ProcedureNames.UpdateApplicationGroup;
            var parameters = parameterCollectionFactory.CreateUpdateApplicationGroupParameters(group);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationGroupStatusList()
        {
            var procedureName = ProcedureNames.GetApplicationGroupStatusList;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetApplicationStatusList()
        {
            var procedureName = ProcedureNames.GetApplicationStatusList;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest DeleteApplicationGroup(int groupId)
        {
            var procedureName = ProcedureNames.DeleteApplicationGroup;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationGroupId(groupId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest DeleteApplication(int applicationId)
        {
            var procedureName = ProcedureNames.DeleteApplication;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationByApplicationName(string name)
        {
            var procedureName = ProcedureNames.GetApplicationByApplicationName;
            var parameters = parameterCollectionFactory.ParameterCollectionWithName(name);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationCount()
        {
            var procedureName = ProcedureNames.GetApplicationCount;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetApplicationGroupCount()
        {
            var procedureName = ProcedureNames.GetApplicationGroupCount;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetApplicationCountByType()
        {
            var procedureName = ProcedureNames.GetApplicationCountByType;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetJenkinsJobCountByType()
        {
            var procedureName = ProcedureNames.GetJenkinsJobCountByType;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetJenkinsJobCount()
        {
            var procedureName = ProcedureNames.GetJenkinsJobCount;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetSvnRepositoryCount()
        {
            var procedureName = ProcedureNames.GetSvnRepositoryCount;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetSonarQubeProjectCountByType()
        {
            var procedureName = ProcedureNames.GetSonarQubeProjectCountByType;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetSonarQubeProjectCount()
        {
            var procedureName = ProcedureNames.GetSonarQubeProjectCount;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetApplicationRedmineProjectCount()
        {
            var procedureName = ProcedureNames.GetApplicationRedmineProjectCount;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetDatabaseRedmineProjectCount()
        {
            var procedureName = ProcedureNames.GetDatabaseRedmineProjectCount;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetSonarqubeProjects(int applicationId)
        {
            var procedureName = ProcedureNames.GetSonarqubeProjects;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetNugetPackageCount()
        {
            var procedureName = ProcedureNames.GetNugetPackageCount;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetLastUpdatedApplications()
        {
            var procedureName = ProcedureNames.GetLastUpdatedApplications;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetApplicationSonarQubeProjectById(int projectId)
        {
            var procedureName = ProcedureNames.GetSonarQubeProjectById;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(projectId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationSonarQubeProjectUpdateInfo(int projectId)
        {
            var procedureName = ProcedureNames.GetApplicationSonarQubeProjectUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(projectId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetSonarQubeProjectTypes()
        {
            var procedureName = ProcedureNames.GetSonarQubeProjectTypes;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest AddApplicationSonarQubeProject(SonarqubeProject project)
        {
            var procedureName = ProcedureNames.AddApplicationSonarQubeProject;
            var parameters = parameterCollectionFactory.CreateAddApplicationSonarQubeProjectParameters(project);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest DeleteApplicationSonarQubeProject(int projectId)
        {
            var procedureName = ProcedureNames.DeleteApplicationSonarQubeProject;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(projectId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateApplicationSonarQubeProject(SonarqubeProject project)
        {
            var procedureName = ProcedureNames.UpdateApplicationSonarQubeProject;
            var parameters = parameterCollectionFactory.CreateUpdateApplicationSonarQubeProjectParameters(project);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetFilteredMenuList(MenuTableParam tableParam)
        {
            var procedureName = ProcedureNames.GetFilteredMenuList;

            string splitOnParameters = GetSplitOnParameters(ColumnNames.ModifiedDate);

            var parameters = parameterCollectionFactory.CreateGetFilteredMenuListParameters(tableParam);

            return DapperDataRequest.Create(procedureName, parameters, splitOnParameters);
        }

        public IDataRequest GetMenuList()
        {
            var procedureName = ProcedureNames.GetMenuList;
            var parameters = parameterCollectionFactory.CreateGetMenuListParameters();

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddMenu(MenuModel menuModel)
        {
            var procedureName = ProcedureNames.AddMenu;
            var parameters = parameterCollectionFactory.CreateAddMenuParameters(menuModel);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetMenu(int id)
        {
            var procedureName = ProcedureNames.GetMenu;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetSubMenuList(int id)
        {
            var procedureName = ProcedureNames.GetSubMenuList;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateMenu(MenuModel menuModel)
        {
            var procedureName = ProcedureNames.UpdateMenu;
            var parameters = parameterCollectionFactory.CreateUpdateMenuParameters(menuModel);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest DeleteMenu(int menuId)
        {
            var procedureName = ProcedureNames.DeleteMenu;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(menuId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetFilteredApplicationList(int skip, int take, string orderBy, string orderDir, string searchText, int applicationGroupId)
        {
            var procedureName = ProcedureNames.GetFilteredApplicationList;

            string splitOnParameters = GetSplitOnParameters(ColumnNames.ModifiedDate);

            var parameters = parameterCollectionFactory.CreateGetFilteredApplicationListParameters(skip, take, orderBy, orderDir, searchText, applicationGroupId);

            return DapperDataRequest.Create(procedureName, parameters, splitOnParameters);
        }

        public IDataRequest GetFavouriteRedmineProjects()
        {
            var procedureName = ProcedureNames.GetFavouriteRedmineProjects;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetFavouriteRedmineWikiPages()
        {
            var procedureName = ProcedureNames.GetFavouriteRedmineWikiPages;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetToolPages()
        {
            var procedureName = ProcedureNames.GetToolPages;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetUser(int id)
        {
            var procedureName = ProcedureNames.GetUser;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetFilteredUserList(UserSearchFilter searchFilter)
        {
            var procedureName = ProcedureNames.GetFilteredUserList;
            var parameters = parameterCollectionFactory.CreateGetFilteredUserListParameters(searchFilter);

            string splitOnParameters = GetSplitOnParameters(ColumnNames.ModifiedDate);

            return DapperDataRequest.Create(procedureName, parameters, splitOnParameters: splitOnParameters);
        }

        public IDataRequest GetUserStatusList()
        {
            var procedureName = ProcedureNames.GetUserStatus;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetUserTypeList()
        {
            var procedureName = ProcedureNames.GetUserTypes;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest AddUser(User user)
        {
            var procedureName = ProcedureNames.AddUser;
            var parameters = parameterCollectionFactory.CreateAddUserParameters(user);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddUserLogOnLog(UserLogOnLog userLogOnLog)
        {
            var procedureName = ProcedureNames.AddUserLogOnLog;
            var parameters = parameterCollectionFactory.CreateAddUserLogOnLogParameters(userLogOnLog);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetUser(string svnUserName)
        {
            var procedureName = ProcedureNames.GetUserBySvnUserName;
            var parameters = parameterCollectionFactory.CreateGetUserParameters(svnUserName);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetUserByEmailAddress(string emailAddress)
        {
            var procedureName = ProcedureNames.GetUserByEmailAddress;
            var parameters = parameterCollectionFactory.CreateGetUserByEmailAddresParameters(emailAddress);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateUser(User user)
        {
            var procedureName = ProcedureNames.UpdateUser;
            var parameters = parameterCollectionFactory.CreateUpdateUserParameters(user);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateOwnUser(User user)
        {
            var procedureName = ProcedureNames.UpdateOwnUser;
            var parameters = parameterCollectionFactory.CreateUpdateOwnUserParameters(user);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddAudit(string tableName, int recordId, string fieldName, string oldValue, string newValue, int modifiedBy)
        {
            var procedureName = ProcedureNames.AddAudit;
            var parameters = parameterCollectionFactory.CreateAddAuditParameters(tableName, recordId, fieldName, oldValue, newValue, modifiedBy);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetFilteredAuditList(int skip, int take, string orderBy, string orderDir, string searchText, string tableName, int recordId)
        {
            var procedureName = ProcedureNames.GetFilteredAuditList;
            var parameters = parameterCollectionFactory.CreateGetAuditListParameters(skip, take, orderBy, orderDir, searchText, tableName, recordId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest DeleteUser(int userId)
        {
            var procedureName = ProcedureNames.DeleteUser;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithUserIdParameters(userId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetUserUpdateInfo(int id)
        {
            var procedureName = ProcedureNames.GetUserUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddPasswordResetRequest(int userId, string ipAddress, string requestCode)
        {
            var procedureName = ProcedureNames.AddPasswordResetRequest;
            var parameters = parameterCollectionFactory.CreatePasswordResetParameters(userId, ipAddress, requestCode);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest CheckPasswordResetRequest(string requestCode)
        {
            var procedureName = ProcedureNames.CheckPasswordResetRequest;
            var parameters = parameterCollectionFactory.CreateCheckPasswordResetParameters(requestCode);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdatePasswordResetRequest(int userId)
        {
            var procedureName = ProcedureNames.UpdatePasswordResetRequest;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithUserIdParameters(userId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetNugetPackages(int applicationId)
        {
            var procedureName = ProcedureNames.GetNugetPackages;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationNugetPackageById(int packageId)
        {
            var procedureName = ProcedureNames.GetNugetPackageById;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(packageId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationByJenkinsJobName(string name)
        {
            var procedureName = ProcedureNames.GetApplicationByJenkinsJobName;

            var parameters = parameterCollectionFactory.CreateGetApplicationByJenkinsJobNameParameters(name);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetPackageUpdateInfo(int id)
        {
            var procedureName = ProcedureNames.GetPackageUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddApplicationNugetPackage(ApplicationNugetPackage package)
        {
            var procedureName = ProcedureNames.AddApplicationNugetPackage;
            var parameters = parameterCollectionFactory.CreateAddApplicationNugetPackageParameters(package);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateApplicationNugetPackage(ApplicationNugetPackage nugetPackage)
        {
            var procedureName = ProcedureNames.UpdateApplicationNugetPackage;
            var parameters = parameterCollectionFactory.CreateUpdateApplicationNugetPackageParameters(nugetPackage);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationNugetPackageByName(string packageName)
        {
            var procedureName = ProcedureNames.GetApplicationNugetPackageByName;
            var parameters = parameterCollectionFactory.CreateGetApplicationNugetPackageByNameParameters(packageName);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest DeleteApplicationNugetPackage(int packageId)
        {
            var procedureName = ProcedureNames.DeleteApplicationNugetPackage;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(packageId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetEnvironmentById(int id)
        {
            var procedureName = ProcedureNames.GetEnvironmentById;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationGroupUpdateInfo(int applicationGroupId)
        {
            var procedureName = ProcedureNames.GetApplicationGroupUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(applicationGroupId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationUpdateInfo(int applicationId)
        {
            var procedureName = ProcedureNames.GetApplicationUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetEnvironmentByName(string name)
        {
            var procedureName = ProcedureNames.GetEnvironmentByName;
            var parameters = parameterCollectionFactory.CreateGetEnvironmentByNameParameters(name);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetEnvironmentUpdateInfo(int id)
        {
            var procedureName = ProcedureNames.GetEnvironmentUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddEnvironment(Environment environment)
        {
            var procedureName = ProcedureNames.AddEnvironment;
            var parameters = parameterCollectionFactory.CreateAddEnvironmentParameters(environment);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateEnvironment(Environment environment)
        {
            var procedureName = ProcedureNames.UpdateEnvironment;
            var parameters = parameterCollectionFactory.CreateUpdateEnvironmentParameters(environment);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest DeleteEnvironment(int environmentId)
        {
            var procedureName = ProcedureNames.DeleteEnvironment;
            var parameters = parameterCollectionFactory.CreateDeleteEnvironmentParameters(environmentId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationEnvironmentCountByEnvironmentId(int environmentId)
        {
            var procedureName = ProcedureNames.GetApplicationEnvironmentCountByEnvironmentId;
            var parameters = parameterCollectionFactory.ParameterCollectionWithEnvironmentId(environmentId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabaseTypes()
        {
            var procedureName = ProcedureNames.GetDatabaseTypes;

            string splitOnParameters = GetSplitOnParameters(ColumnNames.ModifiedDate);

            return DapperDataRequest.Create(procedureName, splitOnParameters: splitOnParameters);
        }

        public IDataRequest GetDatabaseTypeByName(string name)
        {
            var procedureName = ProcedureNames.GetDatabaseTypeByName;
            var parameters = parameterCollectionFactory.CreateGetDatabaseTypeByNameParameters(name);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddDatabaseType(DatabaseType databaseType)
        {
            var procedureName = ProcedureNames.AddDatabaseType;
            var parameters = parameterCollectionFactory.CreateAddDatabaseTypeParameters(databaseType);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateDatabaseType(DatabaseType databaseType)
        {
            var procedureName = ProcedureNames.UpdateDatabaseType;
            var parameters = parameterCollectionFactory.CreateUpdateDatabaseTypeParameters(databaseType);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabaseTypeById(int id)
        {
            var procedureName = ProcedureNames.GetDatabaseTypeById;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabaseTypeUpdateInfo(int id)
        {
            var procedureName = ProcedureNames.GetDatabaseTypeUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest DeleteDatabaseType(int databaseTypeId)
        {
            var procedureName = ProcedureNames.DeleteDatabaseType;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(databaseTypeId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabaseCountByDatabaseTypeId(int databaseTypeId)
        {
            var procedureName = ProcedureNames.GetDatabaseCountByDatabaseTypeId;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(databaseTypeId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabases()
        {
            var procedureName = ProcedureNames.GetDatabases;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetDatabase(int databaseId)
        {
            var procedureName = ProcedureNames.GetDatabase;
            var parameters = parameterCollectionFactory.ParameterCollectionWithDatabaseId(databaseId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabaseGroups()
        {
            var procedureName = ProcedureNames.GetDatabaseGroups;

            string splitOnParameters = GetSplitOnParameters(ColumnNames.ModifiedDate);

            return DapperDataRequest.Create(procedureName, splitOnParameters: splitOnParameters);
        }

        public IDataRequest GetDatabaseGroupByName(string name)
        {
            var procedureName = ProcedureNames.GetDatabaseGroupByName;
            var parameters = parameterCollectionFactory.CreateGetDatabaseGroupByNameParameters(name);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddDatabaseGroup(DatabaseGroup databaseGroup)
        {
            var procedureName = ProcedureNames.AddDatabaseGroup;
            var parameters = parameterCollectionFactory.CreateAddDatabaseGroupParameters(databaseGroup);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabaseGroupById(int id)
        {
            var procedureName = ProcedureNames.GetDatabaseGroupById;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateDatabaseGroup(DatabaseGroup databaseGroup)
        {
            var procedureName = ProcedureNames.UpdateDatabaseGroup;
            var parameters = parameterCollectionFactory.CreateUpdateDatabaseGroupParameters(databaseGroup);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabaseGroupUpdateInfo(int id)
        {
            var procedureName = ProcedureNames.GetDatabaseGroupUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetFilteredDatabaseList(int skip, int take, string orderBy, string orderDir, string searchText, int databaseGroupId)
        {
            var procedureName = ProcedureNames.GetFilteredDatabaseList;

            string splitOnParameters = GetSplitOnParameters(ColumnNames.ModifiedDate);

            var parameters = parameterCollectionFactory.CreateGetFilteredDatabaseListParameters(skip, take, orderBy, orderDir, searchText, databaseGroupId);

            return DapperDataRequest.Create(procedureName, parameters, splitOnParameters);
        }

        public IDataRequest GetDatabaseUpdateInfo(int databaseId)
        {
            var procedureName = ProcedureNames.GetDatabaseUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(databaseId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateDatabase(Database database)
        {
            var procedureName = ProcedureNames.UpdateDatabase;
            var parameters = parameterCollectionFactory.CreateUpdateDatabaseParameters(database);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabaseByDatabaseName(string name)
        {
            var procedureName = ProcedureNames.GetDatabaseByDatabaseName;
            var parameters = parameterCollectionFactory.CreateGetDatabaseByDatabaseNameParameters(name);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddDatabase(Database database)
        {
            var procedureName = ProcedureNames.AddDatabase;
            var parameters = parameterCollectionFactory.CreateAddDatabaseParameters(database);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabaseByDatabaseTypeId(int id)
        {
            var procedureName = ProcedureNames.GetDatabaseByDatabaseTypeId;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest DeleteDatabase(int databaseId)
        {
            var procedureName = ProcedureNames.DeleteDatabase;
            var parameters = parameterCollectionFactory.ParameterCollectionWithDatabaseId(databaseId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabasesByDatabaseGroupId(int groupId)
        {
            var procedureName = ProcedureNames.GetDatabasesByDatabaseGroupId;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(groupId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabasesByDatabaseName(string databaseName)
        {
            var procedureName = ProcedureNames.GetDatabasesByDatabaseName;
            var parameters = parameterCollectionFactory.CreateGetDatabaseByDatabaseNameParameters(databaseName);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabasesByGroupIdAndName(int databaseGroupId, string databaseName)
        {
            var procedureName = ProcedureNames.GetDatabasesByGroupIdAndName;
            var parameters = parameterCollectionFactory.ParameterCollectionWithDatabaseGroupIdAndName(databaseGroupId, databaseName);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetMenuGroups()
        {
            var procedureName = ProcedureNames.GetMenuGroups;

            return DapperDataRequest.Create(procedureName);
        }

        public IDataRequest GetApplicationBuildVariable(int applicationId)
        {
            var procedureName = ProcedureNames.GetApplicationBuildVariable;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest BuildTypes
        {
            get
            {
                var procedureName = ProcedureNames.GetBuildTypes;

                return DapperDataRequest.Create(procedureName);
            }
        }

        public IDataRequest DeleteDatabaseGroup(int databaseGroupId)
        {
            var procedureName = ProcedureNames.DeleteDatabaseGroup;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(databaseGroupId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabaseCountByDatabaseGroupId(int databaseGroupId)
        {
            var procedureName = ProcedureNames.GetDatabaseCountByDatabaseGroupId;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(databaseGroupId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationTypeByApplicationId(int applicationId)
        {
            var procedureName = ProcedureNames.GetApplicationTypeByApplicationId;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetFavouritePagesByUserId(int userId)
        {
            var procedureName = ProcedureNames.GetUserFavouritePages;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithUserIdParameters(userId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddFavouritePage(FavouritePage favouritePage)
        {
            var procedureName = ProcedureNames.AddFavouritePage;
            var parameters = parameterCollectionFactory.CreateAddFavoritePageParameters(favouritePage);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetFavouritePages(int userId)
        {
            var procedureName = ProcedureNames.GetFavouritePages;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithUserIdParameters(userId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetLargestFavouritePageOrderByUserId(int userId)
        {
            var procedureName = ProcedureNames.GetLargestFavouritePageOrderByUserId;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithUserIdParameters(userId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest IsPageInFavourites(int userId, string pageUrl)
        {
            var procedureName = ProcedureNames.GetFavouritePageCountByUserIdAndPageUrl;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithUserIdAndPageUrlParameters(userId, pageUrl);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest DeleteFavouritePage(int favouriteId)
        {
            var procedureName = ProcedureNames.DeleteFavouritePage;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(favouriteId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetFilteredApplicationRedmineProjectList(int skip, int take, string orderBy, string orderDir, string searchText, int applicationGroupId)
        {
            var procedureName = ProcedureNames.GetFilteredApplicationRedmineProjectList;
            var parameters = parameterCollectionFactory.CreateGetFilteredApplicationRedmineProjectListParameters(skip, take, orderBy, orderDir, searchText, applicationGroupId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetFilteredDatabaseRedmineProjectList(int skip, int take, string orderBy, string orderDir, string searchText, int databaseGroupId)
        {
            var procedureName = ProcedureNames.GetFilteredDatabaseRedmineProjectList;
            var parameters = parameterCollectionFactory.CreateGetFilteredDatabaseRedmineProjectListParameters(skip, take, orderBy, orderDir, searchText, databaseGroupId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetFavouritePage(int userId, string pageUrl)
        {
            var procedureName = ProcedureNames.GetFavouritePageByUserIdAndPageUrl;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithUserIdAndPageUrlParameters(userId, pageUrl);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest SortFavouritePages(List<int> pageIdList)
        {
            var procedureName = ProcedureNames.SortFavouritePages;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithPageIdList(pageIdList);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetMenuUpdateInfo(int id)
        {
            var procedureName = ProcedureNames.GetMenuUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        #region application dependency

        public IDataRequest GetApplicationDependencies(int applicationId)
        {
            var procedureName = ProcedureNames.GetApplicationDependencies;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationDependencyById(int id)
        {
            var procedureName = ProcedureNames.GetApplicationDependencyById;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationDependenciesByApplicationId(int applicationId)
        {
            var procedureName = ProcedureNames.GetApplicationDependenciesByApplicationId;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationDependencyUpdateInfo(int applicationId)
        {
            var procedureName = ProcedureNames.GetApplicationDependencyUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddApplicationDependency(ApplicationDependency applicationDependency)
        {
            var procedureName = ProcedureNames.AddApplicationDependency;
            var parameters = parameterCollectionFactory.CreateAddApplicationDependencyParameters(applicationDependency);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest DeleteApplicationDependency(int applicationDependencyId)
        {
            var procedureName = ProcedureNames.DeleteApplicationDependency;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(applicationDependencyId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateApplicationDependency(ApplicationDependency applicationDependency)
        {
            var procedureName = ProcedureNames.UpdateApplicationDependency;
            var parameters = parameterCollectionFactory.CreateUpdateApplicationDependencyParameters(applicationDependency);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        #endregion

        #region database dependency

        public IDataRequest GetDatabaseDependencies(int applicationId)
        {
            var procedureName = ProcedureNames.GetFullDatabaseDependenciesByApplicationId;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabaseDependencyById(int id)
        {
            var procedureName = ProcedureNames.GetDatabaseDependencyById;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabaseDependenciesByApplicationId(int applicationId)
        {
            var procedureName = ProcedureNames.GetDatabaseDependenciesByApplicationId;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetFullDatabaseDependenciesByApplicationId(int applicationId)
        {
            var procedureName = ProcedureNames.GetFullDatabaseDependenciesByApplicationId;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetDatabaseDependencyUpdateInfo(int id)
        {
            var procedureName = ProcedureNames.GetDatabaseDependencyUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddDatabaseDependency(DatabaseDependency databaseDependency)
        {
            var procedureName = ProcedureNames.AddDatabaseDependency;
            var parameters = parameterCollectionFactory.CreateAddDatabaseDependencyParameters(databaseDependency);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest DeleteDatabaseDependency(int databaseDependencyId)
        {
            var procedureName = ProcedureNames.DeleteDatabaseDependency;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(databaseDependencyId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateDatabaseDependency(DatabaseDependency databaseDependency)
        {
            var procedureName = ProcedureNames.UpdateDatabaseDependency;
            var parameters = parameterCollectionFactory.CreateUpdateDatabaseDependencyParameters(databaseDependency);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        #endregion

        #region external dependency

        public IDataRequest GetExternalDependencies(int applicationId)
        {
            var procedureName = ProcedureNames.GetExternalDependencies;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetExternalDependencyById(int id)
        {
            var procedureName = ProcedureNames.GetExternalDependencyById;
            var parameters = parameterCollectionFactory.CreateGetExternalDependentByIdParameters(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetExternalDependenciesByApplicationId(int applicationId)
        {
            var procedureName = ProcedureNames.GetExternalDependenciesByApplicationId;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetFullExternalDependenciesByApplicationId(int applicationId)
        {
            var procedureName = ProcedureNames.GetFullExternalDependenciesByApplicationId;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetExternalDependencyUpdateInfo(int id)
        {
            var procedureName = ProcedureNames.GetExternalDependencyUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddExternalDependency(ExternalDependency externalDependency)
        {
            var procedureName = ProcedureNames.AddExternalDependency;
            var parameters = parameterCollectionFactory.CreateAddExternalDependencyParameters(externalDependency);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest DeleteExternalDependency(ExternalDependency externalDependency)
        {
            var procedureName = ProcedureNames.DeleteExternalDependency;
            var parameters = parameterCollectionFactory.CreateDeleteExternalDependencyParameters(externalDependency);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateExternalDependency(ExternalDependency externalDependency)
        {
            var procedureName = ProcedureNames.UpdateExternalDependency;
            var parameters = parameterCollectionFactory.CreateUpdateExternalDependencyParameters(externalDependency);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        #endregion

        #region nuget package dependency

        public IDataRequest GetNugetPackageDependencies(int applicationId)
        {
            var procedureName = ProcedureNames.GetNugetPackageDependencies;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetNugetPackageDependenciesById(int id)
        {
            var procedureName = ProcedureNames.GetNugetPackageDependenciesById;
            var parameters = parameterCollectionFactory.ParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetNugetPackageDependenciesByApplicationId(int applicationId)
        {
            var procedureName = ProcedureNames.GetNugetPackageDependenciesByApplicationId;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetNugetPackageDependencyUpdateInfo(int id)
        {
            var procedureName = ProcedureNames.GetNugetPackageDependencyUpdateInfo;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(id);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddNugetPackageDependency(NugetPackageDependency nugetPackageDependency)
        {
            var procedureName = ProcedureNames.AddNugetPackageDependency;
            var parameters = parameterCollectionFactory.CreateAddNugetPackageDependencyParameters(nugetPackageDependency);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest DeleteNugetPackageDependency(int nugetPackageDependencyId)
        {
            var procedureName = ProcedureNames.DeleteNugetPackageDependency;
            var parameters = parameterCollectionFactory.CreateParameterCollectionWithId(nugetPackageDependencyId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        #endregion

        #region application build settings

        public IDataRequest GetApplicationBuildSettings(int applicationId)
        {
            var procedureName = ProcedureNames.GetApplicationBuildSettings;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest AddApplicationBuildSettings(ApplicationBuildSettings buildSettings)
        {
            var procedureName = ProcedureNames.AddApplicationBuildSettings;
            var parameters = parameterCollectionFactory.CreateAddApplicationBuildSettingsParameters(buildSettings);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest UpdateApplicationBuildSettings(ApplicationBuildSettings buildSettings)
        {
            var procedureName = ProcedureNames.UpdateApplicationBuildSettings;
            var parameters = parameterCollectionFactory.CreateUpdateApplicationBuildSettingsParameters(buildSettings);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        public IDataRequest GetApplicationBuildSettingsUpdateInfo(int applicationId)
        {
            var procedureName = ProcedureNames.GetApplicationBuildSettingsUpdateInfo;
            var parameters = parameterCollectionFactory.ParameterCollectionWithApplicationId(applicationId);

            return DapperDataRequest.Create(procedureName, parameters);
        }

        #endregion
    }
}