using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Notes.Application;
using Notes.Application.Common.Mapping;
using Notes.Application.Interfaces;
using Notes.Persistence;
using Xunit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Query;
using Notes.Tests.Notes.Queries;
namespace Notes.Tests.Common
{
    public class QueryTestFixture: TestCommandBase
    {
        public NotesDbContext Context { get; set; }
        public IMapper mapper { get; set; }
        public ILoggerFactory loggerFactory { get; set; } // В моей версии AutoMapper это требуется для записей маппинга в журнал событий
        public QueryTestFixture()
        {
            // Context = NotesContextFactory.Create(); Не требуется, так как наш класс наследуется от TestCommandBase, где уже создается бд

            // 1. Создаем конфигурационное выражение
            var configExpression = new MapperConfigurationExpression();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
            },null); // null - Логирование конфигурации не требуется
            // 2. Создаем Mapper, передавая конфигурацию в конструктор Mapper
            mapper = configuration.CreateMapper();

        }
        public void Dispose()
        {
            NotesContextFactory.Destroy(Context);
            base.Dispose();
        }
        [CollectionDefinition("Query Definition")]
        public class QueryCollection: ICollectionFixture<QueryTestFixture> { }
    }
}
