using System.Text;
using Eiromplays.IdentityServer.Application.Common.Mailing;
using RazorEngineCore;

namespace Eiromplays.IdentityServer.Infrastructure.Mailing;

public class EmailTemplateService : IEmailTemplateService
{
    public string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel)
    {
        string template = GetTemplate(templateName);

        var razorEngine = new RazorEngine();
        var modifiedTemplate = razorEngine.Compile(template);

        return modifiedTemplate.Run(mailTemplateModel);
    }

    public async Task<string> GenerateEmailTemplateAsync<T>(string templateName, T mailTemplateModel)
    {
        string template = await GetTemplateAsync(templateName);

        var razorEngine = new RazorEngine();
        var modifiedTemplate = await razorEngine.CompileAsync(template);

        return await modifiedTemplate.RunAsync(mailTemplateModel);
    }

    public string GetTemplate(string templateName)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string tmplFolder = Path.Combine(baseDirectory, "Email-Templates");
        string filePath = Path.Combine(tmplFolder, $"{templateName}.cshtml");

        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var sr = new StreamReader(fs, Encoding.Default);
        string mailText = sr.ReadToEnd();
        sr.Close();

        return mailText;
    }

    public async Task<string> GetTemplateAsync(string templateName)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string tmplFolder = Path.Combine(baseDirectory, "Email-Templates");
        string filePath = Path.Combine(tmplFolder, $"{templateName}.cshtml");

        await using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var sr = new StreamReader(fs, Encoding.Default);
        string mailText = await sr.ReadToEndAsync();
        sr.Close();

        return mailText;
    }
}