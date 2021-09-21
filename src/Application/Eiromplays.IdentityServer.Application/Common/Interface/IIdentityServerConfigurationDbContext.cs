using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Application.Common.Interface
{
    public interface IIdentityServerConfigurationDbContext<TApiResourceSecret>
        where TApiResourceSecret : class
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DbSet<TApiResourceSecret>? ApiSecrets { get; set; }
    }
}
