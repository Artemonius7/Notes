using Notes.Persistence;
using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Xunit.Sdk;
using AutoMapper;
using Notes.Application.Common.Exceptions; // Помогает описывать проверки результатов в формате ShouldBe
using Notes.Application.Notes.Queries.GetNoteDetails;
using Shouldly;
namespace Notes.Tests.Notes.Queries
{
    [Collection("Query Definition")]
    public class GetNoteDetailsQueryHandlerTests
    {
        private readonly NotesDbContext Context;
        private readonly IMapper Mapper;
        public GetNoteDetailsQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            Mapper = fixture.mapper;
        }
        [Fact]
        public async Task GetNoteDetailsHandler_Success()
        {
            // Arrange
            var handler = new GetNoteDetailsQueryHandler(Context,Mapper);
            var UserTestId = NotesContextFactory.UserBId;
            var TestId = NotesContextFactory.NotesIdForUserB;
            // Act
            var result = await handler.Handle(
                new GetNoteDetailsQuery
                {
                    UserId = UserTestId,
                    Id = TestId
                }, CancellationToken.None);
            result.ShouldBeOfType<NoteDetailsVm>();
            result.Title.ShouldBe("Title2");
            result.CreationDate.ShouldBe(DateTime.Now, TimeSpan.FromSeconds(10));
        }
        [Fact]
        public async Task GetNoteDetailsHandler_OnFailureId()
        {
            // Arrange
            var handler = new GetNoteDetailsQueryHandler(Context, Mapper);
            // Act
            // Assery
            await Should.ThrowAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new GetNoteDetailsQuery
                    {
                        UserId = NotesContextFactory.UserBId,
                        Id = Guid.NewGuid()
                    }, CancellationToken.None);
            });
        }
        [Fact]
        public async Task GetNoteDetailsHandler_OnFailureUserId()
        {
            // Arrange
            var handler = new GetNoteDetailsQueryHandler(Context, Mapper);
            // Act
            // Assery
            await Should.ThrowAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new GetNoteDetailsQuery
                    {
                        UserId = Guid.NewGuid(),
                        Id = NotesContextFactory.NotesIdForUserB
                    }, CancellationToken.None);
            });
        }
    }
}
