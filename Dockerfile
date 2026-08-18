# ===========================
# BUILD
# ===========================

FROM mcr.microsoft.com/dotnet/sdk:10.0

WORKDIR /src

COPY . .

RUN dotnet restore AIPortfolio.API/AIPortfolio.API.csproj

RUN dotnet publish AIPortfolio.API/AIPortfolio.API.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ===========================
# RUNTIME
# ===========================

FROM mcr.microsoft.com/dotnet/aspnet:10.0

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY --from=0 /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet","AIPortfolio.API.dll"]
