using System.Net.Mail;

namespace DevPortal.Framework.Abstract
{
    public interface IMailMessageFactory
    {
        MailMessage CreateMailMessage(string senderEmailAddress, string senderName, string subject, string recipientAddresses, string mailBody);
    }
}