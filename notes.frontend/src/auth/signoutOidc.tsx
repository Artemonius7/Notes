// Страница для перенаправления на главную страницу после выхода пользователя из аккаунта
import React,{FC, useEffect} from "react";
import { useNavigate } from "react-router-dom";
import {signoutRedirectCallback} from "./user-service";
const SignOutOidc: FC <{}> = () =>
{
    const navigate = useNavigate(); // Используем навигацию для последующего перенаправления
    useEffect(()=>
    {
        const SignOutAsync = async() =>
        {
            await signoutRedirectCallback(); // Срабатывает при нажатии кнопки 'Выйти'
            navigate('/');
        }
        SignOutAsync();
    }, [navigate]);
    return <div>Redirecting...</div>
}

export default SignOutOidc; 