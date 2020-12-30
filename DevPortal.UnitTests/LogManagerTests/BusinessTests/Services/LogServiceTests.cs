using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Log.Business;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.ObjectModel;

namespace DevPortal.UnitTests.LogManagerTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class LogServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IFileSystemService> fileSystemService;

        StrictMock<ILoggingService> loggingService;

        LogService service;

        [SetUp]
        public void Initialize()
        {
            loggingService = new StrictMock<ILoggingService>();
            fileSystemService = new StrictMock<IFileSystemService>();

            service = new LogService(loggingService.Object, fileSystemService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            fileSystemService.VerifyAll();
            loggingService.VerifyAll();
        }

        #endregion

        #region GetFileContent

        [Test]
        public void GetFileContent_NoCondition_ReturnFileModel()
        {
            // Arrange
            var path = "url";
            string text = "deneme";
            var LogFile = new LogFileModel { Text = text };
            fileSystemService.Setup(x => x.GetFileContent(path)).Returns(LogFile);

            //Act
            var result = service.GetFileContent(path);

            // Assert
            result.Text.Should().Be(text);
            result.Should().NotBeNull();
        }

        [Test]
        public void GetFileContent_NoCondition_ReturnFileModelAndLogError()
        {
            // Arrange
            var path = "url";
            var exception = new Exception("test hatası");
            var message = "Dosya listesi okunamadı.";

            fileSystemService.Setup(x => x.GetFileContent(path)).Throws(exception);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), message, exception));

            //Act
            var result = service.GetFileContent(path);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetFilePathGenericList

        [Test]
        public void GetFilePathGenericList_Success_ReturnFileModel()
        {
            // Arrange
            var path = "url";
            var log = new LogFileModel { DateModified = "" };
            Collection<LogFileModel> logFiles = new Collection<LogFileModel> { log };

            fileSystemService.Setup(x => x.GetFilePathGenericList(path)).Returns(logFiles);

            //Act
            var result = service.GetFilePathGenericList(path);

            // Assert
            result[0].DateModified.Should().Be(logFiles[0].DateModified);

            result.Should().NotBeNull();
        }

        #endregion

        #region GetFileContents

        [Test]
        public void GetFileContents_NoCondition_ReturnDossier()
        {
            // Arrange
            byte[] byt = new byte[1];
            LogFileModel fileModel = new LogFileModel { Path = "name\a", FileContent = byt };

            var path = "\\venus\\desktop";

            fileSystemService.Setup(x => x.GetDownloadFileContents(path)).Returns(fileModel);

            //Act
            var result = service.GetFileContents(path);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(fileModel.Name);
            result.FileContent.Should().HaveCount(byt.Length);
        }

        #endregion
    }
}