using AB.Data.DapperClient.Abstract;
using AB.Data.DapperClient.Model.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.DataTests.Repositories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class AuditRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        AuditRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new AuditRepository(
                dataClient.Object,
                dataRequestFactory.Object,
                settings.Object
            );
        }

        void SetupDataClient()
        {
            const string devPortalDbConnectionString = "devPortalDbConnectionString";
            settings.SetupGet(x => x.DevPortalDbConnectionString).Returns(devPortalDbConnectionString);
            dataClient.Setup(x => x.SetConnectionString(devPortalDbConnectionString));
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            dataClient.VerifyAll();
            dataRequestFactory.VerifyAll();
            settings.VerifyAll();
        }

        #endregion

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task AddAsync_ConditionsInTestCases_ReturnExpectedResult(bool returnValue)
        {
            //Arrange
            string tableName = "tableName";
            int recordId = 1;
            string fieldName = "fieldName";
            string oldValue = "oldValue";
            string newValue = "newValue";
            int modifiedBy = 1;

            IDataRequest dataRequest = null;
            var defaultReturnValue = false;

            dataRequestFactory.Setup(x => x.AddAudit(tableName, recordId, fieldName, oldValue, newValue, modifiedBy)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalarAsync(dataRequest, defaultReturnValue)).ReturnsAsync(returnValue);

            //Act
            var result = await repository.AddAsync(tableName, recordId, fieldName, oldValue, newValue, modifiedBy);

            //Assert
            result.Should().Be(returnValue);
        }

        [Test]
        public async Task GetFilteredAuditListAsync_NoCondition_ReturnTrueAndFalse()
        {
            //Arrange
            int skip = 1;
            int take = 1;
            string orderBy = "orderBy";
            string orderDir = "orderDir";
            string searchText = "searchText";
            var tableName = "tableName";
            int recordId = 1;

            IDataRequest dataRequest = null;
            ICollection<Audit> expectedValue = null;
            var defaultReturnValue = new List<Audit>();

            dataRequestFactory.Setup(x => x.GetFilteredAuditList(skip, take, orderBy, orderDir, searchText, tableName, recordId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollectionAsync(dataRequest, defaultReturnValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetFilteredAuditListAsync(skip, take, orderBy, orderDir, searchText, tableName, recordId);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }
    }
}