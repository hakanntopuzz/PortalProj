using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Repositories
{
    public class ApplicationSvnRepository : BaseDevPortalRepository, IApplicationSvnRepository
    {
        #region ctor

        public ApplicationSvnRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings) : base(dataClient, dataRequestFactory, settings)
        {
        }

        #endregion

        public bool AddApplicationSvnRepository(SvnRepository svnRepository)
        {
            var dataRequest = dataRequestFactory.AddApplicationSvnRepository(svnRepository);
            const bool defaultReturnValue = false;

            return DataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public bool UpdateApplicationSvnRepository(SvnRepository svnRepository)
        {
            var dataRequest = dataRequestFactory.UpdateApplicationSvnRepository(svnRepository);
            const bool defaultReturnValue = false;

            return DataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public bool DeleteApplicationSvnRepository(int svnRepositoryId)
        {
            var dataRequest = dataRequestFactory.DeleteApplicationSvnRepository(svnRepositoryId);
            const bool defaultReturnValue = false;

            return DataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public SvnRepository GetApplicationSvnRepositoryById(int id)
        {
            var dataRequest = dataRequestFactory.GetApplicationSvnRepositoryById(id);
            const SvnRepository defaultReturnValue = null;

            return DataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public ICollection<SvnRepositoryType> GetSvnRepositoryTypes()
        {
            var dataRequest = dataRequestFactory.GetSvnRepositoryTypes();
            var defaultValue = new List<SvnRepositoryType>();

            return DataClient.GetCollection(dataRequest, defaultValue);
        }

        public RecordUpdateInfo GetApplicationSvnRepositoryUpdateInfo(int repositoryId)
        {
            var dataRequest = dataRequestFactory.GetApplicationSvnRepositoryUpdateInfo(repositoryId);
            const RecordUpdateInfo defaultReturnValue = null;

            return DataClient.GetItem(dataRequest, defaultReturnValue);
        }
    }
}