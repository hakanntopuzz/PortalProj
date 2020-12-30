using System.Net.Mail;

namespace DevPortal.Framework.Abstract
{
    public interface ISmtpWrapper
    {
        bool SendMail(MailMessage mail);
    }
}