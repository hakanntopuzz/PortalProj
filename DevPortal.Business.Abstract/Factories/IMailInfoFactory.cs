using DevPortal.Framework.Model;

namespace DevPortal.Business.Abstract
{
    public interface IMailInfoFactory
    {
        MailInfo CreateForgotPasswordMailInfo(string emailAddress, string resetUrl);
    }
}