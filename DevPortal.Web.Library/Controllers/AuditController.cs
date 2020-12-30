using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevPortal.Web.Library.Controllers
{
    public class AuditController : BaseController
    {
        #region ctor

        readonly IAuditService auditService;

        public AuditController(
            IUserSessionService userSessionService,
            IAuditService auditService) : base(userSessionService)
        {
            this.auditService = auditService;
        }

        #endregion

        [HttpPost]

        //TODO: ValidateAntiforgeryToken eklenmeli.
        public async Task<IActionResult> Index(AuditTableParam tableParam)
        {
            var jqTable = await auditService.GetFilteredAuditListAsJqTableAsync(tableParam);

            return Ok(jqTable);
        }
    }
}