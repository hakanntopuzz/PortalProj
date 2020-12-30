using AB.Framework.UnitTests;
using DevPortal.Framework;
using DevPortal.Framework.Factories;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.FrameworkTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class RouteValueFactoryTests : LooseBaseTestFixture
    {
        #region members & setup

        RouteValueFactory factory;

        [SetUp]
        public void Initialize()
        {
            factory = new RouteValueFactory();
        }

        #endregion

        [Test]
        public void CreateRouteValuesForGenerateUrl_NoCondition_ReturnRouteData()
        {
            // Arrange
            int id = 42;

            // Act
            var result = factory.CreateRouteValuesForGenerateUrl(id);

            // Assert
            result.Keys.Should().HaveCount(1);
            result.ContainsKey(WebParameterNames.id).Should().BeTrue();
            result[WebParameterNames.id].Should().Be(id);
        }

        [Test]
        public void CreateRouteValuesForToken_NoCondition_ReturnRouteData()
        {
            // Arrange
            string token = "token";

            // Act
            var result = factory.CreateRouteValuesForToken(token);

            // Assert
            result.Keys.Should().HaveCount(1);
            result.ContainsKey(WebParameterNames.token).Should().BeTrue();
            result[WebParameterNames.token].Should().Be(token);
        }

        [Test]
        public void CreateRouteValueForId_NoCondition_ReturnRouteData()
        {
            // Arrange
            int id = 42;

            // Act
            var result = factory.CreateRouteValueForId(id);

            // Assert
            result.Keys.Should().HaveCount(1);
            result.ContainsKey(WebParameterNames.id).Should().BeTrue();
            result[WebParameterNames.id].Should().Be(id);
        }
    }
}