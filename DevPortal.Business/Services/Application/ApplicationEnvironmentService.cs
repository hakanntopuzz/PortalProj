using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework;
using DevPortal.Framework.Factories;
using DevPortal.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevPortal.Business.Services
{
    public class ApplicationEnvironmentService : IApplicationEnvironmentService
    {
        #region ctor

        readonly IApplicationEnvironmentRepository repository;

        readonly IAuditService auditService;

        readonly IAuditFactory auditFactory;

        readonly ILoggingService loggingService;

        public ApplicationEnvironmentService(
            IApplicationEnvironmentRepository repository,
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

        public ServiceResult AddApplicationEnvironment(ApplicationEnvironment applicationEnvironment)
        {
            var addResult = repository.AddApplicationEnvironment(applicationEnvironment);

            if (!addResult)
            {
                return ServiceResult.Error(Messages.AddingFails);
            }

            return ServiceResult.Success(Messages.ApplicationEnvironmentCreated);
        }

        public ServiceResult UpdateApplicationEnvironment(ApplicationEnvironment applicationEnvironment)
        {
            if (applicationEnvironment == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }

            var applicationEnvModel = repository.GetApplicationEnvironmentByEnvironmentId(applicationEnvironment.ApplicationId, applicationEnvironment.EnvironmentId);

            if (ApplicationEnvironmentExist(applicationEnvModel, applicationEnvironment.Id))
            {
                return ServiceResult.Error(Messages.ApplicationEnvironmentFound);
            }

            var oldApplicationEnv = repository.GetApplicationEnvironmentById(applicationEnvironment.Id);

            var isChanged = auditService.IsChanged(oldApplicationEnv, applicationEnvironment, nameof(ApplicationEnvironment));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.ApplicationEnvironmentUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateApplicationEnvironmentCore(applicationEnvironment);

                    var newApplication = repository.GetApplicationEnvironmentById(applicationEnvironment.Id);

                    AddAuditCore(newApplication, oldApplicationEnv, applicationEnvironment.RecordUpdateInfo.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(UpdateApplicationEnvironment), "Uygulama ortamı güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.ApplicationEnvironmentUpdated);
        }

        void UpdateApplicationEnvironmentCore(ApplicationEnvironment applicationEnvironment)
        {
            var updateSuccess = repository.UpdateApplicationEnvironment(applicationEnvironment);

            if (!updateSuccess)
            {
                throw new TransactionIstopException("Uygulama ortmaı güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        void AddAuditCore(ApplicationEnvironment applicationEnvironment, ApplicationEnvironment oldApplicationEnv, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(ApplicationEnvironment), applicationEnvironment.Id, oldApplicationEnv, applicationEnvironment, userId);

            var isSuccess = auditService.AddAsync(auditInfo).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        public ServiceResult DeleteApplicationEnvironment(int environmentId)
        {
            var isSuccess = repository.DeleteApplicationEnvironment(environmentId);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.DeleteFails);
            }

            return ServiceResult.Success(Messages.ApplicationEnvironmentDeleted);
        }

        public ApplicationEnvironment GetApplicationEnvironment(int environmentId)
        {
            var applicationEnvironment = repository.GetApplicationEnvironmentById(environmentId);

            if (applicationEnvironment == null)
            {
                return null;
            }

            applicationEnvironment.RecordUpdateInfo = repository.GetApplicationEnvironmentUpdateInfo(environmentId);

            return applicationEnvironment;
        }

        public ICollection<ApplicationEnvironment> GetApplicationEnvironments(int applicationId)
        {
            return repository.GetApplicationEnvironments(applicationId);
        }

        public ICollection<ApplicationEnvironment> GetApplicationEnvironmentsHasLog(int applicationId)
        {
            return repository.GetApplicationEnvironments(applicationId).Where(q => q.HasLog).ToList();
        }

        public ICollection<Model.Environment> GetEnvironmentsDoesNotExist(int applicationId)
        {
            return repository.GetEnvironmentsDoesNotExistByApplicationId(applicationId);
        }

        #region helpers

        static bool ApplicationEnvironmentExist(ApplicationEnvironment ApplicationEnvModel, int applicationEnvironmentId)
        {
            return ApplicationEnvModel != null && ApplicationEnvModel.Id != applicationEnvironmentId;
        }

        #endregion
    }
}