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
    public class ExternalDependencyControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationReaderService> applicationReaderService;

        StrictMock<IExternalDependencyService> externalDependencyService;

        StrictMock<IExternalDependencyViewModelFactory> viewModelFactory;

        ExternalDependencyController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();
            externalDependencyService = new StrictMock<IExternalDependencyService>();
            viewModelFactory = new StrictMock<IExternalDependencyViewModelFactory>();

            controller = new ExternalDependencyController(
                userSessionService.Object,
                applicationReaderService.Object,
                externalDependencyService.Object,
                viewModelFactory.Object
                );
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            externalDependencyService.VerifyAll();
            viewModelFactory.VerifyAll();
            applicationReaderService.VerifyAll();
            userSessionService.VerifyAll();
        }

        #endregion

        #region detail

        [Test]
        public void Detail_IdGreatherThanOne_ReturnExternalDependencyNotFound()
        {
            //Arrange
            var id = 0;

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ExternalDependencyNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_ExternalDependencyNotExists_ReturnExternalDependencyNotFound()
        {
            //Arrange
            var id = 1;
            ExternalDependency externalDependency = null;

            externalDependencyService.Setup(x => x.GetExternalDependencyById(id)).Returns(externalDependency);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ExternalDependencyNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_EnvironmentExists_ReturnEnvironmentViewModel()
        {
            //Arrange
            var id = 1;
            var externalDependency = new ExternalDependency();
            var viewModel = new ExternalDependencyViewModel();

            externalDependencyService.Setup(x => x.GetExternalDependencyById(id)).Returns(externalDependency);
            viewModelFactory.Setup(x => x.CreateExternalDependencyViewModel(externalDependency)).Returns(viewModel);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Detail).ModelAs<ExternalDependencyViewModel>().Should().Be(viewModel);
        }

        #endregion

        #region add

        [Test]
        public void Add_ApplicationDoesNotExist_ReturnErrorMessageAndRedirectToApplicationList()
        {
            //Arrange
            var applicationId = 1;
            Application application = null;

            applicationReaderService.Setup(s => s.GetApplication(applicationId)).Returns(application);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(applicationId);

            //Assert
            result.Should().BeRedirectToActionResult()
                .WithActionName(ApplicationControllerActionNames.Index)
                .WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Add_NoCondition_ReturnExternalDependencyViewModel()
        {
            //Arrange
            var applicationId = 1;
            var application = new Application
            {
                Name = "application",
                Id = applicationId
            };
            var viewModel = new ExternalDependencyViewModel();
            var externalDependency = new ExternalDependency();

            applicationReaderService.Setup(s => s.GetApplication(applicationId)).Returns(application);
            viewModelFactory.Setup(x => x.CreateAddExternalDependencyViewModel(application.Name, applicationId)).Returns(viewModel);

            //Act
            var result = controller.Add(applicationId);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<ExternalDependencyViewModel>().Should().Be(viewModel);
            externalDependencyService.Verify(x => x.AddExternalDependency(externalDependency), Times.Never);
        }

        [Test]
        public void Add_InvalidModel_ReturnExternalDependencyViewModel()
        {
            //Arrange
            var viewModel = new ExternalDependencyViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            //Act
            var result = controller.Add(viewModel);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<ExternalDependencyViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Add_AddFails_ReturnExternalDependencyViewModel()
        {
            //Arrange
            var viewModel = new ExternalDependencyViewModel
            {
                ExternalDependency = new ExternalDependency()
            };

            var addResult = Int32ServiceResult.Error("Error");
            var userId = 1;

            externalDependencyService.Setup(x => x.AddExternalDependency(viewModel.ExternalDependency)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(viewModel);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<ExternalDependencyViewModel>().Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Add_AddSuccess_ReturnExternalDependencyViewModel()
        {
            //Arrange
            var externalDependencyId = 1;
            var viewModel = new ExternalDependencyViewModel
            {
                ExternalDependency = new ExternalDependency()
            };
            var addResult = Int32ServiceResult.Success("Success", externalDependencyId);
            var userId = 1;

            externalDependencyService.Setup(x => x.AddExternalDependency(viewModel.ExternalDependency)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(viewModel);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(ExternalDependencyControllerActionNames.Detail).
                WithControllerName(ControllerNames.ExternalDependency).
                WithRouteValue("id", externalDependencyId);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        #endregion

        #region edit

        [Test]
        public void Edit_ExternalDependencyNotFound_ReturnApplicationListView()
        {
            //Arrange
            var id = 1;
            ExternalDependency externalDependency = null;

            externalDependencyService.Setup(s => s.GetExternalDependencyById(id)).Returns(externalDependency);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ExternalDependencyNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(id);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_ExternalDependencyExists_ReturnExternalDependencyViewModel()
        {
            //Arrange
            var id = 1;
            var viewModel = new ExternalDependencyViewModel();
            var externalDependency = new ExternalDependency();

            externalDependencyService.Setup(s => s.GetExternalDependencyById(id)).Returns(externalDependency);
            viewModelFactory.Setup(x => x.CreateEditExternalDependencyViewModel(externalDependency)).Returns(viewModel);

            //Act
            var result = controller.Edit(id);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<ExternalDependencyViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Edit_InvalidModel_ReturnExternalDependencyViewModel()
        {
            //Arrange
            var viewModel = new ExternalDependencyViewModel();
            var externalDependency = new ExternalDependency();

            controller.ModelState.AddModelError("", "invalid model");

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<ExternalDependencyViewModel>().Should().Be(viewModel);
            externalDependencyService.Verify(x => x.UpdateExternalDependency(externalDependency), Times.Never);
        }

        [Test]
        public void Edit_UpdateFails_ReturnExternalDependencyViewModel()
        {
            //Arrange
            var externalDependency = new ExternalDependency
            {
                Id = 1
            };

            var viewModel = new ExternalDependencyViewModel
            {
                ExternalDependency = externalDependency
            };

            var serviceResult = ServiceResult.Error("Error");
            var userId = 1;

            externalDependencyService.Setup(x => x.UpdateExternalDependency(externalDependency)).Returns(serviceResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            externalDependencyService.Setup(s => s.GetExternalDependencyById(externalDependency.Id)).Returns(externalDependency);
            viewModelFactory.Setup(x => x.CreateEditExternalDependencyViewModel(externalDependency)).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<ExternalDependencyViewModel>().Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Edit_UpdateSuccess_ReturnExternalDependencyViewModel()
        {
            //Arrange
            var externalDependency = new ExternalDependency
            {
                Id = 1
            };

            var viewModel = new ExternalDependencyViewModel
            {
                ExternalDependency = externalDependency
            };

            var serviceResult = ServiceResult.Success("Success");
            var userId = 1;

            externalDependencyService.Setup(x => x.UpdateExternalDependency(externalDependency)).Returns(serviceResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(ExternalDependencyControllerActionNames.Detail).WithControllerName(ControllerNames.ExternalDependency)
                .WithRouteValue("id", externalDependency.Id);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region helpers

        public void SetResultMessageTempData(Dictionary<string, string> tempDataKeyValuePairs)
        {
            controller.TempData = SetupHelpers.CreateResultMessageTempData(tempDataKeyValuePairs);
        }

        #endregion

        #region delete

        [Test]
        public void Delete_DeleteFails_ReturnRedirectableClientActionResult()
        {
            //Arrange
            var externalDependencyId = 1;
            var externalDependency = new ExternalDependency
            {
                Id = externalDependencyId
            };

            var serviceResult = StringServiceResult.Error("Error");
            var userId = 1;

            externalDependencyService.Setup(x => x.GetExternalDependencyById(externalDependencyId)).Returns(externalDependency);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            externalDependencyService.Setup(x => x.DeleteExternalDependency(externalDependency)).Returns(serviceResult);

            //Act
            var result = controller.Delete(externalDependencyId);

            //Assert
            var expectedResult = RedirectableClientActionResult.Error(string.Empty, "Error");
            AssertHelpers.AssertRedirectableClientActionResult(result, expectedResult);
        }

        [Test]
        public void Delete_DeleteSuccess_ReturnRedirectableClientActionResult()
        {
            //Arrange
            var externalDependencyId = 1;
            var externalDependency = new ExternalDependency
            {
                Id = externalDependencyId
            };

            var redirectUrl = "redirectUrl";
            var serviceResult = StringServiceResult.Success("Success", redirectUrl);
            var userId = 1;

            externalDependencyService.Setup(x => x.GetExternalDependencyById(externalDependencyId)).Returns(externalDependency);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            externalDependencyService.Setup(x => x.DeleteExternalDependency(externalDependency)).Returns(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Delete(externalDependencyId);

            //Assert
            var expectedResult = RedirectableClientActionResult.Success(redirectUrl, "Success");
            AssertHelpers.AssertRedirectableClientActionResult(result, expectedResult);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion
    }
}