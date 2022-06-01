import { resolve } from 'path';
import { env } from 'process';

import react from '@vitejs/plugin-react';
import { defineConfig } from 'vite';
import envCompatible from 'vite-plugin-env-compatible';
import mkcert from 'vite-plugin-mkcert';
import tsconfigPaths from 'vite-tsconfig-paths';

const target = env.ASPNETCORE_HTTPS_PORT
  ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
  : env.ASPNETCORE_URLS
  ? env.ASPNETCORE_URLS.split(';')[0]
  : 'https://localhost:7001';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react(), tsconfigPaths(), mkcert(), envCompatible()],
  resolve: {
    alias: {
      '@': resolve(__dirname, 'src'),
    },
  },
  server: {
    port: 3000,
    https: true,
    proxy: {
      '/users': {
        target: target,
        changeOrigin: false,
        secure: false,
      },
      '/roles': {
        target: target,
        changeOrigin: false,
        secure: false,
      },
      '/bff': {
        target: target,
        changeOrigin: false,
        secure: false,
      },
      '/signin-oidc': {
        target: target,
        changeOrigin: false,
        secure: false,
      },
      '/signout-callback-oidc': {
        target: target,
        changeOrigin: false,
        secure: false,
      },
      '/personal': {
        target: target,
        changeOrigin: false,
        secure: false,
      },
      '/user-sessions': {
        target: target,
        changeOrigin: false,
        secure: false,
      },
      '/logs': {
        target: target,
        changeOrigin: false,
        secure: false,
      },
      '/persisted-grants': {
        target: target,
        changeOrigin: false,
        secure: false,
      },
      '/external-logins': {
        target: target,
        changeOrigin: false,
        secure: false,
      },
    },
  },
});
