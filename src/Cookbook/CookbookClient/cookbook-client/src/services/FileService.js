import { api } from './api';

export const FileService = {
  async getImage(fileName) {
    try {
      const response = await api.get(`/cookbook/File/Download?fileName=${encodeURIComponent(fileName)}`, {
        responseType: 'blob'
      });
      
      return response.data;
      
    } catch (error) {
      throw error;
    }
  }
};