using Microsoft.AspNetCore.Routing;

namespace DevPortal.Framework.Abstract
{
    public interface IUrlGeneratorService
    {
        #region route url

        /// <summary>
        /// routeName parametresi ile url'i oluşturur.
        /// </summary>
        /// <param name="routeName"></param>
        /// <returns></returns>
        string RouteUrl(string routeName);

        /// <summary>
        /// Route URL oluşturulur.
        /// </summary>
        /// <param name="routeName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        string RouteUrl(string routeName, RouteValueDictionary routeValues);

        #endregion

        #region url

        /// <summary>
        /// Varsayılan route tanımı ile link oluşturur.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        string GenerateUrl(string action, string controller);

        /// <summary>
        /// Varsayılan route tanımı ile link oluşturur.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        string GenerateUrl(string action, string controller, int id);

        /// <summary>
        /// Varsayılan route tanımı ve route bilgileri ile link oluşturur.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        string GenerateUrl(string action, string controller, RouteValueDictionary routeValues);

        #endregion
    }
}