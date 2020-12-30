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
    public class DatabaseGroupService : IDatabaseGroupService
    {
        #region ctor

        readonly IDatabaseGroupRepository databaseGroupRepository;

        readonly IUrlGeneratorService urlHelper;

        readonly IAuditService auditService;

        readonly IAuditFactory auditFactory;

        readonly ILoggingService loggingService;

        public DatabaseGroupService(
            IDatabaseGroupRepository databaseGroupRepository,
            IUrlGeneratorService urlHelper,
            IAuditService auditService,
            IAuditFactory auditFactory,
            ILoggingService loggingService)
        {
            this.databaseGroupRepository = databaseGroupRepository;
            this.urlHelper = urlHelper;
            this.auditService = auditService;
            this.auditFactory = auditFactory;
            this.loggingService = loggingService;
        }

        #endregion

        #region get database groups

        public ICollection<DatabaseGroup> GetDatabaseGroups()
        {
            return databaseGroupRepository.GetDatabaseGroups();
        }

        #endregion

        #region add database group

        public ServiceResult AddDatabaseGroup(DatabaseGroup databaseGroup)
        {
            var databaseGroupModel = databaseGroupRepository.GetDatabaseGroupByName(databaseGroup.Name);

            if (DatabaseGroupExist(databaseGroupModel))
            {
                return ServiceResult.Error(Messages.DatabaseGroupFound);
            }

            var databaseGroupId = databaseGroupRepository.AddDatabaseGroup(databaseGroup);

            if (databaseGroupId == 0)
            {
                return ServiceResult.Error(Messages.DatabaseGroupAdded);
            }

            return ServiceResult.Success(Messages.AddingSucceeds);
        }

        #endregion

        #region get database group by id

        public DatabaseGroup GetDatabaseGroup(int id)
        {
            var databaseGroup = databaseGroupRepository.GetDatabaseGroupById(id);

            if (databaseGroup == null)
            {
                return null;
            }

            databaseGroup.RecordUpdateInfo = databaseGroupRepository.GetDatabaseGroupUpdateInfo(id);

            return databaseGroup;
        }

        #endregion

        #region update database group

        public ServiceResult UpdateDatabaseGroup(DatabaseGroup databaseGroup)
        {
            if (databaseGroup == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }

            var databaseGroupModel = databaseGroupRepository.GetDatabaseGroupByName(databaseGroup.Name);

            if (DatabaseGroupExist(databaseGroupModel, databaseGroup.Id))
            {
                return ServiceResult.Error(Messages.DatabaseGroupFound);
            }

            var oldDatabaseGroup = databaseGroupRepository.GetDatabaseGroupById(databaseGroup.Id);

            var isChanged = auditService.IsChanged(oldDatabaseGroup, databaseGroup, nameof(DatabaseGroup));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.DatabaseGroupUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateDatabaseGroupCore(databaseGroup);

                    var newDatabaseGroup = databaseGroupRepository.GetDatabaseGroupById(databaseGroup.Id);

                    AddAuditCore(newDatabaseGroup, oldDatabaseGroup, databaseGroup.RecordUpdateInfo.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(UpdateDatabaseGroup), "Veritabanı grubu güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.DatabaseGroupUpdated);
        }

        void UpdateDatabaseGroupCore(DatabaseGroup databaseGroup)
        {
            var updateSuccess = databaseGroupRepository.UpdateDatabaseGroup(databaseGroup);

            if (!updateSuccess)
            {
                throw new TransactionIstopException("Veritabanı grubu güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        void AddAuditCore(DatabaseGroup databaseGroup, DatabaseGroup oldDatabaseGroup, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(DatabaseGroup), databaseGroup.Id, oldDatabaseGroup, databaseGroup, userId);

            var isSuccess = auditService.AddAsync(auditInfo).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        #endregion

        #region delete database group

        public RedirectableClientActionResult DeleteDatabaseGroup(int databaseGroupId)
        {
            var relatedDatabaseCount = databaseGroupRepository.GetDatabaseCountByDatabaseGroupId(databaseGroupId);
            var redirectUrl = urlHelper.GenerateUrl(DatabaseGroupControllerActionNames.Index, ControllerNames.DatabaseGroup);

            if (!RelatedDatabaseGroupDatabaseExists(relatedDatabaseCount))
            {
                return RedirectableClientActionResult.Error(redirectUrl, Messages.RelatedDatabaseGroupDatabaseExists);
            }

            var deleteSucceeds = databaseGroupRepository.DeleteDatabaseGroup(databaseGroupId);

            if (!deleteSucceeds)
            {
                return RedirectableClientActionResult.Error(redirectUrl, Messages.DeleteFails);
            }

            return RedirectableClientActionResult.Success(redirectUrl, Messages.DatabaseGroupDeleted);
        }

        static bool RelatedDatabaseGroupDatabaseExists(int databaseCount)
        {
            return databaseCount == 0;
        }

        #endregion

        #region helpers

        static bool DatabaseGroupExist(DatabaseGroup databaseGroupModel)
        {
            return databaseGroupModel != null;
        }

        static bool DatabaseGroupExist(DatabaseGroup databaseGroupModel, int databaseGroupId)
        {
            return databaseGroupModel != null && databaseGroupModel.Id != databaseGroupId;
        }

        #endregion
    }
}