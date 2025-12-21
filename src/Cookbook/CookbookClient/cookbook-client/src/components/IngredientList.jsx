import { useState, useEffect } from 'react';
import { ingredientService } from '../services/ingredientService';

function IngredientList() {
  const [ingredient, setIngredient] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    loadIngredient();
  }, []);

  const loadIngredient = async () => {
    try {
      console.log('Loading ingredient with ID 1...');
      const data = await ingredientService.getIngredientById(3);
      console.log('Received ingredient data:', data);
      setIngredient(data);
    } catch (err) {
      console.error('API Error:', err);
      setError('Ошибка при загрузке ингредиента: ' + (err.response?.data || err.message));
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div>Загрузка ингредиента...</div>;
  if (error) return <div>Ошибка: {error}</div>;
  if (!ingredient) return <div>Ингредиент не найден</div>;

  return (
    <div>
      <h2>Ингредиент</h2>
      <div style={{ 
        border: '1px solid #ddd',
        borderRadius: '8px',
        padding: '20px',
        backgroundColor: '#f9f9f9',
        maxWidth: '400px'
      }}>
        <h3 style={{ margin: '0 0 15px 0', color: '#333' }}>
          {ingredient.name}
        </h3>
        <div style={{ display: 'flex', flexDirection: 'column', gap: '8px' }}>
          <div><strong>ID:</strong> {ingredient.id}</div>
          <div><strong>Белки:</strong> {ingredient.protein || 0}g</div>
          <div><strong>Жиры:</strong> {ingredient.fats || 0}g</div>
          <div><strong>Углеводы:</strong> {ingredient.carbs || 0}g</div>
          <div><strong>Калории:</strong> {ingredient.calories || 0} kcal</div>
        </div>
      </div>
    </div>
  );
}

export default IngredientList;