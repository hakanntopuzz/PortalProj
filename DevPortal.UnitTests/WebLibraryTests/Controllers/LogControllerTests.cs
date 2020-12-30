using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Log.Business.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class LogControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<ILogService> logService;

        StrictMock<IApplicationReaderService> applicationReaderService;

        StrictMock<ILogViewModelFactory> logViewModelFactory;

        LogController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            logService = new StrictMock<ILogService>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();
            logViewModelFactory = new StrictMock<ILogViewModelFactory>();

            controller = new LogController(
                userSessionService.Object,
                logService.Object,
                applicationReaderService.Object,
                logViewModelFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            logService.VerifyAll();
            applicationReaderService.VerifyAll();
            logViewModelFactory.VerifyAll();
        }

        #endregion

        #region index

        [Test]
        public void Index_NoCondition_ReturnViewWithApplicationGroups()
        {
            // Arrange
            var applicationGroups = new List<ApplicationGroup>();
            const string physicalPath = "";
            var logViewModel = new LogViewModel { Applications = null };

            applicationReaderService.Setup(x => x.GetApplicationGroups()).Returns(applicationGroups);
            logViewModelFactory.Setup(x => x.CreateLogViewModel(applicationGroups, physicalPath)).Returns(logViewModel);

            // Act
            var result = controller.Index(physicalPath);

            // Assert
            result.Should().BeViewResult(ViewNames.Index).ModelAs<LogViewModel>();
        }

        #endregion

        #region log detail

        [Test]
        public void Detail_FileExists_ReturnDetailViewWithFile()
        {
            // Arrange
            string path = "path";
            string text = "text";

            var fileModel = new LogFileModel
            {
                Text = text
            };
            var logFile = new LogFileViewModel { LogFile = fileModel };

            logService.Setup(x => x.GetFileContent(path)).Returns(fileModel);
            logViewModelFactory.Setup(x => x.CreateLogFileViewModel(fileModel)).Returns(logFile);

            // Act
            var result = controller.LogDetails(path);

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.LogDetails).ModelAs<LogFileViewModel>();
            resultModel.LogFile.Text.Should().Be(fileModel.Text);
            resultModel.LogFile.Should().Be(fileModel);
        }

        [Test]
        public void Detail_FileDoesNotExist_ReturnDetailViewWithEmptyModel()
        {
            // Arrange
            string path = "path";
            LogFileModel fileModel = null;
            LogViewModel logWiew = new LogViewModel();
            logService.Setup(x => x.GetFileContent(path)).Returns(fileModel);
            logViewModelFactory.Setup(x => x.CreateLogViewModel()).Returns(logWiew);

            // Act
            var result = controller.LogDetails(path);

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.LogDetails).ModelAs<LogViewModel>();
            resultModel.Text.Should().BeNull();
            resultModel.Folders.Should().BeEmpty();
        }

        #endregion

        #region get result file list partial

        [Test]
        public void GetResultFileListPartial_LogFileListEmpty_ReturnClientDataResultErrorAndMessage()
        {
            // Arrange
            string path = null;

            // Act
            var result = controller.GetResultFileListPartial(path);

            // AssertS
            var expectedResult = ClientDataResult.Error("Dosya yolu boş olamaz.");
            var resultModel = result.Should().BeOfType<JsonResult>().Which.Value.As<ClientDataResult>();
            resultModel.Message.Should().Be(expectedResult.Message);
            resultModel.IsSuccess.Should().Be(expectedResult.IsSuccess);
        }

        [Test]
        public void GetResultFileListPartial_LogFileListNotEmpty_ReturnClientDataResultErrorAndMessage()
        {
            // Arrange
            string path = "path";
            Collection<LogFileModel> list = new Collection<LogFileModel> { };
            LogViewModel logViewModel = new LogViewModel { };
            logService.Setup(x => x.GetFilePathGenericList(path)).Returns(list);

            // Act
            var result = controller.GetResultFileListPartial(path);

            // AssertS
            var expectedResult = ClientDataResult.Error(logViewModel.Folders);
            var resultModel = result.Should().BeOfType<JsonResult>().Which.Value.As<ClientDataResult>();

            resultModel.Message.Should().Be(expectedResult.Message);
            resultModel.IsSuccess.Should().Be(expectedResult.IsSuccess);
            resultModel.Data.GetType().Should().Be(expectedResult.Data.GetType());//referans durumunda karşılaştıramadığım için typlerini karşılaştırdım.
        }

        [Test]
        public void GetResultFileListPartial_LogFolderIsFull_ReturnClientDataResultSuccessAndLogViewModelLogFiles()
        {
            // Arrange
            string path = "path";
            Collection<LogFileModel> list = new Collection<LogFileModel> { new LogFileModel { Text = "" } };

            logService.Setup(x => x.GetFilePathGenericList(path)).Returns(list);

            // Act
            var result = controller.GetResultFileListPartial(path);

            // Assert

            var expectedResult = ClientDataResult.Success(list);
            var resultModel = result.Should().BeOfType<JsonResult>().Which.Value.As<ClientDataResult>();
            resultModel.IsSuccess.Should().Be(expectedResult.IsSuccess);
            resultModel.Message.Should().Be(expectedResult.Message);
            resultModel.Data.Should().Be(expectedResult.Data);
        }

        #endregion

        #region download log file

        [Test]
        public void DownloadLogFile_NoCondition_ReturnFile()
        {
            // Arrange
            var path = "\\venus\\desktop";
            byte[] content = new byte[0];

            var fileModel = new LogFileModel
            {
                FileContent = content,
                Path = path
            };

            var logFile = new LogFileViewModel { LogFile = fileModel };

            logService.Setup(x => x.GetFileContents(path)).Returns(fileModel);

            // Act
            var result = controller.DownloadLogFile(path);

            // Assert
            result.Should().BeOfType<FileContentResult>().Which.FileContents.Should().BeSameAs(fileModel.FileContent);
            result.Should().BeOfType<FileContentResult>().Which.ContentType.Should().Be("application/force-download");
            result.Should().BeOfType<FileContentResult>().Which.FileDownloadName.Should().Be(fileModel.Name);
        }

        #endregion

        #region Get Applicatin List

        [Test]
        public void GetApplicationsList_LogViewModel_ReturnClientDataResultSuccessAndLogViewModelApplications()
        {
            // Arrange
            int applicationGroupId = 1;
            LogViewModel logViewModel = new LogViewModel();
            logViewModelFactory.Setup(x => x.CreateLogViewModel()).Returns(logViewModel);
            applicationReaderService.Setup(x => x.GetApplicationsWithLogByApplicationGroup(applicationGroupId)).Returns(logViewModel.Applications);

            // Act
            var result = controller.GetApplicationsList(applicationGroupId);

            // Assert

            var expectedResult = ClientDataResult.Success(logViewModel.Applications);
            var resultModel = result.Should().BeOfType<JsonResult>().Which.Value.As<ClientDataResult>();
            resultModel.IsSuccess.Should().Be(expectedResult.IsSuccess);
            resultModel.Message.Should().Be(expectedResult.Message);
            resultModel.Data.Should().Be(expectedResult.Data);
        }

        #endregion
    }
}