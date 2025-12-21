import { api } from './api';

export class FileService {
  static async getImage(fileName) {
    try {
      const response = await api.get(`/cookbook/File/Download?fileName=${encodeURIComponent(fileName)}`, {
        responseType: 'blob'
      });

      return response.data;
    } catch (error) {
      console.error('Error loading image:', error);
      
      if (error.response?.status === 401) {
        console.error('Ошибка авторизации при загрузке изображения');
      }
      
      throw error;
    }
  }
  
  static async uploadImage(file) {
    try {
      const formData = new FormData();
      formData.append('file', file);
      
      const response = await api.post('/cookbook/File/Upload', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        }
      });

      return response.data;
    } catch (error) {
      console.error('Error uploading image:', error);
      throw error;
    }
  }

}