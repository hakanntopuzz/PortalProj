using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract.Repositories
{
    public interface IApplicationSonarqubeProjectRepository
    {
        ICollection<SonarqubeProject> GetSonarqubeProjects(int applicationId);

        SonarqubeProject GetApplicationSonarQubeProjectById(int projectId);

        ICollection<SonarQubeProjectType> GetSonarQubeProjectTypes();

        bool AddApplicationSonarQubeProject(SonarqubeProject project);

        bool DeleteApplicationSonarQubeProject(int projectId);

        bool UpdateApplicationSonarQubeProject(SonarqubeProject project);

        RecordUpdateInfo GetApplicationSonarQubeProjectUpdateInfo(int projectId);
    }
}