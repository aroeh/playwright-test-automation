#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PlaywrightTestApi/PlaywrightTestApi.csproj", "PlaywrightTestApi/"]
COPY ["PlaywrightTest.Shared/PlaywrightTest.Shared.csproj", "PlaywrightTest.Shared/"]
RUN dotnet restore "PlaywrightTestApi/PlaywrightTestApi.csproj"
COPY ["PlaywrightTestApi/.", "./PlaywrightTestApi/"]
COPY ["PlaywrightTest.Shared/.", "./PlaywrightTest.Shared/"]
WORKDIR "/src/PlaywrightTestApi"
RUN dotnet build "./PlaywrightTestApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PlaywrightTestApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlaywrightTestApi.dll"]



# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# USER app
# WORKDIR /app

# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# ARG BUILD_CONFIGURATION=Release
# WORKDIR /src
# COPY ["PlaywrightTestApi.csproj", "PlaywrightTestApi/"]
# RUN dotnet restore "./PlaywrightTestApi/PlaywrightTestApi.csproj"
# COPY . PlaywrightTestApi/
# WORKDIR "/src/PlaywrightTestApi"
# RUN dotnet build "./PlaywrightTestApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# FROM build AS publish
# ARG BUILD_CONFIGURATION=Release
# RUN dotnet publish "./PlaywrightTestApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "PlaywrightTestApi.dll"]