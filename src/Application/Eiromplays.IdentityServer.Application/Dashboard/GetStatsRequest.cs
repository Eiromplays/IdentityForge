using Eiromplays.IdentityServer.Application.Identity.Roles;
using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.Application.Dashboard;

public class GetStatsRequest : IRequest<StatsDto>
{
}

public class GetStatsRequestHandler : IRequestHandler<GetStatsRequest, StatsDto>
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly IReadRepository<Brand> _brandRepo;
    private readonly IReadRepository<Product> _productRepo;
    private readonly IStringLocalizer _t;

    public GetStatsRequestHandler(IUserService userService, IRoleService roleService, IReadRepository<Brand> brandRepo, IReadRepository<Product> productRepo, IStringLocalizer<GetStatsRequestHandler> localizer)
    {
        _userService = userService;
        _roleService = roleService;
        _brandRepo = brandRepo;
        _productRepo = productRepo;
        _t = localizer;
    }

    public async Task<StatsDto> Handle(GetStatsRequest request, CancellationToken cancellationToken)
    {
        var stats = new StatsDto
        {
            ProductCount = await _productRepo.CountAsync(cancellationToken),
            BrandCount = await _brandRepo.CountAsync(cancellationToken),
            UserCount = await _userService.GetCountAsync(cancellationToken),
            RoleCount = await _roleService.GetCountAsync(cancellationToken)
        };

        var selectedYear = DateTime.Now.Year;
        var productsFigure = new double[13];
        var brandsFigure = new double[13];
        
        for (var i = 1; i <= 12; i++)
        {
            var filterStartDate = new DateTime(selectedYear, i, 01);
            var filterEndDate =
                new DateTime(selectedYear, i, DateTime.DaysInMonth(selectedYear, i), 23, 59,
                    59); // Monthly Based

            var brandSpec = new AuditableEntitiesByCreatedOnBetweenSpec<Brand>(filterStartDate, filterEndDate);
            var productSpec = new AuditableEntitiesByCreatedOnBetweenSpec<Product>(filterStartDate, filterEndDate);

            brandsFigure[i - 1] = await _brandRepo.CountAsync(brandSpec, cancellationToken);
            productsFigure[i - 1] = await _productRepo.CountAsync(productSpec, cancellationToken);
        }

        stats.DataEnterBarChart.Add(new ChartSeries { Name = _t["Products"], Data = productsFigure });
        stats.DataEnterBarChart.Add(new ChartSeries { Name = _t["Brands"], Data = brandsFigure });

        return stats;
    }
}