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
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationDependencyControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationReaderService> applicationReaderService;

        StrictMock<IApplicationDependencyWriterService> applicationDependencyWriterService;

        StrictMock<IApplicationDependencyReaderService> applicationDependencyReaderService;

        StrictMock<IApplicationDependencyViewModelFactory> viewModelFactory;

        StrictMock<IUrlGeneratorService> urlGeneratorService;

        StrictMock<IRouteValueFactory> routeValueFactory;

        ApplicationDependencyController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();
            applicationDependencyWriterService = new StrictMock<IApplicationDependencyWriterService>();
            applicationDependencyReaderService = new StrictMock<IApplicationDependencyReaderService>();
            viewModelFactory = new StrictMock<IApplicationDependencyViewModelFactory>();
            urlGeneratorService = new StrictMock<IUrlGeneratorService>();
            routeValueFactory = new StrictMock<IRouteValueFactory>();

            controller = new ApplicationDependencyController(
                userSessionService.Object,
                applicationReaderService.Object,
                applicationDependencyWriterService.Object,
                applicationDependencyReaderService.Object,
                viewModelFactory.Object,
                urlGeneratorService.Object,
                routeValueFactory.Object
                );
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationReaderService.VerifyAll();
            applicationDependencyWriterService.VerifyAll();
            applicationDependencyReaderService.VerifyAll();
            viewModelFactory.VerifyAll();
            userSessionService.VerifyAll();
            urlGeneratorService.VerifyAll();
            routeValueFactory.VerifyAll();
        }

        #endregion

        #region add- get

        [Test]
        public void Add_NoCondition_ReturnDApplicationDependencyViewModel()
        {
            //Arrange
            var viewModel = new ApplicationDependencyViewModel();
            var applications = new List<ApplicationListItem>();
            var applicationGroups = new List<ApplicationGroup>();
            var applicationDependency = new ApplicationDependency();
            var applicationId = 1;

            applicationReaderService.Setup(x => x.GetApplications()).Returns(applications);
            applicationReaderService.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);
            viewModelFactory.Setup(x => x.CreatApplicationDependencyViewModelAddView(applicationId, applications, applicationGroups)).Returns(viewModel);

            //Act
            var result = controller.Add(applicationId);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<ApplicationDependencyViewModel>().Should().Be(viewModel);
            applicationDependencyWriterService.Verify(x => x.AddApplicationDependency(applicationDependency), Times.Never);
        }

        #endregion

        #region add - post

        [Test]
        public void Add_InvalidModel_ReturnDApplicationDependencyViewModel()
        {
            //Arrange
            var viewModel = new ApplicationDependencyViewModel();
            var applications = new List<ApplicationListItem>();
            var applicationGroups = new List<ApplicationGroup>();
            var applicationId = 1;
            var applicationDependency = new ApplicationDependency { DependedApplicationId = applicationId };

            controller.ModelState.AddModelError("", "invalid model");
            applicationReaderService.Setup(x => x.GetApplications()).Returns(applications);
            applicationReaderService.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);
            viewModelFactory.Setup(x => x.CreatApplicationDependencyViewModelAddView(applicationId, applications, applicationGroups)).Returns(viewModel);

            //Act
            var result = controller.Add(applicationDependency);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<ApplicationDependencyViewModel>().Should().Be(viewModel);
            applicationDependencyWriterService.Verify(x => x.AddApplicationDependency(applicationDependency), Times.Never);
        }

        [Test]
        public void Add_AddFails_ReturnDatabaseDependencyViewModel()
        {
            //Arrange
            var viewModel = new ApplicationDependencyViewModel();
            var applications = new List<ApplicationListItem>();
            var applicationGroups = new List<ApplicationGroup>();
            var applicationId = 1;
            var applicationDependency = new ApplicationDependency { DependedApplicationId = applicationId };
            var addResult = ServiceResult.Error("Error");
            var userId = 1;

            applicationDependencyWriterService.Setup(x => x.AddApplicationDependency(applicationDependency)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationReaderService.Setup(x => x.GetApplications()).Returns(applications);
            applicationReaderService.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);
            viewModelFactory.Setup(x => x.CreatApplicationDependencyViewModelAddView(applicationId, applications, applicationGroups)).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(applicationDependency);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<ApplicationDependencyViewModel>().Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Add_AddSuccess_ReturnDatabaseDependencyViewModel()
        {
            //Arrange
            var applicationId = 1;
            var applicationDependency = new ApplicationDependency { DependentApplicationId = applicationId };
            var addResult = ServiceResult.Success("Success");
            var userId = 1;
            var redirectUrl = $"application/detail{applicationId}";

            applicationDependencyWriterService.Setup(x => x.AddApplicationDependency(applicationDependency)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            urlGeneratorService.Setup(x => x.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, applicationId)).Returns(redirectUrl);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(applicationDependency);

            //Assert
            result.Should().BeRedirectResult().WithUrl(redirectUrl);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        #endregion

        #region detail

        [Test]
        public void Detail_ApplicationDependencyIdLessThanZero_ReturnApplicationDependencyNotFound()
        {
            //Arrange
            var id = -1;

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationDependencyNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_ApplicationDependencyNotExists_ReturnApplicationDependencyNotFound()
        {
            //Arrange
            var id = 1;
            ApplicationDependency applicationDependency = null;

            applicationDependencyReaderService.Setup(x => x.GetApplicationDependency(id)).Returns(applicationDependency);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationDependencyNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_ApplicationDependencyExists_ReturnApplicationDependencyViewModel()
        {
            //Arrange
            var id = 1;
            var applicationDependency = new ApplicationDependency();
            var viewModel = new ApplicationDependencyViewModel();

            applicationDependencyReaderService.Setup(x => x.GetApplicationDependency(id)).Returns(applicationDependency);
            viewModelFactory.Setup(x => x.CreateApplicationDependencyViewModel(applicationDependency)).Returns(viewModel);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Detail).ModelAs<ApplicationDependencyViewModel>().Should().Be(viewModel);
        }

        #endregion

        #region edit

        [Test]
        public void Edit_ApplicationDependencyNotFound_ReturnApplicationListView()
        {
            //Arrange
            var id = 1;
            ApplicationDependency applicationDependency = null;

            applicationDependencyReaderService.Setup(s => s.GetApplicationDependency(id)).Returns(applicationDependency);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationDependencyNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(id);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_ApplicationDependencyExists_ReturnApplicationDependencyViewModel()
        {
            //Arrange
            var id = 1;
            var viewModel = new ApplicationDependencyViewModel();
            var applicationDependency = new ApplicationDependency();

            applicationDependencyReaderService.Setup(s => s.GetApplicationDependency(id)).Returns(applicationDependency);
            viewModelFactory.Setup(x => x.CreateApplicationDependencyEditViewModel(applicationDependency)).Returns(viewModel);

            //Act
            var result = controller.Edit(id);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<ApplicationDependencyViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Edit_InvalidModel_ReturnApplicationDependencyViewModel()
        {
            //Arrange
            var viewModel = new ApplicationDependencyViewModel();
            var applicationDependency = new ApplicationDependency();

            controller.ModelState.AddModelError("", "invalid model");

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<ApplicationDependencyViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Edit_UpdateFails_ReturnApplicationDependencyViewModel()
        {
            //Arrange
            var applicationDependency = new ApplicationDependency { Id = 1 };
            var applicationDependencyViewModel = new ApplicationDependencyViewModel
            {
                ApplicationDependency = applicationDependency
            };
            var serviceResult = ServiceResult.Error("Error");
            var userId = 1;

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationDependencyWriterService.Setup(x => x.UpdateApplicationDependency(applicationDependencyViewModel.ApplicationDependency)).Returns(serviceResult);
            applicationDependencyReaderService.Setup(s => s.GetApplicationDependency(applicationDependency.Id)).Returns(applicationDependency);
            viewModelFactory.Setup(x => x.CreateApplicationDependencyEditViewModel(applicationDependency)).Returns(applicationDependencyViewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(applicationDependencyViewModel);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<ApplicationDependencyViewModel>().Should().Be(applicationDependencyViewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Edit_UpdateSuccess_ReturnExternalDependencyViewModel()
        {
            //Arrange
            var applicationDependency = new ApplicationDependency { Id = 1 };
            var applicationDependencyViewModel = new ApplicationDependencyViewModel
            {
                ApplicationDependency = applicationDependency
            };
            var serviceResult = ServiceResult.Success("Success");
            var userId = 1;

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationDependencyWriterService.Setup(x => x.UpdateApplicationDependency(applicationDependencyViewModel.ApplicationDependency)).Returns(serviceResult);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(applicationDependencyViewModel);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationDependencyControllerActionNames.Detail).WithControllerName(ControllerNames.ApplicationDependency)
                .WithRouteValue("id", applicationDependency.Id);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region delete

        [Test]
        public void Delete_DeleteFails_ReturnRedirectableClientActionResult()
        {
            //Arrange
            var id = 1;
            var applicationId = 44;
            var serviceResult = ServiceResult.Error("Error");
            var routeValue = new RouteValueDictionary();
            var redirectUrl = "redirectUrl";

            applicationDependencyWriterService.Setup(x => x.DeleteApplicationDependency(id)).Returns(serviceResult);
            routeValueFactory.Setup(x => x.CreateRouteValuesForGenerateUrl(applicationId)).Returns(routeValue);
            urlGeneratorService.Setup(x => x.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, routeValue)).Returns(redirectUrl);

            //Act
            var result = controller.Delete(id, applicationId);

            //Assert
            var expectedResult = RedirectableClientActionResult.Error(redirectUrl);
            AssertHelpers.AssertRedirectableClientActionResult(result, expectedResult);
        }

        [Test]
        public void Delete_DeleteSuccess_ReturnRedirectableClientActionResult()
        {
            //Arrange
            var id = 1;
            var applicationId = 44;
            var serviceResult = ServiceResult.Success("Success");
            var routeValue = new RouteValueDictionary();
            var redirectUrl = "redirectUrl";

            applicationDependencyWriterService.Setup(x => x.DeleteApplicationDependency(id)).Returns(serviceResult);
            routeValueFactory.Setup(x => x.CreateRouteValuesForGenerateUrl(applicationId)).Returns(routeValue);
            urlGeneratorService.Setup(x => x.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, routeValue)).Returns(redirectUrl);
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

        #region get applications by group id

        [Test]
        public void GetApplicationsByGroupId_NoCondition_ReturnDApplicationDependencyViewModel()
        {
            //Arrange
            var applicationGroupId = 1;
            var applicationListItems = new List<ApplicationListItem>();

            applicationReaderService.Setup(x => x.GetApplicationsByGroupId(applicationGroupId)).Returns(applicationListItems);

            //Act
            var result = controller.GetApplicationsByGroupId(applicationGroupId);

            //Assert
            result.Should().BeOfType<JsonResult>().Which.Value.As<List<ApplicationListItem>>().Should().BeSameAs(applicationListItems);
        }

        #endregion

        #region helpers

        public void SetResultMessageTempData(Dictionary<string, string> tempDataKeyValuePairs)
        {
            controller.TempData = SetupHelpers.CreateResultMessageTempData(tempDataKeyValuePairs);
        }

        #endregion
    }
}