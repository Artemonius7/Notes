// Этот файл создан для того чтобы защититься от несанкционированного доступа 
// пользователя без токена доступа, чтобы он не попал сразу по ссылке в личный кабинет
import { Navigate } from "react-router-dom";
import React, {Children, useEffect, useState} from "react";
interface ProtectedRouteProps{
    children: React.ReactNode;
}

export const ProtectedRoute = ({children}: ProtectedRouteProps) =>{
    const token = localStorage.getItem('token');
    console.log("ProtectedRoute checked token:",token)
    // если токена нет - отправляем на страницу логина
    if (!token){
        return <Navigate to="/LoginPage" replace/>
    }
    // если все хорошо-показываем дочерний компонент, т.е личный кабинет юзера
    return <>{children}</>
};

export default ProtectedRoute;