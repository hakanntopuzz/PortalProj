using DevPortal.Framework.Abstract;
using System.Net.Mail;

namespace DevPortal.Framework.Factories
{
    public class MailMessageFactory : IMailMessageFactory
    {
        public MailMessage CreateMailMessage(string senderEmailAddress, string senderName, string subject, string recipientAddresses, string mailBody)
        {
            //gönderici bilgilerini ata
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(senderEmailAddress, senderName),
                Subject = subject,
                Body = mailBody,
                IsBodyHtml = true
            };

            //eposta alıcılarını ata
            SetMailRecipients(recipientAddresses, mail);

            return mail;
        }

        /// <summary>
        /// Ayırıcı karakter ile birleştirilmiş e-posta adresi listesini MailMessage sınıfı alıcı özelliklerine atar.
        /// </summary>
        /// <param name="recipientAddresses"></param>
        /// <param name="mail"></param>
        void SetMailRecipients(string recipientAddresses, MailMessage mail)
        {
            const char separator = ';';

            if (string.IsNullOrEmpty(recipientAddresses))
            {
                return;
            }

            //eposta alıcı listesi oluştur
            string[] recipientMailAddressList = recipientAddresses.Trim(separator).Split(separator);

            mail.To.Add(recipientMailAddressList[0]);

            for (int i = 1; i < recipientMailAddressList.Length; i++)
            {
                mail.Bcc.Add(recipientMailAddressList[i]);
            }
        }
    }
}