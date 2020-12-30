using AB.Framework.UnitTests;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.WebLibraryTests.Models
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ClientActionResultTests : LooseBaseTestFixture
    {
        #region success

        [Test]
        public void Success_WithoutMessage_ReturnSuccessWithoutMessage()
        {
            //Arrange

            //Act
            var result = ClientActionResult.Success();

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().BeNull();
        }

        [Test]
        public void SuccessWithMessage_WithMessage_ReturnSuccessWithMessage()
        {
            //Arrange
            string message = "test";

            //Act
            var result = ClientActionResult.Success(message);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be(message);
        }

        #endregion

        #region error

        [Test]
        public void Error_NoCondition_ReturnError()
        {
            //Arrange
            string message = "test";

            //Act
            var result = ClientActionResult.Error(message);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(message);
        }

        #endregion
    }
}