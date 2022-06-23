// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Original file: https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/Quickstarts
// Modified by Eirik Sjøløkken

namespace Eiromplays.IdentityServer.Domain.Constants;

public class ConsentOptions
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static bool EnableOfflineAccess = true;

    public static string OfflineAccessDisplayName = "Offline Access";
    public static string OfflineAccessDescription = "Access to your applications and resources, even when you are offline";
#pragma warning restore CA2211 // Non-constant fields should not be visible
    public static readonly string MustChooseOneErrorMessage = "You must pick at least one permission";
    public static readonly string InvalidSelectionErrorMessage = "Invalid selection";
}