#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Notification/src/Notification/Notification.csproj", "src/Services/Notification/src/Notification/"]
COPY ["src/BuildingBlocks/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/"]
COPY ["src/BuildingBlocks/DataAccess/DataAccess.csproj", "src/BuildingBlocks/DataAccess/"]
RUN dotnet restore "./src/Services/Notification/src/Notification/Notification.csproj"
COPY . .
WORKDIR "/src/src/Services/Notification/src/Notification"
RUN dotnet build "./Notification.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Notification.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notification.dll"]