using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace DT.EmailService
{
    /// <summary>
    /// Методы расширения для регистрации сервисов emailing через DI.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавляет сервисы отправки email в контейнер зависимостей.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="configureTemplates">Делегат для настройки путей к шаблонам.</param>
        /// <param name="configureSmtp">Делегат для настройки SMTP.</param>
        /// <returns>Та же коллекция сервисов для цепочки вызовов.</returns>
        public static IServiceCollection AddEmailing(
            this IServiceCollection services,
            Action<EmailTemplateOptions> configureTemplates,
            Action<SmtpOptions> configureSmtp)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configureTemplates == null)
                throw new ArgumentNullException(nameof(configureTemplates));
            if (configureSmtp == null)
                throw new ArgumentNullException(nameof(configureSmtp));

            services.Configure(configureTemplates);
            services.Configure(configureSmtp);
            services.TryAddSingleton<IEmailTemplateService, FileEmailTemplateService>();
            services.TryAddScoped<IEmailSender, SmtpEmailSender>();
            services.AddSingleton<IValidateOptions<SmtpOptions>, SmtpOptionsValidator>();

            return services;
        }
    }
}
