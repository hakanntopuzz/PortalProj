using AB.Data.DapperClient.Abstract;
using AB.Data.DapperClient.Model.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Factories;
using DevPortal.Data.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.DataTests.Repositories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationGroupRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        ApplicationGroupRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new ApplicationGroupRepository(dataClient.Object, dataRequestFactory.Object, settings.Object);
        }

        void SetupDataClient()
        {
            const string devPortalDbConnectionString = "devPortalDbConnectionString";
            settings.SetupGet(x => x.DevPortalDbConnectionString).Returns(devPortalDbConnectionString);
            dataClient.Setup(x => x.SetConnectionString(devPortalDbConnectionString));
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            dataClient.VerifyAll();
            dataRequestFactory.VerifyAll();
            settings.VerifyAll();
        }

        #endregion

        [Test]
        public void GetApplicationGroupDetailList_NoCondition_ReturnApplicationGroupList()
        {
            //Arrange
            var defaultReturnValue = new List<ApplicationGroup>();
            var expectedValue = new List<ApplicationGroup>();
            Mock<IDataRequest> dataRequest = new Mock<IDataRequest>();

            dataRequestFactory.Setup(x => x.GetApplicationGroupList()).Returns(dataRequest.Object);
            dataClient.Setup(x => x.GetCollection(
                dataRequest.Object,
                DataClientMapFactory.ApplicationGroupsMap,
                defaultReturnValue,
                dataRequest.Object.SplitOnParameters))
                .Returns(expectedValue);

            //Act
            var result = repository.GetApplicationGroups();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void AddApplicationGroup_NoCondition_ReturnTrue()
        {
            //Arrange
            var applicationGroup = new ApplicationGroup();
            var defaultReturnValue = 0;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.AddApplicationGroup(applicationGroup)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(defaultReturnValue);

            //Act
            var result = repository.AddApplicationGroup(applicationGroup);

            //Assert
            result.Should().Be(defaultReturnValue);
        }

        [Test]
        public void AddApplicationGroup_NoCondition_ReturnFalse()
        {
            //Arrange
            var applicationGroup = new ApplicationGroup();
            var defaultReturnValue = 0;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.AddApplicationGroup(applicationGroup)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(0);

            //Act
            var result = repository.AddApplicationGroup(applicationGroup);

            //Assert
            result.Should().Be(0);
        }

        [Test]
        public void GetApplicationGroupByName_NoCondition_ReturnApplicationGroup()
        {
            //Arrange
            var name = "name";
            ApplicationGroup defaultReturnValue = null;
            var expectedValue = new ApplicationGroup();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetApplicationGroupByName(name)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationGroupByName(name);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetApplicationGroupById_NoCondition_ReturnApplicationGroup()
        {
            //Arrange
            var id = 1;
            ApplicationGroup defaultReturnValue = null;
            var expectedValue = new ApplicationGroup();
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetApplicationGroupById(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationGroupById(id);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void UpdateApplicationGroup_NoCondition_ReturnExpectedResult(bool expectedResult)
        {
            //Arrange

            var applicationGroup = new ApplicationGroup();
            var defaultReturnValue = false;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.UpdateApplicationGroup(applicationGroup)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedResult);

            //Act

            var result = repository.UpdateApplicationGroup(applicationGroup);

            //Assert
            result.Should().Be(expectedResult);
        }

        [Test]
        public void GetApplicationGroupStatusList_NoCondition_ReturnApplicationGroupStatusList()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<ApplicationGroupStatus>();
            var expectedValue = new List<ApplicationGroupStatus>();

            dataRequestFactory.Setup(x => x.GetApplicationGroupStatusList()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationGroupStatusList();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DeleteApplicationGroup_NoCondition_ReturnExpectedResult(bool expectedResult)
        {
            //Arrange
            var groupId = 1;
            var defaultReturnValue = false;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.DeleteApplicationGroup(groupId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(expectedResult);

            //Act

            var result = repository.DeleteApplicationGroup(groupId);

            //Assert
            result.Should().Be(expectedResult);
        }

        [Test]
        public void GetApplicationGroupUpdateInfo_NoCondition_ReturnApplicationGroupUpdateInfo()
        {
            //Arrange
            var groupId = 1;
            IDataRequest dataRequest = null;
            RecordUpdateInfo defaultReturnValue = null;
            var expectedValue = new RecordUpdateInfo();

            dataRequestFactory.Setup(x => x.GetApplicationGroupUpdateInfo(groupId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetApplicationGroupUpdateInfo(groupId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }
    }
}