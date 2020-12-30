using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace DevPortal.Framework.Wrappers
{
    public class UrlHelperWrapper : Abstract.IUrlHelper
    {
        #region members

        readonly IUrlHelperFactory urlHelperFactory;

        readonly IActionContextAccessor actionAccessor;

        public IUrlHelper UrlHelper => this.urlHelperFactory.GetUrlHelper(this.actionAccessor.ActionContext);

        #endregion

        #region ctor

        public UrlHelperWrapper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionAccessor)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.actionAccessor = actionAccessor;
        }

        #endregion

        public string Action(string actionName)
        {
            return UrlHelper.Action(actionName);
        }

        public string Action(string actionName, string controllerName)
        {
            return UrlHelper.Action(actionName, controllerName);
        }

        public string Action(string actionName, string controllerName, object routeValues)
        {
            return UrlHelper.Action(actionName, controllerName, routeValues);
        }

        public string Action(string actionName, string controllerName, int id)
        {
            return UrlHelper.Action(actionName, controllerName, new { id });
        }

        public string Action(string actionName, string controllerName, object routeValues, string protocol)
        {
            return UrlHelper.Action(actionName, controllerName, routeValues, protocol);
        }

        public bool IsLocalUrl(string url)
        {
            return UrlHelper.IsLocalUrl(url);
        }

        public string RouteUrl(string routeName)
        {
            return UrlHelper.RouteUrl(routeName);
        }

        public string RouteUrl(string routeName, object routeValues)
        {
            return UrlHelper.RouteUrl(routeName, routeValues);
        }
    }
}