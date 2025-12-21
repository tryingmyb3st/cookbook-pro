import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import './Auth.css';

function Register() {
  const [formData, setFormData] = useState({
    fullName: '',
    email: '',
    password: '',
    confirmPassword: ''
  });
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
    if (error) setError('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');

    try {
      const response = await fetch('/cookbook/Auth/Register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          email: formData.email,
          password: formData.password,
          confirmPassword: formData.confirmPassword,
          fullName: formData.fullName
        })
      });

      const responseText = await response.text();
      
      if (response.ok) {
        // Успешная регистрация
        let data = {};
        try {
          data = JSON.parse(responseText);
        } catch (e) {
          console.log('Не удалось распарсить JSON ответ');
        }
        
        if (data.token) {
          localStorage.setItem('token', data.token);
        }
        
        const userData = {
          id: data.userId,
          email: data.email || formData.email,
          userName: data.userName || formData.fullName,
          fullName: formData.fullName,
          expiresAt: data.expiresAt
        };
        
        localStorage.setItem('user', JSON.stringify(userData));
        
        navigate('/');
        window.location.reload();
      } else {
        // Обработка ошибки
        let errorMessage = responseText;
        
        try {
          // Пытаемся парсить JSON
          const errorData = JSON.parse(responseText);
          
          // Если это массив с объектами, содержащими поле description
          if (Array.isArray(errorData)) {
            // Берем первую ошибку и её description
            if (errorData[0] && errorData[0].description) {
              errorMessage = errorData[0].description;
            } else if (errorData[0] && errorData[0].message) {
              errorMessage = errorData[0].message;
            }
          }
          // Если это объект с полем description
          else if (errorData.description) {
            errorMessage = errorData.description;
          }
          // Если это объект с полем message
          else if (errorData.message) {
            errorMessage = errorData.message;
          }
          // Если это старый формат с полем title
          else if (errorData.title) {
            errorMessage = errorData.title;
          }
        } catch (e) {
          // Если не JSON, оставляем исходный текст
          console.log('Ответ не JSON, используем текст как есть');
        }
        
        setError(errorMessage || `Ошибка регистрации: ${response.status}`);
      }
    } catch (err) {
      setError(`Ошибка соединения: ${err.message}`);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-form">
        <h2>Регистрация</h2>
        
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="fullName">Полное имя</label>
            <input
              type="text"
              id="fullName"
              name="fullName"
              value={formData.fullName}
              onChange={handleChange}
              placeholder="Иван Иванов"
              required
            />
          </div>
          
          <div className="form-group">
            <label htmlFor="email">Email</label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              placeholder="user@example.com"
              required
            />
          </div>
          
          <div className="form-group">
            <label htmlFor="password">Пароль</label>
            <input
              type="password"
              id="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              placeholder="Не менее 6 символов"
              minLength="6"
              required
            />
          </div>
          
          <div className="form-group">
            <label htmlFor="confirmPassword">Подтвердите пароль</label>
            <input
              type="password"
              id="confirmPassword"
              name="confirmPassword"
              value={formData.confirmPassword}
              onChange={handleChange}
              placeholder="Повторите пароль"
              minLength="6"
              required
            />
          </div>

          {error && (
          <div className="error-message">
            {error}
          </div>
          )}
          
          <button 
            type="submit" 
            className="auth-button"
            disabled={isLoading}
          >
            {isLoading ? 'Регистрация...' : 'Зарегистрироваться'}
          </button>
        </form>
        
        <p className="auth-link">
          Уже есть аккаунт? <Link to="/login">Войти</Link>
        </p>
      </div>
    </div>
  );
}

export default Register;