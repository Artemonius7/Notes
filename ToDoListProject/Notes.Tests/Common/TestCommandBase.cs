// Этот Абстрактный класс нужен для того, чтобы проверять команды, связанные с контекстом бд
// Здесь мы создаем Контекст бд из нескольких моделей, и после работы будем вызвать метод для уничтожения данных контекста и бд из ОЗУ
// при помощи интерфейса IDisposable
using System;
using System.Collections.Generic;
using System.Text;
using Notes.Persistence;
using Notes.Tests;
using Notes.Tests.Common;
namespace Notes.Tests.Common
{
    public abstract class TestCommandBase: IDisposable
    {
        protected readonly NotesDbContext Context;
        public TestCommandBase()
        {
            Context = NotesContextFactory.Create();
        }
        public void Dispose() 
        {
            NotesContextFactory.Destroy(Context);
        }

    }
}
