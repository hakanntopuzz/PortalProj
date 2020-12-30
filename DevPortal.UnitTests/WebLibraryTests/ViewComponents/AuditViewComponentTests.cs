using AB.Framework.UnitTests;
using DevPortal.Business.Abstract.Services;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using DevPortal.Web.Library.ViewComponents;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace DevPortal.UnitTests.WebLibraryTests.ViewComponents
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class AuditViewComponentTests : BaseTestFixture
    {
        #region members & setup

        AuditViewComponent component;

        StrictMock<IAuditViewModelFactory> viewModelFactory;

        [SetUp]
        public void Initialize()
        {
            viewModelFactory = new StrictMock<IAuditViewModelFactory>();

            component = new AuditViewComponent(
                viewModelFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            viewModelFactory.VerifyAll();
        }

        #endregion

        [Test]
        public void Invoke_NoCondition_ReturnViewComponentResult()
        {
            // Arrange
            var tableName = "test";
            var recordId = 10;

            var auditViewModel = new AuditViewModel();

            viewModelFactory.Setup(x => x.CreateAuditViewModel(tableName, recordId)).Returns(auditViewModel);

            // Act
            var result = component.Invoke(tableName, recordId);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
