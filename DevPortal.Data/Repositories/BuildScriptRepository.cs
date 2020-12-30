using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Repositories
{
    public class BuildScriptRepository : BaseDevPortalRepository, IBuildScriptRepository
    {
        readonly IDataClient dataClient;

        public BuildScriptRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings) : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
        }

        public IEnumerable<BuildType> BuildTypes
        {
            get
            {
                var dataRequest = dataRequestFactory.BuildTypes;
                var defaultReturnValue = new List<BuildType>();

                return dataClient.GetCollection(dataRequest, defaultReturnValue);
            }
        }
    }
}