# setting up the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# copy each project file into working directory to restore dependencies
# copy the PlaywrightTestClient project and shared library
COPY ["PlaywrightTestClient/PlaywrightTestClient.csproj", "PlaywrightTestClient/"]
COPY ["PlaywrightTest.Shared/PlaywrightTest.Shared.csproj", "PlaywrightTest.Shared/"]

# restore packages on the project api project
RUN dotnet restore "PlaywrightTestClient/PlaywrightTestClient.csproj"

# copy the remaining api and lib project files
# can also do a shorthand command ex: COPY . .
COPY ["PlaywrightTestClient/.", "./PlaywrightTestClient/"]
COPY ["PlaywrightTest.Shared/.", "./PlaywrightTest.Shared/"]

# set the working directory to /source/project
WORKDIR "/src/PlaywrightTestClient"

# build the api and lib projects for release configuration
RUN dotnet build "PlaywrightTestClient.csproj" -c $BUILD_CONFIGURATION

# Get the files from the build and publish them for release
# output the published files into the /app directory
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app

# setting up the staging image for running the app and set the current working directory as app
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# copy the published files from the app directory
COPY --from=publish /app/wwwroot .
COPY ["PlaywrightTestClient/nginx.conf", "/etc/nginx/nginx.conf"]
