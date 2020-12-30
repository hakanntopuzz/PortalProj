using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.NugetManager.Model;
using DevPortal.Web.Library;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class NugetPackageViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        NugetPackageViewModelFactory factory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new NugetPackageViewModelFactory(authorizationServiceWrapper.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            authorizationServiceWrapper.VerifyAll();
        }

        #endregion

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CreateNugetPackageFolderViewModel_NoCondition_ReturnModel(bool isAdmin)
        {
            // Arrange
            var nugetPackageFolders = new List<NugetPackageFolder>();

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminPolicy()).Returns(isAdmin);

            // Act
            var result = factory.CreateNugetPackageFolderViewModel(nugetPackageFolders);

            // Assert
            result.NugetPackageFolders.Should().BeEquivalentTo(nugetPackageFolders);
            result.IsAuthorized.Should().Be(isAdmin);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CreateNugetPackageViewModel_NoCondition_ReturnModel(bool isAdmin)
        {
            // Arrange
            var nugetPackages = new List<NugetPackage>();

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminPolicy()).Returns(isAdmin);

            // Act
            var result = factory.CreateNugetPackageViewModel(nugetPackages);

            // Assert
            result.NugetPackages.Should().BeEquivalentTo(nugetPackages);
            result.IsAuthorized.Should().Be(isAdmin);
        }
    }
}