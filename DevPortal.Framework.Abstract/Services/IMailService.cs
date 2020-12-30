using DevPortal.Framework.Model;

namespace DevPortal.Framework.Abstract
{
    public interface IMailService
    {
        bool SendMail(MailInfo mailInfo);
    }
}