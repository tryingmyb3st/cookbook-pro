import RecipeCard from '../RecipeCard/RecipeCard'
import './MainPage.css'

export default function MainPage() {
    return <div className="main-page">
        <h2>Рецепты</h2>
        <div className="recipe-list"> 
            <RecipeCard recipeId={1}/>
            <RecipeCard recipeId={2}/>
        </div>
    </div>
}