using System;
using System.Collections.Generic;
using System.Text;
using Notes.Application;
using Notes.Application.Interfaces;
using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands;
using Notes.Domain;
using System.Threading.Tasks;
using MediatR;
namespace Notes.Application.Notes.Commands.DeleteNote
{
    public class DeleteNoteCommandHandler: IRequestHandler <DeleteNoteCommand,Unit>
    {
        private readonly INotesDbContext _dbContext;
        public DeleteNoteCommandHandler(INotesDbContext dbContext) => _dbContext = dbContext;
        // Unit - Это тип, означающий пустой ответ
        public async Task<Unit> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            // Поиск заметки
            var entity = await _dbContext.Notes.FindAsync(new object[] { request.Id }, cancellationToken);
            // Если заметка не найдена
            if (entity == null || entity.UserId != request.UserId )
            {
                throw new NotFoundException(nameof(Notes),request.Id);
            }
            // Сохранение изменений
            _dbContext.Notes.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
