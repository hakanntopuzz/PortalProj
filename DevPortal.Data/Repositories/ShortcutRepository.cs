using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Repositories
{
    public class ShortcutRepository : BaseDevPortalRepository, IShortcutRepository
    {
        #region ctor

        readonly IDataClient dataClient;

        public ShortcutRepository(
            IDataClient dataClient,
            IApplicationDataRequestFactory dataRequestFactory,
            ISettings settings)
            : base(dataClient, dataRequestFactory, settings)
        {
            this.dataClient = dataClient;
        }

        #endregion

        public ICollection<Link> GetFavouriteRedmineProjects()
        {
            var dataRequest = dataRequestFactory.GetFavouriteRedmineProjects();
            var defaultReturnValue = new List<Link>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<Link> GetFavouriteRedmineWikiPages()
        {
            var dataRequest = dataRequestFactory.GetFavouriteRedmineWikiPages();
            var defaultReturnValue = new List<Link>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }

        public ICollection<Link> GetToolPages()
        {
            var dataRequest = dataRequestFactory.GetToolPages();
            var defaultReturnValue = new List<Link>();

            return dataClient.GetCollection(dataRequest, defaultReturnValue);
        }
    }
}