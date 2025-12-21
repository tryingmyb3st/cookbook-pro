import { api } from './api';

export const ingredientService = {
  // Получить ингредиент по ID
  async getIngredientById(id) {
    return await api.get(`/cookbook/Ingredient/Get?id=${encodeURIComponent(id)}`);
  },

  // Получить ингредиент по названию
  async getIngredientByName(name) {
    return await api.get(`/cookbook/Ingredient/Search?name=${encodeURIComponent(name)}`);
  },

  // Создать новый ингредиент
  async createIngredient(ingredientData) {
    return await api.post('/cookbook/Ingredient/Create', ingredientData);
  }
};