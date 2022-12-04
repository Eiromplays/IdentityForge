FROM faithlife/aspnetcore-build-yarn:2021.12
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

WORKDIR /app/Presentation/Eiromplays.IdentityServer.WebUI
COPY . /app/Presentation/Eiromplays.IdentityServer.WebUI

ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile"]