using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class AuditControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<IAuditService> auditService;

        AuditController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            auditService = new StrictMock<IAuditService>();

            controller = new AuditController(
                userSessionService.Object,
                auditService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            auditService.VerifyAll();
        }

        #endregion

        #region index

        [Test]
        public async Task Index_NoCondition_Return()
        {
            // Arrange
            AuditTableParam tableParam = new AuditTableParam();
            var jqTable = new JQTable();

            auditService.Setup(x => x.GetFilteredAuditListAsJqTableAsync(tableParam)).ReturnsAsync(jqTable);

            // Act
            var result = await controller.Index(tableParam);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.As<JQTable>().Should().Be(jqTable);
        }

        #endregion
    }
}