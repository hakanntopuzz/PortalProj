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
    public class EnvironmentControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IEnvironmentService> environmentService;

        StrictMock<IEnvironmentViewModelFactory> EnvironmentViewModelFactory;

        StrictMock<IUrlGeneratorService> urlHelper;

        EnvironmentController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            environmentService = new StrictMock<IEnvironmentService>();
            EnvironmentViewModelFactory = new StrictMock<IEnvironmentViewModelFactory>();
            urlHelper = new StrictMock<IUrlGeneratorService>();

            controller = new EnvironmentController(
                userSessionService.Object,
                environmentService.Object,
                EnvironmentViewModelFactory.Object,
                urlHelper.Object
                );
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            environmentService.VerifyAll();
            EnvironmentViewModelFactory.VerifyAll();
            userSessionService.VerifyAll();
            urlHelper.VerifyAll();
        }

        #endregion

        #region index

        [Test]
        public void Index_NoCondition_ReturnEnvironmentViewModel()
        {
            //Arrange
            var environments = new List<Environment>();
            var viewModel = new EnvironmentViewModel();

            environmentService.Setup(x => x.GetEnvironments()).Returns(environments);
            EnvironmentViewModelFactory.Setup(x => x.CreateEnvironmentsViewModel(environments)).Returns(viewModel);

            //Act
            var result = controller.Index();

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Index).ModelAs<EnvironmentViewModel>().Should().Be(viewModel);
        }

        #endregion

        #region add- get

        [Test]
        public void Add_NoCondition_ReturnEnvironmentViewModel()
        {
            //Arrange
            var viewModel = new EnvironmentViewModel();
            var environment = new Environment();

            EnvironmentViewModelFactory.Setup(x => x.CreateEnvironmentAddView()).Returns(viewModel);

            //Act
            var result = controller.Add();

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<EnvironmentViewModel>().Should().Be(viewModel);
            environmentService.Verify(x => x.AddEnvironment(environment), Times.Never);
        }

        #endregion

        #region add - post

        [Test]
        public void Add_InvalidModel_ReturnEnvironmentViewModel()
        {
            //Arrange
            var viewModel = new EnvironmentViewModel();
            var environment = new Environment();

            controller.ModelState.AddModelError("", "invalid model");
            EnvironmentViewModelFactory.Setup(x => x.CreateEnvironmentAddView()).Returns(viewModel);

            //Act
            var result = controller.Add(environment);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<EnvironmentViewModel>().Should().Be(viewModel);
            environmentService.Verify(x => x.AddEnvironment(environment), Times.Never);
        }

        [Test]
        public void Add_AddFails_ReturnEnvironmentViewModel()
        {
            //Arrange
            var viewModel = new EnvironmentViewModel();
            var environment = new Environment();
            var addResult = ServiceResult.Error("Error");
            var userId = 1;

            environmentService.Setup(x => x.AddEnvironment(environment)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            EnvironmentViewModelFactory.Setup(x => x.CreateEnvironmentAddView()).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(environment);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<EnvironmentViewModel>().Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Add_AddSuccess_ReturnEnvironmentViewModel()
        {
            //Arrange
            var environment = new Environment();
            var addResult = ServiceResult.Success("Success");
            var userId = 1;

            environmentService.Setup(x => x.AddEnvironment(environment)).Returns(addResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(environment);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(EnvironmentControllerActionNames.Index).WithControllerName(ControllerNames.Environment);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        #endregion

        #region edit

        [Test]
        public void Edit_EnvironmentNotFound_ReturnEnvironmentListView()
        {
            //Arrange
            var id = 1;
            Environment environment = null;

            environmentService.Setup(s => s.GetEnvironment(id)).Returns(environment);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.EnvironmentNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(id);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(EnvironmentControllerActionNames.Index).WithControllerName(ControllerNames.Environment);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_EnvironmentExists_ReturnEnvironmentViewModel()
        {
            //Arrange
            var id = 1;
            var viewModel = new EnvironmentViewModel();
            var environment = new Environment();

            environmentService.Setup(s => s.GetEnvironment(id)).Returns(environment);
            EnvironmentViewModelFactory.Setup(x => x.CreateEnvironmentEditViewModel(environment)).Returns(viewModel);

            //Act
            var result = controller.Edit(id);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<EnvironmentViewModel>().Should().Be(viewModel);
            environmentService.Verify(x => x.UpdateEnvironment(environment), Times.Never);
        }

        [Test]
        public void Edit_InvalidModel_ReturnEnvironmentViewModel()
        {
            //Arrange
            var viewModel = new EnvironmentViewModel();
            var environment = new Environment();

            controller.ModelState.AddModelError("", "invalid model");

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<EnvironmentViewModel>().Should().Be(viewModel);
            environmentService.Verify(x => x.UpdateEnvironment(environment), Times.Never);
        }

        [Test]
        public void Edit_UpdateFails_ReturnEnvironmentViewModel()
        {
            //Arrange
            var environment = new Environment
            {
                Id = 1
            };

            var viewModel = new EnvironmentViewModel
            {
                Environment = environment
            };
            var serviceResult = ServiceResult.Error("Error");
            var userId = 1;

            environmentService.Setup(x => x.UpdateEnvironment(environment)).Returns(serviceResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            environmentService.Setup(s => s.GetEnvironment(environment.Id)).Returns(environment);
            EnvironmentViewModelFactory.Setup(x => x.CreateEnvironmentEditViewModel(environment)).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<EnvironmentViewModel>().Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Edit_UpdateSuccess_ReturnEnvironmentDetailView()
        {
            //Arrange
            var environment = new Environment
            {
                Id = 1
            };

            var viewModel = new EnvironmentViewModel
            {
                Environment = environment
            };
            var serviceResult = ServiceResult.Success("Success");
            var userId = 1;

            environmentService.Setup(x => x.UpdateEnvironment(environment)).Returns(serviceResult);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(EnvironmentControllerActionNames.Detail).WithControllerName(ControllerNames.Environment)
                .WithRouteValue("id", environment.Id);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region detail

        [Test]
        public void Detail_EnvironmentNotExists_ReturnEnvironmentNotFound()
        {
            //Arrange
            var id = 1;
            Environment environment = null;

            environmentService.Setup(x => x.GetEnvironment(id)).Returns(environment);

            var serviceResult = ServiceResult.Error(Messages.EnvironmentNotFound);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeRedirectToActionResult(EnvironmentControllerActionNames.Index).WithControllerName(ControllerNames.Environment);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_EnvironmentExists_ReturnEnvironmentViewModel()
        {
            //Arrange
            var id = 1;
            var environment = new Environment();
            var viewModel = new EnvironmentViewModel();

            environmentService.Setup(x => x.GetEnvironment(id)).Returns(environment);
            EnvironmentViewModelFactory.Setup(x => x.CreateEnvironmentDetailViewModel(environment)).Returns(viewModel);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Detail).ModelAs<EnvironmentViewModel>().Should().Be(viewModel);
        }

        #endregion

        #region delete

        [Test]
        public void Delete_Error_ReturnErrorResult()
        {
            //Arrange
            var id = 1;
            var redirectUrl = $"/environment/index";
            var deleteResult = ServiceResult.Error();

            environmentService.Setup(x => x.DeleteEnvironment(id)).Returns(deleteResult);
            urlHelper.Setup(x => x.GenerateUrl(EnvironmentControllerActionNames.Index, ControllerNames.Environment)).Returns(redirectUrl);

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
            var redirectUrl = $"/environment/index";
            var deleteResult = ServiceResult.Success();

            environmentService.Setup(x => x.DeleteEnvironment(id)).Returns(deleteResult);
            urlHelper.Setup(x => x.GenerateUrl(EnvironmentControllerActionNames.Index, ControllerNames.Environment)).Returns(redirectUrl);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, Messages.EnvironmentDeleted);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Delete(id);

            //Assert
            var expectedResult = RedirectableClientActionResult.Success(redirectUrl);
            AssertHelpers.AssertRedirectableClientActionResult(result, expectedResult);
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