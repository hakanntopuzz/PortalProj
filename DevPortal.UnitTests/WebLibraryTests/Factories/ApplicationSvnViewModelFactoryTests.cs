using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Factories;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationSvnViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        ApplicationSvnViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new ApplicationSvnViewModelFactory(breadCrumbFactory.Object,
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

        #region application svn repositories

        [Test]
        public void CreateApplicationSvnViewModel_NoCondition_ReturnApplicationSvnViewModel()
        {
            // Arrange
            var applicationId = 1;
            var applicationName = "applicationName";
            var svnUrl = "svnUrl";
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var repositoryTypes = new List<SvnRepositoryType>();
            var viewModel = new ApplicationSvnViewModel
            {
                ApplicationSvn = new SvnRepository
                {
                    ApplicationId = applicationId,
                    SvnUrl = svnUrl,
                    ApplicationName = applicationName
                },
                SvnRepositoryTypeList = repositoryTypes,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateNewSvnRepositoryModel(applicationId)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateApplicationSvnViewModel(applicationId, applicationName, svnUrl, repositoryTypes);

            // Assert
            result.Should().BeEquivalentTo(viewModel);
            result.ApplicationSvn.Should().BeEquivalentTo(viewModel.ApplicationSvn);
            result.BreadCrumbViewModel.Should().BeSameAs(viewModel.BreadCrumbViewModel);
        }

        [Test]
        public void CreateApplicationSvnDetailViewModel_NoCondition_ReturnApplicationSvnViewModel()
        {
            // Arrange
            var svnRepository = new SvnRepository();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var viewModel = new ApplicationSvnViewModel
            {
                ApplicationSvn = svnRepository,
                BreadCrumbViewModel = breadcrumbViewModel,
                IsAuthorized = true,
                FavouritePageName = $"{svnRepository.ApplicationName} - SVN Deposu - {svnRepository.Name}"
            };

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateDetailSvnRepositoryModel(svnRepository.ApplicationId)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateApplicationSvnDetailViewModel(svnRepository);

            // Assert
            result.Should().BeEquivalentTo(viewModel);
            result.ApplicationSvn.Should().BeSameAs(viewModel.ApplicationSvn);
            result.IsAuthorized.Should().BeTrue();
            result.BreadCrumbViewModel.Should().BeSameAs(viewModel.BreadCrumbViewModel);
        }

        [Test]
        public void CreateApplicationSvnEditViewModel_NoCondition_ReturnApplicationSvnViewModel()
        {
            // Arrange
            var svnRepository = new SvnRepository();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var repositoryTypeList = new List<SvnRepositoryType>();
            var viewModel = new ApplicationSvnViewModel
            {
                ApplicationSvn = svnRepository,
                SvnRepositoryTypeList = repositoryTypeList,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateEditSvnRepositoryModel(svnRepository.ApplicationId, svnRepository.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateApplicationSvnEditViewModel(svnRepository, repositoryTypeList);

            // Assert
            result.Should().BeEquivalentTo(viewModel);
            result.ApplicationSvn.Should().BeSameAs(viewModel.ApplicationSvn);
            result.BreadCrumbViewModel.Should().BeSameAs(viewModel.BreadCrumbViewModel);
        }

        #endregion
    }
}