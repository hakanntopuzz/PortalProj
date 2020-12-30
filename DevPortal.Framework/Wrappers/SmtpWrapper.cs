using AB.Framework.Logger.Nlog.Abstract;
using DevPortal.Framework.Abstract;
using System;
using System.Net.Mail;

namespace DevPortal.Framework.Wrappers
{
    public class SmtpWrapper : ISmtpWrapper
    {
        #region ctor

        readonly ISettings settings;

        readonly ILoggingService logger;

        public SmtpWrapper(ISettings settings,
            ILoggingService logger)
        {
            this.settings = settings;
            this.logger = logger;
        }

        #endregion

        public bool SendMail(MailMessage mail)
        {
            var smtpClient = CreateSmtpClient();

            try
            {
                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(SetMethodNameForLogMessage(nameof(SendMail)),
                    $"E-posta gönderilirken hata. Gönderen: {mail.From.Address}",
                    ex);
            }
            finally
            {
                mail.Dispose();
            }

            return false;
        }

        /// <summary>
        /// Format: SınıfAdı.MetotAdı
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private string SetMethodNameForLogMessage(string methodName)
        {
            return $"{nameof(SmtpWrapper)}.{methodName}";
        }

        private SmtpClient CreateSmtpClient()
        {
            string smtpAddress = settings.SmtpAddress;
            string smtpUserName = settings.SmtpUsername;
            string smtpPassword = settings.SmtpPassword;
            int portNumber = settings.SmtpPortNumber;

            SmtpClient smtpClient = new SmtpClient(smtpAddress, portNumber)
            {
                //SSL kullansın mı?
                EnableSsl = settings.SmtpEnableSSL,

                UseDefaultCredentials = false,

                //kullanıcı bilgilerini ata
                Credentials = new System.Net.NetworkCredential(smtpUserName, smtpPassword)
            };

            return smtpClient;
        }
    }
}