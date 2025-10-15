using Microsoft.Extensions.Options;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace DT.EmailService
{
    /// <summary>
    /// Реализация <see cref="IEmailTemplateService"/>, использующая внешние файлы шаблонов.
    /// </summary>
    public class FileEmailTemplateService : IEmailTemplateService
    {
        private readonly string _templatesRoot;
        private static readonly char[] InvalidTemplateChars = { '/', '\\', ':', '*', '?', '"', '<', '>', '|' };

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="FileEmailTemplateService"/>.
        /// </summary>
        /// <param name="options">Настройки путей к шаблонам.</param>
        public FileEmailTemplateService(IOptions<EmailTemplateOptions> options)
        {
            if (options?.Value == null)
                throw new ArgumentNullException(nameof(options));

            var dir = options.Value.TemplatesDirectory;
            if (string.IsNullOrWhiteSpace(dir))
                throw new ArgumentException("TemplatesDirectory не может быть пустым", nameof(dir));

            _templatesRoot = Path.GetFullPath(dir);
            if (!Directory.Exists(_templatesRoot))
                throw new DirectoryNotFoundException($"Директория шаблонов не найдена: {_templatesRoot}");
        }

        /// <inheritdoc/>
        public (string Subject, string Body) Render(string templateName, string language, Dictionary<string, string> model)
        {
            if (string.IsNullOrWhiteSpace(templateName))
                throw new ArgumentException("Имя шаблона обязательно.", nameof(templateName));

            if (templateName.IndexOfAny(InvalidTemplateChars) >= 0)
                throw new ArgumentException("Имя шаблона содержит недопустимые символы.", nameof(templateName));

            var normalizedLang = NormalizeLanguage(language);
            var (subject, body) = TryLoadTemplate(templateName, normalizedLang);

            if (subject == null || body == null && normalizedLang != "en")
            {
                (subject, body) = TryLoadTemplate(templateName, "en");
            }

            if (subject == null || body == null)
            {
                throw new FileNotFoundException(
                    $"Шаблон '{templateName}' не найден для языка '{normalizedLang}' и fallback 'en'. " +
                    $"Ожидались файлы в: {_templatesRoot}/{normalizedLang}/ и {_templatesRoot}/en/");
            }

            return (ReplacePlaceholders(subject, model), ReplacePlaceholders(body, model));
        }

        private (string? Subject, string? Body) TryLoadTemplate(string templateName, string language)
        {
            var langDir = Path.Combine(_templatesRoot, language);
            if (!Directory.Exists(langDir))
                return (null, null);

            var subjectPath = Path.Combine(langDir, $"{templateName}.subject.txt");
            var bodyPath = Path.Combine(langDir, $"{templateName}.html");

            if (!IsPathUnderRoot(subjectPath) || !IsPathUnderRoot(bodyPath))
                throw new SecurityException("Обнаружена попытка выхода за пределы директории шаблонов.");

            if (!File.Exists(subjectPath) || !File.Exists(bodyPath))
                return (null, null);

            try
            {
                var subject = File.ReadAllText(subjectPath, Encoding.UTF8).Trim();
                var body = File.ReadAllText(bodyPath, Encoding.UTF8);
                return (subject, body);
            }
            catch
            {
                return (null, null);
            }
        }

        private bool IsPathUnderRoot(string filePath)
        {
            var full = Path.GetFullPath(filePath);
            return full.StartsWith(_templatesRoot, StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizeLanguage(string? lang)
        {
            if (string.IsNullOrWhiteSpace(lang))
                return "en";

            var primary = lang.Split('-', StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant();
            return primary is "ru" or "en" ? primary : "en";
        }

        private static string ReplacePlaceholders(string content, Dictionary<string, string> model)
        {
            if (string.IsNullOrEmpty(content) || model == null || model.Count == 0)
                return content;

            return Regex.Replace(content, @"\{\{(\w+)\}\}", match =>
            {
                var key = match.Groups[1].Value;
                return model.TryGetValue(key, out var value) ? (value ?? string.Empty) : match.Value;
            });
        }
    }
}
