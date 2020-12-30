using AB.Framework.UnitTests;
using DevPortal.NugetManager.Business;
using DevPortal.NugetManager.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.NugetManagerTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class NugetQueryServiceTest : BaseTestFixture
    {
        #region members & setup

        NugetQueryService service;

        [SetUp]
        public void Initialize()
        {
            service = new NugetQueryService();
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
        }

        #endregion

        [Test]
        public void GetGroupedNugetPackages_NoCondition_ReturnNugetPackageList()
        {
            // Arrange
            DateTime thisDate1 = new DateTime(2011, 6, 10);
            var nugetProperties = new NugetProperties
            {
                Version = "version",
                Description = "description",
                Published = thisDate1,
                Tags = "tags",
                IconUrl = "IconUrl"
            };
            var pack = new NugetPackage { Title = "title", Properties = nugetProperties };
            var packages = new List<NugetPackage> { pack, pack };
            var expectednugetProperties = new NugetProperties
            {
                Version = "version",
                Description = "description",
                Published = thisDate1,
                Tags = "tags",
                IconUrl = "IconUrl"
            };
            var expectedpack = new NugetPackage { Title = "title", Properties = expectednugetProperties };
            var expectedPackageList = new List<NugetPackage> { expectedpack };

            //Act
            var result = service.GetGroupedNugetPackages(packages);

            // Assert
            result.Should().BeEquivalentTo(expectedPackageList);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void OrderPackages_NoCondition_ReturnNugetPackageList(int order)
        {
            // Arrange
            var orderBy = order;
            DateTime thisDate1 = new DateTime(2011, 6, 10);
            var nugetProperties = new NugetProperties
            {
                Version = "version",
                Description = "description",
                Published = thisDate1,
                Tags = "tags",
                IconUrl = "IconUrl",
                Archive = false
            };
            var pack = new NugetPackage { Title = "title", Properties = nugetProperties };
            var packages = new List<NugetPackage> { pack };
            var expectedPackageList = new List<NugetPackage> { pack };

            //Act
            var result = service.OrderPackages(packages, orderBy);

            // Assert
            result.Should().BeEquivalentTo(expectedPackageList);
        }

        [Test]
        public void SearchByTagOrTitle_NoCondition_ReturnNugetPackageList()
        {
            // Arrange
            var searchString = "title";
            DateTime thisDate1 = new DateTime(2011, 6, 10);
            var nugetProperties = new NugetProperties
            {
                Version = "version",
                Description = "description",
                Published = thisDate1,
                Tags = "tags",
                IconUrl = "IconUrl",
                Archive = false
            };
            var pack = new NugetPackage { Title = "title", Properties = nugetProperties };
            var packages = new List<NugetPackage> { pack };
            var expectedPackageList = new List<NugetPackage> { pack };

            //Act
            var result = service.SearchByTagOrTitle(packages, searchString);

            // Assert
            result.Should().BeEquivalentTo(expectedPackageList);
        }

        [Test]
        public void SearchByTagOrTitle_NullSearchString_ReturnNugetPackageList()
        {
            // Arrange
            var searchString = "";
            DateTime thisDate1 = new DateTime(2011, 6, 10);
            var nugetProperties = new NugetProperties
            {
                Version = "version",
                Description = "description",
                Published = thisDate1,
                Tags = "tags",
                IconUrl = "IconUrl",
                Archive = false
            };
            var pack = new NugetPackage { Title = "title", Properties = nugetProperties };
            var packages = new List<NugetPackage> { pack };
            var expectedPackageList = new List<NugetPackage> { pack };

            //Act
            var result = service.SearchByTagOrTitle(packages, searchString);

            // Assert
            result.Should().BeEquivalentTo(expectedPackageList);
        }
    }
}