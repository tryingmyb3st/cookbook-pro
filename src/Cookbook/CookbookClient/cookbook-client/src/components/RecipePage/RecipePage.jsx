import {  useParams,useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import IngredientCard from '../IngredientCard/IngredientCard';
import { ingredientService } from '../../services/RecipeService';
import './RecipePage.css';

export default function RecipePage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [recipe, setRecipe] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [image, setImage] = useState(null)

  useEffect(() => {
    const loadRecipe = async () => {
      try {
        const data = await ingredientService.getRecipeById(id);
        setRecipe(data);
        const image = await ingredientService.getRecipeImage(id);
        setImage(image);
      } catch (err) {
        console.error(err);
        setError('Ошибка при загрузке рецепта');
      } finally {
        setLoading(false);
      }
    };
    loadRecipe();
  }, [id]);

  if (loading) return <div className="recipe-page">Загрузка...</div>;
  if (error) return <div className="recipe-page">{error}</div>;
  if (!recipe) return <div className="recipe-page">Рецепт не найден</div>;

  return (
    <div className="recipe-page">
      <button className="back-button" onClick={() => navigate(-1)}>Назад</button>
      <div className="recipe-header">
        <img src={image || '/assets/default.png'} className="recipe-image" />
        <div className="recipe-info">
          <h1>{recipe.name}</h1>
          <p>Вес: {recipe.weight ? `${recipe.weight} г` : "марк вес не работает"}</p>
        </div>
      </div>
      <div className="recipe-body">
        <div className="ingredients">
          <h2>Ингредиенты:</h2>
          <div className="ingredient-list">
            {recipe.ingredients ? recipe.ingredients.map(ing => (
                <IngredientCard key={ing.id} ingredient={ing} />
                )) : <p>Нет ингредиентов</p>}
          </div>
        </div>
        <div className="instructions">
          <h2>Инструкция:</h2>
          <ol>
            {recipe.instructions && recipe.instructions.length > 0
                 ? recipe.instructions.map((step, index) => <li key={index}>{step}</li>)
                : recipe.instruction
                ? recipe.instruction.split(/\d+\.\s*/).filter(Boolean).map((step, index) => (
                    <li key={index}>{step.trim()}</li>
                ))
                : <li>Нет инструкции</li>
            }
          </ol>
        </div>
      </div>
    </div>
  );
}
