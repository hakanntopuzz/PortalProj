using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework;
using DevPortal.Framework.Factories;
using DevPortal.Model;
using System;

namespace DevPortal.Business.Services
{
    public class ApplicationWriterService : IApplicationWriterService
    {
        #region ctor

        readonly IApplicationRepository applicationRepository;

        readonly IAuditService auditService;

        readonly IAuditFactory auditFactory;

        readonly ILoggingService loggingService;

        public ApplicationWriterService(
            IApplicationRepository applicationRepository,
            IAuditService auditService,
            IAuditFactory auditFactory,
            ILoggingService loggingService)
        {
            this.applicationRepository = applicationRepository;
            this.auditService = auditService;
            this.auditFactory = auditFactory;
            this.loggingService = loggingService;
        }

        #endregion

        #region add application

        public Int32ServiceResult AddApplication(Application application)
        {
            if (application == null)
            {
                return Int32ServiceResult.Error(Messages.NullParameterError);
            }

            var app = GetApplicationByApplicationName(application.Name);

            if (ApplicationExists(app))
            {
                return Int32ServiceResult.Error(Messages.ApplicationNameExists);
            }

            var applicationId = applicationRepository.AddApplication(application);

            if (applicationId == 0)
            {
                return Int32ServiceResult.Error(Messages.AddingFails);
            }

            return Int32ServiceResult.Success(Messages.ApplicationCreated, applicationId);
        }

        static bool ApplicationExists(Application application)
        {
            return application != null;
        }

        #endregion

        #region update application

        public ServiceResult UpdateApplication(Application application)
        {
            if (application == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }

            if (ApplicationExistsForEdit(application))
            {
                return ServiceResult.Error(Messages.ApplicationNameExists);
            }

            var oldApplication = applicationRepository.GetApplication(application.Id);

            var isChanged = auditService.IsChanged(oldApplication, application, nameof(BaseApplication));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.ApplicationUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateApplicationCore(application);

                    var newApplication = applicationRepository.GetApplication(application.Id);

                    AddAuditCore(newApplication, oldApplication, application.RecordUpdateInfo.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(UpdateApplication), "Uygulama güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.ApplicationUpdated);
        }

        void UpdateApplicationCore(Application application)
        {
            var updateSuccess = applicationRepository.UpdateApplication(application);

            if (!updateSuccess)
            {
                throw new TransactionIstopException("Uygulama güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        void AddAuditCore(BaseApplication application, BaseApplication oldApplication, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(Application), application.Id, oldApplication, application, userId);
            var isSuccess = auditService.AddAsync(auditInfo).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        bool ApplicationExistsForEdit(Application application)
        {
            var app = GetApplicationByApplicationName(application.Name);

            if (!ApplicationExists(app))
            {
                return false;
            }

            if (app.Id == application.Id)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region get application by application name

        Application GetApplicationByApplicationName(string name)
        {
            return applicationRepository.GetApplicationByApplicationName(name);
        }

        #endregion

        #region delete application

        public ServiceResult DeleteApplication(int applicationId)
        {
            var isSuccess = applicationRepository.DeleteApplication(applicationId);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.DeleteFails);
            }

            return ServiceResult.Success(Messages.ApplicationDeleted);
        }

        #endregion
    }
}