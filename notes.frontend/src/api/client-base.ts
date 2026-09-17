export class ClientBase{ // Класс, от которого будет наследовать сгенерированный NSwag-клиент
    protected transformOptions(options: RequestInit)
    {
        const token = localStorage.getItem('token'); // Обращаемся к локальному хранилищу браузера и пытаемся достать оттуда сохраненный ранее токен доступа
        options.headers = {   // Расширение текущих заголовков запроса и добавляем главный заголовок безопасности
            ...options.headers,
            Authorization: 'Bearer ' + token
        }
        return Promise.resolve(options);
    }
}
// Благодаря этому к каждому запросу будет применен валидный токен доступа (безопасности)