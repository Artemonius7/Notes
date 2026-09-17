import React, {FC,ReactElement,useRef,useEffect,useState} from 'react'; // Импорт из React
import { CreateNoteDto, Client, NoteLookupDto} from '../api/api'; // импорт из NSwag
import { FormControl } from 'react-bootstrap'; // импорт из Bootstrap

const apiClient = new Client('http://localhost:5237'); // порт бэкенда для обработки запросов

export const NoteList: FC<{}> =(): ReactElement => { // FC - функциональный компонент React
    const textInput = useRef<HTMLInputElement>(null);  // Работа с полем ввода
    const [notes, setNotes] = useState<NoteLookupDto[] | undefined>(undefined); // здесь хранится массив заметок; useState хранит состояние компонента. Он сле-
    // дит за списком заметок и обновляет экран, когда список меняется
    // Функция загрузки и чтения заметок из back-end'a
    const getNotes = async()=>{
        try{
            const noteListVm = await apiClient.getAll();
            setNotes(noteListVm.notes);
        } catch (error) { // Перехват исключения
            console.error('Ошибка при получении заметок:',error);
        }
    };
    // выполнение побочных эффектов на странице браузера, в нашем случае, получение спсика заметок
    useEffect(() =>{
        getNotes();
    },[]);
    // Создание заметки (Новая функция)
    const createNote = async (note:CreateNoteDto) =>{
        try{
            await apiClient.create(note);
            console.log('Заметка создана!');
        } catch (error) {
            console.error('Ошибка при создании заметки:',error);
        }
    };

    // Обработчик нажатия клавиши Enter
    const handleKeyPress = async (event: React.KeyboardEvent<HTMLInputElement>)=>{
        if (event.key === 'Enter'){
            const value = event.currentTarget.value;
            if (!value.trim()) return; // Если строка пустая или из пробелов — ничего не делаем
            const note: CreateNoteDto = { 
                title: value,
                details:"" // Это обязательное поле в соответствии со спецификацией моего NSwag
            };
            await createNote(note); // Отправляем запрос на сервер
            event.currentTarget.value=''; // Очищаем поля ввода в браузере
            await getNotes(); // Заново запрашиваем список, чтобы увидеть свежую заметку
        }
    };
    return (
        <div>
            Notes
            <div>
                <FormControl ref={textInput} onKeyDown={handleKeyPress} />
            </div>
            <div>
                {notes?.map((note: NoteLookupDto, index: number) => (
                    <div key={note.id || index}>{note.title}</div>
                ))}
            </div>
        </div>
    );
};
export default NoteList;
