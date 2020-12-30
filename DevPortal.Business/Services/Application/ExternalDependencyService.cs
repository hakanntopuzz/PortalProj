using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Framework;
using DevPortal.Framework.Abstract;
using DevPortal.Framework.Factories;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System;
using System.Collections.Generic;

namespace DevPortal.Business.Services
{
    public class ExternalDependencyService : IExternalDependencyService
    {
        #region ctor

        readonly IExternalDependencyRepository externalDependencyRepository;

        readonly IUrlGeneratorService urlGeneratorService;

        readonly IRouteValueFactory routeValueFactory;

        readonly IAuditService auditService;

        readonly IAuditFactory auditFactory;

        readonly ILoggingService loggingService;

        public ExternalDependencyService(
            IExternalDependencyRepository externalDependencyRepository,
            IUrlGeneratorService urlGeneratorService,
            IRouteValueFactory routeValueFactory,
            IAuditService auditService,
            IAuditFactory auditFactory,
            ILoggingService loggingService)
        {
            this.externalDependencyRepository = externalDependencyRepository;
            this.urlGeneratorService = urlGeneratorService;
            this.routeValueFactory = routeValueFactory;
            this.auditService = auditService;
            this.auditFactory = auditFactory;
            this.loggingService = loggingService;
        }

        #endregion

        #region get external dependency by id

        public ExternalDependency GetExternalDependencyById(int id)
        {
            var externalDependency = externalDependencyRepository.GetExternalDependencyById(id);

            if (externalDependency == null)
            {
                return null;
            }

            externalDependency.RecordUpdateInfo = externalDependencyRepository.GetExternalDependencyUpdateInfo(id);

            return externalDependency;
        }

        #endregion

        #region add externaldependency

        public Int32ServiceResult AddExternalDependency(ExternalDependency externalDependency)
        {
            var externalDependencyId = externalDependencyRepository.AddExternalDependency(externalDependency);

            if (externalDependencyId == 0)
            {
                return Int32ServiceResult.Error(Messages.AddingFails);
            }

            return Int32ServiceResult.Success(Messages.AddingSucceeds, externalDependencyId);
        }

        #endregion

        #region update external dependency

        public ServiceResult UpdateExternalDependency(ExternalDependency externalDependency)
        {
            if (externalDependency == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }

            var oldApplicationDependency = externalDependencyRepository.GetExternalDependencyById(externalDependency.Id);

            var isChanged = auditService.IsChanged(oldApplicationDependency, externalDependency, nameof(ExternalDependency));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.ExternalDependencyUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateExternalDependencyCore(externalDependency);

                    var newExternalDependency = externalDependencyRepository.GetExternalDependencyById(externalDependency.Id);

                    AddAuditCore(newExternalDependency, oldApplicationDependency, externalDependency.RecordUpdateInfo.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(UpdateExternalDependency), "Harici bağımlılığı güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.ExternalDependencyUpdated);
        }

        void UpdateExternalDependencyCore(ExternalDependency externalDependency)
        {
            var updateSuccess = externalDependencyRepository.UpdateExternalDependency(externalDependency);

            if (!updateSuccess)
            {
                throw new TransactionIstopException("Harici bağımlılığı güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        void AddAuditCore(ExternalDependency externalDependency, ExternalDependency oldExternalDependency, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(ExternalDependency), externalDependency.Id, oldExternalDependency, externalDependency, userId);
            var isSuccess = auditService.AddAsync(auditInfo).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        #endregion

        #region delete external dependency

        public StringServiceResult DeleteExternalDependency(ExternalDependency externalDependency)
        {
            var isSuccess = externalDependencyRepository.DeleteExternalDependency(externalDependency);

            if (isSuccess)
            {
                var urlParams = routeValueFactory.CreateRouteValuesForGenerateUrl(externalDependency.ApplicationId);
                var redirectUrl = urlGeneratorService.GenerateUrl(ApplicationControllerActionNames.Detail, ControllerNames.Application, urlParams);

                return StringServiceResult.Success(Messages.ExternalDependencyDeleted, redirectUrl);
            }

            return StringServiceResult.Error(Messages.DeleteFails);
        }

        #endregion

        #region get external dependency

        public ICollection<ExternalDependenciesExportListItem> GetExternalDependencies(int applicationId)
        {
            return externalDependencyRepository.GetExternalDependencies(applicationId);
        }

        #endregion
    }
}