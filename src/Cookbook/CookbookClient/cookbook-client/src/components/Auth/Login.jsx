import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import './Auth.css';

function Login() {
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    rememberMe: false
  });
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === 'checkbox' ? checked : value
    });
    if (error) setError('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');

    try {
      const response = await fetch('/cookbook/Auth/Login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          email: formData.email,
          password: formData.password,
          rememberMe: formData.rememberMe
        })
      });
      const responseText = await response.text();
      let data = null;
      
      try {
        if (responseText) {
          data = JSON.parse(responseText);
        }
      } catch (jsonError) {
        console.log('Не удалось распарсить JSON:', jsonError.message);
      }
      
      if (response.ok) {
        if (data && data.token) {
          localStorage.setItem('token', data.token);
        }
        
        const userData = {
          id: data?.userId || 0,
          email: data?.email || formData.email,
          userName: data?.userName || data?.email?.split('@')[0] || formData.email.split('@')[0],
          expiresAt: data?.expiresAt,
          rememberMe: formData.rememberMe
        };
        
        localStorage.setItem('user', JSON.stringify(userData));
        navigate('/');
        
        setTimeout(() => window.location.reload(), 100);
      } else {
        if (response.status === 401 || response.status === 400) {
          if (data) {
            if (data.title) {
              setError(data.title);
            } else if (data.message) {
              setError(data.message);
            } else if (data.errors) {
              const firstError = Object.values(data.errors)[0];
              setError(Array.isArray(firstError) ? firstError[0] : String(firstError));
            } else if (typeof data === 'string') {
              setError(data);
            } else {
              setError('Неверный email или пароль');
            }
          } else if (responseText) {
            setError(responseText.length > 100 ? responseText.substring(0, 100) + '...' : responseText);
          } else {
            setError('Неверный email или пароль');
          }
        } else if (data && data.title) {
          setError(data.title);
        } else if (data && data.message) {
          setError(data.message);
        } else if (responseText) {
          setError(`Ошибка сервера: ${response.status}. ${responseText.substring(0, 100)}`);
        } else {
          setError(`Ошибка входа: ${response.status} ${response.statusText}`);
        }
      }
    } catch (err) {
      setError(`Ошибка соединения с сервером: ${err.message}`);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-form">
        <h2>Вход</h2>
        <form onSubmit={handleSubmit}>
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
              placeholder="Введите пароль"
              required
            />
          </div>
          
          <div className="form-group checkbox-group">
            <label className="checkbox-label">
              <input
                type="checkbox"
                id="rememberMe"
                name="rememberMe"
                checked={formData.rememberMe}
                onChange={handleChange}
                className="checkbox-input"
              />
              <span className="checkbox-custom"></span>
              Запомнить меня
            </label>
          </div>
        {error && <div className="error-message">{error}</div>}
          
          <button 
            type="submit" 
            className="auth-button"
            disabled={isLoading}
          >
            {isLoading ? 'Вход...' : 'Войти'}
          </button>
        </form>
        <p className="auth-link">
          Нет аккаунта? <Link to="/register">Зарегистрироваться</Link>
        </p>
      </div>
    </div>
  );
}

export default Login;