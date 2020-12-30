using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Framework;
using DevPortal.Framework.Factories;
using DevPortal.Model;
using System;
using System.Collections.Generic;

namespace DevPortal.Business.Services
{
    public class DatabaseDependencyService : IDatabaseDependencyService
    {
        #region ctor

        readonly IDatabaseDependencyRepository databaseDependencyRepository;

        readonly IAuditService auditService;

        readonly IAuditFactory auditFactory;

        readonly ILoggingService loggingService;

        public DatabaseDependencyService(
            IDatabaseDependencyRepository databaseDependencyRepository,
            IAuditService auditService,
            IAuditFactory auditFactory,
            ILoggingService loggingService)
        {
            this.databaseDependencyRepository = databaseDependencyRepository;
            this.auditService = auditService;
            this.auditFactory = auditFactory;
            this.loggingService = loggingService;
        }

        #endregion

        public DatabaseDependency GetDatabaseDependency(int id)
        {
            var databaseDependency = databaseDependencyRepository.GetDatabaseDependencyById(id);

            if (databaseDependency == null)
            {
                return null;
            }

            databaseDependency.RecordUpdateInfo = databaseDependencyRepository.GetDatabaseDependencyUpdateInfo(id);

            return databaseDependency;
        }

        public ServiceResult AddDatabaseDependency(DatabaseDependency databaseDependency)
        {
            var isSuccess = databaseDependencyRepository.AddDatabaseDependency(databaseDependency);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.AddingFails);
            }

            return ServiceResult.Success(Messages.DatabaseDependencyCreated);
        }

        public ServiceResult UpdateDatabaseDependency(DatabaseDependency databaseDependency)
        {
            if (databaseDependency == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }

            var oldApplicationDependency = databaseDependencyRepository.GetDatabaseDependencyById(databaseDependency.Id);

            var isChanged = auditService.IsChanged(oldApplicationDependency, databaseDependency, nameof(DatabaseDependency));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.DatabaseDependencyUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateDatabaseDependencyCore(databaseDependency);

                    var newDatabaseDependency = databaseDependencyRepository.GetDatabaseDependencyById(databaseDependency.Id);

                    AddAuditCore(newDatabaseDependency, oldApplicationDependency, databaseDependency.RecordUpdateInfo.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(UpdateDatabaseDependency), "Veritabanı bağımlılığı güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.DatabaseDependencyUpdated);
        }

        void UpdateDatabaseDependencyCore(DatabaseDependency databaseDependency)
        {
            var updateSuccess = databaseDependencyRepository.UpdateDatabaseDependency(databaseDependency);

            if (!updateSuccess)
            {
                throw new TransactionIstopException("Veritabanı bağımlılığı güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        void AddAuditCore(DatabaseDependency databaseDependency, DatabaseDependency oldDatabaseDependency, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(DatabaseDependency), databaseDependency.Id, oldDatabaseDependency, databaseDependency, userId);
            var isSuccess = auditService.AddAsync(auditInfo).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        #region delete database dependency

        public ServiceResult DeleteDatabaseDependency(int databaseDependencyId)
        {
            var isSuccess = databaseDependencyRepository.DeleteDatabaseDependency(databaseDependencyId);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.DeleteFails);
            }

            return ServiceResult.Success(Messages.DatabaseDependencyDeleted);
        }

        #endregion

        public ICollection<DatabaseDependency> GetDatabaseDependenciesByApplicationId(int applicationId)
        {
            return databaseDependencyRepository.GetDatabaseDependenciesByApplicationId(applicationId);
        }

        public ICollection<DatabaseDependenciesExportListItem> GetFullDatabaseDependenciesByApplicationId(int applicationId)
        {
            return databaseDependencyRepository.GetFullDatabaseDependenciesByApplicationId(applicationId);
        }
    }
}