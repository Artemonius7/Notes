using AutoMapper;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Notes.Application;
using Notes.Application.Common.Mapping;
using Notes.Application.Interfaces;
using Notes.Persistence;
using Notes.WebAPI.Middleware;
using System;
using System.Reflection;
using Notes.Application.Common.Behaviors;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
var builder = WebApplication.CreateBuilder(args); //  Создание иструкций для приложения
// Регистрация зависимостей и сервисов
builder.Services.AddPersistence(builder.Configuration); // Подключаем свою базу данных заметок
builder.Services.AddApplication();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    cfg.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
}); // Добавление automapper для работы с контекстом бд и сущностями из domain
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
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Стандартная схема для проверки подлинности  JWT-токенов
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Обработка исключения при попытке доступа к незащищенному ресурсу
})
    .AddJwtBearer("Bearer", config =>
    {
            config.Authority = "https://localhost:7020"; // какому серверу доверять
            config.Audience = "NotesWebAPI"; // Для кого этот доступ, имя заголовка токена доверенного сервера
            config.RequireHttpsMetadata = false; 
    });
var app = builder.Build();

using (var scope = app.Services.CreateScope()) // Добавление сервисов, которые имеют жиненный цикл scoped
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
// Сборка приложения

// Настройка middleware
app.UseCustomExceptionHandler();
app.UseHttpsRedirection(); // Перенаправление с http на https
app.UseRouting(); // Включаем использование роутинга
app.UseCors("AllowAll"); // CORS - это технология защиты от межсайтового доступа
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); // новый метод регистрации эндпоинтов
app.MapGet("/", () => "Hello World! How Are You"); // вывод на экран
app.Run(); // запуск




