using System;
using System.Collections.Generic;
using System.Text;
using Notes.Domain;
using Notes.Persistence.EntityTypeConfiguration;
using Microsoft.EntityFrameworkCore.InMemory;
using static Notes.Persistence.NotesDbContext;
using Microsoft.EntityFrameworkCore;
using Notes.Persistence;
namespace Notes.Tests.Common
{
    internal class NotesContextFactory
    {
        public static Guid UserAId = new Guid();
        public static Guid UserBId = new Guid();
        public static Guid NotesIdForDelete = new Guid();
        public static Guid NotesIdForUpdate = new Guid();
        public static NotesDbContext Create()
        {
            var options = new DbContextOptionsBuilder<NotesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Инициализация базы данных по уникальному имени в ОЗУ
                .Options;
            var context = new NotesDbContext(options); // Создание контекста заметок
            context.Database.EnsureCreated(); // Создание базы данных
            context.Notes.AddRange( // Создание тестовых данных
                new Note
                {
                    CreationDate = DateTime.Now,
                    Details = "Details1",
                    EditDate = null,
                    Id = Guid.NewGuid(),
                    Title = "Title1",
                    UserId = UserAId
                },
                new Note
                {
                    CreationDate = DateTime.Now,
                    Details = "Details2",
                    EditDate = null,
                    Id = Guid.NewGuid(),
                    Title = "Title2",
                    UserId = UserBId
                },
                new Note
                {
                    CreationDate = DateTime.Now,
                    Details = "Details3",
                    EditDate = null,
                    Id = Guid.NewGuid(),
                    Title = "Title3",
                    UserId = UserAId
                },
                new Note
                {
                    CreationDate = DateTime.Now,
                    Details = "Details4",
                    EditDate = null,
                    Id = Guid.NewGuid(),
                    Title = "Title4",
                    UserId = UserBId
                }
                );
            context.SaveChanges(); // Сохранение изменений
            return context;
            
        }
        public static void Destroy (NotesDbContext context) // Метод для удаления базы данных из ОЗУ
        {
            context.Database.EnsureDeleted(); // Удаляем БД из ОЗУ
            context.Dispose(); // Полностью очищаем все ресурсы и закрываем контекст БД
            // Интерфейс IDisposable гарантирует, что данные будут полностью высвобождены из памяти после каждого теста
        }

    }
}
