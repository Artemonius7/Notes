using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notes.Application.Notes.Queries.GetNoteList
{
    public class GetNoteListQueryValidator: AbstractValidator<GetNoteListQuery>
    {
        public GetNoteListQueryValidator() 
        {
            RuleFor(note=>note.UserId).NotEqual(Guid.Empty);
        }
    }
}
