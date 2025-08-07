# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY APIVault.API/APIVault.API.csproj ./APIVault.API/
RUN dotnet restore ./APIVault.API/APIVault.API.csproj

# Copy everything and build
COPY . ./
WORKDIR /src/APIVault.API
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Expose the default port
EXPOSE 80

ENTRYPOINT ["dotnet", "APIVault.API.dll"]
