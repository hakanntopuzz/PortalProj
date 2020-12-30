using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.JenkinsManager.Model;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class FavouritesControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IFavouritePageService> favouritePageService;

        StrictMock<IFavouritePageViewModelFactory> viewModelFactory;

        FavouritesController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            favouritePageService = new StrictMock<IFavouritePageService>();
            viewModelFactory = new StrictMock<IFavouritePageViewModelFactory>();

            controller = new FavouritesController(
                favouritePageService.Object,
                userSessionService.Object,
                viewModelFactory.Object);
        }

        #endregion

        #region verify mock

        protected override void VerifyMocks()
        {
            userSessionService.VerifyAll();
            favouritePageService.VerifyAll();
        }

        #endregion

        #region index

        [Test]
        public void Index_NoCondition_ReturnFavouritePageViewModel()
        {
            //Arrange
            var userId = 1;
            var favouritePages = new List<FavouritePage>();

            var viewModel = new FavouritePagesViewModel();

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            favouritePageService.Setup(x => x.GetFavouritePages(userId)).Returns(favouritePages);

            viewModelFactory.Setup(s => s.CreateFavouritePagesViewModel(favouritePages)).Returns(viewModel);

            //Act
            var result = controller.Index();

            //Assert
            result.Should().BeViewResult(ViewNames.Index).ModelAs<FavouritePagesViewModel>().Should().Be(viewModel);
        }

        #endregion

        #region list

        [Test]
        public void List_NoCondition_ReturnFavouritesPartial()
        {
            //Arrange
            var userId = 1;
            var favouritePages = new List<FavouritePage>();
            var viewModel = new FavouritePagesPartialViewModel();

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            favouritePageService.Setup(x => x.GetFavouritePages(userId)).Returns(favouritePages);
            viewModelFactory.Setup(s => s.CreateFavouritePagesPartialViewModel(favouritePages)).Returns(viewModel);

            //Act
            var result = controller.List();

            //Assert
            result.Should().BePartialViewResult(PartialViewNames.List).ModelAs<FavouritePagesPartialViewModel>().Should().Be(viewModel);
        }

        #endregion

        #region add

        [Test]
        public void Add_ModelStateIsNotValid_ReturnServiceResult()
        {
            // Arrange
            var favouritePage = new FavouritePage();
            var addResult = ServiceResult.Error(Messages.AddingFails);
            controller.ModelState.AddModelError("", "invalid model");

            // Act
            var result = controller.Add(favouritePage);

            // Assert
            result.Should().BeOfType<JsonResult>().Which.Value.As<ServiceResult>().Should().BeEquivalentTo(addResult);
        }

        [Test]
        public void Add_NoCondition_ReturnJson()
        {
            //Arrange
            var favouritePage = new FavouritePage();
            var addResult = ServiceResult.Success();
            var userId = 1;

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            favouritePageService.Setup(x => x.AddFavouritePage(favouritePage)).Returns(addResult);

            //Act
            var result = controller.Add(favouritePage);

            //Assert
            result.Should().BeOfType<JsonResult>().Which.Value.As<ServiceResult>().Should().BeEquivalentTo(addResult);
        }

        #endregion

        #region delete

        [Test]
        public void Delete_NoCondition_ReturnJson()
        {
            //Arrange
            var favouriteId = 1;
            var deleteResult = ServiceResult.Success();

            favouritePageService.Setup(x => x.DeleteFavouritePage(favouriteId)).Returns(deleteResult);

            //Act
            var result = controller.Delete(favouriteId);

            //Assert
            result.Should().BeOfType<JsonResult>().Which.Value.As<ServiceResult>().Should().BeEquivalentTo(deleteResult);
        }

        #endregion

        #region  get favourite page count by page url

        [Test]
        public void GetFavouritePageCountByPageUrl_FavouritePageCountNotGreatherThanZero_ReturnJson()
        {
            //Arrange
            FavouritePage favouritePage = null;
            var clientDataResult = ClientDataResult.Error();
            var userId = 1;
            var pageUrl = "pageUrl";

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            favouritePageService.Setup(x => x.GetFavouritePage(userId, pageUrl)).Returns(favouritePage);

            //Act
            var result = controller.GetFavouritePageByUserIdAndPageUrl(pageUrl);

            //Assert
            result.Should().BeOfType<JsonResult>().Which.Value.As<ClientDataResult>().Should().BeEquivalentTo(clientDataResult);
        }

        [Test]
        public void GetFavouritePageCountByPageUrl_FavouritePageCountGreatherThanZero_ReturnJson()
        {
            //Arrange
            var favouritePage = new FavouritePage();
            var clientDataResult = ClientDataResult.Success(favouritePage);
            var userId = 1;
            var pageUrl = "pageUrl";

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            favouritePageService.Setup(x => x.GetFavouritePage(userId, pageUrl)).Returns(favouritePage);

            //Act
            var result = controller.GetFavouritePageByUserIdAndPageUrl(pageUrl);

            //Assert
            result.Should().BeOfType<JsonResult>().Which.Value.As<ClientDataResult>().Should().BeEquivalentTo(clientDataResult);
        }

        #endregion

        #region add/remove/check favourites for jenkins and nuget module

        [Test]
        public void AddToFavourites_InvalidRequest_ReturnBadRequestObjectResult()
        {
            // Arrange
            var request = new AddToFavouritesRequest();
            var serviceResult = new BadRequestObjectResult(Messages.InvalidRequest);

            // Act
            var result = controller.AddToFavourites(request);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddToFavourites_ValidRequest_ReturnOkObjectResult()
        {
            // Arrange
            var request = new AddToFavouritesRequest { PageTitle = "title", PageUrl = new Uri("http://url.com") };
            var favouritePage = new FavouritePage();
            var addResult = ServiceResult.Success();
            var userId = 1;
            var serviceResult = new OkObjectResult(addResult);

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            viewModelFactory.Setup(x => x.CreateAddFavouritePageModel(request.PageTitle, request.PageUrl, userId)).Returns(favouritePage);
            favouritePageService.Setup(x => x.AddFavouritePage(favouritePage)).Returns(addResult);

            // Act
            var result = controller.AddToFavourites(request);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void CheckPageIsFavourite_PageUrlIsNull_ReturnBadRequest()
        {
            //Arrange
            Uri pageUrl = null;
            var validationResult = new BadRequestObjectResult(Messages.InvalidRequest);

            //Act
            var result = controller.CheckPageIsFavourite(pageUrl);

            //Assert
            result.Should().BeEquivalentTo(validationResult);
        }

        [Test]
        public void CheckPageIsFavourite_FavouritePageCountNotGreatherThanZero_ReturnJson()
        {
            //Arrange
            FavouritePage favouritePage = null;
            var clientDataResult = ClientDataResult.Error();
            var userId = 1;
            var pageUrl = new Uri("https://url.com");

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            favouritePageService.Setup(x => x.GetFavouritePage(userId, pageUrl.ToString())).Returns(favouritePage);

            //Act
            var result = controller.CheckPageIsFavourite(pageUrl);

            //Assert
            result.Should().BeOfType<JsonResult>().Which.Value.As<ClientDataResult>().Should().BeEquivalentTo(clientDataResult);
        }

        [Test]
        public void CheckPageIsFavourite_FavouritePageCountGreatherThanZero_ReturnJson()
        {
            //Arrange
            var favouritePage = new FavouritePage();
            var clientDataResult = ClientDataResult.Success(favouritePage);
            var userId = 1;
            var pageUrl = new Uri("https://url.com");

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            favouritePageService.Setup(x => x.GetFavouritePage(userId, pageUrl.ToString())).Returns(favouritePage);

            //Act
            var result = controller.CheckPageIsFavourite(pageUrl);

            //Assert
            result.Should().BeOfType<JsonResult>().Which.Value.As<ClientDataResult>().Should().BeEquivalentTo(clientDataResult);
        }

        [Test]
        public void RemoveFromFavourites_InvalidRequest_ReturnBadRequestObjectResult()
        {
            // Arrange
            RemoveFromFavouritesRequest request = null;
            var serviceResult = new BadRequestObjectResult(Messages.InvalidRequest);

            // Act
            var result = controller.RemoveFromFavourites(request);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void RemoveFromFavourites_ValidRequest_ReturnJson()
        {
            //Arrange
            var request = new RemoveFromFavouritesRequest { Id = 1 };
            var deleteResult = ServiceResult.Success();

            favouritePageService.Setup(x => x.DeleteFavouritePage(request.Id)).Returns(deleteResult);

            //Act
            var result = controller.RemoveFromFavourites(request);

            //Assert
            result.Should().BeOfType<JsonResult>().Which.Value.As<ServiceResult>().Should().BeEquivalentTo(deleteResult);
        }

        #endregion

        #region sort pages

        [Test]
        public void SortPages_ServiceResultIsSuccess_ReturnJson()
        {
            //Arrange
            var data = new Dictionary<string, List<int>>();
            var serviceResult = ServiceResult.Success("success");
            var clientActionResult = ClientActionResult.Success("success");

            favouritePageService.Setup(x => x.SortFavouritePages(data)).Returns(serviceResult);

            //Act
            var result = controller.SortPages(data);

            //Assert
            result.Should().BeOfType<JsonResult>().Which.Value.As<ClientActionResult>().Should().BeEquivalentTo(clientActionResult);
        }

        [Test]
        public void SortPages_ServiceResultIsError_ReturnJson()
        {
            //Arrange
            var data = new Dictionary<string, List<int>>();
            var serviceResult = ServiceResult.Error("error");
            var clientActionResult = ClientActionResult.Error("error");

            favouritePageService.Setup(x => x.SortFavouritePages(data)).Returns(serviceResult);

            //Act
            var result = controller.SortPages(data);

            //Assert
            result.Should().BeOfType<JsonResult>().Which.Value.As<ClientActionResult>().Should().BeEquivalentTo(clientActionResult);
        }

        #endregion
    }
}