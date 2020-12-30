using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Abstract.Services;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using DevPortal.Web.Library.ViewComponents;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.ViewComponents
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class NavbarViewComponentTests : BaseTestFixture
    {
        #region members & setup

        NavbarViewComponent component;

        StrictMock<IMenuService> menuService;

        [SetUp]
        public void Initialize()
        {
            menuService = new StrictMock<IMenuService>();

            component = new NavbarViewComponent(
                menuService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            menuService.VerifyAll();
        }

        #endregion

        [Test]
        public void Invoke_NoCondition_ReturnViewComponentResult()
        {
            // Arrange
            var menus = new List<Menu>();

            menuService.Setup(x => x.GetMenuListAsParentChild()).Returns(menus);

            // Act
            var result = component.Invoke();

            // Assert
            result.Should().NotBeNull();
        }
    }
}

