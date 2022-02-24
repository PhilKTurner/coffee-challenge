# CoffeeChallenge
## Getting Started
Building and running of the solution requires Docker.

Provide passwords used to setup the MariaDB container as UTF-8 text files in ./.secrets

- cc-mariadb-pw
- cc-mariadb-rootpw

Build images and run containers for application and backend:

```
docker-compose --profile backend --profile coffee-services up -d
```

SwaggerUI provides an interactive, documented interface for CoffeeStore's REST API and can be reached under [http://localhost:5000/swagger](http://localhost:5000/swagger), when the service is up on the local machine.

## Development Environment
To set up the dev environment for CoffeeStore set the configured MariaDB user password in the .NET secret storage
```
dotnet user-secrets set "CoffeeStoreDatabase:Password" "<cc-mariadb-pw>"
```

## Deployment
Since the services are already dockerized, deployment should be easily feasible on a server with a docker environment. Deployment to cloud services should also be achievable with manageable effort.
