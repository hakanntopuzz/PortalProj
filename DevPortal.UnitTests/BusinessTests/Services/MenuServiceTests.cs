using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class MenuServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IMenuRepository> menuRepository;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        MenuService service;

        [SetUp]
        public void Initialize()
        {
            menuRepository = new StrictMock<IMenuRepository>();
            auditService = new StrictMock<IAuditService>();
            auditFactory = new StrictMock<IAuditFactory>();
            loggingService = new StrictMock<ILoggingService>();

            service = new MenuService(
                menuRepository.Object,
                auditService.Object,
                auditFactory.Object,
                loggingService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            menuRepository.VerifyAll();
            auditService.VerifyAll();
            auditFactory.VerifyAll();
            loggingService.VerifyAll();
        }

        #endregion

        #region get filtered menu list

        [Test]
        public async Task GetFilteredMenuListAsync_NoCondition_ReturnMenuModelList()
        {
            // Arrange
            MenuTableParam tableParam = null;
            ICollection<MenuModel> menuList = null;

            menuRepository.Setup(x => x.GetFilteredMenuListAsync(tableParam)).ReturnsAsync(menuList);

            // Act
            var result = await service.GetFilteredMenuListAsync(tableParam);

            // Assert
            result.Should().BeSameAs(menuList);
        }

        #endregion

        #region get filtered jq table

        [Test]
        public async Task GetFilteredUsersJqTableAsync_NoCondition_ReturnUser()
        {
            // Arrange
            var users = new List<MenuModel>();
            var tableParam = new MenuTableParam();
            var jqtable = new JQTable
            {
                data = users,
                recordsFiltered = 0,
                recordsTotal = 0
            };

            menuRepository.Setup(x => x.GetFilteredMenuListAsync(tableParam)).ReturnsAsync(users);

            // Act
            var result = await service.GetFilteredMenuListAsJqTableAsync(tableParam);

            // Assert
            result.Should().BeEquivalentTo(jqtable);
        }

        #endregion

        #region get menu list

        [Test]
        public async Task GetMenuListAsync_NoCondition_ReturnMenuModelList()
        {
            // Arrange
            ICollection<MenuModel> menuList = null;

            menuRepository.Setup(x => x.GetMenuListAsync()).ReturnsAsync(menuList);

            // Act
            var result = await service.GetMenuListAsync();

            // Assert
            result.Should().BeSameAs(menuList);
        }

        #endregion

        #region add menu

        [Test]
        public async Task AddMenuAsync_MenuIsNull_ReturnError()
        {
            // Arrange
            MenuModel menu = null;

            // Act
            var result = await service.AddMenuAsync(menu);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.NullParameterError);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task AddMenuAsync_IsNotSuccess_ReturnError()
        {
            // Arrange
            var menu = new MenuModel();
            var isSuccess = false;

            menuRepository.Setup(x => x.AddMenuAsync(menu)).ReturnsAsync(isSuccess);

            // Act
            var result = await service.AddMenuAsync(menu);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.AddingFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task AddMenuAsync_Success_ReturnSuccess()
        {
            // Arrange
            var menu = new MenuModel();
            var isSuccess = true;

            menuRepository.Setup(x => x.AddMenuAsync(menu)).ReturnsAsync(isSuccess);

            // Act
            var result = await service.AddMenuAsync(menu);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.MenuCreated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region get menu

        [Test]
        public async Task GetMenuAsync_MenuIsNull_ReturnNull()
        {
            // Arrange
            var id = 3;
            MenuModel menu = null;

            menuRepository.Setup(x => x.GetMenuAsync(id)).ReturnsAsync(menu);

            // Act
            var result = await service.GetMenuAsync(id);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task GetMenuAsync_NoCondition_ReturnMenuModel()
        {
            // Arrange
            var id = 3;
            var menu = new MenuModel();
            var menuUpdateInfo = new RecordUpdateInfo();

            menuRepository.Setup(x => x.GetMenuAsync(id)).ReturnsAsync(menu);
            menuRepository.Setup(x => x.GetMenuUpdateInfoAsync(id)).ReturnsAsync(menuUpdateInfo);

            // Act
            var result = await service.GetMenuAsync(id);

            // Assert
            result.Should().Be(menu);
        }

        #endregion

        #region get sub menu list

        [Test]
        public async Task GetSubMenuListAsync_NoCondition_ReturnMenuModel()
        {
            // Arrange
            var id = 3;
            var menuList = new List<MenuModel>();

            menuRepository.Setup(x => x.GetSubMenuListAsync(id)).ReturnsAsync(menuList);

            // Act
            var result = await service.GetSubMenuListAsync(id);

            // Assert
            result.Should().BeSameAs(menuList);
        }

        #endregion

        #region get sub menu list

        [Test]
        public void GetMenuListAsParentChild_NoCondition_ReturnMenuModel()
        {
            // Arrange
            var menuModels = new List<MenuModel> { };
            var menuList = new List<Menu>();

            menuRepository.Setup(x => x.GetMenuListAsync()).ReturnsAsync(menuModels);

            // Act
            var result = service.GetMenuListAsParentChild();

            // Assert
            result.Should().BeEquivalentTo(menuList);
        }

        [Test]
        public void GetMenuListAsParentChild_ParentIdEqual_ReturnNewMenuModel()
        {
            // Arrange
            var menuModels = new List<MenuModel> { new MenuModel { Id = 1, ParentId = null, Name = "name", Link = "Link", MenuGroupId = 1, Icon = "Icon" } };
            var menuList = new List<Menu> { new Menu { Name = "name", Link = "Link", Group = 1, Icon = "Icon" } };

            menuRepository.Setup(x => x.GetMenuListAsync()).ReturnsAsync(menuModels);

            // Act
            var result = service.GetMenuListAsParentChild();

            // Assert
            result.Should().NotBeNull();
        }

        #endregion

        #region update menu

        [Test]
        public async Task UpdateMenuAsync_MenuIsNull_ReturnError()
        {
            // Arrange
            MenuModel menu = null;

            // Act
            var result = await service.UpdateMenuAsync(menu);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.NullParameterError);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task UpdateMenuAsync_HasNoChange_ReturnSuccess()
        {
            // Arrange
            var menu = new MenuModel();
            MenuModel oldMenu = null;

            menuRepository.Setup(x => x.GetMenuAsync(menu.Id)).ReturnsAsync(oldMenu);
            auditService.Setup(x => x.IsChanged(oldMenu, menu, nameof(BaseMenu))).Returns(false);

            // Act
            var result = await service.UpdateMenuAsync(menu);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.MenuUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task UpdateMenuAsync_IsNotSuccess_ReturnError()
        {
            // Arrange
            var menu = new MenuModel();
            MenuModel oldMenu = null;

            menuRepository.Setup(x => x.GetMenuAsync(menu.Id)).ReturnsAsync(oldMenu);
            auditService.Setup(x => x.IsChanged(oldMenu, menu, nameof(BaseMenu))).Returns(true);
            menuRepository.Setup(x => x.UpdateMenuAsync(menu)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = await service.UpdateMenuAsync(menu);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task UpdateMenuAsync_AddAuditFalse_ReturnError()
        {
            // Arrange
            var menu = new MenuModel();
            var oldMenu = new MenuModel();
            var auditInfo = new AuditInfo();

            menuRepository.Setup(x => x.GetMenuAsync(menu.Id)).ReturnsAsync(oldMenu);
            auditService.Setup(x => x.IsChanged(oldMenu, menu, nameof(BaseMenu))).Returns(true);
            menuRepository.Setup(x => x.UpdateMenuAsync(menu)).ReturnsAsync(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(Menu), menu.Id, oldMenu, oldMenu, menu.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = await service.UpdateMenuAsync(menu);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task UpdateMenuAsync_IsSuccess_ReturnOk()
        {
            // Arrange
            var menu = new MenuModel();
            var oldMenu = new MenuModel();
            var auditInfo = new AuditInfo();

            menuRepository.Setup(x => x.GetMenuAsync(menu.Id)).ReturnsAsync(oldMenu);
            auditService.Setup(x => x.IsChanged(oldMenu, menu, nameof(BaseMenu))).Returns(true);
            menuRepository.Setup(x => x.UpdateMenuAsync(menu)).ReturnsAsync(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(Menu), menu.Id, oldMenu, oldMenu, menu.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = await service.UpdateMenuAsync(menu);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.MenuUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region delete menu

        [Test]
        public async Task DeleteMenuAsync_Fails_ReturnFalse()
        {
            // Arrange
            var menuId = 3;
            var menu = new MenuModel();

            menuRepository.Setup(x => x.DeleteMenuAsync(menuId)).ReturnsAsync(false);

            // Act
            var result = await service.DeleteMenuAsync(menuId);

            // Assert
            result.Should().BeEquivalentTo(ServiceResult.Error(Messages.DeleteFails));
        }

        [Test]
        public async Task DeleteMenuAsync_Success_ReturnTrue()
        {
            // Arrange
            var menuId = 3;
            var menu = new MenuModel();

            menuRepository.Setup(x => x.DeleteMenuAsync(menuId)).ReturnsAsync(true);

            // Act
            var result = await service.DeleteMenuAsync(menuId);

            // Assert
            result.Should().BeEquivalentTo(ServiceResult.Success(Messages.MenuDeleted));
        }

        #endregion

        #region get menu groups

        [Test]
        public async Task GetMenuGroupsAsync_NoCondition_ReturnMenuModel()
        {
            // Arrange
            var menuList = new List<MenuGroup>();

            menuRepository.Setup(x => x.GetMenuGroupsAsync()).ReturnsAsync(menuList);

            // Act
            var result = await service.GetMenuGroupsAsync();

            // Assert
            result.Should().BeSameAs(menuList);
        }

        #endregion
    }
}