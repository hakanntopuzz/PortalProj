using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace DevPortal.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return PageNotFound();
        }

        [Route("error/{errorCode:int}")]
        public IActionResult Error(int errorCode = 404)
        {
            if (errorCode == Convert.ToInt32(HttpStatusCode.NotFound))
            {
                return PageNotFound();
            }
            else if (errorCode == Convert.ToInt32(HttpStatusCode.Forbidden))
            {
                return Forbidden();
            }
            else
            {
                return InternalServerError();
            }
        }

        public IActionResult PageNotFound()
        {
            var viewName = Convert.ToInt32(HttpStatusCode.NotFound).ToString();

            return View(viewName);
        }

        public IActionResult Forbidden()
        {
            var viewName = Convert.ToInt32(HttpStatusCode.Forbidden).ToString();

            return View(viewName);
        }

        public IActionResult InternalServerError()
        {
            var viewName = Convert.ToInt32(HttpStatusCode.InternalServerError).ToString();

            return View(viewName);
        }
    }
}