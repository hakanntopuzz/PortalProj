using AB.Framework.UnitTests;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Models
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ClientDataResultTests : LooseBaseTestFixture
    {
        #region success

        [Test]
        public void Success_NoCondition_ReturnSuccessWithData()
        {
            //Arrange
            object data = new List<int>();

            //Act
            var result = ClientDataResult.Success(data);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(data);
            result.Message.Should().BeNull();
        }

        #endregion

        #region error

        [Test]
        public void Error_NoCondition_ReturnError()
        {
            //Arrange
            string message = "test";

            //Act
            var result = ClientDataResult.Error(message);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Message.Should().Be(message);
        }

        #endregion
    }
}