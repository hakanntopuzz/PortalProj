using DevPortal.Business.Abstract;
using DevPortal.Framework.Model;
using DevPortal.Model;

namespace DevPortal.Business.Factories
{
    public class MailInfoModelFactory : IMailInfoModelFactory
    {
        public MailInfo CreateMailInfo(string recipient, string subject, string mailBody)
        {
            return new MailInfo
            {
                Recipient = recipient,
                Subject = subject,
                MailBody = mailBody
            };
        }

        public PasswordResetModel CreatePasswordResetModel(string resetUrl)
        {
            return new PasswordResetModel
            {
                ResetUrl = resetUrl
            };
        }
    }
}