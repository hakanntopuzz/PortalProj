using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Framework;
using DevPortal.Framework.Factories;
using DevPortal.Model;
using System;

namespace DevPortal.Business.Services
{
    public class ApplicationDependencyWriterService : IApplicationDependencyWriterService
    {
        #region ctor

        readonly IApplicationDependencyRepository applicationDependencyRepository;

        readonly IAuditService auditService;

        readonly IAuditFactory auditFactory;

        readonly ILoggingService loggingService;

        public ApplicationDependencyWriterService(
            IApplicationDependencyRepository applicationDependencyRepository,
            IAuditService auditService,
            IAuditFactory auditFactory,
            ILoggingService loggingService)
        {
            this.applicationDependencyRepository = applicationDependencyRepository;
            this.auditService = auditService;
            this.auditFactory = auditFactory;
            this.loggingService = loggingService;
        }

        #endregion

        #region add application dependency

        public ServiceResult AddApplicationDependency(ApplicationDependency applicationDependency)
        {
            var isSuccess = applicationDependencyRepository.AddApplicationDependency(applicationDependency);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.AddingFails);
            }

            return ServiceResult.Success(Messages.ApplicationDependencyCreated);
        }

        #endregion

        #region update application dependency

        public ServiceResult UpdateApplicationDependency(ApplicationDependency applicationDependency)
        {
            if (applicationDependency == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }

            var oldApplicationDependency = applicationDependencyRepository.GetApplicationDependencyById(applicationDependency.Id);

            var isChanged = auditService.IsChanged(oldApplicationDependency, applicationDependency, nameof(ApplicationDependency));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.ApplicationDependencyUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateApplicationDependencyCore(applicationDependency);

                    var newApplicationDependency = applicationDependencyRepository.GetApplicationDependencyById(applicationDependency.Id);

                    AddAuditCore(newApplicationDependency, oldApplicationDependency, applicationDependency.RecordUpdateInfo.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(UpdateApplicationDependency), "Uygulama bağımlılığı güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.ApplicationDependencyUpdated);
        }

        void UpdateApplicationDependencyCore(ApplicationDependency applicationDependency)
        {
            var updateSuccess = applicationDependencyRepository.UpdateApplicationDependency(applicationDependency);

            if (!updateSuccess)
            {
                throw new TransactionIstopException("Uygulama bağımlılığı güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        void AddAuditCore(ApplicationDependency applicationDependency, ApplicationDependency oldApplicationDependency, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(ApplicationDependency), applicationDependency.Id, oldApplicationDependency, applicationDependency, userId);
            var isSuccess = auditService.AddAsync(auditInfo).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        #endregion

        #region delete application dependency

        public ServiceResult DeleteApplicationDependency(int applicationDependencyId)
        {
            var isSuccess = applicationDependencyRepository.DeleteApplicationDependency(applicationDependencyId);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.DeleteFails);
            }

            return ServiceResult.Success(Messages.ApplicationDependencyDeleted);
        }

        #endregion
    }
}