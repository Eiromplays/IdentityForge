namespace IdentityForge.IdentityServer.Services;

public interface IDateTimeProvider
{
    DateTimeOffset DateTimeUtcNow { get; }
    DateOnly DateOnlyUtcNow { get; }
}

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset DateTimeUtcNow => DateTimeOffset.UtcNow;
    public DateOnly DateOnlyUtcNow => DateOnly.FromDateTime(DateTimeUtcNow.DateTime);
}