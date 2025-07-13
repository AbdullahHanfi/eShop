# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release
COPY ["eShop.sln", "."]

WORKDIR /src
COPY ["src/eShop.MVC/eShop.MVC.csproj", "eShop.MVC/"]
COPY ["src/eShop.BLL/eShop.BLL.csproj", "eShop.BLL/"]
COPY ["src/eShop.DAL/eShop.DAL.csproj", "eShop.DAL/"]
COPY ["src/eShop.Core/eShop.Core.csproj", "eShop.Core/"]

WORKDIR ../tests/unit
COPY ["tests/unit/eShop.BLL.Tests/eShop.BLL.Tests.csproj", "eShop.BLL.Tests/"]

WORKDIR ../../
RUN dotnet restore "eShop.sln"

COPY . .
WORKDIR "src/eShop.MVC"
RUN dotnet build "./eShop.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./eShop.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eShop.MVC.dll"]