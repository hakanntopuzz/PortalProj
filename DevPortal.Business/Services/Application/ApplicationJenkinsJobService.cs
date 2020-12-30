using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Business.Abstract.Services;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework;
using DevPortal.Framework.Factories;
using DevPortal.Model;
using System;
using System.Collections.Generic;

namespace DevPortal.Business.Services
{
    public class ApplicationJenkinsJobService : IApplicationJenkinsJobService
    {
        #region ctor

        readonly IApplicationJenkinsJobRepository repository;

        readonly IAuditService auditService;

        readonly IAuditFactory auditFactory;

        readonly ILoggingService loggingService;

        public ApplicationJenkinsJobService(
            IApplicationJenkinsJobRepository repository,
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

        public ICollection<JenkinsJobType> GetJenkinsJobTypes()
        {
            return repository.GetJenkinsJobTypes();
        }

        public ServiceResult AddApplicationJenkinsJob(JenkinsJob applicationJenkinsJob)
        {
            var addSucceeds = repository.AddApplicationJenkinsJob(applicationJenkinsJob);

            if (!addSucceeds)
            {
                return ServiceResult.Error(Messages.AddingFails);
            }

            return ServiceResult.Success(Messages.ApplicationJenkinsJobCreated);
        }

        public JenkinsJob GetApplicationJenkinsJob(int jenkinsJobId)
        {
            var job = repository.GetApplicationJenkinsJobById(jenkinsJobId);

            if (job == null)
            {
                return null;
            }

            job.RecordUpdateInfo = repository.GetApplicationJenkinsJobUpdateInfo(jenkinsJobId);

            return job;
        }

        public ServiceResult UpdateApplicationJenkinsJob(JenkinsJob applicationJenkinsJob)
        {
            if (applicationJenkinsJob == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }

            var oldApplicationJenkinsJob = repository.GetApplicationJenkinsJobById(applicationJenkinsJob.JenkinsJobId);

            var isChanged = auditService.IsChanged(oldApplicationJenkinsJob, applicationJenkinsJob, nameof(JenkinsJob));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.ApplicationJenkinsJobUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateApplicationJenkinsJobCore(applicationJenkinsJob);

                    var newApplicationJenkinsJob = repository.GetApplicationJenkinsJobById(applicationJenkinsJob.JenkinsJobId);

                    AddAuditCore(newApplicationJenkinsJob, oldApplicationJenkinsJob, applicationJenkinsJob.RecordUpdateInfo.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(UpdateApplicationJenkinsJob), "Uygulama jenkins görevi güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.ApplicationJenkinsJobUpdated);
        }

        void UpdateApplicationJenkinsJobCore(JenkinsJob applicationJenkinsJob)
        {
            var updateSuccess = repository.UpdateApplicationJenkinsJob(applicationJenkinsJob);

            if (!updateSuccess)
            {
                throw new TransactionIstopException("Uygulama jenkins görevi güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        void AddAuditCore(JenkinsJob applicationJenkinsJob, JenkinsJob oldApplicationJenkinsJob, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(JenkinsJob), applicationJenkinsJob.JenkinsJobId, oldApplicationJenkinsJob, applicationJenkinsJob, userId);
            var isSuccess = auditService.AddAsync(auditInfo).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        public ServiceResult DeleteApplicationJenkinsJob(int jenkinsJobId)
        {
            var isSuccess = repository.DeleteApplicationJenkinsJob(jenkinsJobId);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.DeleteFails);
            }

            return ServiceResult.Success(Messages.ApplicationJenkinsJobDeleted);
        }
    }
}