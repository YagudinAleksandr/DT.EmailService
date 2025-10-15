# DT.EmailService

Библиотека для отправки **локализованных email-писем** в .NET-приложениях с поддержкой внешних шаблонов, DI и безопасной загрузкой контента.

> ✨ Подходит для микросервисов, DDD-приложений и NuGet-пакетов.

## Сборки

Репозиторий содержит две независимые сборки:

| Сборка | Описание | NuGet |
|--------|---------|-------|
| [`DT.EmailService.Abstractions`](DT.EmailServiceAbstractions) | Интерфейсы и модели (без зависимостей) | `DT.EmailService.Abstractions` |
| [`DT.EmailService`](DT.EmailService) | Реализация отправки через SMTP (MailKit) | `DT.EmailService` |

## Особенности

- 🌐 **Полная локализация**: и тема, и тело письма
- 📂 **Внешние шаблоны**: HTML и `.subject.txt` хранятся вне библиотеки
- 🔒 **Безопасность**: защита от path traversal
- 🧪 **Тестируемость**: легко мокать через абстракции
- 📦 **DI-интеграция**: `AddEmailing()` в `IServiceCollection`
- 📤 **Готово к публикации**: поддержка GitHub Packages / NuGet

## Быстрый старт

1. Добавьте `nuget.config` (см. ниже)
2. Установите пакет:
```bash
   dotnet add package DT.EmailService
```

3. Настройте в `Program.cs`:
```csharp
builder.Services.AddEmailing(
    templates => templates.TemplatesDirectory = "EmailTemplates",
    smtp => builder.Configuration.GetSection("Smtp").Bind(smtp)
);
```

4. Использование
```csharp
var (subject, body) = _templateService.Render("Welcome", "ru", new() { ["Name"] = "Иван" });
await _emailSender.SendAsync(new() { To = "user@example.com", Subject = subject, Body = body });
```

5. Пример `appsettings.json:`
```json
{
  "Smtp": {
    "Host": "smtp.example.com",
    "Port": 587,
    "FromAddress": "noreply@myapp.com",
    "FromName": "Мой сервис",
    "Username": "user",
    "Password": "your-app-password"
  }
}
```

## Установка
Так как пакеты публикуются в GitHub Packages, создайте файл `nuget.config` в корне вашего проекта:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="github" value="https://nuget.pkg.github.com/your-username/index.json" />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
```
Замените `your-username` на имя владельца репозитория.

Затем установите:
```bash
dotnet add package YourCompany.Emailing
```

## Структура шаблонов
```txt
/EmailTemplates
  /ru
    Welcome.subject.txt   → "Добро пожаловать, {{Name}}!"
    Welcome.html
  /en
    Welcome.subject.txt   → "Welcome, {{Name}}!"
    Welcome.html
```
Переменные подставляются в формате {{Key}}.

