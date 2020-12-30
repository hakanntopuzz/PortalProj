using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Framework;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DatabaseExportServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDatabaseReaderService> databaseReaderService;

        StrictMock<IDatabaseFactory> databaseFactory;

        StrictMock<ICsvService> csvService;

        StrictMock<IDatabaseWikiExportService> databaseWikiExportService;

        DatabaseExportService service;

        [SetUp]
        public void Initialize()
        {
            databaseReaderService = new StrictMock<IDatabaseReaderService>();
            databaseFactory = new StrictMock<IDatabaseFactory>();
            csvService = new StrictMock<ICsvService>();
            databaseWikiExportService = new StrictMock<IDatabaseWikiExportService>();

            service = new DatabaseExportService(
                databaseReaderService.Object,
                databaseFactory.Object,
                csvService.Object,
                databaseWikiExportService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            databaseReaderService.VerifyAll();
            csvService.VerifyAll();
        }

        #endregion

        [Test]
        public void ExportDatabasesAsCsv_DatabaseGroupIdIsZeroAndDatabaseNameIsEmpty_ReturnDatabase()
        {
            // Arrange
            var databaseGroupId = 0;
            var databaseName = "";
            var database = new Database();
            byte[] encodedBytes = { 1, 2, 3 };
            ICollection<Database> databases = new List<Database> { database };

            databaseReaderService.Setup(x => x.FilterDatabases(databaseGroupId, databaseName)).Returns(databases);
            csvService.Setup(x => x.ExportToCsv(SetupAny<ICollection<DatabaseExportListItem>>(), CsvColumnNames.DatabaseList)).Returns(encodedBytes);

            // Act
            var result = service.ExportDatabasesAsCsv(databaseGroupId, databaseName);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().BeNull();
            result.Value.Should().BeSameAs(encodedBytes);
        }

        [Test]
        public void ExportDatabasesAsWikiText_NoCondition_ReturnWikiText()
        {
            // Arrange
            int databaseGroupId = 14;
            string databaseName = "name";

            var database = new Database();
            var databases = new List<Database> { database };
            var databaseWikiExportListItems = new List<DatabaseWikiExportListItem>();
            var databasesWikiText = "databases-wiki";

            databaseReaderService.Setup(x => x.FilterDatabases(databaseGroupId, databaseName)).Returns(databases);
            databaseFactory.Setup(x => x.CreateDatabaseWikiExportListItems(databases)).Returns(databaseWikiExportListItems);
            databaseWikiExportService.Setup(x => x.ExportDatabasesAsWikiText(databaseWikiExportListItems)).Returns(databasesWikiText);

            // Act
            var result = service.ExportDatabasesAsWikiText(databaseGroupId, databaseName);

            // Assert
            result.Should().Be(databasesWikiText);
        }

        [Test]
        public void ExportDatabasesAsWikiTextFile_NoCondition_ReturnWikiText()
        {
            // Arrange
            int databaseGroupId = 14;
            string databaseName = "name";

            var database = new Database();
            var databases = new List<Database> { database };
            var databaseWikiExportListItems = new List<DatabaseWikiExportListItem>();
            var databasesWikiText = "databases-wiki";

            databaseReaderService.Setup(x => x.FilterDatabases(databaseGroupId, databaseName)).Returns(databases);
            databaseFactory.Setup(x => x.CreateDatabaseWikiExportListItems(databases)).Returns(databaseWikiExportListItems);
            databaseWikiExportService.Setup(x => x.ExportDatabasesAsWikiText(databaseWikiExportListItems)).Returns(databasesWikiText);

            // Act
            var result = service.ExportDatabasesAsWikiTextFile(databaseGroupId, databaseName);

            // Assert
            var expectedData = new FileExportData
            {
                FileData = Encoding.UTF8.GetBytes(databasesWikiText),
                FileDownloadName = $"veritabani-listesi-wiki.txt",
                ContentType = ContentTypes.Txt
            };
            result.Should().BeEquivalentTo(expectedData);
        }
    }
}