using System.Threading.Tasks;

namespace Eiromplays.IdentityServer.Application.Common.Interface
{
    public interface IIdentityService
    {
        Task<string?> GetUserNameAsync(string userId);
    }
}
