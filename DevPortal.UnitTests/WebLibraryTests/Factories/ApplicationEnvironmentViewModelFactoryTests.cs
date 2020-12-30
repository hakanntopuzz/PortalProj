using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using Environment = DevPortal.Model.Environment;

namespace DevPortal.UnitTests.WebLibraryTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationEnvironmentViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        ApplicationEnvironmentViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new ApplicationEnvironmentViewModelFactory(breadCrumbFactory.Object,
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

        [Test]
        public void CreateApplicationEnvironment_NoCondition_ReturnApplicationEnvironment()
        {
            // Arrange
            var applicationEnvironment = new ApplicationEnvironment();
            var expectedResult = new ApplicationEnvironment
            {
                ApplicationId = applicationEnvironment.ApplicationId,
                EnvironmentId = applicationEnvironment.EnvironmentId,
                HasLog = applicationEnvironment.HasLog,
                LogFilePath = applicationEnvironment.LogFilePath,
                PhysicalPath = applicationEnvironment.PhysicalPath,
                Url = applicationEnvironment.Url
            };

            // Act
            var result = factory.CreateApplicationEnvironment(applicationEnvironment);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateApplicationEnvironment_NoCondition_ReturnApplicationEnvironments()
        {
            // Arrange
            int applicationId = 1;
            string applicationName = "applicationName";
            var expectedResult = new ApplicationEnvironment
            {
                ApplicationId = applicationId,
                ApplicationName = applicationName
            };

            // Act
            var result = factory.CreateApplicationEnvironment(applicationId, applicationName);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateApplicationEnvironmentViewModel_NoCondition_ReturnApplicationEnvironmentViewModel()
        {
            // Arrange
            var applicationEnvironment = new ApplicationEnvironment();

            // Act
            var result = factory.CreateApplicationEnvironmentViewModel(applicationEnvironment);

            // Assert
            result.ApplicationEnvironment.Should().BeSameAs(applicationEnvironment);
        }

        [Test]
        public void CreateApplicationEnvironmentViewModel_Environment_ReturnApplicationEnvironmentViewModel()
        {
            // Arrange
            var environments = new List<Environment>();
            var model = new ApplicationEnvironmentViewModel
            {
                ApplicationEnvironment = new ApplicationEnvironment { ApplicationId = 1 }
            };
            var breadCrumb = new BreadCrumbViewModel();

            breadCrumbFactory.Setup(x => x.CreateApplicationEnvironmentModel(model.ApplicationEnvironment.ApplicationId)).Returns(breadCrumb);

            var viewModel = new ApplicationEnvironmentViewModel
            {
                ApplicationEnvironment = model.ApplicationEnvironment,
                EnvironmentList = environments,
                BreadCrumbViewModel = breadCrumb
            };

            // Act
            var result = factory.CreateApplicationEnvironmentViewModel(model, environments);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateApplicationEnvironmentDetailViewModel_NoCondition_ReturnApplicationEnvironmentViewModel()
        {
            // Arrange
            var environments = new List<Environment>();
            var model = new ApplicationEnvironmentViewModel
            {
                ApplicationEnvironment = new ApplicationEnvironment { ApplicationId = 1 }
            };
            var breadCrumb = new BreadCrumbViewModel();

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateApplicationEnvironmentDetailModel(model.ApplicationEnvironment.ApplicationId)).Returns(breadCrumb);

            var viewModel = new ApplicationEnvironmentViewModel
            {
                ApplicationEnvironment = model.ApplicationEnvironment,
                BreadCrumbViewModel = breadCrumb,
                IsAuthorized = true,
                FavouritePageName = $"{model.ApplicationEnvironment.ApplicationName} - Uygulama Ortamı - {model.ApplicationEnvironment.EnvironmentName}"
            };

            // Act
            var result = factory.CreateApplicationEnvironmentDetailViewModel(model.ApplicationEnvironment);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateEditApplicationEnvironmentViewModel_NoCondition_ReturnApplicationEnvironmentViewModel()
        {
            // Arrange
            var environments = new List<Environment>();
            var model = new ApplicationEnvironment
            {
                Id = 1,
                ApplicationId = 3
            };
            var breadCrumb = new BreadCrumbViewModel();

            breadCrumbFactory.Setup(x => x.CreateApplicationEnvironmentEditModel(model.Id, model.ApplicationId)).Returns(breadCrumb);

            var viewModel = new ApplicationEnvironmentViewModel
            {
                ApplicationEnvironment = model,
                EnvironmentList = environments,
                BreadCrumbViewModel = breadCrumb
            };

            // Act
            var result = factory.CreateEditApplicationEnvironmentViewModel(model, environments);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }
    }
}