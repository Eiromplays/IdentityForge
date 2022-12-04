using Eiromplays.IdentityServer.Domain.Enums;
using Eiromplays.IdentityServer.Infrastructure;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment, ProjectType.Spa);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
    options.DefaultSignOutScheme = "oidc";
}).AddCookie("cookie", options =>
{
    options.Cookie.Name = "__Host-bff";
    options.Cookie.SameSite = SameSiteMode.Strict;
}).AddOpenIdConnect("oidc", options =>
{
    options.Authority = "http://auth.eiromplays.local.com";
    options.ClientId = "eiromplays_identity_admin_spa";
    options.ClientSecret = "secret";
    options.ResponseType = "code";
    options.ResponseMode = "query";

    options.GetClaimsFromUserInfoEndpoint = true;
    options.MapInboundClaims = false;
    options.SaveTokens = true;

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("api");
    options.Scope.Add("offline_access");
    options.Scope.Add("roles");
    options.Scope.Add("email");

    options.ClaimActions.MapJsonKey("role", "role", "role");
    options.ClaimActions.MapJsonKey("picture", "picture", "picture");
    options.ClaimActions.MapJsonKey("gravatar_email", "gravatar_email", "gravatar_email");
    options.ClaimActions.MapJsonKey("updated_at", "updated_at", "updated_at");
    options.ClaimActions.MapJsonKey("created_at", "created_at", "created_at");
    options.ClaimActions.MapUniqueJsonKey("given_name", "given_name", "given_name");
    options.ClaimActions.MapUniqueJsonKey("family_name", "family_name", "family_name");
});

var app = builder.Build();

await app.Services.InitializeDatabasesAsync();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");

    // The default HSTS0+ value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseBff();

app.UseAuthorization();

app.MapBffManagementEndpoints();

app.MapRemoteBffApiEndpoint("/users", "http://api.eiromplays.local.com/v1/users")
    .RequireAccessToken();

app.MapRemoteBffApiEndpoint("/roles", "http://api.eiromplays.local.com/v1/roles")
    .RequireAccessToken();

app.MapRemoteBffApiEndpoint("/personal", "http://api.eiromplays.local.com/v1/personal")
    .RequireAccessToken();

app.MapRemoteBffApiEndpoint("/user-sessions", "http://api.eiromplays.local.com/v1/user-sessions")
    .RequireAccessToken();

app.MapRemoteBffApiEndpoint("/server-side-sessions", "http://api.eiromplays.local.com/v1/server-side-sessions")
    .RequireAccessToken();

app.MapRemoteBffApiEndpoint("/logs", "http://api.eiromplays.local.com/v1/logs")
    .RequireAccessToken();

app.MapRemoteBffApiEndpoint("/persisted-grants", "http://api.eiromplays.local.com/v1/persisted-grants")
    .RequireAccessToken();

app.MapRemoteBffApiEndpoint("/dashboard", "http://api.eiromplays.local.com/v1/dashboard")
    .RequireAccessToken();

app.MapRemoteBffApiEndpoint("/clients", "http://api.eiromplays.local.com/v1/clients")
    .RequireAccessToken();

app.MapRemoteBffApiEndpoint("/identity-resources", "http://api.eiromplays.local.com/v1/identity-resources")
    .RequireAccessToken();

app.MapRemoteBffApiEndpoint("/api-scopes", "http://api.eiromplays.local.com/v1/api-scopes")
    .RequireAccessToken();

app.MapRemoteBffApiEndpoint("/api-resources", "http://api.eiromplays.local.com/v1/api-resources")
    .RequireAccessToken();

app.MapRemoteBffApiEndpoint("/products", "http://api.eiromplays.local.com/v1/products")
    .RequireAccessToken();

app.MapRemoteBffApiEndpoint("/brands", "http://api.eiromplays.local.com/v1/brands")
    .RequireAccessToken();

app.MapFallbackToFile("index.html");

app.Run();