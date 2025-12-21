import { api } from './api';

export class AuthService {
  static async getUserInfo(userId) {
    const response = await api.get(`/cookbook/Auth/Get?userId=${encodeURIComponent(userId)}`);
    return response.data;
  }
}