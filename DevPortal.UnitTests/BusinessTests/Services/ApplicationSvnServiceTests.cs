using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationSvnServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IApplicationSvnRepository> applicationSvnRepository;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        ApplicationSvnService service;

        [SetUp]
        public void Initialize()
        {
            applicationSvnRepository = new StrictMock<IApplicationSvnRepository>();
            auditService = new StrictMock<IAuditService>();
            auditFactory = new StrictMock<IAuditFactory>();
            loggingService = new StrictMock<ILoggingService>();

            service = new ApplicationSvnService(
                applicationSvnRepository.Object,
                auditService.Object,
                auditFactory.Object,
                loggingService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationSvnRepository.VerifyAll();
        }

        #endregion

        #region add application svn repository

        [Test]
        public void AddApplicationSvnRepository_NoCondition_ReturnServiceResultError()
        {
            // Arrange
            var svnRepository = new SvnRepository();

            applicationSvnRepository.Setup(x => x.AddApplicationSvnRepository(svnRepository)).Returns(false);

            // Act
            var result = service.AddApplicationSvnRepository(svnRepository);

            // Assert
            result.Should().BeEquivalentTo(ServiceResult.Error(Messages.AddingFails));
        }

        [Test]
        public void AddApplicationSvnRepository_NoCondition_ReturnServiceResultSuccess()
        {
            // Arrange
            var svnRepository = new SvnRepository();

            applicationSvnRepository.Setup(x => x.AddApplicationSvnRepository(svnRepository)).Returns(true);

            // Act
            var result = service.AddApplicationSvnRepository(svnRepository);

            // Assert
            result.Should().BeEquivalentTo(ServiceResult.Success(Messages.ApplicationSvnRepositoryCreated));
        }

        #endregion

        #region get application svn repository by id

        [Test]
        public void GetApplicationSvnRepositoryById_RepositoryNotFound_ReturnSvnRepository()
        {
            // Arrange
            var id = 1;
            SvnRepository svnRepository = null;

            applicationSvnRepository.Setup(x => x.GetApplicationSvnRepositoryById(id)).Returns(svnRepository);

            // Act
            var result = service.GetApplicationSvnRepository(id);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void GetApplicationSvnRepositoryById_RepositoryFound_ReturnSvnRepository()
        {
            // Arrange
            var id = 1;
            var recordUpdateInfo = new RecordUpdateInfo();
            var svnRepository = new SvnRepository
            {
                RecordUpdateInfo = recordUpdateInfo
            };

            applicationSvnRepository.Setup(x => x.GetApplicationSvnRepositoryById(id)).Returns(svnRepository);
            applicationSvnRepository.Setup(x => x.GetApplicationSvnRepositoryUpdateInfo(id)).Returns(recordUpdateInfo);

            // Act
            var result = service.GetApplicationSvnRepository(id);

            // Assert
            result.Should().BeEquivalentTo(svnRepository);
        }

        #endregion

        #region update application svn repository

        [Test]
        public void UpdateApplicationSvnRepository_ApplicationSvnRepositoryIsNull_ReturnServiceErrorResult()
        {
            // Arrange
            SvnRepository appSvnRepository = null;

            // Act
            var result = service.UpdateApplicationSvnRepository(appSvnRepository);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.NullParameterError);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationSvnRepository_ApplicationSvnRepositoryHasNoChanges_ReturnNoChangesDetectedError()
        {
            // Arrange
            var appSvnRepository = new SvnRepository { Id = 3, Name = "Test" };
            var oldAppSvnRepository = new SvnRepository { Id = 3, Name = "Test" };

            applicationSvnRepository.Setup(x => x.GetApplicationSvnRepositoryById(appSvnRepository.Id)).Returns(oldAppSvnRepository);
            auditService.Setup(x => x.IsChanged(oldAppSvnRepository, appSvnRepository, nameof(SvnRepository))).Returns(false);

            // Act
            var result = service.UpdateApplicationSvnRepository(appSvnRepository);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.ApplicationSvnRepositoryUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationSvnRepository_ApplicationSvnRepositoryUpdateFails_ReturnServerError()
        {
            // Arrange
            var appSvnRepository = new SvnRepository { Id = 3, Name = "Test" };
            var oldAppSvnRepository = new SvnRepository { Id = 3, Name = "Test" };

            applicationSvnRepository.Setup(x => x.GetApplicationSvnRepositoryById(appSvnRepository.Id)).Returns(oldAppSvnRepository);
            auditService.Setup(x => x.IsChanged(oldAppSvnRepository, appSvnRepository, nameof(SvnRepository))).Returns(true);
            applicationSvnRepository.Setup(x => x.UpdateApplicationSvnRepository(appSvnRepository)).Returns(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplicationSvnRepository(appSvnRepository);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationSvnRepository_ApplicationSvnRepositoryAddAuditNotSuccess_ReturnServerError()
        {
            // Arrange
            var appSvnRepository = new SvnRepository { Id = 3, Name = "Test" };
            var oldAppSvnRepository = new SvnRepository();

            applicationSvnRepository.Setup(x => x.GetApplicationSvnRepositoryById(appSvnRepository.Id)).Returns(oldAppSvnRepository);
            auditService.Setup(x => x.IsChanged(oldAppSvnRepository, appSvnRepository, nameof(SvnRepository))).Returns(true);
            applicationSvnRepository.Setup(x => x.UpdateApplicationSvnRepository(appSvnRepository)).Returns(true);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateApplicationSvnRepository(appSvnRepository);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateApplicationSvnRepository_UpdateApplicationSvnRepositorySucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var appSvnRepository = new SvnRepository { Id = 3, Name = "Test" };
            var oldAppSvnRepository = new SvnRepository { Id = 3, Name = "Test" };
            var auditInfo = new AuditInfo();

            var serviceResult = ServiceResult.Success(Messages.ApplicationSvnRepositoryUpdated);

            applicationSvnRepository.Setup(x => x.GetApplicationSvnRepositoryById(appSvnRepository.Id)).Returns(oldAppSvnRepository);
            auditService.Setup(x => x.IsChanged(oldAppSvnRepository, appSvnRepository, nameof(SvnRepository))).Returns(true);
            applicationSvnRepository.Setup(x => x.UpdateApplicationSvnRepository(appSvnRepository)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(SvnRepository), appSvnRepository.Id, oldAppSvnRepository, oldAppSvnRepository, appSvnRepository.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.UpdateApplicationSvnRepository(appSvnRepository);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region delete application svn repository

        [Test]
        public void DeleteApplicationSvnRepository_NoCondition_ReturnServiceResultError()
        {
            // Arrange
            int svnRepositoryId = 1;
            var svnRepository = new SvnRepository();

            applicationSvnRepository.Setup(x => x.DeleteApplicationSvnRepository(svnRepositoryId)).Returns(false);

            // Act
            var result = service.DeleteApplicationSvnRepository(svnRepositoryId);

            // Assert
            result.Should().BeEquivalentTo(ServiceResult.Error(Messages.DeleteFails));
        }

        [Test]
        public void DeleteApplicationSvnRepository_NoCondition_ReturnServiceResultSuccess()
        {
            // Arrange
            int svnRepositoryId = 1;
            var svnRepository = new SvnRepository();

            applicationSvnRepository.Setup(x => x.DeleteApplicationSvnRepository(svnRepositoryId)).Returns(true);

            // Act
            var result = service.DeleteApplicationSvnRepository(svnRepositoryId);

            // Assert
            result.Should().BeEquivalentTo(ServiceResult.Success(Messages.ApplicationSvnRepositoryDeleted));
        }

        #endregion

        #region get svn repository types

        [Test]
        public void GetSvnRepositoryTypes_NoCondition_ReturnTypes()
        {
            // Arrange
            var svnRepositoryTypes = new List<SvnRepositoryType> {
                new SvnRepositoryType
                {
                    Id = 1,
                    Name = "type"
                }
            };

            applicationSvnRepository.Setup(x => x.GetSvnRepositoryTypes()).Returns(svnRepositoryTypes);

            // Act
            var result = service.GetSvnRepositoryTypes();

            // Assert
            result.Should().BeSameAs(svnRepositoryTypes);
        }

        #endregion
    }
}