# Original Source: https://github.com/NCarlsonMSFT/CertExample/blob/master/Certs/Create-Certs.ps1

[string]$Password = [Guid]::NewGuid().ToString("N")
Set-Content -Path "${PSScriptRoot}\Password.txt" -Value $Password -NoNewline

docker run --rm --entrypoint="/bin/bash" -v "${PSScriptRoot}:/Certs" -w="/Certs" mcr.microsoft.com/dotnet/sdk:7.0 "/Certs/CreateCerts.sh"

$envTemplate = Get-Content -Path "${PSScriptRoot}\ContainerCerts.env.template"
$envWPassword = $envTemplate.Replace("`$Password", $Password)
$frontEndEnv = $envWPassword.Replace("`$ProjectName", "frontend")
Set-Content -Path "${PSScriptRoot}\..\FrontEnd\ContainerCerts.env" -Value $frontEndEnv
$identityserverEnv = $envWPassword.Replace("`$ProjectName", "identityserver")
Set-Content -Path "${PSScriptRoot}\..\identityserver\ContainerCerts.env" -Value $identityserverEnv

$identityforgeCaCert = New-Object -TypeName "System.Security.Cryptography.X509Certificates.X509Certificate2" @("${PSScriptRoot}\identityforge-ca.crt", $null)

$storeName = [System.Security.Cryptography.X509Certificates.StoreName]::Root;
$storeLocation = [System.Security.Cryptography.X509Certificates.StoreLocation]::CurrentUser
$store = New-Object System.Security.Cryptography.X509Certificates.X509Store($storeName, $storeLocation)
$store.Open(([System.Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite))
try
{
    $store.Add($identityforgeCaCert)
}
finally
{
    $store.Close()
    $store.Dispose()
}