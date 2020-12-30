using DevPortal.Framework.Abstract;
using Microsoft.AspNetCore.Routing;

namespace DevPortal.Framework.Factories
{
    public class RouteValueFactory : IRouteValueFactory
    {
        public RouteValueDictionary CreateRouteValuesForGenerateUrl(int id)
        {
            return CreateRouteValueForId(id);
        }

        public RouteValueDictionary CreateRouteValuesForToken(string token)
        {
            return new RouteValueDictionary
            {
                { WebParameterNames.token, token }
            };
        }

        public RouteValueDictionary CreateRouteValueForId(int id)
        {
            return new RouteValueDictionary
            {
                { WebParameterNames.id, id }
            };
        }
    }
}