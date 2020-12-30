using DevPortal.Model;

namespace DevPortal.Business.Abstract
{
    public interface IDatabaseWriterService
    {
        ServiceResult UpdateDatabase(Database database);

        ServiceResult AddDatabase(Database database);

        ServiceResult DeleteDatabase(int databaseId);
    }
}