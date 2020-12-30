using AB.Framework.UnitTests;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.WebLibraryTests.Models
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ControllerNamesTest : LooseBaseTestFixture
    {
        [Test]
        public void Home_NoCondition_ReturnStringHome()
        {
            //Act
            var result = ControllerNames.Home;

            //Assert
            result.Should().Be("home");
        }

        [Test]
        public void Security_NoCondition_ReturnStringSecurity()
        {
            //Act
            var result = ControllerNames.Security;

            //Assert
            result.Should().Be("security");
        }

        [Test]
        public void Log_NoCondition_ReturnStringLog()
        {
            //Act
            var result = ControllerNames.Log;

            //Assert
            result.Should().Be("log");
        }
    }
}