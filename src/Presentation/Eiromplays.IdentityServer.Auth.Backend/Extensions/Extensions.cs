// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Models;

namespace Eiromplays.IdentityServer.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Checks if the redirect URI is for a native client.
        /// </summary>
        /// <returns></returns>
        public static bool IsNativeClient(this AuthorizationRequest context)
        {
            return !context.RedirectUri.StartsWith("https", StringComparison.Ordinal)
                   && !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);
        }

        public static IResult LoadingPage(this HttpContext httpContext, string redirectUri)
        {
            httpContext.Response.StatusCode = 200;
            httpContext.Response.Headers["Location"] = "";

            return Results.Redirect(redirectUri);
        }
    }
}