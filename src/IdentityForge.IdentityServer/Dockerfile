FROM mcr.microsoft.com/dotnet/sdk:7.0 as base
EXPOSE 80
EXPOSE 443

FROM base as testCerts
RUN ls -la
ADD certs/identityforge-ca.crt /usr/local/share/ca-certificates/identityforge-ca.crt
RUN chmod 644 /usr/local/share/ca-certificates/identityforge-ca.crt && update-ca-certificates

WORKDIR /app/src/IdentityForge.IdentityServer
COPY . /app/src/IdentityForge.IdentityServer

RUN dotnet restore

ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile"]