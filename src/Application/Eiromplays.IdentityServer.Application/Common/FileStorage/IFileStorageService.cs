namespace Eiromplays.IdentityServer.Application.Common.FileStorage;

public interface IFileStorageService : ITransientService
{
    public Task<string> UploadAsync<T>(FileUploadRequest? request, FileType supportedFileType, CancellationToken cancellationToken = default)
        where T : class;

    public void Remove(string? path, CancellationToken cancellationToken = default);

    public Task RemoveAsync(string? path, CancellationToken cancellationToken = default);
}