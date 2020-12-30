using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework;
using DevPortal.Framework.Factories;
using DevPortal.Model;
using System;

namespace DevPortal.Business.Services
{
    public class DatabaseWriterService : IDatabaseWriterService
    {
        #region ctor

        readonly IDatabaseRepository databaseRepository;

        readonly IAuditService auditService;

        readonly IAuditFactory auditFactory;

        readonly ILoggingService loggingService;

        public DatabaseWriterService(
            IDatabaseRepository databaseRepository,
            IAuditService auditService,
            IAuditFactory auditFactory,
            ILoggingService loggingService)
        {
            this.databaseRepository = databaseRepository;
            this.auditService = auditService;
            this.auditFactory = auditFactory;
            this.loggingService = loggingService;
        }

        #endregion

        #region add database

        public ServiceResult AddDatabase(Database database)
        {
            if (database == null)
            {
                return ServiceResult.Error(Messages.GeneralError);
            }

            var databaseModel = databaseRepository.GetDatabaseByDatabaseName(database.Name);

            if (DatabaseExists(databaseModel))
            {
                return ServiceResult.Error(Messages.DatabaseFound);
            }

            var databaseTypeId = databaseRepository.AddDatabase(database);

            if (databaseTypeId == 0)
            {
                return ServiceResult.Error(Messages.DatabaseAdded);
            }

            return ServiceResult.Success(Messages.AddingSucceeds);
        }

        #endregion

        #region update database

        public ServiceResult UpdateDatabase(Database database)
        {
            if (database == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }

            if (!UpdateRequestValid(database))
            {
                return ServiceResult.Error(Messages.DatabaseNameExists);
            }

            var oldDatabase = databaseRepository.GetDatabase(database.Id);
            var isChanged = auditService.IsChanged(oldDatabase, database, nameof(Database));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.DatabaseUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateDatabaseCore(database);

                    var newDatabase = databaseRepository.GetDatabase(database.Id);

                    AddAuditCore(newDatabase, oldDatabase, database.RecordUpdateInfo.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(UpdateDatabase), "Veritabanı güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.DatabaseUpdated);
        }

        void UpdateDatabaseCore(Database database)
        {
            var updateSuccess = databaseRepository.UpdateDatabase(database);

            if (!updateSuccess)
            {
                throw new TransactionIstopException("Veritabanı güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        void AddAuditCore(Database database, Database oldDatabase, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(Database), database.Id, oldDatabase, database, userId);
            var isSuccess = auditService.AddAsync(auditInfo).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        bool UpdateRequestValid(Database database)
        {
            var existingDatabase = GetDatabaseByDatabaseName(database.Name);

            if (!DatabaseExists(existingDatabase))
            {
                return true;
            }

            if (SameDatabase(database, existingDatabase))
            {
                return true;
            }

            return false;
        }

        Database GetDatabaseByDatabaseName(string name)
        {
            return databaseRepository.GetDatabaseByDatabaseName(name);
        }

        static bool SameDatabase(Database database, Database existingDatabase)
        {
            return existingDatabase.Id == database.Id;
        }

        #endregion

        static bool DatabaseExists(Database database)
        {
            return database != null;
        }

        #region delete database

        public ServiceResult DeleteDatabase(int databaseId)
        {
            var isSuccess = databaseRepository.DeleteDatabase(databaseId);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.DeleteFails);
            }

            return ServiceResult.Success(Messages.DatabaseDeleted);
        }

        #endregion
    }
}