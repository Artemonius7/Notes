using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Notes.Application;
using Notes.Application.Common.Mapping;
using Notes.Application.Interfaces;
using Notes.Persistence;
using Xunit;
namespace Notes.Tests.Common
{
    public class QueryTestFixture: TestCommandBase
    {
        public NotesDbContext Context { get; set; }
        public IMapper mapper { get; set; }
        public QueryTestFixture()
        {
            Context = NotesContextFactory.Create();

            // 1. Создаем конфигурационное выражение
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));

            // 2. Создаем Mapper, передавая конфигурацию в конструктор Mapper
           // mapper = new Mapper(configExpression);
        }
        public void Dispose()
        {
            NotesContextFactory.Destroy(Context);
            base.Dispose();
        }
    }
}
