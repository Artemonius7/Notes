import { Container, Button, Stack } from "react-bootstrap";
import { Navigate, useNavigate } from "react-router-dom";
export const HomePage = () =>{
    // Обработчик для кнопки Login (здесь будет вызов oidc-client)
    const navigate = useNavigate();
    const handleLogin = () => {
        navigate("/LoginPage");
    };
    // Обработчик для кнопки Register (переход на страницу регистрации)
    const handleRegister = () =>{
        navigate("/RegisterPage");
    };
    // Используем библиотеку Bootstrap для красоты нашей страницы
    return (
        <Container className="d-flex flex-column align-items-center justify-content-center vh-100">
            <div className="text-center mb-4">
                <h2 className="fade-in-button">Добро пожаловать в приложение "Заметки"!</h2>
                <p className="fade-in-button">Войдите в систему или Зарегистрируйтесь</p>
            </div>
            <Stack direction="horizontal" gap={3}>
                <Button className = "fade-in-button" type="submit"
                    style={{width:'100%',padding: '10px',background:'#007bff',color:'#fff', border:'none',borderRadius:'4px', cursor:'pointer'}} onClick={handleLogin}>
                    Войти
                </Button>
                <Button className="fade-in-button" type="submit"
                    style={{width:'100%',padding: '10px',background:'#007bff',color:'#fff', border:'none',borderRadius:'4px', cursor:'pointer'}} onClick={handleRegister}>
                    Зарегистрироваться
                </Button>
            </Stack>
        </Container>
    );
};

export default HomePage;