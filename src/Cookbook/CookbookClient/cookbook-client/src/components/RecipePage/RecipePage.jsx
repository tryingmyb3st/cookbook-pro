import { useParams, useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { useRecipeImage } from '../../hooks/useRecipeImage';
import IngredientCard from '../IngredientCard/IngredientCard';
import { RecipeService } from '../../services/RecipeService';
import './RecipePage.css';

export default function RecipePage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [recipe, setRecipe] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [instructionsList, setInstructionsList] = useState([]);

  const { image: recipeImage } = useRecipeImage(recipe);

  useEffect(() => {
    const loadRecipe = async () => {
      try {
        const data = await RecipeService.getRecipeById(id);
        setRecipe(data);
        
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
      <div className="recipe-header">
        <button className="back-button" onClick={() => navigate(-1)}>Назад</button>
        <img 
          src={recipeImage} 
          alt={recipe.name}
          className="recipe-image"
        />
        <div className="recipe-page-info">
          <h1>{recipe.name}</h1>
          <p className="recipe-author">{recipe.author ? `@${recipe.author}` : "@author"}</p>
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