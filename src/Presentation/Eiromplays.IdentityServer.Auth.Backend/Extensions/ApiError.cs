﻿using System.Diagnostics.CodeAnalysis;

namespace Eiromplays.IdentityServer.Extensions;

[ExcludeFromCodeCoverage]
public class ApiError
{
    public ApiError(string message, string? innerMessage, string? stackTrace)
    {
        this.Message = message;
        this.InnerMessage = innerMessage;
        this.StackTrace = stackTrace;
    }

    public string Message { get; set; }
    public string? InnerMessage { get; set; }
    public string? StackTrace { get; set; }
}