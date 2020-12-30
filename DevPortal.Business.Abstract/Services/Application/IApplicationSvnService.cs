using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IApplicationSvnService
    {
        ServiceResult AddApplicationSvnRepository(SvnRepository svnRepository);

        SvnRepository GetApplicationSvnRepository(int repositoryId);

        ServiceResult UpdateApplicationSvnRepository(SvnRepository svnRepository);

        ServiceResult DeleteApplicationSvnRepository(int svnRepositoryId);

        ICollection<SvnRepositoryType> GetSvnRepositoryTypes();
    }
}