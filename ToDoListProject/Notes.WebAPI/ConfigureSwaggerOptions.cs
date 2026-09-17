// Класс необходим для автоматического создания документации Swagger для версионирования API
using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Notes.WebAPI
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions> // За основу берем интерфейс IConfigureOptions для класса SwaggerGenOptions
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        // В этом методе мы пройдемся по всем версиям API и сформируем для каждой собственную документацию при помощи цикла foreach
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                var apiVersion = description.ApiVersion.ToString(); // Получаем текущую API версию из конфигурации в файле Program.cs и конвертируем ее в строку, чтобы сохранить название
                options.SwaggerDoc(description.GroupName,
                    new OpenApiInfo
                    {
                        Title = $"Notes API {apiVersion}", // название версии
                        Version = apiVersion,
                        Description = "Makes your Notes with enjoyment", // Описание версии
                        Contact = new OpenApiContact // Контактная информация разработчика
                        {
                            Name = "Artem",
                            Email = string.Empty,
                        },
                        License = new OpenApiLicense // Создание мини-сертификата подлинности
                        {
                            Name = "@2026. All rights reserved"
                        }
                    });
            }

            // Регистрируем схему авторизации
            var securityScheme = new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer",
                Name = "Authorization",
                Description = "Token Authorization"
            };
            options.AddSecurityDefinition("Bearer", securityScheme);

            // Зарегистрированную ранее схему авторизации нужно применять ко всем эндпоинтам API
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            });

            // Задает уникальный operationId для каждого эндпоинта в Swagger-документации
            options.CustomOperationIds(apiDescription =>
                apiDescription.TryGetMethodInfo(out MethodInfo MethodInfo)
                    ? MethodInfo.Name
                    : null);
        }
    }
}