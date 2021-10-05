using System;
using System.Threading.Tasks;
using Eiromplays.IdentityServer.Application.Common.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Permissions
{
    internal class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options, IServiceScopeFactory serviceScopeFactory)
        {
            // There can only be one policy provider in ASP.NET Core.
            // We only handle permissions related policies.
            // We will use the default provider for the rest.
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            _serviceScopeFactory = serviceScopeFactory;
        }

        // Dynamically creates a policy with a requirement that contains the permission.
        // The policy name must match the permission that is needed.
        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var identityService = scope.ServiceProvider.GetService<IIdentityService>();

            if (identityService == null) return await FallbackPolicyProvider.GetPolicyAsync(policyName);

            var policy = new AuthorizationPolicyBuilder();

            return await Task.FromResult(policy.Build());
        }

        public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return await FallbackPolicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
            FallbackPolicyProvider.GetDefaultPolicyAsync();
    }
}