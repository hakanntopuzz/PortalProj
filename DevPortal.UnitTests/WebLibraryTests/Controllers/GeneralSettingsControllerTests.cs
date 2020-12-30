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
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class GeneralSettingsControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IGeneralSettingsService> generalSettingsService;

        StrictMock<IGeneralSettingsViewModelFactory> viewModelFactory;

        GeneralSettingsController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            generalSettingsService = new StrictMock<IGeneralSettingsService>();
            viewModelFactory = new StrictMock<IGeneralSettingsViewModelFactory>();

            controller = new GeneralSettingsController(
                userSessionService.Object,
                generalSettingsService.Object,
                viewModelFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            generalSettingsService.VerifyAll();
            viewModelFactory.VerifyAll();
            userSessionService.VerifyAll();
        }

        #endregion

        #region helpers

        public void SetResultMessageTempData(Dictionary<string, string> tempDataKeyValuePairs)
        {
            controller.TempData = SetupHelpers.CreateResultMessageTempData(tempDataKeyValuePairs);
        }

        #endregion

        #region general settings

        [Test]
        public void Index_NoCondition_ReturnEditView()
        {
            // Arrange
            var generalSettingsModel = new GeneralSettings();
            GeneralSettingsViewModel model = new GeneralSettingsViewModel();

            generalSettingsService.Setup(x => x.GetGeneralSettings()).Returns(generalSettingsModel);
            viewModelFactory.Setup(x => x.CreateGeneralSettingsViewModel(generalSettingsModel)).Returns(model);

            // Act
            var result = controller.Index();

            // Assert
            var resultModel = result.Should().BeViewResult(ViewNames.Edit).ModelAs<GeneralSettingsViewModel>();
            resultModel.Should().BeEquivalentTo(model);
        }

        [Test]
        public void Edit_InvalidModel_ReturnViewModel()
        {
            // Arrange
            var generalSettings = new GeneralSettings();
            controller.ModelState.AddModelError("", "");
            var model = new GeneralSettingsViewModel();

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.UpdateFails);
            SetResultMessageTempData(expectedTempData);
            viewModelFactory.Setup(x => x.CreateGeneralSettingsViewModel(generalSettings)).Returns(model);

            //Act
            var result = controller.Edit(generalSettings);

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.Edit).ModelAs<GeneralSettingsViewModel>();
            resultModel.Should().BeEquivalentTo(model);
        }

        [Test]
        public void Edit_UpdateFails_ReturnViewModel()
        {
            // Arrange
            var generalSettings = new GeneralSettings();
            var userId = 3;
            var updateResult = ServiceResult.Error(Messages.UpdateFails);
            var model = new GeneralSettingsViewModel();

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            generalSettingsService.Setup(x => x.UpdateGeneralSettings(generalSettings)).Returns(updateResult);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.UpdateFails);
            SetResultMessageTempData(expectedTempData);
            viewModelFactory.Setup(x => x.CreateGeneralSettingsViewModel(generalSettings)).Returns(model);

            // Act
            var result = controller.Edit(generalSettings);

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.Edit).ModelAs<GeneralSettingsViewModel>();
            resultModel.Should().BeEquivalentTo(model);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Edit_UpdateSucceeds_ReturnViewModel()
        {
            // Arrange
            var generalSettingsViewModel = new GeneralSettingsViewModel();
            var generalSettings = new GeneralSettings();
            var userId = 3;
            var updateResult = ServiceResult.Success(Messages.UpdateSucceeds);

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            generalSettingsService.Setup(x => x.UpdateGeneralSettings(generalSettingsViewModel.GeneralSettings)).Returns(updateResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, Messages.UpdateSucceeds);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Edit(generalSettingsViewModel.GeneralSettings);

            // Assert
            result.Should().BeRedirectToActionResult().WithActionName(GeneralSettingsControllerActionNames.Edit);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        #endregion
    }
}