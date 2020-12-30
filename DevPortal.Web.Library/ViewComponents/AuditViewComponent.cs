using DevPortal.Web.Library.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.ViewComponents
{
    public class AuditViewComponent : ViewComponent
    {
        #region ctor

        readonly IAuditViewModelFactory viewModelFactory;

        public AuditViewComponent(IAuditViewModelFactory viewModelFactory)
        {
            this.viewModelFactory = viewModelFactory;
        }

        #endregion

        public IViewComponentResult Invoke(string tableName, int recordId)
        {
            var auditViewModel = viewModelFactory.CreateAuditViewModel(tableName, recordId);

            return View(auditViewModel);
        }
    }
}