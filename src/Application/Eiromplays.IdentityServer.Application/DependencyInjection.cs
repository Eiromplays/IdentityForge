using System.Reflection;
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
        Profile Picture Configuration:
        Find more avatar styles here: https://avatars.dicebear.com/styles/
        You can also use a custom provider
    */
    public static void RegisterAccountConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var accountConfiguration = configuration.GetSection(nameof(AccountConfiguration))
            .Get<AccountConfiguration>();

        services.AddSingleton(accountConfiguration);
    }
}