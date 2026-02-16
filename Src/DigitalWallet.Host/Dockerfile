# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DigitalWallet.Host/DigitalWallet.Host.csproj", "src/DigitalWallet.Host/"]
COPY ["src/Modules/Users/DigitalWallet.Modules.Users.Api/DigitalWallet.Modules.Users.Api.csproj", "src/Modules/Users/DigitalWallet.Modules.Users.Api/"]
COPY ["src/Shared/DigitalWallet.Shared.Api/DigitalWallet.Shared.Api.csproj", "src/Shared/DigitalWallet.Shared.Api/"]
COPY ["src/Shared/DigitalWallet.Shared.Infrastructure/DigitalWallet.Shared.Infrastructure.csproj", "src/Shared/DigitalWallet.Shared.Infrastructure/"]
COPY ["src/Shared/DigitalWallet.Shared.Application/DigitalWallet.Shared.Application.csproj", "src/Shared/DigitalWallet.Shared.Application/"]
COPY ["src/Shared/DigitalWallet.Shared.Domain/DigitalWallet.Shared.Domain.csproj", "src/Shared/DigitalWallet.Shared.Domain/"]
COPY ["src/Modules/Users/DigitalWallet.Modules.Users.Infrastructure/DigitalWallet.Modules.Users.Infrastructure.csproj", "src/Modules/Users/DigitalWallet.Modules.Users.Infrastructure/"]
COPY ["src/Modules/Users/DigitalWallet.Modules.Users.Application/DigitalWallet.Modules.Users.Application.csproj", "src/Modules/Users/DigitalWallet.Modules.Users.Application/"]
COPY ["src/Modules/Users/DigitalWallet.Modules.Users.Domain/DigitalWallet.Modules.Users.Domain.csproj", "src/Modules/Users/DigitalWallet.Modules.Users.Domain/"]
COPY ["src/Modules/Users/DigitalWallet.Modules.Users.Contracts/DigitalWallet.Modules.Users.Contracts.csproj", "src/Modules/Users/DigitalWallet.Modules.Users.Contracts/"]
COPY ["src/Read/DigitalWallet.Read.Api/DigitalWallet.Read.Api.csproj", "src/Read/DigitalWallet.Read.Api/"]
COPY ["src/Read/DigitalWallet.Read.Storage/DigitalWallet.Read.Storage.csproj", "src/Read/DigitalWallet.Read.Storage/"]
RUN dotnet restore "./src/DigitalWallet.Host/DigitalWallet.Host.csproj"
COPY . .
WORKDIR "/src/src/DigitalWallet.Host"
RUN dotnet build "./DigitalWallet.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DigitalWallet.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DigitalWallet.Host.dll"]