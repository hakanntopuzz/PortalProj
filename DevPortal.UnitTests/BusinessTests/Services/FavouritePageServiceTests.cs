using AB.Framework.UnitTests;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class FavouritePageServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IFavouritePageRepository> favouritePageRepository;

        FavouritePageService service;

        [SetUp]
        public void Initialize()
        {
            favouritePageRepository = new StrictMock<IFavouritePageRepository>();

            service = new FavouritePageService(favouritePageRepository.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            favouritePageRepository.VerifyAll();
        }

        #endregion

        #region get favourite pages by userId

        [Test]
        public void GetFavouritePageLinksByUserId_NoCondition_ReturnFavouritePageLinks()
        {
            // Arrange
            var userId = 1;
            ICollection<Link> linkList = null;

            favouritePageRepository.Setup(x => x.GetFavouritePageLinksByUserId(userId)).Returns(linkList);

            // Act
            var result = service.GetFavouritePageLinks(userId);

            // Assert
            result.Should().BeSameAs(linkList);
        }

        #endregion

        #region get favourite pages by count

        [Test]
        public void GetFavouritePageLinksByCount_NoCondition_ReturnFavouritePageLinks()
        {
            // Arrange
            var userId = 1;
            var take = 10;
            List<Link> linkList = new List<Link>();

            favouritePageRepository.Setup(x => x.GetFavouritePageLinksByUserId(userId)).Returns(linkList);

            // Act
            var result = service.GetFavouritePageLinksByCount(userId, take);

            // Assert
            result.Should().BeEquivalentTo(linkList);
        }

        #endregion

        #region add favourite page

        [Test]
        public void AddFavouritePage_FavouritePageCountGreatherThanZero_ReturnServiceResult()
        {
            // Arrange
            var favouritepage = new FavouritePage();
            var expectedResult = ServiceResult.Error(Messages.FavoriteExists);

            favouritePageRepository.Setup(x => x.IsPageInFavourites(favouritepage.UserId, favouritepage.PageUrl)).Returns(true);

            // Act
            var result = service.AddFavouritePage(favouritepage);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void AddFavouritePage_IsNotSuccess_ReturnServiceResult()
        {
            // Arrange
            var favouritepage = new FavouritePage();
            var expectedResult = ServiceResult.Error(Messages.AddingFails);
            var maxOrder = 1;

            favouritePageRepository.Setup(x => x.IsPageInFavourites(favouritepage.UserId, favouritepage.PageUrl)).Returns(false);
            favouritePageRepository.Setup(x => x.GetLargestFavouritePageOrderByUserId(favouritepage.UserId)).Returns(maxOrder);
            favouritePageRepository.Setup(x => x.AddFavouritePage(favouritepage)).Returns(false);

            // Act
            var result = service.AddFavouritePage(favouritepage);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void AddFavouritePage_IsSuccess_ReturnServiceResult()
        {
            // Arrange
            var favouritepage = new FavouritePage();
            var expectedResult = ServiceResult.Success(Messages.FavoriteAdded);
            var maxOrder = 1;

            favouritePageRepository.Setup(x => x.IsPageInFavourites(favouritepage.UserId, favouritepage.PageUrl)).Returns(false);
            favouritePageRepository.Setup(x => x.GetLargestFavouritePageOrderByUserId(favouritepage.UserId)).Returns(maxOrder);
            favouritePageRepository.Setup(x => x.AddFavouritePage(favouritepage)).Returns(true);

            // Act
            var result = service.AddFavouritePage(favouritepage);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region get favourite pages by userId

        [Test]
        public void GetFavouritePages_NoCondition_ReturnFavouritePages()
        {
            // Arrange
            var userId = 1;
            ICollection<FavouritePage> favouritePages = null;

            favouritePageRepository.Setup(x => x.GetFavouritePages(userId)).Returns(favouritePages);

            // Act
            var result = service.GetFavouritePages(userId);

            // Assert
            result.Should().BeSameAs(favouritePages);
        }

        #endregion

        #region delete favourite page

        [Test]
        public void DeleteFavouritePage_IsNotSuccess_ReturnServiceResult()
        {
            // Arrange
            var favouriteId = 1;
            var expectedResult = ServiceResult.Error(Messages.DeleteFails);

            favouritePageRepository.Setup(x => x.DeleteFavouritePage(favouriteId)).Returns(false);

            // Act
            var result = service.DeleteFavouritePage(favouriteId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void DeleteFavouritePage_IsSuccess_ReturnServiceResult()
        {
            // Arrange
            var favouriteId = 1;
            var expectedResult = ServiceResult.Success(Messages.FavoriteDeleted);

            favouritePageRepository.Setup(x => x.DeleteFavouritePage(favouriteId)).Returns(true);

            // Act
            var result = service.DeleteFavouritePage(favouriteId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region get favourite page

        [Test]
        public void GetFavouritePage_NoCondition_ReturnFavouritePage()
        {
            // Arrange
            var userId = 1;
            var pageUrl = "";
            var favouritePage = new FavouritePage();

            favouritePageRepository.Setup(x => x.GetFavouritePage(userId, pageUrl)).Returns(favouritePage);

            // Act
            var result = service.GetFavouritePage(userId, pageUrl);

            // Assert
            result.Should().BeEquivalentTo(favouritePage);
        }

        #endregion

        #region sort favourite page

        [Test]
        public void SortFavouritePages_IsNotSuccess_ReturnServiceResult()
        {
            // Arrange
            var pageList = new Dictionary<string, List<int>>();
            var pageIdList = new List<int>();
            pageList.Add("item", pageIdList);

            var expectedResult = ServiceResult.Error(Messages.SortFails);

            favouritePageRepository.Setup(x => x.SortFavouritePages(pageIdList)).Returns(false);

            // Act
            var result = service.SortFavouritePages(pageList);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void SortFavouritePages_IsSuccess_ReturnServiceResult()
        {
            // Arrange
            var pageList = new Dictionary<string, List<int>>();
            var pageIdList = new List<int>();
            pageList.Add("item", pageIdList);

            var expectedResult = ServiceResult.Success(Messages.FavoritePageSorted);

            favouritePageRepository.Setup(x => x.SortFavouritePages(pageIdList)).Returns(true);

            // Act
            var result = service.SortFavouritePages(pageList);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion
    }
}