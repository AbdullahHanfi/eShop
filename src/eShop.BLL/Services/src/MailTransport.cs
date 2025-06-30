using eShop.BLL.Exceptions;
using eShop.BLL.Services.Abstraction;
using eShop.DAL.Utilities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;


namespace eShop.BLL.Services.src
{
    public class MailTransport : IMailTransport
    {
        private readonly EmailSettings _settings;
        public MailTransport(IOptions<EmailSettings> mailSettings)
        {
            _settings = mailSettings.Value;
        }

        public async Task SendAsync(MimeMessage mail)
        {
            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);

                await smtp.AuthenticateAsync(_settings.Email, _settings.Password);
                await smtp.SendAsync(mail);
            }
            catch (SmtpCommandException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new EmailSendingException("Failed to send email", ex);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
