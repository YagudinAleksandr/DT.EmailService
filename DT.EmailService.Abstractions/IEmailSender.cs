using System.Threading;
using System.Threading.Tasks;

namespace DT.EmailService
{
    /// <summary>
    /// Сервис для отправки электронных писем.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Асинхронно отправляет письмо.
        /// </summary>
        /// <param name="message">Сообщение для отправки.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
    }
}
