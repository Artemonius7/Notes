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
const App: FC <{}> = (): ReactElement => {
  useEffect(()=>
  {
    loadUser(); // Проверяет, есть ли в браузере сохраненный токен пользователя
  }, []); // Обернули loadUser с пустым массивом зависимостей
  return (
    <div className="App">
      <header className="App-header">
        <AuthProvider userManager={userManager}>
          <Router>
            <Routes>
                <Route path="/" element={<NoteList/>} />
                <Route 
                    path='/signout-oidc'
                    element={<SignOutOidc/>}
                />
                <Route 
                    path='/signin-oidc'
                    element={<SignInOidc/>}
                />
            </Routes>
          </Router>
        </AuthProvider>
        <button className = "login-button" onClick={()=> signinRedirect()}>Login</button> 
      </header>
    </div>
  );
}

export default App;
