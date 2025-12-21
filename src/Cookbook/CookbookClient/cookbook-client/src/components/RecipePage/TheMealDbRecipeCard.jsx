import { useState, useEffect } from 'react';
import { RecipeService } from '../../services/RecipeService';
import './RecipePage.css';

export default function TheMealDbRecipeCard() {
  const [recipe, setRecipe] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [image, setImage] = useState(null);
  const [instructionsList, setInstructionsList] = useState([]);
  const [showFullInstructions, setShowFullInstructions] = useState(false);
  const [refreshTrigger, setRefreshTrigger] = useState(0);
  
  const MAX_PREVIEW_LENGTH = 500;

  const loadRecipe = async () => {
    try {
      setLoading(true);
      setError(null);
      setShowFullInstructions(false);
      
      const data = await RecipeService.getRandomFromTheMealDB();
      setRecipe(data);
      
      if (data.fileName) {
        setImage(data.fileName);
      } else {
        try {
          const image = await RecipeService.getRecipeImage(data.id);
          setImage(image);
        } catch (imgErr) {
          setImage('/assets/default.png');
        }
      }
      
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
      setError('Ошибка при загрузке рецепта из TheMealDB');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadRecipe();
  }, [refreshTrigger]);

  const handleRefreshRecipe = () => {
    setRefreshTrigger(prev => prev + 1);
  };

  const getFullInstructionsText = () => {
    return instructionsList.join('\n\n');
  };

  const getTruncatedInstructionsText = () => {
    const fullText = getFullInstructionsText();
    if (fullText.length <= MAX_PREVIEW_LENGTH) {
      return fullText;
    }
    
    const truncated = fullText.substring(0, MAX_PREVIEW_LENGTH);
    const lastSpace = truncated.lastIndexOf(' ');
    return truncated.substring(0, lastSpace) + '...';
  };

  const handleShowMore = () => {
    setShowFullInstructions(true);
  };

  const handleShowLess = () => {
    setShowFullInstructions(false);
  };

  const renderInstructions = () => {
    const fullText = getFullInstructionsText();
    const shouldTruncate = fullText.length > MAX_PREVIEW_LENGTH && !showFullInstructions;
    
    if (shouldTruncate) {
      return (
        <>
          <div className="instructions-text">
            {getTruncatedInstructionsText()}
          </div>
          <button className="read-more-button" onClick={handleShowMore}>
            Читать далее
          </button>
        </>
      );
    } else {
      return (
        <>
          <div className="instructions-text">
            {fullText}
          </div>
          {fullText.length > MAX_PREVIEW_LENGTH && (
            <button className="read-more-button" onClick={handleShowLess}>
              Свернуть
            </button>
          )}
        </>
      );
    }
  };


  if (loading) return (
    <div className="recipe-page themealdb">
      <div className="recipe-header">
        <div className="recipe-actions">
          <button 
            className='update-button' 
            onClick={handleRefreshRecipe}
            disabled={loading}
          >
            {loading ? 'Загрузка...' : 'Обновить рецепт'}
          </button>
        </div>
        <div className="loading-placeholder">
          <div className="loading-spinner"></div>
          <p>Загрузка случайного рецепта...</p>
        </div>
      </div>
    </div>
  );

  
  if (!recipe) return <div className="recipe-page">Рецепт не найден</div>;

  return (
    <div className="recipe-page themealdb">
      <div className="recipe-header">
        <div className="recipe-actions">
          <button 
            className='update-button' 
            onClick={handleRefreshRecipe}
            disabled={loading}
          >
            {loading ? '...' : 'Обновить рецепт'}
          </button>
        </div>
        <img src={image || '/assets/default.png'} className="recipe-image" alt={recipe.name} />
        <div className="recipe-page-info">
          <div className="recipe-title-row">
            <h1>{recipe.name}</h1>
            <span className="themealdb-badge">TheMealDB</span>
          </div>
          <p className="recipe-weight">{recipe.weight ? `${recipe.weight} г` : ""}</p>
          
          <div className="recipe-instructions-summary">
            <h3>Инструкция</h3>
            {renderInstructions()}
          </div>
        </div>
      </div>
    </div>
  );
}