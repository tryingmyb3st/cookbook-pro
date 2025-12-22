import RecipeCard from '../RecipeCard/RecipeCard'
import { RecipeService } from '../../services/RecipeService';
import TheMealDbRecipeCard from '../RecipePage/TheMealDbRecipeCard';
import Search from '../Search/Search';
import { useState, useEffect } from 'react';
import './MainPage.css'

export default function MainPage() {
    const [recipes, setRecipes] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [searchResults, setSearchResults] = useState([]);
    const [isSearching, setIsSearching] = useState(false);

    const loadRandomRecipes = async () => {
        setLoading(true);
        setError(null);
        try {
            const fetchedRecipes = [];
            for (let i = 1; i <= 100; i++) {
                const recipe = await RecipeService.getRecipeById(i);
                if (recipe) {
                    fetchedRecipes.push(recipe);
                }
            }
            setRecipes(fetchedRecipes);
        } catch (err) {
            setError('Не удалось загрузить рецепты');
        } finally {
            setLoading(false);
        }
    };

    const handleSearch = async (searchTerm) => {
        if (!searchTerm.trim()) {
            setSearchResults([]);
            setIsSearching(false);
            return;
        }

        setIsSearching(true);
        try {
            const results = await RecipeService.getRecipeByName(searchTerm);
            setSearchResults(results);
        } catch (err) {
            console.error('Ошибка поиска:', err);
            setSearchResults([]);
        } finally {
            setIsSearching(false);
        }
    };

    const handleClearSearch = () => {
        setSearchResults([]);
        setIsSearching(false);
    };

    useEffect(() => {
        loadRandomRecipes();
    }, []);

    return (
        <div className="main-page">
            <div className="main-header">
                <div className="header-left">
                    <h2>Рецепты</h2>
                </div>
                <div className="header-right">
                    <Search 
                        onSearch={handleSearch}
                        onClear={handleClearSearch}
                        placeholder="Поиск..."
                        loading={isSearching}
                    />
                </div>
            </div>
            
            {searchResults.length > 0 ? (
                <>
                    <div className="recipe-list">
                        {searchResults.map(recipe => (
                            <RecipeCard 
                                key={recipe.id}
                                recipeId={recipe.id}
                                recipeData={recipe}
                            />
                        ))}
                    </div>
                </>
            ) : (
                <>
                    {loading && recipes.length === 0 ? (
                        <div className="loading-state">
                            <p>Загрузка рецептов...</p>
                        </div>
                    ) : (
                        <div className="recipe-list">
                            {recipes.map(recipe => (
                                <RecipeCard 
                                    key={recipe.id || recipe.name}
                                    recipeId={recipe.id}
                                    recipeData={recipe}
                                />
                            ))}
                            
                            {recipes.length === 0 && !loading && (
                                <p className='error'>Нет рецептов для отображения</p>
                            )}
                        </div>
                    )}
                </>
            )}

            <div className="random-recipe-section">
                <h2>Случайный рецепт</h2>
                <TheMealDbRecipeCard />
            </div>
        </div>
    );
}