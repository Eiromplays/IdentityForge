using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eiromplays.IdentityServer.Application.Common.Interface;
using Eiromplays.IdentityServer.Application.Common.Mappings;
using Eiromplays.IdentityServer.Application.Common.Models;
using MediatR;

namespace Eiromplays.IdentityServer.Application.Permissions.Queries.GetPermissionsWithPagination
{
    public class GetPermissionsWithPaginationQuery<TPermissionDto, TKey> : IRequest<PaginatedList<TPermissionDto>>
        where TPermissionDto : PermissionDto<TKey>
    {
        public string Name { get; set; } = "";

        public int PageNumber {  get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }

    public class GetPermissionsWithPaginationQueryHandler<TPermissionDto, TKey> : IRequestHandler<GetPermissionsWithPaginationQuery<TPermissionDto, TKey>,
            PaginatedList<TPermissionDto>>
        where TPermissionDto : PermissionDto<TKey>
    {
        private readonly IPermissionDbContext _context;
        private readonly IMapper _mapper;

        public GetPermissionsWithPaginationQueryHandler(IPermissionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TPermissionDto>> Handle(GetPermissionsWithPaginationQuery<TPermissionDto, TKey> request, CancellationToken cancellationToken)
        {
            return await _context.Permissions!
                .Where(x => !string.IsNullOrWhiteSpace(x.Name) &&
                            x.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase))
                .ProjectTo<TPermissionDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
