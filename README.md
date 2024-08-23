# Overview
This project is intended to be used for educational and reference purposes focusing around automation testing with Microsoft Playwright.

## Demonstrated Features
- We'll fill out this section as we make it further in the project

# Dependencies
- MongoDB
- Docker
- Docker Compose V2
- Azure Microsoft Entra Id

# Getting Started
This project has dependencies on MongoDB and Azure Microsoft Entra ID.  You will have to setup app registrations in Azure for both an API and Client App and then configure in appsettings for both projects.  You can either use a deployed MongoDB Atlas instance and a connection to connect, or you can use a containerized instance of MongoDB.

View the MongoService.cs file and review the constructor.  Identify how you will be retrieving your connection string and either comment/uncomment the lines present or alter the code.

This project is locally setup to use a MongoDB Docker Container.  The connection string is setup both in the docker-compose.yaml as well as in the launchSettings.json profiles via Environment Variables.  If you choose to use secrets, you will need to update the code and remove the Environment variables and change the MongoService to use the secrets location.

## Microsoft Entra ID Configuration

### PlaywrightTestClient
1. Update the appsettings.json sections for AzureAd and Scope
2. For AzureAd point to your Azure Tenant and App Registration for the Client application
```
"AzureAd": {
    "Authority": "https://login.microsoftonline.com/22222222-2222-2222-2222-222222222222",
    "ClientId": "33333333-3333-3333-33333333333333333",
    "ValidateAuthority": true
}
```
> The authority can also use a domain name instead of the tenant id like `https://login.microsoftonline.com/<microsoft-entra-primary-domain>`.  The primary domain normally looks something like `someaccount.onmicrosoft.com`

3. Use the scope level permissions defined on the API App Registration.  Replace <client-id> and <scope-name> with values from the API app registration
```
"Scope": "api://<client-id>/<scope-name>"
```

4. Ensure a base URI is set for the http client to use for http requests to the API
```
"BaseApiUri": "<api-url>"
```

### PlaywrightTestApi
1. Update the appsettings.json or use local secrets.json and point to your Azure Tenant and App Registration for the Client application
```
"AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "qualified.domain.name",
    "TenantId": "22222222-2222-2222-2222-222222222222",
    "ClientId": "11111111-1111-1111-11111111111111111",
    "Scopes": "access_as_user",
    "CallbackPath": "/signin-oidc"
}
```
> Domain comes from the Microsoft Entra ID Tenant's Primary Domain.  The primary domain normally looks something like `someaccount.onmicrosoft.com`

## MongoDB Atlas
1. Signup for MongoDB if you do not already have an account - Use the free tier of the service
2. Create a new project and database
3. Get the connection string to the database and store somewhere secure, ex: local secrets config file
4. Create a new collection in the database
5. Build and Run the API
6. Use .http files or Postman to send requests to the API

## MongoDB Container
1. Build all images and containers from the docker-compose.yaml file
2. Run the containers using docker-compose.yaml
3. Use .http files or Postman to send requests to the API


# Run the Solution
The easiest way to run this solution is to use docker compose as that will build the api project and provide containers for the data.  But there are other options as well.

## Docker Compose
1. Optional - build all containers in the compose yaml
```
docker compose build
```
> To build a specific container use `docker compose build <service-name>`

2. Compose up the containers
```
docker compose up
```
> If you do not want to debug, the add the -d parameter.  `docker compose up -d`
> docker compose up will also build all images if they do not exist, so step 1 is optional

3. Use an http client like Postman or the http files in Visual Studio to send requests to the API

4. Stop the containers when done with testing (or leave them running)
```
docker compose stop
```

> Use the start command to start the containers again
```
docker compose start
```

### Clean UP
Once containers are no longer needed you can remove them all using the compose down command
```
docker compose down
```

> Images can also be deleted using the compose down command
```
docker compose down --rmi "all"
```

## Docker
1. Pull the mongo db image
```
docker pull mongo
```

2. Build and Run the mongo db container
```
docker run -d -p 27017:27017 -e "MONGO_INITDB_ROOT_USERNAME=mongoUser" -e "MONGO_INITDB_ROOT_PASSWORD=mongoPassword" --name mongoLocal mongo
```
> Replace values for MONGO_INITDB_ROOT_USERNAME, MONGO_INITDB_ROOT_PASSWORD, and --name to those of your choosing if you prefer.  But note them as they are needed for the connection string in the next step

3. Build the API Project Image
```
docker build -t my_api_image -f Dockerfile .
```
> Docker Context will vary depending on where you run the command.  This command assumes that it is being run from the same path containing the Dockerfile.  ie: "<PATH>\WebApiControllers\"

4. Run the Api image and container
```
docker run -d -p 5112:80 -e "ASPNETCORE_ENVIRONMENT=Development" -e "MONGODB_CONN=mongodb://mongoUser:mongoPassword@mongoLocal:27017" -e "ASPNETCORE_URLS=http://+:80" --name my_api_container my_api_image
```

> This repo is not demonstrating security for the mongo db admin user or connection strings.  But best practice would not be to hard code those values and instead pull them from a secure location.

5. Create a docker network
```
docker network create my_network
```

6. Add the mongo db and api containers to the network
```
docker network connect mongoLocal
```

```
docker network connect my_api_container
```

6. Test the api using an http client like Postman or Visual Studio and .http files

## Containers and IDE
1. Create and the Mongo DB container
> You can use either the docker or docker compose commands
> If using docker compose, you may need to pause the api container

2. Using an IDE of your choice, select a profile to use from the launchSettings.json

3. Run and/or debug the solution
> If you set a different user name and password for the Mongo DB container, then you will also need to update the environment variable in your launchSettings.json profile


# Tests
Work in Progress


# References
- [MongoDB](https://www.mongodb.com/)
- [Web API Versioning](https://github.com/dotnet/aspnet-api-versioning)
- [Output Caching](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/output?view=aspnetcore-8.0)
- [HealthChecks](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-8.0)
- [Metrics](https://learn.microsoft.com/en-us/aspnet/core/log-mon/metrics/metrics?view=aspnetcore-8.0)
- [HTTP File Secrets](https://devblogs.microsoft.com/visualstudio/safely-use-secrets-in-http-requests-in-visual-studio-2022/)
- [Docker Compose CLI](https://docs.docker.com/compose/reference/)
- [Docker Compose Repo](https://github.com/docker/awesome-compose/tree/master)
- [Dotnet CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/)
- [Http Client Factory](https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory)