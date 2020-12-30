using DevPortal.Web.Library.Model;

namespace DevPortal.Web.Library.Abstract
{
    public interface IAuditViewModelFactory
    {
        AuditViewModel CreateAuditViewModel(string tableName, int recordId);
    }
}