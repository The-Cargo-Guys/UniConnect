import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import path from "path";

export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src"), // ✅ Ensures @ is a valid alias for src/
    },
  },
  server: {
    host: "localhost",
    port: 5173, // ✅ Ensure this matches your Vue frontend port
    strictPort: true,
    open: true, // ✅ Auto-opens the browser when starting the dev server
    cors: true, // ✅ Enables CORS
    proxy: {
      "/api": {
        target: "http://localhost:5258", // ✅ Matches your ASP.NET Core backend port
        changeOrigin: true,
        secure: false, // ✅ Allows self-signed SSL certs (if needed)
        ws: true, // ✅ Supports WebSockets (useful for real-time features)
        rewrite: (path) => path.replace(/^\/api/, ""), // ✅ Removes /api prefix when forwarding
      },
    },
  },
});
