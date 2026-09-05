using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Notes.Domain;
using Notes.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Sqlite;
using Notes.Persistence.EntityTypeConfiguration;
using Microsoft.Extensions.Options;

namespace Notes.Persistence
{
    public class NotesDbContext: DbContext, INotesDbContext
    {
        public DbSet<Note> Notes { get; set; }
        public NotesDbContext(DbContextOptions<NotesDbContext> options) :
            base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new NoteConfiguration());
            
            base.OnModelCreating(builder);
        }
    }
}
