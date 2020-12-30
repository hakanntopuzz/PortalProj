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
    public class ApplicationSvnControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IApplicationSvnService> applicationSvnService;

        StrictMock<IApplicationReaderService> applicationReaderService;

        StrictMock<IGeneralSettingsService> generalSettingsService;

        StrictMock<IApplicationSvnViewModelFactory> applicationSvnViewModelFactory;

        StrictMock<IUrlGeneratorService> urlGeneratorService;

        StrictMock<IRouteValueFactory> routeValueFactory;

        ApplicationSvnController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            applicationSvnService = new StrictMock<IApplicationSvnService>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();
            generalSettingsService = new StrictMock<IGeneralSettingsService>();
            applicationSvnViewModelFactory = new StrictMock<IApplicationSvnViewModelFactory>();
            urlGeneratorService = new StrictMock<IUrlGeneratorService>();
            routeValueFactory = new StrictMock<IRouteValueFactory>();

            controller = new ApplicationSvnController(
                userSessionService.Object,

                applicationReaderService.Object,
                applicationSvnViewModelFactory.Object,
                applicationSvnService.Object,
                generalSettingsService.Object,
                urlGeneratorService.Object,
                routeValueFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationSvnService.VerifyAll();
            applicationReaderService.VerifyAll();
            generalSettingsService.VerifyAll();
            urlGeneratorService.VerifyAll();
            routeValueFactory.VerifyAll();
            applicationSvnViewModelFactory.VerifyAll();
            userSessionService.VerifyAll();
        }

        #endregion

        #region add

        [Test]
        public void Add_ApplicationDoesNotExist_ReturnErrorMessageAndRedirectToApplicationList()
        {
            // Arrange
            var applicationId = 1;
            Application application = null;

            applicationReaderService.Setup(x => x.GetApplication(applicationId)).Returns(application);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = controller.Add(applicationId);

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
            var svnUrl = "svn://svn.activebuilder.local/";
            var svnUri = new Uri(svnUrl);
            var repositoryTypeList = new List<SvnRepositoryType>();
            var applicationSvnViewModel = new ApplicationSvnViewModel();

            applicationReaderService.Setup(x => x.GetApplication(applicationId)).Returns(application);
            applicationSvnService.Setup(x => x.GetSvnRepositoryTypes()).Returns(repositoryTypeList);
            generalSettingsService.Setup(x => x.GetSvnUrl()).Returns(svnUri);
            applicationSvnViewModelFactory.Setup(x => x.CreateApplicationSvnViewModel(applicationId, application.Name, svnUrl, repositoryTypeList)).Returns(applicationSvnViewModel);

            //Act
            var result = controller.Add(applicationId);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Add).ModelAs<ApplicationSvnViewModel>().Should().Be(applicationSvnViewModel);
        }

        [Test]
        public void Add_InvalidModel_ReturnView()
        {
            //Arrange
            var viewModel = new ApplicationSvnViewModel
            {
                ApplicationSvn = new SvnRepository()
            };

            controller.ModelState.AddModelError("", "invalid model");

            var repositoryTypes = new List<SvnRepositoryType>();
            var svnUrl = "svn://svn.activebuilder.local/";
            var svnUri = new Uri(svnUrl);

            applicationSvnService.Setup(x => x.GetSvnRepositoryTypes()).Returns(repositoryTypes);
            generalSettingsService.Setup(x => x.GetSvnUrl()).Returns(svnUri);

            //Act
            var result = controller.Add(viewModel);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Add).ModelAs<ApplicationSvnViewModel>().Should().Be(viewModel);
        }

        [Test]
        public void Add_AddingFails_ReturnView()
        {
            //Arrange
            var viewModel = new ApplicationSvnViewModel
            {
                ApplicationSvn = new SvnRepository()
            };
            var repositoryTypes = new List<SvnRepositoryType>();
            var svnUrl = "svn://svn.activebuilder.local/";
            var svnUri = new Uri(svnUrl);
            int userId = 45;
            var serviceResult = ServiceResult.Error("error");

            applicationSvnService.Setup(x => x.GetSvnRepositoryTypes()).Returns(repositoryTypes);
            generalSettingsService.Setup(x => x.GetSvnUrl()).Returns(svnUri);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationSvnService.Setup(x => x.AddApplicationSvnRepository(viewModel.ApplicationSvn)).Returns(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(viewModel);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Add).ModelAs<ApplicationSvnViewModel>().Should().Be(viewModel);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Add_AddingSucceeds_RedirectToApplicationAndSetSuccessResultTempData()
        {
            //Arrange
            var viewModel = new ApplicationSvnViewModel
            {
                ApplicationSvn = new SvnRepository
                {
                    ApplicationId = 1
                }
            };
            int userId = 45;

            var serviceResult = ServiceResult.Success();

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationSvnService.Setup(x => x.AddApplicationSvnRepository(viewModel.ApplicationSvn)).Returns(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Add(viewModel);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Detail).WithControllerName(ControllerNames.Application).WithRouteValue("id", viewModel.ApplicationSvn.ApplicationId);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region detail

        [Test]
        public void Detail_ApplicationSvnRepositoryNotExists_ReturnDetailErrorView()
        {
            //Arrange
            var id = 1;
            SvnRepository repository = null;

            applicationSvnService.Setup(x => x.GetApplicationSvnRepository(id)).Returns(repository);

            var serviceResult = ServiceResult.Error(Messages.ApplicationSvnRepositoryNotFound);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeRedirectToActionResult(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Detail_ApplicationSvnRepositoryExists_ReturnDetailSuccessView()
        {
            //Arrange
            var id = 1;
            var repository = new SvnRepository
            {
                ApplicationId = id
            };
            var svnUrl = new Uri("svn://svn.activebuilder.local/");
            var applicationSvnViewModel = new ApplicationSvnViewModel();

            applicationSvnService.Setup(x => x.GetApplicationSvnRepository(id)).Returns(repository);
            generalSettingsService.Setup(x => x.GetSvnUrl()).Returns(svnUrl);
            applicationSvnViewModelFactory.Setup(x => x.CreateApplicationSvnDetailViewModel(repository)).Returns(applicationSvnViewModel);

            //Act
            var result = controller.Detail(id);

            //Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Detail).ModelAs<ApplicationSvnViewModel>().Should().Be(applicationSvnViewModel);
        }

        #endregion

        #region edit

        [Test]
        public void Edit_ApplicationSvnRepositoryExists_ReturnView()
        {
            //Arrange
            var svnRepositoryId = 1;
            var svnRepository = new SvnRepository();
            var repositoryTypes = new List<SvnRepositoryType>();
            var svnUrl = new Uri("http://wwww.example.com/svn-url");
            var applicationSvnViewModel = new ApplicationSvnViewModel();

            applicationSvnService.Setup(x => x.GetApplicationSvnRepository(svnRepositoryId)).Returns(svnRepository);
            applicationSvnService.Setup(x => x.GetSvnRepositoryTypes()).Returns(repositoryTypes);
            generalSettingsService.Setup(x => x.GetSvnUrl()).Returns(svnUrl);

            applicationSvnViewModelFactory.Setup(x => x.CreateApplicationSvnEditViewModel(svnRepository, repositoryTypes)).Returns(applicationSvnViewModel);

            //Act
            var result = controller.Edit(svnRepositoryId);

            //Assert
            AssertHelpers.AssertViewResult(applicationSvnViewModel, ViewNames.Edit, result);
        }

        [Test]
        public void Edit_ApplicationSvnRepositoryDoesNotExist_RedirectToApplicationListAndSetErrorMessageTempData()
        {
            //Arrange
            var svnRepositoryId = 1;
            SvnRepository svnRepository = null;

            applicationSvnService.Setup(x => x.GetApplicationSvnRepository(svnRepositoryId)).Returns(svnRepository);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.ApplicationSvnRepositoryNotFound);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Edit(svnRepositoryId);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationControllerActionNames.Index).WithControllerName(ControllerNames.Application);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_InvalidModel_ReturnView()
        {
            //Arrange
            var svnRepository = new SvnRepository();
            var repositoryTypes = new List<SvnRepositoryType>();
            var svnUrl = new Uri("http://wwww.example.com/svn-url");
            var applicationSvnViewModel = new ApplicationSvnViewModel
            {
                ApplicationSvn = svnRepository
            };

            controller.ModelState.AddModelError("", "invalid model");

            applicationSvnService.Setup(x => x.GetSvnRepositoryTypes()).Returns(repositoryTypes);
            generalSettingsService.Setup(x => x.GetSvnUrl()).Returns(svnUrl);

            //Act
            var result = controller.Edit(applicationSvnViewModel);

            //Assert
            AssertHelpers.AssertViewResult(applicationSvnViewModel, ViewNames.Edit, result);
        }

        [Test]
        public void Edit_UpdateFails_ReturnView()
        {
            //Arrange
            var applicationId = 1;
            var svnRepository = new SvnRepository
            {
                ApplicationId = applicationId
            };
            var repositoryTypes = new List<SvnRepositoryType>();
            var svnUrl = new Uri("http://wwww.example.com/svn-url");
            int userId = 45;
            var applicationSvnViewModel = new ApplicationSvnViewModel
            {
                ApplicationSvn = svnRepository
            };

            var updateResult = ServiceResult.Error("Error");
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, updateResult.Message);
            SetResultMessageTempData(expectedTempData);

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationSvnService.Setup(x => x.GetSvnRepositoryTypes()).Returns(repositoryTypes);
            generalSettingsService.Setup(x => x.GetSvnUrl()).Returns(svnUrl);
            applicationSvnService.Setup(x => x.UpdateApplicationSvnRepository(applicationSvnViewModel.ApplicationSvn)).Returns(updateResult);

            //Act
            var result = controller.Edit(applicationSvnViewModel);

            //Assert
            AssertHelpers.AssertViewResult(applicationSvnViewModel, ViewNames.Edit, result);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public void Edit_UpdateSucceeds_RedirectToApplicationSvnAndSetSuccessResultTempData()
        {
            //Arrange
            var svnRepository = new SvnRepository();
            var applicationSvnViewModel = new ApplicationSvnViewModel
            {
                ApplicationSvn = svnRepository
            };
            int userId = 45;
            var updateResult = ServiceResult.Success("Success");
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, updateResult.Message);
            SetResultMessageTempData(expectedTempData);

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            applicationSvnService.Setup(x => x.UpdateApplicationSvnRepository(applicationSvnViewModel.ApplicationSvn)).Returns(updateResult);

            //Act
            var result = controller.Edit(applicationSvnViewModel);

            //Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationSvnControllerActionNames.Detail).WithControllerName(ControllerNames.ApplicationSvn).WithRouteValue("id", applicationSvnViewModel.ApplicationSvn.Id);
            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        #endregion

        #region get svn repositories

        [Test]
        public void GetSvnRepositories_NoCondition_ReturnJson()
        {
            // Arrange
            var applicationId = 0;
            var resultList = new List<SvnRepository>();

            applicationReaderService.Setup(x => x.GetSvnRepositories(applicationId)).Returns(resultList);

            // Act
            var result = controller.GetSvnRepositories(applicationId);

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