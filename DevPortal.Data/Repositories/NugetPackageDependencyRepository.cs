using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;

namespace DevPortal.Data.Repositories
{
    public class NugetPackageDependencyRepository : BaseDevPortalRepository, INugetPackageDependencyRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        public NugetPackageDependencyRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings)
            : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
        }

        #endregion

        public NugetPackageDependency GetNugetPackageDependencyById(int id)
        {
            var dataRequest = dataRequestFactory.GetNugetPackageDependenciesById(id);
            const NugetPackageDependency defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public RecordUpdateInfo GetNugetPackageDependencyUpdateInfo(int id)
        {
            var dataRequest = dataRequestFactory.GetNugetPackageDependencyUpdateInfo(id);
            const RecordUpdateInfo defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public bool AddNugetPackageDependency(NugetPackageDependency nugetPackageDependency)
        {
            var dataRequest = dataRequestFactory.AddNugetPackageDependency(nugetPackageDependency);

            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public bool DeleteNugetPackageDependency(int nugetPackageDependencyId)
        {
            var dataRequest = dataRequestFactory.DeleteNugetPackageDependency(nugetPackageDependencyId);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }
    }
}