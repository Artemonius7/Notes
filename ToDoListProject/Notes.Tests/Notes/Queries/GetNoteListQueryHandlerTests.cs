using AutoMapper;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.Persistence;
using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Shouldly;
using Xunit.Sdk;
using Notes.Application.Common.Exceptions; // Помогает описывать проверки результатов в формате ShouldBe
namespace Notes.Tests.Notes.Queries
{
    [Collection("Query Definition")]

    public class GetNoteListQueryHandlerTests
    {
        private readonly NotesDbContext Context;
        private readonly IMapper Mapper;
        public GetNoteListQueryHandlerTests(QueryTestFixture queryTestFixture) // Конструктор
        {
            Context = queryTestFixture.Context;
            Mapper = queryTestFixture.mapper;
        }
        [Fact]
        public async Task GetNoteListHandler_Success()
        {
            // Arrange
            var handler = new GetNoteListQueryHandler(Context,Mapper);
            // Act
            var result = await handler.Handle(
                new GetNoteListQuery
                {
                    UserId = NotesContextFactory.UserBId
                }, CancellationToken.None);
            // Assert
            result.ShouldBeOfType<NoteListVm>(); // Проверяет, что возвращаемый тип объекта имеет тип View-Модели (NotesViewModel)
            result.Notes.Count.ShouldBe(2); // Проверяет, что у пользователя нашлось ровно 2 заметки

        }
        [Fact]
        public async Task GetNoteListHandler_OnFailureUserId()
        {
            var handler = new GetNoteListQueryHandler(Context, Mapper);
            var invalidUserId = Guid.NewGuid(); // Генерируем случайный ID, которого точно нет в базе
            var result = await handler.Handle(
                new GetNoteListQuery
                {
                    UserId = invalidUserId
                },CancellationToken.None);
            result.ShouldBeOfType<NoteListVm>();
            result.Notes.ShouldBeEmpty(); // Проверяем что список заметок пуст
        }
        

    }
}
