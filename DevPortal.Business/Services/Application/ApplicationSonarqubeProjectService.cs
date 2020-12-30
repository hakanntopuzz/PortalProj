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
    public class ApplicationSonarqubeProjectService : IApplicationSonarqubeProjectService
    {
        #region ctor

        readonly IApplicationSonarqubeProjectRepository repository;

        readonly IGeneralSettingsService generalSettingsService;

        readonly IAuditService auditService;

        readonly IAuditFactory auditFactory;

        readonly ILoggingService loggingService;

        public ApplicationSonarqubeProjectService(
            IApplicationSonarqubeProjectRepository repository,
            IGeneralSettingsService generalSettingsService,
            IAuditService auditService,
            IAuditFactory auditFactory,
            ILoggingService loggingService)
        {
            this.repository = repository;
            this.generalSettingsService = generalSettingsService;
            this.auditService = auditService;
            this.auditFactory = auditFactory;
            this.loggingService = loggingService;
        }

        #endregion

        public ICollection<SonarqubeProject> GetSonarqubeProjects(int applicationId)
        {
            var sonarqubeProjectUrl = generalSettingsService.GetSonarqubeProjectUrl();
            var projects = repository.GetSonarqubeProjects(applicationId);

            projects.ToList().ForEach(q => q.ProjectUrl = sonarqubeProjectUrl + q.SonarqubeProjectName);

            return projects;
        }

        public SonarqubeProject GetSonarQubeProject(int projectId)
        {
            var project = repository.GetApplicationSonarQubeProjectById(projectId);

            if (project == null)
            {
                return null;
            }

            project.RecordUpdateInfo = repository.GetApplicationSonarQubeProjectUpdateInfo(projectId);

            return project;
        }

        public ICollection<SonarQubeProjectType> GetSonarQubeProjectTypes()
        {
            return repository.GetSonarQubeProjectTypes();
        }

        public ServiceResult AddApplicationSonarQubeProject(SonarqubeProject project)
        {
            var addSucceeds = repository.AddApplicationSonarQubeProject(project);

            if (!addSucceeds)
            {
                return ServiceResult.Error(Messages.AddingFails);
            }

            return ServiceResult.Success(Messages.ApplicationSonarQubeProjectCreated);
        }

        public ServiceResult DeleteApplicationSonarQubeProject(int projectId)
        {
            var isSuccess = repository.DeleteApplicationSonarQubeProject(projectId);

            if (!isSuccess)
            {
                return ServiceResult.Error(Messages.DeleteFails);
            }

            return ServiceResult.Success(Messages.ApplicationSonarQubeProjectDeleted);
        }

        public ServiceResult UpdateApplicationSonarQubeProject(SonarqubeProject project)
        {
            if (project == null)
            {
                return ServiceResult.Error(Messages.NullParameterError);
            }
            var oldSonarqubeProject = repository.GetApplicationSonarQubeProjectById(project.SonarqubeProjectId);

            var isChanged = auditService.IsChanged(oldSonarqubeProject, project, nameof(SonarqubeProject));

            if (!isChanged)
            {
                return ServiceResult.Success(Messages.ApplicationSonarQubeProjectUpdated);
            }

            try
            {
                using (var scope = TransactionScopeFactory.CreateTransactionScope())
                {
                    UpdateApplicationSonarqubeProjectCore(project);

                    var newSonarqubeProject = repository.GetApplicationSonarQubeProjectById(project.SonarqubeProjectId);

                    AddAuditCore(newSonarqubeProject, oldSonarqubeProject, project.RecordUpdateInfo.ModifiedBy);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                loggingService.LogError(nameof(UpdateApplicationSonarQubeProject), "Uygulama sonarqube projesi güncelleme sırasında beklenmeyen bir hata oluştu.", ex);

                return ServiceResult.Error(Messages.UpdateFails);
            }

            return ServiceResult.Success(Messages.ApplicationSonarQubeProjectUpdated);
        }

        void UpdateApplicationSonarqubeProjectCore(SonarqubeProject sonarqubeProject)
        {
            var updateSuccess = repository.UpdateApplicationSonarQubeProject(sonarqubeProject);

            if (!updateSuccess)
            {
                throw new TransactionIstopException("Uygulama sonarqube projesi güncelleme sırasında beklenmeyen bir hata oluştu");
            }
        }

        void AddAuditCore(SonarqubeProject sonarqubeProject, SonarqubeProject oldSonarqubeProject, int userId)
        {
            var auditInfo = auditFactory.CreateAuditInfo(nameof(SonarqubeProject), sonarqubeProject.SonarqubeProjectId, oldSonarqubeProject, sonarqubeProject, userId);
            var isSuccess = auditService.AddAsync(auditInfo).Result;

            if (!isSuccess)
            {
                throw new TransactionIstopException("Audit ekleme sırasında beklenmeyen bir hata oluştu");
            }
        }
    }
}