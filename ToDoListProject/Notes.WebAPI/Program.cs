using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Notes.Application;
using Notes.Application.Common.Behaviors;
using Notes.Application.Common.Mapping;
using Notes.Application.Interfaces;
using Notes.Persistence;
using Notes.WebAPI;
using Notes.WebAPI.Middleware;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args); // Создание иструкций для приложения

// Регистрация зависимостей и сервисов
builder.Services.AddPersistence(builder.Configuration); // Подключаем свою базу данных заметок
builder.Services.AddApplication();
builder.Services.AddControllers();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    cfg.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
});

builder.Services.AddMediatR(cfg =>
{
    // регистрируем сервисы MediatR для нашей сборки
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    // Добавление Middleware для последующей обработки исключений в pipeline
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// Регистрируем валидаторы из сборки
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

// Добавляем схему для стандартной аутентификации и настраиваем доступ пользователей по JWT-токену
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Стандартная схема для проверки подлинности JWT-токенов
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Обработка исключения при попытке доступа к незащищенному ресурсу
})
.AddJwtBearer("Bearer", config =>
{
    config.Authority = "http://localhost:5026"; // какому серверу доверять
    config.Audience = "NotesWebAPI"; // Для кого этот доступ, имя заголовка токена доверенного сервера
    config.RequireHttpsMetadata = false;
});

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

// Настройка Swagger для кастомизации JSON-Документа и отображения его метаданных на главной странице Swagger UI
builder.Services.AddSwaggerGen(optionVer =>
{
    // Здесь больше НЕТ вызовов options.SwaggerDoc("v1.0", ...)
    // Класс ConfigureSwaggerOptions всё сделает сам!
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // Создание XML-файла и задание его имени исходя из имени сборки при помощи метода GetName()
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile); // Задание пути для хранения xml-файла, BaseDirectory используется для построения пути к XML-файлу
    optionVer.IncludeXmlComments(xmlPath);
}); // Если в комментах в указании версии в шапке SwaggerDoc написать пробел, то это сломает HTTP-маршрутизацию приложения
    // Без SwaggerDoc будут использоваться значения Swagger UI по умолчанию

// добавление версионирования API
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true; // Показывать ли поддерживаемые версии в заголовках ответов
    options.AssumeDefaultVersionWhenUnspecified = true; // Отключаем подстановку дефолтной версии, если версия не передана
    options.DefaultApiVersion = new ApiVersion(1, 0);
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // Формирование групп версий для Swagger (v1.0,v1.1) V-одна цифра в семантическом номере
    options.SubstituteApiVersionInUrl = true; // Автоматически подставляет параметр версии в сегменте URL на выбранную версию в Swagger
});

var app = builder.Build(); // Сборка приложения

using (var scope = app.Services.CreateScope()) // Добавление сервисов, которые имеют жизненный цикл scoped
{
    var service = scope.ServiceProvider; // Получение сервиса
    try
    {
        var context = service.GetRequiredService<NotesDbContext>(); // Подключаем работу с контекстом базы данных
        DbInitializer.Initialize(context); // инициализация сервиса
    }
    catch (Exception exception) // Обработчик ошибок
    {
        var logger = service.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, "Произошла ошибка при инициализации базы данных.");
    }
}

// Настройка middleware
app.UseCustomExceptionHandler();
//app.UseHttpsRedirection(); // Перенаправление с http на https

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Считывание всех эндпоинтов контроллеров, http-методов, DTO-объектов и формирование единого JSON-документа для генерации страницы
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwaggerUI(config =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            config.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json", // description.GroupName содержит систематизированное имя группы каждой версии API
                $"Notes API {description.GroupName.ToUpperInvariant()}"); // ToUpperInvariant() преобразует все символы строки в верхний регистр
        }
        //config.SwaggerEndpoint() по умолчанию генерирует json-документ по следующему пути: 
        config.RoutePrefix = String.Empty; // делает Swagger UI главной страницей при запуске
    });
}

app.UseRouting(); // Включаем использование роутинга
app.UseCors("AllowAll"); // CORS - это технология защиты от межсайтового доступа
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); // новый метод регистрации эндпоинтов
app.Run(); // запуск