FROM mcr.microsoft.com/dotnet/sdk:6.0
EXPOSE 443

WORKDIR /app/Presentation/Eiromplays.IdentityServer.WebUI
COPY . /app/Presentation/Eiromplays.IdentityServer.WebUI

ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile"]