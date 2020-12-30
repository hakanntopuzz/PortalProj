using AB.Framework.UnitTests;
using DevPortal.NugetManager.Business;
using DevPortal.NugetManager.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.NugetManagerTests.BusinessTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class NugetFactoryTests : LooseBaseTestFixture
    {
        #region members & setup

        NugetFactory factory;

        [SetUp]
        public void Initialize()
        {
            factory = new NugetFactory();
        }

        #endregion

        #region CreateLogFile

        [Test]
        public void CreateNugetPackageModel_NoCondition_ReturnNugetPackageList()
        {
            // Arrange
            var nugetPackageList = new List<NugetPackage> { };

            //Act
            var result = factory.CreateNugetPackageModel();

            // Assert
            result.GetType().Should().Be(nugetPackageList.GetType());
        }

        #endregion

        #region create filtered nuget packages model

        [Test]
        public void CreateFilteredNugetPackageModel_NoCondition_ReturnFilteredNugetPackageListModel()
        {
            // Arrange
            int totalCount = 0;
            var filteredPackages = new List<NugetPackage>();

            // Act
            var result = factory.CreateFilteredNugetPackagesModel(totalCount, filteredPackages);

            // Assert
            var expectedResult = new FilteredNugetPackages
            {
                TotalCount = totalCount,
                NugetPackages = filteredPackages
            };
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion
    }
}