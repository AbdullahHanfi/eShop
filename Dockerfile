FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
WORKDIR /src
COPY ["src/eShop.MVC/eShop.MVC.csproj", "eShop.MVC/"]
COPY ["src/eShop.BLL/eShop.BLL.csproj", "eShop.BLL/"]
COPY ["src/eShop.DAL/eShop.DAL.csproj", "eShop.DAL/"]
COPY ["src/eShop.Core/eShop.Core.csproj", "eShop.Core/"]
COPY ["tests/unit/eShop.BLL.Tests/eShop.BLL.Tests.csproj", "/tests/unit/eShop.BLL.Tests/eShop.BLL.Tests/"]

RUN dotnet restore "eShop.MVC/eShop.MVC.csproj"
# Copy the rest of the source code to the build context
WORKDIR /code

COPY [".", "."]
RUN dotnet build "src/eShop.MVC/eShop.MVC.csproj" -c Release -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
RUN dotnet publish "src/eShop.MVC/eShop.MVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

# This stage is used in production 
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eShop.MVC.dll"]