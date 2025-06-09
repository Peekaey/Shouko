# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the entire solution
COPY ["Shouko.sln", "./"]

# Copy all project files
COPY ["Shouko.Api/Shouko.Api.csproj", "Shouko.Api/"]
COPY ["Shouko.BackgroundService/Shouko.BackgroundService.csproj", "Shouko.BackgroundService/"]
COPY ["Shouko.BusinessService/Shouko.BusinessService.csproj", "Shouko.BusinessService/"]
COPY ["Shouko.DataService/Shouko.DataService.csproj", "Shouko.DataService/"]
COPY ["Shouko.DiscordBot/Shouko.DiscordBot.csproj", "Shouko.DiscordBot/"]
COPY ["Shouko.Helpers/Shouko.Helpers.csproj", "Shouko.Helpers/"]
COPY ["Shouko.Models/Shouko.Models.csproj", "Shouko.Models/"]
COPY ["Shouko.Tests/Shouko.Tests.csproj", "Shouko.Tests/"]

# Restore dependencies for the solution
RUN dotnet restore "Shouko.sln"

# Copy the rest of the source code
COPY . .

# Build and publish the Shouko.DiscordBot project
WORKDIR "/src/Shouko.DiscordBot"
RUN dotnet publish "Shouko.DiscordBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Shouko.DiscordBot.dll"]