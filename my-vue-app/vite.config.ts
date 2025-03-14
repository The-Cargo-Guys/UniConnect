import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5258', // ✅ Ensure this matches API port
        changeOrigin: true,
        secure: false
      }
    }
  }
});
