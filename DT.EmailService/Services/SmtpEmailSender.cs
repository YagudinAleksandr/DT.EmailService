using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DT.EmailService
{
    /// <summary>
    /// Реализация <see cref="IEmailSender"/>, использующая SMTP через MailKit.
    /// </summary>
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpOptions _options;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="SmtpEmailSender"/>.
        /// </summary>
        /// <param name="options">Настройки SMTP.</param>
        public SmtpEmailSender(IOptions<SmtpOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc/>
        public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_options.FromName, _options.FromAddress));
            email.To.Add(MailboxAddress.Parse(message.To));
            email.Subject = message.Subject;
            email.Body = new TextPart("html") { Text = message.Body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_options.Host, _options.Port, cancellationToken: cancellationToken);
            await client.AuthenticateAsync(_options.Username, _options.Password, cancellationToken);
            await client.SendAsync(email, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}
