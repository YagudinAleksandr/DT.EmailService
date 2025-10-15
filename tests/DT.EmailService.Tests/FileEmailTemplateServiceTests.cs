using Microsoft.Extensions.Options;

namespace DT.EmailService.Tests
{
    public class FileEmailTemplateServiceTests
    {
        private readonly string _tempDir;

        public FileEmailTemplateServiceTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
                Directory.Delete(_tempDir, true);
        }

        [Fact]
        public void Render_WithValidTemplate_ReturnsLocalizedContent()
        {
            var ruDir = Path.Combine(_tempDir, "ru");
            Directory.CreateDirectory(ruDir);
            File.WriteAllText(Path.Combine(ruDir, "Test.subject.txt"), "Привет, {{Name}}!");
            File.WriteAllText(Path.Combine(ruDir, "Test.html"), "<p>Привет, {{Name}}!</p>");

            var options = Options.Create(new EmailTemplateOptions { TemplatesDirectory = _tempDir });
            var service = new FileEmailTemplateService(options);

            var (subject, body) = service.Render("Test", "ru", new() { ["Name"] = "Анна" });

            Assert.Equal("Привет, Анна!", subject);
            Assert.Equal("<p>Привет, Анна!</p>", body);
        }

        [Fact]
        public void Render_WithMissingLanguage_FallsBackToEnglish()
        {
            var enDir = Path.Combine(_tempDir, "en");
            Directory.CreateDirectory(enDir);
            File.WriteAllText(Path.Combine(enDir, "Test.subject.txt"), "Hello, {{Name}}!");
            File.WriteAllText(Path.Combine(enDir, "Test.html"), "<p>Hello, {{Name}}!</p>");

            var options = Options.Create(new EmailTemplateOptions { TemplatesDirectory = _tempDir });
            var service = new FileEmailTemplateService(options);

            var (subject, body) = service.Render("Test", "fr", new() { ["Name"] = "John" });

            Assert.Equal("Hello, John!", subject);
            Assert.Equal("<p>Hello, John!</p>", body);
        }

        [Fact]
        public void Render_WithInvalidTemplateName_ThrowsArgumentException()
        {
            var options = Options.Create(new EmailTemplateOptions { TemplatesDirectory = _tempDir });
            var service = new FileEmailTemplateService(options);

            Assert.Throws<ArgumentException>(() => service.Render("../etc/passwd", "en", new()));
        }
    }
}