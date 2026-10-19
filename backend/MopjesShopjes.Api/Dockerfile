# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY MopjesShopjes.Api.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
HEALTHCHECK --interval=10s --timeout=5s --retries=5 CMD curl -f http://localhost:8080/health || exit 1
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "MopjesShopjes.Api.dll"]