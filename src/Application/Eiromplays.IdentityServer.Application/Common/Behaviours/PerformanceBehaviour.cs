using Eiromplays.IdentityServer.Application.Common.Interface;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Eiromplays.IdentityServer.Application.Common.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse, TUserDto, TRoleDto> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
        where TUserDto : class
        where TRoleDto : class
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService<TUserDto, TRoleDto> _identityService;

        public PerformanceBehaviour(
            ILogger<TRequest> logger,
            ICurrentUserService currentUserService,
            IIdentityService<TUserDto, TRoleDto> identityService)
        {
            _timer = new Stopwatch();

            _logger = logger;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds <= 500) return response;

            var requestName = typeof(TRequest).Name;

            var userId = _currentUserService.UserId;

            var userName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                userName = await _identityService.GetUserNameAsync(userId);
            }

            _logger.LogWarning("Eiromplays.IdentityServer Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                requestName, elapsedMilliseconds, userId, userName, request);

            return response;
        }
    }
}
