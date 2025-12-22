import { useParams, useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { useRecipeImage } from '../../hooks/useRecipeImage';
import IngredientCard from '../IngredientCard/IngredientCard';
import { RecipeService } from '../../services/RecipeService';
import { AuthService } from '../../services/AuthService';
import './RecipePage.css';

export default function RecipePage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [recipe, setRecipe] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [instructionsList, setInstructionsList] = useState([]);
  const [currentUserId, setCurrentUserId] = useState(null);
  const [isDeleting, setIsDeleting] = useState(false);
  const [deleteError, setDeleteError] = useState(null);

  const { image: recipeImage } = useRecipeImage(recipe);

  useEffect(() => {
    const userData = localStorage.getItem('user');
    if (userData) {
      try {
        const user = JSON.parse(userData);
        if (user.id) {
          setCurrentUserId(user.id);
        }
      } catch (error) {
        console.error('Ошибка при чтении данных пользователя:', error);
      }
    }
  }, []);

  useEffect(() => {
    const loadRecipe = async () => {
      try {
        const data = await RecipeService.getRecipeById(id);

        const userInfo = await AuthService.getUserInfo(data.userId);
        
        const updatedRecipe = {
          ...data,
          userName: userInfo.userName.split('@')[0],
        };
        
        setRecipe(updatedRecipe);
        
        if (data.instructions && data.instructions.length > 0) {
          setInstructionsList(data.instructions);
        } else if (data.instruction) {
          const parsedInstructions = data.instruction
            .split(/\d+\.\s*/)
            .filter(Boolean)
            .map(step => step.trim());
          setInstructionsList(parsedInstructions);
        }
      } catch (err) {
        console.error('Ошибка при загрузке рецепта:', err);
        setError('Ошибка при загрузке рецепта');
      } finally {
        setLoading(false);
      }
    };
    loadRecipe();
  }, [id]);

  const handleDeleteRecipe = async () => {
    if (!window.confirm('Вы уверены, что хотите удалить этот рецепт?')) {
      return;
    }

    setIsDeleting(true);
    setDeleteError(null);

    try {
      await RecipeService.deleteRecipe(id);
      
      const userData = localStorage.getItem('user');
      if (userData) {
        try {
          const user = JSON.parse(userData);
          const userRecipesKey = `userRecipes_${user.id}`;
          const userRecipesData = localStorage.getItem(userRecipesKey);
          
          if (userRecipesData) {
            const parsedData = JSON.parse(userRecipesData);
            
            const updatedRecipes = parsedData.recipes.filter(recipe => recipe.id !== parseInt(id));
            
            const updatedRecipeIds = parsedData.recipeIds.filter(recipeId => recipeId !== parseInt(id));
            
            const updatedData = {
              ...parsedData,
              userId: user.id,
              recipes: updatedRecipes,
              recipeIds: updatedRecipeIds,
              timestamp: Date.now()
            };
            
            localStorage.setItem(userRecipesKey, JSON.stringify(updatedData));
          }
        } catch (error) {
          console.error('Ошибка при обновлении localStorage:', error);
        }
      }

      navigate('/');
      
    } catch (err) {
      console.error('Ошибка при удалении рецепта:', err);
      setDeleteError('Не удалось удалить рецепта. Попробуйте еще раз.');
    } finally {
      setIsDeleting(false);
    }
  };

  const isRecipeOwner = currentUserId && recipe && recipe.userId === currentUserId;

  if (loading) return <div className="recipe-page">Загрузка...</div>;
  if (error) return <div className="recipe-page">{error}</div>;
  if (!recipe) return <div className="recipe-page">Рецепт не найден</div>;

  return (
    <div className="recipe-page">
      <div className="recipe-header">
        <div className="recipe-actions">
          {isRecipeOwner && (
            <button 
              className="delete-button" 
              onClick={handleDeleteRecipe}
              disabled={isDeleting}
            >
              {isDeleting ? 'Удаление...' : 'Удалить'}
            </button>
          )}
          <button className="back-button" onClick={() => navigate(-1)}>Назад</button>
        </div>
        
        <img 
          src={recipeImage} 
          alt={recipe.name}
          className="recipe-image"
        />
        <div className="recipe-page-info">
          <h1>{recipe.name}</h1>
          <p className="recipe-author">{recipe.userName ? `${recipe.userName}` : "@author"}</p>
          <p className="recipe-weight">{recipe.weight ? `${recipe.weight} г` : "200 г"}</p>
          
          <div className="recipe-instructions-summary">
            <h3>Инструкция</h3>
            {instructionsList.length > 0 ? (
              <ol className="instructions-list">
                {instructionsList.map((step, index) => (
                  <li key={index}>{step}</li>
                ))}
              </ol>
            ) : (
              <p>Нет инструкции по приготовлению</p>
            )}
          </div>
        </div>
      </div>
      
      {deleteError && (
        <div className="error-message">
          {deleteError}
        </div>
      )}
      
      <div className="recipe-body">
        <div className="ingredients">
          <h2>Ингредиенты</h2>
          <div className="ingredient-list">
            {recipe.ingredients ? recipe.ingredients.map(ing => (
              <IngredientCard key={ing.id} ingredient={ing} />
            )) : <p>Нет ингредиентов</p>}
          </div>
        </div>
      </div>
    </div>
  );
}