import React, {FC,useEffect, useRef , useState, ReactElement} from "react";
import { useNavigate } from "react-router-dom";
export const RegisterPage: FC <{}> = (): ReactElement =>{
    const [login,setLogin] = useState('');
    const [password,setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [error,setError] = useState('');
    const [isLoading,setIsLoading] = useState(false);
    const navigate = useNavigate();
    const handleRegister = async (e: React.FormEvent) =>
    {
        e.preventDefault();
        setError('');
        // Проверяем, совпадают ли пароли
        if (password != confirmPassword)
        {
            setError('Пароли не совпадают!');
            return;
        }
        setIsLoading(true);
        try
        {
            const response = await fetch('http://localhost:5026/api/auth/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({login,password,confirmPassword}),
            });
            if (response.ok)
            {
                const data = await response.json();
                if (data.token)
                {
                    localStorage.setItem('token',data.token);
                }
                navigate('/notes');
            }
            else
            {
                setError('Ошибка регистрации. Возможно, такой логин уже занят.');
            }

        }
        catch (error)
        {
            console.error(error);
            setError('Ошибка подключения к серверу!');
        }
        finally
        {
            setIsLoading(false);
        }
    };
    return(
        <div style={{ display: 'flex', justifyContent:'center',alignItems:'center',height:'100vh'}}>
            <form onSubmit={handleRegister} style={{width: '300px', padding: '20px', border: '1px solid #ccc', borderRadius: '8px'}}>
                <h2>Регистрация</h2>
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
                <div style={{marginBottom:'15px', textAlign:'left'}}>
                <label style={{display:'block',marginBottom:'5px'}}>Повторите пароль</label>
                <input
                    type="password"
                    value={confirmPassword}
                    onChange={(e)=>setConfirmPassword(e.target.value)}
                    required
                    style={{width:'100%',padding:'8px',boxSizing:'border-box'}}
                />
                </div>
                <button
                    type="submit"
                    disabled={isLoading}
                    style={{width:'100%',padding: '10px',background:'#007bff',color:'#fff', border:'none',borderRadius:'4px', cursor:'pointer'}}
                >
                    {isLoading? 'Регистрация пользователя...':'Зарегистрироваться'}
                </button>
            </form>
        </div>
    );
};