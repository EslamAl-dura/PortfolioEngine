# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and restore dependencies
COPY ["src/PortfolioEngine.Web/PortfolioEngine.Web.csproj", "src/PortfolioEngine.Web/"]
COPY ["src/PortfolioEngine.Application/PortfolioEngine.Application.csproj", "src/PortfolioEngine.Application/"]
COPY ["src/PortfolioEngine.Domain/PortfolioEngine.Domain.csproj", "src/PortfolioEngine.Domain/"]
COPY ["src/PortfolioEngine.Infrastructure/PortfolioEngine.Infrastructure.csproj", "src/PortfolioEngine.Infrastructure/"]

RUN dotnet restore "src/PortfolioEngine.Web/PortfolioEngine.Web.csproj"

# Copy full source and publish
COPY . .
WORKDIR "/src/src/PortfolioEngine.Web"
RUN dotnet publish "PortfolioEngine.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PortfolioEngine.Web.dll"]