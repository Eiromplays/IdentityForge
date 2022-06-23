namespace Eiromplays.IdentityServer.Application.Common.FileStorage;

public class FileUploadRequest
{
    public string Name { get; set; } = default!;
    public string Extension { get; set; } = default!;
    public string Data { get; set; } = default!;
}

public class FileUploadRequestValidator : Validator<FileUploadRequest>
{
    public FileUploadRequestValidator()
    {
        var T = TryResolve<IStringLocalizer<FileUploadRequestValidator>>() ??
                new StringLocalizer<FileUploadRequestValidator>(Resolve<IStringLocalizerFactory>());

        RuleFor(p => p.Name)
            .NotEmpty()
                .WithMessage(T["Image Name cannot be empty!"])
            .MaximumLength(150);

        RuleFor(p => p.Extension)
            .NotEmpty()
                .WithMessage(T["Image Extension cannot be empty!"])
            .MaximumLength(5);

        RuleFor(p => p.Data)
            .NotEmpty()
                .WithMessage(T["Image Data cannot be empty!"]);
    }
}