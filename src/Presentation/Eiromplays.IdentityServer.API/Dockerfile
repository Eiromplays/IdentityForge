FROM mcr.microsoft.com/dotnet/sdk:6.0
EXPOSE 443

WORKDIR /app/Presentation/Eiromplays.IdentityServer.API
COPY . /app/Presentation/Eiromplays.IdentityServer.API

ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile"]