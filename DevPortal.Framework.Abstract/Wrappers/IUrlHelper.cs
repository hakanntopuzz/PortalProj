namespace DevPortal.Framework.Abstract
{
    public interface IUrlHelper
    {
        bool IsLocalUrl(string url);

        string Action(string actionName);

        string Action(string actionName, string controllerName);

        string Action(string actionName, string controllerName, object routeValues);

        string Action(string actionName, string controllerName, int id);

        string Action(string actionName, string controllerName, object routeValues, string protocol);

        string RouteUrl(string routeName);

        string RouteUrl(string routeName, object routeValues);
    }
}