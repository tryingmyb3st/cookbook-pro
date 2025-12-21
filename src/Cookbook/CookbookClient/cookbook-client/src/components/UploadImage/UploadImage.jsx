import React, { useState, useRef } from 'react';
import { FileService } from '../../services/FileService';
import './UploadImage.css';

const UploadImage = ({ 
  onUploadSuccess,
  onUploadError,
  buttonText = "Загрузить изображение",
  accept = "image/*",
  maxSizeMB = 5,
  className = ""
}) => {
  const [selectedFile, setSelectedFile] = useState(null);
  const [uploading, setUploading] = useState(false);
  const [error, setError] = useState('');
  const [uploadedFileName, setUploadedFileName] = useState('');
  const fileInputRef = useRef(null);

  const handleFileSelect = (e) => {
    const file = e.target.files[0];
    if (!file) return;

    setError('');
    setUploadedFileName('');
    
    if (!file.type.startsWith('image/')) {
      setError('Пожалуйста, выберите изображение');
      return;
    }

    const maxSize = maxSizeMB * 1024 * 1024;
    if (file.size > maxSize) {
      setError(`Файл слишком большой. Максимальный размер: ${maxSizeMB}MB`);
      return;
    }

    setSelectedFile(file);
  };

  const handleButtonClick = () => {
    fileInputRef.current.click();
  };

  const handleUpload = async () => {
    if (!selectedFile) {
      setError('Пожалуйста, выберите файл');
      return;
    }

    setUploading(true);
    setError('');

    try {
      const result = await FileService.uploadImage(selectedFile);
      
      const fileName = result?.fileName || selectedFile.name;
      setUploadedFileName(fileName);
      
      if (onUploadSuccess) {
        onUploadSuccess(fileName, selectedFile);
      }

      setSelectedFile(null);
      
    } catch (error) {
      console.error('Ошибка загрузки:', error);
      const errorMessage = error.response?.data?.message || error.message || 'Ошибка загрузки файла';
      setError(errorMessage);
      
      if (onUploadError) {
        onUploadError(error);
      }
    } finally {
      setUploading(false);
    }
  };

  const handleRemoveFile = () => {
    setSelectedFile(null);
    setUploadedFileName('');
    setError('');
    if (fileInputRef.current) {
      fileInputRef.current.value = '';
    }
  };

  const formatFileSize = (bytes) => {
    if (bytes < 1024) return bytes + ' байт';
    if (bytes < 1024 * 1024) return (bytes / 1024).toFixed(1) + ' KB';
    return (bytes / (1024 * 1024)).toFixed(1) + ' MB';
  };

  return (
    <div className={`upload-image ${className}`}>
      <input
        type="file"
        ref={fileInputRef}
        onChange={handleFileSelect}
        accept={accept}
        style={{ display: 'none' }}
        disabled={uploading}
      />

      {!uploadedFileName && !selectedFile && (
        <button
          type="button"
          onClick={handleButtonClick}
          disabled={uploading}
          className="upload-image__button"
        >
          {uploading ? 'Загрузка...' : buttonText}
        </button>
      )}

      {selectedFile && (
        <div className="upload-image__file-info">
          <div className="upload-image__file-details">
            <span className="upload-image__file-name">{selectedFile.name}</span>
            <span className="upload-image__file-size">({formatFileSize(selectedFile.size)})</span>
          </div>
          
          <div className="upload-image__actions">
            <button
              type="button"
              onClick={handleUpload}
              disabled={uploading}
              className="upload-image__upload-btn"
            >
              {uploading ? 'Загрузка...' : 'Загрузить'}
            </button>
            <button
              type="button"
              onClick={handleRemoveFile}
              disabled={uploading}
              className="upload-image__remove-btn"
              title="Удалить файл"
            >
              ×
            </button>
          </div>
        </div>
      )}

      {uploadedFileName && !selectedFile && (
        <div className="upload-image__uploaded-info">
          <span className="upload-image__uploaded-name">✓ {uploadedFileName}</span>
          <button
            type="button"
            onClick={handleRemoveFile}
            className="upload-image__remove-btn"
            title="Удалить файл"
          >
            ×
          </button>
        </div>
      )}

      {error && (
        <div className="upload-image__error">
          {error}
        </div>
      )}
    </div>
  );
};

export default UploadImage;