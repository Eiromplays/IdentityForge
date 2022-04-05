using Eiromplays.IdentityServer.Application.Identity.PersistedGrants;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.PersistedGrants.GetList;

public class Models
{
    public class Response
    {
        public List<PersistedGrantDto> PersistedGrants { get; set; }
    }
}