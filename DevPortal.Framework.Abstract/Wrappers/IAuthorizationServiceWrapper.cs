namespace DevPortal.Framework.Abstract
{
    public interface IAuthorizationServiceWrapper
    {
        bool CheckUserHasAdminPolicy();

        bool CheckUserHasAdminDeveloperPolicy();

        bool CheckUserHasPolicy(string policy);
    }
}