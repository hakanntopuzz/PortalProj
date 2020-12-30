using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface IApplicationSonarQubeProjectViewModelFactory
    {
        #region sonarQube project

        ApplicationSonarQubeProjectViewModel CreateApplicationSonarQubeProjectViewModel(Application application, ICollection<SonarQubeProjectType> projectTypeList, string projectUrl);

        ApplicationSonarQubeProjectViewModel CreateApplicationSonarQubeProjectViewModel(Application application, SonarqubeProject project, ICollection<SonarQubeProjectType> projectTypeList, string projectUrl);

        ApplicationSonarQubeProjectViewModel CreateEditApplicationSonarQubeProjectViewModel(Application application, SonarqubeProject project, ICollection<SonarQubeProjectType> projectTypeList, string projectUrl);

        #endregion
    }
}