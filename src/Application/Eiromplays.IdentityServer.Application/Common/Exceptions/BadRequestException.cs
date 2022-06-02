using System.Net;

namespace Eiromplays.IdentityServer.Application.Common.Exceptions;

public class BadRequestException : CustomException
{
    public BadRequestException(string? message, List<string>? errors = default)
        : base(message, errors, HttpStatusCode.BadRequest)
    {
    }
}