using AB.Framework.UnitTests;
using DevPortal.NugetManager.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.UnitTests.NugetManagerTests.ModelTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class NugetPackageFolderTests : LooseBaseTestFixture
    {
        NugetPackageFolder nugetPackageFolder;

        [Test]
        public void FilePath_NoCondition_ReturnFilePath()
        {
            //Arrange
            nugetPackageFolder = new NugetPackageFolder
            {
                Path = @"\Path\"
            };

            //Act
            var result = nugetPackageFolder.FilePath;

            //Assert
            result.Should().Be("/Path/");
        }

        [Test]
        public void Path_NoCondition_ReturnPath()
        {
            //Arrange
            nugetPackageFolder = new NugetPackageFolder
            {
                Path = @"\Path\"
            };

            //Act
            var result = nugetPackageFolder.Path;

            //Assert
            result.Should().Be(nugetPackageFolder.Path);
        }

        [Test]
        public void Name_NoCondition_ReturnName()
        {
            //Arrange
            nugetPackageFolder = new NugetPackageFolder
            {
                Path = @"\Path\a"
            };

            //Act
            var result = nugetPackageFolder.Name;

            //Assert
            result.Should().Be("a");
        }

        [Test]
        public void DateModified_NoCondition_ReturnDateModified()
        {
            //Arrange
            nugetPackageFolder = new NugetPackageFolder
            {
                DateModified = DateTime.Now
            };

            //Act
            var result = nugetPackageFolder.DateModified;

            //Assert
            result.Year.Should().Be(DateTime.Now.Year);
        }

        [Test]
        public void CreationTime_NoCondition_ReturnCreationTime()
        {
            //Arrange
            nugetPackageFolder = new NugetPackageFolder
            {
                CreationTime = DateTime.Now
            };

            //Act
            var result = nugetPackageFolder.CreationTime;

            //Assert
            result.Year.Should().Be(DateTime.Now.Year);
        }

        [Test]
        public void VersionHistories_NoCondition_ReturnStringList()
        {
            //Arrange
            List<string> list = new List<string> { "a", "b" };
            nugetPackageFolder = new NugetPackageFolder
            {
                SubDirectory = list
            };

            //Act
            var result = nugetPackageFolder.SubDirectory;

            //Assert
            result.Count().Should().Be(list.Count);
        }
    }
}