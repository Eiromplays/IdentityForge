// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

namespace Eiromplays.IdentityServer.Domain.Constants;

public class ConsentOptions
{
    public const bool EnableOfflineAccess = true;

    public const string OfflineAccessDisplayName = "Offline Access";
    public const string OfflineAccessDescription = "Access to your applications and resources, even when you are offline";

    // ReSharper disable once ConvertToConstant.Global
    public const string MustChooseOneErrorMessage = "You must pick at least one permission";
    public const string InvalidSelectionErrorMessage = "Invalid selection";
}