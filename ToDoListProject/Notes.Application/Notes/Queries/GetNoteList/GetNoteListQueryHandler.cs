using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Notes.Domain;
using Notes.Application.Interfaces;
using Notes.Application.Common.Exceptions;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection.Metadata.Ecma335;
namespace Notes.Application.Notes.Queries.GetNoteList
{
    public class GetNoteListQueryHandler:IRequestHandler<GetNoteListQuery,NoteListVm>
    {
        private readonly INotesDbContext _dbcontext;
        private readonly IMapper _mapper;
        public GetNoteListQueryHandler(INotesDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }
        public async Task<NoteListVm> Handle(GetNoteListQuery request,CancellationToken cancellationToken)
        {
            var notesQuery = await _dbcontext.Notes.Where(note => note.UserId == request.UserId)
                .ProjectTo<NoteLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            return new NoteListVm { Notes = notesQuery };
        }
        
    }
}
