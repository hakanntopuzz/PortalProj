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
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationEnvironmentControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationEnvironmentService> applicationEnvironmentService;

        StrictMock<IApplicationReaderService> applicationReaderService;

        StrictMock<IApplicationEnvironmentViewModelFactory> viewModelFactory;

        StrictMock<IUrlGeneratorService> urlHelper;

        StrictMock<IRouteValueFactory> routeValueFactory;

        StrictMock<IEnvironmentService> environmentService;

        ApplicationEnvironmentController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();
            applicationEnvironmentService = new StrictMock<IApplicationEnvironmentService>();
            viewModelFactory = new StrictMock<IApplicationEnvironmentViewModelFactory>();
            urlHelper = new StrictMock<IUrlGeneratorService>();
            routeValueFactory = new StrictMock<IRouteValueFactory>();
            environmentService = new StrictMock<IEnvironmentService>();

            controller = new ApplicationEnvironmentController(
                userSessionService.Object,
                applicationEnvironmentService.Object,
                applicationReaderService.Object,
                viewModelFactory.Object,
                urlHelper.Object,
                routeValueFactory.Object,
                environmentService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationEnvironmentService.VerifyAll();
            applicationReaderService.VerifyAll();
            viewModelFactory.VerifyAll();
            urlHelper.VerifyAll();
            routeValueFactory.VerifyAll();
            environmentService.VerifyAll();
            userSessionService.VerifyAll();
        }

        #endregion

        #region index

        [Test]
        public void Index_EnvironmentsExistTrue_ReturnClientResult()
        {
            //Arrange
            var applicationId = 1;
            ICollection<ApplicationEnvironment> environments = null;

            applicationEnvironmentService.Setup(x => x.GetApplicationEnvironments(applicationId)).Returns(environments);

            //Act
            var result = controller.Index(applicationId);

            //Assert
            var expectedResult = ClientDataResult.Error(environments);
            AssertHelpers.AssertClientDataResult(result, expectedResult);
        }

        [Test]
        public void Index_EnvironmentsExistFalse_ReturnClientResult()
        {
            //Arrange
            var applicationId = 1;
            var environments = new List<ApplicationEnvironment>();

            applicationEnvironmentService.Setup(x => x.GetApplicationEnvironments(applicationId)).Returns(environments);

            //Act
            var result = controller.Index(applicationId);

            //Assert
            var expectedResult = ClientDataResult.Success(environments);
            AssertHelpers.AssertClientDataResult(result, expectedResult);
        }

        #endregion

        #region add

        [Test]
        public void Add_ApplicationExists_ReturnView()
        {
            //Arrange
            var applicationId = 1;
            var application = new Application();
            var environments = new List<Environment>();
            var applicationEnvironment = CreateApplicationEnvironment(applicationId, application.Name);
            var model = CreateApplicationEnvironmentViewModel(applicationEnvironment);
            var viewModel = new ApplicationEnvironmentViewModel();

            applicationReaderService.Setup(x => x.GetApplication(applicationId)).Returns(application);
            viewModelFactory.Setup(x => x.CreateApplicationEnvironment(applicationId, application.Name)).Returns(applicationEnvironment);
            viewModelFactory.Setup(x => x.CreateApplicationEnvironmentViewModel(applicationEnvironment)).Returns(model);
            applicationEnvironmentService.Setup(x => x.GetEnvironmentsDoesNotExist(model.ApplicationEnvironment.ApplicationId)).Returns(environments);
            viewModelFactory.Setup(x => x.CreateApplicationEnvironmentViewModel(model, environments)).Returns(viewModel);

            //Act
            var result = controller.Add(applicationId);

            //Assert
            AssertHelpers.AssertViewResult(viewModel, ViewNames.Add, result);
        }

        [Test]
        public void Add_ApplicationDoesNotExist_ReturnErrorMessageAndRedirectToApplicationList()
        {
            // Arrange
            var id = 1;
            Application application = null;

            applicationReaderService.Setup(x => x.GetApplication(id)).Returns(application);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Add(id);

            // Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Add_InvalidModel_ReturnView()
        {
            //Arrange
            var applicationId = 1;
            var application = new Application();
            var environments = new List<Environment>();
            var applicationEnvironment = CreateApplicationEnvironment(applicationId, application.Name);
            var viewModel = CreateApplicationEnvironmentViewModel(applicationEnvironment);

            controller.ModelState.AddModelError("", "invalid model");

            applicationEnvironmentService.Setup(x => x.GetEnvironmentsDoesNotExist(viewModel.ApplicationEnvironment.ApplicationId)).Returns(environments);
            viewModelFactory.Setup(x => x.CreateApplicationEnvironmentViewModel(viewModel, environments)).Returns(viewModel);

            //Act
            var result = controller.Add(viewModel);

            //Assert
            AssertHelpers.AssertViewResult(viewModel, ViewNames.Add, result);
        }

        [Test]
        public void Add_AddingFails_ReturnView()
        {
            //Arrange
            var applicationId = 1;
            var application = new Application();
            var environments = new List<Environment>();
            var applicationEnvironment = CreateApplicationEnvironment(applicationId, application.Name);

            var viewModel = CreateApplicationEnvironmentViewModel(applicationEnvironment);
            int userId = 45;
            var updateResult = ServiceResult.Error();

            applicationEnvironmentService.Setup(x => x.GetEnvironmentsDoesNotExist(viewModel.ApplicationEnvironment.ApplicationId)).Returns(environments);
            viewModelFactory.Setup(x => x.CreateApplicationEnvironmentViewModel(viewModel, environments)).Returns(viewModel);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationEnvironmentService.Setup(x => x.AddApplicationEnvironment(viewModel.ApplicationEnvironment)).Returns(updateResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, updateResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(viewModel);

            //Assert
            AssertHelpers.AssertViewResult(viewModel, ViewNames.Add, result);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Add_AddingSucceeds_RedirectToApplicationAndSetSuccessResultTempData()
        {
            //Arrange
            var applicationId = 1;
            var application = new Application();
            var applicationEnvironment = CreateApplicationEnvironment(applicationId, application.Name);
            var viewModel = CreateApplicationEnvironmentViewModel(applicationEnvironment);
            int userId = 45;

            var addResult = ServiceResult.Success();

            viewModelFactory.Setup(x => x.CreateApplicationEnvironment(viewModel.ApplicationEnvironment)).Returns(applicationEnvironment);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationEnvironmentService.Setup(x => x.AddApplicationEnvironment(viewModel.ApplicationEnvironment)).Returns(addResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(viewModel);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Detail).WithControllerName(ControllerNames.Application).WithRouteValue("id", applicationEnvironment.ApplicationId);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region edit

        [Test]
        public void Edit_NoCondition_ReturnView()
        {
            //Arrange
            var applicationId = 1;
            var applicationEnvironment = new ApplicationEnvironment();
            var viewModel = new ApplicationEnvironmentViewModel();
            var environments = SetupGetEnvironments();

            applicationEnvironmentService.Setup(x => x.GetApplicationEnvironment(applicationId)).Returns(applicationEnvironment);
            viewModelFactory.Setup(x => x.CreateApplicationEnvironmentViewModel(applicationEnvironment)).Returns(viewModel);
            environmentService.Setup(x => x.GetEnvironments()).Returns(environments);
            viewModelFactory.Setup(x => x.CreateEditApplicationEnvironmentViewModel(viewModel.ApplicationEnvironment, environments)).Returns(viewModel);

            //Act
            var result = controller.Edit(applicationId);

            //Assert
            AssertHelpers.AssertViewResult(viewModel, ViewNames.Edit, result);
        }

        [Test]
        public void Edit_EnvironmentNotFound_RedirectToApplicationListAndSetErrorMessageTempData()
        {
            //Arrange
            var id = 0;

            ApplicationEnvironment applicationEnvironment = null;

            applicationEnvironmentService.Setup(x => x.GetApplicationEnvironment(id)).Returns(applicationEnvironment);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationEnvironmentNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(id);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_InvalidModel_ReturnView()
        {
            //Arrange
            var applicationId = 1;
            var application = new Application();
            var applicationEnvironment = CreateApplicationEnvironment(applicationId, application.Name);
            var viewModel = CreateApplicationEnvironmentViewModel(applicationEnvironment);

            controller.ModelState.AddModelError("", "invalid model");

            var environments = SetupGetEnvironments();
            environmentService.Setup(x => x.GetEnvironments()).Returns(environments);
            viewModelFactory.Setup(x => x.CreateEditApplicationEnvironmentViewModel(viewModel.ApplicationEnvironment, environments)).Returns(viewModel);

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            AssertHelpers.AssertViewResult(viewModel, ViewNames.Edit, result);
        }

        [Test]
        public void Edit_UpdateFails_ReturnView()
        {
            //Arrange
            var applicationId = 1;
            var application = new Application();
            var applicationEnvironment = CreateApplicationEnvironment(applicationId, application.Name);
            var viewModel = CreateApplicationEnvironmentViewModel(applicationEnvironment);
            int userId = 45;
            var updateResult = ServiceResult.Error("error");

            var environments = SetupGetEnvironments();
            viewModelFactory.Setup(x => x.CreateEditApplicationEnvironmentViewModel(viewModel.ApplicationEnvironment, environments)).Returns(viewModel);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationEnvironmentService.Setup(x => x.UpdateApplicationEnvironment(viewModel.ApplicationEnvironment)).Returns(updateResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, updateResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            AssertHelpers.AssertViewResult(viewModel, ViewNames.Edit, result);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_UpdateSucceeds_RedirectToApplicationAndSetSuccessResultTempData()
        {
            //Arrange
            var applicationId = 1;
            var application = new Application();
            var applicationEnvironment = CreateApplicationEnvironment(applicationId, application.Name);
            var viewModel = CreateApplicationEnvironmentViewModel(applicationEnvironment);
            int userId = 45;
            var updateResult = ServiceResult.Success();

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationEnvironmentService.Setup(x => x.UpdateApplicationEnvironment(viewModel.ApplicationEnvironment)).Returns(updateResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, updateResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Detail).WithControllerName(ControllerNames.Application).WithRouteValue("id", applicationEnvironment.ApplicationId);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region detail

        [Test]
        public void Detail_EnvironmentNotFound_RedirectToApplicationListAndSetErrorMessageTempData()
        {
            //Arrange
            var id = 0;

            ApplicationEnvironment applicationEnvironment = null;

            applicationEnvironmentService.Setup(x => x.GetApplicationEnvironment(id)).Returns(applicationEnvironment);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationEnvironmentNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_EnvironmentExists_ReturnView()
        {
            //Arrange
            var id = 1;
            var applicationEnvironmentViewModel = new ApplicationEnvironmentViewModel();

            var applicationEnvironment = new ApplicationEnvironment()
            {
                ApplicationId = id
            };

            applicationEnvironmentService.Setup(x => x.GetApplicationEnvironment(id)).Returns(applicationEnvironment);
            viewModelFactory.Setup(x => x.CreateApplicationEnvironmentDetailViewModel(applicationEnvironment)).Returns(applicationEnvironmentViewModel);

            //Act
            var result = controller.Detail(id);

            //Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.Detail).ModelAs<ApplicationEnvironmentViewModel>();
            resultModel.Should().Be(applicationEnvironmentViewModel);
        }

        #endregion

        #region delete

        [Test]
        public void Delete_Error_ReturnErrorResult()
        {
            //Arrange
            var id = 1;
            var applicationId = 1;
            var redirectUrl = $"/application/detail/{applicationId}";
            var routeValues = new RouteValueDictionary();
            var serviceResult = ServiceResult.Error();

            applicationEnvironmentService.Setup(x => x.DeleteApplicationEnvironment(applicationId)).Returns(serviceResult);
            routeValueFactory.Setup(x => x.CreateRouteValuesForGenerateUrl(applicationId)).Returns(routeValues);
            urlHelper.Setup(x => x.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, routeValues)).Returns(redirectUrl);

            //Act
            var result = controller.Delete(id, applicationId);

            //Assert
            var expectedResult = RedirectableClientActionResult.Error(redirectUrl);
            AssertHelpers.AssertRedirectableClientActionResult(result, expectedResult);
        }

        [Test]
        public void Delete_Success_ReturnSuccessAndRedirectUrl()
        {
            //Arrange
            var id = 1;
            var applicationId = 1;
            var redirectUrl = $"/application/detail/{applicationId}";
            var routeValues = new RouteValueDictionary();
            var serviceResult = ServiceResult.Success(redirectUrl);

            applicationEnvironmentService.Setup(x => x.DeleteApplicationEnvironment(applicationId)).Returns(serviceResult);
            routeValueFactory.Setup(x => x.CreateRouteValuesForGenerateUrl(applicationId)).Returns(routeValues);
            urlHelper.Setup(x => x.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, routeValues)).Returns(redirectUrl);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Delete(id, applicationId);

            //Assert
            var expectedResult = RedirectableClientActionResult.Success(redirectUrl);
            AssertHelpers.AssertRedirectableClientActionResult(result, expectedResult);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region GetEnvironments

        [Test]
        public void GetEnvironmentList_LogViewModelIsFull_ReturnClientDataResultSuccessAndLogViewModelApplications()
        {
            // Arrange

            var ApplicationEnvironmentList = new List<ApplicationEnvironment> { new ApplicationEnvironment
            {
                HasLog=true,
                EnvironmentId=1,
                PhysicalPath="path"
            },
            new ApplicationEnvironment
            {
                HasLog=false,
                EnvironmentId=2
            }
            };
            var hasLog = ApplicationEnvironmentList.Where(q => q.HasLog).ToList();

            int applicationId = 1;
            LogViewModel logViewModel = new LogViewModel { Environments = hasLog };

            applicationEnvironmentService.Setup(x => x.GetApplicationEnvironmentsHasLog(applicationId)).Returns(hasLog);

            // Act
            var result = controller.GetEnvironmentList(applicationId);

            // Assert

            var expectedResult = ClientDataResult.Success(logViewModel.Environments);
            var resultModel = result.Should().BeOfType<JsonResult>().Which.Value.As<ClientDataResult>();
            resultModel.IsSuccess.Should().Be(expectedResult.IsSuccess);
            resultModel.Message.Should().Be(expectedResult.Message);
            resultModel.Data.Should().NotBeNull();

            //Datası hasLog yüzünden referans hatası vermekte kontrol edilemedi.
        }

        [Test]
        public void GetEnvironmentList_LogViewModelIsNull_ReturnClientDataResultErrorAndLogViewModelNull()
        {
            // Arrange

            var ApplicationEnvironmentList = new List<ApplicationEnvironment> { };
            var hasLog = ApplicationEnvironmentList.Where(q => q.HasLog).ToList();

            int applicationId = 1;

            applicationEnvironmentService.Setup(x => x.GetApplicationEnvironmentsHasLog(applicationId)).Returns(hasLog);

            // Act
            var result = controller.GetEnvironmentList(applicationId);

            // Assert

            var expectedResult = ClientDataResult.Error(Messages.ApplicationEnvironmentNotFound);
            var resultModel = result.Should().BeOfType<JsonResult>().Which.Value.As<ClientDataResult>();
            resultModel.IsSuccess.Should().Be(expectedResult.IsSuccess);
            resultModel.Message.Should().Be(expectedResult.Message);
            resultModel.Data.Should().Be(expectedResult.Data);
        }

        #endregion

        #region helpers

        public void SetResultMessageTempData(Dictionary<string, string> tempDataKeyValuePairs)
        {
            controller.TempData = SetupHelpers.CreateResultMessageTempData(tempDataKeyValuePairs);
        }

        static ApplicationEnvironmentViewModel CreateApplicationEnvironmentViewModel(ApplicationEnvironment applicationEnvironment)
        {
            return new ApplicationEnvironmentViewModel
            {
                ApplicationEnvironment = applicationEnvironment
            };
        }

        static ApplicationEnvironment CreateApplicationEnvironment(int applicationId, string applicationName)
        {
            return new ApplicationEnvironment
            {
                ApplicationId = applicationId,
                ApplicationName = applicationName
            };
        }

        List<Environment> SetupGetEnvironments()
        {
            var environments = new List<Environment>();
            environmentService.Setup(x => x.GetEnvironments()).Returns(environments);

            return environments;
        }

        #endregion
    }
}