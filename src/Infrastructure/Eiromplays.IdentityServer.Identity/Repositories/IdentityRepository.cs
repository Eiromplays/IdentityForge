using AutoMapper;
using Eiromplays.IdentityServer.Infrastructure.Identity.Persistence.DbContexts;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Repositories
{
    public class IdentityRepository<TKey, TUser, TRole>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
    {
        private readonly IdentityDbContext _identityDbContext;

        private readonly UserManager<TUser> _userManager;
        private readonly RoleManager<TRole> _roleManager;

        private readonly IMapper _mapper;

        public IdentityRepository(IdentityDbContext identityDbContext, UserManager<TUser> userManager,
            RoleManager<TRole> roleManager, IMapper mapper)
        {
            _identityDbContext = identityDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
    }
}
