using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework;
using DevPortal.Framework.Factories;
using DevPortal.Model;
using System;
using System.Collections.Generic;

namespace DevPortal.Business.Services
{
    public class ApplicationSvnService : IApplicationSvnService
    {
        #region Ctor

        readonly IApplicationSvnRepository applicationSvnRepository;

        readonly IAuditService auditService;

        readonly IAuditFactory auditFactory;

        readonly ILoggingService loggingService;

        public ApplicationSvnService(
            IApplicationSvnRepository applicationSvnRepository,
            IAuditService auditService,
            IAuditFactory auditFactory,
            ILoggingService loggingService)
        {
            this.applicationSvnRepository = applicationSvnRepository;
            this.auditService = auditService;
            this.auditFactory = auditFactory;
            this.loggingService = loggingService;
        }

        #endregion

        public ServiceResult AddApplicationSvnRepository(SvnRepository svnRepository)
        {
            var isSuccess = applicationSvnRepository.AddApplicationSvnRepository(svnRepository);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.AddingFails);
            }

            return ServiceResult.Success(Messages.ApplicationSvnRepositoryCreated);
        }

        public SvnRepository GetApplicationSvnRepository(int repositoryId)
        {
            var repository = applicationSvnRepository.GetApplicationSvnRepositoryById(repositoryId);

            if (repository == null)
            {
                return null;
            }

            repository.RecordUpdateInfo = applicationSvnRepository.GetApplicationSvnRepositoryUpdateInfo(repositoryId);

            return repository;
        }

        public ServiceResult UpdateApplicationSvnRepository(SvnRepository svnRepository)
        {
            if (svnRepository == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }
            var oldSvnRepository = applicationSvnRepository.GetApplicationSvnRepositoryById(svnRepository.Id);

            var isChanged = auditService.IsChanged(oldSvnRepository, svnRepository, nameof(SvnRepository));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.ApplicationSvnRepositoryUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateApplicationSvnRepositoryCore(svnRepository);

                    var newSvnRepository = applicationSvnRepository.GetApplicationSvnRepositoryById(svnRepository.Id);

                    AddAuditCore(newSvnRepository, oldSvnRepository, svnRepository.RecordUpdateInfo.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(UpdateApplicationSvnRepository), "Uygulama svn deposu güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.ApplicationSvnRepositoryUpdated);
        }

        void UpdateApplicationSvnRepositoryCore(SvnRepository svnRepository)
        {
            var updateSuccess = applicationSvnRepository.UpdateApplicationSvnRepository(svnRepository);

            if (!updateSuccess)
            {
                throw new TransactionIstopException("Uygulama svn deposu güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        void AddAuditCore(SvnRepository svnRepository, SvnRepository oldSvnRepository, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(SvnRepository), svnRepository.Id, oldSvnRepository, svnRepository, userId);
            var isSuccess = auditService.AddAsync(auditInfo).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        public ServiceResult DeleteApplicationSvnRepository(int svnRepositoryId)
        {
            var isSuccess = applicationSvnRepository.DeleteApplicationSvnRepository(svnRepositoryId);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.DeleteFails);
            }

            return ServiceResult.Success(Messages.ApplicationSvnRepositoryDeleted);
        }

        public ICollection<SvnRepositoryType> GetSvnRepositoryTypes()
        {
            return applicationSvnRepository.GetSvnRepositoryTypes();
        }
    }
}