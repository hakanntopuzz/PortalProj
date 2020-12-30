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
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationWriterService> applicationWriterService;

        StrictMock<IApplicationReaderService> applicationReaderService;

        StrictMock<IApplicationViewModelFactory> viewModelFactory;

        StrictMock<IUrlGeneratorService> urlHelper;

        ApplicationController controller;

        [SetUp]
        public void Initialize()
        {
            applicationWriterService = new StrictMock<IApplicationWriterService>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();
            viewModelFactory = new StrictMock<IApplicationViewModelFactory>();
            urlHelper = new StrictMock<IUrlGeneratorService>();
            userSessionService = new StrictMock<IUserSessionService>();

            controller = new ApplicationController(
                userSessionService.Object,
                applicationWriterService.Object,
                applicationReaderService.Object,
                viewModelFactory.Object,
                urlHelper.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationWriterService.VerifyAll();
            viewModelFactory.VerifyAll();
            urlHelper.VerifyAll();
            userSessionService.VerifyAll();
        }

        #endregion

        #region helpers

        public void SetResultMessageTempData(Dictionary<string, string> tempDataKeyValuePairs)
        {
            controller.TempData = SetupHelpers.CreateResultMessageTempData(tempDataKeyValuePairs);
        }

        #endregion

        #region get applications

        [Test]
        public void Index_ApplicationsViewModelIsNull_ReturnApplicationsViewModel()
        {
            // Arrange
            var applicationGroups = new List<ApplicationGroup>();
            var applications = new List<ApplicationListItem>();
            var viewModel = new ApplicationsViewModel();

            applicationReaderService.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);
            applicationReaderService.Setup(x => x.GetApplications()).Returns(applications);

            viewModelFactory.Setup(x => x.CreateApplicationsViewModel(applicationGroups, applications)).Returns(viewModel);

            // Act
            var result = controller.Index();

            // Assert
            result.Should().BeViewResult(ViewNames.Index).ModelAs<ApplicationsViewModel>().Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public async Task Index_NoCondition_ReturnApplicationsListModel()
        {
            // Arrange
            var applicationListModel = new ApplicationListModel();
            var application = new List<Application>();
            var tableParam = new ApplicationTableParam();

            applicationReaderService.Setup(x => x.GetFilteredApplicationListAsync(tableParam)).ReturnsAsync(application);
            viewModelFactory.Setup(x => x.CreateApplicationListModel(application)).Returns(applicationListModel);

            // Act
            var result = await controller.Index(tableParam);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.As<ApplicationListModel>().Should().Be(applicationListModel);
        }

        #endregion

        #region application

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void GetApplication_InvalidIdParameterOnTestsCases_ReturnErrorMessageAndRedirectToApplicationList(int id)
        {
            // Arrange
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Detail(id);

            // Assert
            AssertHelpers.AssertRedirectToAction(result, ApplicationControllerActionNames.Index, ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
            applicationReaderService.Verify(x => x.GetApplication(id), Times.Never);
        }

        [Test]
        public void GetApplication_ApplicationExists_ReturnApplication()
        {
            // Arrange
            var id = 1;
            var application = GetApplication();
            var viewModel = new ApplicationViewModel();
            var databases = new List<Database>();

            applicationReaderService.Setup(x => x.GetApplication(id)).Returns(application);
            viewModelFactory.Setup(x => x.CreateApplication(application)).Returns(viewModel);

            // Act
            var result = controller.Detail(id);

            // Assert
            result.Should().BeViewResult(ViewNames.Detail).ModelAs<ApplicationViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void GetApplication_ApplicationDoesNotExist_ReturnErrorMessageAndRedirectToApplicationList()
        {
            // Arrange
            var id = 1;
            Application application = null;
            var database = new List<Database>();

            applicationReaderService.Setup(x => x.GetApplication(id)).Returns(application);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Detail(id);

            // Assert
            AssertHelpers.AssertRedirectToAction(result, ApplicationControllerActionNames.Index, ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region edit application

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Edit_InvalidIdParameterOnTestsCases_ReturnErrorMessageAndRedirectToApplicationList(int id)
        {
            // Arrange
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Edit(id);

            // Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
            applicationReaderService.Verify(x => x.GetApplication(id), Times.Never);
        }

        [Test]
        public void Edit_ApplicationNotFound_ReturnBadRequest()
        {
            // Arrange
            var id = 1;

            Application application = null;

            applicationReaderService.Setup(x => x.GetApplication(id)).Returns(application);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Edit(id);

            // Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_ApplicationFound_CreateEditViewModelAndReturnEditView()
        {
            // Arrange
            var id = 1;
            var application = GetApplication();
            var applicationTypes = GetApplicationTypes();
            var applicationGroups = GetApplicationGroups();
            var applicationStatusList = GetApplicationStatusList();

            var viewModel = new EditApplicationViewModel();

            applicationReaderService.Setup(x => x.GetApplication(id)).Returns(application);
            applicationReaderService.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);
            applicationReaderService.Setup(x => x.GetApplicationTypes()).Returns(applicationTypes);
            applicationReaderService.Setup(x => x.GetApplicationStatusList()).Returns(applicationStatusList);
            viewModelFactory.Setup(x => x.CreateEditApplication(application, applicationGroups, applicationTypes, applicationStatusList)).Returns(viewModel);

            // Act
            var result = controller.Edit(id);

            // Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<EditApplicationViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Edit_ModelStateIsNotValid_ReturnEditView()
        {
            // Arrange
            var application = GetApplication();
            var applicationGroups = GetApplicationGroups();
            var applicationTypes = GetApplicationTypes();
            var applicationStatusList = GetApplicationStatusList();
            var viewModel = new EditApplicationViewModel
            {
                Application = application
            };

            applicationReaderService.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);
            applicationReaderService.Setup(x => x.GetApplicationTypes()).Returns(applicationTypes);
            applicationReaderService.Setup(x => x.GetApplicationStatusList()).Returns(applicationStatusList);
            viewModelFactory.Setup(x => x.CreateEditApplication(application, applicationGroups, applicationTypes, applicationStatusList)).Returns(viewModel);

            controller.ModelState.AddModelError("", "invalid model");

            // Act
            var result = controller.Edit(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Edit)
                .ModelAs<EditApplicationViewModel>()
                .Should().Be(viewModel);

            applicationWriterService.Verify(x => x.UpdateApplication(VerifyAny<Application>()), Times.Never);
        }

        [Test]
        public void Edit_ServiceReturnsError_SetErrorMessageToTempDataAndReturnEditView()
        {
            // Arrange
            var application = GetApplication();
            var applicationGroups = GetApplicationGroups();
            var applicationTypes = GetApplicationTypes();
            var applicationStatusList = GetApplicationStatusList();
            var viewModel = new EditApplicationViewModel
            {
                Application = application
            };

            var serviceResult = ServiceResult.Error(Messages.UpdateFails);
            var userId = 3;

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationWriterService.Setup(x => x.UpdateApplication(viewModel.Application)).Returns(serviceResult);
            applicationReaderService.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);
            applicationReaderService.Setup(x => x.GetApplicationTypes()).Returns(applicationTypes);
            applicationReaderService.Setup(x => x.GetApplicationStatusList()).Returns(applicationStatusList);
            viewModelFactory.Setup(x => x.CreateEditApplication(application, applicationGroups, applicationTypes, applicationStatusList)).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Edit(viewModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Edit)
                .ModelAs<EditApplicationViewModel>()
                .Should().Be(viewModel);

            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Edit_ServiceReturnsSuccess_SetSuccessMessageToTempDataAndReturnIndexView()
        {
            // Arrange
            var viewModel = new EditApplicationViewModel
            {
                Application = GetApplication()
            };

            var serviceResult = ServiceResult.Success(Messages.UpdateSucceeds);
            var userId = 3;

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationWriterService.Setup(x => x.UpdateApplication(viewModel.Application)).Returns(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, Messages.UpdateSucceeds);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Edit(viewModel);

            // Assert
            result.Should().BeRedirectToActionResult()
                .WithActionName(ApplicationControllerActionNames.Detail)
                .WithRouteValue("id", viewModel.Application.Id);

            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        #endregion

        #region add application

        [Test]
        public void Add_NoCondition_CreateAddViewModelAndReturnAddView()
        {
            // Arrange
            var applicationGroups = GetApplicationGroups();
            var applicationTypes = GetApplicationTypes();
            var applicationStatus = GetApplicationStatusList();
            var viewModel = new AddApplicationViewModel();

            applicationReaderService.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);
            applicationReaderService.Setup(x => x.GetApplicationTypes()).Returns(applicationTypes);
            applicationReaderService.Setup(x => x.GetApplicationStatusList()).Returns(applicationStatus);
            viewModelFactory.Setup(x => x.CreateApplicationAddViewModel(applicationGroups, applicationTypes, applicationStatus)).Returns(viewModel);

            // Act
            var result = controller.Add();

            // Assert
            result.Should().BeViewResult(ViewNames.Add)
                .ModelAs<AddApplicationViewModel>()
                .Should().Be(viewModel);
        }

        [Test]
        public void Add_ModelStateIsNotValid_ReturnAddView()
        {
            // Arrange
            var application = GetApplication();
            var applicationGroups = GetApplicationGroups();
            var applicationTypes = GetApplicationTypes();
            var applicationStatus = GetApplicationStatusList();
            var viewModel = new AddApplicationViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            applicationReaderService.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);
            applicationReaderService.Setup(x => x.GetApplicationTypes()).Returns(applicationTypes);
            applicationReaderService.Setup(x => x.GetApplicationStatusList()).Returns(applicationStatus);
            viewModelFactory.Setup(x => x.CreateApplicationAddViewModel(applicationGroups, applicationTypes, applicationStatus)).Returns(viewModel);

            // Act
            var result = controller.Add(application);

            // Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<AddApplicationViewModel>().Should().Be(viewModel);
            applicationWriterService.Verify(x => x.AddApplication(application), Times.Never);
        }

        [Test]
        public void Add_AddingFails_SetErrorMessageToTempDataAndReturnAddView()
        {
            // Arrange
            var application = GetApplication();
            var applicationGroups = GetApplicationGroups();
            var applicationTypes = GetApplicationTypes();
            var applicationStatus = GetApplicationStatusList();
            var viewModel = new AddApplicationViewModel();
            int userId = 45;
            var addResult = Int32ServiceResult.Error("Error");

            applicationReaderService.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);
            applicationReaderService.Setup(x => x.GetApplicationTypes()).Returns(applicationTypes);
            applicationReaderService.Setup(x => x.GetApplicationStatusList()).Returns(applicationStatus);
            viewModelFactory.Setup(x => x.CreateApplicationAddViewModel(applicationGroups, applicationTypes, applicationStatus)).Returns(viewModel);
            applicationWriterService.Setup(x => x.AddApplication(application)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Add(application);

            // Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<AddApplicationViewModel>().Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Add_ServiceReturnsSuccess_SetSuccessMessageToTempDataAndReturnDetailView()
        {
            // Arrange
            var application = new Application
            {
                Id = 1058
            };
            int userId = 45;
            var addResult = Int32ServiceResult.Success(application.Id);

            applicationWriterService.Setup(x => x.AddApplication(application)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Add(application);

            // Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationControllerActionNames.Detail).WithRouteValue("id", application.Id);

            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        #endregion

        #region delete

        [Test]
        public void Delete_Error_ReturnErrorResult()
        {
            //Arrange
            var id = 1;
            var redirectUrl = $"/application/index";
            var deleteResult = ServiceResult.Error();

            applicationWriterService.Setup(x => x.DeleteApplication(id)).Returns(deleteResult);
            urlHelper.Setup(x => x.GenerateUrl(ApplicationControllerActionNames.Index, ControllerNames.Application)).Returns(redirectUrl);

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
            var redirectUrl = $"/application/index";
            var deleteResult = ServiceResult.Success();

            applicationWriterService.Setup(x => x.DeleteApplication(id)).Returns(deleteResult);
            urlHelper.Setup(x => x.GenerateUrl(ApplicationControllerActionNames.Index, ControllerNames.Application)).Returns(redirectUrl);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, Messages.ApplicationDeleted);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Delete(id);

            //Assert
            var expectedResult = RedirectableClientActionResult.Success(redirectUrl);
            AssertHelpers.AssertRedirectableClientActionResult(result, expectedResult);
        }

        #endregion

        #region setup helper

        static Application GetApplication()
        {
            return new Application();
        }

        static List<ApplicationGroup> GetApplicationGroups()
        {
            return new List<ApplicationGroup>();
        }

        static List<ApplicationType> GetApplicationTypes()
        {
            return new List<ApplicationType>();
        }

        static List<ApplicationStatus> GetApplicationStatusList()
        {
            return new List<ApplicationStatus>();
        }

        #endregion
    }
}