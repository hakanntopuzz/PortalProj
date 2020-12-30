using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class SamplesController : Controller
    {
        public IActionResult Index()
        {
            return View(ViewNames.Index);
        }

        public IActionResult Tables()
        {
            return View(ViewNames.Tables);
        }

        public IActionResult Notifications()
        {
            return View(ViewNames.Notifications);
        }

        public IActionResult Buttons()
        {
            return View(ViewNames.Buttons);
        }

        public IActionResult Forms()
        {
            return View(ViewNames.Forms);
        }
    }
}