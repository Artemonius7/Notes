// Тестирование команды создания заметки
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Tests.Common;
using Xunit;
namespace Notes.Tests.Notes.Commands
{
    public class CreateNoteCommandHandlerTests: TestCommandBase
    {
        [Fact]
        public async Task CreateNoteCommandHandler_Success()
        {
            // 1. Arrange - подготовка данных к тестированию
            var handler = new CreateNoteCommandHandler(Context);
            var noteName = "note name";
            var noteDetails = "note details";
            // 2. Act - начало обработки и тестирования
            var noteId = await handler.Handle(
                new CreateNoteCommand
                {
                    Title = noteName,
                    Details = noteDetails,
                    UserId = NotesContextFactory.UserAId
                },
                CancellationToken.None);
            // 3. Assert - проверка результата
            Assert.NotNull(
                await Context.Notes.SingleOrDefaultAsync(note =>
                note.Id == noteId && note.Title == noteName && note.Details == noteDetails));
        }
    }
}
