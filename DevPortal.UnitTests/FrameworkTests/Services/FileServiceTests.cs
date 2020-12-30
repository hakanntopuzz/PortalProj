using AB.Framework.UnitTests;
using DevPortal.Framework.Services;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.FrameworkTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class FileServiceTests : BaseTestFixture
    {
        #region members & setup

        FileService service;

        [SetUp]
        public void Initialize()
        {
            service = new FileService();
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
        }

        #endregion

        #region FilePathListContainsFile

        [Test]
        public void FilePathListContainsFile_InvalidFileList_ReturnFalse()
        {
            // Arrange
            var fileName = "fileName";
            List<string> filePathList = null;

            // Act
            var result = service.FilePathListContainsFile(fileName, filePathList);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void FilePathListContainsFile_DoesNotContainFile_ReturnFalse()
        {
            // Arrange
            var fileName = "fileName";
            var filePathList = new List<string>();

            // Act
            var result = service.FilePathListContainsFile(fileName, filePathList);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void FilePathListContainsFile_ContainsFile_ReturnTrue()
        {
            // Arrange
            var fileName = "fileName";
            var filePathList = new List<string> { fileName };

            // Act
            var result = service.FilePathListContainsFile(fileName, filePathList);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region GetFileIndexInFilePathList

        [Test]
        public void GetFileIndexInFilePathList_InvalidList_ReturnZero()
        {
            // Arrange
            var fileName = "fileName";
            List<string> filePathList = null;

            var expectedFileIndex = 0;

            // Act
            var result = service.GetFileIndexInFilePathList(filePathList, fileName);

            // Assert
            result.Should().Be(expectedFileIndex);
        }

        [Test]
        public void GetFileIndexInFilePathList_ListContainsFile_ReturnFileIndex()
        {
            // Arrange
            var fileName = "fileName";
            var filePathList = new List<string> {
                "test",
                fileName
            };

            var expectedFileIndex = 1;

            // Act
            var result = service.GetFileIndexInFilePathList(filePathList, fileName);

            // Assert
            result.Should().Be(expectedFileIndex);
        }

        [Test]
        public void GetFileIndexInFilePathList_ListDoesNotContainFile_ReturnZero()
        {
            // Arrange
            var fileName = "fileName";
            var file = "file";
            var filePathList = new List<string> { file };

            var expectedFileIndex = 0;

            // Act
            var result = service.GetFileIndexInFilePathList(filePathList, fileName);

            // Assert
            result.Should().Be(expectedFileIndex);
        }

        #endregion
    }
}