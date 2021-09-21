using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eiromplays.IdentityServer.Application.Common.Interface
{
    public interface IIdentityServerPersistedGrantDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
