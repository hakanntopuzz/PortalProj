using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.UnitTests.TestHelpers;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DependencyListControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationDependencyReaderService> applicationDependencyReaderService;

        StrictMock<IDatabaseDependencyService> databaseDependencyService;

        DependencyListController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            applicationDependencyReaderService = new StrictMock<IApplicationDependencyReaderService>();
            databaseDependencyService = new StrictMock<IDatabaseDependencyService>();

            controller = new DependencyListController(
                userSessionService.Object,
                applicationDependencyReaderService.Object,
                databaseDependencyService.Object
                );
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationDependencyReaderService.VerifyAll();
            databaseDependencyService.VerifyAll();
        }

        #endregion

        #region get database dependencies by application id

        [Test]
        public void GetDatabaseDependenciesByApplicationId_Success_ReturnSuccessAndRedirectUrl()
        {
            // Arrange
            var applicationId = 0;
            var resultList = new List<DatabaseDependency>();

            databaseDependencyService.Setup(x => x.GetDatabaseDependenciesByApplicationId(applicationId)).Returns(resultList);

            // Act
            var result = controller.GetDatabaseDependenciesByApplicationId(applicationId);

            // Assert
            var resultModel = result.Should().BeOfType<JsonResult>().Which.Value.As<ClientDataResult>();
            var expectedResult = ClientDataResult.Success(resultList);
            resultModel.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetDatabaseDependenciesByApplicationId_Error_ReturnErrorResult()
        {
            // Arrange
            var applicationId = 0;
            List<DatabaseDependency> resultList = null;

            databaseDependencyService.Setup(x => x.GetDatabaseDependenciesByApplicationId(applicationId)).Returns(resultList);

            // Act
            var result = controller.GetDatabaseDependenciesByApplicationId(applicationId);

            // Assert
            var resultModel = result.Should().BeOfType<JsonResult>().Which.Value.As<ClientDataResult>();
            var expectedResult = ClientDataResult.Error(resultList);
            resultModel.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region get external dependencies by application id

        [Test]
        public void GetExternalDependenciesByApplicationId_DependenciesNotExist_ReturnJson()
        {
            //Arrange
            var applicationId = 1;
            ICollection<ExternalDependency> externalDependencies = null;

            applicationDependencyReaderService.Setup(x => x.GetExternalDependencies(applicationId)).Returns(externalDependencies);

            //Act
            var result = controller.GetExternalDependenciesByApplicationId(applicationId);

            //Assert
            var expectedResult = ClientDataResult.Error(externalDependencies);
            AssertHelpers.AssertClientDataResult(result, expectedResult);
        }

        [Test]
        public void GetExternalDependenciesByApplicationId_DependenciesExist_ReturnJson()
        {
            //Arrange
            var applicationId = 1;
            var externalDependencies = new List<ExternalDependency>();

            applicationDependencyReaderService.Setup(x => x.GetExternalDependencies(applicationId)).Returns(externalDependencies);

            //Act
            var result = controller.GetExternalDependenciesByApplicationId(applicationId);

            //Assert
            var expectedResult = ClientDataResult.Success(externalDependencies);
            AssertHelpers.AssertClientDataResult(result, expectedResult);
        }

        #endregion

        #region get nuget package dependencies by application id

        [Test]
        public void GetNugetPackageDependenciesByApplicationId_DependenciesNotExist_ReturnJson()
        {
            //Arrange
            var applicationId = 1;
            ICollection<NugetPackageDependency> nugetPackageDependencies = null;

            applicationDependencyReaderService.Setup(x => x.GetNugetPackageDependencies(applicationId)).Returns(nugetPackageDependencies);

            //Act
            var result = controller.GetNugetPackageDependenciesByApplicationId(applicationId);

            //Assert
            var expectedResult = ClientDataResult.Error(nugetPackageDependencies);
            AssertHelpers.AssertClientDataResult(result, expectedResult);
        }

        [Test]
        public void GetNugetPackageDependenciesByApplicationId_DependenciesExist_ReturnJson()
        {
            //Arrange
            var applicationId = 1;
            var nugetPackageDependencies = new List<NugetPackageDependency>();

            applicationDependencyReaderService.Setup(x => x.GetNugetPackageDependencies(applicationId)).Returns(nugetPackageDependencies);

            //Act
            var result = controller.GetNugetPackageDependenciesByApplicationId(applicationId);

            //Assert
            var expectedResult = ClientDataResult.Success(nugetPackageDependencies);
            AssertHelpers.AssertClientDataResult(result, expectedResult);
        }

        #endregion

        #region helpers

        public void SetResultMessageTempData(Dictionary<string, string> tempDataKeyValuePairs)
        {
            controller.TempData = SetupHelpers.CreateResultMessageTempData(tempDataKeyValuePairs);
        }

        #endregion
    }
}