using AB.Framework.UnitTests;
using DevPortal.Framework;
using DevPortal.Framework.Abstract;
using DevPortal.Framework.Services;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.FrameworkTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class CsvServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<ICsvSerializer> csvSerializer;

        StrictMock<IEncoding> encoding;

        CsvService service;

        [SetUp]
        public void Initialize()
        {
            csvSerializer = new StrictMock<ICsvSerializer>();
            encoding = new StrictMock<IEncoding>();

            service = new CsvService(
                csvSerializer.Object,
                encoding.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            csvSerializer.VerifyAll();
            encoding.VerifyAll();
        }

        #endregion

        #region export to csv

        [Test]
        public void ExportToCsv_NoCondition_ReturnCsvByteArray()
        {
            // Arrange
            var repositories = new List<ApplicationExportListItem>
            {
                new ApplicationExportListItem(),
            };
            var columnNames = CsvColumnNames.ApplicationList;
            const string textData = "csv-data";
            byte[] encodedBytes = { 1, 2, 3 };

            csvSerializer.Setup(x => x.SerializeArray(repositories, columnNames)).Returns(textData);
            encoding.Setup(x => x.GetBytes(textData)).Returns(encodedBytes);

            // Act
            var result = service.ExportToCsv(repositories, columnNames);

            // Assert
            result.Should().BeEquivalentTo(encodedBytes);
        }

        #endregion
    }
}