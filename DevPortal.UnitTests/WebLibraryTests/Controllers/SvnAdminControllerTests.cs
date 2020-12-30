using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.SvnAdmin.Business.Abstract;
using DevPortal.SvnAdmin.Model;
using DevPortal.UnitTests.TestHelpers;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SvnAdminControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<ISvnAdminService> svnAdminService;

        StrictMock<ISvnAdminViewModelFactory> viewModelFactory;

        SvnAdminController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            svnAdminService = new StrictMock<ISvnAdminService>();
            viewModelFactory = new StrictMock<ISvnAdminViewModelFactory>();

            controller = new SvnAdminController(
                userSessionService.Object,
                svnAdminService.Object,
                viewModelFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            svnAdminService.VerifyAll();
            viewModelFactory.VerifyAll();
        }

        #endregion

        #region helpers

        public void SetResultMessageTempData(Dictionary<string, string> tempDataKeyValuePairs)
        {
            controller.TempData = SetupHelpers.CreateResultMessageTempData(tempDataKeyValuePairs);
        }

        #endregion

        #region get svn repository list

        [Test]
        public void Index_ServiceResultFails_ReturnErrorView()
        {
            // Arrange
            SvnRepositoryListResult serviceResult = SvnRepositoryListResult.Error("message");

            svnAdminService.Setup(x => x.GetRepositoriesByLastUpdatedOrder()).Returns(serviceResult);

            // Act
            var result = controller.Index();

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Error).ModelAs<string>().Should().Be(serviceResult.Message);
        }

        [Test]
        public void Index_ServiceResultSuccess_ReturnView()
        {
            ICollection<SvnRepositoryFolderListItem> data = new List<SvnRepositoryFolderListItem>
            {
                new SvnRepositoryFolderListItem
                {
                    Name = "mahmut"
                }
            };

            // Arrange
            SvnRepositoryListResult serviceResult = SvnRepositoryListResult.Success(data);
            var viewModel = new SvnRepositoryListViewModel { Items = data };

            svnAdminService.Setup(x => x.GetRepositoriesByLastUpdatedOrder()).Returns(serviceResult);
            viewModelFactory.Setup(x => x.CreateSvnRepositoryListViewModel(data)).Returns(viewModel);

            // Act
            var result = controller.Index();

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Index).ModelAs<SvnRepositoryListViewModel>().Items.Should().HaveSameCount(data);
        }

        #endregion

        #region create svn repository

        [Test]
        public void CreateSvnRepositoryFolder_NoCondition_ReturnView()
        {
            // Arrange
            var model = new SvnRepositoryFolderViewModel();
            var folder = null as SvnRepositoryFolder;

            viewModelFactory.Setup(x => x.CreateSvnRepositoryFolderViewModel(folder)).Returns(model);

            // Act
            var result = controller.CreateSvnRepositoryFolder();

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Create)
                .ModelAs<SvnRepositoryFolderViewModel>().Should().Be(model);
        }

        [Test]
        public async Task CreateSvnRepositoryFolder_InvalidModel_ReturnErrorView()

        {
            // Arrange
            var folder = new SvnRepositoryFolder();
            var model = new SvnRepositoryFolderViewModel
            {
                Folder = folder
            };

            controller.ModelState.AddModelError("", "Invalid model");
            viewModelFactory.Setup(x => x.CreateSvnRepositoryFolderViewModel(folder)).Returns(model);

            // Act
            var result = await controller.CreateSvnRepositoryFolder(folder);

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Create)
                .ModelAs<SvnRepositoryFolderViewModel>().Should().Be(model);
        }

        [Test]
        public async Task CreateSvnRepositoryFolder_CreatingFolderFails_ReturnErrorView()
        {
            // Arrange
            var folder = new SvnRepositoryFolder();
            var model = new SvnRepositoryFolderViewModel
            {
                Folder = folder
            };
            const string errorMessage = "error";
            var serviceResult = ServiceResult.Error(errorMessage);
            var tempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, errorMessage);
            SetResultMessageTempData(tempData);

            svnAdminService.Setup(x => x.CreateSvnRepositoryFolderAsync(folder)).ReturnsAsync(serviceResult);
            viewModelFactory.Setup(x => x.CreateSvnRepositoryFolderViewModel(folder)).Returns(model);

            // Act
            var result = await controller.CreateSvnRepositoryFolder(folder);

            // Assert
            AssertHelpers.AssertResultMessageTempData(controller, tempData);
            result.Should().BeViewResult().WithViewName(ViewNames.Create)
                .ModelAs<SvnRepositoryFolderViewModel>().Should().Be(model);
        }

        [Test]
        public async Task CreateSvnRepositoryFolder_FolderCreated_RedirectToIndexWithSuccessMessage()
        {
            // Arrange
            var folder = new SvnRepositoryFolder();
            var model = new SvnRepositoryFolderViewModel
            {
                Folder = folder
            };
            var serviceResult = ServiceResult.Success(Messages.SvnRepositoryFolderCreated);
            var tempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(tempData);

            svnAdminService.Setup(x => x.CreateSvnRepositoryFolderAsync(folder)).ReturnsAsync(serviceResult);

            // Act
            var result = await controller.CreateSvnRepositoryFolder(folder);

            // Assert
            AssertHelpers.AssertResultMessageTempData(controller, tempData);
            result.Should().BeRedirectToActionResult().WithActionName(SvnAdminControllerActionNames.Index);
        }

        #endregion
    }
}