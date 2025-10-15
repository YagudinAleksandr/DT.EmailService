using Microsoft.Extensions.Options;

namespace DT.EmailService
{
    /// <summary>
    /// Валидатор настроек SMTP.
    /// </summary>
    internal class SmtpOptionsValidator : IValidateOptions<SmtpOptions>
    {
        public ValidateOptionsResult Validate(string? name, SmtpOptions options)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(options.Host))
                errors.Add("Host не может быть пустым.");
            if (string.IsNullOrWhiteSpace(options.FromAddress))
                errors.Add("FromAddress не может быть пустым.");
            if (string.IsNullOrWhiteSpace(options.Username))
                errors.Add("Username не может быть пустым.");
            if (string.IsNullOrWhiteSpace(options.Password))
                errors.Add("Password не может быть пустым.");

            return errors.Any()
                ? ValidateOptionsResult.Fail(errors)
                : ValidateOptionsResult.Success;
        }
    }
}
