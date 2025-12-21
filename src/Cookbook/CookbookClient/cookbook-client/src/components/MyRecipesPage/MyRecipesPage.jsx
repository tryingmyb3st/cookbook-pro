import { useState, useEffect } from 'react';
import AddRecipeCard from '../AddRecipeCard/AddRecipeCard';
import RecipeCard from '../RecipeCard/RecipeCard';
import { RecipeService } from '../../services/RecipeService';
import './MyRecipesPage.css';

export default function MyRecipesPage() {
  const [showAddForm, setShowAddForm] = useState(false);
  const [userRecipes, setUserRecipes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentUserId, setCurrentUserId] = useState(null);
  const [isInitialLoad, setIsInitialLoad] = useState(true);

  useEffect(() => {
    const userData = localStorage.getItem('user');
    if (userData) {
      try {
        const user = JSON.parse(userData);
        if (user && user.id) {
          setCurrentUserId(user.id);
        } else {
          setError('Не удалось получить ID пользователя');
        }
      } catch (err) {
        console.error('Ошибка парсинга user data:', err);
        setError('Ошибка загрузки данных пользователя');
      }
    } else {
      setError('Пользователь не авторизован');
    }
  }, []);

  const saveRecipesToStorage = (recipes, userId) => {
    try {
      const cacheData = {
        userId,
        recipes,
        timestamp: Date.now(),
        recipeIds: recipes.map(recipe => recipe.id) // Сохраняем только ID для быстрого доступа
      };
      localStorage.setItem(`userRecipes_${userId}`, JSON.stringify(cacheData));
    } catch (err) {
    }
  };

  const loadRecipesFromStorage = (userId) => {
    try {
      const cachedData = localStorage.getItem(`userRecipes_${userId}`);
      if (!cachedData) return null;
      
      const parsedData = JSON.parse(cachedData);
      
      if (parsedData.userId !== userId) return null;
      if (Date.now() - parsedData.timestamp > 60 * 60 * 1000) { // 1 час
        return null;
      }
      
      return parsedData.recipes;
    } catch (err) {
      console.error('Ошибка загрузки из LocalStorage:', err);
      return null;
    }
  };

  const loadUserRecipes = async (userId, forceRefresh = false) => {
    setLoading(true);
    setError(null);
    
    try {
      if (!forceRefresh) {
        const cachedRecipes = loadRecipesFromStorage(userId);
        if (cachedRecipes && cachedRecipes.length > 0) {
          setUserRecipes(cachedRecipes);
          setIsInitialLoad(false);
          setLoading(false);
          return;
        }
      }
      
      const userRecipesList = [];
      
      let recipeIdsToCheck = [];
      const cachedData = localStorage.getItem(`userRecipes_${userId}`);
      
      if (cachedData && !forceRefresh) {
        const parsedData = JSON.parse(cachedData);
        recipeIdsToCheck = parsedData.recipeIds || [];
      } else {
        recipeIdsToCheck = Array.from({length: 100}, (_, i) => i + 1);
      }
      
      for (const recipeId of recipeIdsToCheck) {
        try {
          const recipe = await RecipeService.getRecipeById(recipeId);
          if (recipe && recipe.userId === userId) {
            userRecipesList.push(recipe);
          }
        } catch (err) {
        }
        
        if (recipeId % 10 === 0) {
          await new Promise(resolve => setTimeout(resolve, 10));
        }
      }
      
      setUserRecipes(userRecipesList);
      saveRecipesToStorage(userRecipesList, userId);
      setIsInitialLoad(false);      
    } catch (err) {
      console.error('Ошибка загрузки рецептов:', err);
      setError('Не удалось загрузить рецепты');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (!currentUserId) return;
    
    loadUserRecipes(currentUserId, false);
  }, [currentUserId]);

  const handleSuccess = async (newRecipe) => {
    setShowAddForm(false);
    
    if (newRecipe && newRecipe.id) {
      const updatedRecipes = [...userRecipes, newRecipe];
      setUserRecipes(updatedRecipes);
      
      if (currentUserId) {
        saveRecipesToStorage(updatedRecipes, currentUserId);
      }
    } else {
      if (currentUserId) {
        await loadUserRecipes(currentUserId, true);
      }
    }
  };

  const handleDeleteRecipe = (recipeId) => {
    const updatedRecipes = userRecipes.filter(recipe => recipe.id !== recipeId);
    setUserRecipes(updatedRecipes);
    
    if (currentUserId) {
      saveRecipesToStorage(updatedRecipes, currentUserId);
    }
  };

  const handleRefreshCache = () => {
    if (currentUserId) {
      loadUserRecipes(currentUserId, true);
    }
  };

  const handleAddClick = () => {
    setShowAddForm(true);
  };

  const handleCancel = () => {
    setShowAddForm(false);
  };

  return (
    <div className="my-recipes-page">
      <div className="my-recipes-page-content">
        <div className="page-header">
          <div className="header-left">
            <h2>Мои рецепты</h2>
          </div>
          {!showAddForm && (
            <button className="add-new-recipe" onClick={handleAddClick}>
              Добавить рецепт
            </button>
          )}
        </div>

        <div className="page-content">
          {loading && (
            <div className="loading-state">
              <p>{isInitialLoad ? 'Загрузка ваших рецептов...' : 'Обновление списка...'}</p>
            </div>
          )}
          
          {error && !loading && (
            <div className="error-state">
              <p>{error}</p>
            </div>
          )}
          
          {!loading && !error && (
            <div className="user-recipes-list">
              {userRecipes.length > 0 ? (
                <>
                  <div className="recipes-grid">
                    {userRecipes.map(recipe => (
                      <RecipeCard 
                        key={recipe.id}
                        recipeId={recipe.id}
                        recipeData={recipe}
                        onDelete={handleDeleteRecipe}
                      />
                    ))}
                  </div>
                </>
              ) : (
                <div className="no-recipes-message">
                  <p>У вас еще нет рецептов. Добавьте первый!</p>
                </div>
              )}
            </div>
          )}

          <div className="my-recipes-page-add-recipe-card">
            {showAddForm && (
              <AddRecipeCard 
                onCancel={handleCancel}
                onSuccess={handleSuccess}
              />
            )}
          </div>
        </div>
      </div>
    </div>
  );
}