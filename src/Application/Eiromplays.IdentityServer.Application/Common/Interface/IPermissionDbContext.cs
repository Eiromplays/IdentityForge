using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eiromplays.IdentityServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Application.Common.Interface
{
    public interface IPermissionDbContext
    {
        DbSet<Permission>? Permissions {  get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
