using Microsoft.AspNetCore.Routing;

namespace DevPortal.Framework.Abstract
{
    public interface IRouteValueFactory
    {
        RouteValueDictionary CreateRouteValuesForGenerateUrl(int id);

        RouteValueDictionary CreateRouteValuesForToken(string token);

        RouteValueDictionary CreateRouteValueForId(int id);
    }
}