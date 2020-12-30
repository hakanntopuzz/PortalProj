using AB.Data.DapperClient.Abstract;
using AB.Data.DapperClient.Model.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.DataTests.Repositories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class FavouritePageRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        FavouritePageRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new FavouritePageRepository(dataClient.Object,
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

        #region get favouritePageLinks by user id

        [Test]
        public void GetFavouritePageLinksByUserId_NoCondition_ReturnFavouritePageLinks()
        {
            //Arrange
            var userId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<Link>();
            var expectedValue = new List<Link>();

            dataRequestFactory.Setup(x => x.GetFavouritePagesByUserId(userId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetFavouritePageLinksByUserId(userId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region add favourite page

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AddFavouritePage_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var favouritePage = new FavouritePage();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.AddFavouritePage(favouritePage)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.AddFavouritePage(favouritePage);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region get favourite pages

        [Test]
        public void GetFavouritePages_NoCondition_ReturnFavouritePages()
        {
            //Arrange
            var userId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = new List<FavouritePage>();
            var expectedValue = new List<FavouritePage>();

            dataRequestFactory.Setup(x => x.GetFavouritePages(userId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollection(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetFavouritePages(userId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region get largest favorite

        [Test]
        public void GetLargestFavouritePageOrderByUserId_NoCondition_ReturnCount()
        {
            //Arrange
            var userId = 1;
            var defaultReturnValue = 0;
            var expectedValue = 0;
            IDataRequest dataRequest = null;

            dataRequestFactory.Setup(x => x.GetLargestFavouritePageOrderByUserId(userId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetLargestFavouritePageOrderByUserId(userId);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region is page in favourites

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void IsPageInFavourites_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var userId = 1;
            var pageUrl = "pageUrl";
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.IsPageInFavourites(userId, pageUrl)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.IsPageInFavourites(userId, pageUrl);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region delete favourite page

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DeleteFavouritePage_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var favouriteId = 1;
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.DeleteFavouritePage(favouriteId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.DeleteFavouritePage(favouriteId);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion

        #region get favourite page

        [Test]
        public void GetFavouritePage_NoCondition_ReturnFavouritePage()
        {
            //Arrange
            var userId = 1;
            var pageUrl = "";
            IDataRequest dataRequest = null;
            FavouritePage defaultReturnValue = null;
            var expectedValue = new FavouritePage();

            dataRequestFactory.Setup(x => x.GetFavouritePage(userId, pageUrl)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItem(dataRequest, defaultReturnValue)).Returns(expectedValue);

            //Act
            var result = repository.GetFavouritePage(userId, pageUrl);

            //Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        #endregion

        #region sort favourite pages

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SortFavouritePages_NoCondition_ReturnTrueAndFalse(bool returnValue)
        {
            //Arrange
            var pageIdList = new List<int>();
            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.SortFavouritePages(pageIdList)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalar(dataRequest, defaultReturnValue)).Returns(returnValue);

            //Act
            var result = repository.SortFavouritePages(pageIdList);

            //Assert
            result.Should().Be(returnValue);
        }

        #endregion
    }
}