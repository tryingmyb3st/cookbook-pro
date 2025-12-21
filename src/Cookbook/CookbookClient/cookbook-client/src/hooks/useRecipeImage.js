import { useState, useEffect, useRef } from 'react';
import { FileService } from '../services/FileService';

export const useRecipeImage = (recipe) => {
  const [image, setImage] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(false);
  const imageBlobUrlRef = useRef(null);

  useEffect(() => {
    const loadImage = async () => {
      try {
        setLoading(true);
        setError(false);
        
        if (recipe?.fileName?.startsWith('http')) {
          setImage(recipe.fileName);
          setLoading(false);
          return;
        }

        const fileName = recipe.fileName;

        const token = localStorage.getItem('token');
        
        const authHeaders = {};
        if (token) {
          authHeaders['Authorization'] = `Bearer ${token}`;
        }

        const imageBlob = await FileService.getImage(fileName, authHeaders);

        if (imageBlobUrlRef.current) {
          URL.revokeObjectURL(imageBlobUrlRef.current);
        }

        const imageUrl = URL.createObjectURL(imageBlob);
        imageBlobUrlRef.current = imageUrl;
        setImage(imageUrl);

      } catch (err) {
        console.log('Ошибка загрузки изображения:', err);
        setError(true);
        
        try {
          const defaultImage = await import('../assets/recipes/default.jpg');
          setImage(defaultImage.default);
        } catch {
          setImage(null);
        }
      } finally {
        setLoading(false);
      }
    };

    if (recipe) {
      loadImage();
    }

    return () => {
      if (imageBlobUrlRef.current) {
        URL.revokeObjectURL(imageBlobUrlRef.current);
        imageBlobUrlRef.current = null;
      }
    };
  }, [recipe]);

  return { image, loading, error };
};