using AB.Framework.UnitTests;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using NUnit.Framework;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SamplesControllerTests : LooseBaseTestFixture
    {
        #region members & setup

        SamplesController controller;

        [SetUp]
        public void Initialize()
        {
            controller = new SamplesController();
        }

        #endregion

        [Test]
        public void Index_NoCondition_ReturnView()
        {
            // Arrange

            //Act
            var result = controller.Index();

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Index).Model.Should().BeNull();
        }

        [Test]
        public void Tables_NoCondition_ReturnView()
        {
            // Arrange

            //Act
            var result = controller.Tables();

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Tables).Model.Should().BeNull();
        }

        [Test]
        public void Notifications_NoCondition_ReturnView()
        {
            // Arrange

            //Act
            var result = controller.Notifications();

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Notifications).Model.Should().BeNull();
        }

        [Test]
        public void Buttons_NoCondition_ReturnView()
        {
            // Arrange

            //Act
            var result = controller.Buttons();

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Buttons).Model.Should().BeNull();
        }

        [Test]
        public void Forms_NoCondition_ReturnView()
        {
            // Arrange

            //Act
            var result = controller.Forms();

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.Forms).Model.Should().BeNull();
        }
    }
}