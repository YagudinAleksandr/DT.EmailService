using System.Collections.Generic;

namespace DT.EmailService
{
    /// <summary>
    /// Сервис для рендеринга локализованных шаблонов писем.
    /// </summary>
    public interface IEmailTemplateService
    {
        /// <summary>
        /// Рендерит шаблон письма для указанного языка.
        /// </summary>
        /// <param name="templateName">Имя шаблона без расширения (например, "OrderConfirmed").</param>
        /// <param name="language">Код языка (например, "ru", "en").</param>
        /// <param name="model">Словарь переменных для подстановки в шаблон (в формате {{Key}}).</param>
        /// <returns>Кортеж из темы и HTML-тела письма.</returns>
        (string Subject, string Body) Render(string templateName, string language, Dictionary<string, string> model);
    }
}
