using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework;
using DevPortal.Framework.Factories;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Services
{
    public class EnvironmentService : IEnvironmentService
    {
        #region ctor

        readonly IEnvironmentRepository environmentRepository;

        readonly IAuditService auditService;

        readonly IAuditFactory auditFactory;

        readonly ILoggingService loggingService;

        public EnvironmentService(IEnvironmentRepository environmentRepository,
               IAuditService auditService,
                IAuditFactory auditFactory,
               ILoggingService loggingService)
        {
            this.environmentRepository = environmentRepository;
            this.auditService = auditService;
            this.auditFactory = auditFactory;
            this.loggingService = loggingService;
        }

        #endregion

        #region get environments

        public ICollection<Environment> GetEnvironments()
        {
            return environmentRepository.GetEnvironments();
        }

        #endregion

        #region get environment by id

        public Environment GetEnvironment(int id)
        {
            var environment = environmentRepository.GetEnvironmentById(id);

            if (environment == null)
            {
                return null;
            }

            environment.RecordUpdateInfo = environmentRepository.GetEnvironmentUpdateInfo(id);

            return environment;
        }

        #endregion

        #region add environment

        public ServiceResult AddEnvironment(Environment environment)
        {
            var environmentModel = environmentRepository.GetEnvironmentByName(environment.Name);

            if (EnvironmentExist(environmentModel))
            {
                return ServiceResult.Error(Messages.EnvironmentFound);
            }

            var environmentId = environmentRepository.AddEnvironment(environment);

            if (environmentId == 0)
            {
                return ServiceResult.Error(Messages.AddingFails);
            }

            return ServiceResult.Success(Messages.AddingSucceeds);
        }

        static bool EnvironmentExist(Environment environmentModel)
        {
            return environmentModel != null;
        }

        #endregion

        #region update environment

        public ServiceResult UpdateEnvironment(Environment environment)
        {
            if (environment == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }

            var environmentModel = environmentRepository.GetEnvironmentByName(environment.Name);

            if (EnvironmentExist(environmentModel, environment.Id))
            {
                return ServiceResult.Error(Messages.EnvironmentFound);
            }

            var oldEnvironment = environmentRepository.GetEnvironmentById(environment.Id);

            var isChanged = auditService.IsChanged(oldEnvironment, environment, nameof(Environment));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.EnvironmentUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateEnvironmentCore(environment);

                    var newEnvironment = environmentRepository.GetEnvironmentById(environment.Id);

                    AddAuditCore(newEnvironment, oldEnvironment, environment.RecordUpdateInfo.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (System.Exception ex)
            {
                loggingService.LogError(nameof(UpdateEnvironment), "Ortam güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.EnvironmentUpdated);
        }

        void UpdateEnvironmentCore(Environment environment)
        {
            var updateSuccess = environmentRepository.UpdateEnvironment(environment);

            if (!updateSuccess)
            {
                throw new TransactionIstopException("Ortam güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        void AddAuditCore(Environment environment, Environment oldEnvironment, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(Environment), environment.Id, oldEnvironment, environment, userId);

            var isSuccess = auditService.AddAsync(auditInfo).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        static bool EnvironmentExist(Environment environmentModel, int environmentId)
        {
            return environmentModel != null && environmentModel.Id != environmentId;
        }

        #endregion

        #region delete environment

        public ServiceResult DeleteEnvironment(int environmentId)
        {
            var relatedAppEnvironmentCount = environmentRepository.GetApplicationEnvironmentCountByEnvironmentId(environmentId);

            if (!RelatedApplicationEnvironmentsExists(relatedAppEnvironmentCount))
            {
                return ServiceResult.Error(Messages.RelatedApplicationEnvironmentsExists);
            }

            var deleteSucceeds = environmentRepository.DeleteEnvironment(environmentId);

            if (!deleteSucceeds)
            {
                return ServiceResult.Error(Messages.DeleteFails);
            }

            return ServiceResult.Success(Messages.EnvironmentDeleted);
        }

        static bool RelatedApplicationEnvironmentsExists(int appEnvironmentCount)
        {
            return appEnvironmentCount == 0;
        }

        #endregion
    }
}