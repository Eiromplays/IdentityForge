using Eiromplays.IdentityServer.Application.Dashboard;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Dashboard.GetStats;

public class Models
{
    public class Request
    {

    }

    public class Response
    {
        public StatsDto Stats { get; set; }
    }
}