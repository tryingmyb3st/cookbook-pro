import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/cookbook': {
        target: 'http://localhost:5144', 
        changeOrigin: true,
        secure: false,
      }
    }
  }
})