import './IngredientCard.css';

export default function IngredientCard({ ingredient }) {
  return (
    <div className="ingredient-card">
      <h3>{ingredient.name}</h3>
      <p>{ingredient.amount} {ingredient.unit}</p>
    </div>
  );
}
