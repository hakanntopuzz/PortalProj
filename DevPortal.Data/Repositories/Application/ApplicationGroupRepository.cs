using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Data.Factories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Repositories
{
    public class ApplicationGroupRepository : BaseDevPortalRepository, IApplicationGroupRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        new readonly IApplicationDataRequestFactory dataRequestFactory;

        public ApplicationGroupRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings)
          : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
            this.dataRequestFactory = dataRequestFactory;
        }

        #endregion

        public ICollection<ApplicationGroup> GetApplicationGroups()
        {
            var dataRequest = dataRequestFactory.GetApplicationGroupList();
            var defaultReturnValue = new List<ApplicationGroup>();

            return dataClient.GetCollection<ApplicationGroup, RecordUpdateInfo, ApplicationGroup>(
                dataRequest,
                DataClientMapFactory.ApplicationGroupsMap,
                defaultReturnValue,
                dataRequest.SplitOnParameters);
        }

        public int AddApplicationGroup(ApplicationGroup applicationGroup)
        {
            var dataRequest = dataRequestFactory.AddApplicationGroup(applicationGroup);
            const int defaultReturnValue = 0;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public ApplicationGroup GetApplicationGroupByName(string name)
        {
            var dataRequest = dataRequestFactory.GetApplicationGroupByName(name);
            const ApplicationGroup defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public ApplicationGroup GetApplicationGroupById(int id)
        {
            var dataRequest = dataRequestFactory.GetApplicationGroupById(id);
            const ApplicationGroup defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }

        public bool UpdateApplicationGroup(ApplicationGroup group)
        {
            var dataRequest = dataRequestFactory.UpdateApplicationGroup(group);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public ICollection<ApplicationGroupStatus> GetApplicationGroupStatusList()
        {
            var dataRequest = dataRequestFactory.GetApplicationGroupStatusList();
            var defaultReturnValue = new List<ApplicationGroupStatus>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public bool DeleteApplicationGroup(int groupId)
        {
            var dataRequest = dataRequestFactory.DeleteApplicationGroup(groupId);
            const bool defaultReturnValue = false;

            return dataClient.GetScalar(dataRequest, defaultReturnValue);
        }

        public RecordUpdateInfo GetApplicationGroupUpdateInfo(int applicationGroupId)
        {
            var dataRequest = dataRequestFactory.GetApplicationGroupUpdateInfo(applicationGroupId);
            const RecordUpdateInfo defaultReturnValue = null;

            return dataClient.GetItem(dataRequest, defaultReturnValue);
        }
    }
}