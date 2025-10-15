namespace DT.EmailService
{
    /// <summary>
    /// Настройки путей к внешним локализованным шаблонам писем.
    /// Шаблоны должны быть организованы по папкам языков:
    /// EmailTemplates/ru/OrderConfirmed.html
    /// EmailTemplates/ru/OrderConfirmed.subject.txt
    /// </summary>
    public class EmailTemplateOptions
    {
        /// <summary>
        /// Путь к корневой директории с шаблонами.
        /// Может быть относительным или абсолютным.
        /// </summary>
        public string TemplatesDirectory { get; set; } = "EmailTemplates";
    }
}
