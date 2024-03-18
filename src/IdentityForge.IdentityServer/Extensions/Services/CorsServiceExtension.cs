namespace IdentityForge.IdentityServer.Extensions.Services;

public static class CorsServiceExtension
{
    public static void AddCorsService(this IServiceCollection services, string policyName, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.IsEnvironment(Consts.Testing.IntegrationTestingEnvName) ||
            env.IsEnvironment(Consts.Testing.FunctionalTestingEnvName))
        {
            services.AddCors(options =>
            {
                options.AddPolicy(policyName, builder =>
                    builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("X-Pagination"));
            });
        }
        else
        {
            //TODO update origins here with env vars or secret
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(policyName, builder =>
            //        builder.WithOrigins(origins)
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .WithExposedHeaders("X-Pagination"));
            //});
        }
    }
}