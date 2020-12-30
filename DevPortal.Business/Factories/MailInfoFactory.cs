using AB.Framework.TextTemplates.Abstract;
using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.Framework.Model;
using DevPortal.Model;
using System.IO;

namespace DevPortal.Business.Factories
{
    public class MailInfoFactory : IMailInfoFactory
    {
        #region ctor

        readonly ISettings settings;

        readonly ITextTemplateService textTemplateService;

        readonly IMailInfoModelFactory mailInfoModelFactory;

        public MailInfoFactory(
            ISettings settings,
            ITextTemplateService textTemplateService,
            IMailInfoModelFactory mailInfoModelFactory)
        {
            this.settings = settings;
            this.textTemplateService = textTemplateService;
            this.mailInfoModelFactory = mailInfoModelFactory;
        }

        #endregion

        public MailInfo CreateForgotPasswordMailInfo(string emailAddress, string resetUrl)
        {
            var passwordResetModel = mailInfoModelFactory.CreatePasswordResetModel(resetUrl);
            var mailBody = GetForgotPasswordMailBody(passwordResetModel);
            var subject = Messages.ForgotPasswordMailSubject;

            return mailInfoModelFactory.CreateMailInfo(emailAddress, subject, mailBody);
        }

        string GetForgotPasswordMailBody(PasswordResetModel passwordResetModel)
        {
            var templateFileFullPath = GetTemplateFileFullPath(nameof(MailTemplateType.PasswordReset));

            return textTemplateService.GetTemplateContent(templateFileFullPath, passwordResetModel);
        }

        string GetTemplateFileFullPath(string templateFileName)
        {
            var mailTemplateRootPath = settings.MailTemplateRootPath;

            return Path.Combine(mailTemplateRootPath, $"{templateFileName}.hbs");
        }
    }
}