using AB.Data.DapperClient.Model.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Data;
using DevPortal.Data.Abstract.Infrastructure;
using DevPortal.Data.Factories;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.DataTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationDataRequestFactoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IParameterCollectionFactory> parameterCollectionFactory;

        ApplicationDataRequestFactory factory;

        [SetUp]
        public void Initialize()
        {
            parameterCollectionFactory = new StrictMock<IParameterCollectionFactory>();

            factory = new ApplicationDataRequestFactory(parameterCollectionFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            parameterCollectionFactory.VerifyAll();
        }

        #endregion

        #region GetApplicationEnvironments

        [Test]
        public void GetApplicationEnvironments_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationEnvironments(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationEnvironments);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationGroups

        [Test]
        public void GetApplicationGroups_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetApplicationGroups();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationGroups);
        }

        #endregion

        #region GetApplicationsWithLogByApplicationGroup

        [Test]
        public void GetApplicationsWithLogByApplicationGroup_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationGroupId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationGroupId(applicationGroupId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationsWithLogByApplicationGroup(applicationGroupId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationsWithLogByApplicationGroup);
        }

        #endregion

        #region GetApplicationTypes

        [Test]
        public void GetApplicationTypes_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetApplicationTypes();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationTypes);
        }

        #endregion

        #region GetApplicationsByApplicationGroupId

        [Test]
        public void GetApplicationsByApplicationGroupId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationGroupId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationGroupId(applicationGroupId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationsByApplicationGroupId(applicationGroupId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationsByApplicationGroupId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddApplication

        [Test]
        public void AddApplication_NoCondition_ReturnDataRequest()
        {
            //Arrange
            int createdBy = 45;
            var application = new Application
            {
                Name = "name",
                ApplicationGroupId = 7,
                StatusId = 5,
                ApplicationTypeId = 15,
                RedmineProjectName = "redmineProjectName",
                Description = "description",
                RecordUpdateInfo = new RecordUpdateInfo { CreatedBy = createdBy }
            };
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddApplicationParameters(application)).Returns(parameterCollection);

            //Act
            var result = factory.AddApplication(application);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddApplication);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplications

        [Test]
        public void GetApplications_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetApplications();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplications);
        }

        #endregion

        #region GetApplicationsByGroupIdAndName

        [Test]
        public void GetApplicationsByGroupIdAndName_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationGroupId = 2;
            var applicationName = "applicationName";
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationGroupIdAndName(applicationGroupId, applicationName)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationsByGroupIdAndName(applicationGroupId, applicationName);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationsByGroupIdAndName);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationsByGroupId

        [Test]
        public void GetApplicationsByGroupId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationGroupId = 2;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationGroupId(applicationGroupId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationsByGroupId(applicationGroupId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationsByGroupId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationsByApplicationName

        [Test]
        public void GetApplicationsByApplicationName_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationName = "applicationName";
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithName(applicationName)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationsByApplicationName(applicationName);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationsByApplicationName);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationGroupList

        [Test]
        public void GetApplicationGroupList_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetApplicationGroupList();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationGroupList);
        }

        #endregion

        #region AddApplicationGroup

        [Test]
        public void AddApplicationGroup_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationGroup = new ApplicationGroup();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddApplicationGroupParameters(applicationGroup)).Returns(parameterCollection);

            //Act
            var result = factory.AddApplicationGroup(applicationGroup);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddApplicationGroup);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationGroupByName

        [Test]
        public void GetApplicationGroupByName_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var name = "name";
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetApplicationGroupByNameParameters(name)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationGroupByName(name);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationGroupByName);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplication

        [Test]
        public void GetApplication_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplication(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplication);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetSvnRepositories

        [Test]
        public void GetSvnRepositories_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetSvnRepositories(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetSvnRepositories);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetJenkinsJobs

        [Test]
        public void GetJenkinsJobs_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetJenkinsJobs(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetJenkinsJobs);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationJenkinsJobById

        [Test]
        public void GetApplicationJenkinsJobById_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var jenkinsJobId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationJenkinsJobId(jenkinsJobId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationJenkinsJobById(jenkinsJobId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationJenkinsJobById);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationJenkinsJobUpdateInfo

        [Test]
        public void GetApplicationJenkinsJobUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var jenkinsJobId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(jenkinsJobId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationJenkinsJobUpdateInfo(jenkinsJobId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationJenkinsJobUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetJenkinsJobTypes

        [Test]
        public void GetJenkinsJobTypes_NoCondition_ReturnDataRequest()
        {
            //Arrange
            IParameterCollection parameterCollection = null;

            //Act
            var result = factory.GetJenkinsJobTypes();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetJenkinsJobTypes);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region UpdateApplicationJenkinsJob

        [Test]
        public void UpdateApplicationJenkinsJob_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var jenkinsJob = new JenkinsJob();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateApplicationJenkinsJobParameters(jenkinsJob)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateApplicationJenkinsJob(jenkinsJob);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateApplicationJenkinsJob);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetGeneralSettings

        [Test]
        public void GetGeneralSettings_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetGeneralSettings();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetGeneralSettings);
        }

        #endregion

        #region UpdateGeneralSettings

        [Test]
        public void UpdateGeneralSettings_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var generalSettings = new GeneralSettings();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGeneralSettingsParameters(generalSettings)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateGeneralSettings(generalSettings);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateGeneralSettings);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddApplicationEnvironment

        [Test]
        public void AddApplicationEnvironment_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationEnvironment = new ApplicationEnvironment();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddApplicationEnvironmentParameters(applicationEnvironment)).Returns(parameterCollection);

            //Act
            var result = factory.AddApplicationEnvironment(applicationEnvironment);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddApplicationEnvironment);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddApplicationJenkinsJob

        [Test]
        public void AddApplicationJenkinsJob_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var jenkinsJob = new JenkinsJob();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddApplicationJenkinsJobParameters(jenkinsJob)).Returns(parameterCollection);

            //Act
            var result = factory.AddApplicationJenkinsJob(jenkinsJob);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddApplicationJenkinsJob);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetEnvironmentsDoesNotExistByApplicationId

        [Test]
        public void GetEnvironmentsDoesNotExistByApplicationId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 5;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetEnvironmentsDoesNotExistByApplicationId(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetEnvironmentsDoesNotExistByApplicationId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationEnvironmentDetailById

        [Test]
        public void GetApplicationEnvironmentDetailById_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 3;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationEnvironmentId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationEnvironmentById(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationEnvironmentById);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationEnvironmentByEnvironmentId

        [Test]
        public void GetApplicationEnvironmentByEnvironmentId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 3;
            var environmentId = 5;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationAndEnvironmentId(applicationId, environmentId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationEnvironmentByEnvironmentId(applicationId, environmentId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationEnvironmentByEnvironmentId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationEnvironmentUpdateInfo

        [Test]
        public void GetApplicationEnvironmentUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 3;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationEnvironmentUpdateInfo(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationEnvironmentUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region UpdateApplicationEnvironment

        [Test]
        public void UpdateApplicationEnvironment_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationEnvironmentForm = new ApplicationEnvironment();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateApplicationEnvironmentParameters(applicationEnvironmentForm)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateApplicationEnvironment(applicationEnvironmentForm);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateApplicationEnvironment);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetEnvironments

        [Test]
        public void GetEnvironments_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetEnvironments();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetEnvironments);
        }

        #endregion

        #region DeleteApplicationEnvironment

        [Test]
        public void DeleteApplicationEnvironment_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 7;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationEnvironmentId(id)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteApplicationEnvironment(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteApplicationEnvironment);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        [Test]
        public void EditApplication_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var application = new Application();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateApplicationParameters(application)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateApplication(application);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateApplication);
            result.Parameters.Should().Be(parameterCollection);
        }

        [Test]
        public void GetApplicationGroupById_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 7;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetApplicationGroupByIdParameters(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationGroupById(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationGroupById);
            result.Parameters.Should().Be(parameterCollection);
        }

        #region Update Application Group

        [Test]
        public void UpdateApplicationGroup_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationGroup = new ApplicationGroup();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateApplicationGroupParameters(applicationGroup)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateApplicationGroup(applicationGroup);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateApplicationGroup);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region Get Application Group Status

        [Test]
        public void GetApplicationGroupStatus_NoCondition_ReturnDataRequest()
        {
            //Arrange
            IParameterCollection parameterCollection = null;

            //Act
            var result = factory.GetApplicationGroupStatusList();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationGroupStatusList);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region Get Application Status List

        [Test]
        public void GetApplicationStatusList_NoCondition_ReturnDataRequest()
        {
            //Arrange
            IParameterCollection parameterCollection = null;

            //Act
            var result = factory.GetApplicationStatusList();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationStatusList);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region Delete Application Group

        [Test]
        public void DeleteApplicationGroup_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var groupId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationGroupId(groupId)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteApplicationGroup(groupId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteApplicationGroup);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region DeleteApplication

        [Test]
        public void DeleteApplication_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 7;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteApplication(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteApplication);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationCount

        [Test]
        public void GetApplicationCount_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetApplicationCount();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationCount);
        }

        #endregion

        #region GetApplicationGroupCount

        [Test]
        public void GetApplicationGroupCount_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetApplicationGroupCount();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationGroupCount);
        }

        #endregion

        #region GetApplicationCountsByType

        [Test]
        public void GetApplicationCountsByType_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetApplicationCountByType();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationCountByType);
        }

        #endregion

        #region GetJenkinsJobCountByType

        [Test]
        public void GetJenkinsJobCountByType_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetJenkinsJobCountByType();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetJenkinsJobCountByType);
        }

        #endregion

        #region GetJenkinsJobCount

        [Test]
        public void GetJenkinsJobCount_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetJenkinsJobCount();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetJenkinsJobCount);
        }

        #endregion

        #region GetSvnRepositoryCount

        [Test]
        public void GetSvnRepositoryCount_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetSvnRepositoryCount();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetSvnRepositoryCount);
        }

        #endregion

        #region GetSonarQubeProjectCountByType

        [Test]
        public void GetSonarQubeProjectCountByType_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetSonarQubeProjectCountByType();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetSonarQubeProjectCountByType);
        }

        #endregion

        #region GetSonarQubeProjectCount

        [Test]
        public void GetSonarQubeProjectCount_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetSonarQubeProjectCount();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetSonarQubeProjectCount);
        }

        #endregion

        #region GetApplicationRedmineProjectCount

        [Test]
        public void GetApplicationRedmineProjectCount_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetApplicationRedmineProjectCount();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationRedmineProjectCount);
        }

        #endregion

        #region GetDatabaseRedmineProjectCount

        [Test]
        public void GetDatabaseRedmineProjectCount_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetDatabaseRedmineProjectCount();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseRedmineProjectCount);
        }

        #endregion

        #region GetNugetPackageCount

        [Test]
        public void GetNugetPackageCount_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetNugetPackageCount();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetNugetPackageCount);
        }

        #endregion

        #region GetApplicationByApplicationName

        [Test]
        public void GetApplicationByApplicationName_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var name = "name";
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithName(name)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationByApplicationName(name);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationByApplicationName);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetSonarqubeProjects

        [Test]
        public void GetSonarqubeProjects_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetSonarqubeProjects(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetSonarqubeProjects);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetLastUpdatedApplications

        [Test]
        public void GetLastUpdatedApplications_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetLastUpdatedApplications();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetLastUpdatedApplications);
        }

        #endregion

        #region GetApplicationSonarQubeProjectById

        [Test]
        public void GetApplicationSonarQubeProjectById_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var projectId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(projectId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationSonarQubeProjectById(projectId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetSonarQubeProjectById);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationSonarQubeProjectUpdateInfo

        [Test]
        public void GetApplicationSonarQubeProjectUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var projectId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(projectId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationSonarQubeProjectUpdateInfo(projectId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationSonarQubeProjectUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetSonarQubeProjectTypes

        [Test]
        public void GetSonarQubeProjectTypes_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetSonarQubeProjectTypes();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetSonarQubeProjectTypes);
            result.Parameters.Should().BeNull();
        }

        #endregion

        #region AddApplicationSonarQubeProject

        [Test]
        public void AddApplicationSonarQubeProject_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var project = new SonarqubeProject();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddApplicationSonarQubeProjectParameters(project)).Returns(parameterCollection);

            //Act
            var result = factory.AddApplicationSonarQubeProject(project);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddApplicationSonarQubeProject);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region DeleteApplicationSonarQubeProject

        [Test]
        public void DeleteApplicationSonarQubeProject_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var projectId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(projectId)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteApplicationSonarQubeProject(projectId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteApplicationSonarQubeProject);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddApplicationSonarQubeProject

        [Test]
        public void UpdateApplicationSonarQubeProject_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var project = new SonarqubeProject();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateApplicationSonarQubeProjectParameters(project)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateApplicationSonarQubeProject(project);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateApplicationSonarQubeProject);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetFilteredMenuList

        [Test]
        public void GetFilteredMenuList_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var tableParam = new MenuTableParam();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetFilteredMenuListParameters(tableParam)).Returns(parameterCollection);

            //Act
            var result = factory.GetFilteredMenuList(tableParam);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetFilteredMenuList);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetMenuList

        [Test]
        public void GetMenuList_NoCondition_ReturnDataRequest()
        {
            //Arrange
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetMenuListParameters()).Returns(parameterCollection);

            //Act
            var result = factory.GetMenuList();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetMenuList);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddMenu

        [Test]
        public void AddMenu_NoCondition_ReturnDataRequest()
        {
            //Arrange
            const string name = "name";
            int? parentId = null;
            const string link = "link";
            int? order = null;
            const string description = "description";
            var menuModel = new MenuModel
            {
                Name = name,
                ParentId = parentId,
                Link = link,
                Order = order,
                Description = description
            };
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddMenuParameters(menuModel)).Returns(parameterCollection);

            //Act
            var result = factory.AddMenu(menuModel);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddMenu);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetMenu

        [Test]
        public void GetMenu_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 13;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetMenu(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetMenu);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetSubMenuList

        [Test]
        public void GetSubMenuList_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetSubMenuList(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetSubMenuList);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region UpdateMenu

        [Test]
        public void UpdateMenu_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var menuModel = new MenuModel();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateMenuParameters(menuModel)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateMenu(menuModel);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateMenu);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region DeleteMenu

        [Test]
        public void DeleteMenu_NoCondition_ReturnDataRequest()
        {
            //Arrange
            int menuId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(menuId)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteMenu(menuId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteMenu);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetFilteredApplicationList

        [Test]
        public void GetFilteredApplicationList_NoCondition_ReturnDataRequest()
        {
            //Arrange
            int skip = 1;
            int take = 1;
            string orderBy = "tableName";
            string orderDir = "fieldName";
            string searchText = "fieldName";
            int applicationGroupId = 1;

            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetFilteredApplicationListParameters(skip, take, orderBy, orderDir, searchText, applicationGroupId)).Returns(parameterCollection);

            //Act
            var result = factory.GetFilteredApplicationList(skip, take, orderBy, orderDir, searchText, applicationGroupId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetFilteredApplicationList);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetFavouriteRedmineProjects

        [Test]
        public void GetFavouriteRedmineProjects_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetFavouriteRedmineProjects();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetFavouriteRedmineProjects);
            result.Parameters.Should().BeNull();
        }

        #endregion

        #region GetFavouriteRedmineWikiPages

        [Test]
        public void GetFavouriteRedmineWikiPages_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetFavouriteRedmineWikiPages();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetFavouriteRedmineWikiPages);
            result.Parameters.Should().BeNull();
        }

        #endregion

        #region GetToolPages

        [Test]
        public void GetToolPages_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetToolPages();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetToolPages);
            result.Parameters.Should().BeNull();
        }

        #endregion

        #region GetUser

        [Test]
        public void GetUser_UserId_ReturnDataRequest()
        {
            //Arrange
            var id = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetUser(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetUser);
            result.Parameters.Should().BeNull();
        }

        #endregion

        #region DeleteApplicationJenkinsJob

        [Test]
        public void DeleteApplicationJenkinsJob_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 7;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationJenkinsJobId(id)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteApplicationJenkinsJob(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteApplicationJenkinsJob);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddApplicationSvnRepository

        [Test]
        public void AddApplicationSvnRepository_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var svnRepository = new SvnRepository();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddApplicationSvnRepositoryParameters(svnRepository)).Returns(parameterCollection);

            //Act
            var result = factory.AddApplicationSvnRepository(svnRepository);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddApplicationSvnRepository);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region UpdateApplicationSvnRepository

        [Test]
        public void UpdateApplicationSvnRepository_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var svnRepository = new SvnRepository();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateApplicationSvnRepositoryParameters(svnRepository)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateApplicationSvnRepository(svnRepository);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateApplicationSvnRepository);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region DeleteApplicationSvnRepository

        [Test]
        public void DeleteApplicationSvnRepository_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var svnRepositoryId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetApplicationSvnRepositoryByIdParameters(svnRepositoryId)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteApplicationSvnRepository(svnRepositoryId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteApplicationSvnRepository);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region DeleteApplicationSvnRepository

        [Test]
        public void GetApplicationSvnRepositoryById_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var svnRepositoryId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetApplicationSvnRepositoryByIdParameters(svnRepositoryId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationSvnRepositoryById(svnRepositoryId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationSvnRepositoryById);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationSvnRepositoryUpdateInfo

        [Test]
        public void GetApplicationSvnRepositoryUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var svnRepositoryId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(svnRepositoryId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationSvnRepositoryUpdateInfo(svnRepositoryId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationSvnRepositoryUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetSvnRepositoryTypes

        [Test]
        public void GetSvnRepositoryTypes_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetSvnRepositoryTypes();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetSvnRepositoryTypes);
            result.Parameters.Should().BeNull();
        }

        #endregion

        #region GetFilteredUserList

        [Test]
        public void GetFilteredUserList_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var searchFilter = new UserSearchFilter();

            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetFilteredUserListParameters(searchFilter)).Returns(parameterCollection);

            //Act
            var result = factory.GetFilteredUserList(searchFilter);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetFilteredUserList);
            result.Parameters.Should().Be(parameterCollection);
            result.SplitOnParameters.Should().Be(ColumnNames.ModifiedDate);
        }

        #endregion

        #region GetUserStatusList

        [Test]
        public void GetUserStatusList_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetUserStatusList();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetUserStatus);
        }

        #endregion

        #region GetUserTypeList

        [Test]
        public void GetUserTypeList_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetUserTypeList();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetUserTypes);
        }

        #endregion

        #region AddUser

        [Test]
        public void AddUser_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var user = new User();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddUserParameters(user)).Returns(parameterCollection);

            //Act
            var result = factory.AddUser(user);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddUser);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddUserLogOnLog

        [Test]
        public void AddUserLogOnLog_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var userLogOnLog = new UserLogOnLog();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddUserLogOnLogParameters(userLogOnLog)).Returns(parameterCollection);

            //Act
            var result = factory.AddUserLogOnLog(userLogOnLog);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddUserLogOnLog);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetUser

        [Test]
        public void GetUser_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var svnUserName = "svnUserName";
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetUserParameters(svnUserName)).Returns(parameterCollection);

            //Act
            var result = factory.GetUser(svnUserName);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetUserBySvnUserName);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetUser

        [Test]
        public void GetUserByEmailAddress_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var emailAddress = "emailAddress";
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetUserByEmailAddresParameters(emailAddress)).Returns(parameterCollection);

            //Act
            var result = factory.GetUserByEmailAddress(emailAddress);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetUserByEmailAddress);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region UpdateUser

        [Test]
        public void UpdateUser_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var user = new User();

            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateUserParameters(user)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateUser(user);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateUser);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region UpdateOwnUser

        [Test]
        public void UpdateOwnUser_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var user = new User();

            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateOwnUserParameters(user)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateOwnUser(user);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateOwnUser);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddAudit

        [Test]
        public void AddAudit_NoCondition_ReturnDataRequest()
        {
            //Arrange
            string tableName = "tableName";
            int recordId = 1;
            string fieldName = "fieldName";
            string oldValue = "oldValue";
            string newValue = "newValue";
            int modifiedBy = 1;

            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddAuditParameters(tableName, recordId, fieldName, oldValue, newValue, modifiedBy)).Returns(parameterCollection);

            //Act
            var result = factory.AddAudit(tableName, recordId, fieldName, oldValue, newValue, modifiedBy);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddAudit);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetFilteredAuditList

        [Test]
        public void GetFilteredAuditList_NoCondition_ReturnDataRequest()
        {
            //Arrange
            int skip = 1;
            int take = 1;
            string orderBy = "tableName";
            string orderDir = "fieldName";
            string searchText = "fieldName";
            string tableName = "oldValue";
            int recordId = 1;

            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetAuditListParameters(skip, take, orderBy, orderDir, searchText, tableName, recordId)).Returns(parameterCollection);

            //Act
            var result = factory.GetFilteredAuditList(skip, take, orderBy, orderDir, searchText, tableName, recordId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetFilteredAuditList);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region DeleteUser

        [Test]
        public void DeleteUser_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var userId = 1;

            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithUserIdParameters(userId)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteUser(userId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteUser);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetUserUpdateInfo

        [Test]
        public void GetUserUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;

            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetUserUpdateInfo(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetUserUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddPasswordResetRequest

        [Test]
        public void AddPasswordResetRequest_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var userId = 1;
            var ipAddress = "1.1.1.1";
            var requestCode = "requestcode";

            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreatePasswordResetParameters(userId, ipAddress, requestCode)).Returns(parameterCollection);

            //Act
            var result = factory.AddPasswordResetRequest(userId, ipAddress, requestCode);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddPasswordResetRequest);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region CheckPasswordResetRequest

        [Test]
        public void CheckPasswordResetRequest_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var requestCode = "requestcode";

            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateCheckPasswordResetParameters(requestCode)).Returns(parameterCollection);

            //Act
            var result = factory.CheckPasswordResetRequest(requestCode);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.CheckPasswordResetRequest);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region UpdatePasswordResetRequest

        [Test]
        public void UpdatePasswordResetRequest_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var userId = 1;

            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithUserIdParameters(userId)).Returns(parameterCollection);

            //Act
            var result = factory.UpdatePasswordResetRequest(userId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdatePasswordResetRequest);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetNugetPackages

        [Test]
        public void GetNugetPackages_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetNugetPackages(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetNugetPackages);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationByJenkinsJobName

        [Test]
        public void GetApplicationByJenkinsJobName_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var jenkinsJobName = "job-name";
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetApplicationByJenkinsJobNameParameters(jenkinsJobName)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationByJenkinsJobName(jenkinsJobName);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationByJenkinsJobName);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationNugetPackageById

        [Test]
        public void GetApplicationNugetPackageById_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var packageId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(packageId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationNugetPackageById(packageId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetNugetPackageById);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetPackageUpdateInfo

        [Test]
        public void GetPackageUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetPackageUpdateInfo(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetPackageUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddApplicationNugetPackage

        [Test]
        public void AddApplicationNugetPackage_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var package = new ApplicationNugetPackage();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddApplicationNugetPackageParameters(package)).Returns(parameterCollection);

            //Act
            var result = factory.AddApplicationNugetPackage(package);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddApplicationNugetPackage);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region UpdateApplicationNugetPackage

        [Test]
        public void UpdateApplicationNugetPackage_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var package = new ApplicationNugetPackage();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateApplicationNugetPackageParameters(package)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateApplicationNugetPackage(package);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateApplicationNugetPackage);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationNugetPackageByName

        [Test]
        public void GetApplicationNugetPackageByName_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var packageName = "package";
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetApplicationNugetPackageByNameParameters(packageName)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationNugetPackageByName(packageName);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationNugetPackageByName);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region DeleteApplicationNugetPackage

        [Test]
        public void DeleteApplicationNugetPackage_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var packageId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(packageId)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteApplicationNugetPackage(packageId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteApplicationNugetPackage);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetEnvironmentById

        [Test]
        public void GetEnvironmentById_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 5;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetEnvironmentById(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetEnvironmentById);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationGroupUpdateInfo

        [Test]
        public void GetApplicationGroupUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationGroupId = 21;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(applicationGroupId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationGroupUpdateInfo(applicationGroupId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationGroupUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationUpdateInfo

        [Test]
        public void GetApplicationUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 28;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationUpdateInfo(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetEnvironmentByName

        [Test]
        public void GetEnvironmentByName_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var name = "name";
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetEnvironmentByNameParameters(name)).Returns(parameterCollection);

            //Act
            var result = factory.GetEnvironmentByName(name);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetEnvironmentByName);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetEnvironmentUpdateInfo

        [Test]
        public void GetEnvironmentUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetEnvironmentUpdateInfo(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetEnvironmentUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddEnvironment

        [Test]
        public void AddEnvironment_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var environment = new Environment();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddEnvironmentParameters(environment)).Returns(parameterCollection);

            //Act
            var result = factory.AddEnvironment(environment);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddEnvironment);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region UpdateEnvironment

        [Test]
        public void UpdateEnvironment_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var environment = new Environment();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateEnvironmentParameters(environment)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateEnvironment(environment);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateEnvironment);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region DeleteEnvironment

        [Test]
        public void DeleteEnvironment_NoCondition_ReturnDataRequest()
        {
            //Arrange
            int environmentId = 85;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateDeleteEnvironmentParameters(environmentId)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteEnvironment(environmentId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteEnvironment);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationEnvironmentCountByEnvironmentId

        [Test]
        public void GetApplicationEnvironmentCountByEnvironmentId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var environmentId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithEnvironmentId(environmentId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationEnvironmentCountByEnvironmentId(environmentId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationEnvironmentCountByEnvironmentId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetDatabases

        [Test]
        public void GetDatabases_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetDatabases();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabases);
        }

        [Test]
        public void GetDatabase_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithDatabaseId(databaseId)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabase(databaseId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabase);
        }

        [Test]
        public void GetDatabaseUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(databaseId)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabaseUpdateInfo(databaseId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseUpdateInfo);
        }

        [Test]
        public void UpdateDatabase_NoCondition_ReturnDataRequest()
        {
            //Arrange
            Database database = new Database();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateDatabaseParameters(database)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateDatabase(database);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateDatabase);
        }

        [Test]
        public void GetDatabaseByDatabaseName_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var name = "name";
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetDatabaseByDatabaseNameParameters(name)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabaseByDatabaseName(name);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseByDatabaseName);
        }

        [Test]
        public void AddDatabase_NoCondition_ReturnDataRequest()
        {
            //Arrange
            Database database = new Database();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddDatabaseParameters(database)).Returns(parameterCollection);

            //Act
            var result = factory.AddDatabase(database);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddDatabase);
        }

        #endregion

        #region GetFilteredDatabaseList

        [Test]
        public void GetFilteredDatabaseList_NoCondition_ReturnDataRequest()
        {
            //Arrange
            int skip = 1;
            int take = 1;
            string orderBy = "tableName";
            string orderDir = "fieldName";
            string searchText = "fieldName";
            int databaseGroupId = 1;

            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetFilteredDatabaseListParameters(skip, take, orderBy, orderDir, searchText, databaseGroupId)).Returns(parameterCollection);

            //Act
            var result = factory.GetFilteredDatabaseList(skip, take, orderBy, orderDir, searchText, databaseGroupId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetFilteredDatabaseList);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        [Test]
        public void GetDatabaseTypes_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetDatabaseTypes();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseTypes);
        }

        [Test]
        public void GetDatabaseTypeByName_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var name = "name";
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetDatabaseTypeByNameParameters(name)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabaseTypeByName(name);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseTypeByName);
            result.Parameters.Should().Be(parameterCollection);
        }

        [Test]
        public void AddDatabaseType_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseType = new DatabaseType();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddDatabaseTypeParameters(databaseType)).Returns(parameterCollection);

            //Act
            var result = factory.AddDatabaseType(databaseType);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddDatabaseType);
            result.Parameters.Should().Be(parameterCollection);
        }

        [Test]
        public void UpdateDatabaseType_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseType = new DatabaseType();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateDatabaseTypeParameters(databaseType)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateDatabaseType(databaseType);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateDatabaseType);
            result.Parameters.Should().Be(parameterCollection);
        }

        [Test]
        public void GetDatabaseTypeById_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabaseTypeById(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseTypeById);
            result.Parameters.Should().Be(parameterCollection);
        }

        [Test]
        public void GeDatabaseTypeUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabaseTypeUpdateInfo(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseTypeUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        [Test]
        public void DeleteDatabaseType_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseTypeId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(databaseTypeId)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteDatabaseType(databaseTypeId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteDatabaseType);
            result.Parameters.Should().Be(parameterCollection);
        }

        [Test]
        public void GetDatabaseCountByDatabaseTypeId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseTypeId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(databaseTypeId)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabaseCountByDatabaseTypeId(databaseTypeId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseCountByDatabaseTypeId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #region GetDatabaseGroups

        [Test]
        public void GetDatabaseGroups_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetDatabaseGroups();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseGroups);
        }

        #endregion

        #region GetDatabaseGroupByName

        [Test]
        public void GetDatabaseGroupByName_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var name = "name";
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetDatabaseGroupByNameParameters(name)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabaseGroupByName(name);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseGroupByName);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddDatabaseGroup

        [Test]
        public void AddDatabaseGroup_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseGroup = new DatabaseGroup();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddDatabaseGroupParameters(databaseGroup)).Returns(parameterCollection);

            //Act
            var result = factory.AddDatabaseGroup(databaseGroup);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddDatabaseGroup);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetDatabaseGroupById

        [Test]
        public void GetDatabaseGroupById_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabaseGroupById(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseGroupById);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetDatabaseGroupUpdateInfo

        [Test]
        public void GetDatabaseGroupUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabaseGroupUpdateInfo(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseGroupUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region UpdateDatabaseGroup

        [Test]
        public void UpdateDatabaseGroup_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseGroup = new DatabaseGroup();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateDatabaseGroupParameters(databaseGroup)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateDatabaseGroup(databaseGroup);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateDatabaseGroup);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region DeleteDatabaseGroup

        [Test]
        public void DeleteDatabaseGroup_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseGroupId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(databaseGroupId)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteDatabaseGroup(databaseGroupId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteDatabaseGroup);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetDatabaseCountByDatabaseGroupId

        [Test]
        public void GetDatabaseCountByDatabaseGroupId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseGroupId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(databaseGroupId)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabaseCountByDatabaseGroupId(databaseGroupId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseCountByDatabaseGroupId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region get database by database type id

        [Test]
        public void GetDatabaseByDatabaseTypeId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseTypeId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(databaseTypeId)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabaseByDatabaseTypeId(databaseTypeId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseByDatabaseTypeId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region get database by database group id

        [Test]
        public void GetDatabaseByDatabaseGroupId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseGroupId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(databaseGroupId)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabasesByDatabaseGroupId(databaseGroupId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabasesByDatabaseGroupId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region get databases by database name

        [Test]
        public void GetDatabasesByDatabaseName_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseName = "databaseName";
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetDatabaseByDatabaseNameParameters(databaseName)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabasesByDatabaseName(databaseName);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabasesByDatabaseName);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region get databases by Group Id And Name

        [Test]
        public void GetDatabasesByGroupIdAndName_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseGroupId = 1;
            var databaseName = "databaseName";
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithDatabaseGroupIdAndName(databaseGroupId, databaseName)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabasesByGroupIdAndName(databaseGroupId, databaseName);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabasesByGroupIdAndName);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region delete database

        [Test]
        public void DeleteDatabase_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithDatabaseId(databaseId)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteDatabase(databaseId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteDatabase);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetMenuGroups

        [Test]
        public void GetMenuGroups_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.GetMenuGroups();

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetMenuGroups);
        }

        #endregion

        #region GetApplicationBuildVariable

        [Test]
        public void GetApplicationBuildVariable_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationBuildVariable(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationBuildVariable);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetBuildTypes

        [Test]
        public void GetBuildTypes_NoCondition_ReturnDataRequest()
        {
            //Arrange

            //Act
            var result = factory.BuildTypes;

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetBuildTypes);
        }

        #endregion

        #region GetMenuUpdateInfo

        [Test]
        public void GetMenuUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseDependency = new DatabaseDependency();
            IParameterCollection parameterCollection = null;
            var id = 5;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetMenuUpdateInfo(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetMenuUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationTypeByApplicationId

        [Test]
        public void GetApplicationTypeByApplicationId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 5;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationTypeByApplicationId(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationTypeByApplicationId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetFavouritePagesByUserId

        [Test]
        public void GetFavouritePagesByUserId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var userId = 5;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithUserIdParameters(userId)).Returns(parameterCollection);

            //Act
            var result = factory.GetFavouritePagesByUserId(userId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetUserFavouritePages);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddFavouritePage

        [Test]
        public void AddFavouritePage_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var favouritePage = new FavouritePage();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddFavoritePageParameters(favouritePage)).Returns(parameterCollection);

            //Act
            var result = factory.AddFavouritePage(favouritePage);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddFavouritePage);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetFavouritePages

        [Test]
        public void GetFavouritePages_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var userId = 11;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithUserIdParameters(userId)).Returns(parameterCollection);

            //Act
            var result = factory.GetFavouritePages(userId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetFavouritePages);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetLargestFavouritePageOrderByUserId

        [Test]
        public void GetLargestFavouritePageOrderByUserId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var userId = 11;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithUserIdParameters(userId)).Returns(parameterCollection);

            //Act
            var result = factory.GetLargestFavouritePageOrderByUserId(userId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetLargestFavouritePageOrderByUserId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetFavouritePageCountByUserIdAndPageUrl

        [Test]
        public void GetFavouritePageCountByUserIdAndPageUrl_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var userId = 11;
            var pageUrl = "pageUrl";
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithUserIdAndPageUrlParameters(userId, pageUrl)).Returns(parameterCollection);

            //Act
            var result = factory.IsPageInFavourites(userId, pageUrl);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetFavouritePageCountByUserIdAndPageUrl);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region DeleteFavouritePage

        [Test]
        public void DeleteFavouritePage_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var favouriteId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(favouriteId)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteFavouritePage(favouriteId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteFavouritePage);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetFavouritePage

        [Test]
        public void GetFavouritePage_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var userId = 11;
            var pageUrl = "pageUrl";
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithUserIdAndPageUrlParameters(userId, pageUrl)).Returns(parameterCollection);

            //Act
            var result = factory.GetFavouritePage(userId, pageUrl);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetFavouritePageByUserIdAndPageUrl);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetFilteredApplicationRedmineProjectList

        [Test]
        public void GetFilteredApplicationRedmineProjectList_NoCondition_ReturnDataRequest()
        {
            //Arrange
            int skip = 1;
            int take = 1;
            string orderBy = "tableName";
            string orderDir = "fieldName";
            string searchText = "fieldName";
            int applicationGroupId = 45;

            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetFilteredApplicationRedmineProjectListParameters(skip, take, orderBy, orderDir, searchText, applicationGroupId)).Returns(parameterCollection);

            //Act
            var result = factory.GetFilteredApplicationRedmineProjectList(skip, take, orderBy, orderDir, searchText, applicationGroupId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetFilteredApplicationRedmineProjectList);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetFilteredDatabaseRedmineProjectList

        [Test]
        public void GetFilteredDatabaseRedmineProjectList_NoCondition_ReturnDataRequest()
        {
            //Arrange
            int skip = 1;
            int take = 1;
            string orderBy = "tableName";
            string orderDir = "fieldName";
            string searchText = "fieldName";
            int databaseGroupId = 45;

            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetFilteredDatabaseRedmineProjectListParameters(skip, take, orderBy, orderDir, searchText, databaseGroupId)).Returns(parameterCollection);

            //Act
            var result = factory.GetFilteredDatabaseRedmineProjectList(skip, take, orderBy, orderDir, searchText, databaseGroupId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetFilteredDatabaseRedmineProjectList);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region SortFavouritePages

        [Test]
        public void SortFavouritePages_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var pageIdList = new List<int>();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithPageIdList(pageIdList)).Returns(parameterCollection);

            //Act
            var result = factory.SortFavouritePages(pageIdList);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.SortFavouritePages);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationDependencies

        [Test]
        public void GetApplicationDependencies_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 5;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationDependencies(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationDependencies);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationDependencyById

        [Test]
        public void GetApplicationDependencyById_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 5;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationDependencyById(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationDependencyById);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationDependenciesByApplicationId

        [Test]
        public void GetApplicationDependenciesByApplicationId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationDependenciesByApplicationId(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationDependenciesByApplicationId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetApplicationDependencyUpdateInfo

        [Test]
        public void GetApplicationDependencyUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 5;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationDependencyUpdateInfo(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationDependencyUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddApplicationDependency

        [Test]
        public void AddApplicationDependency_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationDependency = new ApplicationDependency();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddApplicationDependencyParameters(applicationDependency)).Returns(parameterCollection);

            //Act
            var result = factory.AddApplicationDependency(applicationDependency);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddApplicationDependency);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region DeleteApplicationDependency

        [Test]
        public void DeleteApplicationDependency_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationDependencyId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(applicationDependencyId)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteApplicationDependency(applicationDependencyId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteApplicationDependency);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region UpdateApplicationDependency

        [Test]
        public void UpdateApplicationDependency_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationDependency = new ApplicationDependency();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateApplicationDependencyParameters(applicationDependency)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateApplicationDependency(applicationDependency);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateApplicationDependency);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetDatabaseDependencies

        [Test]
        public void GetDatabaseDependencies_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 5;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabaseDependencies(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetFullDatabaseDependenciesByApplicationId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetDatabaseDependencyById

        [Test]
        public void GetDatabaseDependencyById_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabaseDependencyById(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseDependencyById);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetDatabaseDependenciesByApplicationId

        [Test]
        public void GetDatabaseDependenciesByApplicationId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabaseDependenciesByApplicationId(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseDependenciesByApplicationId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetFullDatabaseDependenciesByApplicationId

        [Test]
        public void GetFullDatabaseDependenciesByApplicationId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 5;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetFullDatabaseDependenciesByApplicationId(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetFullDatabaseDependenciesByApplicationId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetDatabaseDependencyUpdateInfo

        [Test]
        public void GetDatabaseDependencyUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetDatabaseDependencyUpdateInfo(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetDatabaseDependencyUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddDatabaseDependency

        [Test]
        public void AddDatabaseDependency_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseDependency = new DatabaseDependency();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddDatabaseDependencyParameters(databaseDependency)).Returns(parameterCollection);

            //Act
            var result = factory.AddDatabaseDependency(databaseDependency);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddDatabaseDependency);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region DeleteDatabaseDependency

        [Test]
        public void DeleteDatabaseDependency_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseDependencyId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(databaseDependencyId)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteDatabaseDependency(databaseDependencyId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteDatabaseDependency);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region UpdateDatabaseDependency

        [Test]
        public void UpdateDatabaseDependency_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var databaseDependency = new DatabaseDependency();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateDatabaseDependencyParameters(databaseDependency)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateDatabaseDependency(databaseDependency);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateDatabaseDependency);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetExternalDependencies

        [Test]
        public void GetExternalDependencies_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 5;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetExternalDependencies(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetExternalDependencies);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetExternalDependencyById

        [Test]
        public void GetExternalDependencyById_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateGetExternalDependentByIdParameters(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetExternalDependencyById(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetExternalDependencyById);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetExternalDependenciesByApplicationId

        [Test]
        public void GetExternalDependenciesByApplicationId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetExternalDependenciesByApplicationId(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetExternalDependenciesByApplicationId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetFullExternalDependenciesByApplicationId

        [Test]
        public void GetFullExternalDependenciesByApplicationId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 5;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetFullExternalDependenciesByApplicationId(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetFullExternalDependenciesByApplicationId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetExternalDependencyUpdateInfo

        [Test]
        public void GetExternalDependencyUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetExternalDependencyUpdateInfo(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetExternalDependencyUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddExternalDependency

        [Test]
        public void AddExternalDependency_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var externalDependency = new ExternalDependency();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddExternalDependencyParameters(externalDependency)).Returns(parameterCollection);

            //Act
            var result = factory.AddExternalDependency(externalDependency);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddExternalDependency);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region DeleteExternalDependency

        [Test]
        public void DeleteExternalDependency_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var externalDependency = new ExternalDependency();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateDeleteExternalDependencyParameters(externalDependency)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteExternalDependency(externalDependency);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteExternalDependency);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region UpdateExternalDependency

        [Test]
        public void UpdateExternalDependency_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var externalDependency = new ExternalDependency();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateExternalDependencyParameters(externalDependency)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateExternalDependency(externalDependency);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateExternalDependency);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetNugetPackageDependencies

        [Test]
        public void GetNugetPackageDependencies_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 5;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetNugetPackageDependencies(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetNugetPackageDependencies);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetNugetPackageDependenciesById

        [Test]
        public void GetNugetPackageDependenciesById_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 5;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetNugetPackageDependenciesById(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetNugetPackageDependenciesById);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetNugetPackageDependenciesByApplicationId

        [Test]
        public void GetNugetPackageDependenciesByApplicationId_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetNugetPackageDependenciesByApplicationId(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetNugetPackageDependenciesByApplicationId);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region GetNugetPackageDependencyUpdateInfo

        [Test]
        public void GetNugetPackageDependencyUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var id = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(id)).Returns(parameterCollection);

            //Act
            var result = factory.GetNugetPackageDependencyUpdateInfo(id);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetNugetPackageDependencyUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region AddNugetPackageDependency

        [Test]
        public void AddNugetPackageDependency_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var nugetPackageDependency = new NugetPackageDependency();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddNugetPackageDependencyParameters(nugetPackageDependency)).Returns(parameterCollection);

            //Act
            var result = factory.AddNugetPackageDependency(nugetPackageDependency);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddNugetPackageDependency);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region DeleteNugetPackageDependency

        [Test]
        public void DeleteNugetPackageDependency_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var nugetPackageDependencyId = 5;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateParameterCollectionWithId(nugetPackageDependencyId)).Returns(parameterCollection);

            //Act
            var result = factory.DeleteNugetPackageDependency(nugetPackageDependencyId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.DeleteNugetPackageDependency);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion

        #region Application Build Settings

        [Test]
        public void GetApplicationBuildSettings_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationBuildSettings(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationBuildSettings);
            result.Parameters.Should().Be(parameterCollection);
        }

        [Test]
        public void AddApplicationBuildSettings_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var buildSettings = new ApplicationBuildSettings();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateAddApplicationBuildSettingsParameters(buildSettings)).Returns(parameterCollection);

            //Act
            var result = factory.AddApplicationBuildSettings(buildSettings);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.AddApplicationBuildSettings);
            result.Parameters.Should().Be(parameterCollection);
        }

        [Test]
        public void UpdateApplicationBuildSettings_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var buildSettings = new ApplicationBuildSettings();
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.CreateUpdateApplicationBuildSettingsParameters(buildSettings)).Returns(parameterCollection);

            //Act
            var result = factory.UpdateApplicationBuildSettings(buildSettings);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.UpdateApplicationBuildSettings);
            result.Parameters.Should().Be(parameterCollection);
        }

        [Test]
        public void GetApplicationBuildSettingsUpdateInfo_NoCondition_ReturnDataRequest()
        {
            //Arrange
            var applicationId = 1;
            IParameterCollection parameterCollection = null;

            parameterCollectionFactory.Setup(x => x.ParameterCollectionWithApplicationId(applicationId)).Returns(parameterCollection);

            //Act
            var result = factory.GetApplicationBuildSettingsUpdateInfo(applicationId);

            //Assert
            result.ProcedureName.Should().Be(ProcedureNames.GetApplicationBuildSettingsUpdateInfo);
            result.Parameters.Should().Be(parameterCollection);
        }

        #endregion
    }
}