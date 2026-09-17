// Компонент для настройки работы с сервисом идентификации

import { UserManager, UserManagerSettings } from "oidc-client"; // библиотека нужна для работы с OpenIDConnetc
import React, {FC,useEffect} from "react";
import { Navigate } from "react-router-dom";
import setAuthHeader from "./auth-headers";

const userManagerSettings: UserManagerSettings = 
{
    client_id: 'notes-web-api', // Обязательно смотримс на то, что мы делали, кому разрешен доступ к ресурсам, когда мы настраивали сервис авторизации
    redirect_uri: 'http://localhost:3000/signin-oidc', // React-приложение по умолчанию запускается по порту 3000, куда вернуть пользователя
    response_type: 'code', // использование современного безопасного протокола (API) дял перенаправления
    scope: 'openid profile NotesWebAPI', // Права и данные, которые запрашивает пользователь
    authority: 'http://localhost:5026', // URL-адрес сервера идентификации(какой порт слушает)
    post_logout_redirect_uri: 'http://localhost:3000/signout-oidc', // куда вернуть пользователя после выхода
};
// Дублируем доступные области из нашего сервера идентификации
const userManager = new UserManager(userManagerSettings); // Инициализируем менеджера по работе с пользователями и передаем ему настройки для работы с сервером идентификации
export async function loadUser() // Проверяет, есть ли уже сохраненный пользователь
{
    const user = await userManager.getUser();
    console.log('User: ', user);
    const token = user?.access_token;
    setAuthHeader(token);
}

export const signinRedirect = () => userManager.signinRedirect(); // Создание ссылки дял перенаправления

export const signinRedirectCallback = () => userManager.signinRedirectCallback(); // Вызов

export const signoutRedirect = (args?: any) => { // Удаление пользователя(очистка локальных данных) и перенаправление
    userManager.clearStaleState();
    userManager.removeUser();
    return userManager.signoutRedirect(args);
}

export const signoutRedirectCallback = () => { // Вызов
    userManager.clearStaleState();
    userManager.removeUser();
    return userManager.signoutRedirectCallback();
}

export default userManager;