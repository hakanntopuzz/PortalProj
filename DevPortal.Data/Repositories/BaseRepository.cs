using DevPortal.Data.Abstract.Factories;

namespace DevPortal.Data.Repositories
{
    public class BaseRepository
    {
        #region ctor

        protected readonly IApplicationDataRequestFactory dataRequestFactory;

        public BaseRepository(IApplicationDataRequestFactory dataRequestFactory)
        {
            this.dataRequestFactory = dataRequestFactory;
        }

        #endregion
    }
}