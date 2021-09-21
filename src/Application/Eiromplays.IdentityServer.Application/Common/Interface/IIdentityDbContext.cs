using System.Threading;
using System.Threading.Tasks;

namespace Eiromplays.IdentityServer.Application.Common.Interface
{
    public interface IIdentityDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
