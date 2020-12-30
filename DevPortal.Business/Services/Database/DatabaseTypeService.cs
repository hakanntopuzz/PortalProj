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
    public class DatabaseTypeService : IDatabaseTypeService
    {
        #region ctor

        readonly IDatabaseTypeRepository databaseTypeRepository;

        readonly IUrlGeneratorService urlHelper;

        readonly IAuditService auditService;

        readonly IAuditFactory auditFactory;

        readonly ILoggingService loggingService;

        public DatabaseTypeService(
            IDatabaseTypeRepository databaseTypeRepository,
            IUrlGeneratorService urlHelper,
            IAuditService auditService,
            IAuditFactory auditFactory,
            ILoggingService loggingService)
        {
            this.databaseTypeRepository = databaseTypeRepository;
            this.urlHelper = urlHelper;
            this.auditService = auditService;
            this.auditFactory = auditFactory;
            this.loggingService = loggingService;
        }

        #endregion

        #region get database types

        public ICollection<DatabaseType> GetDatabaseTypes()
        {
            return databaseTypeRepository.GetDatabaseTypes();
        }

        #endregion

        #region add databaseType

        public ServiceResult AddDatabaseType(DatabaseType databaseType)
        {
            var databaseTypeModel = databaseTypeRepository.GetDatabaseTypeByName(databaseType.Name);

            if (DatabaseTypeExist(databaseTypeModel))
            {
                return ServiceResult.Error(Messages.DatabaseTypeFound);
            }

            var isSuccess = databaseTypeRepository.AddDatabaseType(databaseType);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.AddingFails);
            }

            return ServiceResult.Success(Messages.AddingSucceeds);
        }

        #endregion

        #region get database type by id

        public DatabaseType GetDatabaseType(int id)
        {
            var databaseType = databaseTypeRepository.GetDatabaseTypeById(id);

            if (databaseType == null)
            {
                return null;
            }

            databaseType.RecordUpdateInfo = databaseTypeRepository.GetDatabaseTypeUpdateInfo(id);

            return databaseType;
        }

        #endregion

        #region update database type

        public ServiceResult UpdateDatabaseType(DatabaseType databaseType)
        {
            if (databaseType == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }

            var databaseTypeModel = databaseTypeRepository.GetDatabaseTypeByName(databaseType.Name);

            if (DatabaseTypeExist(databaseTypeModel, databaseType.Id))
            {
                return ServiceResult.Error(Messages.DatabaseTypeFound);
            }

            var oldDatabaseType = databaseTypeRepository.GetDatabaseTypeById(databaseType.Id);

            var isChanged = auditService.IsChanged(oldDatabaseType, databaseType, nameof(DatabaseType));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.DatabaseTypeUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateDatabaseTypeCore(databaseType);

                    var newDatabaseType = databaseTypeRepository.GetDatabaseTypeById(databaseType.Id);

                    AddAuditCore(newDatabaseType, oldDatabaseType, databaseType.RecordUpdateInfo.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(UpdateDatabaseType), "Veritabanı tipi güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.DatabaseTypeUpdated);
        }

        void UpdateDatabaseTypeCore(DatabaseType databaseType)
        {
            var updateSuccess = databaseTypeRepository.UpdateDatabaseType(databaseType);

            if (!updateSuccess)
            {
                throw new TransactionIstopException("Veritabanı tipi güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        void AddAuditCore(DatabaseType databaseType, DatabaseType oldDatabaseType, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(DatabaseType), databaseType.Id, oldDatabaseType, databaseType, userId);

            var isSuccess = auditService.AddAsync(auditInfo).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        #endregion

        #region delete database type

        public RedirectableClientActionResult DeleteDatabaseType(int databaseTypeId)
        {
            var relatedDatabaseCount = databaseTypeRepository.GetDatabaseCountByDatabaseTypeId(databaseTypeId);
            var redirectUrl = urlHelper.GenerateUrl(DatabaseTypeControllerActionNames.Index, ControllerNames.DatabaseType);

            if (!RelatedDatabaseTypeDatabaseExists(relatedDatabaseCount))
            {
                return RedirectableClientActionResult.Error(redirectUrl, Messages.RelatedDatabaseTypeDatabaseExists);
            }

            var deleteSucceeds = databaseTypeRepository.DeleteDatabaseType(databaseTypeId);

            if (!deleteSucceeds)
            {
                return RedirectableClientActionResult.Error(redirectUrl, Messages.DeleteFails);
            }

            return RedirectableClientActionResult.Success(redirectUrl, Messages.DatabaseTypeDeleted);
        }

        static bool RelatedDatabaseTypeDatabaseExists(int databaseCount)
        {
            return databaseCount == 0;
        }

        #endregion

        #region helpers

        static bool DatabaseTypeExist(DatabaseType databaseTypeModel)
        {
            return databaseTypeModel != null;
        }

        static bool DatabaseTypeExist(DatabaseType databaseTypeModel, int databaseTypeId)
        {
            return databaseTypeModel != null && databaseTypeModel.Id != databaseTypeId;
        }

        #endregion
    }
}