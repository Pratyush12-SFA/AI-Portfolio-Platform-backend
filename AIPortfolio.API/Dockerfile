# =========================
# Runtime Base
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

USER root

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

USER $APP_UID

WORKDIR /app

EXPOSE 8080
EXPOSE 8081


# =========================
# Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["AIPortfolio.API/AIPortfolio.API.csproj", "AIPortfolio.API/"]
COPY ["AIPortfolio.Application/AIPortfolio.Application.csproj", "AIPortfolio.Application/"]
COPY ["AIPortfolio.Domain/AIPortfolio.Domain.csproj", "AIPortfolio.Domain/"]
COPY ["AIPortfolio.Infrastructure/AIPortfolio.Infrastructure.csproj", "AIPortfolio.Infrastructure/"]
COPY ["AIPortfolio.Persistence/AIPortfolio.Persistence.csproj", "AIPortfolio.Persistence/"]

RUN dotnet restore "AIPortfolio.API/AIPortfolio.API.csproj"

COPY . .

WORKDIR "/src/AIPortfolio.API"

RUN dotnet build "AIPortfolio.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build \
    --no-restore


# =========================
# Publish
# =========================
FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "AIPortfolio.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false \
    --no-restore


# =========================
# Final Runtime Image
# =========================
FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "AIPortfolio.API.dll"]