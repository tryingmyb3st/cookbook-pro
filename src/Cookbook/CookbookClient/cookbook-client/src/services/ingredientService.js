import { api } from './api';

export const ingredientService = {
  // Получить ингредиент по ID
  async getIngredientById(id) {
    const response = await api.get(`/cookbook/Ingredient/Get?=${encodeURIComponent(id)}`);
    return response.data;
  },

  // Получить ингредиент по названию
  async getIngredientByName(name) {
    const response = await api.post(`/cookbook/Ingredient/Search?name=${encodeURIComponent(name)}`);
    return response.data;
  },

  // Создать новый ингредиент
  async createIngredient(ingredientData) {
    const response = await api.post('/cookbook/Ingredient/Create', ingredientData);
    return response.data;
  }
};