import React, {useEffect,useRef,useState, FC, ReactElement} from "react";
import {useNavigate} from 'react-router-dom';
export const LoginPage: FC<{}> = (): ReactElement =>
{
    const [login,setLogin] = useState('');
    const [password,setPassword] = useState('');
    const [error,setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();

    const handleLogin = async (e: React.FormEvent) => 
    {
        e.preventDefault();
        setError('');
        setIsLoading(true);
        try {
            // Делаем запрос к нашему бэкенду
            const response = await fetch('http://localhost:5026/api/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({login,password}),
            });
            if (response.ok)
            {
                const data = await response.json();
                // Если сервер возвращает токен, то сохраняем его на React для нашего личного кабинета
                if (data.token){
                    localStorage.setItem('token',data.token);
                }
                // Делаем перенаправление в личный кабинет пользователя
                navigate('/notes');
            }
            else{
                setError('Неверный логин или пароль!');
            }
        } catch (error){
            console.error(error);
            setError('Ошибка подключения к серверу!');
        } finally{
            setIsLoading(false);
        }
    };
    return(
        <div style={{ display: 'flex', justifyContent:'center',alignItems:'center',height:'100vh'}}>
            <form onSubmit={handleLogin} style={{width: '300px', padding: '20px', border: '1px solid #ccc', borderRadius: '8px'}}>
                <h2>Вход в систему</h2>
                {error && <div style={{color:'red', marginBottom: '10px',fontSize: '14px'}}>{error}</div>}

                <div style={{marginBottom: '15px', textAlign: 'left'}}>
                <label style={{display: 'block', marginBottom: '5px'}}>Логин</label>
                <input
                    type="text"
                    value={login}
                    onChange={(e)=> setLogin(e.target.value)}
                    required
                    style={{width:'100%',padding:'8px',boxSizing: 'border-box'}}
                />
                </div>

                <div style={{marginBottom: '15px', textAlign: 'left'}}>
                <label style={{display: 'block', marginBottom: '5px'}}>Пароль</label>
                <input
                    type="password"
                    value={password}
                    onChange={(e)=> setPassword(e.target.value)}
                    required
                    style={{width:'100%',padding:'8px',boxSizing: 'border-box'}}
                />
                </div>
                <button
                    type="submit"
                    disabled={isLoading}
                    style={{width:'100%',padding: '10px',background:'#007bff',color:'#fff', border:'none',borderRadius:'4px', cursor:'pointer'}}
                >
                    {isLoading? 'Вход...':'Войти'}
                </button>
            </form>
        </div>
    );
};
export default LoginPage;