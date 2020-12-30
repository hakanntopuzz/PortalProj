using AB.Framework.UnitTests;
using DevPortal.Data;
using DevPortal.JenkinsManager.Business.Factories;
using DevPortal.JenkinsManager.Model;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.BusinessTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class JenkinsJobFactoryTests : LooseBaseTestFixture
    {
        #region members & setup

        JenkinsJobFactory factory;

        [SetUp]
        public void Initialize()
        {
            factory = new JenkinsJobFactory();
        }

        #endregion

        #region Create Jenkins Job Item

        [Test]
        public void CreateJenkinsJobItem_JenkinsJobItemNull_ReturnNull()
        {
            // Arrange
            JenkinsJobItem jobItem = null;
            var application = new Application();

            // Act
            var result = factory.CreateJenkinsJobItem(jobItem, application);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void CreateJenkinsJobItem_ApplicationNull_ReturnJenkinsJobItemModel()
        {
            // Arrange
            var jobItem = new JenkinsJobItem { Color = JenkinsStatusColorNames.Blue };
            Application application = null;

            var model = new JenkinsJobItem
            {
                Name = jobItem.Name,
                Url = jobItem.Url,
                Color = jobItem.Color,
                ApplicationId = 0,
                ApplicationName = ""
            };

            // Act
            var result = factory.CreateJenkinsJobItem(jobItem, application);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(model);
            result.Name.Should().Be(jobItem.Name);
            result.Url.Should().Be(jobItem.Url);
            result.Color.Should().Be(jobItem.Color);
        }

        [Test]
        public void CreateJenkinsJobItem_JenkinsJobItemAndApplicationExists_ReturnJenkinsJobItemModelWithApplicationInfo()
        {
            // Arrange
            var jobItem = new JenkinsJobItem { Color = JenkinsStatusColorNames.Blue };
            var application = new Application();

            var model = new JenkinsJobItem
            {
                Name = jobItem.Name,
                Url = jobItem.Url,
                Color = jobItem.Color,
                ApplicationId = application.Id,
                ApplicationName = application.Name
            };

            // Act
            var result = factory.CreateJenkinsJobItem(jobItem, application);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(model);
            result.Name.Should().Be(jobItem.Name);
            result.Url.Should().Be(jobItem.Url);
            result.Color.Should().Be(jobItem.Color);
            result.ApplicationId.Should().Be(application.Id);
            result.ApplicationName.Should().Be(application.Name);
        }

        #endregion
    }
}