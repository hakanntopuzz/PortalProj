using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace DevPortal.Web.Library
{
    public class GlobalExceptionFilterAttribute : IExceptionFilter
    {
        #region ctor

        readonly ILoggingService loggingService;

        public GlobalExceptionFilterAttribute(ILoggingService loggingService)
        {
            this.loggingService = loggingService;
        }

        #endregion

        public void OnException(ExceptionContext context)
        {
            var methodName = $"{ context.RouteData.Values["controller"].ToString()}.{ context.RouteData.Values["action"].ToString()}";
            var message = $"{nameof(GlobalExceptionFilterAttribute)}.{nameof(OnException)}";
            loggingService.LogError(methodName, message, context.Exception);

            var routeData = new RouteValueDictionary(new
            {
                controller = ControllerNames.Error,
                action = ErrorControllerActionNames.InternalServerError
            });

            context.ExceptionHandled = true;
            context.Result = new RedirectToRouteResult(routeData);
            context.Result.ExecuteResultAsync(context);
        }
    }
}