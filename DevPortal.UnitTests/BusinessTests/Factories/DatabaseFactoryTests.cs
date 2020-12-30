using AB.Framework.UnitTests;
using DevPortal.Business.Factories;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DatabaseFactoryTests : LooseBaseTestFixture
    {
        #region members & setup

        DatabaseFactory factory;

        [SetUp]
        public void Initialize()
        {
            factory = new DatabaseFactory();
        }

        #endregion

        [Test]
        public void CreateDatabaseWikiExportListItems_NoCondition_ReturnExportListItems()
        {
            // Arrange
            var database1 = new Database
            {
                Id = 45,
                Name = "database-1",
                DatabaseGroupName = "database-group-1",
                DatabaseTypeName = "database-type-1",
                ModifiedDate = new System.DateTime(2020, 6, 20, 1, 30, 24),
                RedmineProjectName = "database-1"
            };
            var database2 = new Database
            {
                Id = 46,
                Name = "database-2",
                DatabaseGroupName = "database-group-2",
                DatabaseTypeName = "database-type-2",
                ModifiedDate = new System.DateTime(2020, 6, 21, 2, 30, 24),
                RedmineProjectName = "database-2"
            };

            var databases = new List<Database> {
                database1,
                database2
            };

            var databasesWikiExportItems = new List<DatabaseWikiExportListItem> {
                new DatabaseWikiExportListItem{
                    Name = database1.Name,
                    DatabaseGroupName = database1.DatabaseGroupName,
                    DatabaseTypeName = database1.DatabaseTypeName,
                    RedmineProjectName = database1.RedmineProjectName
                },
                 new DatabaseWikiExportListItem{
                    Name = database2.Name,
                    DatabaseGroupName = database2.DatabaseGroupName,
                    DatabaseTypeName = database2.DatabaseTypeName,
                    RedmineProjectName = database2.RedmineProjectName
                },
            };

            // Act
            var result = factory.CreateDatabaseWikiExportListItems(databases);

            // Assert
            result.Should().BeEquivalentTo(databasesWikiExportItems);
        }
    }
}