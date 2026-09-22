import {FC, ReactElement, useEffect} from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import './App.css'
import logo from './logo.svg';
import{
  loadUser,
  signinRedirect,
  signoutRedirect
} from './auth/user-service';
import AuthProvider from './auth/auth-provider';
import SignInOidc from './auth/SigninOidc';
import SignOutOidc from './auth/signoutOidc';
import userManager from './auth/user-service';
import NoteList from './Notes/NoteList';
import HomePage from './HomePage/HomePage';
import LoginPage from './LoginPage/LoginPage';
import { RegisterPage } from './RegisterPage/RegisterPage';
import {ProtectedRoute} from './components/ProtectedRoute'
const App: FC <{}> = (): ReactElement => {
  useEffect(()=>
  {
    loadUser(); // Проверяет, есть ли в браузере сохраненный токен пользователя
  }, []); // Обернули loadUser с пустым массивом зависимостей
  return (
    <div className="App">
      <header className='App-header'>
        <AuthProvider userManager={userManager}>
            <Router>
              <Routes>
                  {/* Главная публичная страница при запуске приложения */}
                  <Route path='/' element = {<HomePage/>}/>
                  {/* Подключаем страницы регистрации и авторизации*/}
                  <Route path='/LoginPage' element={<LoginPage/>}/> {/*Аналогично, только с выходом, перенаправляем на главную страницу*/}
                  <Route path='/RegisterPage' element={<RegisterPage/>}/>
                  {/* Личная страница пользователя после успешного получения токена */}
                  <Route path='/notes' element={<ProtectedRoute><NoteList/></ProtectedRoute>}/>
              </Routes>
            </Router>
          </AuthProvider>
      </header>
    </div>
  );
}

export default App;
