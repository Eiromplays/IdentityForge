using Eiromplays.IdentityServer.Domain.Common.Contracts;

namespace Eiromplays.IdentityServer.Infrastructure.Auditing;

public class Trail : BaseEntity
{
    public string? UserId { get; set; }
    public string? Type { get; set; }
    public string? TableName { get; set; }
    public DateTime DateTime { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? AffectedColumns { get; set; }
    public string? PrimaryKey { get; set; }
}