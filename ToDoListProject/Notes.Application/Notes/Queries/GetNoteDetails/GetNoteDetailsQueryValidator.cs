using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notes.Application.Notes.Queries.GetNoteDetails
{
    public class GetNoteDetailsQueryValidator: AbstractValidator<GetNoteDetailsQuery>
    {
        public GetNoteDetailsQueryValidator() 
        {
            RuleFor(getNoteDetailsQuery => getNoteDetailsQuery.UserId).NotEqual(Guid.Empty);
            RuleFor(getNoteDetailsQuery => getNoteDetailsQuery.Id).NotEqual(Guid.Empty);
        }
    }
}
