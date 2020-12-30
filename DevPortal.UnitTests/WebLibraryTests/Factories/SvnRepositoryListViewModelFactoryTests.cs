using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.SvnAdmin.Model;
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
    public class SvnRepositoryListViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        SvnAdminViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new SvnAdminViewModelFactory(breadCrumbFactory.Object,
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

        #region svn repository list

        [Test]
        public void CreateSvnRepositoryListViewModel_NoCondition_ReturnSvnRepositoryListViewModel()
        {
            // Arrange
            var svnRepositoryListItems = new List<SvnRepositoryFolderListItem>();
            var breadCrumbViewModel = new BreadCrumbViewModel();

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateSvnAdminListModel()).Returns(breadCrumbViewModel);

            // Act
            var result = factory.CreateSvnRepositoryListViewModel(svnRepositoryListItems);

            // Assert
            result.Items.Should().BeEquivalentTo(svnRepositoryListItems);
            result.IsAuthorized.Should().BeTrue();
            result.BreadCrumbViewModel.Should().BeSameAs(breadCrumbViewModel);
        }

        #endregion

        #region svn repository folder

        [Test]
        public void CreateSvnRepositoryFolderViewModel_NoCondition_ReturnSvnRepositoryFolderViewModel()
        {
            // Arrange
            var svnRepositoryListItems = new List<SvnRepositoryFolderListItem>();
            var breadCrumbViewModel = new BreadCrumbViewModel();
            var folder = new SvnRepositoryFolder();

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminDeveloperPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateSvnRepositoryFolderModel()).Returns(breadCrumbViewModel);

            // Act
            var result = factory.CreateSvnRepositoryFolderViewModel(folder);

            // Assert
            result.Folder.Should().BeEquivalentTo(folder);
            result.IsAuthorized.Should().BeTrue();
            result.BreadCrumbViewModel.Should().BeSameAs(breadCrumbViewModel);
        }

        #endregion
    }
}