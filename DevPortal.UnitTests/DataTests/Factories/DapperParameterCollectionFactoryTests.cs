using AB.Data.DapperClient.Model;
using AB.Framework.UnitTests;
using DevPortal.Data;
using DevPortal.Data.Factories;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.UnitTests.DataTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DapperParameterCollectionFactoryTests : LooseBaseTestFixture
    {
        #region members & setup

        DapperParameterCollectionFactory factory;

        [SetUp]
        public void Initialize()
        {
            factory = new DapperParameterCollectionFactory();
        }

        #endregion

        [Test]
        public void GetApplicationEnvironments_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationGroupId = 3;

            //Act
            var result = factory.ParameterCollectionWithApplicationGroupId(applicationGroupId);

            //Assert
            result.Get<int>(ParameterNames.ApplicationGroupId).Should().Be(applicationGroupId);
        }

        [Test]
        public void ParameterCollectionWithApplicationId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 3;

            //Act
            var result = factory.ParameterCollectionWithApplicationId(applicationId);

            //Assert
            result.Get<int>(ParameterNames.ApplicationId).Should().Be(applicationId);
        }

        [Test]
        public void ParameterCollectionWithApplicationGroupIdAndName_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 3;
            var applicationName = "applicationGroupId";

            //Act
            var result = factory.ParameterCollectionWithApplicationGroupIdAndName(applicationId, applicationName);

            //Assert
            result.Get<int>(ParameterNames.ApplicationGroupId).Should().Be(applicationId);
            result.Get<string>(ParameterNames.Name).Should().Be(applicationName);
        }

        [Test]
        public void ParameterCollectionWithApplicationName_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationName = "applicationName";

            //Act
            var result = factory.ParameterCollectionWithName(applicationName);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(applicationName);
        }

        #region CreateAddApplicationGroupParameters

        [Test]
        public void CreateAddApplicationGroupParameters_ApplicationGroupNull_ReturnEmptyList()
        {
            //Arrange
            ApplicationGroup applicationGroup = null;

            //Act
            var result = factory.CreateAddApplicationGroupParameters(applicationGroup);

            //Assert
            var expectedResult = new DapperParameterCollection();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateAddApplicationGroupParameters_ApplicationGroupValid_ReturnDataRequest()
        {
            //Arrange
            var applicationGroup = new ApplicationGroup
            {
                Name = "name",
                StatusId = 45
            };

            //Act
            var result = factory.CreateAddApplicationGroupParameters(applicationGroup);

            //Assert
            result.Get<string>(ParameterNames.ApplicationGroupName).Should().Be(applicationGroup.Name);
            result.Get<int>(ParameterNames.StatusId).Should().Be(applicationGroup.StatusId);
        }

        #endregion

        #region CreateAddApplicationParameters

        [Test]
        public void CreateAddApplicationParameters_ApplicationNull_ReturnEmptyList()
        {
            //Arrange
            Application application = null;

            //Act
            var result = factory.CreateAddApplicationParameters(application);

            //Assert
            var expectedResult = new DapperParameterCollection();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateAddApplicationParameters_ApplicationValid_ReturnDataRequest()
        {
            //Arrange

            int createdBy = 45;
            int modifiedBy = createdBy;
            var application = new Application
            {
                Name = "name",
                ApplicationGroupId = 7,
                StatusId = 5,
                ApplicationTypeId = 15,
                RedmineProjectName = "redmineProjectName",
                Description = "description",
                RecordUpdateInfo = new RecordUpdateInfo
                {
                    CreatedBy = createdBy,
                    ModifiedBy = modifiedBy
                }
            };

            //Act
            var result = factory.CreateAddApplicationParameters(application);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(application.Name);
            result.Get<int>(ParameterNames.ApplicationGroupId).Should().Be(application.ApplicationGroupId);
            result.Get<int>(ParameterNames.StatusId).Should().Be(application.StatusId);
            result.Get<int>(ParameterNames.ApplicationTypeId).Should().Be(application.ApplicationTypeId);
            result.Get<string>(ParameterNames.RedmineProjectName).Should().Be(application.RedmineProjectName);
            result.Get<string>(ParameterNames.Description).Should().Be(application.Description);
            result.Get<int>(ParameterNames.CreatedBy).Should().Be(createdBy);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(modifiedBy);
        }

        #endregion

        #region CreateGeneralSettingsParameters

        [Test]
        public void CreateGeneralSettingsParameters_GeneralSettingsNull_ReturnEmptyList()
        {
            //Arrange
            GeneralSettings generalSettings = null;

            //Act
            var result = factory.CreateGeneralSettingsParameters(generalSettings);

            //Assert
            var expectedResult = new DapperParameterCollection();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateGeneralSettingsParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var generalSettings = new GeneralSettings();

            //Act
            var result = factory.CreateGeneralSettingsParameters(generalSettings);

            //Assert
            result.Get<string>(ParameterNames.RedmineUrl).Should().Be(generalSettings.RedmineUrl);
            result.Get<string>(ParameterNames.SvnUrl).Should().Be(generalSettings.SvnUrl);
            result.Get<string>(ParameterNames.JenkinsUrl).Should().Be(generalSettings.JenkinsUrl);
            result.Get<string>(ParameterNames.SonarQubeUrl).Should().Be(generalSettings.SonarQubeUrl);
            result.Get<string>(ParameterNames.NugetUrl).Should().Be(generalSettings.NugetUrl);
            result.Get<string>(ParameterNames.NugetApiKey).Should().Be(generalSettings.NugetApiKey);
            result.Get<string>(ParameterNames.NugetPackageArchiveFolderPath).Should().Be(generalSettings.NugetPackageArchiveFolderPath);
            result.Get<string>(ParameterNames.ApplicationVersionPackageProdFolderPath).Should().Be(generalSettings.ApplicationVersionPackageProdFolderPath);
            result.Get<string>(ParameterNames.ApplicationVersionPackagePreProdFolderPath).Should().Be(generalSettings.ApplicationVersionPackagePreProdFolderPath);
            result.Get<string>(ParameterNames.DatabaseDeploymentPackageProdFolderPath).Should().Be(generalSettings.DatabaseDeploymentPackageProdFolderPath);
            result.Get<string>(ParameterNames.DatabaseDeploymentPackagePreProdFolderPath).Should().Be(generalSettings.DatabaseDeploymentPackagePreProdFolderPath);
        }

        #endregion

        [Test]
        public void CreateAddApplicationEnvironmentParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationEnvironment = new ApplicationEnvironment();

            //Act
            var result = factory.CreateAddApplicationEnvironmentParameters(applicationEnvironment);

            //Assert
            result.Get<int>(ParameterNames.ApplicationId).Should().Be(applicationEnvironment.ApplicationId);
            result.Get<int>(ParameterNames.EnvironmentId).Should().Be(applicationEnvironment.EnvironmentId);
            result.Get<bool>(ParameterNames.HasLog).Should().Be(applicationEnvironment.HasLog);
            result.Get<string>(ParameterNames.Url).Should().Be(applicationEnvironment.Url);
            result.Get<string>(ParameterNames.PhysicalPath).Should().Be(applicationEnvironment.PhysicalPath);
            result.Get<string>(ParameterNames.LogFilePath).Should().Be(applicationEnvironment.LogFilePath);
            result.Get<int>(ParameterNames.CreatedBy).Should().Be(applicationEnvironment.RecordUpdateInfo.CreatedBy);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(applicationEnvironment.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateAddApplicationJenkinsJobParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var jenkinsJob = new JenkinsJob();

            //Act
            var result = factory.CreateAddApplicationJenkinsJobParameters(jenkinsJob);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(jenkinsJob.JenkinsJobName);
            result.Get<int>(ParameterNames.JobTypeId).Should().Be(jenkinsJob.JenkinsJobTypeId);
            result.Get<int>(ParameterNames.ApplicationId).Should().Be(jenkinsJob.ApplicationId);
            result.Get<int>(ParameterNames.CreatedBy).Should().Be(jenkinsJob.RecordUpdateInfo.CreatedBy);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(jenkinsJob.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateUpdateApplicationJenkinsJobParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var jenkinsJob = new JenkinsJob();

            //Act
            var result = factory.CreateUpdateApplicationJenkinsJobParameters(jenkinsJob);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(jenkinsJob.JenkinsJobId);
            result.Get<string>(ParameterNames.Name).Should().Be(jenkinsJob.JenkinsJobName);
            result.Get<int>(ParameterNames.JobTypeId).Should().Be(jenkinsJob.JenkinsJobTypeId);
            result.Get<int>(ParameterNames.ApplicationId).Should().Be(jenkinsJob.ApplicationId);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(jenkinsJob.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateAddApplicationSvnRepositoryParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var svnRepository = new SvnRepository();

            //Act
            var result = factory.CreateAddApplicationSvnRepositoryParameters(svnRepository);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(svnRepository.Name);
            result.Get<int>(ParameterNames.RepositoryTypeId).Should().Be(svnRepository.SvnRepositoryTypeId);
            result.Get<int>(ParameterNames.ApplicationId).Should().Be(svnRepository.ApplicationId);
            result.Get<int>(ParameterNames.CreatedBy).Should().Be(svnRepository.RecordUpdateInfo.CreatedBy);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(svnRepository.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateUpdateApplicationSvnRepositoryParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var svnRepository = new SvnRepository();

            //Act
            var result = factory.CreateUpdateApplicationSvnRepositoryParameters(svnRepository);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(svnRepository.Id);
            result.Get<string>(ParameterNames.Name).Should().Be(svnRepository.Name);
            result.Get<int>(ParameterNames.RepositoryTypeId).Should().Be(svnRepository.SvnRepositoryTypeId);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(svnRepository.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateGetApplicationSvnRepositoryByIdParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var svnRepositoryId = 1;

            //Act
            var result = factory.CreateGetApplicationSvnRepositoryByIdParameters(svnRepositoryId);

            //Assert
            result.Get<int>(ParameterNames.SvnRepositoryId).Should().Be(svnRepositoryId);
        }

        [Test]
        public void ParameterCollectionWithApplicationJenkinsJobId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var jenkinsJobId = 3;

            //Act
            var result = factory.ParameterCollectionWithApplicationJenkinsJobId(jenkinsJobId);

            //Assert
            result.Get<int>(ParameterNames.ApplicationJenkinsJobId).Should().Be(jenkinsJobId);
        }

        [Test]
        public void ParameterCollectionWithApplicationEnvironmentId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 3;

            //Act
            var result = factory.ParameterCollectionWithApplicationEnvironmentId(id);

            //Assert
            result.Get<int>(ParameterNames.ApplicationEnvironmentId).Should().Be(id);
        }

        [Test]
        public void ParameterCollectionWithApplicationAndEnvironmentId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 3;
            var environmentId = 3;

            //Act
            var result = factory.ParameterCollectionWithApplicationAndEnvironmentId(applicationId, environmentId);

            //Assert
            result.Get<int>(ParameterNames.ApplicationId).Should().Be(applicationId);
            result.Get<int>(ParameterNames.EnvironmentId).Should().Be(environmentId);
        }

        #region ParameterCollectionWithId

        [Test]
        public void ParameterCollectionWithId_NoCondition_ReturnId()
        {
            //Arrange
            var id = 10;

            //Act
            var result = factory.ParameterCollectionWithId(id);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(id);
        }

        #endregion

        [Test]
        public void CreateUpdateApplicationEnvironmentParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationEnvironment = new Model.ApplicationEnvironment();

            //Act
            var result = factory.CreateUpdateApplicationEnvironmentParameters(applicationEnvironment);

            //Assert
            result.Get<int>(ParameterNames.ApplicationId).Should().Be(applicationEnvironment.ApplicationId);
            result.Get<int>(ParameterNames.EnvironmentId).Should().Be(applicationEnvironment.EnvironmentId);
            result.Get<bool>(ParameterNames.HasLog).Should().Be(applicationEnvironment.HasLog);
            result.Get<string>(ParameterNames.Url).Should().Be(applicationEnvironment.Url);
            result.Get<string>(ParameterNames.PhysicalPath).Should().Be(applicationEnvironment.PhysicalPath);
            result.Get<string>(ParameterNames.LogFilePath).Should().Be(applicationEnvironment.LogFilePath);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(applicationEnvironment.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateGetApplicationGroupByNameParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var name = "name";

            //Act
            var result = factory.CreateGetApplicationGroupByNameParameters(name);

            //Assert
            result.Get<string>(ParameterNames.ApplicationGroupName).Should().Be(name);
        }

        #region CreateUpdateApplicationParameters

        [Test]
        public void CreateEditApplicationParameters_ApplicationNull_ReturnEmptyList()
        {
            //Arrange
            Application application = null;

            //Act
            var result = factory.CreateUpdateApplicationParameters(application);

            //Assert
            var expectedResult = new DapperParameterCollection();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateEditApplicationParameters_ApplicationValid_ReturnDataRequest()
        {
            //Arrange
            int modifiedBy = 45;
            var application = new Application
            {
                Id = 75,
                Name = "name",
                ApplicationGroupId = 7,
                StatusId = 1,
                ApplicationTypeId = 1,
                RedmineProjectName = "redmine",
                RecordUpdateInfo = new RecordUpdateInfo
                {
                    ModifiedBy = modifiedBy
                }
            };

            //Act
            var result = factory.CreateUpdateApplicationParameters(application);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(application.Id);
            result.Get<string>(ParameterNames.Name).Should().Be(application.Name);
            result.Get<int>(ParameterNames.ApplicationGroupId).Should().Be(application.ApplicationGroupId);
            result.Get<int>(ParameterNames.StatusId).Should().Be(application.StatusId);
            result.Get<int>(ParameterNames.ApplicationTypeId).Should().Be(application.ApplicationTypeId);
            result.Get<string>(ParameterNames.RedmineProjectName).Should().Be(application.RedmineProjectName);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(modifiedBy);
        }

        #endregion

        [Test]
        public void CreateGetApplicationGroupByIdParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;

            //Act
            var result = factory.CreateGetApplicationGroupByIdParameters(id);

            //Assert
            result.Get<int>(ParameterNames.ApplicationGroupId).Should().Be(id);
        }

        #region CreateUpdateApplicationGroupParameters

        [Test]
        public void CreateUpdateApplicationGroupParameters_ApplicationGroupNull_ReturnEmptyList()
        {
            //Arrange
            ApplicationGroup applicationGroup = null;

            //Act
            var result = factory.CreateUpdateApplicationGroupParameters(applicationGroup);

            //Assert
            var expectedResult = new DapperParameterCollection();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateUpdateApplicationGroupParameters_ApplicationGroupValid_ReturnDataRequest()
        {
            //Arrange
            var applicationGroup = new ApplicationGroup
            {
                Id = 12,
                Name = "group",
                StatusId = 45,
                RecordUpdateInfo = new RecordUpdateInfo { ModifiedBy = 56 }
            };

            //Act
            var result = factory.CreateUpdateApplicationGroupParameters(applicationGroup);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(applicationGroup.Id);
            result.Get<string>(ParameterNames.Name).Should().Be(applicationGroup.Name);
            result.Get<int>(ParameterNames.StatusId).Should().Be(applicationGroup.StatusId);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(applicationGroup.RecordUpdateInfo.ModifiedBy);
        }

        #endregion

        [Test]
        public void CreateParameterCollectionWithId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 3;

            //Act
            var result = factory.CreateParameterCollectionWithId(id);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(id);
        }

        [Test]
        public void CreateAddApplicationSonarQubeProjectParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var project = new SonarqubeProject();

            //Act
            var result = factory.CreateAddApplicationSonarQubeProjectParameters(project);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(project.SonarqubeProjectName);
            result.Get<int>(ParameterNames.ProjectTypeId).Should().Be(project.SonarqubeProjectTypeId);
            result.Get<int>(ParameterNames.ApplicationId).Should().Be(project.ApplicationId);
            result.Get<int>(ParameterNames.CreatedBy).Should().Be(project.RecordUpdateInfo.CreatedBy);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(project.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateUpdateApplicationSonarQubeProjectParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var project = new SonarqubeProject();

            //Act
            var result = factory.CreateUpdateApplicationSonarQubeProjectParameters(project);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(project.SonarqubeProjectId);
            result.Get<string>(ParameterNames.Name).Should().Be(project.SonarqubeProjectName);
            result.Get<int>(ParameterNames.ProjectTypeId).Should().Be(project.SonarqubeProjectTypeId);
            result.Get<int>(ParameterNames.ApplicationId).Should().Be(project.ApplicationId);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(project.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateGetFilteredMenuListParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var tableParam = new MenuTableParam { order = new List<TableOrder>() };

            //Act
            var result = factory.CreateGetFilteredMenuListParameters(tableParam);

            //Assert
            result.Get<int>(ParameterNames.DisplayLength).Should().Be(tableParam.length);
            result.Get<int>(ParameterNames.DisplayStart).Should().Be(tableParam.start);
            result.Get<string>(ParameterNames.SortCol).Should().Be(tableParam.SortColumn);
            result.Get<string>(ParameterNames.SortDir).Should().Be(tableParam.order.FirstOrDefault()?.dir);
            result.Get<string>(ParameterNames.SearchValue).Should().Be(tableParam.SearchText);
        }

        [Test]
        public void CreateGetMenuListParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.CreateGetMenuListParameters();

            //Assert
            result.Should().BeEquivalentTo(new DapperParameterCollection());
        }

        [Test]
        public void CreateAddMenuParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            string name = null;
            int? parentId = null;
            string link = null;
            int? order = null;
            string description = null;

            var menuModel = new MenuModel
            {
                Name = name,
                ParentId = parentId,
                Link = link,
                Order = order,
                Description = description
            };

            //Act
            var result = factory.CreateAddMenuParameters(menuModel);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(name);
            result.Get<int?>(ParameterNames.ParentId).Should().Be(parentId);
            result.Get<string>(ParameterNames.Link).Should().Be(link);
            result.Get<int?>(ParameterNames.Order).Should().Be(order);
            result.Get<string>(ParameterNames.Description).Should().Be(description);
        }

        [Test]
        public void CreateUpdateMenuParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 3;
            var name = "name";
            var parentId = 1;
            var link = "link";
            var order = 1;
            var group = 1;
            var description = "description";
            var isDeleted = false;

            //Act
            var result = factory.CreateUpdateMenuParameters(id, name, parentId, link, order, group, description, isDeleted);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(id);
            result.Get<string>(ParameterNames.Name).Should().Be(name);
            result.Get<int>(ParameterNames.ParentId).Should().Be(parentId);
            result.Get<string>(ParameterNames.Link).Should().Be(link);
            result.Get<int>(ParameterNames.Order).Should().Be(order);
            result.Get<int>(ParameterNames.Group).Should().Be(group);
            result.Get<string>(ParameterNames.Description).Should().Be(description);
            result.Get<bool>(ParameterNames.IsDeleted).Should().Be(isDeleted);
        }

        [Test]
        public void CreateUpdateMenuParameters_Update_ReturnDataRequest()
        {
            //Arrange
            var menuModel = new MenuModel
            {
                Id = 1,
                Name = "",
                ParentId = 1,
                Link = "link",
                Order = 1,
                MenuGroupId = 1,
                Description = "description"
            };

            //Act
            var result = factory.CreateUpdateMenuParameters(menuModel);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(menuModel.Id);
            result.Get<string>(ParameterNames.Name).Should().Be(menuModel.Name);
            result.Get<int>(ParameterNames.ParentId).Should().Be(menuModel.ParentId);
            result.Get<string>(ParameterNames.Link).Should().Be(menuModel.Link);
            result.Get<int>(ParameterNames.Order).Should().Be(menuModel.Order);
            result.Get<int>(ParameterNames.Group).Should().Be(menuModel.MenuGroupId);
            result.Get<string>(ParameterNames.Description).Should().Be(menuModel.Description);
        }

        [Test]
        public void CreateGetFilteredApplicationListParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var skip = 3;
            var take = 1;
            var orderBy = "orderBy";
            var orderDir = "orderDir";
            var searchText = "searchText";
            var applicationGroupId = 1;

            //Act
            var result = factory.CreateGetFilteredApplicationListParameters(skip, take, orderBy, orderDir, searchText, applicationGroupId);

            //Assert
            result.Get<int>(ParameterNames.DisplayStart).Should().Be(skip);
            result.Get<int>(ParameterNames.DisplayLength).Should().Be(take);
            result.Get<string>(ParameterNames.SortCol).Should().Be(orderBy);
            result.Get<string>(ParameterNames.SortDir).Should().Be(orderDir);
            result.Get<string>(ParameterNames.SearchValue).Should().Be(searchText);
            result.Get<int>(ParameterNames.ApplicationGroupId).Should().Be(applicationGroupId);
        }

        [Test]
        public void CreateGetFilteredUserListParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var searchFilter = new UserSearchFilter
            {
                order = new List<TableOrder>()
            };

            //Act
            var result = factory.CreateGetFilteredUserListParameters(searchFilter);

            //Assert
            result.Get<int>(ParameterNames.DisplayLength).Should().Be(searchFilter.length);
            result.Get<int>(ParameterNames.DisplayStart).Should().Be(searchFilter.start);
            result.Get<string>(ParameterNames.SortCol).Should().Be(searchFilter.SortColumn);
            result.Get<string>(ParameterNames.SortDir).Should().Be(searchFilter.order.FirstOrDefault()?.dir);
            result.Get<string>(ParameterNames.SearchValue).Should().Be(searchFilter.SearchText);
        }

        [Test]
        public void CreateAddUserParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var user = new User();

            //Act
            var result = factory.CreateAddUserParameters(user);

            //Assert
            result.Get<string>(ParameterNames.SecureId).Should().Be(user.SecureId);
            result.Get<string>(ParameterNames.FirstName).Should().Be(user.FirstName);
            result.Get<string>(ParameterNames.LastName).Should().Be(user.LastName);
            result.Get<string>(ParameterNames.SvnUserName).Should().Be(user.SvnUserName);
            result.Get<string>(ParameterNames.EmailAddress).Should().Be(user.EmailAddress);
            result.Get<string>(ParameterNames.PasswordHash).Should().Be(user.PasswordHash);
            result.Get<int>(ParameterNames.UserStatusId).Should().Be(user.UserStatusId);
            result.Get<int>(ParameterNames.UserTypeId).Should().Be(user.UserTypeId);
            result.Get<int>(ParameterNames.CreatedBy).Should().Be(user.RecordUpdateInfo.CreatedBy);
        }

        [Test]
        public void CreateAddUserLogOnLogParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var userLogOnLog = new UserLogOnLog();

            //Act
            var result = factory.CreateAddUserLogOnLogParameters(userLogOnLog);

            //Assert
            result.Get<int>(ParameterNames.UserId).Should().Be(userLogOnLog.UserId);
            result.Get<string>(ParameterNames.IpAddress).Should().Be(userLogOnLog.IpAddress);
            result.Get<string>(ParameterNames.EmailAddress).Should().Be(userLogOnLog.EmailAddress);
            result.Get<string>(ParameterNames.BrowserInfo).Should().Be(userLogOnLog.BrowserInfo);
            result.Get<string>(ParameterNames.BrowserName).Should().Be(userLogOnLog.BrowserName);
            result.Get<string>(ParameterNames.BrowserVersion).Should().Be(userLogOnLog.BrowserVersion);
        }

        [Test]
        public void CreateGetUserParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var svnUserName = "svnUserName";

            //Act
            var result = factory.CreateGetUserParameters(svnUserName);

            //Assert
            result.Get<string>(ParameterNames.SvnUserName).Should().Be(svnUserName);
        }

        [Test]
        public void CreateGetUserByEmailAddresParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var emailAddress = "emailAddress";

            //Act
            var result = factory.CreateGetUserByEmailAddresParameters(emailAddress);

            //Assert
            result.Get<string>(ParameterNames.EmailAddress).Should().Be(emailAddress);
        }

        [Test]
        public void CreateUpdateUserParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var user = new User();

            //Act
            var result = factory.CreateUpdateUserParameters(user);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(user.Id);
            result.Get<string>(ParameterNames.FirstName).Should().Be(user.FirstName);
            result.Get<string>(ParameterNames.LastName).Should().Be(user.LastName);
            result.Get<string>(ParameterNames.SvnUserName).Should().Be(user.SvnUserName);
            result.Get<string>(ParameterNames.EmailAddress).Should().Be(user.EmailAddress);
            result.Get<int>(ParameterNames.UserStatusId).Should().Be(user.UserStatusId);
            result.Get<int>(ParameterNames.UserTypeId).Should().Be(user.UserTypeId);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(user.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateUpdateOwnUserParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var user = new User();

            //Act
            var result = factory.CreateUpdateOwnUserParameters(user);

            //Assert
            result.Get<string>(ParameterNames.FirstName).Should().Be(user.FirstName);
            result.Get<string>(ParameterNames.LastName).Should().Be(user.LastName);
            result.Get<string>(ParameterNames.SvnUserName).Should().Be(user.SvnUserName);
            result.Get<int>(ParameterNames.UserId).Should().Be(user.Id);
            result.Get<string>(ParameterNames.PasswordHash).Should().Be(user.PasswordHash);
        }

        [Test]
        public void CreateAddAuditParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            string tableName = "tableName";
            int recordId = 1;
            string fieldName = "fieldName";
            string oldValue = "oldValue";
            string newValue = "newValue";
            int modifiedBy = 1;

            //Act
            var result = factory.CreateAddAuditParameters(tableName, recordId, fieldName, oldValue, newValue, modifiedBy);

            //Assert
            result.Get<string>(ParameterNames.TableName).Should().Be(tableName);
            result.Get<int>(ParameterNames.RecordId).Should().Be(recordId);
            result.Get<string>(ParameterNames.FieldName).Should().Be(fieldName);
            result.Get<string>(ParameterNames.OldValue).Should().Be(oldValue);
            result.Get<string>(ParameterNames.NewValue).Should().Be(newValue);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(modifiedBy);
        }

        [Test]
        public void CreateGetAuditListParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var skip = 3;
            var take = 1;
            var orderBy = "orderBy";
            var orderDir = "orderDir";
            var searchText = "searchText";
            var tableName = "tableName";
            var recordId = 1;

            //Act
            var result = factory.CreateGetAuditListParameters(skip, take, orderBy, orderDir, searchText, tableName, recordId);

            //Assert
            result.Get<int>(ParameterNames.DisplayLength).Should().Be(take);
            result.Get<int>(ParameterNames.DisplayStart).Should().Be(skip);
            result.Get<string>(ParameterNames.SortCol).Should().Be(orderBy);
            result.Get<string>(ParameterNames.SortDir).Should().Be(orderDir);
            result.Get<string>(ParameterNames.SearchValue).Should().Be(searchText);
            result.Get<string>(ParameterNames.TableName).Should().Be(tableName);
            result.Get<int>(ParameterNames.RecordId).Should().Be(recordId);
        }

        [Test]
        public void CreateParameterCollectionWithUserIdParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var userId = 5;

            //Act
            var result = factory.CreateParameterCollectionWithUserIdParameters(userId);

            //Assert
            result.Get<int>(ParameterNames.UserId).Should().Be(userId);
        }

        [Test]
        public void CreatePasswordResetParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var userId = 1;
            var ipAddress = "1.1.1.1";
            var requestCode = "requestcode";

            //Act
            var result = factory.CreatePasswordResetParameters(userId, ipAddress, requestCode);

            //Assert
            result.Get<int>(ParameterNames.UserId).Should().Be(userId);
            result.Get<string>(ParameterNames.IpAddress).Should().Be(ipAddress);
            result.Get<string>(ParameterNames.RequestCode).Should().Be(requestCode);
        }

        [Test]
        public void CreateCheckPasswordResetParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var requestCode = "requestcode";

            //Act
            var result = factory.CreateCheckPasswordResetParameters(requestCode);

            //Assert
            result.Get<string>(ParameterNames.RequestCode).Should().Be(requestCode);
        }

        [Test]
        public void CreateGetApplicationByJenkinsJobNameParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var jobName = "job-name";

            //Act
            var result = factory.CreateGetApplicationByJenkinsJobNameParameters(jobName);

            //Assert
            result.Get<string>(ParameterNames.JenkinsJobName).Should().Be(jobName);
        }

        [Test]
        public void CreateAddApplicationNugetPackageParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var package = new ApplicationNugetPackage();

            //Act
            var result = factory.CreateAddApplicationNugetPackageParameters(package);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(package.NugetPackageName);
            result.Get<int>(ParameterNames.ApplicationId).Should().Be(package.ApplicationId);
            result.Get<int>(ParameterNames.CreatedBy).Should().Be(package.RecordUpdateInfo.CreatedBy);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(package.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateUpdateApplicationNugetPackageParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var package = new ApplicationNugetPackage();

            //Act
            var result = factory.CreateUpdateApplicationNugetPackageParameters(package);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(package.NugetPackageId);
            result.Get<string>(ParameterNames.Name).Should().Be(package.NugetPackageName);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(package.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateGetApplicationNugetPackageByNameParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var packageName = "package-name";

            //Act
            var result = factory.CreateGetApplicationNugetPackageByNameParameters(packageName);

            //Assert
            result.Get<string>(ParameterNames.NugetPackageName).Should().Be(packageName);
        }

        [Test]
        public void CreateGetEnvironmentByNameParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var name = "name";

            //Act
            var result = factory.CreateGetEnvironmentByNameParameters(name);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(name);
        }

        [Test]
        public void CreateAddEnvironmentParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var environment = new DevPortal.Model.Environment();

            //Act
            var result = factory.CreateAddEnvironmentParameters(environment);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(environment.Name);
            result.Get<int>(ParameterNames.CreatedBy).Should().Be(environment.RecordUpdateInfo.CreatedBy);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(environment.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateUpdateEnvironmentParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var environment = new DevPortal.Model.Environment();

            //Act
            var result = factory.CreateUpdateEnvironmentParameters(environment);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(environment.Id);
            result.Get<string>(ParameterNames.Name).Should().Be(environment.Name);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(environment.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateDeleteEnvironmentParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;

            //Act
            var result = factory.CreateDeleteEnvironmentParameters(id);

            //Assert
            result.Get<int>(ParameterNames.EnvironmentId).Should().Be(id);
        }

        [Test]
        public void ParameterCollectionWithEnvironmentId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;

            //Act
            var result = factory.ParameterCollectionWithEnvironmentId(id);

            //Assert
            result.Get<int>(ParameterNames.EnvironmentId).Should().Be(id);
        }

        [Test]
        public void CreateGetExternalDependentByIdParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;

            //Act
            var result = factory.CreateGetExternalDependentByIdParameters(id);

            //Assert
            result.Get<int>(ParameterNames.ExternalDependencyId).Should().Be(id);
        }

        [Test]
        public void CreateDeleteExternalDependencyParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var externalDependency = new ExternalDependency
            {
                Id = 5,
                RecordUpdateInfo = new RecordUpdateInfo
                {
                    ModifiedBy = 6
                }
            };

            //Act
            var result = factory.CreateDeleteExternalDependencyParameters(externalDependency);

            //Assert
            result.Get<int>(ParameterNames.ExternalDependencyId).Should().Be(externalDependency.Id);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(externalDependency.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void ParameterCollectionWithDatabaseId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;

            //Act
            var result = factory.ParameterCollectionWithDatabaseId(id);

            //Assert
            result.Get<int>(ParameterNames.DatabaseId).Should().Be(id);
        }

        [Test]
        public void CreateGetDatabaseTypeByNameParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var name = "name";

            //Act
            var result = factory.CreateGetDatabaseTypeByNameParameters(name);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(name);
        }

        [Test]
        public void CreateAddDatabaseTypeParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseType = new DatabaseType();

            //Act
            var result = factory.CreateAddDatabaseTypeParameters(databaseType);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(databaseType.Name);
            result.Get<int>(ParameterNames.CreatedBy).Should().Be(databaseType.RecordUpdateInfo.CreatedBy);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(databaseType.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateUpdateDatabaseTypeParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseType = new DatabaseType();

            //Act
            var result = factory.CreateUpdateDatabaseTypeParameters(databaseType);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(databaseType.Name);
            result.Get<int>(ParameterNames.Id).Should().Be(databaseType.Id);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(databaseType.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateGetDatabaseGroupByNameParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var name = "name";

            //Act
            var result = factory.CreateGetDatabaseGroupByNameParameters(name);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(name);
        }

        [Test]
        public void CreateAddDatabaseGroupParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseGroup = new DatabaseGroup();

            //Act
            var result = factory.CreateAddDatabaseGroupParameters(databaseGroup);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(databaseGroup.Name);
            result.Get<int>(ParameterNames.CreatedBy).Should().Be(databaseGroup.RecordUpdateInfo.CreatedBy);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(databaseGroup.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateUpdateDatabaseGroupParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseGroup = new DatabaseGroup();

            //Act
            var result = factory.CreateUpdateDatabaseGroupParameters(databaseGroup);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(databaseGroup.Id);
            result.Get<string>(ParameterNames.Name).Should().Be(databaseGroup.Name);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(databaseGroup.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateAddExternalDependencyParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var externalDependency = new ExternalDependency
            {
                Name = "Name",
                Description = "Description",
                ApplicationId = 6,
                RecordUpdateInfo = new RecordUpdateInfo
                {
                    CreatedBy = 3
                }
            };

            //Act
            var result = factory.CreateAddExternalDependencyParameters(externalDependency);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(externalDependency.Name);
            result.Get<string>(ParameterNames.Description).Should().Be(externalDependency.Description);
            result.Get<int>(ParameterNames.ApplicationId).Should().Be(externalDependency.ApplicationId);
            result.Get<int>(ParameterNames.CreatedBy).Should().Be(externalDependency.RecordUpdateInfo.CreatedBy);
        }

        [Test]
        public void CreateUpdateExternalDependencyParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var externalDependency = new ExternalDependency
            {
                Id = 23,
                Name = "Name",
                Description = "Description",
                RecordUpdateInfo = new RecordUpdateInfo
                {
                    ModifiedBy = 3
                }
            };

            //Act
            var result = factory.CreateUpdateExternalDependencyParameters(externalDependency);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(externalDependency.Id);
            result.Get<string>(ParameterNames.Name).Should().Be(externalDependency.Name);
            result.Get<string>(ParameterNames.Description).Should().Be(externalDependency.Description);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(externalDependency.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateEditDatabaseParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            int modifiedBy = 45;
            var database = new Database
            {
                Id = 75,
                Name = "name",
                DatabaseGroupId = 7,
                DatabaseTypeId = 1,
                RedmineProjectName = "redmine",
                RecordUpdateInfo = new RecordUpdateInfo
                {
                    ModifiedBy = modifiedBy
                }
            };

            //Act
            var result = factory.CreateUpdateDatabaseParameters(database);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(database.Id);
            result.Get<string>(ParameterNames.Name).Should().Be(database.Name);
            result.Get<int>(ParameterNames.DatabaseGroupId).Should().Be(database.DatabaseGroupId);
            result.Get<int>(ParameterNames.DatabaseTypeId).Should().Be(database.DatabaseTypeId);
            result.Get<string>(ParameterNames.RedmineProjectName).Should().Be(database.RedmineProjectName);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(modifiedBy);
        }

        [Test]
        public void CreateGetFilteredDatabaseListParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var skip = 3;
            var take = 1;
            var orderBy = "orderBy";
            var orderDir = "orderDir";
            var searchText = "searchText";
            var databaseGroupId = 1;

            //Act
            var result = factory.CreateGetFilteredDatabaseListParameters(skip, take, orderBy, orderDir, searchText, databaseGroupId);

            //Assert
            result.Get<int>(ParameterNames.DisplayStart).Should().Be(skip);
            result.Get<int>(ParameterNames.DisplayLength).Should().Be(take);
            result.Get<string>(ParameterNames.SortCol).Should().Be(orderBy);
            result.Get<string>(ParameterNames.SortDir).Should().Be(orderDir);
            result.Get<string>(ParameterNames.SearchValue).Should().Be(searchText);
            result.Get<int>(ParameterNames.DatabaseGroupId).Should().Be(databaseGroupId);
        }

        #region CreateAddDatabaseParameters

        [Test]
        public void CreateAddDatabaseParameters_DatabaseNull_ReturnEmptyList()
        {
            //Arrange
            Database database = null;

            //Act
            var result = factory.CreateAddDatabaseParameters(database);

            //Assert
            var expectedResult = new DapperParameterCollection();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateAddDatabaseParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            int modifiedBy = 45;
            var database = new Database
            {
                Id = 75,
                Name = "name",
                DatabaseGroupId = 7,
                DatabaseTypeId = 1,
                RedmineProjectName = "redmine",
                RecordUpdateInfo = new RecordUpdateInfo
                {
                    ModifiedBy = modifiedBy
                }
            };

            //Act
            var result = factory.CreateAddDatabaseParameters(database);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(database.Name);
            result.Get<string>(ParameterNames.Description).Should().Be(database.Description);
            result.Get<int>(ParameterNames.DatabaseGroupId).Should().Be(database.DatabaseGroupId);
            result.Get<int>(ParameterNames.DatabaseTypeId).Should().Be(database.DatabaseTypeId);
            result.Get<string>(ParameterNames.RedmineProjectName).Should().Be(database.RedmineProjectName);
            result.Get<int>(ParameterNames.CreatedBy).Should().Be(database.RecordUpdateInfo.CreatedBy);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(database.RecordUpdateInfo.ModifiedBy);
        }

        #endregion

        [Test]
        public void CreateGetDatabaseByNameParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var name = "name";

            //Act
            var result = factory.CreateGetDatabaseByDatabaseNameParameters(name);

            //Assert
            result.Get<string>(ParameterNames.Name).Should().Be(name);
        }

        [Test]
        public void ParameterCollectionWithDatabaseGroupId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseGroupId = 45;

            //Act
            var result = factory.ParameterCollectionWithDatabaseGroupId(databaseGroupId);

            //Assert
            result.Get<int>(ParameterNames.DatabaseGroupId).Should().Be(databaseGroupId);
        }

        [Test]
        public void ParameterCollectionWithDatabaseGroupIdAndName_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseGroupId = 45;
            var databaseName = "databaseName";

            //Act
            var result = factory.ParameterCollectionWithDatabaseGroupIdAndName(databaseGroupId, databaseName);

            //Assert
            result.Get<int>(ParameterNames.DatabaseGroupId).Should().Be(databaseGroupId);
            result.Get<string>(ParameterNames.Name).Should().Be(databaseName);
        }

        [Test]
        public void CreateAddDatabaseDependencyParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            int createdBy = 45;
            int modifiedBy = 45;
            var databaseDependency = new DatabaseDependency
            {
                ApplicationId = 1,
                DatabaseId = 1,
                Description = "Description",
                RecordUpdateInfo = new RecordUpdateInfo
                {
                    CreatedBy = createdBy,
                    ModifiedBy = modifiedBy
                }
            };

            //Act
            var result = factory.CreateAddDatabaseDependencyParameters(databaseDependency);

            //Assert
            result.Get<int>(ParameterNames.DependentApplicationId).Should().Be(databaseDependency.ApplicationId);
            result.Get<int>(ParameterNames.DatabaseId).Should().Be(databaseDependency.DatabaseId);
            result.Get<string>(ParameterNames.Description).Should().Be(databaseDependency.Description);
            result.Get<int>(ParameterNames.CreatedBy).Should().Be(databaseDependency.RecordUpdateInfo.CreatedBy);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(databaseDependency.RecordUpdateInfo.ModifiedBy);
        }

        #region CreateUpdateDatabaseDependencyParameters

        [Test]
        public void CreateUpdateDatabaseDependencyParameters_DatabaseDependencyNull_ReturnEmptyList()
        {
            //Arrange
            DatabaseDependency databaseDependency = null;

            //Act
            var result = factory.CreateUpdateDatabaseDependencyParameters(databaseDependency);

            //Assert
            var expectedResult = new DapperParameterCollection();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateUpdateDatabaseDependencyParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            int modifiedBy = 45;
            var databaseDependency = new DatabaseDependency
            {
                Id = 1,
                Description = "Description",
                RecordUpdateInfo = new RecordUpdateInfo
                {
                    ModifiedBy = modifiedBy
                }
            };

            //Act
            var result = factory.CreateUpdateDatabaseDependencyParameters(databaseDependency);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(databaseDependency.Id);
            result.Get<string>(ParameterNames.Description).Should().Be(databaseDependency.Description);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(databaseDependency.RecordUpdateInfo.ModifiedBy);
        }

        #endregion

        #region CreateAddApplicationDependencyParameters

        [Test]
        public void CreateAddApplicationDependencyParameters_ApplicationDependencyNull_ReturnEmptyList()
        {
            //Arrange
            ApplicationDependency applicationDependency = null;

            //Act
            var result = factory.CreateAddApplicationDependencyParameters(applicationDependency);

            //Assert
            var expectedResult = new DapperParameterCollection();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateAddApplicationDependencyParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            int createdBy = 45;
            int modifiedBy = 45;
            var applicationDependency = new ApplicationDependency
            {
                DependedApplicationId = 1,
                ApplicationGroupId = 1,
                Description = "Description",
                RecordUpdateInfo = new RecordUpdateInfo
                {
                    CreatedBy = createdBy,
                    ModifiedBy = modifiedBy
                }
            };

            //Act
            var result = factory.CreateAddApplicationDependencyParameters(applicationDependency);

            //Assert
            result.Get<int>(ParameterNames.DependentApplicationId).Should().Be(applicationDependency.DependentApplicationId);
            result.Get<int>(ParameterNames.DependedApplicationId).Should().Be(applicationDependency.DependedApplicationId);
            result.Get<string>(ParameterNames.Description).Should().Be(applicationDependency.Description);
            result.Get<int>(ParameterNames.CreatedBy).Should().Be(applicationDependency.RecordUpdateInfo.CreatedBy);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(applicationDependency.RecordUpdateInfo.ModifiedBy);
        }

        #endregion

        [Test]
        public void CreateUpdateApplicationDependencyParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationDependency = new ApplicationDependency
            {
                Id = 1,
                Description = "Description",
                RecordUpdateInfo = new RecordUpdateInfo { ModifiedBy = 45 }
            };

            //Act
            var result = factory.CreateUpdateApplicationDependencyParameters(applicationDependency);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(applicationDependency.Id);
            result.Get<string>(ParameterNames.Description).Should().Be(applicationDependency.Description);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(applicationDependency.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateAddFavoritePageParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var favouritePage = new FavouritePage();

            //Act
            var result = factory.CreateAddFavoritePageParameters(favouritePage);

            //Assert
            result.Get<int>(ParameterNames.UserId).Should().Be(favouritePage.Id);
            result.Get<string>(ParameterNames.PageName).Should().Be(favouritePage.PageName);
            result.Get<string>(ParameterNames.PageUrl).Should().Be(favouritePage.PageUrl);
            result.Get<int>(ParameterNames.Order).Should().Be(favouritePage.Order);
        }

        [Test]
        public void CreateParameterCollectionWithUserIdAndPageUrlParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var userId = 3;
            var pageUrl = "pageUrl";

            //Act
            var result = factory.CreateParameterCollectionWithUserIdAndPageUrlParameters(userId, pageUrl);

            //Assert
            result.Get<int>(ParameterNames.UserId).Should().Be(userId);
            result.Get<string>(ParameterNames.PageUrl).Should().Be(pageUrl);
        }

        [Test]
        public void CreateGetFilteredRedmineProjectListParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var skip = 3;
            var take = 1;
            var orderBy = "orderBy";
            var orderDir = "orderDir";
            var searchText = "searchText";
            int applicationGroupId = 45;

            //Act
            var result = factory.CreateGetFilteredApplicationRedmineProjectListParameters(skip, take, orderBy, orderDir, searchText, applicationGroupId);

            //Assert
            result.Get<int>(ParameterNames.DisplayStart).Should().Be(skip);
            result.Get<int>(ParameterNames.DisplayLength).Should().Be(take);
            result.Get<string>(ParameterNames.SortCol).Should().Be(orderBy);
            result.Get<string>(ParameterNames.SortDir).Should().Be(orderDir);
            result.Get<string>(ParameterNames.SearchValue).Should().Be(searchText);
            result.Get<int>(ParameterNames.ApplicationGroupId).Should().Be(applicationGroupId);
        }

        [Test]
        public void CreateGetFilteredDatabaseRedmineProjectListParameters_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var skip = 3;
            var take = 1;
            var orderBy = "orderBy";
            var orderDir = "orderDir";
            var searchText = "searchText";
            int databaseGroupId = 45;

            //Act
            var result = factory.CreateGetFilteredDatabaseRedmineProjectListParameters(skip, take, orderBy, orderDir, searchText, databaseGroupId);

            //Assert
            result.Get<int>(ParameterNames.DisplayStart).Should().Be(skip);
            result.Get<int>(ParameterNames.DisplayLength).Should().Be(take);
            result.Get<string>(ParameterNames.SortCol).Should().Be(orderBy);
            result.Get<string>(ParameterNames.SortDir).Should().Be(orderDir);
            result.Get<string>(ParameterNames.SearchValue).Should().Be(searchText);
            result.Get<int>(ParameterNames.DatabaseGroupId).Should().Be(databaseGroupId);
        }

        #region CreateAddNugetPackageDependencyParameters

        [Test]
        public void CreateAddNugetPackageDependencyParameters_NoCondition_ReturnNugetPackageDependency()
        {
            //Arrange
            var nugetPackageDependency = new NugetPackageDependency();

            //Act
            var result = factory.CreateAddNugetPackageDependencyParameters(nugetPackageDependency);

            //Assert
            result.Get<string>(ParameterNames.NugetPackageName).Should().Be(nugetPackageDependency.NugetPackageName);
            result.Get<int>(ParameterNames.DependentApplicationId).Should().Be(nugetPackageDependency.DependentApplicationId);
            result.Get<int>(ParameterNames.CreatedBy).Should().Be(nugetPackageDependency.RecordUpdateInfo.CreatedBy);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(nugetPackageDependency.RecordUpdateInfo.ModifiedBy);
        }

        #endregion

        [Test]
        public void CreateAddApplicationBuildSettingsParameters_NoCondition_ReturnParameters()
        {
            //Arrange
            var buildSettings = new ApplicationBuildSettings();

            //Act
            var result = factory.CreateAddApplicationBuildSettingsParameters(buildSettings);

            //Assert
            result.Get<string>(ParameterNames.Workspace).Should().Be(buildSettings.Workspace);
            result.Get<string>(ParameterNames.SolutionName).Should().Be(buildSettings.SolutionName);
            result.Get<string>(ParameterNames.ProjectName).Should().Be(buildSettings.ProjectName);
            result.Get<string>(ParameterNames.DeployPath).Should().Be(buildSettings.DeployPath);
            result.Get<string>(ParameterNames.DevPublishProfileName).Should().Be(buildSettings.DevPublishProfileName);
            result.Get<string>(ParameterNames.TestPublishProfileName).Should().Be(buildSettings.TestPublishProfileName);
            result.Get<string>(ParameterNames.PreProdPublishProfileName).Should().Be(buildSettings.PreProdPublishProfileName);
            result.Get<string>(ParameterNames.ProdPublishProfileName).Should().Be(buildSettings.ProdPublishProfileName);
            result.Get<string>(ParameterNames.DevRemoteAddress).Should().Be(buildSettings.DevRemoteAddress);
            result.Get<string>(ParameterNames.TestRemoteAddress).Should().Be(buildSettings.TestRemoteAddress);
            result.Get<string>(ParameterNames.PreProdRemoteAddress).Should().Be(buildSettings.PreProdRemoteAddress);
            result.Get<string>(ParameterNames.ProdRemoteAddress).Should().Be(buildSettings.ProdRemoteAddress);
            result.Get<int>(ParameterNames.CreatedBy).Should().Be(buildSettings.RecordUpdateInfo.CreatedBy);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(buildSettings.RecordUpdateInfo.ModifiedBy);
        }

        [Test]
        public void CreateUpdateApplicationBuildSettingsParameters_NoCondition_ReturnParameters()
        {
            //Arrange
            var buildSettings = new ApplicationBuildSettings();

            //Act
            var result = factory.CreateUpdateApplicationBuildSettingsParameters(buildSettings);

            //Assert
            result.Get<int>(ParameterNames.Id).Should().Be(buildSettings.Id);
            result.Get<int>(ParameterNames.ApplicationId).Should().Be(buildSettings.ApplicationId);
            result.Get<string>(ParameterNames.Workspace).Should().Be(buildSettings.Workspace);
            result.Get<string>(ParameterNames.SolutionName).Should().Be(buildSettings.SolutionName);
            result.Get<string>(ParameterNames.ProjectName).Should().Be(buildSettings.ProjectName);
            result.Get<string>(ParameterNames.DeployPath).Should().Be(buildSettings.DeployPath);
            result.Get<string>(ParameterNames.DevPublishProfileName).Should().Be(buildSettings.DevPublishProfileName);
            result.Get<string>(ParameterNames.TestPublishProfileName).Should().Be(buildSettings.TestPublishProfileName);
            result.Get<string>(ParameterNames.PreProdPublishProfileName).Should().Be(buildSettings.PreProdPublishProfileName);
            result.Get<string>(ParameterNames.ProdPublishProfileName).Should().Be(buildSettings.ProdPublishProfileName);
            result.Get<string>(ParameterNames.DevRemoteAddress).Should().Be(buildSettings.DevRemoteAddress);
            result.Get<string>(ParameterNames.TestRemoteAddress).Should().Be(buildSettings.TestRemoteAddress);
            result.Get<string>(ParameterNames.PreProdRemoteAddress).Should().Be(buildSettings.PreProdRemoteAddress);
            result.Get<string>(ParameterNames.ProdRemoteAddress).Should().Be(buildSettings.ProdRemoteAddress);
            result.Get<int>(ParameterNames.ModifiedBy).Should().Be(buildSettings.RecordUpdateInfo.ModifiedBy);
        }
    }
}