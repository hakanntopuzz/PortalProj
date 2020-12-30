using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface IApplicationSvnViewModelFactory
    {
        ApplicationSvnViewModel CreateApplicationSvnViewModel(int applicationId, string applicationName, string svnUrl, ICollection<SvnRepositoryType> repositoryTypeList);

        ApplicationSvnViewModel CreateApplicationSvnDetailViewModel(SvnRepository svnRepository);

        ApplicationSvnViewModel CreateApplicationSvnEditViewModel(SvnRepository svnRepository, ICollection<SvnRepositoryType> repositoryTypeList);
    }
}