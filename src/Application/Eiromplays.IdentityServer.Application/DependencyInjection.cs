using System.Reflection;
using System.Text.Json;
using Eiromplays.IdentityServer.Application.Common.Behaviours;
using Eiromplays.IdentityServer.Application.Common.Configurations.Account;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eiromplays.IdentityServer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterAccountConfiguration(configuration);

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        return services;
    }

    /*
        Registers the Account Configuration
        Information:
        Profile Picture Configuration:
        Find more avatar styles here: https://avatars.dicebear.com/styles/
        You can also use a custom provider
    */
    private static void RegisterAccountConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AccountConfiguration>(configuration.GetSection(nameof(AccountConfiguration)));
    }
}