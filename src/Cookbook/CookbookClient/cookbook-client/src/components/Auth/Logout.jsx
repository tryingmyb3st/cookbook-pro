import './Auth.css'
import { useNavigate } from 'react-router-dom';

export default function Logout() {
    const navigate = useNavigate();
    const handleLogout = () => {
        localStorage.removeItem('token');
    
        const userData = localStorage.getItem('user');
    
        if (userData) {
            try {
                const user = JSON.parse(userData);
                const userId = user.id;
        
                if (userId) {
                    localStorage.removeItem(`userRecipes_${userId}`);
                }
        
                localStorage.removeItem('user');
                window.dispatchEvent(new Event('storage'));
        
            } catch (error) {
                console.error('Ошибка при обработке данных пользователя:', error);
            }
        }
        navigate('/register');
  };

  return <div onClick={handleLogout} className='logout-button'>Выйти</div>;
}