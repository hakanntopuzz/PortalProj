using AB.Framework.UnitTests;
using DevPortal.Business.Services;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class WikiTextServiceTests : LooseBaseTestFixture
    {
        #region members & setup

        WikiTextService service;

        [SetUp]
        public void Initialize()
        {
            service = new WikiTextService();
        }

        #endregion

        [Test]
        public void GenerateH2_NoCondition_ReturnWikiText()
        {
            // Arrange
            var headerText = "Header Text";

            // Act
            var result = service.GenerateH2(headerText);

            // Assert
            var expectedWikiText = $"h2. {headerText}\r\n\r\n";
            result.Should().Be(expectedWikiText);
        }

        #region GenerateTableHeader

        [Test]
        public void GenerateTableHeader_ColumnsNull_ReturnEmptyText()
        {
            // Arrange
            string[] columns = null;

            // Act
            var result = service.GenerateTableHeader(columns);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void GenerateTableHeader_ColumnsValid_ReturnWikiText()
        {
            // Arrange
            var columns = new[] { "Column 1", "Column 2" };

            // Act
            var result = service.GenerateTableHeader(columns);

            // Assert
            var expectedWikiText = $"|*{columns[0]}*|*{columns[1]}*|\r\n";
            result.Should().Be(expectedWikiText);
        }

        #endregion

        #region GenerateTableRow

        [Test]
        public void GenerateTableRow_RowDataNull_ReturnEmptyText()
        {
            // Arrange
            string[] rowData = null;

            // Act
            var result = service.GenerateTableRow(rowData);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void GenerateTableRow_NoCondition_ReturnWikiText()
        {
            // Arrange
            var rowData = new[] { "Row Data 1", "Row Data 2" };

            // Act
            var result = service.GenerateTableRow(rowData);

            // Assert
            var expectedWikiText = $"|{rowData[0]}|{rowData[1]}|\r\n";
            result.Should().Be(expectedWikiText);
        }

        #endregion

        [Test]
        public void GenerateWikiLink_NoCondition_ReturnWikiText()
        {
            // Arrange
            string redmineProjectName = "project-name";
            string linkText = "link-text";

            // Act
            var result = service.GenerateWikiLink(redmineProjectName, linkText);

            // Assert
            var expectedWikiText = $"[[{redmineProjectName}:|{linkText}]]";
            result.Should().Be(expectedWikiText);
        }
    }
}