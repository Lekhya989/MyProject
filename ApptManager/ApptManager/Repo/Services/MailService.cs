using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using ApptManager.Models;

namespace ApptManager.Repo.Services
{
        public class MailService : IMailService
        {
            private readonly MailSettings _mailSettings;
            public MailService(IOptions<MailSettings> mailSettings)
            {
                _mailSettings = mailSettings.Value;
            }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;

                var builder = new BodyBuilder { HtmlBody = mailRequest.Body };
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                #if DEBUG 

                smtp.ServerCertificateValidationCallback = (s, cert, chain, sslPolicyErrors) => true;

                #endif

                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                Console.WriteLine($"Email sent to {mailRequest.ToEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                throw; 
            }
        }
    }
}
