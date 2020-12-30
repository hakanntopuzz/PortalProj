using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.UnitTests.TestHelpers;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationNugetPackageControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationNugetPackageService> applicationNugetPackageService;

        StrictMock<IApplicationReaderService> applicationReaderService;

        StrictMock<IApplicationNugetPackageViewModelFactory> viewModelFactory;

        StrictMock<IUrlGeneratorService> urlHelper;

        StrictMock<IRouteValueFactory> routeValueFactory;

        StrictMock<INugetPackageService> nugetPackageService;

        ApplicationNugetPackageController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            applicationNugetPackageService = new StrictMock<IApplicationNugetPackageService>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();
            viewModelFactory = new StrictMock<IApplicationNugetPackageViewModelFactory>();
            urlHelper = new StrictMock<IUrlGeneratorService>();
            routeValueFactory = new StrictMock<IRouteValueFactory>();
            nugetPackageService = new StrictMock<INugetPackageService>();

            controller = new ApplicationNugetPackageController(
                userSessionService.Object,
                applicationNugetPackageService.Object,
                applicationReaderService.Object,
                viewModelFactory.Object,
                urlHelper.Object,
                routeValueFactory.Object,
                nugetPackageService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationNugetPackageService.VerifyAll();
            applicationReaderService.VerifyAll();
            viewModelFactory.VerifyAll();
            userSessionService.VerifyAll();
            urlHelper.VerifyAll();
            routeValueFactory.VerifyAll();
        }

        #endregion

        #region index

        [Test]
        public void GetNugetPackages_NoCondition_ReturnJson()
        {
            // Arrange
            var applicationId = 0;
            var resultList = new List<ApplicationNugetPackage>();

            applicationNugetPackageService.Setup(x => x.GetNugetPackages(applicationId)).Returns(resultList);

            // Act
            var result = controller.Index(applicationId);

            // Assert
            var resultModel = result.Should().BeOfType<JsonResult>().Which.Value.As<ClientDataResult>();
            var expectedResult = ClientDataResult.Success(resultList);
            resultModel.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region detail

        [Test]
        public void Detail_ApplicationNugetPackageNotExists_ReturnNugetPackageNotFound()
        {
            //Arrange
            var id = 1;
            ApplicationNugetPackage package = null;

            applicationNugetPackageService.Setup(x => x.GetApplicationNugetPackage(id)).Returns(package);

            var serviceResult = ServiceResult.Error(Messages.ApplicationNugetPackageNotFound);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_ApplicationNugetPackageExists_ReturnApplicationNugetPackageViewModel()
        {
            //Arrange
            var id = 1;
            var package = new ApplicationNugetPackage
            {
                ApplicationId = id
            };
            var nugetPackageRootUrl = new Uri("http://www.example.com/packageUrl/");
            var application = new Application();
            var packageUpdateInfo = new RecordUpdateInfo();
            var viewModel = new ApplicationNugetPackageViewModel();

            applicationNugetPackageService.Setup(x => x.GetApplicationNugetPackage(id)).Returns(package);
            nugetPackageService.Setup(x => x.GetNugetPackageRootUrl()).Returns(nugetPackageRootUrl);
            applicationReaderService.Setup(x => x.GetApplication(package.ApplicationId)).Returns(application);
            viewModelFactory.Setup(x => x.CreateDetailApplicationNugetPackageViewModel(application, package, nugetPackageRootUrl.ToString())).Returns(viewModel);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Detail).ModelAs<ApplicationNugetPackageViewModel>().Should().Be(viewModel);
        }

        #endregion

        #region add

        [Test]
        public void Add_ApplicationDoesNotExist_ReturnErrorMessageAndRedirectToApplicationList()
        {
            // Arrange
            var id = 1;
            Application application = null;

            applicationReaderService.Setup(x => x.GetApplication(id)).Returns(application);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Add(id);

            // Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Add_ApplicationExists_ReturnView()
        {
            //Arrange
            var applicationId = 1;
            var application = new Application();
            var viewModel = new ApplicationNugetPackageViewModel();

            applicationReaderService.Setup(x => x.GetApplication(applicationId)).Returns(application);
            viewModelFactory.Setup(x => x.CreateApplicationNugetPackageViewModel(application)).Returns(viewModel);

            //Act
            var result = controller.Add(applicationId);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Add).ModelAs<ApplicationNugetPackageViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Add_InvalidModel_ReturnView()
        {
            //Arrange
            var viewModel = new ApplicationNugetPackageViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            //Act
            var result = controller.Add(viewModel);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Add).ModelAs<ApplicationNugetPackageViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Add_AddingFails_ReturnView()
        {
            //Arrange
            var userId = 2;
            var viewModel = new ApplicationNugetPackageViewModel
            {
                ApplicationNugetPackage = new ApplicationNugetPackage()
            };
            var serviceResult = ServiceResult.Error("error");

            userSessionService.Setup(s => s.GetCurrentUserId()).Returns(userId);
            applicationNugetPackageService.Setup(x => x.AddApplicationNugetPackage(viewModel.ApplicationNugetPackage)).Returns(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(viewModel);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Add).ModelAs<ApplicationNugetPackageViewModel>().Should().Be(viewModel);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Add_AddingSucceeds_RedirectToApplicationAndSetSuccessResultTempData()
        {
            //Arrange
            var userId = 2;
            var viewModel = new ApplicationNugetPackageViewModel
            {
                ApplicationNugetPackage = new ApplicationNugetPackage
                {
                    ApplicationId = 1
                }
            };

            var serviceResult = ServiceResult.Success();

            userSessionService.Setup(s => s.GetCurrentUserId()).Returns(userId);
            applicationNugetPackageService.Setup(x => x.AddApplicationNugetPackage(viewModel.ApplicationNugetPackage)).Returns(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(viewModel);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Detail).WithControllerName(ControllerNames.Application).WithRouteValue("id", viewModel.ApplicationNugetPackage.ApplicationId);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region edit

        [Test]
        public void Edit_NoCondition_CreateEditViewModelAndReturnAddView()
        {
            // Arrange
            var applicationNugetPackageId = 1;
            var nugetPackageRootUrl = new Uri("http://www.example.com/packageUrl/");
            var application = new Application();
            var applicationNugetPackage = new ApplicationNugetPackage();
            var viewModel = new ApplicationNugetPackageViewModel();

            applicationNugetPackageService.Setup(x => x.GetApplicationNugetPackage(applicationNugetPackageId)).Returns(applicationNugetPackage);
            nugetPackageService.Setup(s => s.GetNugetPackageRootUrl()).Returns(nugetPackageRootUrl);
            applicationReaderService.Setup(s => s.GetApplication(applicationNugetPackage.ApplicationId)).Returns(application);
            viewModelFactory.Setup(x => x.CreateEditApplicationNugetPackageViewModel(application, applicationNugetPackage, nugetPackageRootUrl.ToString())).Returns(viewModel);

            // Act
            var result = controller.Edit(applicationNugetPackageId);

            // Assert
            result.Should().BeViewResult(ViewNames.Edit)
                .ModelAs<ApplicationNugetPackageViewModel>()
                .Should().Be(viewModel);
        }

        [Test]
        public void Edit_ApplicationNugetPackageExists_ReturnView()
        {
            //Arrange
            var applicationNugetPackageId = 1;
            ApplicationNugetPackage applicationNugetPackage = null;

            applicationNugetPackageService.Setup(x => x.GetApplicationNugetPackage(applicationNugetPackageId)).Returns(applicationNugetPackage);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationNugetPackageNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(applicationNugetPackageId);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_InvalidModel_ReturnView()
        {
            //Arrange
            var viewModel = new ApplicationNugetPackageViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Edit).ModelAs<ApplicationNugetPackageViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Edit_UpdatingFails_ReturnView()
        {
            //Arrange
            var userId = 2;
            var viewModel = new ApplicationNugetPackageViewModel
            {
                ApplicationNugetPackage = new ApplicationNugetPackage
                {
                    ApplicationId = 1
                }
            };
            var nugetPackageRootUrl = new Uri("http://www.example.com/packageUrl/");
            var application = new Application();
            var serviceResult = ServiceResult.Error("error");

            userSessionService.Setup(s => s.GetCurrentUserId()).Returns(userId);
            applicationNugetPackageService.Setup(x => x.UpdateApplicationNugetPackage(viewModel.ApplicationNugetPackage)).Returns(serviceResult);
            nugetPackageService.Setup(s => s.GetNugetPackageRootUrl()).Returns(nugetPackageRootUrl);
            applicationReaderService.Setup(s => s.GetApplication(viewModel.ApplicationNugetPackage.ApplicationId)).Returns(application);

            viewModelFactory.Setup(s => s.CreateEditApplicationNugetPackageViewModel(application, viewModel.ApplicationNugetPackage, nugetPackageRootUrl.ToString())).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Edit).ModelAs<ApplicationNugetPackageViewModel>().Should().Be(viewModel);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_UpdatingSucceeds_RedirectToApplicationAndSetSuccessResultTempData()
        {
            //Arrange
            var userId = 2;
            var viewModel = new ApplicationNugetPackageViewModel
            {
                ApplicationNugetPackage = new ApplicationNugetPackage
                {
                    ApplicationId = 1
                }
            };

            var serviceResult = ServiceResult.Success();

            userSessionService.Setup(s => s.GetCurrentUserId()).Returns(userId);
            applicationNugetPackageService.Setup(x => x.UpdateApplicationNugetPackage(viewModel.ApplicationNugetPackage)).Returns(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationNugetPackageControllerActionNames.Detail).WithControllerName(ControllerNames.ApplicationNugetPackage).WithRouteValue("id", viewModel.ApplicationNugetPackage.NugetPackageId);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
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