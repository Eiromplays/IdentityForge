namespace Eiromplays.IdentityServer.Application.Dashboard;

public class StatsDto
{
    public int UserCount { get; set; }
    public int RoleCount { get; set; }
    public int ClientCount { get; set; }
    public int IdentityResourceCount { get; set; }
    public int ApiResourceCount { get; set; }
    public int ApiScopeCount { get; set; }

    public List<ChartSeries> DataEnterBarChart { get; set; } = new();
    public Dictionary<string, double>? ProductByBrandTypePieChart { get; set; }
}

public class ChartSeries
{
    public string? Name { get; set; }
    public double[]? Data { get; set; }
}