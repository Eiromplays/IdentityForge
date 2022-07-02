namespace Eiromplays.IdentityServer.Application.Common.Mailing;

public interface IEmailTemplateService : ITransientService
{
    string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel);

    Task<string> GenerateEmailTemplateAsync<T>(string templateName, T mailTemplateModel);
}