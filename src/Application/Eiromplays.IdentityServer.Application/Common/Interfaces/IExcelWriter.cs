namespace Eiromplays.IdentityServer.Application.Common.Interfaces;

public interface IExcelWriter : ITransientService
{
    Stream WriteToStream<T>(IList<T> data);
}