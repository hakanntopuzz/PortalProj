using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationSonarqubeProjectService
    {
        ICollection<SonarqubeProject> GetSonarqubeProjects(int applicationId);

        SonarqubeProject GetSonarQubeProject(int projectId);

        ICollection<SonarQubeProjectType> GetSonarQubeProjectTypes();

        ServiceResult AddApplicationSonarQubeProject(SonarqubeProject project);

        ServiceResult DeleteApplicationSonarQubeProject(int projectId);

        ServiceResult UpdateApplicationSonarQubeProject(SonarqubeProject project);
    }
}