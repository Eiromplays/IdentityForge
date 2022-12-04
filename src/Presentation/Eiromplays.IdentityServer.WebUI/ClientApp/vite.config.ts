import { resolve } from 'path';
import { env } from 'process';

import react from '@vitejs/plugin-react';
import { defineConfig } from 'vite';
import envCompatible from 'vite-plugin-env-compatible';
import tsconfigPaths from 'vite-tsconfig-paths';

const target = process.env.PROXY_TARGET || 'http://localhost:80';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react(), tsconfigPaths(), /*mkcert(),*/ envCompatible()],
  resolve: {
    alias: {
      '@': resolve(__dirname, 'src'),
    },
  },
  server: {
    port: 3000,
    https: false,
    proxy: {
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
    },
  },
});
