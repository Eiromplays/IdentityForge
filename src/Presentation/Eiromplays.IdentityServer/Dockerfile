FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Domain/Eiromplays.IdentityServer.Domain/Eiromplays.IdentityServer.Domain.csproj", "src/Domain/Eiromplays.IdentityServer.Domain/"]
COPY ["src/Application/Eiromplays.IdentityServer.Application/Eiromplays.IdentityServer.Application.csproj", "src/Application/Eiromplays.IdentityServer.Application/"]
COPY ["src/Infrastructure/Eiromplays.IdentityServer.Infrastructure/Eiromplays.IdentityServer.Infrastructure.csproj", "src/Infrastructure/Eiromplays.IdentityServer.Infrastructure/"]
COPY ["src/Presentation/Eiromplays.IdentityServer/Eiromplays.IdentityServer.csproj", "src/Presentation/Eiromplays.IdentityServer/"]
COPY ["src/Presentation/Eiromplays.IdentityServer.WebUI/Eiromplays.IdentityServer.WebUI.csproj", "src/Presentation/Eiromplays.IdentityServer.WebUI/"]
COPY ["src/Presentation/Eiromplays.IdentityServer.API/Eiromplays.IdentityServer.API.csproj", "src/Presentation/Eiromplays.IdentityServer.API/"]
COPY ["src/Infrastructure/Migrators.MySql/Migrators.MySql.csproj", "src/Infrastructure/Migrators.MySql/"]
COPY ["src/Infrastructure/Migrators.PostgreSql/Migrators.PostgreSql.csproj", "src/Infrastructure/Migrators.PostgreSql/"]
COPY ["src/Infrastructure/Migrators.Sqlite/Migrators.Sqlite.csproj", "src/Infrastructure/Migrators.Sqlite/"]
COPY ["src/Infrastructure/Migrators.SqlServer/Migrators.SqlServer.csproj", "src/Infrastructure/Migrators.SqlServer/"]
COPY ["src/Presentation/Eiromplays.IdentityServer.Admin.WebUI/Eiromplays.IdentityServer.Admin.WebUI.csproj", "src/Presentation/Eiromplays.IdentityServer.Admin.WebUI/"]
COPY ["src/Core/Shared/Shared.csproj", "src/Core/Shared/"]
RUN dotnet restore "src/Presentation/Eiromplays.IdentityServer/Eiromplays.IdentityServer.csproj"
COPY . .
WORKDIR "/src/src/Presentation/Eiromplays.IdentityServer"
RUN dotnet build "Eiromplays.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_14.x | bash \
    && apt-get install nodejs -yq
RUN dotnet publish "Eiromplays.IdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Eiromplays.IdentityServer.dll"]
