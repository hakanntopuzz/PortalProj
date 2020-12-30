using AB.Framework.UnitTests;
using DevPortal.Model;
using DevPortal.Web.Library;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class FavouritePageViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        FavouritePageViewModelFactory factory;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();

            factory = new FavouritePageViewModelFactory(breadCrumbFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            breadCrumbFactory.VerifyAll();
        }

        #endregion

        [Test]
        public void CreateFavouritePagesViewModel_NoCondition_ReturnViewModel()
        {
            // Arrange
            var favouritePages = new List<FavouritePage>();
            var partialModel = new FavouritePagesPartialViewModel { FavouritePages = favouritePages };
            BreadCrumbViewModel breadCrumbViewModel = null;
            var viewModel = new FavouritePagesViewModel
            {
                BreadCrumbViewModel = breadCrumbViewModel,
                FavouritePagesPartialModel = partialModel
            };

            breadCrumbFactory.Setup(x => x.CreateFavouritePagesModel()).Returns(breadCrumbViewModel);

            // Act
            var result = factory.CreateFavouritePagesViewModel(favouritePages);

            // Assert
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateFavouritePagesPartialViewModel_NoCondition_ReturnViewModel()
        {
            // Arrange
            var favouritePages = new List<FavouritePage>();
            var viewModel = new FavouritePagesPartialViewModel { FavouritePages = favouritePages };

            // Act
            var result = factory.CreateFavouritePagesPartialViewModel(favouritePages);

            // Assert
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateAddFavouritePageModel_PageUrlNull_ReturnModel()
        {
            // Arrange
            var pageName = "name";
            Uri pageUrl = null;
            var userId = 1;

            // Act
            var result = factory.CreateAddFavouritePageModel(pageName, pageUrl, userId);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void CreateAddFavouritePageModel_ParametersValid_ReturnModel()
        {
            // Arrange
            var pageName = "name";
            var pageUrl = new Uri("http://url.com");
            var userId = 1;

            var model = new FavouritePage { PageName = pageName, PageUrl = pageUrl.ToString(), UserId = userId };

            // Act
            var result = factory.CreateAddFavouritePageModel(pageName, pageUrl, userId);

            // Assert
            result.Should().BeEquivalentTo(model);
            result.UserId.Should().Be(model.UserId);
            result.PageName.Should().Be(model.PageName);
            result.PageUrl.Should().Be(model.PageUrl);
        }
    }
}