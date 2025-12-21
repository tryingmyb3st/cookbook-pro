import { api } from './api';

export const RecipeService = {
  // Получить рецепт по ID
  async getRecipeById(id) {
    const response = await api.get(`/cookbook/Recipe/Get?id=${encodeURIComponent(id)}`);
    return response.data;
  },

  // Получить рецепты по названию
  async getRecipeByName(name) {
    const response = await api.get(`/cookbook/Recipe/Search?name=${encodeURIComponent(name)}`);
    return response.data;
  },

  // Создать новый рецепт
  async createRecipe(recipeData) {
    const response = await api.post('/cookbook/Recipe/Create', recipeData);
    return response.data;
  },

  async getRandomFromTheMealDB() {
    try {
      const response = await api.get('/cookbook/Recipe/GetRandomFromTheMealDB');
      return response.data; 
    } catch (error) {
      throw error;
  }
}
};