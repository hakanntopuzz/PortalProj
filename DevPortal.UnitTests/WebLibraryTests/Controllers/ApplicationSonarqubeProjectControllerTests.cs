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
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationSonarqubeProjectControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationSonarqubeProjectService> applicationSonarqubeProjectService;

        StrictMock<IApplicationReaderService> applicationReaderService;

        StrictMock<IGeneralSettingsService> generalSettingsService;

        StrictMock<IApplicationSonarQubeProjectViewModelFactory> viewModelFactory;

        StrictMock<IUrlGeneratorService> urlHelper;

        StrictMock<IRouteValueFactory> routeValueFactory;

        ApplicationSonarqubeProjectController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            applicationSonarqubeProjectService = new StrictMock<IApplicationSonarqubeProjectService>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();
            generalSettingsService = new StrictMock<IGeneralSettingsService>();
            viewModelFactory = new StrictMock<IApplicationSonarQubeProjectViewModelFactory>();
            urlHelper = new StrictMock<IUrlGeneratorService>();
            routeValueFactory = new StrictMock<IRouteValueFactory>();

            controller = new ApplicationSonarqubeProjectController(
                userSessionService.Object,
                applicationSonarqubeProjectService.Object,
                applicationReaderService.Object,
                generalSettingsService.Object,
                viewModelFactory.Object,
                urlHelper.Object,
                routeValueFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationSonarqubeProjectService.VerifyAll();
            applicationReaderService.VerifyAll();
            generalSettingsService.VerifyAll();
            viewModelFactory.VerifyAll();
            urlHelper.VerifyAll();
            routeValueFactory.VerifyAll();
            userSessionService.VerifyAll();
        }

        #endregion

        #region index

        [Test]
        public void Index_ProjectsExistTrue_ReturnClientResult()
        {
            //Arrange
            var applicationId = 1;
            ICollection<SonarqubeProject> projects = null;

            applicationSonarqubeProjectService.Setup(x => x.GetSonarqubeProjects(applicationId)).Returns(projects);

            //Act
            var result = controller.Index(applicationId);

            //Assert
            var expectedResult = ClientDataResult.Error(projects);
            AssertHelpers.AssertClientDataResult(result, expectedResult);
        }

        [Test]
        public void Index_ProjectsExistFalse_ReturnClientResult()
        {
            //Arrange
            var applicationId = 1;
            var projects = new List<SonarqubeProject>();

            applicationSonarqubeProjectService.Setup(x => x.GetSonarqubeProjects(applicationId)).Returns(projects);

            //Act
            var result = controller.Index(applicationId);

            //Assert
            var expectedResult = ClientDataResult.Success(projects);
            AssertHelpers.AssertClientDataResult(result, expectedResult);
        }

        #endregion

        #region detail

        [Test]
        public void Detail_ApplicationSonarQubeProjectNotExists_ReturnSonarQubeProjectNotFound()
        {
            //Arrange
            var id = 1;
            SonarqubeProject project = null;

            applicationSonarqubeProjectService.Setup(x => x.GetSonarQubeProject(id)).Returns(project);

            var serviceResult = ServiceResult.Error(Messages.SonarQubeProjectNotFound);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_ApplicationSonarQubeProjectExists_ReturnSonarQubeProjectViewModel()
        {
            //Arrange
            var id = 1;
            var project = new SonarqubeProject
            {
                ApplicationId = id
            };
            var projectTypes = new List<SonarQubeProjectType>();
            var projectUrl = "http://sonarqube.activebuilder.local:9000/projects";
            var projectUri = new Uri(projectUrl);
            var application = new Application();
            var viewModel = new ApplicationSonarQubeProjectViewModel();

            applicationSonarqubeProjectService.Setup(x => x.GetSonarQubeProject(id)).Returns(project);
            applicationSonarqubeProjectService.Setup(x => x.GetSonarQubeProjectTypes()).Returns(projectTypes);
            generalSettingsService.Setup(x => x.GetSonarqubeProjectUrl()).Returns(projectUri);
            applicationReaderService.Setup(x => x.GetApplication(project.ApplicationId)).Returns(application);
            viewModelFactory.Setup(x => x.CreateApplicationSonarQubeProjectViewModel(application, project, projectTypes, projectUrl)).Returns(viewModel);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Detail).ModelAs<ApplicationSonarQubeProjectViewModel>().Should().Be(viewModel);
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
            var projectTypes = new List<SonarQubeProjectType>();
            var projectUrl = "http://sonarqube.activebuilder.local:9000/projects";
            var projectUri = new Uri(projectUrl);
            var viewModel = new ApplicationSonarQubeProjectViewModel();

            applicationReaderService.Setup(x => x.GetApplication(applicationId)).Returns(application);
            applicationSonarqubeProjectService.Setup(x => x.GetSonarQubeProjectTypes()).Returns(projectTypes);
            generalSettingsService.Setup(x => x.GetSonarqubeProjectUrl()).Returns(projectUri);
            viewModelFactory.Setup(x => x.CreateApplicationSonarQubeProjectViewModel(application, projectTypes, projectUrl)).Returns(viewModel);

            //Act
            var result = controller.Add(applicationId);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Add).ModelAs<ApplicationSonarQubeProjectViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Add_InvalidModel_ReturnView()
        {
            //Arrange
            var viewModel = new ApplicationSonarQubeProjectViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            //Act
            var result = controller.Add(viewModel);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Add).ModelAs<ApplicationSonarQubeProjectViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Add_AddingFails_ReturnView()
        {
            //Arrange
            var viewModel = new ApplicationSonarQubeProjectViewModel
            {
                ApplicationSonarQubeProject = new SonarqubeProject()
            };
            int userId = 45;
            var serviceResult = ServiceResult.Error("error");

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationSonarqubeProjectService.Setup(x => x.AddApplicationSonarQubeProject(viewModel.ApplicationSonarQubeProject)).Returns(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(viewModel);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Add).ModelAs<ApplicationSonarQubeProjectViewModel>().Should().Be(viewModel);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Add_AddingSucceeds_RedirectToApplicationAndSetSuccessResultTempData()
        {
            //Arrange
            var viewModel = new ApplicationSonarQubeProjectViewModel
            {
                ApplicationSonarQubeProject = new SonarqubeProject
                {
                    ApplicationId = 1
                }
            };
            int userId = 45;
            var serviceResult = ServiceResult.Success();

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationSonarqubeProjectService.Setup(x => x.AddApplicationSonarQubeProject(viewModel.ApplicationSonarQubeProject)).Returns(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(viewModel);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Detail).WithControllerName(ControllerNames.Application).WithRouteValue("id", viewModel.ApplicationSonarQubeProject.ApplicationId);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region edit

        [Test]
        public void Edit_NoCondition_ReturnView()
        {
            //Arrange
            var applicationId = 1;
            var project = new SonarqubeProject
            {
                ApplicationId = applicationId
            };
            var projectTypeList = new List<SonarQubeProjectType>();
            var projectUrl = new Uri("http://wwww.example.com/project-url");
            var application = new Application();
            var viewModel = new ApplicationSonarQubeProjectViewModel();

            applicationSonarqubeProjectService.Setup(x => x.GetSonarQubeProject(applicationId)).Returns(project);
            applicationSonarqubeProjectService.Setup(x => x.GetSonarQubeProjectTypes()).Returns(projectTypeList);
            generalSettingsService.Setup(x => x.GetSonarqubeProjectUrl()).Returns(projectUrl);
            applicationReaderService.Setup(x => x.GetApplication(project.ApplicationId)).Returns(application);

            viewModelFactory.Setup(x => x.CreateEditApplicationSonarQubeProjectViewModel(application, project, projectTypeList, projectUrl.ToString())).Returns(viewModel);

            //Act
            var result = controller.Edit(applicationId);

            //Assert
            AssertHelpers.AssertViewResult(viewModel, ViewNames.Edit, result);
        }

        [Test]
        public void Edit_SonarqubeProjectNotFound_RedirectToApplicationListAndSetErrorMessageTempData()
        {
            //Arrange
            var applicationId = 1;
            SonarqubeProject project = null;

            applicationSonarqubeProjectService.Setup(x => x.GetSonarQubeProject(applicationId)).Returns(project);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.SonarQubeProjectNotFound);
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
            var project = new SonarqubeProject
            {
                ApplicationId = applicationId
            };
            var projectTypeList = new List<SonarQubeProjectType>();
            var projectUrl = new Uri("http://wwww.example.com/project-url");
            var application = new Application();
            var viewModel = new ApplicationSonarQubeProjectViewModel
            {
                ApplicationSonarQubeProject = project
            };

            controller.ModelState.AddModelError("", "invalid model");

            applicationSonarqubeProjectService.Setup(x => x.GetSonarQubeProjectTypes()).Returns(projectTypeList);
            generalSettingsService.Setup(x => x.GetSonarqubeProjectUrl()).Returns(projectUrl);
            applicationReaderService.Setup(x => x.GetApplication(viewModel.ApplicationSonarQubeProject.ApplicationId)).Returns(application);

            viewModelFactory.Setup(x => x.CreateApplicationSonarQubeProjectViewModel(application, project, projectTypeList, projectUrl.ToString())).Returns(viewModel);

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            AssertHelpers.AssertViewResult(viewModel, ViewNames.Edit, result);
        }

        [Test]
        public void Edit_UpdateFails_ReturnView()
        {
            //Arrange
            var applicationId = 1;
            var project = new SonarqubeProject
            {
                ApplicationId = applicationId
            };
            var projectTypeList = new List<SonarQubeProjectType>();
            var projectUrl = new Uri("http://wwww.example.com/project-url");
            var application = new Application();
            var viewModel = new ApplicationSonarQubeProjectViewModel
            {
                ApplicationSonarQubeProject = project
            };
            int userId = 45;
            var updateResult = ServiceResult.Error("Error");
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, updateResult.Message);
            SetResultMessageTempData(expectedTempData);

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationSonarqubeProjectService.Setup(x => x.UpdateApplicationSonarQubeProject(viewModel.ApplicationSonarQubeProject)).Returns(updateResult);
            applicationSonarqubeProjectService.Setup(x => x.GetSonarQubeProjectTypes()).Returns(projectTypeList);
            generalSettingsService.Setup(x => x.GetSonarqubeProjectUrl()).Returns(projectUrl);
            applicationReaderService.Setup(x => x.GetApplication(viewModel.ApplicationSonarQubeProject.ApplicationId)).Returns(application);
            viewModelFactory.Setup(x => x.CreateApplicationSonarQubeProjectViewModel(application, project, projectTypeList, projectUrl.ToString())).Returns(viewModel);

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            AssertHelpers.AssertViewResult(viewModel, ViewNames.Edit, result);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_UpdateSucceeds_RedirectToApplicationAndSetSuccessResultTempData()
        {
            //Arrange
            var applicationId = 1;
            var project = new SonarqubeProject
            {
                ApplicationId = applicationId
            };
            var viewModel = new ApplicationSonarQubeProjectViewModel
            {
                ApplicationSonarQubeProject = project
            };
            int userId = 45;
            var updateResult = ServiceResult.Success("Success");
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, updateResult.Message);
            SetResultMessageTempData(expectedTempData);

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationSonarqubeProjectService.Setup(x => x.UpdateApplicationSonarQubeProject(viewModel.ApplicationSonarQubeProject)).Returns(updateResult);

            //Act
            var result = controller.Edit(viewModel);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationSonarqubeProjectControllerActionNames.Detail).WithControllerName(ControllerNames.ApplicationSonarqubeProject).WithRouteValue("id", project.SonarqubeProjectId);
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