using AB.Framework.UnitTests;
using DevPortal.Business.Factories;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.BusinessTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class AuditFactoryTests : BaseTestFixture
    {
        #region members & setup

        AuditFactory factory;

        [SetUp]
        public void Initialize()
        {
            factory = new AuditFactory();
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
        }

        #endregion

        #region create audit info

        [Test]
        public void CreateAuditInfo_NoCondition_ReturnAuditInfo()
        {
            // Arrange
            string tableName = "tableName";
            int recordId = 1;
            object oldRecord = "oldRecord";
            object newRecord = "newRecord";
            int modifiedBy = 1;

            var expectedResult = new AuditInfo
            {
                TableName = tableName,
                RecordId = recordId,
                OldRecord = oldRecord,
                NewRecord = newRecord,
                ModifiedBy = modifiedBy
            };

            // Act
            var result = factory.CreateAuditInfo(tableName, recordId, oldRecord, newRecord, modifiedBy);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion
    }
}