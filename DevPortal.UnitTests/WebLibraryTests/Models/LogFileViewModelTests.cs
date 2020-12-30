using AB.Framework.UnitTests;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace DevPortal.UnitTests.WebLibraryTests.Models
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class LogFileViewModelTests : LooseBaseTestFixture
    {
        #region members & setup

        LogFileViewModel model;

        [SetUp]
        public void Initialize()
        {
            model = new LogFileViewModel();
        }

        #endregion

        [Test]
        public void LogFileViewModel_NoCondition_ReturnStringDate()
        {
            // Arrange
            var date = DateTime.Now;

            LogFileModel logFile = new LogFileModel { DateModified = date.ToString() };
            LogFileViewModel logFileModel = new LogFileViewModel { LogFile = logFile };

            //Act
            var result = logFileModel.LogFile;

            //Assert
            result.DateModified.Should().Be(logFile.DateModified);
        }

        [Test]
        public void IsLogFile_logFileIsFull_ReturnTrue()
        {
            // Arrange
            var date = DateTime.Now;

            LogFileModel logFile = new LogFileModel { DateModified = date.ToString() };
            LogFileViewModel logFileModel = new LogFileViewModel { LogFile = logFile };

            //Act
            var result = logFileModel.IsLogFile;

            //Assert
            result.Should().Be(true);
        }

        [Test]
        public void IsLogFile_logFileIsNull_ReturnFalse()
        {
            // Arrange
            LogFileViewModel logFileModel = new LogFileViewModel { LogFile = null };

            //Act
            var result = logFileModel.IsLogFile;

            //Assert
            result.Should().Be(false);
        }

        [Test]
        public void IsLogFileText_logFileTextIsFull_ReturnTrue()
        {
            // Arrange
            var text = "text";

            LogFileModel logFile = new LogFileModel { Text = text };
            LogFileViewModel logFileModel = new LogFileViewModel { LogFile = logFile };

            //Act
            var result = logFileModel.HasText;

            //Assert
            result.Should().Be(true);
        }

        [Test]
        public void IsLogFileText_logFileTextIsNull_ReturnFalse()
        {
            // Arrange
            LogFileViewModel logFileModel = new LogFileViewModel { LogFile = new LogFileModel { } };

            //Act
            var result = logFileModel.HasText;

            //Assert
            result.Should().Be(false);
        }
    }
}