using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.UnitTests.TestHelpers;
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
    public class ApplicationPdfExportControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationPdfExportService> applicationPdfExportService;

        ApplicationPdfExportController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            applicationPdfExportService = new StrictMock<IApplicationPdfExportService>();

            controller = new ApplicationPdfExportController(
                userSessionService.Object,
                applicationPdfExportService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationPdfExportService.VerifyAll();
        }

        #endregion

        #region helpers

        public void SetResultMessageTempData(Dictionary<string, string> tempDataKeyValuePairs)
        {
            controller.TempData = SetupHelpers.CreateResultMessageTempData(tempDataKeyValuePairs);
        }

        #endregion

        #region ExportToPdf

        [Test]
        public void ExportToPdf_ServiceResultSuccess_ReturnView()
        {
            //Arrange
            var applicationId = 1;
            var applicationFullModel = new ApplicationFullModel();
            var serviceResult = PdfServiceResult.Success(applicationFullModel);

            applicationPdfExportService.Setup(x => x.ExportApplicationToPdf(applicationId)).Returns(serviceResult);

            //Act
            var result = controller.ExportToPdf(applicationId);

            //Assert
            result.Should().BeViewResult(ViewNames.ExportToPdf).ModelAs<ApplicationFullModel>().Should().Be(applicationFullModel);
        }

        [Test]
        public void ExportToPdf_ServiceResultError_ReturnRedirectApplicationDetailView()
        {
            //Arrange
            var applicationId = 1;
            var serviceResult = PdfServiceResult.Error(Messages.GeneralError);

            applicationPdfExportService.Setup(x => x.ExportApplicationToPdf(applicationId)).Returns(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.GeneralError);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.ExportToPdf(applicationId);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationControllerActionNames.Detail).WithRouteValue("id", applicationId);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion
    }
}