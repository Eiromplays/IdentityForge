// vite proxy table example

import { env } from "process";

const target = env.ASPNETCORE_HTTPS_PORT
  ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
  : env.ASPNETCORE_URLS
  ? env.ASPNETCORE_URLS.split(";")[0]
  : "https://localhost:7001";

module.exports = {
    dev: {
      "/users": {
        target: target,
        rewrite: (path: any) => path.replace(/^\/users/, ""),
      },
      "/roles": {
        target: target,
        rewrite: (path: any) => path.replace(/^\/roles/, ""),
      },
      "/bff": {
        target: target,
        rewrite: (path: any) => path.replace(/^\/bff/, ""),
      },
      "/signin-oidc": {
        target: target,
        rewrite: (path: any) => path.replace(/^\/signin-oidc/, ""),
      },
      "/signout-callback-oidc": {
        target: target,
        rewrite: (path: any) => path.replace(/^\/signout-callback-oidc/, ""),
      },
      "/personal": {
        target: target,
        rewrite: (path: any) => path.replace(/^\/personal/, ""),
      },
      "/user-sessions": {
        target: target,
        rewrite: (path: any) => path.replace(/^\/user-sessions/, ""),
      },
      "/logs": {
        target: target,
        rewrite: (path: any) => path.replace(/^\/logs/, ""),
      },
      "/persisted-grants": {
        target: target,
        rewrite: (path: any) => path.replace(/^\/persisted-grants/, ""),
      }
    },
    test: {
      "/test": {
        target: target,
        rewrite: (path: any) => path.replace(/^\/admin/, ""),
      },
    },
    gray: {
      "/gray": {
        target: target,
        rewrite: (path: any) => path.replace(/^\/admin/, ""),
      },
    },
    prod: {
      "/prod": {
        target: target,
        rewrite: (path: any) => path.replace(/^\/admin/, ""),
      },
    },
};