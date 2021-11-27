using Eiromplays.IdentityServer.Application.Common.Interface;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest, TUserDto, TRoleDto> : IRequestPreProcessor<TRequest>
        where TRequest : class
        where TUserDto : class
        where TRoleDto : class
    {
        private readonly ILogger _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService<TUserDto, TRoleDto> _identityService;

        public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService,
            IIdentityService<TUserDto, TRoleDto> identityService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId;
            var userName = "";

            if (!string.IsNullOrEmpty(userId))
            {
                userName = await _identityService.GetUserNameAsync(userId);
            }

            _logger.LogInformation("Eiromplays.IdentityServer Request: {Name} {@UserId} {@UserName} {@Request}",
                requestName, userId, userName, request);
        }
    }
}
