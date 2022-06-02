using Duende.IdentityServer;

namespace Eiromplays.IdentityServer.Application.Identity.Resources;

public class SecretDto
{
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the expiration.
    /// </summary>
    /// <value>
    /// The expiration.
    /// </value>
    public DateTime? Expiration { get; set; }

    /// <summary>
    /// Gets or sets the type of the client secret.
    /// </summary>
    /// <value>
    /// The type of the client secret.
    /// </value>
    public string Type { get; set; }


    public SecretDto()
    {
        Type = IdentityServerConstants.SecretTypes.SharedSecret;
    }
    
    public SecretDto(string value, DateTime? expiration = null)
        : this()
    {
        Value = value;
        Expiration = expiration;
    }
    
    public SecretDto(string value, string description, DateTime? expiration = null)
        : this()
    {
        Description = description;
        Value = value;
        Expiration = expiration;
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
    /// </returns>
    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + (Value?.GetHashCode() ?? 0);
            hash = hash * 23 + (Type?.GetHashCode() ?? 0);

            return hash;
        }
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not SecretDto other) return false;
        if (ReferenceEquals(other, this)) return true;

        return string.Equals(other.Type, Type, StringComparison.Ordinal) && 
               string.Equals(other.Value, Value, StringComparison.Ordinal);
    }
}