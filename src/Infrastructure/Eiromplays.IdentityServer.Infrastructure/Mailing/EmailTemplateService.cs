using System.Text;
using Eiromplays.IdentityServer.Application.Common.Mailing;
using RazorEngineCore;

namespace Eiromplays.IdentityServer.Infrastructure.Mailing;

public class EmailTemplateService : IEmailTemplateService
{
    public string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel)
    {
        var template = GetTemplate(templateName);

        var razorEngine = new RazorEngine();
        var modifiedTemplate = razorEngine.Compile(template);

        return modifiedTemplate.Run(mailTemplateModel);
    }

    public string GetTemplate(string templateName)
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var tmplFolder = Path.Combine(baseDirectory, "Email-Templates");
        var filePath = Path.Combine(tmplFolder, $"{templateName}.cshtml");

        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var sr = new StreamReader(fs, Encoding.Default);
        var mailText = sr.ReadToEnd();
        sr.Close();

        return mailText;
    }
}