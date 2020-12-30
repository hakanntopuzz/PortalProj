using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
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
    public class ApplicationGroupViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        ApplicationGroupViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new ApplicationGroupViewModelFactory(breadCrumbFactory.Object,
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

        #region application group

        [Test]
        public void CreateApplicationGroupsViewModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationGroups = new List<ApplicationGroup>();
            var breadcrumbViewModel = new BreadCrumbViewModel();

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateApplicationGroupsModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateApplicationGroupsViewModel(applicationGroups);

            // Assert
            result.ApplicationGroups.Should().BeSameAs(applicationGroups);
        }

        [Test]
        public void CreateApplicationGroupAddViewModel_NoCondition_ReturnApplicationGroupViewModel()
        {
            // Arrange
            var status = new List<ApplicationGroupStatus>();
            var breadcrumbViewModel = new BreadCrumbViewModel();

            var viewModel = new ApplicationGroupViewModel
            {
                Status = status,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateApplicationGroupAddModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateApplicationGroupAddViewModel(status);

            // Assert
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateEditApplicationGroup_NoCondition_ReturnApplicationGroupViewModel()
        {
            // Arrange
            var status = new List<ApplicationGroupStatus>();
            var applicationGroup = new ApplicationGroup();
            var breadcrumbViewModel = new BreadCrumbViewModel();

            var viewModel = new ApplicationGroupViewModel
            {
                ApplicationGroup = applicationGroup,
                Status = status,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateApplicationGroupEditModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateEditApplicationGroup(applicationGroup, status);

            // Assert
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateDetailApplicationGroup_NoCondition_ReturnApplicationGroupViewModel()
        {
            // Arrange
            var applicationLists = new List<ApplicationListItem>();
            var applicationGroup = new ApplicationGroup();
            var breadcrumbViewModel = new BreadCrumbViewModel();

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateApplicationGroupDetailModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateDetailApplicationGroup(applicationGroup, applicationLists);

            // Assert
            var expectedResult = CreateApplicationGroupExpectedResult(null, breadcrumbViewModel, applicationGroup, applicationLists);
            result.Should().BeEquivalentTo(expectedResult);
        }

        static ApplicationGroupViewModel CreateApplicationGroupExpectedResult(
        List<ApplicationGroupStatus> status,
         BreadCrumbViewModel breadCrumbViewModel,
        ApplicationGroup applicationGroup,
        List<ApplicationListItem> applicationLists)
        {
            return new ApplicationGroupViewModel
            {
                ApplicationGroup = applicationGroup,
                Status = status,
                ApplicationList = applicationLists,
                BreadCrumbViewModel = breadCrumbViewModel,
                IsAuthorized = true
            };
        }

        #endregion
    }
}