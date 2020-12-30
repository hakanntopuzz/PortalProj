using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Web.Library;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class EnvironmentViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        EnvironmentViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new EnvironmentViewModelFactory(breadCrumbFactory.Object,
                authorizationServiceWrapper.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            breadCrumbFactory.VerifyAll();
            authorizationServiceWrapper.VerifyAll();
        }

        #endregion

        #region environment

        [Test]
        public void CreateEnvironmentsViewModel_NoCondition_ReturnEnvironmentListViewModel()
        {
            // Arrange
            var environments = new List<DevPortal.Model.Environment>();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var expectedResult = new EnvironmentViewModel
            {
                Environments = environments,
                IsAuthorized = true,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateEnvironmentsModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateEnvironmentsViewModel(environments);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateEnvironmentDetailViewModel_NoCondition_ReturnEnvironmentListViewModel()
        {
            // Arrange
            var environment = new DevPortal.Model.Environment();
            var environments = new List<DevPortal.Model.Environment>();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var expectedResult = new EnvironmentViewModel
            {
                Environment = environment,
                IsAuthorized = true,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateEnvironmentDetailModel(environment.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateEnvironmentDetailViewModel(environment);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateEnvironmentEditViewModel_NoCondition_ReturnEnvironmentListViewModel()
        {
            // Arrange
            var environment = new DevPortal.Model.Environment();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var expectedResult = new EnvironmentViewModel
            {
                Environment = environment,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateEnvironmentEditModel(environment.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateEnvironmentEditViewModel(environment);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateEnvironmentAddView_NoCondition_ReturnEnvironmentListViewModel()
        {
            // Arrange
            var breadcrumbViewModel = new BreadCrumbViewModel();

            breadCrumbFactory.Setup(x => x.CreateEnvironmentAddModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateEnvironmentAddView();

            // Assert
            result.BreadCrumbViewModel.Should().Be(breadcrumbViewModel);
        }

        #endregion
    }
}