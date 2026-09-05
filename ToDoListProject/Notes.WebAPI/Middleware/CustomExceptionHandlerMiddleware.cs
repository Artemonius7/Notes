using System.Net;
using System.Text.Json; // для перевода ошибок в текст что то типа того
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Notes.Application.Common.Exceptions;
namespace Notes.WebAPI.Middleware
{
    
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next; // Создается один раз при старте приложения (Singleton)
        public CustomExceptionHandlerMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke (HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError; // Внутренняя ошибка сервера (дословно)
            var result = string.Empty;
            // Сериализация ошибок в тексте                       
            switch (ex)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(validationException.Errors);
                    break;
                case NotFoundException notFoundException:
                    code = HttpStatusCode.NotFound;
                    break;

            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            if (result == string.Empty)
            {
                result = JsonSerializer.Serialize(new { error = ex.Message });
            }
            return context.Response.WriteAsync(result);
        }
    }
}
