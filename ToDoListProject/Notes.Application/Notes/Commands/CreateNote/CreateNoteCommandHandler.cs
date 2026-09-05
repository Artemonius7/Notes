using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Notes.Application;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using MediatR;
using Notes.Domain;
using Notes.Application.Interfaces;
namespace Notes.Application.Notes.Commands.CreateNote
{
    // Логика создания заметки
    public class CreateNoteCommandHandler: IRequestHandler <CreateNoteCommand,Guid> // Обработчик запроса создания заметки // Тип запроса / Тип ответа 
    {
        private readonly INotesDbContext _dbContext; // Сохранение изменений в базу
        public CreateNoteCommandHandler(INotesDbContext dbContext) => _dbContext = dbContext; // Внедрение зависимостей на контекст базы данных
        
        // Логика создания заметки
        public async Task<Guid> Handle(CreateNoteCommand request, CancellationToken cancellationToken) // cancellation token - это токен отмены
        {
            var note = new Note
            {
                UserId = request.UserId,
                Title = request.Title,
                Details = request.Details,
                Id = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                EditDate = null
            };
            await _dbContext.Notes.AddAsync(note, cancellationToken); // Добавление заметки в контекст БД
            await _dbContext.SaveChangesAsync(cancellationToken); // Сохранение изменений в Базу Данных
            return note.Id;
        }
    }
}
