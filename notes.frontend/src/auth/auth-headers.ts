// Функция помещения токена в локальное хранилище браузера
export function setAuthHeader(token: string | null | undefined)
{
    localStorage.setItem('token' , token ? token : ''); // помещаем значение токена с заголовком 'token' в локальное хранлище браузера
}
export default setAuthHeader; // Добавляем по возможности экспорт в другие файлы в будущем