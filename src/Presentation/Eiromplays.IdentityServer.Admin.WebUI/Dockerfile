FROM mcr.microsoft.com/dotnet/sdk:6.0
EXPOSE 443

WORKDIR /app/Presentation/Eiromplays.IdentityServer.Admin.WebUI
COPY . /app/Presentation/Eiromplays.IdentityServer.Admin.WebUI

ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile"]