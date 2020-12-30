namespace DevPortal.Business.Abstract
{
    public interface IEmailSenderService
    {
        bool SendForgotPasswordMail(string emailAddress, string resetUrl);
    }
}