using AB.Data.DapperClient.Abstract;
using AB.Data.DapperClient.Model.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.DataTests.Repositories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ShortcutRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        ShortcutRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();

            repository = new ShortcutRepository(
                dataClient.Object,
                dataRequestFactory.Object,
                settings.Object);
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
        public void GetFavouriteRedmineProjects_NoCondition_ReturnProjects()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<Link>();
            var expectedValue = new List<Link>();

            dataRequestFactory.Setup(x => x.GetFavouriteRedmineProjects()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetFavouriteRedmineProjects();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetFavouriteRedmineWikiPages_NoCondition_ReturnPages()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<Link>();
            var expectedValue = new List<Link>();

            dataRequestFactory.Setup(x => x.GetFavouriteRedmineWikiPages()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetFavouriteRedmineWikiPages();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        [Test]
        public void GetToolPages_NoCondition_ReturnPages()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<Link>();
            var expectedValue = new List<Link>();

            dataRequestFactory.Setup(x => x.GetToolPages()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetToolPages();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }
    }
}