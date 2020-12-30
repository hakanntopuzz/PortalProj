using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Repositories
{
    public class ApplicationNugetPackageRepository : BaseDevPortalRepository, IApplicationNugetPackageRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        new readonly IApplicationDataRequestFactory dataRequestFactory;

        public ApplicationNugetPackageRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings)
          : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
            this.dataRequestFactory = dataRequestFactory;
        }

        #endregion

        public ICollection<ApplicationNugetPackage> GetNugetPackages(int applicationId)
        {
            var dataRequest = dataRequestFactory.GetNugetPackages(applicationId);
            var defaultReturnValue = new List<ApplicationNugetPackage>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ApplicationNugetPackage GetApplicationNugetPackageById(int packageId)
        {
            var dataRequest = dataRequestFactory.GetApplicationNugetPackageById(packageId);
            const ApplicationNugetPackage defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public RecordUpdateInfo GetPackageUpdateInfo(int id)
        {
            var dataRequest = dataRequestFactory.GetPackageUpdateInfo(id);
            const RecordUpdateInfo defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public bool AddApplicationNugetPackage(ApplicationNugetPackage package)
        {
            var dataRequest = dataRequestFactory.AddApplicationNugetPackage(package);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public bool UpdateApplicationNugetPackage(ApplicationNugetPackage nugetPackage)
        {
            var dataRequest = dataRequestFactory.UpdateApplicationNugetPackage(nugetPackage);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public ApplicationNugetPackage GetApplicationNugetPackageByName(string packageName)
        {
            var dataRequest = dataRequestFactory.GetApplicationNugetPackageByName(packageName);
            const ApplicationNugetPackage defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public bool DeleteApplicationNugetPackage(int packageId)
        {
            var dataRequest = dataRequestFactory.DeleteApplicationNugetPackage(packageId);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }
    }
}