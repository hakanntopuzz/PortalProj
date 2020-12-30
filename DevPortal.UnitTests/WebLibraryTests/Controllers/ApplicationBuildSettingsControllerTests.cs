using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.UnitTests.TestHelpers;
using DevPortal.Web.Library.Abstract.Factories;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationBuildSettingsControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationReaderService> applicationReaderService;

        StrictMock<IApplicationBuildSettingsViewModelFactory> viewModelFactory;

        StrictMock<IApplicationBuildSettingsService> buildSettingsService;

        ApplicationBuildSettingsController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();
            viewModelFactory = new StrictMock<IApplicationBuildSettingsViewModelFactory>();
            buildSettingsService = new StrictMock<IApplicationBuildSettingsService>();

            controller = new ApplicationBuildSettingsController(
                userSessionService.Object,
                applicationReaderService.Object,
                viewModelFactory.Object,
                buildSettingsService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationReaderService.VerifyAll();
            viewModelFactory.VerifyAll();
            userSessionService.VerifyAll();
            buildSettingsService.VerifyAll();
        }

        #endregion

        #region get application build settings

        [Test]
        public void GetApplicationBuildSettings_NoCondition_ReturnSettings()
        {
            // Arrange
            var applicationId = 1;
            var settings = new ApplicationBuildSettings();

            buildSettingsService.Setup(x => x.GetApplicationBuildSettings(applicationId)).Returns(settings);

            // Act
            var result = controller.GetApplicationBuildSettings(applicationId);

            // Assert
            result.Should().Be(settings);
        }

        #endregion

        #region edit

        [Test]
        public void Edit_ApplicationDoesNotExist_ReturnErrorMessageAndRedirectToApplicationList()
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
        public void Edit_ApplicationExists_ReturnView()
        {
            //Arrange
            var applicationId = 1;
            var buildSettings = new ApplicationBuildSettings();
            var application = new Application { Name = "name" };
            var viewModel = new ApplicationBuildSettingsViewModel();

            applicationReaderService.Setup(x => x.GetApplication(applicationId)).Returns(application);
            buildSettingsService.Setup(x => x.GetApplicationBuildSettings(applicationId)).Returns(buildSettings);
            viewModelFactory.Setup(x => x.CreateApplicationBuildSettingsViewModel(buildSettings, application)).Returns(viewModel);

            //Act
            var result = controller.Edit(applicationId);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Edit)
                .ModelAs<ApplicationBuildSettingsViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Edit_InvalidModel_ReturnView()
        {
            //Arrange
            var viewModel = new ApplicationBuildSettingsViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Edit)
                .ModelAs<ApplicationBuildSettingsViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Edit_UpdatingFails_RedirectToApplicationAndSetSuccessResultTempData()
        {
            //Arrange
            var userId = 28;
            var viewModel = new ApplicationBuildSettingsViewModel
            {
                BuildSettings = new ApplicationBuildSettings
                {
                    ApplicationId = 1,
                    RecordUpdateInfo = new RecordUpdateInfo
                    {
                        CreatedBy = userId,
                        ModifiedBy = userId
                    }
                }
            };

            var serviceResult = ServiceResult.Error();
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            buildSettingsService.Setup(x => x.AddOrUpdateApplicationBuildSettings(viewModel.BuildSettings)).Returns(serviceResult);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Edit).ModelAs<ApplicationBuildSettingsViewModel>().Should().Be(viewModel);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_UpdatingSucceeds_RedirectToApplicationAndSetSuccessResultTempData()
        {
            //Arrange
            var userId = 28;
            var viewModel = new ApplicationBuildSettingsViewModel
            {
                BuildSettings = new ApplicationBuildSettings
                {
                    ApplicationId = 1,
                    RecordUpdateInfo = new RecordUpdateInfo
                    {
                        CreatedBy = userId,
                        ModifiedBy = userId
                    }
                }
            };
            var serviceResult = ServiceResult.Success();
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            buildSettingsService.Setup(x => x.AddOrUpdateApplicationBuildSettings(viewModel.BuildSettings)).Returns(serviceResult);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            result.Should().BeRedirectToActionResult(BaseActionNames.Edit)
                .WithControllerName(ControllerNames.ApplicationBuildSettings)
                .WithRouteValue("id", viewModel.BuildSettings.ApplicationId);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
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