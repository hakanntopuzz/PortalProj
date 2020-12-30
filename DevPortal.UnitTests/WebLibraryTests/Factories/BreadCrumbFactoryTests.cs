using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Factories;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class BreadCrumbFactoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUrlHelper> urlHelper;

        BreadCrumbFactory factory;

        [SetUp]
        public void Initialize()
        {
            urlHelper = new StrictMock<IUrlHelper>();

            factory = new BreadCrumbFactory(urlHelper.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            urlHelper.VerifyAll();
        }

        #endregion

        #region application

        [Test]
        public void CreateApplicationsModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl())
            };

            // Act
            var result = factory.CreateApplicationsModel();

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateApplicationAddModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.NewApplication, GetAddApplicationUrl())
            };

            // Act
            var result = factory.CreateApplicationAddModel();

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateApplicationEditModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationId = 1;
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var applicationEditUrl = GetApplicationEditUrl(applicationId);
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.ApplicationUpdate, applicationEditUrl)
            };

            // Act
            var result = factory.CreateApplicationEditModel(applicationId);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateNewApplicationModel_NoCondition_ReturnModel()
        {
            // Arrange
            int applicationId = 78;
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl)
            };

            // Act
            var result = factory.CreateApplicationDetailModel(applicationId);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region  application group

        [Test]
        public void CreateApplicationGroupsModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.ApplicationGroups, GetApplicationGroupUrl())
            };

            // Act
            var result = factory.CreateApplicationGroupsModel();

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateApplicationGroupAddModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.ApplicationGroups, GetApplicationGroupUrl()),
                BreadCrumbModel.Create(PageNames.NewApplicationGroup, GetAddApplicationGroupUrl())
            };

            // Act
            var result = factory.CreateApplicationGroupAddModel();

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateApplicationGroupEditModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.ApplicationGroups, GetApplicationGroupUrl()),
                BreadCrumbModel.Create(PageNames.DetailApplicationGroup, GetDetailApplicationGroupUrl()),
                BreadCrumbModel.Create(PageNames.EditApplicationGroup)
            };

            // Act
            var result = factory.CreateApplicationGroupEditModel();

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateApplicationGroupDetailModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.ApplicationGroups, GetApplicationGroupUrl()),
                BreadCrumbModel.Create(PageNames.DetailApplicationGroup, GetDetailApplicationGroupUrl())
            };

            // Act
            var result = factory.CreateApplicationGroupDetailModel();

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region  application environment

        [Test]
        public void CreateApplicationEnvironmentModel_NoCondition_ReturnModel()
        {
            // Arrange
            int applicationId = 3;

            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var addapplicationEnvironmentUrl = "app-env-url";
            urlHelper.Setup(x => x.Action(ApplicationEnvironmentControllerActionNames.Add, ControllerNames.ApplicationEnvironment, SetupAny<object>())).Returns(addapplicationEnvironmentUrl);
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications,GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation,applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.NewApplicationEnvironment,addapplicationEnvironmentUrl)
            };

            // Act
            var result = factory.CreateApplicationEnvironmentModel(applicationId);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateApplicationEnvironmentDetailModel_NoCondition_ReturnModel()
        {
            // Arrange
            int applicationId = 3;

            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications,GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation,applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.ApplicationEnvironmentInformations)
            };

            // Act
            var result = factory.CreateApplicationEnvironmentDetailModel(applicationId);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateEditApplicationEnvironmentModel_NoCondition_ReturnModel()
        {
            // Arrange
            int applicaitonEnvironmentId = 1;
            int applicationId = 3;

            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var applicationEnvironmentDetailUrl = "app-env-url";
            urlHelper.Setup(x => x.Action(ApplicationEnvironmentControllerActionNames.Detail, ControllerNames.ApplicationEnvironment, applicaitonEnvironmentId)).Returns(applicationEnvironmentDetailUrl);
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications,GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation,applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.ApplicationEnvironmentInformations,applicationEnvironmentDetailUrl),
                BreadCrumbModel.Create(PageNames.ApplicationEnvironmentUpdate)
            };

            // Act
            var result = factory.CreateApplicationEnvironmentEditModel(applicaitonEnvironmentId, applicationId);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region application svn

        [Test]
        public void CreateNewSvnRepositoryModel_NoCondition_ReturnModel()
        {
            // Arrange
            int applicationId = 78;
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.NewSVNRepository)
            };

            // Act
            var result = factory.CreateNewSvnRepositoryModel(applicationId);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateEditSvnRepositoryModel_NoCondition_ReturnModel()
        {
            // Arrange
            int applicationId = 78;
            int svnRepositoryId = 78;
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var applicationSvnDetailUrl = "";

            urlHelper.Setup(x => x.Action(ApplicationSvnControllerActionNames.Detail, ControllerNames.ApplicationSvn, svnRepositoryId)).Returns(applicationSvnDetailUrl);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.SVNRepositoryInformation,applicationSvnDetailUrl),
                BreadCrumbModel.Create(PageNames.SVNRepositoryUpdate)
            };

            // Act
            var result = factory.CreateEditSvnRepositoryModel(applicationId, svnRepositoryId);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateDetailSvnRepositoryModel_NoCondition_ReturnModel()
        {
            // Arrange
            int applicationId = 78;

            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications,  GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.SVNRepositoryInformation)
            };

            // Act
            var result = factory.CreateDetailSvnRepositoryModel(applicationId);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region application jenkins

        [Test]
        public void CreateNewJenkinsJobModel_NoCondition_ReturnModel()
        {
            // Arrange
            int applicationId = 78;

            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.NewJenkinsQuest)
            };

            // Act
            var result = factory.CreateNewJenkinsJobModel(applicationId);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateDetailJenkinsJobModel_NoCondition_ReturnModel()
        {
            // Arrange
            int applicationId = 78;
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.JenkinsTaskInformation)
            };

            // Act
            var result = factory.CreateDetailJenkinsJobModel(applicationId);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateEditJenkinsJobModel_NoCondition_ReturnModel()
        {
            // Arrange
            int applicationId = 78;
            int jenkinsJobId = 78;
            var applicationJenkinsJobDetailUrl = "";
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            urlHelper.Setup(x => x.Action(ApplicationControllerActionNames.Detail, ControllerNames.ApplicationJenkinsJob, jenkinsJobId)).Returns(applicationJenkinsJobDetailUrl);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.JenkinsTaskInformation, applicationJenkinsJobDetailUrl),
                BreadCrumbModel.Create(PageNames.JenkinsTaskUpdate)
            };

            // Act
            var result = factory.CreateEditJenkinsJobModel(jenkinsJobId, applicationId);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region application sonarqube

        [Test]
        public void CreateDetailSonarQubeProjectModel_NoCondition_ReturnModel()
        {
            // Arrange
            int applicationId = 2;
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.SonarQubeProjectInformation)
            };

            // Act
            var result = factory.CreateDetailSonarQubeProjectModel(applicationId);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateNewSonarQubeProjectModel_NoCondition_ReturnModel()
        {
            // Arrange
            int applicationId = 3;
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.NewSonarQubeProject)
            };

            // Act
            var result = factory.CreateNewSonarQubeProjectModel(applicationId);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateEditSonarQubeProjectModel_NoCondition_ReturnModel()
        {
            // Arrange
            int applicationId = 3;
            int projectId = 5;

            var projectDetailUrl = "project-detail-url";
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            urlHelper.Setup(x => x.Action(ApplicationSonarqubeProjectControllerActionNames.Detail, ControllerNames.ApplicationSonarqubeProject, projectId)).Returns(projectDetailUrl);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.SonarQubeProjectInformation, projectDetailUrl),
                BreadCrumbModel.Create(PageNames.SonarQubeProjectUpdate)
            };

            // Act
            var result = factory.CreateEditSonarQubeProjectModel(projectId, applicationId);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region log

        [Test]
        public void CreateLogListModel_NoCondition_ReturnModel()
        {
            // Arrange

            var applicationsUrl = "url";

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create("Log Listesi", applicationsUrl)
            };

            urlHelper.Setup(x => x.Action(ApplicationControllerActionNames.Index, ControllerNames.Log)).Returns(applicationsUrl);

            // Act
            var result = factory.CreateLogListModel();

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.LogManagement,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateLogDetailModel_NoCondition_ReturnModel()
        {
            // Arrange

            var applicationsUrl = "url";
            var detailUrl = "detailUrl";
            string path = null;

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create("Log Listesi", applicationsUrl),
                BreadCrumbModel.Create("Log Bilgileri", detailUrl)
            };

            urlHelper.Setup(x => x.Action(ApplicationControllerActionNames.Index, ControllerNames.Log)).Returns(applicationsUrl);
            urlHelper.Setup(x => x.Action(ApplicationControllerActionNames.LogDetails, ControllerNames.Log, SetupAny<object>())).Returns(detailUrl);

            // Act
            var result = factory.CreateLogDetailModel(path);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.LogManagement,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region menu

        [Test]
        public void CreateMenuListModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Menu,GetMenuUrl())
            };

            // Act
            var result = factory.CreateMenuListModel();

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Management,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateAddMenuModel_NoCondition_ReturnModel()
        {
            // Arrange
            var addMenuUrl = "menu";
            urlHelper.Setup(x => x.Action(ApplicationControllerActionNames.Add, ControllerNames.Menu)).Returns(addMenuUrl);
            var pages = new List<BreadCrumbModel>
            {
                 BreadCrumbModel.Create(PageNames.Menu,GetMenuUrl()),
                 BreadCrumbModel.Create(PageNames.NewMenu,addMenuUrl)
            };

            // Act
            var result = factory.CreateAddMenuModel();

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Management,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateDetailMenuModel_NoCondition_ReturnModel()
        {
            // Arrange
            var detailMenuUrl = "menu";
            urlHelper.Setup(x => x.Action(ApplicationControllerActionNames.Detail, ControllerNames.Menu)).Returns(detailMenuUrl);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Menu,GetMenuUrl()),
                BreadCrumbModel.Create(PageNames.MenuInformation, detailMenuUrl)
            };

            // Act
            var result = factory.CreateDetailMenuModel();

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Management,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateEditMenuModel_NoCondition_ReturnModel()
        {
            // Arrange
            var id = 1;
            var detailMenuUrl = "menu-detail/1";
            var editMenuUrl = "menu-edit/1";
            urlHelper.Setup(x => x.Action(ApplicationControllerActionNames.Edit, ControllerNames.Menu, id)).Returns(editMenuUrl);
            urlHelper.Setup(x => x.Action(ApplicationControllerActionNames.Detail, ControllerNames.Menu, id)).Returns(detailMenuUrl);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Menu,GetMenuUrl()),
                BreadCrumbModel.Create(PageNames.MenuInformation,detailMenuUrl),
                BreadCrumbModel.Create(PageNames.UpdateMenu,editMenuUrl)
            };

            // Act
            var result = factory.CreateEditMenuModel(id);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Management,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region general settings

        [Test]
        public void CreateGeneralSettingsModel_NoCondition_ReturnModel()
        {
            // Arrange

            var generalSettingsUrl = "url";

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create("Genel Ayarlar", generalSettingsUrl),
            };

            urlHelper.Setup(x => x.Action(ApplicationControllerActionNames.Index, ControllerNames.GeneralSettings)).Returns(generalSettingsUrl);

            // Act
            var result = factory.CreateGeneralSettingsModel();

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Management,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region  svn admin

        [Test]
        public void CreateSvnAdminListModel_NoCondition_ReturnBreadCrumbViewModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.SvnRepositories, GetSvnUrl())
            };
            var breadCrumbViewModel = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.SVN,
                PageList = pages
            };

            // Act
            var result = factory.CreateSvnAdminListModel();

            // Assert

            result.Should().BeEquivalentTo(breadCrumbViewModel);
        }

        [Test]
        public void CreateSvnRepositoryFolderModel_NoCondition_ReturnBreadCrumbViewModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.SvnRepositories, GetSvnUrl()),
                BreadCrumbModel.Create(PageNames.SvnRepositoryFolder, GetSvnRepositoryFolderUrl())
            };
            var breadCrumbViewModel = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.SVN,
                PageList = pages
            };

            // Act
            var result = factory.CreateSvnRepositoryFolderModel();

            // Assert

            result.Should().BeEquivalentTo(breadCrumbViewModel);
        }

        #endregion

        #region security

        [Test]
        public void CreateGuidModel_NoCondition_ReturnModel()
        {
            // Arrange

            var guidUrl = "url";

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create("GUID Oluşturma", guidUrl),
            };

            urlHelper.Setup(x => x.Action(SecurityControllerActionNames.Guid, ControllerNames.Security)).Returns(guidUrl);

            // Act
            var result = factory.CreateGuidModel();

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Security,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateHashModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.CreateHash)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Security,
                PageList = pages
            };

            // Act
            var result = factory.CreateHashModel();

            // Assert

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreatePasswordModel_NoCondition_ReturnModel()
        {
            // Arrange
            var passwordUrl = "url";

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.CreateNewPassword,passwordUrl)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Security,
                PageList = pages
            };
            urlHelper.Setup(x => x.Action(SecurityControllerActionNames.Password, ControllerNames.Security)).Returns(passwordUrl);

            // Act
            var result = factory.CreatePasswordModel();

            // Assert

            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region user

        [Test]
        public void CreateUserListModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.User, GetUserUrl())
            };

            // Act
            var result = factory.CreateUserListModel();

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.UserManagement,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateUserAddModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.User,GetUserUrl()),
                BreadCrumbModel.Create(PageNames.NewUser)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.UserManagement,
                PageList = pages
            };

            // Act
            var result = factory.CreateUserAddModel();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateDetailUserModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.User, GetUserUrl()),
                BreadCrumbModel.Create(PageNames.UserInformation)
            };

            // Act
            var result = factory.CreateDetailUserModel();

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.UserManagement,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateEditUserModel_NoCondition_ReturnModel()
        {
            // Arrange
            var id = 5;
            var userDetailUrl = "user-detail-url";
            var userEditUrl = "user-edit-url";
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.User,GetUserUrl()),
                BreadCrumbModel.Create(PageNames.UserInformation,userDetailUrl),
                BreadCrumbModel.Create(PageNames.UpdateUser,userEditUrl)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Management,
                PageList = pages
            };

            urlHelper.Setup(x => x.Action(UserControllerActionNames.Detail, ControllerNames.User, id)).Returns(userDetailUrl);
            urlHelper.Setup(x => x.Action(UserControllerActionNames.Edit, ControllerNames.User, id)).Returns(userEditUrl);

            // Act
            var result = factory.CreateEditUserModel(id);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateUserProfileModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.UpdateOwnUser)
            };

            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.User,
                PageList = pages
            };

            // Act
            var result = factory.CreateUserProfileModel();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateChangePasswordModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.ChangePassword)
            };

            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.User,
                PageList = pages
            };

            // Act
            var result = factory.CreateChangePasswordModel();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateResetPasswordModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.ResetPassword)
            };

            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.User,
                PageList = pages
            };

            // Act
            var result = factory.CreateResetPasswordModel();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateForgotPasswordModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.ForgotPassword)
            };

            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.User,
                PageList = pages
            };

            // Act
            var result = factory.CreateForgotPasswordModel();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region application nuget package

        [Test]
        public void CreateDetailNugetPackageModel_NoCondition_ReturnModel()
        {
            // Arrange
            int applicationId = 2;
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.NugetPackageInformation)
            };

            // Act
            var result = factory.CreateDetailNugetPackageModel(applicationId);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateNewNugetPackageModel_NoCondition_ReturnModel()
        {
            // Arrange
            int applicationId = 3;
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);

            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.NewNugetPackage)
            };

            // Act
            var result = factory.CreateNewNugetPackageModel(applicationId);

            // Assert
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateEditNugetPackageModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationId = 5;
            var nugetPackageId = 16;
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var applicationNugetPackageDetailUrl = GetApplicationNugetPackageDetailUrl(nugetPackageId);

            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.NugetPackage, applicationNugetPackageDetailUrl),
                BreadCrumbModel.Create(PageNames.NugetPackageUpdate)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateEditNugetPackageModel(applicationId, nugetPackageId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region  environment

        [Test]
        public void CreateEnvironmentsModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                    BreadCrumbModel.Create(PageNames.Environments,GetEnvironmentUrl())
            };
            var breadCrumbViewModel = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateEnvironmentsModel();

            // Assert
            result.Should().BeEquivalentTo(breadCrumbViewModel);
        }

        [Test]
        public void CreateEnvironmentDetailModel_NoCondition_ReturnModel()
        {
            // Arrange
            var environmentId = 5;
            var environmentDetailUrl = GetEnvironmentDetailUrl(environmentId);
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Environments, GetEnvironmentUrl()),
                BreadCrumbModel.Create(PageNames.EnvironmentDetail, environmentDetailUrl)
            };
            var breadCrumbViewModel = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateEnvironmentDetailModel(environmentId);

            // Assert
            result.Should().BeEquivalentTo(breadCrumbViewModel);
        }

        [Test]
        public void CreateEnvironmentEditModel_NoCondition_ReturnModel()
        {
            // Arrange
            var environmentId = 5;
            var environmentDetailUrl = GetEnvironmentDetailUrl(environmentId);
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Environments, GetEnvironmentUrl()),
                BreadCrumbModel.Create(PageNames.EnvironmentDetail, environmentDetailUrl),
                BreadCrumbModel.Create(PageNames.EnvironmentEdit)
            };
            var breadCrumbViewModel = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateEnvironmentEditModel(environmentId);

            // Assert
            result.Should().BeEquivalentTo(breadCrumbViewModel);
        }

        [Test]
        public void CreateEnvironmentAddModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.Environments, GetEnvironmentUrl()),
                BreadCrumbModel.Create(PageNames.NewEnvironment)
            };
            var breadCrumbViewModel = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateEnvironmentAddModel();

            // Assert
            result.Should().BeEquivalentTo(breadCrumbViewModel);
        }

        #endregion

        #region database type

        [Test]
        public void CreateDatabaseTypesModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                    BreadCrumbModel.Create(PageNames.DatabaseTypes,GetDatabaseTypeUrl())
            };
            var breadCrumbViewModel = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateDatabaseTypesModel();

            // Assert
            result.Should().BeEquivalentTo(breadCrumbViewModel);
        }

        [Test]
        public void CreateDatabaseTypeAddModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.DatabaseTypes, GetDatabaseTypeUrl()),
                BreadCrumbModel.Create(PageNames.NewDatabaseType)
            };
            var breadCrumbViewModel = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateDatabaseTypeAddModel();

            // Assert
            result.Should().BeEquivalentTo(breadCrumbViewModel);
        }

        [Test]
        public void CreateDatabaseTypeDetailModel_NoCondition_ReturnModel()
        {
            // Arrange
            var id = 5;
            var databaseTypeDetailUrl = GetDatabaseTypeDetailUrl(id);
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.DatabaseTypes, GetDatabaseTypeUrl()),
                BreadCrumbModel.Create(PageNames.DatabaseTypeDetail, databaseTypeDetailUrl)
            };
            var breadCrumbViewModel = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateDatabaseTypeDetailModel(id);

            // Assert
            result.Should().BeEquivalentTo(breadCrumbViewModel);
        }

        [Test]
        public void CreateDatabaseTypeEditModel_NoCondition_ReturnModel()
        {
            // Arrange
            var id = 5;
            var databaseTypeDetailUrl = GetDatabaseTypeDetailUrl(id);
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.DatabaseTypes, GetDatabaseTypeUrl()),
                BreadCrumbModel.Create(PageNames.DatabaseTypeDetail, databaseTypeDetailUrl),
                BreadCrumbModel.Create(PageNames.DatabaseTypeEdit)
            };
            var breadCrumbViewModel = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateDatabaseTypeEditModel(id);

            // Assert
            result.Should().BeEquivalentTo(breadCrumbViewModel);
        }

        #endregion

        #region database

        [Test]
        public void CreateDatabasesModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Databases, GetDatabaseUrl())
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateDatabasesModel();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateDatabaseDetailModel_NoCondition_ReturnModel()
        {
            // Arrange
            var databaseId = 12;
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Databases, GetDatabaseUrl()),
                BreadCrumbModel.Create(PageNames.DatabaseInformation, GetDatabaseDetailUrl(databaseId))
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateDatabaseDetailModel(databaseId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateDatabaseAddModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Databases, GetDatabaseUrl()),
                BreadCrumbModel.Create(PageNames.NewDatabase)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateDatabaseAddModel();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateDatabaseEditModel_NoCondition_ReturnModel()
        {
            // Arrange
            var databaseId = 12;
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Databases, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.DatabaseInformation, GetDatabaseDetailUrl(databaseId)),
                BreadCrumbModel.Create(PageNames.DatabaseUpdate, GetDatabaseEditUrl(databaseId))
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateDatabaseEditModel(databaseId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region database group

        [Test]
        public void CreateDatabaseGroupsModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.DatabaseGroups,GetDatabaseGroupUrl())
            };
            var breadCrumbViewModel = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateDatabaseGroupsModel();

            // Assert
            result.Should().BeEquivalentTo(breadCrumbViewModel);
        }

        [Test]
        public void CreateDatabaseGroupAddModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.DatabaseGroups,GetDatabaseGroupUrl()),
                    BreadCrumbModel.Create(PageNames.NewDatabaseGroup)
            };
            var breadCrumbViewModel = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateDatabaseGroupAddModel();

            // Assert
            result.Should().BeEquivalentTo(breadCrumbViewModel);
        }

        [Test]
        public void CreateDatabaseGroupDetailModel_NoCondition_ReturnModel()
        {
            // Arrange
            var id = 5;
            var databaseGroupDetailUrl = GetDatabaseGroupDetailUrl(id);
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.DatabaseGroups, GetDatabaseGroupUrl()),
                BreadCrumbModel.Create(PageNames.DatabaseGroupDetail, databaseGroupDetailUrl)
            };
            var breadCrumbViewModel = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateDatabaseGroupDetailModel(id);

            // Assert
            result.Should().BeEquivalentTo(breadCrumbViewModel);
        }

        [Test]
        public void CreateDatabaseGroupEditModel_NoCondition_ReturnModel()
        {
            // Arrange
            var id = 5;
            var databaseGroupDetailUrl = GetDatabaseGroupDetailUrl(id);
            var pages = new List<BreadCrumbModel>
            {
                BreadCrumbModel.Create(PageNames.DatabaseGroups, GetDatabaseGroupUrl()),
                BreadCrumbModel.Create(PageNames.DatabaseGroupDetail, databaseGroupDetailUrl),
                BreadCrumbModel.Create(PageNames.DatabaseGroupEdit)
            };
            var breadCrumbViewModel = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateDatabaseGroupEditModel(id);

            // Assert
            result.Should().BeEquivalentTo(breadCrumbViewModel);
        }

        #endregion

        #region external dependency

        [Test]
        public void CreateExternalDependencyDetailModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationId = 12;
            var pages = new List<BreadCrumbModel> {
                 BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, GetApplicationDetailUrl(applicationId)),
                BreadCrumbModel.Create(PageNames.ExternalDependencyDetail)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateExternalDependencyDetailModel(applicationId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateExternalDependencyAddModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationId = 12;
            var pages = new List<BreadCrumbModel> {
                 BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, GetApplicationDetailUrl(applicationId)),
                BreadCrumbModel.Create(PageNames.ExternalDependencyAdd)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateExternalDependencyAddModel(applicationId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateExternalDependencyEditModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationId = 12;
            var externalDependencyId = 1;
            var pages = new List<BreadCrumbModel> {
                 BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, GetApplicationDetailUrl(applicationId)),
               BreadCrumbModel.Create(PageNames.ExternalDependencyDetail, GetExternalDependencyDetailUrl(externalDependencyId)),
                BreadCrumbModel.Create(PageNames.ExternalDependencyEdit)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateExternalDependencyEditModel(applicationId, externalDependencyId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region database dependency

        [Test]
        public void CreateDatabaseDependencyDetailModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationId = 12;
            var pages = new List<BreadCrumbModel> {
                 BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, GetApplicationDetailUrl(applicationId)),
                BreadCrumbModel.Create(PageNames.DatabaseDependencyDetail)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateDatabaseDependencyDetailModel(applicationId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateDatabaseDependencyAddModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationId = 12;
            var pages = new List<BreadCrumbModel> {
                 BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, GetApplicationDetailUrl(applicationId)),
                BreadCrumbModel.Create(PageNames.NewDatabaseDependency)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateDatabaseDependencyAddModel(applicationId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateDatabaseDependencyEditModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationId = 12;
            var databaseDependencyId = 1;
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, GetApplicationDetailUrl(applicationId)),
                BreadCrumbModel.Create(PageNames.DatabaseDependencyDetail, GetDatabaseDependencyDetailUrl(databaseDependencyId)),
                BreadCrumbModel.Create(PageNames.DatabaseDependencyEdit)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateDatabaseDependencyEditModel(applicationId, databaseDependencyId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region database dependency

        [Test]
        public void CreateApplicationDependencyAddModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationId = 12;
            var pages = new List<BreadCrumbModel> {
                 BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, GetApplicationDetailUrl(applicationId)),
                BreadCrumbModel.Create(PageNames.NewApplicationDependency)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateApplicationDependencyAddModel(applicationId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateApplicationDependencyDetailModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationId = 12;
            var pages = new List<BreadCrumbModel> {
                 BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, GetApplicationDetailUrl(applicationId)),
                BreadCrumbModel.Create(PageNames.ApplicationDependencyDetail)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateApplicationDependencyDetailModel(applicationId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateApplicationDependencyEditModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationId = 12;
            var applicationDependencyId = 1;
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, GetApplicationDetailUrl(applicationId)),
                BreadCrumbModel.Create(PageNames.ApplicationDependencyDetail, GetApplicationDependencyDetailUrl(applicationDependencyId)),
                BreadCrumbModel.Create(PageNames.ApplicationDependencyEdit)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateApplicationDependencyEditModel(applicationId, applicationDependencyId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        [Test]
        public void CreateApplicationRedmineProjectsModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.ApplicationRedmineProjects, GetApplicationRedmineProjectsUrl())
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Redmine,
                PageList = pages
            };

            // Act
            var result = factory.CreateApplicationRedmineProjectsModel();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateDatabaseRedmineProjectsModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.DatabaseRedmineProjects, GetDatabaseRedmineProjectsUrl())
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Redmine,
                PageList = pages
            };

            // Act
            var result = factory.CreateDatabaseRedmineProjectsModel();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #region nuget package depency

        [Test]
        public void CreateNugetPackageDependencyDetailModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationId = 12;
            var pages = new List<BreadCrumbModel> {
                 BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                 BreadCrumbModel.Create(PageNames.ApplicationInformation, GetApplicationDetailUrl(applicationId)),
                 BreadCrumbModel.Create(PageNames.NugetPackageDependency)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateNugetPackageDependencyDetailModel(applicationId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void CreateNugetPackageDependencyAddModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationId = 12;
            var pages = new List<BreadCrumbModel> {
                 BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                 BreadCrumbModel.Create(PageNames.ApplicationInformation, GetApplicationDetailUrl(applicationId)),
                 BreadCrumbModel.Create(PageNames.NewNugetPackageDependency)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateNugetPackageDependencyAddModel(applicationId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region favourite pages

        [Test]
        public void CreateFavouritePagesModel_NoCondition_ReturnModel()
        {
            // Arrange
            var pages = new List<BreadCrumbModel> {
                 BreadCrumbModel.Create(PageNames.FavouritePages)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.User,
                PageList = pages
            };

            // Act
            var result = factory.CreateFavouritePagesModel();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region create application build settings

        [Test]
        public void CreateApplicationBuildSettingsModel_NoCondition_ReturnModel()
        {
            // Arrange
            var applicationId = 6;
            var applicationDetailUrl = GetApplicationDetailUrl(applicationId);
            var pages = new List<BreadCrumbModel> {
                BreadCrumbModel.Create(PageNames.Applications, GetApplicationUrl()),
                BreadCrumbModel.Create(PageNames.ApplicationInformation, applicationDetailUrl),
                BreadCrumbModel.Create(PageNames.ApplicationBuildSettings)
            };
            var expectedResult = new BreadCrumbViewModel
            {
                ModuleName = ModuleNames.Application,
                PageList = pages
            };

            // Act
            var result = factory.CreateApplicationBuildSettingsModel(applicationId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region helper methods

        string GetApplicationUrl()
        {
            var applicationsUrl = "app";
            urlHelper.Setup(x => x.Action(ApplicationControllerActionNames.Index, ControllerNames.Application)).Returns(applicationsUrl);
            return applicationsUrl;
        }

        string GetAddApplicationUrl()
        {
            var applicationUrl = "app-Url";
            urlHelper.Setup(x => x.Action(ApplicationControllerActionNames.Add, ControllerNames.Application)).Returns(applicationUrl);
            return applicationUrl;
        }

        string GetApplicationDetailUrl(int applicationId)
        {
            var applicationDetailUrl = "app-detail-url";
            urlHelper.Setup(x => x.Action(ApplicationControllerActionNames.Detail, ControllerNames.Application, applicationId)).Returns(applicationDetailUrl);
            return applicationDetailUrl;
        }

        string GetApplicationNugetPackageDetailUrl(int nugetPackageId)
        {
            var applicationNugetPackageDetailUrl = "app-nuget-package-detail-url";
            urlHelper.Setup(x => x.Action(ApplicationNugetPackageControllerActionNames.Detail, ControllerNames.ApplicationNugetPackage, nugetPackageId)).Returns(applicationNugetPackageDetailUrl);

            return applicationNugetPackageDetailUrl;
        }

        string GetApplicationEditUrl(int applicationId)
        {
            var applicationDetailUrl = "app-edit-url";
            urlHelper.Setup(x => x.Action(ApplicationControllerActionNames.Edit, ControllerNames.Application, applicationId)).Returns(applicationDetailUrl);
            return applicationDetailUrl;
        }

        string GetApplicationGroupUrl()
        {
            var applicationGroupsUrl = "appGroups";
            urlHelper.Setup(x => x.Action(ApplicationGroupControllerActionNames.Index, ControllerNames.ApplicationGroup)).Returns(applicationGroupsUrl);
            return applicationGroupsUrl;
        }

        string GetDetailApplicationGroupUrl()
        {
            var applicationDetailUrl = "appGroups";
            urlHelper.Setup(x => x.Action(ApplicationControllerActionNames.Detail, ControllerNames.ApplicationGroup)).Returns(applicationDetailUrl);
            return applicationDetailUrl;
        }

        string GetMenuUrl()
        {
            var menuUrl = "menu";
            urlHelper.Setup(x => x.Action(ApplicationControllerActionNames.Index, ControllerNames.Menu)).Returns(menuUrl);
            return menuUrl;
        }

        string GetAddApplicationGroupUrl()
        {
            var addApplicationGroupUrl = "addApplicationGroupUrl";
            urlHelper.Setup(x => x.Action(ApplicationGroupControllerActionNames.Add, ControllerNames.ApplicationGroup)).Returns(addApplicationGroupUrl);
            return addApplicationGroupUrl;
        }

        string GetUserUrl()
        {
            var userUrl = "userUrl";
            urlHelper.Setup(x => x.Action(UserControllerActionNames.Index, ControllerNames.User)).Returns(userUrl);
            return userUrl;
        }

        string GetSvnUrl()
        {
            var svnUrl = "svnUrl";
            urlHelper.Setup(x => x.Action(ApplicationControllerActionNames.Index, ControllerNames.SvnAdmin)).Returns(svnUrl);
            return svnUrl;
        }

        string GetSvnRepositoryFolderUrl()
        {
            var url = "svnUrl";
            urlHelper.Setup(x => x.Action(SvnAdminControllerActionNames.CreateSvnRepositoryFolder, ControllerNames.SvnAdmin)).Returns(url);
            return url;
        }

        string GetEnvironmentUrl()
        {
            var environmentUrl = "environmentUrl";
            urlHelper.Setup(x => x.Action(EnvironmentControllerActionNames.Index, ControllerNames.Environment)).Returns(environmentUrl);
            return environmentUrl;
        }

        string GetEnvironmentDetailUrl(int environmentId)
        {
            var environmentDetailUrl = "environmentDetailUrl";
            urlHelper.Setup(x => x.Action(EnvironmentControllerActionNames.Detail, ControllerNames.Environment, environmentId)).Returns(environmentDetailUrl);
            return environmentDetailUrl;
        }

        string GetDatabaseUrl()
        {
            var databaseUrl = "db";
            urlHelper.Setup(x => x.Action(DatabaseControllerActionNames.Index, ControllerNames.Database)).Returns(databaseUrl);
            return databaseUrl;
        }

        string GetDatabaseDetailUrl(int databaseId)
        {
            var databaseDetailUrl = "databaseDetailUrl";
            urlHelper.Setup(x => x.Action(DatabaseControllerActionNames.Detail, ControllerNames.Database, databaseId)).Returns(databaseDetailUrl);
            return databaseDetailUrl;
        }

        string GetDatabaseEditUrl(int databaseId)
        {
            var databaseEditUrl = "databaseEditUrl";
            urlHelper.Setup(x => x.Action(DatabaseControllerActionNames.Edit, ControllerNames.Database, databaseId)).Returns(databaseEditUrl);
            return databaseEditUrl;
        }

        string GetDatabaseTypeUrl()
        {
            var databaseTypeUrl = "databaseTypeUrl";
            urlHelper.Setup(x => x.Action(DatabaseTypeControllerActionNames.Index, ControllerNames.DatabaseType)).Returns(databaseTypeUrl);
            return databaseTypeUrl;
        }

        string GetDatabaseTypeDetailUrl(int databaseTypeId)
        {
            var databaseTypeDetailUrl = "app-edit-url";
            urlHelper.Setup(x => x.Action(DatabaseTypeControllerActionNames.Detail, ControllerNames.DatabaseType, databaseTypeId)).Returns(databaseTypeDetailUrl);
            return databaseTypeDetailUrl;
        }

        string GetDatabaseGroupUrl()
        {
            var databaseGroupUrl = "databaseGroupUrl";
            urlHelper.Setup(x => x.Action(DatabaseGroupControllerActionNames.Index, ControllerNames.DatabaseGroup)).Returns(databaseGroupUrl);
            return databaseGroupUrl;
        }

        string GetDatabaseGroupDetailUrl(int databaseGroupId)
        {
            var databaseGroupDetailUrl = "app-detail-url";
            urlHelper.Setup(x => x.Action(DatabaseGroupControllerActionNames.Detail, ControllerNames.DatabaseGroup, databaseGroupId)).Returns(databaseGroupDetailUrl);
            return databaseGroupDetailUrl;
        }

        string GetExternalDependencyDetailUrl(int externalDependencyId)
        {
            var externalDependencyDetailUrl = "external-dependency-detail-url";
            urlHelper.Setup(x => x.Action(ExternalDependencyControllerActionNames.Detail, ControllerNames.ExternalDependency, externalDependencyId)).Returns(externalDependencyDetailUrl);
            return externalDependencyDetailUrl;
        }

        string GetDatabaseDependencyDetailUrl(int databaseDependencyId)
        {
            var databaseDependencyDetailUrl = "database-dependency-detail-url";
            urlHelper.Setup(x => x.Action(DatabaseDependencyControllerActionNames.Detail, ControllerNames.DatabaseDependency, databaseDependencyId)).Returns(databaseDependencyDetailUrl);
            return databaseDependencyDetailUrl;
        }

        string GetApplicationDependencyDetailUrl(int applicationDependencyId)
        {
            var applicaitonDependencyDetailUrl = "application-dependency-detail-url";
            urlHelper.Setup(x => x.Action(ApplicationDependencyControllerActionNames.Detail, ControllerNames.ApplicationDependency, applicationDependencyId)).Returns(applicaitonDependencyDetailUrl);
            return applicaitonDependencyDetailUrl;
        }

        string GetApplicationRedmineProjectsUrl()
        {
            var url = "svnUrl";
            urlHelper.Setup(x => x.Action(RedmineControllerActionNames.Index, ControllerNames.Redmine)).Returns(url);
            return url;
        }

        string GetDatabaseRedmineProjectsUrl()
        {
            var url = "svnUrl";
            urlHelper.Setup(x => x.Action(RedmineControllerActionNames.DatabaseProjects, ControllerNames.Redmine)).Returns(url);
            return url;
        }

        #endregion
    }
}