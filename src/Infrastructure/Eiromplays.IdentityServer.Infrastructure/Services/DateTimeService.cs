using Eiromplays.IdentityServer.Application.Common.Interfaces;

namespace Eiromplays.IdentityServer.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
    
    public DateTime UtcNow => DateTime.UtcNow;
}