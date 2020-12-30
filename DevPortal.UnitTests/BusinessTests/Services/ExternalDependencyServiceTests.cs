using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ExternalDependencyServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IExternalDependencyRepository> externalDependencyRepository;

        StrictMock<IUrlGeneratorService> urlGeneratorService;

        StrictMock<IRouteValueFactory> routeValueFactory;

        StrictMock<IAuditService> auditService;

        StrictMock<IAuditFactory> auditFactory;

        StrictMock<ILoggingService> loggingService;

        ExternalDependencyService service;

        [SetUp]
        public void Initialize()
        {
            externalDependencyRepository = new StrictMock<IExternalDependencyRepository>();
            urlGeneratorService = new StrictMock<IUrlGeneratorService>();
            routeValueFactory = new StrictMock<IRouteValueFactory>();
            auditService = new StrictMock<IAuditService>();
            auditFactory = new StrictMock<IAuditFactory>();
            loggingService = new StrictMock<ILoggingService>();

            service = new ExternalDependencyService(
                externalDependencyRepository.Object,
                urlGeneratorService.Object,
                routeValueFactory.Object,
                auditService.Object,
                auditFactory.Object,
                loggingService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            externalDependencyRepository.VerifyAll();
            urlGeneratorService.VerifyAll();
            routeValueFactory.VerifyAll();
        }

        #endregion

        #region get external dependency byid

        [Test]
        public void GetExternalDependencyById_EnvironmentExists_ReturnExternalDependency()
        {
            // Arrange
            var externalDependency = new ExternalDependency();
            var id = 1;

            externalDependencyRepository.Setup(x => x.GetExternalDependencyById(id)).Returns(externalDependency);
            externalDependencyRepository.Setup(x => x.GetExternalDependencyUpdateInfo(id)).Returns(externalDependency.RecordUpdateInfo);

            // Act
            var result = service.GetExternalDependencyById(id);

            // Assert
            result.Should().BeSameAs(externalDependency);
        }

        [Test]
        public void GetExternalDependencyById_ExternalDependencyDoesNotExist_ReturnNull()
        {
            // Arrange
            var id = 1;
            ExternalDependency externalDependency = null;

            externalDependencyRepository.Setup(x => x.GetExternalDependencyById(id)).Returns(externalDependency);

            // Act

            var result = service.GetExternalDependencyById(id);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region add external dependency

        [Test]
        public void AddExternalDependency_EnvironmentIdZero_ReturnServiceResult()
        {
            // Arrange
            ExternalDependency externalDependency = new ExternalDependency();
            var serviceResult = Int32ServiceResult.Error(Messages.AddingFails);
            var externalDependencyId = 0;

            externalDependencyRepository.Setup(x => x.AddExternalDependency(externalDependency)).Returns(externalDependencyId);

            // Act
            var result = service.AddExternalDependency(externalDependency);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void AddExternalDependency_AddingSuccess_ReturnServiceResult()
        {
            // Arrange
            ExternalDependency externalDependency = new ExternalDependency();
            var externalDependencyId = 1;
            var serviceResult = Int32ServiceResult.Success(Messages.AddingSucceeds, externalDependencyId);

            externalDependencyRepository.Setup(x => x.AddExternalDependency(externalDependency)).Returns(externalDependencyId);

            // Act
            var result = service.AddExternalDependency(externalDependency);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region update external dependency

        [Test]
        public void UpdateExternalDependency_ParameterIsNull_ReturnServiceResult()
        {
            // Arrange
            ExternalDependency externalDependency = null;
            var serviceResult = ServiceResult.Error(Messages.NullParameterError);

            // Act
            var result = service.UpdateExternalDependency(externalDependency);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void UpdateExternalDependency_ExternalDependencyHasNoChanges_ReturnNoChangesDetectedError()
        {
            // Arrange
            var externalDependency = new ExternalDependency { Id = 3, Name = "Test" };
            var oldExternalDependency = new ExternalDependency { Id = 3, Name = "Test" };

            externalDependencyRepository.Setup(x => x.GetExternalDependencyById(externalDependency.Id)).Returns(oldExternalDependency);
            auditService.Setup(x => x.IsChanged(oldExternalDependency, externalDependency, nameof(ExternalDependency))).Returns(false);

            // Act
            var result = service.UpdateExternalDependency(externalDependency);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.ExternalDependencyUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateExternalDependency_ExternalDependencyUpdateFails_ReturnServerError()
        {
            // Arrange
            var externalDependency = new ExternalDependency { Id = 3, Name = "Test" };
            var oldExternalDependency = new ExternalDependency { Id = 3, Name = "Test" };

            externalDependencyRepository.Setup(x => x.GetExternalDependencyById(externalDependency.Id)).Returns(oldExternalDependency);
            auditService.Setup(x => x.IsChanged(oldExternalDependency, externalDependency, nameof(ExternalDependency))).Returns(true);
            externalDependencyRepository.Setup(x => x.UpdateExternalDependency(externalDependency)).Returns(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateExternalDependency(externalDependency);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateExternalDependency_ExternalDependencyAddAuditNotSuccess_ReturnServerError()
        {
            // Arrange
            var externalDependency = new ExternalDependency { Id = 3, Name = "Test" };
            var oldExternalDependency = new ExternalDependency();

            externalDependencyRepository.Setup(x => x.GetExternalDependencyById(externalDependency.Id)).Returns(oldExternalDependency);
            auditService.Setup(x => x.IsChanged(oldExternalDependency, externalDependency, nameof(ExternalDependency))).Returns(true);
            externalDependencyRepository.Setup(x => x.UpdateExternalDependency(externalDependency)).Returns(true);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateExternalDependency(externalDependency);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateExternalDependency_UpdateExternalDependencyNotSucceeds_ReturnServiceErrorResult()
        {
            // Arrange
            var externalDependency = new ExternalDependency { Id = 3, Name = "Test" };
            var oldExternalDependency = new ExternalDependency { Id = 3, Name = "Test" };
            var auditInfo = new AuditInfo();

            externalDependencyRepository.Setup(x => x.GetExternalDependencyById(externalDependency.Id)).Returns(oldExternalDependency);
            auditService.Setup(x => x.IsChanged(oldExternalDependency, externalDependency, nameof(ExternalDependency))).Returns(true);
            externalDependencyRepository.Setup(x => x.UpdateExternalDependency(externalDependency)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(ExternalDependency), externalDependency.Id, oldExternalDependency, oldExternalDependency, externalDependency.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(false);
            loggingService.Setup(x => x.LogError(SetupAny<string>(), SetupAny<string>(), SetupAny<Exception>()));

            // Act
            var result = service.UpdateExternalDependency(externalDependency);

            // Assert
            var expectedResult = ServiceResult.Error(Messages.UpdateFails);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void UpdateExternalDependency_UpdateExternalDependencySucceeds_ReturnServiceSuccessResult()
        {
            // Arrange
            var externalDependency = new ExternalDependency { Id = 3, Name = "Test" };
            var oldExternalDependency = new ExternalDependency { Id = 3, Name = "Test" };
            var auditInfo = new AuditInfo();

            externalDependencyRepository.Setup(x => x.GetExternalDependencyById(externalDependency.Id)).Returns(oldExternalDependency);
            auditService.Setup(x => x.IsChanged(oldExternalDependency, externalDependency, nameof(ExternalDependency))).Returns(true);
            externalDependencyRepository.Setup(x => x.UpdateExternalDependency(externalDependency)).Returns(true);
            auditFactory.Setup(x => x.CreateAuditInfo(nameof(ExternalDependency), externalDependency.Id, oldExternalDependency, oldExternalDependency, externalDependency.RecordUpdateInfo.ModifiedBy)).Returns(auditInfo);
            auditService.Setup(x => x.AddAsync(auditInfo)).ReturnsAsync(true);

            // Act
            var result = service.UpdateExternalDependency(externalDependency);

            // Assert
            var expectedResult = ServiceResult.Success(Messages.ExternalDependencyUpdated);
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region delete external dependency

        [Test]
        public void DeleteExternalDependency_DeleteFails_ReturnServiceResult()
        {
            // Arrange
            var externalDependency = new ExternalDependency();
            var serviceResult = StringServiceResult.Error(Messages.DeleteFails);

            externalDependencyRepository.Setup(x => x.DeleteExternalDependency(externalDependency)).Returns(false);

            // Act
            var result = service.DeleteExternalDependency(externalDependency);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        [Test]
        public void DeleteExternalDependency_DeleteSuccess_ReturnServiceResult()
        {
            // Arrange
            var externalDependency = new ExternalDependency();
            var routeValue = new RouteValueDictionary();
            string redirectUrl = "redirectUrl";
            var serviceResult = StringServiceResult.Success(Messages.ExternalDependencyDeleted, redirectUrl);

            externalDependencyRepository.Setup(x => x.DeleteExternalDependency(externalDependency)).Returns(true);
            routeValueFactory.Setup(s => s.CreateRouteValuesForGenerateUrl(externalDependency.Id)).Returns(routeValue);
            urlGeneratorService.Setup(s => s.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, routeValue)).Returns(redirectUrl);

            // Act
            var result = service.DeleteExternalDependency(externalDependency);

            // Assert
            result.Should().BeEquivalentTo(serviceResult);
        }

        #endregion

        #region get external dependencies

        [Test]
        public void GetExternalDependencies_NoCondition_ReturnApplicationExportListItems()
        {
            // Arrange
            var applicationId = 5;
            var externalExportListItems = new List<ExternalDependenciesExportListItem>();

            externalDependencyRepository.Setup(x => x.GetExternalDependencies(applicationId)).Returns(externalExportListItems);

            // Act
            var result = service.GetExternalDependencies(applicationId);

            // Assert
            result.Should().BeEquivalentTo(externalExportListItems);
        }

        #endregion
    }
}