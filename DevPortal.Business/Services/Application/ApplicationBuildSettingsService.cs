using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Framework;
using DevPortal.Framework.Factories;
using DevPortal.Model;
using System;
using System.Threading.Tasks;

namespace DevPortal.Business.Services
{
    public class ApplicationBuildSettingsService : IApplicationBuildSettingsService
    {
        #region ctor

        private readonly IApplicationBuildSettingsRepository repository;
        private readonly IAuditService auditService;
        private readonly IAuditFactory auditFactory;
        private readonly ILoggingService loggingService;

        public ApplicationBuildSettingsService(
            IApplicationBuildSettingsRepository repository,
            IAuditService auditService,
            IAuditFactory auditFactory,
            ILoggingService loggingService)
        {
            this.repository = repository;
            this.auditService = auditService;
            this.auditFactory = auditFactory;
            this.loggingService = loggingService;
        }

        #endregion

        public ApplicationBuildSettings GetApplicationBuildSettings(int applicationId)
        {
            var buildSettings = repository.GetApplicationBuildSettings(applicationId);
            if (buildSettings == null)
            {
                return null;
            }

            buildSettings.RecordUpdateInfo = repository.GetApplicationBuildSettingsUpdateInfo(applicationId);

            return buildSettings;
        }

        public ServiceResult AddOrUpdateApplicationBuildSettings(ApplicationBuildSettings buildSettings)
        {
            if (!BuildSettingsExists(buildSettings))
            {
                return AddApplicationBuildSettings(buildSettings);
            }
            else
            {
                return UpdateApplicationBuildSettings(buildSettings);
            }
        }

        bool BuildSettingsExists(ApplicationBuildSettings buildSettings)
        {
            var settings = GetApplicationBuildSettings(buildSettings.ApplicationId);
            if (settings == null)
            {
                return false;
            }

            return true;
        }

        ServiceResult AddApplicationBuildSettings(ApplicationBuildSettings buildSettings)
        {
            var isSuccess = repository.AddApplicationBuildSettings(buildSettings);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.AddingFails);
            }

            return ServiceResult.Success(Messages.ApplicationBuildSettingsUpdated);
        }

        ServiceResult UpdateApplicationBuildSettings(ApplicationBuildSettings buildSettings)
        {
            var oldBuildSettings = repository.GetApplicationBuildSettings(buildSettings.ApplicationId);

            var isChanged = auditService.IsChanged(oldBuildSettings, buildSettings, nameof(ApplicationBuildSettings));
            if (!isChanged)
            {
                return ServiceResult.Success(Messages.UpdateSucceeds);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateApplicationBuildSettingsCore(buildSettings);

                    var newBuildSettings = repository.GetApplicationBuildSettings(buildSettings.ApplicationId);

                    AddAuditCore(newBuildSettings, oldBuildSettings, buildSettings.RecordUpdateInfo.ModifiedBy).Wait();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(UpdateApplicationBuildSettings), "Uygulama derleme ayarları güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.ApplicationBuildSettingsUpdated);
        }

        void UpdateApplicationBuildSettingsCore(ApplicationBuildSettings buildSettings)
        {
            var updateSuccess = repository.UpdateApplicationBuildSettings(buildSettings);

            if (!updateSuccess)
            {
                throw new TransactionIstopException("Uygulama derleme ayarları güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        async Task AddAuditCore(ApplicationBuildSettings buildSettings, ApplicationBuildSettings oldBuildSettings, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(ApplicationBuildSettings), buildSettings.Id, oldBuildSettings, buildSettings, userId);
            var isSuccess = await auditService.AddAsync(auditInfo);

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }
    }
}
