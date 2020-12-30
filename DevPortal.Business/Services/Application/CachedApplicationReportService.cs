using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework;
using DevPortal.Framework.Abstract;

namespace DevPortal.Business.Services
{
    public class CachedApplicationReportService : ApplicationReportService, IApplicationReportService
    {
        #region members & ctor

        readonly ICacheWrapper cache;

        readonly ISettings settings;

        public CachedApplicationReportService(
            IApplicationRepository applicationRepository,
            ICacheWrapper cache,
            ISettings settings)
            : base(applicationRepository)
        {
            this.cache = cache;
            this.settings = settings;
        }

        #endregion

        #region overrides

        protected override int GetJenkinsJobCountCore()
        {
            return cache.GetOrCreateWithSlidingExpiration(
                CacheKeyNames.JenkinsJobCountCacheKey,
                base.GetJenkinsJobCountCore,
                settings.JenkinsJobCountCacheTimeInMinutes);
        }

        #endregion
    }
}