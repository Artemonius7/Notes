import {FC, ReactElement, useEffect} from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import './css/App.css'
import './css/Animations.css'
import logo from './logo.svg';
import AuthProvider from './auth/auth-provider';
import NoteList from './Notes/NoteList';
import HomePage from './HomePage/HomePage';
import LoginPage from './LoginPage/LoginPage';
import PageTransition from './effects/PageTransition';
import { RegisterPage } from './RegisterPage/RegisterPage';
import {ProtectedRoute} from './components/ProtectedRoute'
const App: FC <{}> = (): ReactElement => {
  return (
    <div className="App">
      <header className='App-header'>
            <Router>
              <Routes>
                  {/* Главная публичная страница при запуске приложения */}
                  <Route path='/' element = {<HomePage/>}/>
                  {/* Подключаем страницы регистрации и авторизации*/}
                  <Route path='/LoginPage' element={<PageTransition><LoginPage/></PageTransition>}/> {/*Аналогично, только с выходом, перенаправляем на главную страницу*/}
                  <Route path='/RegisterPage' element={<PageTransition><RegisterPage/></PageTransition>}/>
                  {/* Личная страница пользователя после успешного получения токена */}
                  <Route path='/notes' element={<ProtectedRoute><PageTransition><NoteList/></PageTransition></ProtectedRoute>}/>
              </Routes>
            </Router>
      </header>
    </div>
  );
}

export default App;
