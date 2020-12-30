using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DevPortal.Web.Library
{
    public class PolicyBasedAuthorizeAttribute : TypeFilterAttribute
    {
        public PolicyBasedAuthorizeAttribute(Policy policy) : base(typeof(PolicyBasedAuthorize))
        {
            Arguments = new object[] { policy.ToString() };
        }
    };

    public class PolicyBasedAuthorize : IAuthorizationFilter
    {
        readonly string policy;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public PolicyBasedAuthorize(string policy,
            IAuthorizationServiceWrapper authorizationWrapper)
        {
            this.policy = policy;
            this.authorizationWrapper = authorizationWrapper;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Check active user policy.
            var isAuthorize = authorizationWrapper.CheckUserHasPolicy(policy);

            if (isAuthorize)
            {
                return;
            }

            var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            string returnType = controllerActionDescriptor.MethodInfo.ReturnType.Name;

            if (returnType == nameof(JsonResult))
            {
                context.Result = new JsonResult(BaseResult.Create(false, Messages.NotAuthorizedTransaction));
            }
            else
            {
                context.Result = new ForbidResult();
            }
        }
    }
}