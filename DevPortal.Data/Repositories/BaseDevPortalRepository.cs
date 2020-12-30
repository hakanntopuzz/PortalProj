using AB.Data.DapperClient.Abstract;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Framework.Abstract;

namespace DevPortal.Data.Repositories
{
    public class BaseDevPortalRepository : BaseRepository
    {
        public IDataClient DataClient { get; set; }

        //TODO: IApplicationDataRequestFactory bağımlılığı diğer modüller için zorluk çıkarıyor.
        public BaseDevPortalRepository(IDataClient dataClient, IApplicationDataRequestFactory dataRequestFactory, ISettings settings)
            : base(dataRequestFactory)
        {
            DataClient = dataClient;

            dataClient.SetConnectionString(settings.DevPortalDbConnectionString);
        }
    }
}