import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { env } from 'process';
const path = require("path");

const target = env.ASPNETCORE_HTTPS_PORT
  ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
  : env.ASPNETCORE_URLS
  ? env.ASPNETCORE_URLS.split(";")[0]
  : "https://localhost:7001";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, 'src'),
    }
  },
  server: {
    port: 3001,
    https: true,
    proxy: {
      '/users': {
        target: target,
        changeOrigin: false,
        secure: false,
        ws: true,
      },
      '/roles': {
        target: target,
        changeOrigin: true,
        secure: false,
        ws: true,
      },
      '/bff': {
        target: target,
        changeOrigin: false,
        secure: false,
        ws: true,
      },
      '/signin-oidc': {
        target: target,
        changeOrigin: false,
        secure: false,
        ws: true,
      },
      '/signout-callback-oidc': {
        target: target,
        changeOrigin: false,
        secure: false,
        ws: true,
      },
      '/personal': {
        target: target,
        changeOrigin: true,
        secure: false,
        ws: true,
      },
      '/user-sessions': {
        target: target,
        changeOrigin: true,
        secure: false,
        ws: true,
      },
      '/logs': {
        target: target,
        changeOrigin: true,
        secure: false,
        ws: true,
      },
      '/persisted-grants': {
        target: target,
        changeOrigin: true,
        secure: false,
        ws: true,
      }
    }
  }
})
