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
    public class ApplicationGroupService : IApplicationGroupService
    {
        #region ctor

        readonly IApplicationGroupRepository applicationGroupRepository;

        readonly IApplicationRepository applicationRepository;

        readonly IAuditService auditService;

        readonly IAuditFactory auditFactory;

        readonly ILoggingService loggingService;

        public ApplicationGroupService(
            IApplicationGroupRepository applicationGroupRepository,
            IApplicationRepository applicationRepository,
            IAuditService auditService,
            IAuditFactory auditFactory,
            ILoggingService loggingService)
        {
            this.applicationGroupRepository = applicationGroupRepository;
            this.applicationRepository = applicationRepository;
            this.auditService = auditService;
            this.auditFactory = auditFactory;
            this.loggingService = loggingService;
        }

        #endregion

        public ICollection<ApplicationGroup> GetApplicationGroups()
        {
            return applicationGroupRepository.GetApplicationGroups();
        }

        public Int32ServiceResult AddApplicationGroup(ApplicationGroup applicationGroup)
        {
            var group = GetApplicationGroupByName(applicationGroup.Name);

            if (ApplicationGroupExists(group))
            {
                return Int32ServiceResult.Error(Messages.ApplicationGroupNameExists);
            }

            var applicationGroupId = applicationGroupRepository.AddApplicationGroup(applicationGroup);

            if (applicationGroupId == 0)
            {
                return Int32ServiceResult.Error(Messages.AddingFails);
            }

            return Int32ServiceResult.Success(Messages.AddingSucceeds, applicationGroupId);
        }

        public ApplicationGroup GetApplicationGroupByName(string name)
        {
            return applicationGroupRepository.GetApplicationGroupByName(name);
        }

        public ApplicationGroup GetApplicationGroupById(int applicationGroupId)
        {
            var applicationGroup = applicationGroupRepository.GetApplicationGroupById(applicationGroupId);

            if (applicationGroup == null)
            {
                return null;
            }

            applicationGroup.RecordUpdateInfo = applicationGroupRepository.GetApplicationGroupUpdateInfo(applicationGroupId);

            return applicationGroup;
        }

        public ServiceResult UpdateApplicationGroup(ApplicationGroup applicationGroup)
        {
            if (applicationGroup == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }

            if (ApplicationGroupExistsForEdit(applicationGroup))
            {
                return ServiceResult.Error(Messages.ApplicationGroupNameExists);
            }

            var oldApplicationGroup = applicationGroupRepository.GetApplicationGroupById(applicationGroup.Id);

            var isChanged = auditService.IsChanged(oldApplicationGroup, applicationGroup, nameof(BaseApplicationGroup));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.ApplicationGroupUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateApplicationGroupCore(applicationGroup);

                    var newApplication = applicationGroupRepository.GetApplicationGroupById(applicationGroup.Id);

                    AddAuditCore(newApplication, oldApplicationGroup, applicationGroup.RecordUpdateInfo.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(UpdateApplicationGroup), "Uygulama güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.ApplicationGroupUpdated);
        }

        void UpdateApplicationGroupCore(ApplicationGroup applicationGroup)
        {
            var updateSuccess = applicationGroupRepository.UpdateApplicationGroup(applicationGroup);

            if (!updateSuccess)
            {
                throw new TransactionIstopException("Uygulama grubu güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        void AddAuditCore(ApplicationGroup applicationGroup, ApplicationGroup oldApplicationGroup, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(ApplicationGroup), applicationGroup.Id, oldApplicationGroup, applicationGroup, userId);
            var isSuccess = auditService.AddAsync(auditInfo).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        bool ApplicationGroupExistsForEdit(ApplicationGroup applicationGroup)
        {
            var group = GetApplicationGroupByName(applicationGroup.Name);
            if (ApplicationGroupExists(group))
            {
                if (group.Id == applicationGroup.Id)
                {
                    return false;
                }
                return true;
            }

            return false;
        }

        static bool ApplicationGroupExists(ApplicationGroup group)
        {
            return group != null;
        }

        public ICollection<ApplicationGroupStatus> GetApplicationGroupStatusList()
        {
            return applicationGroupRepository.GetApplicationGroupStatusList();
        }

        public ServiceResult DeleteApplicationGroup(int groupId)
        {
            var applications = applicationRepository.GetApplicationsByGroupId(groupId);

            if (RelatedApplicationsExists(applications))
            {
                return ServiceResult.Error(Messages.RelatedApplicationsExists);
            }

            var addResult = applicationGroupRepository.DeleteApplicationGroup(groupId);

            if (!addResult)
            {
                return ServiceResult.Error(Messages.DeleteFails);
            }

            return ServiceResult.Success(Messages.ApplicationGroupDeleted);
        }

        static bool RelatedApplicationsExists(ICollection<ApplicationListItem> applications)
        {
            return applications != null && applications.Count > 0;
        }
    }
}