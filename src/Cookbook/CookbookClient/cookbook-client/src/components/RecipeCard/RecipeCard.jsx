import './RecipeCard.css'
import { ingredientService } from '../../services/RecipeService.js'
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

export default function RecipeCard({recipeId}) {
    const [recipe, setRecipe] = useState(null);
    const [loading, setLoading] = useState(true);
    const [image, setImage] = useState(null);
    
    const navigate = useNavigate();
    
    useEffect(() => {
        const fetchRecipe = async () => {
            try {
                setLoading(true);
                const recipeData = await ingredientService.getRecipeById(recipeId);
                setRecipe(recipeData);
                const image = await ingredientService.getRecipeImage(recipeId);
                setImage(image);
            } catch (err) {
                console.log(err.message);
            } finally {
                setLoading(false);
            }
        };
        
        fetchRecipe();
    }, [recipeId]);

    function getRecipeIngredients(recipe) {
        const ingredients = recipe.ingredients;
        const names = ingredients.map(ingredient => ingredient.name);
        return names.join(" · ");
    }

    function handleCardClick() {
        navigate(`/recipe/${recipeId}`);
    }

    if (loading) return <div>Загрузка...</div>;
    
    return (
        <div className='recipe-card' onClick={handleCardClick}>
            <img src={image}></img>
            <div className='recipe-info'>
                <h4>{recipe.name}</h4>
                <h5 className='weight'>{recipe.weight == null ? '-' : recipe.weight + ' г'}</h5>
                <h5>{getRecipeIngredients(recipe)}</h5>
            </div>
            
        </div>
    )
}