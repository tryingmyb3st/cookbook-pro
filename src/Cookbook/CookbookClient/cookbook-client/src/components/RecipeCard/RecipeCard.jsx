import './RecipeCard.css'
import { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { RecipeService } from '../../services/RecipeService';
import { FileService } from '../../services/FileService';

export default function RecipeCard({ recipeId, recipeData }) {
    const [recipe, setRecipe] = useState(recipeData || null);
    const [loading, setLoading] = useState(!recipeData);
    const [image, setImage] = useState(null);
    const imageBlobUrlRef = useRef(null);
    
    const navigate = useNavigate();
    
    useEffect(() => {
        if (recipeData) {
            setRecipe(recipeData);
            setLoading(false);
            loadImage(recipeData);
            return;
        }
        
        const fetchRecipe = async () => {
            try {
                setLoading(true);
                const recipeData = await RecipeService.getRecipeById(recipeId);
                setRecipe(recipeData);
                loadImage(recipeData);
            } catch (err) {
                console.log('Ошибка загрузки рецепта:', err.message);
            } finally {
                setLoading(false);
            }
        };
        
        fetchRecipe();
    }, [recipeId, recipeData]);

    useEffect(() => {
        return () => {
            if (imageBlobUrlRef.current) {
                URL.revokeObjectURL(imageBlobUrlRef.current);
                imageBlobUrlRef.current = null;
            }
        };
    }, []);

    const loadImage = async (recipe) => {
        try {            
            if (recipe?.fileName?.startsWith('http')) {
                setImage(recipe.file_name);
                return;
            }
            
            const fileName = recipe.fileName;
            const recipeImage = await FileService.getImage(fileName);

            
            if (recipeImage instanceof Blob) {
                if (imageBlobUrlRef.current) {
                    URL.revokeObjectURL(imageBlobUrlRef.current);
                }
                
                const imageUrl = URL.createObjectURL(recipeImage);
                imageBlobUrlRef.current = imageUrl;
                setImage(imageUrl);
            } else {
                setImage(recipeImage);
            }
            
        } catch (err) {
            const defaultImage = await import('../../assets/recipes/default.jpg');
            setImage(defaultImage.default);
        }
    };

    function getRecipeIngredients(recipe) {
        if (!recipe || !recipe.ingredients) return '';
        const ingredients = recipe.ingredients;
        const names = ingredients.map(ingredient => ingredient.name);
        const fullText = names.join(" · ");
    
        if (fullText.length <= 50) return fullText;
        return fullText.substring(0, 47) + '...';
    }

    function handleCardClick() {
        const id = recipe?.id || recipeId;
        if (id) {
            navigate(`/recipe/${id}`);
        }
    }

    if (loading) return <div className='recipe-card-loading'>Загрузка...</div>;
    if (!recipe) return <div className='recipe-card-error'>Рецепт не найден</div>;
    
    return (
        <div className='recipe-card' onClick={handleCardClick}>
            <div className='recipe-image-container'>
                <img 
                    src={image} 
                    alt={recipe.name}
                />
            </div>
            <div className='recipe-info'>
                <h4>{recipe.name}</h4>
                <h5 className='weight'>
                    {recipe.weight == null || recipe.weight === 0 ? '-' : recipe.weight + ' г'}
                </h5>
                <h5>{getRecipeIngredients(recipe)}</h5>
            </div>
        </div>
    );
}