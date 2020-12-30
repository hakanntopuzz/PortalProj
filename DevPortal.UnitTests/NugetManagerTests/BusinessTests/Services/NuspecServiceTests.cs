using AB.Framework.UnitTests;
using DevPortal.Data.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.NugetManager.Business.Services;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.NugetManagerTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class NuspecServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<INugetServerRepository> nugetServerRepository;

        StrictMock<IFileService> fileService;

        NuspecService service;

        [SetUp]
        public void Initialize()
        {
            nugetServerRepository = new StrictMock<INugetServerRepository>();
            fileService = new StrictMock<IFileService>();

            service = new NuspecService(
                nugetServerRepository.Object,
                fileService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            nugetServerRepository.VerifyAll();
            fileService.VerifyAll();
        }

        #endregion

        #region get nuspec file contents

        [Test]
        public void GetNuspecFileContents_DoesntExistFileInList_ReturnNull()
        {
            // Arrange
            var filePath = "path";
            var fileName = "fileName";
            var list = new List<string> { };

            nugetServerRepository.Setup(x => x.GetFileList(filePath)).Returns(list);
            fileService.Setup(x => x.FilePathListContainsFile(fileName, list)).Returns(false);

            //Act
            var result = service.GetNuspecFileContents(filePath, fileName);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void GetNuspecFileContents_DoesExistFileInList_ReturnFileModel()
        {
            // Arrange
            var filePath = "path";
            var fileName = "fileName";
            var list = new List<string> { fileName };
            var fileModel = new LogFileModel();
            var itemNumber = 0;

            nugetServerRepository.Setup(x => x.GetFileList(filePath)).Returns(list);
            fileService.Setup(x => x.FilePathListContainsFile(fileName, list)).Returns(true);
            fileService.Setup(x => x.GetFileIndexInFilePathList(list, fileName)).Returns(itemNumber);
            nugetServerRepository.Setup(x => x.GetDownloadFileContents(filePath + list[itemNumber])).Returns(fileModel);

            //Act
            var result = service.GetNuspecFileContents(filePath, fileName);

            // Assert
            result.Should().Be(fileModel);
        }

        #endregion
    }
}