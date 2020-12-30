using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Abstract.Services;
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
    public class ApplicationJenkinsJobControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationJenkinsJobService> applicationJenkinsJobService;

        StrictMock<IApplicationReaderService> applicationReaderService;

        StrictMock<IGeneralSettingsService> generalSettingsService;

        StrictMock<IApplicationJenkinsJobViewModelFactory> viewModelFactory;

        StrictMock<IUrlGeneratorService> urlGeneratorService;

        StrictMock<IRouteValueFactory> routeValueFactory;

        ApplicationJenkinsJobController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            applicationJenkinsJobService = new StrictMock<IApplicationJenkinsJobService>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();
            generalSettingsService = new StrictMock<IGeneralSettingsService>();
            viewModelFactory = new StrictMock<IApplicationJenkinsJobViewModelFactory>();
            urlGeneratorService = new StrictMock<IUrlGeneratorService>();
            routeValueFactory = new StrictMock<IRouteValueFactory>();

            controller = new ApplicationJenkinsJobController(
                userSessionService.Object,
                applicationJenkinsJobService.Object,
                applicationReaderService.Object,
                generalSettingsService.Object,
                viewModelFactory.Object,
                urlGeneratorService.Object,
                routeValueFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationJenkinsJobService.VerifyAll();
            applicationReaderService.VerifyAll();
            generalSettingsService.VerifyAll();
            viewModelFactory.VerifyAll();
            urlGeneratorService.VerifyAll();
            routeValueFactory.VerifyAll();
            userSessionService.VerifyAll();
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
            var jenkinsJobTypes = new List<JenkinsJobType>();
            var jenkinsJobUrl = new Uri("http://jenkinsUrl.activebuilder.com/job");
            var applicationJenkinsJobViewModel = new ApplicationJenkinsJobViewModel();

            applicationReaderService.Setup(x => x.GetApplication(applicationId)).Returns(application);
            applicationJenkinsJobService.Setup(x => x.GetJenkinsJobTypes()).Returns(jenkinsJobTypes);
            generalSettingsService.Setup(x => x.GetJenkinsJobUrl()).Returns(jenkinsJobUrl);
            viewModelFactory.Setup(x => x.CreateApplicationJenkinsJobViewModel(application, jenkinsJobTypes, jenkinsJobUrl.ToString())).Returns(applicationJenkinsJobViewModel);

            //Act
            var result = controller.Add(applicationId);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Add).ModelAs<ApplicationJenkinsJobViewModel>().Should().Be(applicationJenkinsJobViewModel);
        }

        [Test]
        public void Add_InvalidModel_ReturnView()
        {
            //Arrange
            var viewModel = new ApplicationJenkinsJobViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            //Act
            var result = controller.Add(viewModel);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Add).ModelAs<ApplicationJenkinsJobViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Add_AddingFails_ReturnView()
        {
            //Arrange
            var viewModel = new ApplicationJenkinsJobViewModel
            {
                ApplicationJenkinsJob = new JenkinsJob()
            };
            int userId = 45;
            var serviceResult = ServiceResult.Error("error");

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationJenkinsJobService.Setup(x => x.AddApplicationJenkinsJob(viewModel.ApplicationJenkinsJob)).Returns(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(viewModel);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Add).ModelAs<ApplicationJenkinsJobViewModel>().Should().Be(viewModel);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Add_AddingSucceeds_RedirectToApplicationAndSetSuccessResultTempData()
        {
            //Arrange
            var viewModel = new ApplicationJenkinsJobViewModel
            {
                ApplicationJenkinsJob = new JenkinsJob
                {
                    ApplicationId = 1
                }
            };

            int userId = 45;
            var serviceResult = ServiceResult.Success();

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationJenkinsJobService.Setup(x => x.AddApplicationJenkinsJob(viewModel.ApplicationJenkinsJob)).Returns(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(viewModel);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Detail).WithControllerName(ControllerNames.Application).WithRouteValue("id", viewModel.ApplicationJenkinsJob.ApplicationId);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region detail

        [Test]
        public void Detail_ApplicationJenkinsJobNotExists_ReturnDetailErrorView()
        {
            //Arrange
            var id = 1;
            JenkinsJob jenkinsJob = null;

            applicationJenkinsJobService.Setup(x => x.GetApplicationJenkinsJob(id)).Returns(jenkinsJob);

            var serviceResult = ServiceResult.Error(Messages.ApplicationJenkinsJobNotFound);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_ApplicationJenkinsJobExists_ReturnDetailSuccessView()
        {
            //Arrange
            var id = 1;
            var jenkinsJob = new JenkinsJob
            {
                ApplicationId = id
            };
            var jenkinsJobTypes = new List<JenkinsJobType>();
            var jenkinsJobUrl = new Uri("http://jenkinsUrl.activebuilder.com/job");
            var application = new Application();
            var applicationJenkinsJobViewModel = new ApplicationJenkinsJobViewModel();

            applicationJenkinsJobService.Setup(x => x.GetApplicationJenkinsJob(id)).Returns(jenkinsJob);
            applicationJenkinsJobService.Setup(x => x.GetJenkinsJobTypes()).Returns(jenkinsJobTypes);
            generalSettingsService.Setup(x => x.GetJenkinsJobUrl()).Returns(jenkinsJobUrl);
            applicationReaderService.Setup(x => x.GetApplication(jenkinsJob.ApplicationId)).Returns(application);
            viewModelFactory.Setup(x => x.CreateDetailApplicationJenkinsJobViewModel(application, jenkinsJob, jenkinsJobTypes, jenkinsJobUrl.ToString())).Returns(applicationJenkinsJobViewModel);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Detail).ModelAs<ApplicationJenkinsJobViewModel>().Should().Be(applicationJenkinsJobViewModel);
        }

        #endregion

        #region edit

        [Test]
        public void Edit_NoCondition_ReturnView()
        {
            //Arrange
            var applicationId = 1;
            var jenkinsJob = new JenkinsJob
            {
                ApplicationId = applicationId
            };
            var jenkinsJobTypeList = new List<JenkinsJobType>();
            var jenkinsUrl = new Uri("http://wwww.example.com/jenkins-url");
            var application = new Application();
            var applicationJenkinsJobViewModel = new ApplicationJenkinsJobViewModel();

            applicationJenkinsJobService.Setup(x => x.GetApplicationJenkinsJob(applicationId)).Returns(jenkinsJob);
            applicationJenkinsJobService.Setup(x => x.GetJenkinsJobTypes()).Returns(jenkinsJobTypeList);
            generalSettingsService.Setup(x => x.GetJenkinsJobUrl()).Returns(jenkinsUrl);
            applicationReaderService.Setup(x => x.GetApplication(jenkinsJob.ApplicationId)).Returns(application);

            viewModelFactory.Setup(x => x.CreateEditApplicationJenkinsJobViewModel(application, jenkinsJob, jenkinsJobTypeList, jenkinsUrl.ToString())).Returns(applicationJenkinsJobViewModel);

            //Act
            var result = controller.Edit(applicationId);

            //Assert
            AssertHelpers.AssertViewResult(applicationJenkinsJobViewModel, ViewNames.Edit, result);
        }

        [Test]
        public void Edit_JenkinsJobNotFound_RedirectToApplicationListAndSetErrorMessageTempData()
        {
            //Arrange
            var applicationId = 1;
            JenkinsJob jenkinsJob = null;

            applicationJenkinsJobService.Setup(x => x.GetApplicationJenkinsJob(applicationId)).Returns(jenkinsJob);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationJenkinsJobNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(applicationId);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_InvalidModel_ReturnView()
        {
            //Arrange
            var applicationId = 1;
            var jenkinsJob = new JenkinsJob
            {
                ApplicationId = applicationId
            };
            var jenkinsJobTypeList = new List<JenkinsJobType>();
            var jenkinsUrl = new Uri("http://wwww.example.com/jenkins-url");
            var application = new Application();
            var applicationJenkinsJobViewModel = new ApplicationJenkinsJobViewModel
            {
                ApplicationJenkinsJob = jenkinsJob
            };

            controller.ModelState.AddModelError("", "invalid model");

            applicationJenkinsJobService.Setup(x => x.GetJenkinsJobTypes()).Returns(jenkinsJobTypeList);
            generalSettingsService.Setup(x => x.GetJenkinsJobUrl()).Returns(jenkinsUrl);
            applicationReaderService.Setup(x => x.GetApplication(applicationJenkinsJobViewModel.ApplicationJenkinsJob.ApplicationId)).Returns(application);

            viewModelFactory.Setup(x => x.CreateEditApplicationJenkinsJobViewModel(application, jenkinsJob, jenkinsJobTypeList, jenkinsUrl.ToString())).Returns(applicationJenkinsJobViewModel);

            //Act
            var result = controller.Edit(applicationJenkinsJobViewModel);

            //Assert
            AssertHelpers.AssertViewResult(applicationJenkinsJobViewModel, ViewNames.Edit, result);
        }

        [Test]
        public void Edit_UpdateFails_ReturnView()
        {
            //Arrange
            var applicationId = 1;
            var jenkinsJob = new JenkinsJob
            {
                ApplicationId = applicationId
            };
            var jenkinsJobTypeList = new List<JenkinsJobType>();
            var jenkinsUrl = new Uri("http://wwww.example.com/jenkins-url");
            var application = new Application();
            var applicationJenkinsJobViewModel = new ApplicationJenkinsJobViewModel
            {
                ApplicationJenkinsJob = jenkinsJob
            };
            int userId = 45;
            var updateResult = ServiceResult.Error("Error");
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, updateResult.Message);
            SetResultMessageTempData(expectedTempData);

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationJenkinsJobService.Setup(x => x.UpdateApplicationJenkinsJob(applicationJenkinsJobViewModel.ApplicationJenkinsJob)).Returns(updateResult);
            applicationJenkinsJobService.Setup(x => x.GetJenkinsJobTypes()).Returns(jenkinsJobTypeList);
            generalSettingsService.Setup(x => x.GetJenkinsJobUrl()).Returns(jenkinsUrl);
            applicationReaderService.Setup(x => x.GetApplication(applicationJenkinsJobViewModel.ApplicationJenkinsJob.ApplicationId)).Returns(application);
            viewModelFactory.Setup(x => x.CreateEditApplicationJenkinsJobViewModel(application, jenkinsJob, jenkinsJobTypeList, jenkinsUrl.ToString())).Returns(applicationJenkinsJobViewModel);

            //Act
            var result = controller.Edit(applicationJenkinsJobViewModel);

            //Assert
            AssertHelpers.AssertViewResult(applicationJenkinsJobViewModel, ViewNames.Edit, result);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_UpdateSucceeds_RedirectToApplicationAndSetSuccessResultTempData()
        {
            //Arrange
            var applicationId = 1;
            var jenkinsJob = new JenkinsJob
            {
                ApplicationId = applicationId
            };
            var applicationJenkinsJobViewModel = new ApplicationJenkinsJobViewModel
            {
                ApplicationJenkinsJob = jenkinsJob
            };
            int userId = 45;
            var updateResult = ServiceResult.Success("Success");
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, updateResult.Message);
            SetResultMessageTempData(expectedTempData);

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationJenkinsJobService.Setup(x => x.UpdateApplicationJenkinsJob(applicationJenkinsJobViewModel.ApplicationJenkinsJob)).Returns(updateResult);

            //Act
            var result = controller.Edit(applicationJenkinsJobViewModel);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationJenkinsJobControllerActionNames.Detail).WithControllerName(ControllerNames.ApplicationJenkinsJob).WithRouteValue("id", jenkinsJob.JenkinsJobId);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region get jenkins jobs

        [Test]
        public void GetJenkinsJobs_NoCondition_ReturnJson()
        {
            // Arrange
            var applicationId = 0;
            var resultList = new List<JenkinsJob>();

            applicationReaderService.Setup(x => x.GetJenkinsJobs(applicationId)).Returns(resultList);

            // Act
            var result = controller.GetJenkinsJobs(applicationId);

            // Assert
            var resultModel = result.Should().BeOfType<JsonResult>().Which.Value.As<ClientDataResult>();
            var expectedResult = ClientDataResult.Success(resultList);
            resultModel.Should().BeEquivalentTo(expectedResult);
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