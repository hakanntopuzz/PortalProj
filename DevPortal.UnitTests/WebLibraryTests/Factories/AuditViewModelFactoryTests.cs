using AB.Framework.UnitTests;
using DevPortal.Web.Library;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.WebLibraryTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class AuditViewModelFactoryTests : LooseBaseTestFixture
    {
        #region members & setup

        AuditViewModelFactory factory;

        [SetUp]
        public void Initialize()
        {
            factory = new AuditViewModelFactory();
        }

        #endregion

        [Test]
        public void CreateAuditViewModel_NoCondition_ReturnAuditViewModel()
        {
            // Arrange
            var tableName = "tableName";
            var recordId = 3;
            var auditViewModel = new AuditViewModel
            {
                TableName = tableName,
                RecordId = recordId
            };

            // Act
            var result = factory.CreateAuditViewModel(tableName, recordId);

            // Assert
            result.Should().BeEquivalentTo(auditViewModel);
            result.TableName.Should().BeSameAs(auditViewModel.TableName);
            result.RecordId.Should().Be(auditViewModel.RecordId);
        }
    }
}