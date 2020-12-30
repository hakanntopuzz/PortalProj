using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.Data.Repositories
{
    public class AuditRepository : BaseDevPortalRepository, IAuditRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        readonly IApplicationDataRequestFactory applicationDataRequestFactory;

        public AuditRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings) : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
            this.applicationDataRequestFactory = dataRequestFactory;
        }

        #endregion

        public async Task<bool> AddAsync(string tableName, int recordId, string fieldName, string oldValue, string newValue, int modifiedBy)
        {
            var dataRequest = applicationDataRequestFactory.AddAudit(tableName, recordId, fieldName, oldValue, newValue, modifiedBy);
            const bool defaultReturnValue = false;

            return await dataClient.GetScalarAsync(dataRequest, defaultReturnValue);
        }

        public async Task<ICollection<Audit>> GetFilteredAuditListAsync(int skip, int take, string orderBy, string orderDir, string searchText, string tableName, int recordId)
        {
            var dataRequest = applicationDataRequestFactory.GetFilteredAuditList(skip, take, orderBy, orderDir, searchText, tableName, recordId);
            var defaultReturnValue = new List<Audit>();

            return await dataClient.GetCollectionAsync<Audit>(dataRequest, defaultReturnValue);
        }
    }
}