using eShop.DAL.Utilities;
using eShop.BLL.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;

namespace eShop.BLL.Services.src
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;
        public EmailSender(IOptions<EmailSettings> mailSettings)
        {
            _settings = mailSettings.Value;
        }
        public async Task SendEmailAsync(string mailto, string subject, string body)
        {
            var email = new MimeMessage
            {
                To = { MailboxAddress.Parse(mailto) },
                Subject = subject
            };
            email.From.Add(new MailboxAddress(_settings.DisplayName, _settings.Email));

            var builder = new BodyBuilder();

            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_settings.Email, _settings.Password);
            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }

        public async Task SendEmailAsync(string mailto, string subject, string body, List<IFormFile> files)
        {
            var email = new MimeMessage
            {
                To = { MailboxAddress.Parse(mailto) },
                Subject = subject
            };
            email.From.Add(new MailboxAddress(_settings.DisplayName, _settings.Email));

            var builder = new BodyBuilder();
            if (files != null)
            {
                byte[] filebytes;
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        filebytes = ms.ToArray();
                        builder.Attachments.Add(file.FileName, filebytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_settings.Email, _settings.Password);
            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}
