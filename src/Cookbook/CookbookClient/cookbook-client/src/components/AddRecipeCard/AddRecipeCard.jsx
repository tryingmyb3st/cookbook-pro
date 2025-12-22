import { useState } from 'react';
import { RecipeService } from '../../services/RecipeService';
import { ingredientService } from '../../services/ingredientService';
import './AddRecipeCard.css';
import UploadImage from '../UploadImage/UploadImage';

export default function AddRecipeCard({ onCancel, onSuccess }) {
  const [formData, setFormData] = useState({
    name: '',
    servingsNumber: 1,
    instruction: '',
    ingredients: [{ ingredientName: '', weight: 0 }],
    fileName: ''
  });
  const [loading, setLoading] = useState(false);
  const [ingredientSuggestions, setIngredientSuggestions] = useState([]);
  const [searchTimeout, setSearchTimeout] = useState(null);
  const [activeSuggestionIndex, setActiveSuggestionIndex] = useState(-1);

  const handleImageUploadSuccess = (response, file) => {    
    setFormData(prev => ({
      ...prev,
      fileName: response
    }));
  };

  const handleImageUploadError = (error) => {
    console.error('Ошибка загрузки изображения:', error);
    alert(`Ошибка загрузки изображения: ${error.message}`);
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: name === 'servingsNumber' ? parseInt(value) || 0 : value
    });
  };

  const handleIngredientChange = (index, field, value) => {
    const updatedIngredients = [...formData.ingredients];
    
    if (field === 'ingredientName') {
      updatedIngredients[index][field] = value;
      updatedIngredients[index].ingredientId = null;
      
      if (searchTimeout) {
        clearTimeout(searchTimeout);
      }
      
      if (value.trim().length >= 2) {
        const timeout = setTimeout(async () => {
          try {
            const suggestions = await ingredientService.getIngredientByName(value);
            setIngredientSuggestions(suggestions || []);
            setActiveSuggestionIndex(-1);
          } catch (error) {
            console.error('Error fetching ingredient suggestions:', error);
            setIngredientSuggestions([]);
          }
        }, 300);
        setSearchTimeout(timeout);
      } else {
        setIngredientSuggestions([]);
      }
    } else if (field === 'weight') {
      updatedIngredients[index][field] = parseInt(value) || 0;
    }
    
    setFormData({
      ...formData,
      ingredients: updatedIngredients
    });
  };

  const handleAddIngredient = () => {
    setFormData({
      ...formData,
      ingredients: [...formData.ingredients, { ingredientName: '', weight: 0 }]
    });
    setIngredientSuggestions([]);
  };

  const handleRemoveIngredient = (index) => {
    if (formData.ingredients.length > 1) {
      const updatedIngredients = formData.ingredients.filter((_, i) => i !== index);
      setFormData({
        ...formData,
        ingredients: updatedIngredients
      });
      setIngredientSuggestions([]);
    }
  };

  const handleSuggestionClick = (suggestion, index) => {
    const updatedIngredients = [...formData.ingredients];
    updatedIngredients[index].ingredientName = suggestion.name;
    updatedIngredients[index].ingredientId = suggestion.id;
    
    setFormData({
      ...formData,
      ingredients: updatedIngredients
    });
    setIngredientSuggestions([]);
    setActiveSuggestionIndex(-1);
  };

  const handleKeyDown = (e, index) => {
    if (ingredientSuggestions.length === 0) return;

    switch (e.key) {
      case 'ArrowDown':
        e.preventDefault();
        setActiveSuggestionIndex(prev => 
          prev < ingredientSuggestions.length - 1 ? prev + 1 : prev
        );
        break;
      case 'ArrowUp':
        e.preventDefault();
        setActiveSuggestionIndex(prev => prev > 0 ? prev - 1 : 0);
        break;
      case 'Enter':
        e.preventDefault();
        if (activeSuggestionIndex >= 0) {
          handleSuggestionClick(ingredientSuggestions[activeSuggestionIndex], index);
        }
        break;
      case 'Escape':
        setIngredientSuggestions([]);
        setActiveSuggestionIndex(-1);
        break;
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!formData.name.trim()) {
      alert('Пожалуйста, введите название рецепта');
      return;
    }
    
    if (!formData.instruction.trim()) {
      alert('Пожалуйста, введите инструкцию приготовления');
      return;
    }
    
    for (const ingredient of formData.ingredients) {
      if (!ingredient.ingredientName.trim()) {
        alert('Пожалуйста, заполните все названия ингредиентов');
        return;
      }
      if (ingredient.weight <= 0) {
        alert('Пожалуйста, укажите вес для всех ингредиентов');
        return;
      }
    }

    setLoading(true);

    try {
      const ingredientsToSend = await Promise.all(
        formData.ingredients.map(async (ingredient) => {
          if (ingredient.ingredientId) {
            return {
              ingredientId: ingredient.ingredientId,
              weight: ingredient.weight
            };
          }
          
          try {
            const existingIngredients = await ingredientService.getIngredientByName(ingredient.ingredientName.trim());
          
            if (existingIngredients && existingIngredients.data.length > 0) {
              return {
                ingredientId: existingIngredients.data[0].id,
                weight: ingredient.weight
              };
            } else {
              const newIngredient = await ingredientService.createIngredient({
                name: ingredient.ingredientName.trim(),
                protein: 0,
                fats: 0,
                carbs: 0,
                calories: 0,
              });

              return {
                ingredientId: newIngredient.data,
                weight: ingredient.weight
              };
            }
          } catch (error) {
            console.error('Error processing ingredient:', error);
            throw new Error(`Ошибка при обработке ингредиента "${ingredient.ingredientName}": ${error.message}`);
          }
        })
      );

      const userData = localStorage.getItem('user');
      let userId = null;
      if (userData) {
        try {
          const user = JSON.parse(userData);
          userId = user.id;
        } catch (error) {
          console.error('Error parsing user data:', error);
        }
      }

      const recipeData = {
        name: formData.name.trim(),
        servingsNumber: formData.servingsNumber,
        instruction: formData.instruction.trim(),
        ingredients: ingredientsToSend,
        ...(formData.fileName && { fileName: formData.fileName }),
        userId: userId 
      };

      console.log('Отправляемые данные рецепта:', recipeData);
      const result = await RecipeService.createRecipe(recipeData);
      
      setFormData({
        name: '',
        servingsNumber: 1,
        instruction: '',
        ingredients: [{ ingredientName: '', weight: 0 }],
        fileName: ''
      });
      
      if (onSuccess) {
        onSuccess(result);
      }      
    } catch (error) {
      console.error('Error creating recipe:', error);
      alert('Ошибка при создании рецепта: ' + (error.response?.data?.message || error.message));
    } finally {
      setLoading(false);
    }
  };

  const handleCancel = () => {
    if (onCancel) {
      onCancel();
    }
  };

  return (
    <div className="add-recipe-card">
      <div className="recipe-form-container">
        <h2>Добавить новый рецепт</h2>

        <div className="form-group">
          <UploadImage 
            onUploadSuccess={handleImageUploadSuccess}
            onUploadError={handleImageUploadError}
            buttonText="Загрузить изображение"
            maxSizeMB={2}
          />
        </div>

        <form onSubmit={handleSubmit} className="recipe-form">
          <div className="form-group">
            <label htmlFor="name">Название рецепта</label>
            <input
              type="text"
              id="name"
              name="name"
              value={formData.name}
              onChange={handleChange}
              required
              placeholder="Введите название рецепта"
              disabled={loading}
            />
          </div>

          <div className="form-group">
            <label htmlFor="servingsNumber">Количество порций</label>
            <input
              type="number"
              id="servingsNumber"
              name="servingsNumber"
              value={formData.servingsNumber}
              onChange={handleChange}
              min="1"
              required
              disabled={loading}
            />
          </div>

          <div className="form-group">
            <label htmlFor="instruction">Инструкция приготовления</label>
            <textarea
              id="instruction"
              name="instruction"
              value={formData.instruction}
              onChange={handleChange}
              required
              rows="6"
              placeholder="Опишите шаги приготовления"
              disabled={loading}
            />
          </div>

          <div className="form-group">
            <label>Ингредиенты</label>
            <div className="ingredients-list">
              {formData.ingredients.map((ingredient, index) => (
                <div key={index} className="ingredient-row">
                  <div className="ingredient-input-group">
                    <input
                      type="text"
                      placeholder="Название ингредиента"
                      value={ingredient.ingredientName}
                      onChange={(e) => handleIngredientChange(index, 'ingredientName', e.target.value)}
                      onKeyDown={(e) => handleKeyDown(e, index)}
                      required
                      disabled={loading}
                    />
                    {ingredientSuggestions.length > 0 && (
                      <div className="suggestions-dropdown">
                        {ingredientSuggestions.map((suggestion, idx) => (
                          <div
                            key={idx}
                            className={`suggestion-item ${idx === activeSuggestionIndex ? 'active' : ''}`}
                            onClick={() => handleSuggestionClick(suggestion, index)}
                          >
                            {suggestion.name}
                          </div>
                        ))}
                      </div>
                    )}
                  </div>
                  <div className="weight-input-group">
                    <input
                      type="number"
                      placeholder="Вес"
                      value={ingredient.weight}
                      onChange={(e) => handleIngredientChange(index, 'weight', e.target.value)}
                      min="0"
                      required
                      disabled={loading}
                    />
                    <span className="unit">г</span>
                  </div>
                  {formData.ingredients.length > 1 && (
                    <button
                      type="button"
                      className="remove-ingredient"
                      onClick={() => handleRemoveIngredient(index)}
                      disabled={loading}
                      title="Удалить ингредиент"
                    >
                      ×
                    </button>
                  )}
                </div>
              ))}
            </div>
            <button
              type="button"
              className="add-ingredient"
              onClick={handleAddIngredient}
              disabled={loading}
            >
              + Добавить ингредиент
            </button>
          </div>

          <div className="form-footer">
            <div className="form-buttons">
              <button
                type="button"
                className="cancel-button"
                onClick={handleCancel}
                disabled={loading}
              >
                Отмена
              </button>
              <button
                type="submit"
                className="submit-button"
                disabled={loading}
              >
                Создать рецепт
              </button>
            </div>
          </div>
        </form>
      </div>
    </div>
  );
}