// Этот файл создан для того чтобы защититься от несанкционированного доступа 
// пользователя без токена доступа, чтобы он не попал сразу по ссылке в личный кабинет
import { Navigate } from "react-router-dom";
import { UserManager } from "oidc-client";
import React, {Children, useEffect, useState} from "react";
import { User } from "oidc-client";
import userManager from "../auth/user-service";

interface ProtectedRouteProps{
    children: React.ReactNode;
}

export const ProtectedRoute = ({children}: ProtectedRouteProps) =>{
    const token = localStorage.getItem('token');
    // если токена нет - отправляем на страницу логина
    if (!token){
        return <Navigate to="LoginPage" replace/>
    }
    // если все хорошо-показываем дочерний компонент, т.е личный кабинет юзера
    return <>{children}</>
};

export default ProtectedRoute;