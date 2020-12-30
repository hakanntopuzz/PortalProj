using DevPortal.Framework.Abstract;
using DevPortal.Web.Library.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Controllers
{
    public class SiteController : BaseController
    {
        #region ctor

        readonly ISettings settings;

        public SiteController(IUserSessionService userSessionService, ISettings settings)
            : base(userSessionService)
        {
            this.settings = settings;
        }

        #endregion

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Version()
        {
            return Content(settings.ApplicationVersion);
        }
    }
}