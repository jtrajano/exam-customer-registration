import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      // Whenever React asks for something starting with "/api"
      '/api': {
        target: 'http://localhost:5280', // Send it here!
        secure: false,
        changeOrigin: true
      }
    }
  }
})