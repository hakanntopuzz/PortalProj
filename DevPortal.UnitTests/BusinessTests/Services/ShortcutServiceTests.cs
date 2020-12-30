using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ShortcutServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IGeneralSettingsService> generalSettingsService;

        StrictMock<IShortcutRepository> shortcutRepository;

        ShortcutService service;

        [SetUp]
        public void Initialize()
        {
            generalSettingsService = new StrictMock<IGeneralSettingsService>();
            shortcutRepository = new StrictMock<IShortcutRepository>();

            service = new ShortcutService(
                generalSettingsService.Object,
                shortcutRepository.Object
                );
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            generalSettingsService.VerifyAll();
            shortcutRepository.VerifyAll();
        }

        #endregion

        #region get favourite redmine projects

        [Test]
        public void GetFavouriteRedmineProjects_NoCondition_ReturnProjects()
        {
            // Arrange
            var project1Uri = new Uri("http://wwww.example.com/redmine-url/project1");
            var project2Uri = new Uri("http://wwww.example.com/redmine-url/project2");
            var projectLinks = new List<Link>
            {
                new Link { Url = "url-1", Text = "Text-1" },
                new Link { Url = "url-2", Text = "Text-2" }
            };

            var expectedResult = new List<Link>
            {
                new Link { Url = project1Uri.ToString(), Text = projectLinks[0].Text },
                new Link { Url = project2Uri.ToString(), Text = projectLinks[1].Text }
            };

            generalSettingsService.Setup(x => x.GetRedmineProjectUrl(projectLinks[0].Url)).Returns(project1Uri);
            generalSettingsService.Setup(x => x.GetRedmineProjectUrl(projectLinks[1].Url)).Returns(project2Uri);
            shortcutRepository.Setup(x => x.GetFavouriteRedmineProjects()).Returns(projectLinks);

            // Act
            var result = service.GetFavouriteRedmineProjects();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region get favourite redmine wiki pages

        [Test]
        public void GetFavouriteRedmineWikiPages_NoCondition_ReturnPages()
        {
            // Arrange

            var redmineUrl = "http://wwww.example.com/redmine-url";
            var redmineUri = new Uri(redmineUrl);

            var pages = new List<Link>
            {
                new Link { Url = "url-1", Text = "Text-1" },
                new Link { Url = "url-2", Text = "Text-2" }
            };

            var expectedResult = new List<Link>
            {
                new Link { Url = $"{redmineUrl}{pages[0].Url}", Text = pages[0].Text },
                new Link { Url = $"{redmineUrl}{pages[1].Url}", Text = pages[1].Text }
            };

            generalSettingsService.Setup(x => x.GetRedmineUrl()).Returns(redmineUri);
            shortcutRepository.Setup(x => x.GetFavouriteRedmineWikiPages()).Returns(pages);

            // Act
            var result = service.GetFavouriteRedmineWikiPages();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region get favourite redmine wiki pages

        [Test]
        public void GetToolPages_NoCondition_ReturnPages()
        {
            // Arrange

            var pages = new List<Link>
            {
                new Link { Url = "url-1", Text = "Text-1" },
                new Link { Url = "url-2", Text = "Text-2" }
            };

            shortcutRepository.Setup(x => x.GetToolPages()).Returns(pages);

            // Act
            var result = service.GetToolPages();

            // Assert
            result.Should().BeEquivalentTo(pages);
        }

        #endregion
    }
}