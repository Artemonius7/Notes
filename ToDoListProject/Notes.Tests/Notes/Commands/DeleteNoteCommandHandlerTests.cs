using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Notes.Application;
using Xunit;
using Notes.Application.Notes.Commands.DeleteNote;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Interfaces;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens.Experimental;
using Notes.Application.Common.Exceptions;
namespace Notes.Tests.Notes.Commands
{
    public class DeleteNoteCommandHandlerTests: TestCommandBase
    {
        [Fact]
        public async Task DeleteNoteCommandHandler_Success()
        {
            var handler = new DeleteNoteCommandHandler(Context);
            var UserId = await handler.Handle(
                new DeleteNoteCommand
                {
                    Id = NotesContextFactory.NotesIdForDelete,
                    UserId = NotesContextFactory.UserAId,
                }, CancellationToken.None);
                 Assert.Null(
                     await Context.Notes.SingleOrDefaultAsync(note =>
                     note.Id == NotesContextFactory.NotesIdForDelete));
        }
        // обработка исключения
        [Fact]
        public async Task DeleteNoteCommandHandler_FailOnWrongId()
        {
            var handler = new DeleteNoteCommandHandler(Context);
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new DeleteNoteCommand
                    {
                        Id = Guid.NewGuid(),
                        UserId = NotesContextFactory.UserAId
                    }, CancellationToken.None));
        }
        [Fact]
        public async Task DeleteNoteCommandHandler_FailOnWrongUserId()
        {
            var deleteHandler = new DeleteNoteCommandHandler(Context);
            var createHandler = new CreateNoteCommandHandler(Context);
            var noteID = await createHandler.Handle(
                new CreateNoteCommand
                {
                    Title = "NoteTitle",
                    UserId = NotesContextFactory.UserAId
                }, CancellationToken.None);
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await deleteHandler.Handle(
                    new DeleteNoteCommand
                    {
                        Id= noteID, 
                        UserId = NotesContextFactory.UserBId
                    }, CancellationToken.None));

        }
    }
}
