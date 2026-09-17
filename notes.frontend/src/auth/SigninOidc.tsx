// Отображение страницы перенаправления обратно после аутентификации 
import React, {FC, useEffect} from "react";
import { useNavigate } from  'react-router-dom';
import {signinRedirectCallback} from "./user-service";

const SignInOidc: FC <{}> = () =>
{
    const navigate = useNavigate();
    useEffect (()=>
    {
        async function signinAsync()
        {
            await signinRedirectCallback(); // Срабатывает при нажатии кнопки 'Войти'
            navigate('/'); // Возврат на главную страницу
        }
        signinAsync();
    },[navigate]);
    return <div>Redirecting...</div>; // заглушка во время перенаправления
}

export default SignInOidc;