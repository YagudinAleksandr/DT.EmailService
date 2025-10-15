namespace DT.EmailService
{
    /// <summary>
    /// Настройки SMTP-сервера для отправки электронной почты.
    /// </summary>
    public class SmtpOptions
    {
        /// <summary>
        /// Хост SMTP-сервера (например, "smtp.gmail.com").
        /// </summary>
        public string Host { get; set; } = null!;

        /// <summary>
        /// Порт SMTP-сервера (обычно 587 или 465).
        /// </summary>
        public int Port { get; set; } = 587;

        /// <summary>
        /// Адрес отправителя (например, "noreply@myapp.com").
        /// </summary>
        public string FromAddress { get; set; } = null!;

        /// <summary>
        /// Имя отправителя (отображается в письме).
        /// </summary>
        public string FromName { get; set; } = "Сервис";

        /// <summary>
        /// Логин для аутентификации на SMTP-сервере.
        /// </summary>
        public string Username { get; set; } = null!;

        /// <summary>
        /// Пароль или App Password для аутентификации.
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// Использовать ли шифрование STARTTLS.
        /// </summary>
        public bool UseStartTls { get; set; } = true;
    }
}
