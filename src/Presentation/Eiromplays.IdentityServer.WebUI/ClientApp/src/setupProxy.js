const { createProxyMiddleware } = require("http-proxy-middleware");
const { env } = require("process");

const target = env.ASPNETCORE_HTTPS_PORT
  ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
  : env.ASPNETCORE_URLS
  ? env.ASPNETCORE_URLS.split(";")[0]
  : "https://localhost:7001";

const context = ["/users/self-register", "/roles", "/bff", "/signin-oidc", "/signout-callback-oidc", "/personal", "/user-sessions", "/logs"];

module.exports = function (app) {
  const appProxy = createProxyMiddleware(context, {
    target: target,
    secure: false,
    logLevel: 'debug',
  });

  app.use(appProxy);
};
