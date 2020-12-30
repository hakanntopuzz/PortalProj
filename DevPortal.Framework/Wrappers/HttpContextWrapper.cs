using DevPortal.Framework.Abstract;
using jsreport.AspNetCore;
using jsreport.Types;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevPortal.Framework.Wrappers
{
    public class HttpContextWrapper : IHttpContextWrapper
    {
        readonly IActionContextAccessor actionContextAccessor;

        readonly ISettings settings;

        public HttpContextWrapper(IActionContextAccessor actionContextAccessor,
             ISettings settings)
        {
            this.actionContextAccessor = actionContextAccessor;
            this.settings = settings;
        }

        private HttpContext HttpContext => actionContextAccessor.ActionContext.HttpContext;

        public string GetUserAgentString()
        {
            return this.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        public string GetUserIpAddress()
        {
            return this.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        public async Task SignInAsync(ClaimsPrincipal claimsPrincipal, bool isRemember)
        {
            await this.HttpContext.SignInAsync(settings.AuthenticationScheme,
               claimsPrincipal, new AuthenticationProperties { IsPersistent = isRemember });
        }

        public async Task SignOutAsync()
        {
            await this.HttpContext.SignOutAsync();
        }

        public ClaimsPrincipal GetCurrentUser()
        {
            return this.HttpContext.User;
        }

        public void DownloadAsPdf(string fileName)
        {
            var httpContext = this.HttpContext;

            httpContext.JsReportFeature().Recipe(Recipe.ChromePdf)
           .OnAfterRender((r) => httpContext.Response.Headers["Content-Disposition"] = $"attachment; filename=\"{fileName}.pdf\"");
        }
    }
}