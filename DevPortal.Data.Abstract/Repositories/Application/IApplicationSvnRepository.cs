using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract.Repositories
{
    public interface IApplicationSvnRepository
    {
        bool AddApplicationSvnRepository(SvnRepository svnRepository);

        SvnRepository GetApplicationSvnRepositoryById(int id);

        bool UpdateApplicationSvnRepository(SvnRepository svnRepository);

        bool DeleteApplicationSvnRepository(int svnRepositoryId);

        ICollection<SvnRepositoryType> GetSvnRepositoryTypes();

        RecordUpdateInfo GetApplicationSvnRepositoryUpdateInfo(int repositoryId);
    }
}