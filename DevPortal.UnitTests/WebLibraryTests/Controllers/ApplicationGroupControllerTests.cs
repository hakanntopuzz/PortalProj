using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.UnitTests.TestHelpers;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationGroupControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationGroupService> applicationGroupService;

        StrictMock<IApplicationGroupViewModelFactory> applicationGroupViewModelFactory;

        StrictMock<IUrlGeneratorService> urlHelper;

        StrictMock<IApplicationReaderService> applicationReaderService;

        ApplicationGroupController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            applicationGroupService = new StrictMock<IApplicationGroupService>();
            applicationGroupViewModelFactory = new StrictMock<IApplicationGroupViewModelFactory>();
            urlHelper = new StrictMock<IUrlGeneratorService>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();

            controller = new ApplicationGroupController(
                userSessionService.Object,
                applicationGroupService.Object,
                applicationGroupViewModelFactory.Object,
                urlHelper.Object,
                applicationReaderService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationGroupService.VerifyAll();
            applicationGroupViewModelFactory.VerifyAll();
            urlHelper.VerifyAll();
            applicationReaderService.VerifyAll();
            userSessionService.VerifyAll();
        }

        #endregion

        #region set result message temp data

        public void SetResultMessageTempData(Dictionary<string, string> tempDataKeyValuePairs)
        {
            controller.TempData = SetupHelpers.CreateResultMessageTempData(tempDataKeyValuePairs);
        }

        #endregion

        #region index

        [Test]
        public void Index_ApplicationGroupsViewModelIsNull_ReturnViewNamesAndApplicationGroupsViewModel()
        {
            // Arrange
            var applicationGroups = new List<ApplicationGroup>();
            var viewModel = new ApplicationGroupViewModel();

            applicationGroupService.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);

            applicationGroupViewModelFactory.Setup(x => x.CreateApplicationGroupsViewModel(applicationGroups)).Returns(viewModel);

            // Act
            var result = controller.Index();

            // Assert
            result.Should().BeViewResult(ViewNames.Index).ModelAs<ApplicationGroupViewModel>().Should().BeEquivalentTo(viewModel);
        }

        #endregion

        #region detail

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Detail_InvalidIdParameterOnTestsCases_RedirectToGroupListAndSetErrorMessageTempData(int id)
        {
            // Arrange
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationGroupNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Detail(id);

            // Assert
            result.Should().BeRedirectToActionResult()
                .WithActionName(ApplicationGroupControllerActionNames.Index)
                .WithControllerName(ControllerNames.ApplicationGroup);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
            applicationGroupService.Verify(x => x.GetApplicationGroupById(id), Times.Never);
        }

        [Test]
        public void Detail_ApplicationGroupNotFound_RedirectToGroupListAndSetErrorMessageTempData()
        {
            // Arrange
            var id = 86;

            ApplicationGroup applicationGroup = null;

            applicationGroupService.Setup(x => x.GetApplicationGroupById(id)).Returns(applicationGroup);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationGroupNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Detail(id);

            // Assert
            result.Should().BeRedirectToActionResult()
                .WithActionName(ApplicationGroupControllerActionNames.Index)
                .WithControllerName(ControllerNames.ApplicationGroup);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_ApplicationGroupExists_ReturnApplicationGroupDetail()
        {
            // Arrange
            var id = 1;
            var applicationGroup = new ApplicationGroup();
            var applicationList = new List<ApplicationListItem>();
            var viewModel = new ApplicationGroupViewModel();

            applicationGroupService.Setup(x => x.GetApplicationGroupById(id)).Returns(applicationGroup);
            applicationReaderService.Setup(x => x.GetApplicationsByGroupId(id)).Returns(applicationList);
            applicationGroupViewModelFactory.Setup(x => x.CreateDetailApplicationGroup(applicationGroup, applicationList)).Returns(viewModel);

            // Act
            var result = controller.Detail(id);

            // Assert
            result.Should().BeViewResult(ViewNames.Detail)
                .ModelAs<ApplicationGroupViewModel>()
                .Should().Be(viewModel);
        }

        #endregion

        #region edit - get

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Edit_InvalidIdParameterOnTestsCases_RedirectToGroupListAndSetErrorMessageTempData(int id)
        {
            // Arrange
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationGroupNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Edit(id);

            // Assert
            result.Should().BeRedirectToActionResult()
                .WithActionName(ApplicationGroupControllerActionNames.Index)
                .WithControllerName(ControllerNames.ApplicationGroup);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
            applicationGroupService.Verify(x => x.GetApplicationGroupById(id), Times.Never);
            applicationGroupService.Verify(x => x.UpdateApplicationGroup(VerifyAny<ApplicationGroup>()), Times.Never);
        }

        [Test]
        public void Edit_ApplicationGroupNotFound_RedirectToGroupListAndSetErrorMessageTempData()
        {
            // Arrange
            var id = 86;
            ApplicationGroup applicationGroup = null;

            applicationGroupService.Setup(x => x.GetApplicationGroupById(id)).Returns(applicationGroup);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationGroupNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Edit(id);

            // Assert
            result.Should().BeRedirectToActionResult()
                .WithActionName(ApplicationGroupControllerActionNames.Index)
                .WithControllerName(ControllerNames.ApplicationGroup);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);

            applicationGroupService.Verify(x => x.UpdateApplicationGroup(VerifyAny<ApplicationGroup>()), Times.Never);
        }

        [Test]
        public void Edit_ApplicationGroupFound_CreateEditViewModelAndReturnEditView()
        {
            // Arrange
            var id = 1;

            var appGroup = new ApplicationGroup();
            var status = new List<ApplicationGroupStatus>();
            var viewModel = new ApplicationGroupViewModel();

            applicationGroupService.Setup(x => x.GetApplicationGroupById(id)).Returns(appGroup);
            applicationGroupService.Setup(x => x.GetApplicationGroupStatusList()).Returns(status);
            applicationGroupViewModelFactory.Setup(x => x.CreateEditApplicationGroup(appGroup, status)).Returns(viewModel);

            // Act
            var result = controller.Edit(id);

            // Assert
            result.Should().BeViewResult(ViewNames.Edit)
                .ModelAs<ApplicationGroupViewModel>()
                .Should().Be(viewModel);
        }

        #endregion

        #region edit - post

        [Test]
        public void Edit_InvalidModel_ReturnEditView()
        {
            // Arrange
            var viewModel = new ApplicationGroupViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            var statusList = new List<ApplicationGroupStatus>();

            applicationGroupService.Setup(x => x.GetApplicationGroupStatusList()).Returns(statusList);

            // Act
            var result = controller.Edit(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Edit)
                .ModelAs<ApplicationGroupViewModel>()
                .Should().Be(viewModel);

            applicationGroupService.Verify(x => x.UpdateApplicationGroup(VerifyAny<ApplicationGroup>()), Times.Never);
        }

        [Test]
        public void Edit_UpdateFails_SetErrorMessageToTempDataAndReturnEditView()
        {
            // Arrange
            var applicationGroup = new ApplicationGroup();
            var viewModel = new ApplicationGroupViewModel
            {
                ApplicationGroup = applicationGroup
            };
            var userId = 5;
            var updateResult = ServiceResult.Error(Messages.UpdateFails);
            var statusList = new List<ApplicationGroupStatus>();

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationGroupService.Setup(x => x.UpdateApplicationGroup(viewModel.ApplicationGroup)).Returns(updateResult);
            applicationGroupService.Setup(x => x.GetApplicationGroupStatusList()).Returns(statusList);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.UpdateFails);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Edit(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Edit)
                .ModelAs<ApplicationGroupViewModel>()
                .Should().Be(viewModel);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_UpdateSucceeds_SetSuccessMessageToTempDataAndReturnIndexView()
        {
            // Arrange
            var viewModel = new ApplicationGroupViewModel
            {
                ApplicationGroup = new ApplicationGroup()
            };
            var userId = 5;
            var updateResult = ServiceResult.Success("success");

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationGroupService.Setup(x => x.UpdateApplicationGroup(viewModel.ApplicationGroup)).Returns(updateResult);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, updateResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Edit(viewModel);

            // Assert
            result.Should().BeRedirectToActionResult()
                .WithActionName(ApplicationGroupControllerActionNames.Detail)
                .WithRouteValue("id", viewModel.ApplicationGroup.Id);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region add - get

        [Test]
        public void Add_NoCondition_ReturnViewNamesAndApplicationGroupViewModel()
        {
            // Arrange
            var applicationGroupViewModel = new ApplicationGroupViewModel();
            var applicationGroupStatuses = new List<ApplicationGroupStatus>();

            applicationGroupService.Setup(x => x.GetApplicationGroupStatusList()).Returns(applicationGroupStatuses);
            applicationGroupViewModelFactory.Setup(x => x.CreateApplicationGroupAddViewModel(applicationGroupStatuses)).Returns(applicationGroupViewModel);

            // Act
            var result = controller.Add();

            // Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<ApplicationGroupViewModel>().Should().Be(applicationGroupViewModel);
        }

        #endregion

        #region add - post

        [Test]
        public void Add_InvalidModel_ReturnViewNamesAndApplicationGroupViewModel()
        {
            // Arrange
            var applicationGroup = new ApplicationGroup();
            var applicationGroupViewModel = new ApplicationGroupViewModel();
            var applicationGroupStatuses = new List<ApplicationGroupStatus>();

            controller.ModelState.AddModelError("", "invalid model");

            applicationGroupService.Setup(x => x.GetApplicationGroupStatusList()).Returns(applicationGroupStatuses);
            applicationGroupViewModelFactory.Setup(x => x.CreateApplicationGroupAddViewModel(applicationGroupStatuses)).Returns(applicationGroupViewModel);

            // Act
            var result = controller.Add(applicationGroup);

            // Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<ApplicationGroupViewModel>().Should().Be(applicationGroupViewModel);
        }

        [Test]
        public void Add_AddFails_ReturnViewNamesAndApplicationGroupViewModel()
        {
            // Arrange
            var applicationGroup = new ApplicationGroup();
            var applicationGroupViewModel = new ApplicationGroupViewModel();
            var applicationGroupStatuses = new List<ApplicationGroupStatus>();
            int userId = 45;
            var addResult = Int32ServiceResult.Error("error");

            applicationGroupService.Setup(x => x.AddApplicationGroup(applicationGroup)).Returns(addResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            applicationGroupService.Setup(x => x.GetApplicationGroupStatusList()).Returns(applicationGroupStatuses);
            applicationGroupViewModelFactory.Setup(x => x.CreateApplicationGroupAddViewModel(applicationGroupStatuses)).Returns(applicationGroupViewModel);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);

            // Act
            var result = controller.Add(applicationGroup);

            // Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<ApplicationGroupViewModel>().Should().Be(applicationGroupViewModel);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Add_AddSucceeds_ReturnViewNamesAndApplicationGroupViewModel()
        {
            // Arrange
            var applicationGroup = new ApplicationGroup
            {
                Id = 10
            };

            int userId = 45;
            var addResult = Int32ServiceResult.Success(applicationGroup.Id);

            applicationGroupService.Setup(x => x.AddApplicationGroup(applicationGroup)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Add(applicationGroup);

            // Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationGroupControllerActionNames.Detail).WithRouteValue("id", applicationGroup.Id);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region delete

        [Test]
        public void Delete_Error_ReturnErrorResult()
        {
            //Arrange
            var id = 1;
            var redirectUrl = $"/applicationgroup/index";
            var deleteResult = ServiceResult.Error();

            applicationGroupService.Setup(x => x.DeleteApplicationGroup(id)).Returns(deleteResult);
            urlHelper.Setup(x => x.GenerateUrl(ApplicationGroupControllerActionNames.Index, ControllerNames.ApplicationGroup)).Returns(redirectUrl);

            //Act
            var result = controller.Delete(id);

            //Assert
            var expectedResult = RedirectableClientActionResult.Error(redirectUrl);
            AssertHelpers.AssertRedirectableClientActionResult(result, expectedResult);
        }

        [Test]
        public void Delete_Success_ReturnSuccessAndRedirectUrl()
        {
            //Arrange
            var id = 1;
            var redirectUrl = $"/applicationgroup/index";
            var deleteResult = ServiceResult.Success();

            applicationGroupService.Setup(x => x.DeleteApplicationGroup(id)).Returns(deleteResult);
            urlHelper.Setup(x => x.GenerateUrl(ApplicationGroupControllerActionNames.Index, ControllerNames.ApplicationGroup)).Returns(redirectUrl);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, Messages.ApplicationDeleted);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Delete(id);

            //Assert
            var expectedResult = RedirectableClientActionResult.Success(redirectUrl);
            AssertHelpers.AssertRedirectableClientActionResult(result, expectedResult);
        }

        #endregion
    }
}