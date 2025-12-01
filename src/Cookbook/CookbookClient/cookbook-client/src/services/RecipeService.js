import { api } from './api';

export const ingredientService = {
  // Получить рецепт по ID
  async getRecipeById(id) {
    const response = await api.get(`/cookbook/Recipe/${id}`);
    return response.data;
  },

  // Получить рецепт по названию
  async getRecipeByName(name) {
    const response = await api.post(`/cookbook/Recipe/search?ingredientName=${encodeURIComponent(name)}`);
    return response.data;
  },

  // Создать новый рецепт
  async createRecipe(ingredientData) {
    const response = await api.post('/cookbook/Recipe', ingredientData);
    return response.data;
  },

  async getRecipeImage(id) {
    try {
      const imageModule = await import(`../assets/recipes/${id}.jpg`);
      return imageModule.default;
    } catch (error) {
      const defaultImage = await import('../assets/recipes/default.jpg');
      return defaultImage.default;
    }
  }
};