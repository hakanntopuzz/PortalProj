using DevPortal.Framework.Model;
using DevPortal.Model;

namespace DevPortal.Business.Abstract
{
    public interface IMailInfoModelFactory
    {
        MailInfo CreateMailInfo(string recipient, string subject, string mailBody);

        PasswordResetModel CreatePasswordResetModel(string resetUrl);
    }
}