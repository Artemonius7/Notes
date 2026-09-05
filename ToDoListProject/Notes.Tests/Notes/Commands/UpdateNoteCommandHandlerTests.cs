using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Notes.Application;
using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Tests.Common;
namespace Notes.Tests.Notes.Commands
{
    public class UpdateNoteCommandHandlerTests: TestCommandBase
    {
        [Fact]
        public async Task UpdateNoteCommandHandler_Success()
        {
            // Arrange
            var handler = new UpdateNoteCommandHandler(Context);
            var noteTitle = "NoteTitle";
            var noteDetails = "NoteDetails";
            // Act
            await handler.Handle(
                new UpdateNoteCommand
                {
                    Id = NotesContextFactory.NotesIdForUpdate,
                    Title = noteTitle,
                    Details = noteDetails,
                    UserId = NotesContextFactory.UserAId
                }, CancellationToken.None);
            // Assert
            Assert.NotNull(
            await Context.Notes.SingleOrDefaultAsync(note =>
                note.Id == NotesContextFactory.NotesIdForUpdate && note.Details == noteDetails && note.Title == noteTitle));
        }
        [Fact]
        // Это негативные сценарии?
        public async Task UpdateNoteCommandHandler_FailureOnId()
        {
            // Arrange
            var handler = new UpdateNoteCommandHandler(Context);
            var noteTitle = "NoteTitle";
            var noteDetails = "NoteDetails";
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new UpdateNoteCommand
                    {
                        Id = Guid.NewGuid(),
                        Title = noteTitle,
                        Details = noteDetails,
                        UserId = NotesContextFactory.UserAId
                    }, 
                    CancellationToken.None));
        }
        [Fact]
        public async Task UpdateNoteCommandHandler_FailureOnUserId()
        {
            // Arrange
            var updateHandler = new UpdateNoteCommandHandler(Context);
            var createHandler = new CreateNoteCommandHandler(Context);
            var noteTitle = "NoteTitle";
            var noteDetails = "NoteDetails";
            var NoteID = await createHandler.Handle(
                new CreateNoteCommand
                {
                    UserId = NotesContextFactory.UserAId,
                    Title = noteTitle,
                    Details = noteDetails
                }, CancellationToken.None);
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await updateHandler.Handle(
                    new UpdateNoteCommand
                    {
                        Id = NoteID,
                        UserId = NotesContextFactory.UserBId,
                        Title = noteTitle,
                        Details = noteDetails
                    }, CancellationToken.None));
                
            
        }
    }
}
