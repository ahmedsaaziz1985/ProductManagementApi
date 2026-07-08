# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API/ProductManagementApi.API.csproj", "API/"]
COPY ["Application/ProductManagementApi.Application.csproj", "Application/"]
COPY ["Domain/ProductManagementApi.Domain.csproj", "Domain/"]
COPY ["Infrastructure/ProductManagementApi.Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "./API/ProductManagementApi.API.csproj"
COPY . .
WORKDIR "/src/API"
RUN dotnet build "./ProductManagementApi.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProductManagementApi.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductManagementApi.API.dll"]
