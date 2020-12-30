using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;

namespace DevPortal.Business.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        #region ctor

        readonly IMailInfoFactory mailInfoFactory;

        readonly IMailService mailService;

        public EmailSenderService(
            IMailService mailService,
            IMailInfoFactory mailInfoFactory)
        {
            this.mailService = mailService;
            this.mailInfoFactory = mailInfoFactory;
        }

        #endregion

        public bool SendForgotPasswordMail(string emailAddress, string resetUrl)
        {
            var mailInfo = mailInfoFactory.CreateForgotPasswordMailInfo(emailAddress, resetUrl);

            return mailService.SendMail(mailInfo);
        }
    }
}