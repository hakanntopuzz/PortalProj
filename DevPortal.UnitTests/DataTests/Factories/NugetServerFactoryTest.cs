using AB.Framework.UnitTests;
using DevPortal.Data;
using DevPortal.Framework.Abstract;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.DataTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class NugetServerFactoryTest : BaseTestFixture
    {
        #region members & setup

        StrictMock<IFileSystem> fileSystem;

        NugetServerFactory factory;

        [SetUp]
        public void Initialize()
        {
            fileSystem = new StrictMock<IFileSystem>();
            factory = new NugetServerFactory(fileSystem.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            fileSystem.VerifyAll();
        }

        #endregion

        [Test]
        public void CreateOldNugetPackages_NoCondition_ReturnNugetPackageFolders()
        {
            //Arrange
            List<string> directory = new List<string> { "path1" };
            DateTime dateTime = DateTime.Now;
            fileSystem.Setup(x => x.GetFileLastModified(directory[0])).Returns(dateTime);
            fileSystem.Setup(x => x.GetFileCreationTime(directory[0])).Returns(dateTime);

            //Act
            var result = factory.CreateNugetPackageFolders(directory);

            //Assert
            result.Count.Should().Be(1);
            result[0].FilePath.Should().Be(directory[0]);
        }

        [Test]
        public void CreateNugetPackageFolders_NoCondition_ReturnEmptyNugetPackageFolders()
        {
            //Arrange

            //Act
            var result = factory.CreateNugetPackageFolders();

            //Assert
            result.Should().BeNullOrEmpty();
        }

        [Test]
        public void GetOldNugetPackages_NoCondition_ReturnStringList()
        {
            //Arrange
            List<string> directory = new List<string> { "path1" };
            var expectedPath = "path";
            fileSystem.Setup(x => x.GetNameFromPath(directory[0])).Returns(expectedPath);

            //Act
            var result = factory.GetSubDirectoryFoldersName(directory);

            //Assert
            result.Count.Should().Be(1);
            result[0].Should().Be(expectedPath);
        }

        [Test]
        public void CreateEmptyStringList_NoCondition_ReturnEmptyStringList()
        {
            //Arrange

            //Act
            var result = factory.CreateEmptyStringList();

            //Assert
            result.Should().BeNullOrEmpty();
            result.Count.Should().Be(0);
        }
    }
}