import { useNavigate } from 'react-router-dom';
import './Menu.css'
import Logout from '../Auth/Logout';

export default function Header() {
  const navigate = useNavigate();

  const handleAllRecipesClick = () => {
    navigate('/');
  };

  const handleMyRecipesClick = () => {
    navigate('/my');
  }

  return (
    <div className="menu">
      <h2>Cookbook</h2>
      <ul>
        <li onClick={handleAllRecipesClick}>Все рецепты</li>
        <li onClick={handleMyRecipesClick}>Мои рецепты</li>
        <Logout/>
      </ul>
    </div>
  );
};