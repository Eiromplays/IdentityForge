FROM mcr.microsoft.com/dotnet/sdk:7.0
EXPOSE 80
EXPOSE 443

WORKDIR /app/src/IdentityForge.IdentityServer
COPY . /app/src/IdentityForge.IdentityServer

RUN dotnet restore

ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile"]