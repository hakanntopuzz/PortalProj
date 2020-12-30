using DevPortal.Framework.Abstract;
using Microsoft.AspNetCore.Routing;

namespace DevPortal.Framework.Services
{
    public class UrlGeneratorService : IUrlGeneratorService
    {
        #region ctor

        readonly ISettings settings;

        readonly IUrlHelper urlHelper;

        readonly IRouteValueFactory routeValueFactory;

        public UrlGeneratorService(
            ISettings settings,
            IUrlHelper urlHelper,
            IRouteValueFactory routeValueFactory)
        {
            this.settings = settings;
            this.urlHelper = urlHelper;
            this.routeValueFactory = routeValueFactory;
        }

        #endregion

        #region private

        string GenerateFullUrl(string relativeUrl)
        {
            return $"{settings.SiteUrl}{relativeUrl}";
        }

        #endregion

        #region route url

        public string RouteUrl(string routeName)
        {
            return urlHelper.RouteUrl(routeName);
        }

        public string RouteUrl(string routeName, RouteValueDictionary routeValues)
        {
            return urlHelper.RouteUrl(routeName, routeValues);
        }

        #endregion

        #region url

        public string GenerateUrl(string action, string controller)
        {
            var relativeUrl = urlHelper.Action(action, controller);

            return GenerateFullUrl(relativeUrl);
        }

        public string GenerateUrl(string action, string controller, int id)
        {
            var routeValues = routeValueFactory.CreateRouteValuesForGenerateUrl(id);

            var relativeUrl = urlHelper.Action(action, controller, routeValues);

            return GenerateFullUrl(relativeUrl);
        }

        public string GenerateUrl(string action, string controller, RouteValueDictionary routeValues)
        {
            var relativeUrl = urlHelper.Action(action, controller, routeValues);

            return GenerateFullUrl(relativeUrl);
        }

        #endregion
    }
}