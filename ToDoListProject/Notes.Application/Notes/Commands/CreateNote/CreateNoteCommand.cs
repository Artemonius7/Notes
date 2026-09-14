using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
namespace Notes.Application.Notes.Commands.CreateNote
{
    // В классе описывается все, что необходимо для создания заметки
    public class CreateNoteCommand : IRequest<Guid> // помечает результат определенной команды и вернет результат определенного типа
    {
        public Guid UserId { get; set; } // Возврат ид пользователя
        public string Title { get; set; } = string.Empty; // Возврат названия заметки
        public string Details { get; set; } = string.Empty; // Возврат деталей операции
    }
}
