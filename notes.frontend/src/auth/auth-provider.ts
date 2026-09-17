// компонент для настройки аутентификации пользователя при помощи OpenIDConnect
import React, {FC, use, useEffect, useRef} from "react";
import { User, UserManager } from "oidc-client";
import { setAuthHeader } from "./auth-headers";

type AuthProviderProps = { // Для явного объявления параметров
    userManager: UserManager;
    children : React.ReactNode;
};

const AuthProvider: FC<AuthProviderProps> = ({
    userManager: manager,
    children,
}): any => {
    let userManager = useRef<UserManager | null>(null); // let - тип динамической переменной, которую можно будет переприсваивать
    useEffect(()=>{  // Создаем определенные события с состоянием пользователя (функции-обработчики)
        userManager.current = manager;
        const onUserLoaded = (user: User) => {
            console.log('User loaded :', user);
            setAuthHeader(user.access_token);
        };
        const onUserUnloaded = () => {
            setAuthHeader(null);
            console.log('User unloaded:'); // Значение будет выводиться в консоль разработчика браузера
        };
        const onAccessTokenExpiring = () => {
            console.log('User token expiring');
        };
        const onAccessTokenExpired = () => {
            console.log('User token expired');
        };
        const onUserSignedOut = () => {
            console.log('User signed out');
        };
        // Подписываемся на события, Они будут вызываться Тогда, когда это будет необходимо
        userManager.current.events.addUserLoaded(onUserLoaded);
        userManager.current.events.addUserUnloaded(onUserUnloaded);
        userManager.current.events.addAccessTokenExpiring(onAccessTokenExpiring);
        userManager.current.events.addAccessTokenExpired(onAccessTokenExpired);
        userManager.current.events.addUserSignedOut(onUserSignedOut);
        // Возвращаем функцию очистки события после его использования, чтобы избежать утечки памяти
        return function cleanup() {
            if (userManager && userManager.current){
                userManager.current.events.removeUserLoaded(onUserLoaded);
                userManager.current.events.removeUserUnloaded(onUserUnloaded);
                userManager.current.events.removeAccessTokenExpiring(onAccessTokenExpiring);
                userManager.current.events.removeAccessTokenExpired(onAccessTokenExpired);
                userManager.current.events.removeUserSignedOut(onUserSignedOut);
            }
        };

    }, [manager]);
    return React.Children.only(children);
};

export default AuthProvider;




