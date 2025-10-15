namespace DT.EmailService
{
    /// <summary>
    /// Представляет сообщение электронной почты для отправки.
    /// </summary>
    public class EmailMessage
    {
        /// <summary>
        /// Адрес получателя письма.
        /// </summary>
        public string To { get; set; } = null!;

        /// <summary>
        /// Тема письма (локализованная).
        /// </summary>
        public string Subject { get; set; } = null!;

        /// <summary>
        /// HTML-тело письма (локализованное).
        /// </summary>
        public string Body { get; set; } = null!;
    }
}
