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
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class MenuControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IMenuService> menuService;

        StrictMock<IMenuViewModelFactory> menuViewModelFactory;

        StrictMock<IUrlGeneratorService> urlHelper;

        MenuController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            menuService = new StrictMock<IMenuService>();
            menuViewModelFactory = new StrictMock<IMenuViewModelFactory>();
            urlHelper = new StrictMock<IUrlGeneratorService>();

            controller = new MenuController(
                userSessionService.Object,
                menuService.Object,
                menuViewModelFactory.Object,
                urlHelper.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            menuService.VerifyAll();
            menuViewModelFactory.VerifyAll();
            userSessionService.VerifyAll();
            urlHelper.VerifyAll();
        }

        #endregion

        #region helpers

        public void SetResultMessageTempData(Dictionary<string, string> tempDataKeyValuePairs)
        {
            controller.TempData = SetupHelpers.CreateResultMessageTempData(tempDataKeyValuePairs);
        }

        #endregion

        #region index

        [Test]
        public void Index_NoCondition_ReturnView()
        {
            MenuListViewModel viewModel = null;

            // Arrange
            menuViewModelFactory.Setup(x => x.CreateMenuListViewModel()).Returns(viewModel);

            // Act
            var result = controller.Index();

            // Assert
            result.Should().BeViewResult();
        }

        [Test]
        public async Task Index_GetAllMenu_ReturnView()
        {
            MenuTableParam tableParam = null;
            var jqTable = new JQTable();

            // Arrange
            menuService.Setup(x => x.GetFilteredMenuListAsJqTableAsync(tableParam)).ReturnsAsync(jqTable);

            // Act
            var result = await controller.Index(tableParam);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region add

        [Test]
        public async Task Add_NoCondition_CreateAddViewModelAndReturnAddView()
        {
            // Arrange
            var viewModel = new AddMenuViewModel();
            IEnumerable<MenuModel> allMenu = null;
            IEnumerable<MenuGroup> menuGroups = null;

            menuService.Setup(x => x.GetMenuListAsync()).ReturnsAsync(allMenu);
            menuService.Setup(x => x.GetMenuGroupsAsync()).ReturnsAsync(menuGroups);
            menuViewModelFactory.Setup(x => x.CreateAddMenuViewModel(allMenu, menuGroups)).Returns(viewModel);

            // Act
            var result = await controller.Add();

            // Assert
            result.Should().BeViewResult(ViewNames.Add)
                .ModelAs<AddMenuViewModel>()
                .Should().Be(viewModel);
        }

        [Test]
        public async Task Add_ModelStateIsNotValid_ReturnAddView()
        {
            // Arrange
            var menuModel = new MenuModel();
            IEnumerable<MenuModel> allMenu = null;
            IEnumerable<MenuGroup> menuGroups = null;
            var viewModel = new AddMenuViewModel();

            controller.ModelState.AddModelError("", "invalid model");

            menuService.Setup(x => x.GetMenuListAsync()).ReturnsAsync(allMenu);
            menuService.Setup(x => x.GetMenuGroupsAsync()).ReturnsAsync(menuGroups);
            menuViewModelFactory.Setup(x => x.CreateAddMenuViewModel(allMenu, menuGroups)).Returns(viewModel);

            // Act
            var result = await controller.Add(menuModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<AddMenuViewModel>().Should().Be(viewModel);
            menuService.Verify(x => x.AddMenuAsync(menuModel), Times.Never);
        }

        [Test]
        public async Task Add_AddingFails_SetErrorMessageToTempDataAndReturnAddView()
        {
            // Arrange
            var menuModel = new MenuModel();

            IEnumerable<MenuModel> allMenu = null;
            IEnumerable<MenuGroup> menuGroups = null;
            var viewModel = new AddMenuViewModel();
            var addResult = ServiceResult.Error();
            var userId = 3;

            menuService.Setup(x => x.GetMenuListAsync()).ReturnsAsync(allMenu);
            menuService.Setup(x => x.GetMenuGroupsAsync()).ReturnsAsync(menuGroups);
            menuViewModelFactory.Setup(x => x.CreateAddMenuViewModel(allMenu, menuGroups)).Returns(viewModel);
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            menuService.Setup(x => x.AddMenuAsync(menuModel)).ReturnsAsync(addResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Add(menuModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<AddMenuViewModel>().Should().Be(viewModel);
            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        [Test]
        public async Task Add_ServiceReturnsSuccess_SetSuccessMessageToTempDataAndReturnIndexView()
        {
            // Arrange
            var menuModel = new MenuModel();
            var userId = 3;

            var addResult = ServiceResult.Success();
            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            menuService.Setup(x => x.AddMenuAsync(menuModel)).ReturnsAsync(addResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, addResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Add(menuModel);

            // Assert
            result.Should().BeRedirectToActionResult().WithActionName(ApplicationControllerActionNames.Index);

            AssertHelpers.AssertResultMessageTempData(controller, expectedTempData);
        }

        #endregion

        #region detail

        [Test]
        public async Task Detail_MenuNotFound_RedirectToIndexAndSetErrorMessageTempData()
        {
            // Arrange
            var id = 86;
            MenuModel menu = null;

            menuService.Setup(x => x.GetMenuAsync(id)).ReturnsAsync(menu);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.MenuNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Detail(id);

            // Assert
            result.Should().BeRedirectToActionResult()
                .WithActionName(ApplicationGroupControllerActionNames.Index)
                .WithControllerName(ControllerNames.Menu);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public async Task Detail_MenuExists_ReturnMenuDetail()
        {
            // Arrange
            var id = 1;
            var menu = new MenuModel();
            var subMenuList = new List<MenuModel>();
            var viewModel = new MenuDetailViewModel();

            menuService.Setup(x => x.GetMenuAsync(id)).ReturnsAsync(menu);
            menuService.Setup(x => x.GetSubMenuListAsync(id)).ReturnsAsync(subMenuList);
            menuViewModelFactory.Setup(x => x.CreateMenuDetailViewModel(menu, subMenuList)).Returns(viewModel);

            // Act
            var result = await controller.Detail(id);

            // Assert
            result.Should().BeViewResult(ViewNames.Detail)
                .ModelAs<MenuDetailViewModel>()
                .Should().Be(viewModel);
        }

        #endregion

        #region edit- get

        [Test]
        public async Task Edit_MenuIsNull_CreateAddViewModelAndReturnEditView()
        {
            // Arrange
            var id = 86;
            MenuModel menu = null;

            menuService.Setup(x => x.GetMenuAsync(id)).ReturnsAsync(menu);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, Messages.MenuNotFound);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Edit(id);

            // Assert
            result.Should().BeRedirectToActionResult()
                .WithActionName(ApplicationGroupControllerActionNames.Index)
                .WithControllerName(ControllerNames.Menu);

            controller.TempData[nameof(TempDataKeys.ResultMessage)].Should().BeEquivalentTo(expectedTempData);
        }

        [Test]
        public async Task Edit_MenuExists_ReturnMenuEdit()
        {
            // Arrange
            var id = 1;
            var menu = new MenuModel();
            var menuList = new List<MenuModel>();
            var viewModel = new EditMenuViewModel();
            var menuGroups = new List<MenuGroup>();

            menuService.Setup(x => x.GetMenuAsync(id)).ReturnsAsync(menu);
            menuService.Setup(x => x.GetMenuListAsync()).ReturnsAsync(menuList);
            menuService.Setup(x => x.GetMenuGroupsAsync()).ReturnsAsync(menuGroups);
            menuViewModelFactory.Setup(x => x.CreateEditMenuViewModel(menu, menuList, menuGroups)).Returns(viewModel);

            // Act
            var result = await controller.Edit(id);

            // Assert
            result.Should().BeViewResult(ViewNames.Edit)
                .ModelAs<EditMenuViewModel>()
                .Should().Be(viewModel);
        }

        #endregion

        #region edit - post

        [Test]
        public async Task Edit_ModelStateIsNotValid_ReturnEditView()
        {
            // Arrange
            var menuModel = new MenuModel();
            IEnumerable<MenuModel> allMenu = null;
            IEnumerable<MenuGroup> menuGroups = null;
            var viewModel = new EditMenuViewModel();

            controller.ModelState.AddModelError("", "invalid model");
            menuService.Setup(x => x.GetMenuListAsync()).ReturnsAsync(allMenu);
            menuService.Setup(x => x.GetMenuGroupsAsync()).ReturnsAsync(menuGroups);
            menuViewModelFactory.Setup(x => x.CreateAddMenuViewModel(allMenu, menuGroups)).Returns(viewModel);

            // Act
            var result = await controller.Edit(menuModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Add).ModelAs<AddMenuViewModel>().Should().Be(viewModel);
        }

        [Test]
        public async Task Edit_UpdateResultFails_ReturnEditView()
        {
            // Arrange
            var menuModel = new MenuModel();
            var userId = 11;
            var serviceResult = ServiceResult.Error();
            IEnumerable<MenuModel> allMenu = null;
            IEnumerable<MenuGroup> menuGroups = null;
            var viewModel = new EditMenuViewModel();

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            menuService.Setup(x => x.UpdateMenuAsync(menuModel)).ReturnsAsync(serviceResult);
            menuService.Setup(x => x.GetMenuListAsync()).ReturnsAsync(allMenu);
            menuService.Setup(x => x.GetMenuGroupsAsync()).ReturnsAsync(menuGroups);
            menuViewModelFactory.Setup(x => x.CreateEditMenuViewModel(menuModel, allMenu, menuGroups)).Returns(viewModel);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Edit(menuModel);

            // Assert
            result.Should().BeViewResult(ViewNames.Edit).ModelAs<AddMenuViewModel>().Should().Be(viewModel);
        }

        [Test]
        public async Task Edit_UpdateResultSuccess_ReturnEditView()
        {
            // Arrange
            var menuModel = new MenuModel();
            var userId = 11;
            var serviceResult = ServiceResult.Success();

            userSessionService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            menuService.Setup(x => x.UpdateMenuAsync(menuModel)).ReturnsAsync(serviceResult);

            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Error, serviceResult.Message);
            SetResultMessageTempData(expectedTempData);

            // Act
            var result = await controller.Edit(menuModel);

            // Assert
            AssertHelpers.AssertRedirectToAction(result, ApplicationControllerActionNames.Index, ControllerNames.Menu);
        }

        #endregion

        #region delete

        [Test]
        public void Delete_Error_ReturnErrorResult()
        {
            //Arrange
            var id = 1;
            var redirectUrl = $"/application/index";
            var deleteResult = ServiceResult.Error();

            menuService.Setup(x => x.DeleteMenuAsync(id)).ReturnsAsync(deleteResult);
            urlHelper.Setup(x => x.GenerateUrl(ApplicationControllerActionNames.Index, ControllerNames.Menu)).Returns(redirectUrl);

            //Act
            var result = controller.Delete(id);

            //Assert
            var expectedResult = RedirectableClientActionResult.Error(redirectUrl);
            var resultModel = result.Should().BeOfType<Task<JsonResult>>().Which.Result.As<RedirectableClientActionResult>();
        }

        [Test]
        public void Delete_Success_ReturnSuccessAndRedirectUrl()
        {
            //Arrange
            var id = 1;
            var redirectUrl = $"/application/index";
            var deleteResult = ServiceResult.Success();

            menuService.Setup(x => x.DeleteMenuAsync(id)).ReturnsAsync(deleteResult);
            urlHelper.Setup(x => x.GenerateUrl(ApplicationControllerActionNames.Index, ControllerNames.Menu)).Returns(redirectUrl);
            var expectedTempData = SetupHelpers.CreateResultMessageForTempData(MessageType.Success, deleteResult.Message);
            SetResultMessageTempData(expectedTempData);

            //Act
            var result = controller.Delete(id);

            //Assert
            var expectedResult = RedirectableClientActionResult.Success(redirectUrl, deleteResult.Message);
            var resultModel = result.Should().BeOfType<Task<JsonResult>>().Which.Result.As<RedirectableClientActionResult>();
        }

        #endregion

        #region get menu for jenkins and nuget module

        [Test]
        public void GetMenu_NoCondition_ReturnNugetPackages()
        {
            // Arrange
            var menus = new List<Menu>();

            menuService.Setup(x => x.GetMenuListAsParentChild()).Returns(menus);

            // Act
            var result = controller.MenuList();

            // Assert
            result.Should().NotBeNull();
        }

        #endregion
    }
}