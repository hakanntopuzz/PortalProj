using AB.Framework.UnitTests;
using DevPortal.Data;
using DevPortal.Data.Factories;
using DevPortal.JenkinsManager.Model;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace DevPortal.UnitTests.DataTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class JenkinsFactoryTests : LooseBaseTestFixture
    {
        #region members & setup

        JenkinsFactory factory;

        [SetUp]
        public void Initialize()
        {
            factory = new JenkinsFactory();
        }

        #endregion

        #region GetApplicationEnvironments

        [Test]
        public void GetApplicationEnvironments_NoCondition_ReturnDataRequest()
        {
            //Arrange
            string name = "name";
            Uri url = new Uri("http://url.com");
            string color = JenkinsStatusColorNames.Red;

            //Act
            var result = factory.CreateJenkinsJobItem(name, url, color);

            //Assert
            var expectedResult = new JenkinsJobItem
            {
                Name = name,
                Url = url,
                Color = color
            };

            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion
    }
}