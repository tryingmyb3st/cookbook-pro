import { useState } from 'react';
import AddRecipeCard from '../AddRecipeCard/AddRecipeCard';
import './MyRecipesPage.css';

export default function MyRecipesPage() {
  const [showAddForm, setShowAddForm] = useState(false);
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  const handleAddClick = () => {
    setShowAddForm(true);
  };

  const handleCancel = () => {
    setShowAddForm(false);
  };

  const handleSuccess = (newRecipe) => {
    console.log('Recipe created successfully:', newRecipe);
    setShowAddForm(false);
    setRefreshTrigger(prev => prev + 1);
  };

  return (
    <div className="my-recipes-page">
        <div className="my-recipes-page-content">
            <div className="page-header">
                <h2>Мои рецепты</h2>
                {!showAddForm && <button className="add-new-recipe" onClick={handleAddClick}>
                    Добавить
                </button>}
            </div>
            <div className="page-content">
                <div>Cards</div>
                <div className="my-recipes-page-add-recipe-card">
                    {showAddForm && <AddRecipeCard 
                        onCancel={handleCancel}
                        onSuccess={handleSuccess}
                    />}
                </div>
            </div>
        </div>
    </div>
  );
}