using DevPortal.Framework.Abstract;
using DevPortal.Framework.Model;

namespace DevPortal.Framework.Services
{
    public class MailService : IMailService
    {
        #region ctor

        readonly ISmtpWrapper smtpWrapper;

        readonly ISettings settings;

        readonly IMailMessageFactory mailMessageFactory;

        public MailService(ISmtpWrapper smtpWrapper,
            ISettings settings,
            IMailMessageFactory mailMessageFactory)
        {
            this.smtpWrapper = smtpWrapper;
            this.settings = settings;
            this.mailMessageFactory = mailMessageFactory;
        }

        #endregion

        public bool SendMail(MailInfo mailInfo)
        {
            string senderEmailAddress = settings.SenderEmailAddress;
            string senderName = settings.DefaultMailSenderName;

            var mail = mailMessageFactory.CreateMailMessage(
                senderEmailAddress,
                senderName,
                mailInfo.Subject,
                mailInfo.Recipient,
                mailInfo.MailBody);

            return smtpWrapper.SendMail(mail);
        }
    }
}