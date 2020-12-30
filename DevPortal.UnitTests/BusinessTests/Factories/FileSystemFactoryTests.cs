using AB.Framework.UnitTests;
using DevPortal.Business.Factories;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class FileSystemFactoryTests : LooseBaseTestFixture
    {
        #region members & setup

        FileSystemFactory factory;

        [SetUp]
        public void Initialize()
        {
            factory = new FileSystemFactory();
        }

        #endregion

        #region CreateLogFile

        [Test]
        public void CreateLogFile_NoCondition_ReturnFileModel()
        {
            // Arrange
            string path = "path";
            var dateModified = DateTime.Now;
            string size = "123";
            string text = "text";
            LogFileModel fileModel = new LogFileModel
            {
                DateModified = dateModified.ToString("yyyy-MM-dd HH:mm:ss"),
                Path = path.Replace(@"\", "/"),
                Size = size,
                Text = text
            };

            // Act
            var result = factory.CreateLogFile(path, dateModified, size, text);

            // Assert
            result.Should().BeEquivalentTo(fileModel);
            result.Should().NotBeNull();
        }

        #endregion

        #region GetDownloadFileContents

        [Test]
        public void GetDownloadFileContents_NoCondition_ReturnFileModel()
        {
            // Arrange
            string path = "path";
            byte[] content = new byte[0];
            LogFileModel fileModel = new LogFileModel
            {
                FileContent = content,
                Path = path
            };

            // Act
            var result = factory.GetDownloadFileContents(path, content);

            // Assert
            result.Should().BeEquivalentTo(fileModel);
            result.Should().NotBeNull();
        }

        #endregion

        #region CreateLogFiles

        [Test]
        public void CreateLogFiles_NoCondition_ReturnFileModelsButEmpty()
        {
            // Arrange
            List<LogFileModel> fileModels = new List<LogFileModel>();

            // Act
            var result = factory.CreateLogFiles();

            // Assert
            result.Count.Should().Be(fileModels.Count);
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        #endregion

        #region CreateLogFile

        [Test]
        public void CreateLogFile_HaveNotText_ReturnFileModel()
        {
            // Arrange
            string path = "path";
            var dateModified = DateTime.Now;
            string size = "123";
            LogFileModel fileModel = new LogFileModel
            {
                DateModified = dateModified.ToString("yyyy-MM-dd HH:mm:ss"),
                Path = path.Replace(@"\", "/"),
                Size = size
            };

            // Act
            var result = factory.CreateLogFile(path, dateModified, size);

            // Assert
            result.Should().BeEquivalentTo(fileModel);
            result.Should().NotBeNull();
        }

        #endregion
    }
}