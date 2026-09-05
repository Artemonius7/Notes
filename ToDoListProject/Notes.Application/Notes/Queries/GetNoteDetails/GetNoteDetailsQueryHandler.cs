using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Notes.Domain;
namespace Notes.Application.Notes.Queries.GetNoteDetails
{
    public class GetNoteDetailsQueryHandler:IRequestHandler<GetNoteDetailsQuery,NoteDetailsVm>
    {
        public readonly INotesDbContext _dbcontext;
        private readonly IMapper _mapper;
        public GetNoteDetailsQueryHandler(INotesDbContext dbContext, IMapper mapper) => (_dbcontext, _mapper) = (dbContext, mapper);
        public async Task<NoteDetailsVm> Handle(GetNoteDetailsQuery request,CancellationToken cancellationToken)
        {
            var entity = await _dbcontext.Notes.FirstOrDefaultAsync(note => note.Id == request.Id, cancellationToken);
            if (entity==null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Note),request.Id);
            }
            // Маппинг сущности из БД в наш объект ViewModel и возвращаем пользователю
            return _mapper.Map<NoteDetailsVm>(entity);
        }
    }
}
