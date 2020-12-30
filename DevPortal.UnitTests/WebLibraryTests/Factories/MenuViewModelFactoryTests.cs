using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class MenuViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        MenuViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        StrictMock<IAuthorizationServiceWrapper> authorizationServiceWrapper;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();
            authorizationServiceWrapper = new StrictMock<IAuthorizationServiceWrapper>();

            factory = new MenuViewModelFactory(breadCrumbFactory.Object,
                authorizationServiceWrapper.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            breadCrumbFactory.VerifyAll();
            authorizationServiceWrapper.VerifyAll();
        }

        #endregion

        #region menu

        [Test]
        public void CreateMenuListViewModel_NoCondition_ReturnMenuListViewModel()
        {
            // Arrange
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var menuListViewModel = new MenuListViewModel
            {
                BreadCrumbViewModel = breadcrumbViewModel,
                IsAuthorized = true
            };

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateMenuListModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateMenuListViewModel();

            // Assert
            result.Should().BeEquivalentTo(menuListViewModel);
            result.IsAuthorized.Should().BeTrue();
            result.BreadCrumbViewModel.Should().BeSameAs(menuListViewModel.BreadCrumbViewModel);
        }

        [Test]
        public void CreateAddMenuViewModel_NoCondition_ReturnAddMenuViewModel()
        {
            // Arrange
            var allMenu = new List<MenuModel>();
            var menuGroups = new List<MenuGroup>();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var addMenuViewModel = new AddMenuViewModel
            {
                Menu = new MenuModel(),
                MenuList = allMenu,
                BreadCrumbViewModel = breadcrumbViewModel,
                MenuGroups = menuGroups
            };

            breadCrumbFactory.Setup(x => x.CreateAddMenuModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateAddMenuViewModel(allMenu, menuGroups);

            // Assert
            result.Should().BeEquivalentTo(addMenuViewModel);
            result.Menu.Should().BeEquivalentTo(addMenuViewModel.Menu);
            result.MenuList.Should().BeEquivalentTo(addMenuViewModel.MenuList);
            result.BreadCrumbViewModel.Should().BeSameAs(addMenuViewModel.BreadCrumbViewModel);
        }

        [Test]
        public void CreateMenuDetailViewModel_NoCondition_ReturnMenuDetailViewModel()
        {
            // Arrange
            var menu = new MenuModel();
            var subMenuList = new List<MenuModel>();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var menuDetailViewModel = new MenuDetailViewModel
            {
                Menu = menu,
                SubMenuList = subMenuList,
                BreadCrumbViewModel = breadcrumbViewModel,
                IsAuthorized = true
            };

            authorizationServiceWrapper.Setup(s => s.CheckUserHasAdminPolicy()).Returns(true);
            breadCrumbFactory.Setup(x => x.CreateDetailMenuModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateMenuDetailViewModel(menu, subMenuList);

            // Assert
            result.Should().BeEquivalentTo(menuDetailViewModel);
            result.Menu.Should().BeSameAs(menuDetailViewModel.Menu);
            result.SubMenuList.Should().BeEquivalentTo(menuDetailViewModel.SubMenuList);
            result.IsAuthorized.Should().BeTrue();
            result.BreadCrumbViewModel.Should().BeSameAs(menuDetailViewModel.BreadCrumbViewModel);
        }

        [Test]
        public void CreateEditMenuViewModel_NoCondition_ReturnEditMenuViewModel()
        {
            // Arrange
            var menu = new MenuModel();
            var allMenu = new List<MenuModel>();
            var menuGroups = new List<MenuGroup>();
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var editMenuViewModel = new EditMenuViewModel
            {
                Menu = menu,
                MenuList = allMenu,
                BreadCrumbViewModel = breadcrumbViewModel,
                MenuGroups = menuGroups
            };

            breadCrumbFactory.Setup(x => x.CreateEditMenuModel(menu.Id)).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateEditMenuViewModel(menu, allMenu, menuGroups);

            // Assert
            result.Should().BeEquivalentTo(editMenuViewModel);
            result.Menu.Should().BeSameAs(editMenuViewModel.Menu);
            result.MenuList.Should().BeEquivalentTo(editMenuViewModel.MenuList);
            result.BreadCrumbViewModel.Should().BeSameAs(editMenuViewModel.BreadCrumbViewModel);
        }

        #endregion
    }
}