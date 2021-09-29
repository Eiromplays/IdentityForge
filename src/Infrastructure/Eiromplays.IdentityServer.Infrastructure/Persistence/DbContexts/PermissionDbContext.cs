using Eiromplays.IdentityServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Persistence.DbContexts
{
    public class PermissionDbContext : DbContext
    {
        public DbSet<Permission>? Permissions { get; set; }

        public PermissionDbContext(DbContextOptions<PermissionDbContext> options) : base(options)
        {

        }
    }
}