using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Abstract.Services;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.NugetManager.Business.Abstract;
using DevPortal.NugetManager.Model;
using DevPortal.UnitTests.TestHelpers;
using DevPortal.Web.Library.Abstract.Factories;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class NugetPackageDependencyControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<INugetPackageDependencyService> nugetPackageDependencyService;

        StrictMock<INugetPackageDependencyViewModelFactory> nugetPackageDependencyViewModelFactory;

        StrictMock<INugetService> nugetService;

        StrictMock<INugetPackageService> nugetPackageService;

        StrictMock<IApplicationReaderService> applicationReaderService;

        StrictMock<IUrlGeneratorService> urlGeneratorService;

        StrictMock<IRouteValueFactory> routeValueFactory;

        StrictMock<IUserSessionService> userSessionService;

        NugetPackageDependencyController controller;

        [SetUp]
        public void Initialize()
        {
            nugetPackageDependencyService = new StrictMock<INugetPackageDependencyService>();
            nugetPackageDependencyViewModelFactory = new StrictMock<INugetPackageDependencyViewModelFactory>();
            nugetService = new StrictMock<INugetService>();
            nugetPackageService = new StrictMock<INugetPackageService>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();
            urlGeneratorService = new StrictMock<IUrlGeneratorService>();
            routeValueFactory = new StrictMock<IRouteValueFactory>();
            userSessionService = new StrictMock<IUserSessionService>();

            controller = new NugetPackageDependencyController(
                nugetService.Object,
                userSessionService.Object,
                nugetPackageDependencyService.Object,
                nugetPackageDependencyViewModelFactory.Object,
                nugetPackageService.Object,
                applicationReaderService.Object,
                urlGeneratorService.Object,
                routeValueFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            nugetService.VerifyAll();
            userSessionService.VerifyAll();
            nugetPackageDependencyService.VerifyAll();
            nugetPackageDependencyViewModelFactory.VerifyAll();
            nugetPackageService.VerifyAll();
            applicationReaderService.VerifyAll();
            urlGeneratorService.VerifyAll();
            routeValueFactory.VerifyAll();
        }

        #endregion

        #region helpers

        public void SetResultMessageTempData(Dictionary<string, string> tempDataKeyValuePairs)
        {
            controller.TempData = SetupHelpers.CreateResultMessageTempData(tempDataKeyValuePairs);
        }

        #endregion

        #region add- get

        [Test]
        public void Add_NoCondition_ReturnDatabaseViewModel()
        {
            //Arrange
            var applicationId = 1;
            var application = new Application();
            var nugetPackageDependencyViewModel = new NugetPackageDependencyViewModel();

            applicationReaderService.Setup(x => x.GetApplication(applicationId)).Returns(application);
            nugetPackageDependencyViewModelFactory.Setup(x => x.CreateNugetPackageDependencyViewModelAddView(application)).Returns(nugetPackageDependencyViewModel);

            //Act
            var result = controller.Add(applicationId);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<NugetPackageDependencyViewModel>().Should().Be(nugetPackageDependencyViewModel);
        }

        #endregion

        #region add - post

        [Test]
        public void Add_InvalidModel_ReturnDatabaseViewModel()
        {
            //Arrange
            var nugetPackageDependency = new NugetPackageDependency();
            var application = new Application();
            var nugetPackageDependencyViewModel = new NugetPackageDependencyViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            applicationReaderService.Setup(x => x.GetApplication(nugetPackageDependency.DependentApplicationId)).Returns(application);
            nugetPackageDependencyViewModelFactory.Setup(x => x.CreateNugetPackageDependencyViewModelAddView(application)).Returns(nugetPackageDependencyViewModel);

            //Act
            var result = controller.Add(nugetPackageDependency);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<NugetPackageDependencyViewModel>().Should().Be(nugetPackageDependencyViewModel);
        }

        [Test]
        public void Add_AddFails_ReturnDatabaseViewModel()
        {
            //Arrange
            var userId = 1;
            var recordUpdateInfo = new RecordUpdateInfo
            {
                CreatedBy = userId,
                ModifiedBy = userId
            };
            var nugetPackageDependency = new NugetPackageDependency
            {
                RecordUpdateInfo = recordUpdateInfo
            };
            var application = new Application();
            var nugetPackageDependencyViewModel = new NugetPackageDependencyViewModel();

            var addResult = ServiceResult.Error("Error");

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            nugetPackageDependencyService.Setup(x => x.AddNugetPackageDependency(nugetPackageDependency)).Returns(addResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            applicationReaderService.Setup(x => x.GetApplication(nugetPackageDependency.DependentApplicationId)).Returns(application);
            nugetPackageDependencyViewModelFactory.Setup(x => x.CreateNugetPackageDependencyViewModelAddView(application)).Returns(nugetPackageDependencyViewModel);

            //Act
            var result = controller.Add(nugetPackageDependency);

            //Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<NugetPackageDependencyViewModel>().Should().Be(nugetPackageDependencyViewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public void Add_AddSuccess_ReturnDatabaseViewModel()
        {
            //Arrange
            var userId = 1;
            var applicationId = 1;
            var redirectToUrl = "redirectToUrl";
            var recordUpdateInfo = new RecordUpdateInfo
            {
                CreatedBy = userId,
                ModifiedBy = userId
            };
            var nugetPackageDependency = new NugetPackageDependency
            {
                RecordUpdateInfo = recordUpdateInfo,
                DependentApplicationId = applicationId
            };
            var application = new Application();
            var nugetPackageDependencyViewModel = new NugetPackageDependencyViewModel();
            var addResult = ServiceResult.Success("Success");

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            nugetPackageDependencyService.Setup(x => x.AddNugetPackageDependency(nugetPackageDependency)).Returns(addResult);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, addResult.Message);
            SetResultMessageTempData(expectedTempData);
            urlGeneratorService.Setup(x => x.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, applicationId)).Returns(redirectToUrl);

            //Act
            var result = controller.Add(nugetPackageDependency);

            //Assert
            result.Should().BeRedirectResult().WithUrl(redirectToUrl);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        #endregion

        #region detail

        [Test]
        public void Detail_NugetPackageDependencyIdLessThanZero_ReturnNugetPackageDependencyNotFound()
        {
            //Arrange
            var id = -1;

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.NugetPackageDependencyNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_NugetPackageDependencyNotExists_ReturnNugetPackageDependencyNotFound()
        {
            //Arrange
            var id = 1;
            NugetPackageDependency nugetPackageDependency = null;

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.NugetPackageDependencyNotFound);
            SetResultMessageTempData(expectedTempData);
            nugetPackageDependencyService.Setup(x => x.GetNugetPackageDependencyById(id)).Returns(nugetPackageDependency);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_NugetPackageDependencyExists_ReturnNugetPackageDependencyViewModel()
        {
            //Arrange
            var id = 1;
            var nugetPackageDependency = new NugetPackageDependency();
            var viewModel = new NugetPackageDependencyViewModel();

            nugetPackageDependencyService.Setup(x => x.GetNugetPackageDependencyById(id)).Returns(nugetPackageDependency);
            nugetPackageDependencyViewModelFactory.Setup(x => x.CreateNugetPackageDependencyViewModel(nugetPackageDependency)).Returns(viewModel);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Detail).ModelAs<NugetPackageDependencyViewModel>().Should().Be(viewModel);
        }

        #endregion

        #region delete

        [Test]
        public void Delete_Error_ReturnErrorResult()
        {
            //Arrange
            var id = 1;
            var applicationId = 1;
            var serviceResult = ServiceResult.Error("Error");
            var routeValueDictionary = new RouteValueDictionary();
            var redirectUrl = "redirectUrl";

            nugetPackageDependencyService.Setup(x => x.DeleteNugetPackageDependency(id)).Returns(serviceResult);
            routeValueFactory.Setup(x => x.CreateRouteValuesForGenerateUrl(applicationId)).Returns(routeValueDictionary);
            urlGeneratorService.Setup(x => x.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, routeValueDictionary)).Returns(redirectUrl);

            //Act
            var result = controller.Delete(id, applicationId);

            //Assert
            var expectedResult = RedirectableClientActionResult.Error(redirectUrl);
            AssertHelpers.AssertRedirectableClientActionResult(result, expectedResult);
        }

        [Test]
        public void Delete_Success_ReturnSuccessAndRedirectUrl()
        {
            //Arrange
            //Arrange
            var id = 1;
            var applicationId = 1;
            var serviceResult = ServiceResult.Success("Success");
            var routeValueDictionary = new RouteValueDictionary();
            var redirectUrl = "redirectUrl";
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            nugetPackageDependencyService.Setup(x => x.DeleteNugetPackageDependency(id)).Returns(serviceResult);
            routeValueFactory.Setup(x => x.CreateRouteValuesForGenerateUrl(applicationId)).Returns(routeValueDictionary);
            urlGeneratorService.Setup(x => x.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, routeValueDictionary)).Returns(redirectUrl);

            //Act
            var result = controller.Delete(id, applicationId);

            //Assert
            var expectedResult = RedirectableClientActionResult.Success(redirectUrl);
            AssertHelpers.AssertRedirectableClientActionResult(result, expectedResult);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        [Test]
        public void GetFilteredNugetPackages_NoCondition_ReturnNugetPackageList()
        {
            //Arrange
            var nugetPackageList = new List<NugetPackage>();

            nugetService.Setup(x => x.GetGroupedNugetPackages()).Returns(nugetPackageList);

            //Act
            var result = controller.GetFilteredNugetPackages();

            //Assert
            result.Should().BeSameAs(nugetPackageList);
        }

        [Test]
        public void GetNugetPackageRootUrl_NoCondition_ReturnNugetPackageRootUrl()
        {
            //Arrange
            var rootUrl = new Uri("http://www.example.com/packageUrl/");

            nugetPackageService.Setup(x => x.GetNugetPackageRootUrl()).Returns(rootUrl);

            //Act
            var result = controller.GetNugetPackageRootUrl();

            //Assert
            result.Should().BeContentResult().WithContent(rootUrl.ToString());
        }
    }
}