using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Application.Common.Interface
{
    public interface IIdentityDbContext<TUser>
        where TUser : class
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DbSet<TUser> Users { get; set; }
    }
}
