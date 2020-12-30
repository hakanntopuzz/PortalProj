using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class NugetPackageDependencyServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<INugetPackageDependencyRepository> nugetPackageDependencyRepository;

        StrictMock<INugetPackageService> nugetPackageService;

        NugetPackageDependencyService service;

        [SetUp]
        public void Initialize()
        {
            nugetPackageDependencyRepository = new StrictMock<INugetPackageDependencyRepository>();
            nugetPackageService = new StrictMock<INugetPackageService>();

            service = new NugetPackageDependencyService(
                nugetPackageDependencyRepository.Object,
                nugetPackageService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            nugetPackageDependencyRepository.VerifyAll();
            nugetPackageService.VerifyAll();
        }

        #endregion

        #region  Get Nuget Package Dependency By Id

        [Test]
        public void GetNugetPackageDependencyById_GetNugetPackageDependencyByIdNull_ReturnNugetPackageDependency()
        {
            // Arrange
            var id = 1;
            NugetPackageDependency nugetPackageDependency = null;

            nugetPackageDependencyRepository.Setup(x => x.GetNugetPackageDependencyById(id)).Returns(nugetPackageDependency);

            // Act
            var result = service.GetNugetPackageDependencyById(id);

            // Assert
            result.Should().Be(nugetPackageDependency);
        }

        [Test]
        public void GetNugetPackageDependencyById_NoCondition_ReturnNugetPackageDependency()
        {
            // Arrange
            var id = 1;
            var packageUrl = new Uri("http://www.example.com/packageUrl/");
            var nugetPackageDependency = new NugetPackageDependency
            {
                NugetPackageName = "nugetPackageName"
            };
            var recordUpdateInfo = new RecordUpdateInfo
            {
                CreatedBy = 18,
                ModifiedBy = 18,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };
            nugetPackageDependencyRepository.Setup(x => x.GetNugetPackageDependencyById(id)).Returns(nugetPackageDependency);
            nugetPackageService.Setup(x => x.GetNugetPackageUrl(nugetPackageDependency.NugetPackageName)).Returns(packageUrl);
            nugetPackageDependencyRepository.Setup(x => x.GetNugetPackageDependencyUpdateInfo(id)).Returns(recordUpdateInfo);

            // Act
            var result = service.GetNugetPackageDependencyById(id);

            // Assert
            result.Should().Be(nugetPackageDependency);
            result.PackageUrl.Should().Be(packageUrl.ToString());
            result.RecordUpdateInfo.Should().Be(recordUpdateInfo);
        }

        #endregion

        #region  Get Nuget Package Dependency By Id

        [Test]
        public void AddNugetPackageDependency_Success_ReturnServiceResult()
        {
            // Arrange
            var nugetPackageDependency = new NugetPackageDependency
            {
                NugetPackageName = "nugetPackageName"
            };

            var expectedResult = ServiceResult.Success(Messages.NugetPackageDependencyCreated);

            nugetPackageDependencyRepository.Setup(x => x.AddNugetPackageDependency(nugetPackageDependency)).Returns(true);

            // Act
            var result = service.AddNugetPackageDependency(nugetPackageDependency);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void AddNugetPackageDependency_Error_ReturnServiceResult()
        {
            // Arrange
            var nugetPackageDependency = new NugetPackageDependency
            {
                NugetPackageName = "nugetPackageName"
            };

            var expectedResult = ServiceResult.Error(Messages.AddingFails);

            nugetPackageDependencyRepository.Setup(x => x.AddNugetPackageDependency(nugetPackageDependency)).Returns(false);

            // Act
            var result = service.AddNugetPackageDependency(nugetPackageDependency);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region  Get Nuget Package Dependency By Id

        [Test]
        public void DeleteNugetPackageDependency_Success_ReturnServiceResult()
        {
            // Arrange
            var nugetPackageDependencyId = 1;

            var expectedResult = ServiceResult.Success(Messages.NugetPackageDependencyDeleted);

            nugetPackageDependencyRepository.Setup(x => x.DeleteNugetPackageDependency(nugetPackageDependencyId)).Returns(true);

            // Act
            var result = service.DeleteNugetPackageDependency(nugetPackageDependencyId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void DeleteNugetPackageDependency_Error_ReturnServiceResult()
        {
            // Arrange
            var nugetPackageDependencyId = 1;

            var expectedResult = ServiceResult.Error(Messages.DeleteFails);

            nugetPackageDependencyRepository.Setup(x => x.DeleteNugetPackageDependency(nugetPackageDependencyId)).Returns(false);

            // Act
            var result = service.DeleteNugetPackageDependency(nugetPackageDependencyId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion
    }
}