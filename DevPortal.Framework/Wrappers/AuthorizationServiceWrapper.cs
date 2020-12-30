using DevPortal.Framework.Abstract;
using DevPortal.Model;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace DevPortal.Framework.Wrappers
{
    public class AuthorizationServiceWrapper : IAuthorizationServiceWrapper
    {
        #region ctor

        readonly IAuthorizationService authorizationService;

        readonly IHttpContextWrapper httpContextWrapper;

        public AuthorizationServiceWrapper(
            IAuthorizationService authorizationService,
            IHttpContextWrapper httpContextWrapper)
        {
            this.authorizationService = authorizationService;
            this.httpContextWrapper = httpContextWrapper;
        }

        #endregion

        async Task<bool> CheckPolicyAsync(string policy)
        {
            var user = httpContextWrapper.GetCurrentUser();
            var result = await authorizationService.AuthorizeAsync(user, policy);

            return result.Succeeded;
        }

        public bool CheckUserHasAdminPolicy()
        {
            var isAuthorized = CheckPolicyAsync(Policy.Admin.ToString());

            return isAuthorized.Result;
        }

        public bool CheckUserHasAdminDeveloperPolicy()
        {
            var isAuthorized = CheckPolicyAsync(Policy.AdminDeveloper.ToString());

            return isAuthorized.Result;
        }

        public bool CheckUserHasPolicy(string policy)
        {
            var isAuthorized = CheckPolicyAsync(policy);

            return isAuthorized.Result;
        }
    }
}