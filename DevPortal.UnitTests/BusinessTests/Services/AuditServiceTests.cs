using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class AuditServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IAuditRepository> auditRepository;

        StrictMock<ILoggingService> loggingService;

        AuditService service;

        [SetUp]
        public void Initialize()
        {
            auditRepository = new StrictMock<IAuditRepository>();
            loggingService = new StrictMock<ILoggingService>();

            service = new AuditService(auditRepository.Object, loggingService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            auditRepository.VerifyAll();
            loggingService.VerifyAll();
        }

        #endregion

        #region add audit

        [Test]
        public async Task AddAsync_SuccessAndNoChanges_ReturnAuditList()
        {
            // Arrange
            var auditInfo = new AuditInfo
            {
                TableName = "table",
                RecordId = 45,
                OldRecord = new Application(),
                NewRecord = new Application(),
                ModifiedBy = 1
            };

            // Act
            var result = await service.AddAsync(auditInfo);

            // Assert
            result.Should().BeTrue();
            auditRepository.Verify(x => x.AddAsync(
                    VerifyAny<string>(),
                        VerifyAny<int>(),
                        VerifyAny<string>(),
                        VerifyAny<string>(),
                        VerifyAny<string>(),
                        VerifyAny<int>()),
                    Times.Never);
        }

        [Test]
        public async Task AddAsync_SuccessAndOnlyRecordUpdateInfoChanged_ReturnAuditList()
        {
            // Arrange
            var auditInfo = new AuditInfo
            {
                TableName = "table",
                RecordId = 45,
                OldRecord = new Application(),
                NewRecord = new Application(),
                ModifiedBy = 1
            };

            // Act
            var result = await service.AddAsync(auditInfo);

            // Assert
            result.Should().BeTrue();
            auditRepository.Verify(x => x.AddAsync(
                    VerifyAny<string>(),
                    VerifyAny<int>(),
                    VerifyAny<string>(),
                    VerifyAny<string>(),
                    VerifyAny<string>(),
                    VerifyAny<int>()),
                Times.Never);
        }

        [Test]
        public async Task AddAsync_RecordChangedAndAddingSucceeds_ReturnSuccess()
        {
            // Arrange
            object oldRecord = new Application
            {
                Description = "description-1"
            };
            object newRecord = new Application
            {
                Description = "description-2"
            };
            bool addSuccess = true;
            var auditInfo = new AuditInfo
            {
                TableName = "table",
                RecordId = 45,
                OldRecord = oldRecord,
                NewRecord = newRecord,
                ModifiedBy = 18
            };

            auditRepository.Setup(x => x.AddAsync(auditInfo.TableName, auditInfo.RecordId, "Description", "description-1", "description-2", auditInfo.ModifiedBy)).ReturnsAsync(addSuccess);

            // Act
            var result = await service.AddAsync(auditInfo);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task AddAsync_RecordChangedAndAddingFails_RollbackTransactionAndLogExceptionAndReturnErrorResult()
        {
            // Arrange
            object oldRecord = new Application
            {
                Description = "description-1"
            };
            object newRecord = new Application
            {
                Description = "description-2"
            };
            bool addSuccess = false;
            var auditInfo = new AuditInfo
            {
                TableName = "table",
                RecordId = 45,
                OldRecord = oldRecord,
                NewRecord = newRecord,
                ModifiedBy = 18
            };

            auditRepository.Setup(x => x.AddAsync(auditInfo.TableName, auditInfo.RecordId, "Description", "description-1", "description-2", auditInfo.ModifiedBy)).ReturnsAsync(addSuccess);
            loggingService.Setup(x => x.LogError(VerifyAny<string>(), VerifyAny<string>(), VerifyAny<Exception>()));

            // Act
            var result = await service.AddAsync(auditInfo);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region get audit list

        [Test]
        public async Task GetFilteredAuditListAsJqTableAsync_ParamsNull_ReturnNull()
        {
            // Arrange
            AuditTableParam auditTableParam = null;

            var result = await service.GetFilteredAuditListAsJqTableAsync(auditTableParam);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task GetFilteredAuditListAsJqTableAsync_NoRecord_ReturnAuditList()
        {
            // Arrange
            var auditTableParam = new AuditTableParam
            {
                order = new List<TableOrder>
                { new TableOrder { dir = "asc" } }
            };

            var auditList = new List<Audit>();

            auditRepository.Setup(x => x.GetFilteredAuditListAsync(
                auditTableParam.start,
                auditTableParam.length,
                auditTableParam.SortColumn,
                auditTableParam.order.FirstOrDefault().dir,
                auditTableParam.SearchText,
                auditTableParam.TableName,
                auditTableParam.RecordId))
                .ReturnsAsync(auditList);

            var expectedResult = new JQTable
            {
                data = auditList,
                recordsFiltered = 0,
                recordsTotal = 0
            };

            // Act
            var result = await service.GetFilteredAuditListAsJqTableAsync(auditTableParam);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task GetFilteredAuditListAsJqTableAsync_RecordsFound_ReturnAuditList()
        {
            // Arrange
            var auditTableParam = new AuditTableParam
            {
                order = new List<TableOrder> { new TableOrder { dir = "asc" } }
            };
            var sortDirection = auditTableParam.order.First().dir;
            var totalCount = 1;
            var auditList = new List<Audit> {
                new Audit
                {
                    TotalCount = totalCount
                }
            };

            auditRepository.Setup(x => x.GetFilteredAuditListAsync(
                auditTableParam.start,
                auditTableParam.length,
                auditTableParam.SortColumn,
                sortDirection,
                auditTableParam.SearchText,
                auditTableParam.TableName,
                auditTableParam.RecordId))
                .ReturnsAsync(auditList);

            var expectedResult = new JQTable
            {
                data = auditList,
                recordsFiltered = totalCount,
                recordsTotal = totalCount
            };

            // Act
            var result = await service.GetFilteredAuditListAsJqTableAsync(auditTableParam);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task GetFilteredAuditListAsJqTableAsync_SortDirectionEmptyAndRecordsFound_ReturnAuditList()
        {
            // Arrange
            var auditTableParam = new AuditTableParam
            {
                order = new List<TableOrder> { }
            };
            string sortDirection = null;
            var totalCount = 1;
            var auditList = new List<Audit> {
                new Audit
                {
                    TotalCount = totalCount
                }
            };

            auditRepository.Setup(x => x.GetFilteredAuditListAsync(
                auditTableParam.start,
                auditTableParam.length,
                auditTableParam.SortColumn,
                sortDirection,
                auditTableParam.SearchText,
                auditTableParam.TableName,
                auditTableParam.RecordId))
                .ReturnsAsync(auditList);

            var expectedResult = new JQTable
            {
                data = auditList,
                recordsFiltered = totalCount,
                recordsTotal = totalCount
            };

            // Act
            var result = await service.GetFilteredAuditListAsJqTableAsync(auditTableParam);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region is changed

        [Test]
        public void IsChanged_NoChanges_ReturnFalse()
        {
            // Arrange
            var oldApplication = new Application();
            var newApplication = new Application();

            // Act
            var result = service.IsChanged(oldApplication, newApplication, nameof(BaseApplication));

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsChanged_OneValueIsNull_ReturnTrue()
        {
            // Arrange
            var oldApplication = new Application { Description = null };
            var newApplication = new Application { Description = "asdasd" };

            // Act
            var result = service.IsChanged(oldApplication, newApplication, nameof(BaseApplication));

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsChanged_OldValueNotEqualsNewValue_ReturnTrue()
        {
            // Arrange
            var oldApplication = new Application { Description = "asd" };
            var newApplication = new Application { Description = "asdasd" };

            // Act
            var result = service.IsChanged(oldApplication, newApplication, nameof(BaseApplication));

            // Assert
            result.Should().BeTrue();
        }

        #endregion
    }
}