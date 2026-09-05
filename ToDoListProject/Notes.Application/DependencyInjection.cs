using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Notes.Application.Common.Mapping;
using FluentValidation;
using Notes.Application.Common.Behaviors;
namespace Notes.Application
{
    // Внедрение зависимостей и их регистрация в контейнер приложения
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication (this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(cfg=>cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly())));
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }
    }
}
