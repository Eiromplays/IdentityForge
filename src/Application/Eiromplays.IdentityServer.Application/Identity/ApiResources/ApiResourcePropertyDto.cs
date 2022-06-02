namespace Eiromplays.IdentityServer.Application.Identity.ApiResources;

public class ApiResourcePropertyDto
{
    public int Id { get; set; }
    
    public string Key { get; set; } = default!;

    public string Value { get; set; } = default!;
    
    public int ApiResourceId { get; set; }
}