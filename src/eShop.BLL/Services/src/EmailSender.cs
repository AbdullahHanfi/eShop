using eShop.DAL.Utilities;
using eShop.BLL.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;

namespace eShop.BLL.Services.src
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;
        private readonly IMailTransport _mailTransport;

        public EmailSender(IOptions<EmailSettings> mailSettings, IMailTransport mailTransport)
        {
            _settings = mailSettings.Value;
            _mailTransport = mailTransport;
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

            await _mailTransport.SendAsync(email);
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

            await _mailTransport.SendAsync(email);
        }
    }
}
